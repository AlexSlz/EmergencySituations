<template>
  <div class="p-4 sm:flex h-[70vh]">
    <select class="bg-mySecond" size="5">
      <template v-for="i in help">
        <option v-if="i.forAdmin == auth.isAuth || !i.forAdmin" @click="select = i.Description" :value="i">
          {{ i.Name }}
        </option>
      </template>
    </select>
    <h1 class="w-full bg-mySecond text-justify text-xl p-2">{{ select }}</h1>
  </div>
</template>

<script>
import { useAuthStore } from '@/stores/auth'
import help from '@/main/help'
export default {
  data() {
    return {
      help: help,
      select: '',
      auth: useAuthStore(),
    }
  },
  mounted() {
    this.select = this.help[0].Description
  },
}
</script>

<style scoped>
option {
  @apply text-myText;
}
option:checked {
  @apply bg-myElement;
}
</style>
