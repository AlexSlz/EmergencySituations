<script setup>
import MapView from '@/components/MapView.vue'
import EventList from '@/components/EventList/EventList.vue'

import MyPanel from '@/components/Panel/PanelBody.vue'
import Tab from '@/components/Panel/PanelTab.vue'
import Login from '@/components/Login.vue'
import ManipulationMenu from './components/ManipulationMenu.vue'

import database from '@/main/database'
</script>

<template>
  <MapView class="h-screen z-0 w-full sm:w-[70vw]" ref="Map" />
  <MyPanel>
    <Tab name="Події" @ifActive="checkUpdateImportance" :selected="true">
      <h1 class="text-center pt-3" v-if="isListLoading">Завантаження списку подій...</h1>
      <EventList ref="List" v-if="!isListLoading" />
    </Tab>
    <Tab name="Позначення"><Notation /></Tab>
    <template v-if="!authStore.isAuth">
      <Tab v-if="!authStore.isAuth" name="Login"><Login /></Tab>
    </template>
    <template v-else>
      <Tab
        :openByDefault="true"
        v-if="editmenu.show"
        @ifMount="this.$refs.EditMenu.loadForm()"
        @ifUnMount="unLoadForm"
        :name="editmenu.name"
        :fullPage="editmenu.fullPage"
      >
        <ManipulationMenu :selectedElement="editmenu.selectedElement" ref="EditMenu" :tableName="editmenu.table" />
      </Tab>
      <Tab :fullPage="true" name="Admin">
        <Admin />
      </Tab>
      <Tab name="Logout"><button @click="authStore.logout()">Confirm</button></Tab>
    </template>
  </MyPanel>
</template>

<script>
import { useAuthStore } from '@/stores/auth'
import { useEmergencyStore } from './stores/emergency'
import { useMenuStore } from './stores/editMenu'
import Notation from './components/Notation.vue'
import axios from 'axios'
import Admin from './components/AdminMenu/Admin.vue'
export default {
  data() {
    return {
      isListLoading: false,
      authStore: useAuthStore(),
      emergencyStore: useEmergencyStore(),
      editmenu: useMenuStore(),
      oldInformation: false,
    }
  },
  methods: {
    unLoadForm() {
      this.emergencyStore.tempPoints = []
      this.loadEmergency()
      //this.emergencyStore.selectElement(null)
    },
    Logout() {
      this.authStore.logout()
    },
    checkUpdateImportance() {
      if (new Date(this.emergencyStore.needUpdate.lastUpdate) >= new Date() - 10 || this.emergencyStore.needUpdate.extra) {
        this.loadEmergency()
        this.emergencyStore.needUpdate.extra = false
      }
    },
    loadEmergency() {
      this.isListLoading = true
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

          let next = new Date()
          next.setMinutes(next.getMinutes() + 1)
          this.emergencyStore.needUpdate.lastUpdate = next
        })
    },
  },
  beforeMount() {
    //this.loadEmergency()
    if (this.authStore.isAuth)
      axios
        .get(`api/auth/${this.authStore.userData.token}/check`)
        .then((res) => {
          if (!res.data) this.Logout()
        })
        .catch((e) => {
          this.Logout()
        })
  },
  components: { Notation, Admin },
}
</script>
