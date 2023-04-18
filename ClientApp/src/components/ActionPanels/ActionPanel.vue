<template>
  <EmergencyTable :element="actionPanel.selected" />
  <button @click="confirm">Ok</button>
  <button @click="actionPanel.show = false">Cancel</button>
</template>
<script>
import EmergencyTable from './EmergencyTable.vue'
import { useActionPanel } from '@/stores/actionPanel'
import database from '@/main/database'

export default {
  data() {
    return {
      element: '',
      actionPanel: useActionPanel(),
    }
  },
  methods: {
    confirm() {
      console.log(this.actionPanel.selected)
      database.AddToTable(this.actionPanel.tableName, this.actionPanel.selected).then((res) => {
        console.log(res)
      })
    },
  },
  components: { EmergencyTable },
}
</script>
