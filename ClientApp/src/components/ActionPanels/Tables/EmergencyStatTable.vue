<template>
  <h1>Назва</h1>
  <input v-model="element.name" type="text" />
</template>
<script>
export default {
  props: {
    element: {
      type: Object,
      default: {},
    },
  },
}
</script>
