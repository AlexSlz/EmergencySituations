import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useNavPanel = defineStore('NavPanel', () => {
    const Select = ref({})
    return { Select }
})