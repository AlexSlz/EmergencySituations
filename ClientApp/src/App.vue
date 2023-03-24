<script setup>
import MapView from '@/components/MapView.vue'
import EventList from '@/components/EventList/EventList.vue'

import MyPanel from '@/components/Panel/PanelBody.vue'
import Tab from '@/components/Panel/PanelTab.vue'
import Login from '@/components/Login.vue'

import database from '@/main/database'
</script>

<template>
  <MapView class="h-screen z-0 w-full sm:w-[79vw]" ref="Map" />
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
      <Tab :fullPage="true" name="Admin"></Tab>
      <Tab name="Logout"><button @click="authStore.logout()">Confirm</button></Tab>
    </template>
  </MyPanel>
</template>

<script>
import { useAuthStore } from '@/stores/auth'
import { useEmergencyStore } from './stores/emergency'
import Notation from './components/Notation.vue'
export default {
  data() {
    return {
      isListLoading: true,
      authStore: useAuthStore(),
      emergencyStore: useEmergencyStore(),
    }
  },
  methods: {
    Logout() {
      console.log('a')
      this.authStore.logout()
    },
    loadEmergency() {
      database
        .getEmergencyData()
        .then((e) => {
          this.emergencyStore.emergencyList = e
        })
        .catch((e) => {
          console.log(e)
        })
        .finally((e) => {
          this.isListLoading = false
        })
    },
  },
  beforeMount() {
    if (this.emergencyStore.emergencyList != []) {
      this.loadEmergency()
    }
  },
  components: { Notation },
}
</script>
