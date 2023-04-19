<template>
  <div id="map"></div>
</template>

<script>
import { useEmergencyStore } from '@/stores/emergency'
import { useActionPanel } from '@/stores/actionPanel'
import color from '@/main/color'
import L from 'leaflet'
import geo from '@/main/geo'

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
      emergency: useEmergencyStore(),
      actionPanel: useActionPanel(),
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
      if (element == null || element.positions === undefined || element.positions.length == 0) return

      var Points = []
      this.markerList.forEach((m) => {
        element.positions.forEach((i) => {
          if (m._latlng.lat == i.x && m._latlng.lng == i.y) {
            L.DomUtil.addClass(m._icon, 'gps_ring')
            Points.push([i.x, i.y])
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
      this.deleteMarkers(this.markerList)
      elements.forEach((element) => {
        element.positions.forEach((i) => {
          var icon = L.divIcon({
            html: `<span style="background-color: ${
              color[this.emergency.colorBy][element[this.emergency.colorBy]]
            }" class="circle"></span>`,
            className: '',
            iconSize: [30, 30],
          })
          var marker = L.marker([i.x, i.y], {
            icon: icon,
          })
            .addTo(this.map)
            .on('click', (e) => {
              this.emergency.select(element)
            })
          this.markerList.push(marker)
        })
      })
    },
    displayNewMarkers(items) {
      this.deleteMarkers(this.tempList)
      items.forEach((element) => {
        var pos = this.map.getCenter()
        if (element.x == -1 && element.y == -1) {
          element.x = pos.lat
          element.y = pos.lng
        }
        var temp = L.marker([element.x, element.y], { draggable: true, autoPan: true })
          .addTo(this.map)
          .on('dragend', function (e) {
            var position = e.target.getLatLng()
            element.x = position.lat
            element.y = position.lng
            geo(position.lat, position.lng).then((d) => {
              element.location = d
            })
          })
        this.tempList.push(temp)
      })
    },

    deleteMarkers(list) {
      list.forEach((item) => {
        if (this.map.hasLayer(item)) {
          this.map.removeLayer(item)
        }
      })
      list.splice(0, list.length)
    },
  },
  watch: {
    'emergency.list': {
      immediate: true,
      deep: true,
      handler(val) {
        setTimeout(() => {
          this.displayMarkers(val)
        })
      },
    },
    'emergency.selected': {
      deep: true,
      handler(val) {
        if (val == null) this.zoomOut()
        this.LookAtElement(val)
      },
    },
    'emergency.colorBy': {
      deep: true,
      handler() {
        this.displayMarkers(this.emergency.list)
      },
    },
    'actionPanel.selected.positions.length': {
      deep: true,
      handler() {
        if (this.actionPanel.selected.positions != undefined) this.displayNewMarkers(this.actionPanel.selected.positions)
        else this.deleteMarkers(this.tempList)
      },
    },
  },
}
</script>
