<template>
  <div>
    <slot></slot>
    <h1 class="break-all mt-5">{{ selectedElement.Назва }}</h1>
  </div>
</template>
<script>
export default {
  props: {
    selectedElement: {
      type: Object,
      required: true,
    },
  },
}
</script>
