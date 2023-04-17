<script setup>
import ItemInfo from './ItemInfo.vue'
import ItemList from './ItemList.vue'
import { useEmergencyStore } from '@/stores/emergency'
import database from '@/main/database'
</script>

<template>
  <div class="block">
    <div v-if="emergency.selected != null">
      <input @click="emergency.select(null)" value="Назад" type="button" />
      <ItemInfo :element="emergency.selected" />
    </div>
    <template v-else>
      <ItemList @click="emergency.select(item)" :data="item" v-for="(item, i) in emergency.list" />
      <h1 v-if="emergency.list.length == 0" class="text-center pt-3">{{ message }}</h1>
    </template>
  </div>
</template>

<script>
export default {
  data() {
    return {
      emergency: useEmergencyStore(),
      message: 'Loading...',
    }
  },
  beforeMount() {
    database
      .GetData('Emergency')
      .then((res) => {
        this.emergency.list = res
        this.message = 'Список порожній.'
      })
      .catch((e) => {
        this.message = e
      })
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
