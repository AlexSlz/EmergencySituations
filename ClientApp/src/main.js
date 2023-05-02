import { createPinia } from 'pinia'
import { createApp } from 'vue'

import '@/assets/style.css'
import 'leaflet/dist/leaflet.css'
import 'https://kit.fontawesome.com/84af5c4d23.js'

import App from '@/App.vue'

import components from '@/components/UI'

const app = createApp(App).use(createPinia())
components.forEach(component => {
    app.component(component.name, component)
})

app.mount('#app')
