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
      <input
        v-if="tableVisual[item].element == 'file'"
        type="file"
        :disabled="tableVisual[item].disabled"
        accept="image/png, image/jpeg"
        @change="loadFile"
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
      <MarkerList :items="this.emergencyStore.tempPoints" v-if="tableVisual[item].element == 'Points'" />
    </div>
    <button @click="emergencyStore.selectedElement != null ? editData() : addData()" :disabled="process">OK</button>
    <button @click="editmenu.show = false" :disabled="process">Cancel</button>
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
      tableVisual: [],
      tableData: [],
      emergencyStore: useEmergencyStore(),
      authStore: useAuthStore(),
      editmenu: useMenuStore(),
      tempFile: null,
      process: false,
    }
  },
  methods: {
    loadFile(event) {
      let temp = event.target.files[0]
      if (temp != null) {
        this.tempFile = temp
      }
    },
    editData() {
      this.process = true
      var temp = { ...this.tableData }
      if ('Позиції' in temp) {
        delete temp['Позиції']
      }

      database
        .editTable(this.tableName, temp)
        .then((e) => {
          if (this.tableName == 'Надзвичайні ситуації') {
            if ('Зображення' in temp && this.tempFile != null) {
              database.loadFile(this.tableName, this.tempFile, temp['Код'])
            }
            this.emergencyStore.tempPoints.map((i) => (i['Код нс'] = temp['Код']))
            let difference = this.emergencyStore.selectedElement.Позиції.filter((x) => {
              var re = !this.emergencyStore.tempPoints.some((y) => y.Код == x.Код)
              return re
            })
            if (difference.length > 0) {
              database.deleteData('Позиції НС', difference)
            }

            database.editTable('Позиції НС', this.emergencyStore.tempPoints).then((e) => {
              console.log(e)
            })
          }
        })
        .finally((e) => {
          this.editmenu.show = false
        })
        .catch((e) => {
          this.process = false
          console.log(e)
        })
    },
    addData() {
      this.process = true
      var temp = { ...this.tableData }

      delete temp.Код
      if ('Позиції' in temp) {
        delete temp['Позиції']
      }
      database
        .addToTable(this.tableName, temp)
        .then((e) => {
          if (this.tableName == 'Надзвичайні ситуації') {
            if ('Зображення' in temp && this.tempFile != null) {
              database.loadFile(this.tableName, this.tempFile, e[0]['Код'])
            }
            this.emergencyStore.tempPoints.map((i) => (i['Код нс'] = e[0].Код))
            database.addToTable('Позиції НС', this.emergencyStore.tempPoints).then((e) => {
              console.log(e)
            })
          }
        })
        .finally((e) => {
          this.editmenu.show = false
        })
        .catch((e) => {
          this.process = false
        })
    },
    loadForm() {
      this.process = false

      database.getKeys(this.tableName).then((res) => {
        var temp = { Код: this.currentId }
        if (this.tableName == 'Надзвичайні ситуації') {
          res.Позиції = 'Points'
          if (this.emergencyStore.selectedElement != null) {
            temp = this.emergencyStore.selectedElement
          }
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
    },
  },
  components: { MarkerList },
}
</script>
