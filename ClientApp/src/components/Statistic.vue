<template>
  <button ref="myBtn" @click="GetStatistic()">Update</button>

  <myTable :headers="table1[0]" :body="table1[1]" />
  <h1 class="p-3">Кількість подій за рівнем:</h1>
  <myTable :headers="table2[0]" :body="table2[1]" />
  <h1 class="p-3">Кількість подій за типом:</h1>
  <myTable :headers="table3[0]" :body="table3[1]" />
</template>

<script>
import database from '@/main/database'
export default {
  data() {
    return {
      table1: [['Дата', 'Усього', 'Збитки'], []],
      table2: [[], []],
      table3: [[], []],
    }
  },
  methods: {
    GetStatistic() {
      database.GetStatistic(0).then((i) => {
        console.log(i)
        if (i.length == 0) return

        this.table2[0] = ['Дата'].concat(Object.keys(i[0].level))
        this.table3[0] = ['Дата'].concat(Object.keys(i[0].type))

        this.table1[1] = []
        this.table2[1] = []
        this.table3[1] = []
        i.forEach((element) => {
          this.table1[1].push([this.getMonthName(element.date), element.totalCount, element.costs])
          this.table2[1].push([this.getMonthName(element.date)].concat(Object.values(element.level)))
          this.table3[1].push([this.getMonthName(element.date)].concat(Object.values(element.type)))
        })

        // this.h = ['', 'За весь час'].concat(i.map((h) => this.getMonthName(h['Дата'])))
        // this.b = this.flipObject(i)
        // this.$refs.myBtn.disabled = true
        // setTimeout(() => {
        //   this.$refs.myBtn.disabled = false
        // }, 5000)
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

      if (monthNumber > 1000) return monthNumber
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
