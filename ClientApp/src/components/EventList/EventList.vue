<script setup>
import ItemInfo from './ItemInfo.vue'
import ItemList from './ItemList.vue'
import { useEmergencyStore } from '@/stores/emergency'
import { useActionPanel } from '@/stores/actionPanel'
import { useNotify } from '../../stores/Notify'
import database from '@/main/database'
import Filter from './Filter.vue'
import info from '@/main/infoManager'
</script>

<template>
  <my-modal ref="modal" :show="showModal">
    <Filter :params="filterParam" />
  </my-modal>
  <div class="block">
    <div v-if="emergency.selected != null">
      <input @click="emergency.select(null)" value="Назад" type="button" />
      <ItemInfo :element="emergency.selected" />
      <template v-if="isAuth">
        <button @click="actionPanel.open({ selected: emergency.selected, tableName: 'Emergency' })">Edit</button>
        <button class="bg-myRed" @click="DeleteData">Delete</button>
      </template>
    </div>
    <template v-else>
      <button v-if="isAuth" @click="actionPanel.open()">Add New</button>
      <h1>Пошук</h1>
      <input v-model="filterParam.name" />
      <button @click="this.$refs.modal.Open()">Filter</button>
      <ItemList @click="emergency.select(item)" :data="item" v-for="(item, i) in filteredEmergency" />
      <h1 v-if="emergency.list.length == 0" class="text-center pt-3">{{ message }}</h1>
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
      filterParam: { name: '', description: '', level: 0, type: 0, sort: 'id', orderBy: false },
      showModal: false,
      emergency: useEmergencyStore(),
      actionPanel: useActionPanel(),
      notify: useNotify(),
      message: 'Loading...',
    }
  },
  computed: {
    filteredEmergency() {
      var filtered = this.emergency.list.filter((item) => {
        var name = item.name.toLowerCase().includes(this.filterParam.name.toLowerCase())
        var description = item.description.toLowerCase().includes(this.filterParam.description.toLowerCase())
        var level = true
        if (this.filterParam.level > 0) level = item.level.id == this.filterParam.level
        var type = true
        if (this.filterParam.type > 0) type = item.type.id == this.filterParam.type
        return name && description && level && type
      })
      var sorted = filtered.sort((a, b) => {
        if (!Number.isInteger(a[this.filterParam.sort])) return a[this.filterParam.sort].localeCompare(b[this.filterParam.sort])
        return a[this.filterParam.sort] - b[this.filterParam.sort]
      })
      if (this.filterParam.orderBy) return sorted.reverse()
      return sorted
    },
  },
  methods: {
    async loadEmergency() {
      await database
        .GetData('Emergency')
        .then((res) => {
          console.log(res)
          this.emergency.list = res
          this.message = 'Список порожній.'
        })
        .catch((e) => {
          this.message = e
        })
        .finally(() => {
          this.emergency.select(null)
        })
    },
    DeleteData() {
      console.log(this.emergency.selected)
      database
        .DeleteData('Emergency', this.emergency.selected)
        .then((e) => {
          this.loadEmergency()
        })
        .finally(() => {
          this.notify.Open('Запис видалено.', 'success')
        })
    },
  },
  beforeMount() {
    this.emergency.reLoad = this.loadEmergency
    this.loadEmergency()
  },
  components: { Filter },
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
