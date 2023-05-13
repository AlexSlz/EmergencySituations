<script setup>
import ItemInfo from './ItemInfo.vue'
import ItemList from './ItemList.vue'
import { useEmergencyStore } from '@/stores/emergency'
import { useActionPanel } from '@/stores/actionPanel'
import { useNotify } from '@/stores/Notify'
import database from '@/main/database'
import Search from './Search.vue'
import { useSearch } from '@/stores/search'
</script>

<template>
  <div class="block">
    <div v-if="emergency.selected != null">
      <input @click="emergency.select(null)" value="Назад" type="button" />
      <ItemInfo :element="emergency.selected" />
      <template v-if="isAuth && !loading">
        <button @click="actionPanel.open({ selected: emergency.selected, tableName: 'Emergency' })">Редагувати</button>
        <button class="bg-myRed" @click="DeleteData">Видалити</button>
      </template>
    </div>
    <template v-else>
      <button v-if="isAuth" @click="actionPanel.open()">Додати нову подію</button>
      <Search :search="searchStore" ref="search" @onSortChange="loadEmergency" />
      <ItemList @click="emergency.select(item)" :data="item" v-for="(item, i) in emergency.list" />
      <h1 v-if="loading" class="text-center p-3">Завантаження...</h1>
      <h1 v-if="emergency.list.length <= 0 && !loading" class="text-center p-3">Список порожній.</h1>
      <span class="flex">
        <template v-if="!loading">
          <input v-if="page != 1" @click="movePage(-page + 1)" value="<<-" type="button" />
          <input v-if="page != 1" @click="movePage(-1)" value="<-" type="button" />
          <input v-if="page < maxPage" @click="movePage(1)" :value="`${page}/${maxPage} ->`" type="button"
        /></template>
      </span>
    </template>
  </div>
</template>

<script>
export default {
  props: {
    isAuth: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      showModal: false,
      emergency: useEmergencyStore(),
      actionPanel: useActionPanel(),
      notify: useNotify(),
      searchStore: useSearch(),
      page: 1,
      maxPage: 1,
      loading: false,
    }
  },
  methods: {
    movePage(i) {
      this.page += i

      if (this.searchStore.notEmpty) {
        this.$refs.search.Find(this.page)
      } else {
        this.loadEmergency()
      }
    },
    async loadEmergency() {
      this.emergency.list = []
      this.loading = true
      await database
        .GetData('Emergency', this.page, this.searchStore.GetOrder())
        .then((res) => {
          this.emergency.list = res.data
          this.maxPage = res.maxPage
        })
        .catch((e) => {
          this.notify.Open(e, 'error')
        })
        .finally(() => {
          this.emergency.select(null)
          this.loading = false
        })
    },
    DeleteData() {
      this.loading = true
      database
        .DeleteData('Emergency', this.emergency.selected)
        .then((e) => {
          this.loading = false
          if (e != null) this.loadEmergency()
        })
        .catch((e) => {
          this.notify.Open(e, 'error')
        })
    },
  },
  beforeMount() {
    this.emergency.reLoad = this.loadEmergency
    this.loadEmergency()
  },
  watch: {
    'searchStore.result.length': {
      deep: true,
      handler(val) {
        if (val > 0) {
          this.maxPage = this.searchStore.maxPage
          this.emergency.list = this.searchStore.result
        } else {
          this.loadEmergency()
        }
      },
    },
  },
  components: { Search },
}
</script>
