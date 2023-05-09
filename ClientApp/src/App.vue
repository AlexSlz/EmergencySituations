<script setup>
import MapView from '@/components/MapView.vue'
import EventList from '@/components/EventList/EventList.vue'
import Notation from './components/Notation.vue'
import MyPanel from '@/components/Panel/PanelBody.vue'
import Tab from '@/components/Panel/PanelTab.vue'
import Login from '@/components/Login.vue'
import ActionPanel from '@/components/ActionPanels/ActionPanel.vue'
import Statistic from '@/components/Statistic.vue'
import Admin from './components/AdminMenu/Admin.vue'
import Notification from './components/Notification.vue'

import database from '@/main/database'
import { useEmergencyStore } from '@/stores/emergency'
import { useAuthStore } from '@/stores/auth'
import { useActionPanel } from '@/stores/actionPanel'
</script>

<template>
  <Notification />
  <MapView class="h-screen z-0 w-full sm:w-[70vw]" ref="Map" />
  <MyPanel>
    <Tab @onTabClick="emergency.select(null)" name="Події" :selected="true">
      <EventList ref="list" :isAuth="authStore.isAuth" />
    </Tab>
    <Tab fullPage="true" name="Статистика"><Statistic /></Tab>
    <Tab name="Позначення"><Notation /></Tab>
    <template v-if="!authStore.isAuth">
      <Tab v-if="!authStore.isAuth" name="Авторизація"><Login /></Tab>
    </template>
    <template v-else>
      <Tab name="Таблиці" fullPage="true"><Admin /></Tab>
      <Tab name="Вихід"><input @click="authStore.logout()" type="submit" value="Підтвердити" /></Tab>
      <Tab v-if="actionPanel.show" :name="actionPanel.name" openByDefault="true">
        <ActionPanel />
      </Tab>
    </template>
  </MyPanel>
</template>

<script>
export default {
  data() {
    return {
      emergency: useEmergencyStore(),
      authStore: useAuthStore(),
      actionPanel: useActionPanel(),
    }
  },
  beforeMount() {
    database.CheckUser()
  },
}
</script>
