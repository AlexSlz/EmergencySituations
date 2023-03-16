<script setup>
import MapView from './components/MapView.vue'
import EventList from './components/EventList/EventList.vue'

import MyPanel from './components/Panel/PanelBody.vue'
import Tab from './components/Panel/PanelTab.vue'
import AddMenu from './components/AddMenu.vue'

import database from './main/database'
import Login from './components/Login.vue'
</script>

<template>
  <div class="h-screen">
    <MapView class="h-full z-0" ref="Map" @ClickOnElement="selectElement" :list="emergencyList" />
  </div>
  <MyPanel>
    <Tab name="Події" :selected="true"
      ><EventList ref="List" :elementList="emergencyList" :selectedElement="selectedElement" @ClickOnElement="selectElement"
    /></Tab>
    <Tab name="Login"><Login /></Tab>
    <Tab :fullPage="true" name="Admin"></Tab>
  </MyPanel>
</template>

<script>
export default {
  data() {
    return {
      selectedElement: null,
      emergencyList: [],
    }
  },
  methods: {
    UpdateItems(items) {
      this.$refs.Map.displayNewMarkers(items)
    },
    selectElement(element) {
      if (element === undefined) return
      this.selectedElement = element
      this.$refs.Map.LookAtElement(element)
      if (element == null) this.$refs.Map.zoomOut()
    },
  },
  beforeMount() {
    database
      .getEmergencyData()
      .then((e) => {
        this.emergencyList = e
      })
      .catch((e) => {
        console.log(e)
      })
  },
  components: { Login },
}
</script>
