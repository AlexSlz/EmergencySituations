<template>
  <div>
    <slot></slot>
    <h1 class="break-all">{{ this.emergencyStore.selectedElement.Назва }}</h1>

    <p>{{ getPosition() }}</p>

    <template v-if="authStore.isAuth">
      <button v-if="authStore.isAuth" @click="editmenu.open({ name: 'Edit', element: emergencyStore.selectedElement })">
        Edit
      </button>
      <button class="hover:bg-myRed" @click="deleteData">Delete</button>
    </template>
  </div>
</template>
<script>
import { useAuthStore } from '@/stores/auth'
import { useMenuStore } from '@/stores/editMenu'
import { useEmergencyStore } from '@/stores/emergency'
export default {
  data() {
    return {
      authStore: useAuthStore(),
      editmenu: useMenuStore(),
      emergencyStore: useEmergencyStore(),
    }
  },
  methods: {
    getPosition() {
      let res = this.emergencyStore.selectedElement.Позиції.map((i) => i.Розташування)
      res = [...new Set(res)]
      return res.join(', ')
    },
    deleteData() {
      this.emergencyStore
        .removeSelected()
        .then((e) => {
          console.log(e)
        })
        .catch((e) => {
          console.log(e)
        })
      // database
      //   .deleteData('Надзвичайні ситуації', this.emergencyStore.selectedElement)
      //   .then((e) => {
      //     this.emergencyStore.selectElement(null)
      //     this.emergencyStore.needUpdate.extra = true
      //   })
      //   .catch((e) => {
      //     console.log(e)
      //   })
    },
  },
}
</script>
