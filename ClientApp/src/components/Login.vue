<template>
  <my-input v-model="login"></my-input>
  <my-input v-model="pass"></my-input>
  <my-button>OK</my-button>
</template>
<script>
export default {
  data() {
    return {
      login: '',
      pass: '',
    }
  },
}
</script>
<style lang=""></style>
