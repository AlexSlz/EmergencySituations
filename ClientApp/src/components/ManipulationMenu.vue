<template>
  <div>
    <div v-for="item in Object.keys(tableVisual)">
      <h1>{{ item }}</h1>

      <input
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
        v-model="tableData[item]"
        v-if="tableVisual[item].element == 'textarea'"
        :disabled="tableVisual[item].disabled"
      ></textarea>
      <MarkerList :items="this.emergencyStore.tempPoints" v-if="tableVisual[item].element == 'Points'" :currentId="currentId" />
    </div>
    <button @click="emergencyStore.selectedElement != null ? editData() : addData()">OK</button>
    <button @click="editmenu.show = false">Cancel</button>
  </div>
</template>
<script>
import MarkerList from '@/components/MarkerList.vue'
import { useEmergencyStore } from '@/stores/emergency'
import database from '@/main/database'
import formManager from '@/main/formInit'
import { useAuthStore } from '@/stores/auth'
import { useMenuStore } from '@/stores/editMenu'

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
      editmenu: useMenuStore(),
    }
  },
  methods: {
    editData() {
      var temp = { ...this.tableData }
      console.log(temp)
      if ('Позиції' in temp) {
        delete temp['Позиції']
      }
      database
        .editTable(this.tableName, temp)
        .then((e) => {
          if (this.tableName == 'Надзвичайні ситуації') {
            this.emergencyStore.tempPoints.map((i) => (i['Код нс'] = temp['Код']))
            let difference = this.emergencyStore.selectedElement.Позиції.filter((x) => {
              var re = !this.emergencyStore.tempPoints.some((y) => y.Код == x.Код)
              return re
            })
            if (difference.length > 0) {
              database.deleteData('Позиції НС', difference)
            }

            database.editTable('Позиції НС', this.emergencyStore.tempPoints).then((e) => {
              this.editmenu.show = false
            })
          }
        })
        .catch((e) => {
          console.log(e)
        })
    },
    addData() {
      var temp = { ...this.tableData }
      console.log(temp)
      delete temp.Код

      if ('Позиції' in temp) {
        delete temp['Позиції']
      }
      database
        .addToTable(this.tableName, temp)
        .then((e) => {
          if (this.tableName == 'Надзвичайні ситуації') {
            this.emergencyStore.tempPoints.map((i) => (i['Код нс'] = e[0].Код))

            database.addToTable('Позиції НС', this.emergencyStore.tempPoints).then((e) => {
              this.editmenu.show = false
            })
          }
        })
        .catch((e) => {})
    },
    loadForm() {
      database.getLastIdFromTable(this.tableName).then((id) => {
        this.currentId = id + 1
        database.getKeys(this.tableName).then((res) => {
          var temp = { Код: this.currentId }
          if (this.tableName == 'Надзвичайні ситуації') {
            res.Позиції = 'Points'
            if (this.emergencyStore.selectedElement != null) temp = this.emergencyStore.selectedElement
          }
          var data = formManager.setupObject(res, temp)
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
  components: { MarkerList },
}
</script>
