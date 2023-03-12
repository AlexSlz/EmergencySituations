<script setup>
import MapView from './components/MapView.vue'
import ListView from './components/ListView/ListView.vue'

import AddMenu from './components/AddMenu.vue'

import axios from 'axios'
</script>

<template>
  <div class="h-screen">
    <MapView class="h-full z-0" ref="Map" :list="emergencyList" />
  </div>
  <my-panel>
    <AddMenu @UpdateItems="UpdateItems" :tableName="'Надзвичайні ситуації'" />
    <ListView ref="List" :elementList="emergencyList" :selectedElement="selectedElement" @ClickOnElement="selectElement" />
  </my-panel>
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
      if (element != null) this.$refs.Map.LookAtElement(element)
      else this.$refs.Map.zoomOut()
    },
  },
  beforeMount() {
    axios('api/tables/Надзвичайні ситуації').then((eme) => {
      this.emergencyList = eme.data
      axios('api/tables/Позиція НС')
        .then((points) => {
          this.emergencyList.forEach((element) => {
            var result = points.data.filter((i) => i['Код нс'] == element.Код)
            element.Points = result
          })
        })
        .then((e) => {
          this.$refs.Map.displayMarkers()
        })
    })
    // axios('api/tables/Позиція НС')
    //   .then((res) => {
    //     this.emergencyList = res.data
    //   })
    //   .then(() => {
    //     this.$refs.Map.displayMarkers()
    //   })
  },
}
</script>
