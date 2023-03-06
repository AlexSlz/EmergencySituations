<script setup lang="ts">
import axios from 'axios'
</script>

<template>
  <input v-model="userName" />
  <br />
  <button v-on:click="AddUser()">Add</button>

  <div v-for="user in users">
    <a>{{ user.код }}</a>
    <input v-model="user.логін" />
    <p>{{ user.пароль }}</p>
    <button v-on:click="UpdateUser(user)">Update</button>
    <button v-on:click="RemoveUser(user.код)">Remove</button>
  </div>
</template>

<script lang="ts">
interface User {
  код: number
  логін: string
  пароль: string
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
        .then((data) => {
          console.log(data[0].код)
          this.users = data
        })
    },
    AddUser() {
      var user = { логін: this.userName, пароль: '123' } as User
      axios.post('/api/users', user).then((res) => {
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
    UpdateUser(user: User) {
      axios.put(`/api/users`, user).then((res) => {
        console.log(res)
        this.GetData()
      })
    },
  },
}
</script>
