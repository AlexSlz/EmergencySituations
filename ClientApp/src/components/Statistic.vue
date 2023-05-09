<template>
  <button ref="myBtn" @click="GetStatistic()" disabled>Оновити</button>

  <select ref="mySelect" v-model="year" @change="GetStatistic()" disabled>
    <option class="text-myBG" value="0">За весь час</option>
    <option class="text-myBG" :value="y" v-for="y in years">{{ y }}</option>
  </select>
  <template v-if="isLoaded">
    <myTable :headers="table1[0]" :body="table1[1]" />
    <h1 class="p-3 font-bold">Кількість подій за рівнем:</h1>
    <myTable :headers="table2[0]" :body="table2[1]" />
    <h1 class="p-3 font-bold">Кількість подій за типом:</h1>
    <myTable :headers="table3[0]" :body="table3[1]" />
  </template>
  <template v-else>Завантаження...</template>

  <ReportCreation />
</template>

<script>
import database from '@/main/database'
import { useNotify } from '@/stores/Notify'
import ReportCreation from './ReportCreation.vue'
export default {
  data() {
    return {
      isLoaded: false,
      years: [],
      year: 0,
      table1: [['Дата', 'Усього', 'Збитки'], []],
      table2: [[], []],
      table3: [[], []],
      notify: useNotify(),
    }
  },
  methods: {
    GetStatistic() {
      this.$refs.myBtn.disabled = true
      this.$refs.mySelect.disabled = true
      this.isLoaded = false
      database
        .GetStatistic(this.year)
        .then((i) => {
          if (i.length == 0) return
          this.table2[0] = ['Дата'].concat(Object.keys(i[0].level))
          this.table3[0] = ['Дата'].concat(Object.keys(i[0].type))
          this.table1[1] = []
          this.table2[1] = []
          this.table3[1] = []
          i.forEach((element) => {
            this.table1[1].push([this.getMonthName(element.date), element.totalCount, element.losses.costs])
            this.table2[1].push([this.getMonthName(element.date)].concat(Object.values(element.level)))
            this.table3[1].push([this.getMonthName(element.date)].concat(Object.values(element.type)))
          })
        })
        .finally(() => {
          this.isLoaded = true
          setTimeout(() => {
            this.$refs.myBtn.disabled = false
            this.$refs.mySelect.disabled = false
          }, 1000)
        })
        .catch((e) => {
          this.notify.Open(e, 'error')
        })
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
    database.GetYears().then((i) => (this.years = i))
  },
  components: { ReportCreation },
}
</script>
