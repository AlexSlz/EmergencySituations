<template>
  <div class="pt-10">
    <h1 class="px-2 font-bold">Створення звіту</h1>
    <span class="flex items-center">
      <h1 class="px-2">Рік</h1>
      <select v-model="year" @change="GetMonth">
        <option class="text-myBG" :value="y" v-for="y in years">{{ y }}</option>
      </select>
    </span>
    <span class="flex items-center">
      <h1 class="px-2">Місяць</h1>
      <select v-model="month" @change="GetMonth">
        <option class="text-myBG" value="0">Тільки за рік</option>
        <option class="text-myBG" :value="m" v-for="m in months">{{ getMonthName(m) }}</option>
      </select>
    </span>
    <button v-if="!loading" class="mb-10" @click="CreateReport">
      Створити звіт за {{ year }}{{ month != 0 ? `, ${getMonthName(month)}` : '' }}
    </button>
    <h1 class="mb-10 p-2" v-if="loading">Створення...</h1>
  </div>
</template>

<script>
import { useNotify } from '@/stores/Notify'
import database from '@/main/database'

export default {
  props: {
    years: {
      required: true,
    },
  },
  data() {
    return {
      year: new Date().getFullYear(),
      months: [],
      month: 0,
      notify: useNotify(),
      loading: false,
    }
  },
  methods: {
    getMonthName(monthNumber) {
      const date = new Date()
      date.setMonth(monthNumber - 1)
      return date.toLocaleString('uk-UA', {
        month: 'long',
      })
    },
    GetMonth() {
      database.GetYears(this.year).then((res) => {
        this.months = res
      })
    },
    CreateReport() {
      this.loading = true
      database
        .CreateReport(this.year, this.month)
        .then((e) => {
          this.loading = false
          console.log(e)
        })
        .catch((e) => {
          console.log(e)
          this.notify.Open(e, 'error')
        })
    },
  },
  beforeMount() {
    this.GetMonth()
  },
}
</script>
