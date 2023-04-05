<script setup>
import ItemInfo from './ItemInfo.vue'
import ItemList from './ItemList.vue'
</script>

<template>
  <div class="block">
    <div v-if="emergencyStore.selectedElement != null">
      <input @click="emergencyStore.selectElement(null)" value="Назад" type="button" />
      <ItemInfo :selectedElement="emergencyStore.selectedElement" />
    </div>
    <template v-else>
      <button v-if="authStore.isAuth" @click="editmenu.open()">Add New</button>
      <ItemList @click="emergencyStore.selectElement(item)" :data="item" v-for="(item, i) in getItems()" />
      <!-- <p class="fixed bottom-1 right-9">{{ itemsCount }} / {{ emergencyStore.emergencyList.length }}</p>
      <h1 v-if="emergencyStore.emergencyList.length == 0" class="text-center pt-3">Список порожній.</h1> -->
    </template>
    <div ref="obs"></div>
  </div>
</template>

<script>
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
</script>
