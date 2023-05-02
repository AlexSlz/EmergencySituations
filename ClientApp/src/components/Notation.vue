<template>
  <p class="p-2">Відображати події на карті за</p>
  <select v-model="emergency.colorBy" name="select">
    <option class="text-neutral-800" :value="index" v-for="(item, index) in label">{{ item.Name }}</option>
  </select>

  <div class="px-2" v-for="item in label">
    <h1>{{ item.Name }}</h1>
    <div class="flex py-2" v-for="c in Object.keys(item.element)">
      <i class="bg-black p-2 rounded-full w-10 h-10 fas text-center items-center" :class="item.element[c]"></i>
      <p class="pl-2 my-auto">{{ c }}</p>
    </div>
  </div>
</template>
<script>
import { useEmergencyStore } from '@/stores/emergency'
import label from '@/main/label'

export default {
  data() {
    return {
      label: label,
      name: [''],
      emergency: useEmergencyStore(),
    }
  },
  mounted() {
    this.name = ''
  },
}
</script>
