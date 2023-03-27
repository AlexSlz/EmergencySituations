import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useMenuStore = defineStore('editMenu', () => {
    const show = ref(false)
    const name = ref('Add')

    function open(tabName) {
        show.value = true
        name.value = tabName
    }

    return { show, name, open }
})