<template>
  <EmergencyStatTable
    v-if="actionPanel.tableName == 'EmergencyLevel' || actionPanel.tableName == 'EmergencyType'"
    :element="actionPanel.selected"
  />
  <LossesTable v-if="actionPanel.tableName == 'Losses'" :element="actionPanel.selected" />
  <EmergencyTable v-if="actionPanel.tableName == 'Emergency'" :element="actionPanel.selected" />
  <PositionsTable v-if="actionPanel.tableName == 'Positions'" :element="actionPanel.selected" />
  <UsersTable v-if="actionPanel.tableName == 'Users'" :element="actionPanel.selected" />
  <h1 v-if="loading">Завантаження...</h1>
  <template v-else>
    <button @click="confirm">Підтвердити</button>
    <button @click="actionPanel.show = false">Скасувати</button>
  </template>
</template>
<script>
import EmergencyTable from './Tables/EmergencyTable.vue'
import LossesTable from './Tables/LossesTable.vue'
import EmergencyStatTable from './Tables/EmergencyStatTable.vue'
import { useActionPanel } from '@/stores/actionPanel'
import database from '@/main/database'
import { useEmergencyStore } from '@/stores/emergency'
import { useAuthStore } from '@/stores/auth'
import { useNotify } from '@/stores/Notify'
import PositionsTable from './Tables/PositionsTable.vue'
import UsersTable from './Tables/UsersTable.vue'
export default {
  data() {
    return {
      element: '',
      actionPanel: useActionPanel(),
      emergency: useEmergencyStore(),
      notify: useNotify(),
      auth: useAuthStore(),
      loading: false,
    }
  },
  methods: {
    confirm() {
      this.loading = true
      if ('id' in this.actionPanel.selected) {
        if (this.actionPanel.tableName == 'Positions') {
          delete this.actionPanel.selected.positions
        }
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
  components: { EmergencyTable, LossesTable, EmergencyStatTable, PositionsTable, UsersTable },
}
</script>
