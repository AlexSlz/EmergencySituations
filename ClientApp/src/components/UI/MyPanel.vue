<template>
  <div class="panel">
    <slot></slot>
  </div>
</template>
<script>
export default {
  name: 'my-panel',
  props: {},
  data() {
    return {
      show: false,
    }
  },
}
</script>
<style scoped>
.panel {
  @apply absolute p-3 z-20 right-0 top-0 h-full overflow-y-auto bg-gray-800 min-w-[100%] sm:min-w-[20%] sm:max-w-[400px];
}
</style>
