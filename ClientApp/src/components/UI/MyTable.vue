<template>
  <div class="w-full p-3 overflow-auto">
    <table class="w-full text-center">
      <thead>
        <tr class="bg-zinc-700">
          <th class="p-3 border-x-2" v-for="item in Object.keys(headers)">{{ item }}</th>
          <th>Дії</th>
        </tr>
      </thead>
      <tbody>
        <tr class="bg-zinc-900 hover:bg-zinc-800" v-for="item in body">
          <td class="border-x-2" v-for="i in item">{{ i }}</td>
          <td class="max-w-[2rem]"><button>Edit</button></td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
import database from '@/main/database'
export default {
  props: {
    tableName: {
      required: true,
    },
  },
  data() {
    return {
      headers: [],
      body: [],
    }
  },
  watch: {
    tableName: {
      immediate: true,
      handler(val) {
        if (val != undefined) {
          database.getKeys(this.tableName).then((i) => {
            console.log(i)
            this.headers = i
          })
          database.getDataFromTable(this.tableName).then((i) => {
            this.body = i
          })
        }
      },
    },
  },
}
</script>
