<template>
  <div :class="{ fold: fold }" class="panel">
    <ul class="tabs">
      <li v-for="tab in tabs" :class="{ active: tab.isActive, mobile: tab.onlyMobile, fullPage: tab.isActive && tab.fullPage }">
        <a class="select-none" @click="selectTab(tab)">{{ tab.name }}</a>
      </li>
    </ul>
    <div class="">
      <slot></slot>
    </div>
  </div>
</template>
<script>
import { useNavPanel } from '@/stores/NavPanel'
export default {
  data() {
    return {
      tabs: [],
      fold: false,
      nav: useNavPanel(),
    }
  },
  created() {
    this.tabs.push({ name: 'Map', fold: '', onlyMobile: true })
    this.nav.Select = this.selectTabById
  },
  methods: {
    selectTab(selectedTab) {
      if (selectedTab.isActive && !selectedTab.onlyMobile) selectedTab.tabClick()
      this.fold = 'fold' in selectedTab
      this.tabs.forEach((tab) => {
        tab.isActive = tab.name == selectedTab.name
      })
    },
    selectTabById(id) {
      if (!this.tabs[id].isActive) this.selectTab(this.tabs[id])
    },
  },
}
</script>
<style scoped>
.panel {
  @apply absolute z-20 right-0 top-0 h-full overflow-y-auto bg-myBG min-w-[100%] sm:min-w-[30vw] sm:max-w-sm;
}
.fold {
  @apply h-auto;
}
.tabs {
  @apply overflow-hidden bg-mySecond flex flex-wrap justify-center m-2;
}
.tabs li {
  @apply text-center p-2 text-base; /*float-left list-item*/
}
.active {
  @apply border-myMain border-b-2;
}
.mobile {
  @apply block sm:hidden;
}
.panel:has(.fullPage) {
  @apply w-full sm:max-w-full;
}
</style>
