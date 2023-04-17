<template>
  <div>
    <slot></slot>
    <h1 class="break-all">{{ element.name }}</h1>

    <template v-if="authStore.isAuth">
      <h1>WOW</h1>
    </template>
  </div>
</template>
<script>
import { useAuthStore } from '@/stores/auth'
export default {
  props: {
    element: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      authStore: useAuthStore(),
    }
  },
  methods: {},
}
</script>
