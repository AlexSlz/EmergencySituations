<script setup>
import MarkerList from '@/components/MarkerList.vue'
import { useEmergencyStore } from '@/stores/emergency'
import database from '@/main/database'
import formManager from '@/main/formInit'
import { useAuthStore } from '@/stores/auth'
</script>

<template>
  <button class="input" @click="loadForm()">TeSt</button>
  <form @submit.prevent>
    <div v-for="item in Object.keys(tableVisual)">
      <h1>{{ item }}</h1>

      <input
        class="input"
        v-model="tableData[item]"
        v-if="tableVisual[item].element == 'input'"
        :type="tableVisual[item].type"
        :disabled="tableVisual[item].disabled"
      />
      <my-combo
        v-model="tableData[item]"
        v-if="tableVisual[item].element == 'combo'"
        :tableName="tableVisual[item].table"
        :disabled="tableVisual[item].disabled"
      />
      <textarea
        class="input"
        v-model="tableData[item]"
        v-if="tableVisual[item].element == 'textarea'"
        :disabled="tableVisual[item].disabled"
      ></textarea>
      <MarkerList :items="this.emergencyStore.tempPoints" v-if="tableVisual[item].element == 'Points'" :currentId="currentId" />
    </div>
    <button class="input" @click="addData()" type="submit">OK</button>
  </form>
</template>
<script>
export default {
  props: {
    tableName: {
      required: true,
    },
  },
  data() {
    return {
      currentId: 0,
      tableVisual: [],
      tableData: [],
      emergencyStore: useEmergencyStore(),
      authStore: useAuthStore(),
    }
  },
  methods: {
    addData() {
      var temp = { ...this.tableData }
      delete temp.Код

      if ('Позиції' in temp) {
        delete temp['Позиції']
      }
      console.log(temp)
      database
        .addToTable(this.tableName, temp)
        .then((e) => {
          if (this.tableName == 'Надзвичайні ситуації') {
            database.addToTable('Позиції НС', this.emergencyStore.tempPoints)
          }
          console.log(e)
        })
        .catch((e) => {})
    },
    loadForm() {
      database.getLastIdFromTable(this.tableName).then((id) => {
        this.currentId = id + 1
        database.getKeys(this.tableName).then((res) => {
          if (this.tableName == 'Надзвичайні ситуації') {
            res.Позиції = 'Points'
          }
          var data = formManager.setupObject(res, this.emergencyStore.selectedElement || { Код: this.currentId })
          this.tableVisual = data.dataVisual
          this.tableData = data.dataValue
          if (this.tableName == 'Надзвичайні ситуації') {
            this.emergencyStore.tempPoints = JSON.parse(JSON.stringify(data.dataValue.Позиції))
            this.tableData.Додав = this.authStore.userData.Код
          }
          console.log(this.tableData, this.tableVisual)
        })
      })
    },
  },
  mounted() {
    //this.loadForm()
  },
}
</script>
