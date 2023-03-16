<template>
  <div id="map"></div>
</template>

<script>
import L from 'leaflet'

export default {
  props: {
    list: {
      type: Array,
      required: true,
    },
    newList: {
      type: Array,
    },
  },

  data() {
    return {
      map: null,
      markerList: [],
      tempList: [],
    }
  },
  mounted() {
    this.map = L.map('map', {
      zoomControl: false,
      fadeAnimation: true,
      markerZoomAnimation: false,
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
    L.control
      .attribution({
        position: 'bottomleft',
      })
      .addTo(this.map)
  },
  methods: {
    LookAtElement(element) {
      // const animatedCircleIcon = {
      //   icon: L.divIcon({
      //     className: 'css-icon',
      //     html: '<div class="gps_ring"></div>',
      //     iconSize: [18, 22],
      //   }),
      // }

      var Points = []
      this.markerList.forEach((m) => {
        m.setStyle({ color: 'red' })
        if (element != null)
          element.Points.forEach((i) => {
            if (m._latlng.lat == i.X && m._latlng.lng == i.Y) {
              m.setStyle({ color: 'yellow' }).bringToFront()
              Points.push([i.X, i.Y])
            }
          })
      })

      if (element == null) return

      // animate: true, duration: 1.5
      this.map.fitBounds(new L.LatLngBounds(Points), { maxZoom: 10 })
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
          var marker = L.circleMarker([i.X, i.Y], { color: 'red', radius: 13, fillOpacity: 0.9 })
            .addTo(this.map)
            .on('click', (e) => {
              this.$emit('ClickOnElement', element)
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
    list: {
      immediate: true,
      handler(val, oldVal) {
        this.displayMarkers(val)
      },
    },
  },
  emits: ['ClickOnElement'],
}
</script>
