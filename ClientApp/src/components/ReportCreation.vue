<template>
  <button @click="CreateReport">Create</button>
</template>

<script>
import { useNotify } from '@/stores/Notify'
import database from '@/main/database'

export default {
  data() {
    return {
      notify: useNotify(),
    }
  },
  methods: {
    CreateReport() {
      database
        .CreateReport(2023)
        .then((e) => {
          console.log(e)
        })
        .catch((e) => {
          console.log(e)
          this.notify.Open(e, 'error')
        })
    },
  },
}
</script>
