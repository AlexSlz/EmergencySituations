import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useActionPanel = defineStore('actionPanel', () => {
    const show = ref(false)
    const name = ref('')
    const tableName = ref('Emergency')
    const fullPage = ref(false)
    const selected = ref({})

    function open(obj) {
        show.value = true
        name.value = 'Add'
        if (obj == undefined) {
            tableName.value = 'Emergency'
            fullPage.value = false
            selected.value = {}
            return
        }
        if ('tableName' in obj)
            tableName.value = obj['tableName']
        if ('fullPage' in obj)
            fullPage.value = obj['fullPage']
        if ('selected' in obj && obj['selected']) {
            selected.value = JSON.parse(JSON.stringify(obj['selected']))
            name.value = 'Edit'
        }
    }


    return { show, name, tableName, fullPage, selected, open }
})