<template>
  <ItemList :data="item" v-for="item in list" />
  <h1 v-if="loading" class="text-center p-3">Завантаження...</h1>
  <button v-if="page < maxPage && !loading" @click="loadNextPage()">Загрузить еще</button>
</template>

<script>
import ItemList from './ItemList.vue'
import { useEmergencyStore } from '@/stores/emergency'
import database from '@/main/database'
export default {
  props: {
    list: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      page: 0,
      maxPage: 1,
      limit: 30,

      emergency: useEmergencyStore(),
      loading: false,
    }
  },
  methods: {
    loadNextPage() {
      if (this.page >= this.maxPage) return
      this.page++
      this.loadEmergency()
    },
    async loadEmergency() {
      this.loading = true
      await database
        .GetData('Emergency', this.limit, this.page)
        .then((res) => {
          this.emergency.list = this.emergency.list.concat(res.data)
          this.maxPage = res.maxPage
        })
        .catch((e) => {})
        .finally(() => {
          this.emergency.select(null)
          this.loading = false
        })
    },
  },
  beforeMount() {
    this.emergency.reLoad = this.loadEmergency
    //this.loadEmergency()
  },
  components: { ItemList },
}
</script>
