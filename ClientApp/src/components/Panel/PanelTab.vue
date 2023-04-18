<template>
  <a v-show="isActive"><slot></slot></a>
</template>
<script>
export default {
  props: {
    name: { required: true },
    selected: { default: false },
    fullPage: { default: false },
    openByDefault: { default: false },
  },
  data() {
    return {
      isActive: false,
    }
  },
  created() {
    this.isActive = this.selected
    this.$parent.tabs.push(this)
    if (this.openByDefault) this.$parent.selectTab(this)
  },
  methods: {
    tabClick() {
      this.$emit('onTabClick')
    },
  },
  mounted() {
    this.$emit('ifMount')
  },
  unmounted() {
    this.$emit('ifUnMount')
    let id = this.$parent.tabs.indexOf(this)
    this.$parent.tabs.splice(id, 1)
    this.$parent.selectTab(this.$parent.tabs[1])
  },
  watch: {
    isActive: {
      handler(val, oldVal) {
        if (this.isActive) {
          this.$emit('ifActive')
        } else {
          this.$emit('ifNotActive')
        }
      },
    },
  },
  emits: ['ifActive', 'ifNotActive', 'ifMount', 'ifUnMount', 'onTabClick'],
}
</script>
