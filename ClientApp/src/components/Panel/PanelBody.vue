<template>
  <div :class="{ fold: fold }" class="panel">
    <ul class="tabs">
      <li v-for="tab in tabs" :class="{ active: tab.isActive, mobile: tab.onlyMobile, fullPage: tab.isActive && tab.fullPage }">
        <a @click="selectTab(tab)">{{ tab.name }}</a>
      </li>
    </ul>
    <div class="">
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
  @apply absolute z-20 right-0 top-0 h-full overflow-y-auto bg-neutral-800 min-w-[100%] sm:min-w-[30vw] sm:max-w-sm;
}
.fold {
  @apply h-auto;
}
.tabs {
  @apply relative overflow-hidden bg-neutral-600 flex justify-center m-2;
}
.tabs li {
  @apply text-center p-2 text-base; /*float-left list-item*/
}
.active {
  @apply border-emerald-600 border-b-2;
}
.mobile {
  @apply block sm:hidden;
}
.panel:has(.fullPage) {
  @apply w-full sm:max-w-full;
}
</style>
