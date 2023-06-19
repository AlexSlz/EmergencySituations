<template>
  <form @submit.prevent="auth()">
    <span class="flex items-center">
      <h1 class="px-2">Логін</h1>
      <input type="text" placeholder="Введіть логін..." v-model="login" />
    </span>
    <span class="flex items-center">
      <h1 class="px-2">Пароль</h1>
      <input type="password" placeholder="Введіть пароль..." v-model="pass" />
    </span>
    <input type="submit" value="Підтвердити" />
  </form>
</template>
<script>
import database from '@/main/database'
import { useAuthStore } from '@/stores/auth'
import { useNotify } from '@/stores/Notify'
export default {
  data() {
    return {
      login: '',
      pass: '',
      authStore: useAuthStore(),
      notify: useNotify(),
    }
  },
  methods: {
    auth() {
      database
        .GetToken(this.login, this.pass)
        .then((t) => {
          database.GetUserByToken(t).then((res) => {
            this.authStore.auth(res, t)
            this.notify.Open('Авторизація успішна.', 'success')
          })
        })
        .catch((e) => {
          this.notify.Open(e, 'error')
        })
    },
  },
}
</script>
