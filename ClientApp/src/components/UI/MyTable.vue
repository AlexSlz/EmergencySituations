<template>
  <button>Update</button>
  <div class="w-full p-3 overflow-auto">
    <table class="w-full text-center">
      <thead>
        <tr class="bg-mySecond">
          <th class="p-3 border-x-2" v-for="item in Object.keys(headers)">{{ item }}</th>
          <th>Дії</th>
        </tr>
      </thead>
      <tbody>
        <tr class="hover:bg-mySecondActive transition-colors" v-for="item in body">
          <td class="border-x-2 p-2" v-for="i in item">
            <p class="line-clamp-2 hover:line-clamp-none">{{ i }}</p>
          </td>
          <td class="w-56">
            <button
              @click="
                editmenu.open({ name: 'Edit', table: tableName, element: item, fullPage: tableName != 'Надзвичайні ситуації' })
              "
            >
              Edit
            </button>
            <button class="hover:bg-myRed" @click="deleteElement(item)">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
import { useMenuStore } from '@/stores/editMenu'
import database from '@/main/database'
export default {
  props: {
    tableName: {
      required: true,
    },
  },
  data() {
    return {
      editmenu: useMenuStore(),
      headers: [],
      body: [],
    }
  },
  methods: {
    deleteElement(item) {
      database.deleteData(this.tableName, item).then((i) => {
        let index = this.body.indexOf(item)
        if (index > -1) this.body.splice(index, 1)
      })
    },
    loadData() {
      database.getKeys(this.tableName).then((i) => {
        console.log(i)
        this.headers = i
      })
      database.getDataFromTable(this.tableName).then((i) => {
        this.body = i
      })
    },
  },
  watch: {
    tableName: {
      immediate: true,
      handler(val) {
        if (val != undefined) {
          this.loadData()
        }
      },
    },
  },
}
</script>
