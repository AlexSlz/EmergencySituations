<template>
  <div class="p-2">
    <span class="flex">
      <my-combo :disabled="disableSelect" v-model="tableId" :items="tableNameList" @onChange="LoadTable"></my-combo>
      <button ref="myBtn" @click="LoadTable(tableId)">Оновити</button>
    </span>
    <template v-if="!loading && !empty">
      <button
        v-if="tableNameList[tableId - 1] != 'Positions' && tableNameList[tableId - 1] != 'Losses'"
        @click="actionPanel.open({ tableName: tableNameList[tableId - 1] })"
      >
        Додати запис до таблиці {{ tableNameList[tableId - 1] }}
      </button>
      <span v-if="maxPage > 1" class="flex flex-wrap">
        <h1
          :class="i == page ? 'bg-myActive' : ''"
          class="p-2 border-2 m-1 cursor-pointer select-none hover:bg-myElement w-10 text-center"
          @click="SelectPage(i)"
          v-for="i in parseInt(maxPage)"
        >
          {{ i }}
        </h1>
      </span>
      <myTable :addon="true" :headers="table[0]" :body="table[1]" @onLink="LoadTableById" v-slot="prop">
        <button @click="actionPanel.open({ tableName: tableNameList[tableId - 1], selected: prop.item })">Редагувати</button>
        <button @click="DeleteData(prop.item)" class="bg-myRed">Delete</button>
      </myTable>
    </template>
    <h1 v-if="loading">Завантаження...</h1>
    <h1 v-if="empty">{{ message }}</h1>
  </div>
</template>
<script>
import { useActionPanel } from '@/stores/actionPanel'
import { useNotify } from '@/stores/Notify'
import database from '@/main/database.js'
export default {
  data() {
    return {
      page: 1,
      maxPage: 1,
      tableId: 1,
      tableNameList: [],
      table: [[], []],
      link: [],
      actionPanel: useActionPanel(),
      notify: useNotify(),
      loading: true,
      disableSelect: true,
      empty: false,
      message: 'Таблиця порожня.',
    }
  },
  methods: {
    LoadTableById(id) {
      this.tableNameList.forEach((i) => {
        if (i.toLowerCase().includes(id.toLowerCase())) {
          this.LoadTable(this.tableNameList.indexOf(i) + 1)
          return
        }
      })
    },
    SelectPage(p) {
      this.page = p
      this.LoadTable(this.tableId)
    },
    LoadTable(id) {
      this.loading = true
      this.$refs.myBtn.disabled = true
      this.disableSelect = true
      this.tableId = id
      database
        .GetData(this.tableNameList[id - 1], 30, this.page)
        .then((res) => {
          this.empty = false
          if (res.data.length <= 0) {
            this.empty = true
            return
          }
          this.maxPage = res.maxPage
          this.table[0] = Object.keys(res.data[0]).concat('Дії')
          this.table[1] = Object.values(res.data)
        })
        .then(() => {
          this.loading = false
          setTimeout(() => {
            this.$refs.myBtn.disabled = false
            this.disableSelect = false
          }, 1000)
        })
    },
    DeleteData(item) {
      this.loading = true
      database
        .DeleteData(this.tableNameList[this.tableId - 1], item)
        .then((e) => {
          this.loading = false
          if (e != null) this.LoadTable(this.tableId)
        })
        .catch((e) => {
          this.notify.Open(e, 'error')
        })
    },
  },
  mounted() {
    database
      .GetTableNameList()
      .then((res) => {
        this.tableNameList = res
      })
      .finally(() => {
        this.LoadTable(this.tableId)
      })
  },
}
</script>
<style></style>
