<script setup>
import ItemInfo from './ItemInfo.vue'
import ItemList from './ItemList.vue'
</script>

<template>
  <div class="block">
    <div v-if="emergencyStore.selectedElement != null">
      <my-button @click="emergencyStore.selectElement(null)">Назад</my-button>
      <ItemInfo :selectedElement="emergencyStore.selectedElement" />
    </div>
    <ItemList @click="emergencyStore.selectElement(item)" v-else :data="item" v-for="item in emergencyStore.emergencyList" />
    <h1 v-if="emergencyStore.emergencyList.length == 0" class="text-center pt-3">Список порожній.</h1>
  </div>
</template>

<script>
import { useEmergencyStore } from '@/stores/emergency'
export default {
  data() {
    return {
      emergencyStore: useEmergencyStore(),
    }
  },
}
</script>
