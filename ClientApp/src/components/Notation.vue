<template>
  <select v-model="emergencyStore.colorBy" name="select">
    <!--Supplement an id here instead of using 'name'-->
    <option class="text-neutral-800" :value="item" v-for="item in Object.keys(color)">{{ item }}</option>
  </select>

  <div class="px-2" v-for="items in Object.keys(color)">
    <h1>{{ items }}</h1>
    <div class="py-2 flex flex-row" v-for="c in Object.keys(color[items])">
      <p :style="'background-color: ' + color[items][c]" class="w-10 h-10"></p>
      <p class="pl-2 my-auto">{{ c }}</p>
    </div>
  </div>
</template>
<script>
import { useEmergencyStore } from '@/stores/emergency'
import color from '@/main/color'

export default {
  data() {
    return {
      color: color,
      emergencyStore: useEmergencyStore(),
    }
  },
}
</script>
