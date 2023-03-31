import { defineStore } from 'pinia'
import { ref, watch } from 'vue'
import database from '@/main/database'

export const useEmergencyStore = defineStore('emergency', () => {
    const emergencyList = ref([])
    const selectedElement = ref(null)
    const needUpdate = ref({ lastUpdate: new Date(), extra: false })
    const colorBy = ref('Тип')
    const tempPoints = ref([])

    const localEmergency = localStorage.getItem("emergency")
    const localUpdate = localStorage.getItem("updateData")

    if (localEmergency) {
        try {
            emergencyList.value = JSON.parse(localEmergency)
        } catch { }
    }

    if (localUpdate) {
        try {
            needUpdate.value = JSON.parse(localUpdate)
        } catch { }
    }

    watch(() => emergencyList, (state) => {
        localStorage.setItem('emergency', JSON.stringify(state.value))
    }, { deep: true })

    watch(() => needUpdate, (state) => {
        localStorage.setItem('updateData', JSON.stringify(state.value))
    }, { deep: true })

    function selectElement(element) {
        if (element === undefined) return
        selectedElement.value = element
    }

    async function removeSelected() {
        let result = await database.deleteData('Надзвичайні ситуації', selectedElement.value).then((res) => {
            let index = emergencyList.value.indexOf(selectedElement.value)
            if (index > -1)
                emergencyList.value.splice(index, 1)
            selectElement(null)
            return 'Successfully.'
        })
        return await result
    }

    return { emergencyList, needUpdate, selectedElement, selectElement, colorBy, tempPoints, removeSelected }
})