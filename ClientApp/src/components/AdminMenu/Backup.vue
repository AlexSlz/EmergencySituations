<template>
  <table>
    <tr>
      <th>
        <button @click="LoadList()" :disabled="loading">Оновити</button>
      </th>
      <th class="w-full">
        <button @click="LoadBackUp()">Завантажити файл</button>
        <button class="bg-myRed" @click="LoadBackUp(true)">Видалити</button>
      </th>
    </tr>
    <tr>
      <td>
        <button @click="Create()">Створити</button>
      </td>
      <td>
        <select v-model="backUp" class="bg-mySecond" size="5">
          <option :value="i" v-for="i in list">{{ i }}</option>
        </select>
      </td>
    </tr>
  </table>
</template>

<script>
import database from '@/main/database'
import { useNotify } from '@/stores/Notify'
export default {
  data() {
    return {
      backUp: '',
      list: [],
      notify: useNotify(),
      loading: false,
    }
  },
  methods: {
    Create() {
      database
        .CreateBackup()
        .then((i) => {
          this.notify.Open(i, 'success')
          this.LoadList()
        })
        .catch((e) => {
          this.notify.Open(e, 'error')
        })
    },
    LoadList() {
      this.loading = true
      database
        .GetBackUpList()
        .then((i) => {
          this.list = i
          this.loading = false
        })
        .catch((e) => {
          this.notify.Open(e, 'error')
        })
    },
    LoadBackUp(del = false) {
      if (this.backUp == '') {
        this.notify.Open('Файл не вибрано.', 'error')
        return
      }
      database
        .LoadFileBackup(this.backUp, del)
        .then((i) => {
          this.notify.Open(i, 'success')
          this.LoadList()
        })
        .catch((e) => {
          this.notify.Open(e, 'error')
        })
    },
  },
  mounted() {
    this.LoadList()
  },
}
</script>

<style scoped>
option {
  @apply text-myText;
}
option:checked {
  @apply bg-myElement;
}
</style>
