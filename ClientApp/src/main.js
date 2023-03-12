import { createApp } from 'vue'
import './assets/style.css'
import 'leaflet/dist/leaflet.css'

import components from './components/UI'

import App from './App.vue'

const app = createApp(App)

components.forEach(component => {
    app.component(component.name, component)
})

app.mount('#app')
