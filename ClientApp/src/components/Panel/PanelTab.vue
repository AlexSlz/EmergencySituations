<template>
  <div v-show="isActive"><slot></slot></div>
</template>
<script>
export default {
  props: {
    name: { required: true },
    selected: { default: false },
    fullPage: { default: false },
  },
  data() {
    return {
      isActive: false,
    }
  },
  created() {
    this.isActive = this.selected
    this.$parent.tabs.push(this)
  },
}
</script>
