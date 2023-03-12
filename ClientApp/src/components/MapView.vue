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
  },
  methods: {
    LookAtElement(element) {
      const animatedCircleIcon = {
        icon: L.divIcon({
          className: 'css-icon',
          html: '<div class="gps_ring"></div>',
          iconSize: [18, 22],
        }),
      }
      var temp = []
      element.Points.forEach((i) => {
        temp.push(L.marker([i.X, i.Y], animatedCircleIcon).addTo(this.map))
      })
      setTimeout(() => {
        temp.forEach((i) => {
          this.map.removeLayer(i)
        })
      }, 4700)
      var pos = this.getElementsCenter(element.Points)
      console.log(pos.zoom)
      this.map.flyTo([pos.X, pos.Y], pos.zoom, {
        animate: true,
        duration: 1.5,
      })
    },

    getElementsCenter(element) {
      var count = 0
      var xSum = 0
      var ySum = 0
      element.forEach((i) => {
        xSum += i.X
        ySum += i.Y
        count++
      })
      xSum /= count
      ySum /= count
      return { X: xSum, Y: ySum, zoom: 9 }
    },

    zoomOut() {
      if (this.map.getZoom() > 7)
        this.map.setZoom(7, {
          animate: true,
          duration: 1.5,
        })
    },

    displayMarkers() {
      this.list.forEach((element) => {
        element.Points.forEach((i) => {
          L.marker([i.X, i.Y]).addTo(this.map)
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
}
</script>
