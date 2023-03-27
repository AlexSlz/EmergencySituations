<template>
  <div>
    <slot></slot>
    <h1 class="break-all mt-5">{{ selectedElement.Назва }}</h1>
    <button v-if="authStore.isAuth" class="input" @click="editmenu.open('Edit', true)">Edit</button>
  </div>
</template>
<script>
import { useAuthStore } from '@/stores/auth'
import { useMenuStore } from '@/stores/editMenu'
export default {
  props: {
    selectedElement: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      authStore: useAuthStore(),
      editmenu: useMenuStore(),
    }
  },
}
</script>
