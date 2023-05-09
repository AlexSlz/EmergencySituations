<template>
  <h1>Ім'я</h1>
  <input v-model="element.name" />
  <h1>Номер телефону</h1>
  <input v-model="element.phoneNumber" type="text" />
  <h1>Електронна пошта</h1>
  <input v-model="element.email" type="text" />
  <h1>Логін</h1>
  <input v-model="element.login" type="text" />
  <h1>Пароль</h1>
  <input v-model="element.password" type="text" />
</template>

<script>
export default {
  props: {
    element: {
      type: Object,
      default: {},
    },
  },
}
</script>
