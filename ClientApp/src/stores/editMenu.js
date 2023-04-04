import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useMenuStore = defineStore('editMenu', () => {
    const show = ref(false)
    const name = ref('Add')
    const fullPage = ref(false)
    const table = ref('Надзвичайні ситуації')
    const selectedElement = ref(null)

    function open(obj) {
        show.value = true
        if (obj == undefined) {
            name.value = 'Add'
            fullPage.value = false
            table.value = 'Надзвичайні ситуації'
            selectedElement.value = null
            return
        }
        if ('name' in obj)
            name.value = obj['name']
        if ('table' in obj)
            table.value = obj['table']
        if ('fullPage' in obj)
            fullPage.value = obj['fullPage']
        if ('element' in obj)
            selectedElement.value = obj['element']
    }


    return { show, name, fullPage, table, selectedElement, open }
})