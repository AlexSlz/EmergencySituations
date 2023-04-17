<template>
  <p class="p-2 text-myRed">{{ ErrorMsg }}</p>
  <form @submit.prevent="auth()">
    <input type="text" placeholder="Login" v-model="login" />
    <input type="password" placeholder="Password" v-model="pass" />
    <input type="submit" />
  </form>
</template>
<script>
import database from '@/main/database'
import { useAuthStore } from '@/stores/auth'

export default {
  data() {
    return {
      login: 'Admin',
      pass: '123',
      ErrorMsg: '',
      authStore: useAuthStore(),
    }
  },
  methods: {
    auth() {
      database
        .GetToken(this.login, this.pass)
        .then((t) => {
          database.GetUserByToken(t).then((res) => {
            this.authStore.auth(res, t)
          })
        })
        .catch((e) => {
          this.ErrorMsg = e
          this.authStore.logout()
        })
    },
  },
}
</script>
