<template>
  <p class="p-2 text-rose-600">{{ ErrorMsg }}</p>
  <form @submit.prevent="auth()">
    <input class="input" type="text" placeholder="Login" v-model="login" />
    <input class="input" type="password" placeholder="Password" v-model="pass" />
    <input class="input" type="submit" />
  </form>
</template>
<script>
import database from '@/main/database'
import { useAuthStore } from '@/stores/auth'

export default {
  data() {
    return {
      login: 'admin',
      pass: '123',
      ErrorMsg: '',
      authStore: useAuthStore(),
    }
  },
  methods: {
    auth() {
      database
        .getToken(this.login, this.pass)
        .then((t) => {
          this.authStore.token = t
          this.ErrorMsg = 'ok'
        })
        .catch((e) => {
          this.ErrorMsg = e
          this.authStore.logout()
        })
    },
  },
}
</script>
<style lang=""></style>
