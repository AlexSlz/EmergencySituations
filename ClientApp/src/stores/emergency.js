import { defineStore } from 'pinia'
import { ref, watch } from 'vue'

export const useEmergencyStore = defineStore('emergency', () => {
    const emergencyList = ref([])
    const selectedElement = ref(null)
    const colorBy = ref('Тип')

    const local = localStorage.getItem("emergency")
    if (local) {
        try {
            emergencyList.value = JSON.parse(local)
        } catch { }
    }

    watch(() => emergencyList, (state) => {
        localStorage.setItem('emergency', JSON.stringify(state.value))
    }, { deep: true })

    function selectElement(element) {
        if (element === undefined) return
        selectedElement.value = element
    }

    return { emergencyList, selectedElement, selectElement, colorBy }
})