<script setup lang="ts">
import axios from 'axios'
</script>

<template>
  <input v-model="userName" />
  <br />
  <button v-on:click="AddUser()">Add</button>

  <div v-for="user in users">
    <a>{{ user.Код }}</a>
    <h1>{{ user.Логін }}</h1>
    <p>{{ user.Пароль }}</p>
    <button v-on:click="RemoveUser(user.Код)">Remove</button>
  </div>
</template>

<script lang="ts">
interface User {
  Код: number
  Логін: string
  Пароль: string
}

export default {
  data() {
    return {
      userName: 'test',
      users: [] as User[],
    }
  },
  created() {
    this.GetData()
  },
  methods: {
    GetData() {
      fetch('/api/users')
        .then((res) => res.json())
        .then((data) => (this.users = data))
    },
    AddUser() {
      axios
        .post('/api/users', {
          Логін: this.userName,
          Пароль: '123',
        } as User)
        .then((res) => {
          console.log(res)
          this.GetData()
        })
    },
    RemoveUser(id: number) {
      axios.delete(`/api/users/${id}`).then((res) => {
        console.log(res)
        this.GetData()
      })
    },
  },
}
</script>
