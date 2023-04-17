<script setup>
import MapView from '@/components/MapView.vue'
import EventList from '@/components/EventList/EventList.vue'
import Notation from './components/Notation.vue'
import Admin from './components/AdminMenu/Admin.vue'
import MyPanel from '@/components/Panel/PanelBody.vue'
import Tab from '@/components/Panel/PanelTab.vue'
import Login from '@/components/Login.vue'

import { useAuthStore } from '@/stores/auth'
import { useMenuStore } from '@/stores/editMenu'
import database from '@/main/database'
</script>

<template>
  <MapView class="h-screen z-0 w-full sm:w-[70vw]" ref="Map" />
  <MyPanel>
    <Tab name="Події" :selected="true">
      <h1 class="text-center pt-3" v-if="isListLoading">Завантаження списку подій...</h1>
      <EventList ref="List" v-if="!isListLoading" />
    </Tab>
    <Tab name="Позначення"><Notation /></Tab>
    <template v-if="!authStore.isAuth">
      <Tab v-if="!authStore.isAuth" name="Login"><Login /></Tab>
    </template>
    <template v-else>
      <!-- <Tab :fullPage="true" name="Admin">
        <Admin />
      </Tab> -->
      <Tab name="Logout"><input @click="authStore.logout()" type="submit" /></Tab>
    </template>
  </MyPanel>
</template>

<script>
export default {
  data() {
    return {
      isListLoading: false,
      authStore: useAuthStore(),
      editmenu: useMenuStore(),
      oldInformation: false,
    }
  },
  methods: {
    Logout() {
      this.authStore.logout()
    },
  },
  beforeMount() {
    database.CheckUser()
  },
}
</script>
