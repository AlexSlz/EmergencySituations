<template>
  <EmergencyTable :element="actionPanel.selected" />
  <button @click="confirm">Ok</button>
  <button @click="actionPanel.show = false">Cancel</button>
</template>
<script>
import EmergencyTable from './EmergencyTable.vue'
import { useActionPanel } from '@/stores/actionPanel'
import database from '@/main/database'
import { useEmergencyStore } from '@/stores/emergency'
import { useAuthStore } from '@/stores/auth'
import { useNotify } from '@/stores/Notify'
export default {
  data() {
    return {
      element: '',
      actionPanel: useActionPanel(),
      emergency: useEmergencyStore(),
      notify: useNotify(),
      auth: useAuthStore(),
    }
  },
  methods: {
    confirm() {
      console.log(this.actionPanel.selected)
      if ('id' in this.actionPanel.selected) {
        database.EditTable(this.actionPanel.tableName, this.actionPanel.selected).then((res) => {
          this.onComplete()
        })
      } else {
        if (this.actionPanel.tableName == 'Emergency') {
          this.actionPanel.selected.addedBy = this.auth.userData.id
        }
        database.AddToTable(this.actionPanel.tableName, this.actionPanel.selected).then((res) => {
          this.onComplete()
        })
      }
    },
    onComplete() {
      this.emergency.reLoad().then(() => {
        if (this.actionPanel.tableName == 'Emergency') database.DeleteFiles('Emergency')
        this.actionPanel.show = false
        this.notify.Open(`${this.actionPanel.name}, ${this.actionPanel.tableName}.`, 'success')
      })
    },
  },
  beforeUnmount() {
    this.actionPanel.selected = {}
  },
  components: { EmergencyTable },
}
</script>
