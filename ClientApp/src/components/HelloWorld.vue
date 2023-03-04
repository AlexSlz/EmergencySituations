<script setup lang="ts">
import axios from 'axios'
</script>

<template>
  <input v-model="userName" />
  <p>Power: {{ userPower }}</p>
  <input v-model="userPower" type="range" min="1" max="100" />
  <br />
  <button v-on:click="AddUser()">Add</button>

  <div v-for="(user, i) in users">
    <h1>{{ user.name }}</h1>
    <p>{{ user.power }}</p>
    <button v-on:click="RemoveUser(i)">Remove</button>
  </div>
</template>

<script lang="ts">
interface User {
  name: string
  power: number
}

export default {
  data() {
    return {
      userName: 'test',
      userPower: 0,
      users: [] as User[],
    }
  },
  created() {
    this.GetData()
  },
  methods: {
    GetData() {
      fetch('/api/getUsers')
        .then((res) => res.json())
        .then((data) => (this.users = data))
    },
    AddUser() {
      axios
        .post('/api/addUser', {
          name: this.userName,
          power: this.userPower,
        } as User)
        .then((res) => this.GetData())
    },
    RemoveUser(id: number) {
      axios.delete(`/api/removeUser/${id}`).then((res) => this.GetData())
    },
  },
}
</script>
