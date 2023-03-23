<template>
  <a v-show="isActive"><slot></slot></a>
</template>
<script>
export default {
  props: {
    name: { required: true },
    selected: { default: false },
    fullPage: { default: false },
    link: { default: '' },
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
  unmounted() {
    let id = this.$parent.tabs.indexOf(this)
    this.$parent.tabs.splice(id, 1)
    this.$parent.tabs[1].isActive = true
  },
}
</script>
