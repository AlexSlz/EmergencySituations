<template>
  <template v-if="items.length > 0">
    <div class="fixed select-none block m-auto bottom-4 sm:top-4 z-[40] w-64">
      <div
        class="bg-myMain hover:bg-mySecondActive transition flex items-center p-4 my-2 space-x-2"
        :class="item.type"
        :style="`opacity:${item.time}`"
        @click="closeNotify(item)"
        v-for="item in items"
      >
        <span class="text-3xl">
          <i v-if="item.type == 'success'" class="fas fa-info-circle"></i>
          <i v-else-if="item.type == 'error'" class="fas fa-exclamation-triangle"></i>
        </span>
        <div class="line-clamp-3 hover:line-clamp-none">{{ item.message }}</div>
      </div>
    </div>
  </template>
</template>

<script>
import { useNotify } from '@/stores/Notify'
export default {
  data() {
    return {
      items: [],
      notify: useNotify(),
    }
  },
  methods: {
    addNotify(message, type) {
      let notify = { message, type, time: 1, pause: false }
      this.items.push(notify)
    },
    closeNotify(item) {
      this.items.splice(this.items.indexOf(item), 1)
    },
  },
  mounted() {
    this.notify.Open = this.addNotify
  },
}
</script>

<style>
.success {
  @apply border-l-4 border-l-myActive;
}
.error {
  @apply border-l-4 border-l-myRed;
}
</style>
