<template>
  <h1>Назва</h1>
  <input v-model="element.name" type="text" required />
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
