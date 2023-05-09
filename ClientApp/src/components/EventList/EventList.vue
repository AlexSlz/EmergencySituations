<script setup>
import ItemInfo from './ItemInfo.vue'
import ItemList from './ItemList.vue'
import { useEmergencyStore } from '@/stores/emergency'
import { useActionPanel } from '@/stores/actionPanel'
import { useNotify } from '@/stores/Notify'
import database from '@/main/database'
</script>

<template>
  <div class="block">
    <div v-if="emergency.selected != null">
      <input @click="emergency.select(null)" value="Назад" type="button" />
      <ItemInfo :element="emergency.selected" />
      <template v-if="isAuth && !loading">
        <button @click="actionPanel.open({ selected: emergency.selected, tableName: 'Emergency' })">Edit</button>
        <button class="bg-myRed" @click="DeleteData">Delete</button>
      </template>
    </div>
    <template v-else>
      <button v-if="isAuth" @click="actionPanel.open()">Додати нову подію</button>
      <ItemList @click="emergency.select(item)" :data="item" v-for="(item, i) in emergency.list" />
      <h1 v-if="loading" class="text-center p-3">Завантаження...</h1>
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

      page: 1,
      maxPage: 1,
      limit: 50,
      loading: false,
    }
  },
  methods: {
    movePage(i) {
      this.page += i
      this.loadEmergency()
    },
    async loadEmergency() {
      this.emergency.list = []
      this.loading = true
      await database
        .GetData('Emergency', this.limit, this.page, 'DateAndTime DESC')
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
}
</script>

<!-- <script>
import { useAuthStore } from '@/stores/auth'
import { useEmergencyStore } from '@/stores/emergency'
import { useMenuStore } from '@/stores/editMenu'
export default {
  data() {
    return {
      itemsCount: 10,
      itemsLimit: 5,
      authStore: useAuthStore(),
      emergencyStore: useEmergencyStore(),
      editmenu: useMenuStore(),
    }
  },
  mounted() {
    var options = {
      rootMargin: '0px',
      threshold: 0.3,
    }

    var obs = new IntersectionObserver((e, o) => {
      if (e[0].isIntersecting && this.emergencyStore.selectedElement == null) {
        this.itemsCount += this.itemsLimit
        if (this.itemsCount > this.emergencyStore.emergencyList.length) {
          this.itemsCount = this.emergencyStore.emergencyList.length
        }
      }
    }, options)
    obs.observe(this.$refs.obs)
  },
  methods: {
    getItems() {
      return this.emergencyStore.emergencyList.filter((e, i) => i < this.itemsCount)
    },
  },
}
</script> -->
