<template>
  <div class="p-2">
    <span class="flex">
      <my-combo :disabled="disableSelect" v-model="tableId" :items="tableNameList" @onChange="LoadTable"></my-combo>
      <button ref="myBtn" @click="LoadTable(tableId, false)">Оновити</button>
    </span>
    <template v-if="!loading && !empty">
      <Search
        ref="search"
        :keys="Object.keys(keys)"
        :search="searchStore"
        :tableName="tableNameList[tableId - 1]"
        :hideMenu="true"
        @onSortChange="LoadTable(tableId, false)"
      />
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
        <button @click="DeleteData(prop.item)" class="bg-myRed">Видалити</button>
      </myTable>
    </template>
    <h1 v-if="loading">Завантаження...</h1>
    <h1 v-if="empty">{{ message }}</h1>
  </div>
  <h1 class="p-3">Резервне копіювання БД</h1>
  <Backup />
</template>
<script>
import { useActionPanel } from '@/stores/actionPanel'
import { useNotify } from '@/stores/Notify'
import database from '@/main/database.js'
import Search from '@/components/EventList/Search.vue'
import { useSearch } from '@/stores/search'
import Backup from './Backup.vue'
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
      searchStore: useSearch('Id'),
      loading: true,
      disableSelect: true,
      empty: false,
      message: 'Таблиця порожня.',
      keys: [],
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
      if (this.searchStore.notEmpty) {
        this.$refs.search.Find(this.page)
      } else {
        this.LoadTable(this.tableId)
      }
    },
    LoadTable(id, first = true) {
      if (first) this.searchStore.order = 'Id'
      this.loading = true
      this.$refs.myBtn.disabled = true
      this.disableSelect = true
      this.tableId = id
      database
        .GetData(this.tableNameList[id - 1], this.page, this.searchStore.GetOrder())
        .then((res) => {
          this.empty = false
          if (res.data.length <= 0) {
            this.empty = true
            return
          }
          this.maxPage = res.maxPage
          this.table[0] = Object.keys(res.data[0]).concat('Дії')
          this.table[1] = Object.values(res.data)
          database.GetKeys(this.tableNameList[id - 1]).then((keys) => {
            this.keys = keys
          })
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
  watch: {
    'searchStore.result.length': {
      deep: true,
      handler(val) {
        if (val > 0) {
          this.maxPage = this.searchStore.maxPage
          this.table[0] = Object.keys(this.searchStore.result[0]).concat('Дії')
          this.table[1] = Object.values(this.searchStore.result)
        } else {
          this.LoadTable(this.tableId)
        }
      },
    },
  },
  components: { Search, Backup },
}
</script>
<style></style>
