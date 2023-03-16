<template>
  <form>
    <div v-for="(item, i) in Object.keys(tableVisual)">
      <h1>{{ item }}</h1>
      <my-input
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
      <MarkerList
        :items="tableData[item]"
        v-if="tableVisual[item].element == 'MarkerList'"
        @UpdateItems="updateCustomElement"
        :currentId="currentId"
      />
    </div>

    <my-button @click="addData()">OK</my-button>
  </form>
</template>
<script>
import axios from 'axios'
import database from '../main/database'
import MarkerList from './MarkerList.vue'

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
      customParameter: [{ table: 'Надзвичайні ситуації', child: 'Позиція НС', name: 'Позиції', elementName: 'MarkerList' }],
    }
  },
  methods: {
    loadData() {
      database.getLastIdFromTable(this.tableName).then((id) => {
        this.currentId = id + 1
        database.getKeys(this.tableName).then((res) => {
          var result = [...this.customParameter.filter((e) => e.table == this.tableName)]
          if (result != null) {
            result.forEach((element) => {
              res[element.name] = element.elementName
            })
          }
          var data = database.setupObject(res, { Код: this.currentId, Додав: 0 })
          this.tableVisual = data.dataVisual
          this.tableData = data.dataValue
        })
      })
    },

    updateCustomElement(element) {
      this.tableData['Позиції'] = element
      this.$emit('UpdateItems', element)
    },

    addData() {
      var temp = { ...this.tableData }
      delete temp.Код
      var filter = this.customParameter.filter((e) => e.name in temp)
      if (filter != null) {
        filter.forEach((i) => {
          axios.post(`api/tables/${i.child}/many`, temp[i.name])
          delete temp[i.name]
        })
      }
      axios.post(`api/tables/${this.tableName}`, temp).then((e) => {
        console.log('k')
      })
    },
  },
  watch: {
    tableName: {
      immediate: true,
      handler(val, oldVal) {
        if (val != undefined) this.loadData()
      },
    },
  },
  emits: ['UpdateItems', 'GetPosition'],
  components: { MarkerList },
}
</script>
