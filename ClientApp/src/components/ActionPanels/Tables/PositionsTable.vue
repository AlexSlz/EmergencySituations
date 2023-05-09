<template>
  <h1>Назва</h1>
  <input v-model="element.name" />
  <h1>Розташування</h1>
  <input v-model="element.location" type="text" />
  <h1>X</h1>
  <input v-model="element.x" type="number" />
  <h1>Y</h1>
  <input v-model="element.y" type="number" />
</template>

<script>
import { useActionPanel } from '@/stores/actionPanel'
export default {
  props: {
    element: {
      type: Object,
      default: {},
    },
  },
  data() {
    return {
      actionPanel: useActionPanel(),
    }
  },
  mounted() {
    this.actionPanel.selected.positions = [this.element]
    console.log(this.actionPanel.selected.positions)
  },
}
</script>
