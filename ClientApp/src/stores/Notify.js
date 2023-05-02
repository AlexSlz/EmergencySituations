import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useNotify = defineStore('Notify', () => {
    const Open = ref({})
    return { Open }
})