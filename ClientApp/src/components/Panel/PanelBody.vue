<template>
  <div :class="{ fold: fold }" class="panel">
    <ul class="tabs">
      <li v-for="tab in tabs" :class="{ active: tab.isActive, mobile: tab.onlyMobile, fullPage: tab.isActive && tab.fullPage }">
        <a @click="selectTab(tab)">{{ tab.name }}</a>
      </li>
    </ul>
    <div class="content">
      <slot></slot>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      tabs: [],
      fold: false,
    }
  },
  created() {
    this.tabs.push({ name: 'Map', fold: true, onlyMobile: true })
  },
  methods: {
    selectTab(selectedTab) {
      this.fold = 'fold' in selectedTab
      this.tabs.forEach((tab) => {
        tab.isActive = tab.name == selectedTab.name
      })
    },
  },
}
</script>
<style scoped>
.panel {
  @apply absolute p-3 z-20 right-0 top-0 h-full overflow-y-auto bg-gray-800 min-w-[100%] sm:min-w-[38vh];
}
.fold {
  @apply h-auto;
}
.tabs {
  @apply relative overflow-hidden bg-gray-500 flex justify-center m-2;
}
.tabs li {
  @apply text-center p-2; /*float-left list-item*/
}
.active {
  @apply bg-cyan-800;
}
.mobile {
  @apply block sm:hidden;
}
.panel:has(.fullPage) {
  @apply w-full;
}
</style>
