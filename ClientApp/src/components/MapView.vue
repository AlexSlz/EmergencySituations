<template>
  <div id="map"></div>
</template>

<script>
import { useEmergencyStore } from '@/stores/emergency'
import color from '@/main/color'
import L from 'leaflet'

export default {
  props: {
    newList: {
      type: Array,
    },
  },

  data() {
    return {
      map: null,
      markerList: [],
      tempList: [],
      emergencyStore: useEmergencyStore(),
    }
  },
  mounted() {
    this.map = L.map('map', {
      zoomControl: false,
      fadeAnimation: true,
      //      markerZoomAnimation: false,
    }).setView([50.45, 30.53], 10)

    L.tileLayer(`https://api.maptiler.com/maps/streets-v2/{z}/{x}/{y}.png?key=${import.meta.env.VITE_MAP_API_KEY}`, {
      tileSize: 512,
      zoomOffset: -1,
      minZoom: 3,
      attribution:
        '\u003ca href="https://www.maptiler.com/copyright/" target="_blank"\u003e\u0026copy; MapTiler\u003c/a\u003e \u003ca href="https://www.openstreetmap.org/copyright" target="_blank"\u003e\u0026copy; OpenStreetMap contributors\u003c/a\u003e',
      crossOrigin: true,
      noWrap: true,
      continuousWorld: true,
      bounds: [
        [-90, -180],
        [90, 180],
      ],
    }).addTo(this.map)
  },
  methods: {
    LookAtElement(element) {
      this.markerList.forEach((m) => {
        L.DomUtil.removeClass(m._icon, 'gps_ring')
      })
      if (element == null || element.Points === undefined || element.Points.length == 0) return

      var Points = []
      this.markerList.forEach((m) => {
        element.Points.forEach((i) => {
          if (m._latlng.lat == i.X && m._latlng.lng == i.Y) {
            L.DomUtil.addClass(m._icon, 'gps_ring')
            Points.push([i.X, i.Y])
          }
        })
      })

      this.map.flyToBounds(new L.LatLngBounds(Points), { maxZoom: this.map.getZoom() })
    },

    zoomOut() {
      if (this.map.getZoom() > 7)
        this.map.setZoom(7, {
          animate: true,
          duration: 1.5,
        })
    },

    displayMarkers(elements) {
      elements.forEach((element) => {
        element.Points.forEach((i) => {
          // var marker = L.circleMarker([i.X, i.Y], {
          //   color: color[this.emergencyStore.colorBy][element[this.emergencyStore.colorBy]],
          //   radius: 13,
          //   fillOpacity: 0.9,
          // })
          var icon = L.divIcon({
            html: `<span style="background-color: ${
              color[this.emergencyStore.colorBy][element[this.emergencyStore.colorBy]]
            }" class="circle"></span>`,
            className: '',
            iconSize: [30, 30],
          })
          var marker = L.marker([i.X, i.Y], {
            icon: icon,
          })
            .addTo(this.map)
            .on('click', (e) => {
              this.emergencyStore.selectElement(element)
            })
          this.markerList.push(marker)
        })
      })
    },
    displayNewMarkers(items) {
      this.deleteMarkers()
      items.forEach((element) => {
        var pos = this.map.getCenter()
        if (element.X == -1 && element.Y == -1) {
          element.X = pos.lat
          element.Y = pos.lng
        }
        var temp = L.marker([element.X, element.Y], { draggable: true, autoPan: true })
          .addTo(this.map)
          .on('drag', function (e) {
            var position = e.target.getLatLng()
            element.X = position.lat
            element.Y = position.lng
          })
        this.tempList.push(temp)
      })
    },

    deleteMarkers() {
      this.tempList.forEach((item) => {
        if (this.map.hasLayer(item)) {
          this.map.removeLayer(item)
        }
      })
      this.tempList = []
    },
  },
  watch: {
    'emergencyStore.emergencyList': {
      immediate: true,
      deep: true,
      handler(val, oldVal) {
        console.log(val)
        setTimeout(() => {
          this.displayMarkers(val)
        })
      },
    },
    'emergencyStore.selectedElement': {
      deep: true,
      handler(val, oldVal) {
        if (val == null) this.zoomOut()
        this.LookAtElement(val)
      },
    },
    'emergencyStore.colorBy': {
      deep: true,
      handler(val, oldVal) {
        this.deleteMarkers()
        this.displayMarkers(this.emergencyStore.emergencyList)
      },
    },
  },
}
</script>
