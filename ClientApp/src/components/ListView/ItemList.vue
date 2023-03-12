<template>
  <div class="item">
    <h1 class="truncate">{{ data.Назва }}</h1>
    <p>{{ data.Опис }}</p>
  </div>
</template>
<script>
export default {
  props: {
    data: {
      type: Object,
      required: true,
    },
  },
}
</script>
<style>
.item {
  @apply bg-gray-900 p-3 mb-5;
}
</style>
