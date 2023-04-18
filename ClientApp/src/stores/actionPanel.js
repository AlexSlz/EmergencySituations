import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useActionPanel = defineStore('actionPanel', () => {
    const show = ref(false)
    const name = ref('Add')
    const tableName = ref('Emergency')
    const fullPage = ref(false)
    const selected = ref({})

    function open(obj) {
        show.value = true
        if (obj == undefined) {
            name.value = 'Add'
            tableName.value = 'Emergency'
            fullPage.value = false
            selected.value = {}
            return
        }
        if ('name' in obj)
            name.value = obj['name']
        if ('tableName' in obj)
            tableName.value = obj['tableName']
        if ('fullPage' in obj)
            fullPage.value = obj['fullPage']
        if ('element' in obj)
            selected.value = obj['element']
    }


    return { show, name, tableName, fullPage, selected, open }
})