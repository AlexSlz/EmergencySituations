<template>
  <div class="p-2">
    <my-combo v-model="tableId" :items="tableNameList" @onChange="LoadTable"></my-combo>
    <myTable :headers="table[0]" :body="table[1]" :link="link" @onLink="test" />
  </div>
</template>
<script>
import database from '@/main/database.js'
export default {
  data() {
    return {
      tableId: 1,
      tableNameList: [],
      table: [[], []],
      link: [],
    }
  },
  methods: {
    test(id) {
      this.tableNameList.forEach((i) => {
        if (i.toLowerCase().includes(id.toLowerCase())) {
          this.LoadTable(this.tableNameList.indexOf(i) + 1)
          return
        }
      })
    },
    LoadTable(id) {
      this.tableId = id
      database.GetData(this.tableNameList[id - 1]).then((res) => {
        this.table[0] = Object.keys(res[0])
        this.table[1] = Object.values(res)
        this.table[1].forEach((i, index) => {
          Object.keys(i).forEach((j) => {
            if (typeof i[j] == 'object') {
              if ('name' in i[j]) {
                i[j] = i[j].name
                this.link.push({ name: j })
                return
              }
              if ('id' in i[j]) {
                i[j] = i[j].id
                this.link.push({ name: j })
                return
              }
            }
            if (Array.isArray(i[j])) {
              i[j] = i[j].map((p) => p.location).join(', ')
              this.link.push({ name: j })
              return
            }
          })
        })
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
