<script setup lang="ts">
import { onMounted, ref } from 'vue'

interface Data {
  name: string
}

const result = ref<Data>({ name: '' })
onMounted(async () => {
  result.value = await (await fetch('/api')).json()
})
</script>

<template>
  <h1>{{ result.name }}</h1>
</template>
