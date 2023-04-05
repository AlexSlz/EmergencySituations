<template>
  <div class="p-2">
    <my-combo class="w-56 m-3" v-model="tableId" :items="tableNameList" />
    <MyTable :tableName="tableNameList[tableId - 1]" />
    <button
      @click="
        editmenu.open({ table: tableNameList[tableId - 1], fullPage: tableNameList[tableId - 1] != 'Надзвичайні ситуації' })
      "
    >
      Додати поле до таблиці {{ tableNameList[tableId - 1] }}
    </button>
  </div>
</template>
<script>
import { useEmergencyStore } from '@/stores/emergency'
import { useMenuStore } from '@/stores/editMenu'
import MyTable from '../UI/MyTable.vue'
import database from '@/main/database.js'
export default {
  data() {
    return {
      emergencyStore: useEmergencyStore(),
      editmenu: useMenuStore(),
      tableId: 1,
      tableNameList: [],
    }
  },
  mounted() {
    database.getTableNameList().then((res) => {
      this.tableNameList = res
    })
  },
  components: { MyTable },
}
</script>
<style></style>
