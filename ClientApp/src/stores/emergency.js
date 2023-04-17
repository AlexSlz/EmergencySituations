import { defineStore } from 'pinia'
import { ref, watch } from 'vue'
import database from '@/main/database'

export const useEmergencyStore = defineStore('emergency', () => {
    const list = ref([])
    const selected = ref(null)
    const colorBy = ref('type')
    const tempPoints = ref([])

    function select(element) {
        if (element === undefined) return
        selected.value = element
    }

    // async function removeSelected() {
    //     let result = await database.deleteData('Надзвичайні ситуації', selectedElement.value).then((res) => {
    //         let index = emergencyList.value.indexOf(selectedElement.value)
    //         if (index > -1)
    //             emergencyList.value.splice(index, 1)
    //         selectElement(null)
    //         return 'Successfully.'
    //     })
    //     return await result
    // }

    return { list, selected, select, colorBy, tempPoints }
})