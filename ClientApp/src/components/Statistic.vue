<template>
  <button ref="myBtn" @click="GetStatistic()">Update</button>
  <select v-model="select" @change="GetStatistic()">
    <option class="text-mySecond" :value="item" v-for="item in type">{{ item }}</option>
  </select>
  <myTable :first="true" :headers="h" :body="b" />
</template>

<script>
import database from '@/main/database'
export default {
  data() {
    return {
      h: [],
      b: [],
      type: ['Місяць', 'Рік'],
      select: 'Місяць',
    }
  },
  methods: {
    GetStatistic() {
      database.GetStatistic(this.select).then((i) => {
        if (i.length == 0) return

        this.h = ['', 'За весь час'].concat(i.map((h) => this.getMonthName(h['Дата'])))
        this.b = this.flipObject(i)
        this.$refs.myBtn.disabled = true
        setTimeout(() => {
          this.$refs.myBtn.disabled = false
        }, 5000)
      })
    },
    flipObject(data) {
      let res = []
      Object.keys(data[0]).forEach((key) => {
        if (key != 'Дата') {
          let count = data.map((t) => t[key]).reduce((p, a) => p + a, 0)
          var temp = [key, count].concat(data.map((h) => h[key]))
          if (typeof data[0][key] == 'object') {
            Object.keys(data[0][key]).forEach((k) => {
              let count = data.map((t) => t[key][k]).reduce((p, a) => p + a, 0)
              res.push([k + ' ' + key, count].concat(data.map((h) => h[key][k])))
            })
          } else {
            res.push(temp)
          }
        }
      })
      return res
    },
    getMonthName(monthNumber) {
      const date = new Date()
      date.setMonth(monthNumber - 1)

      if (this.select != this.type[0]) return monthNumber
      return date.toLocaleString('uk-UA', {
        month: 'long',
      })
    },
  },
  mounted() {
    this.GetStatistic()
  },
}
</script>
