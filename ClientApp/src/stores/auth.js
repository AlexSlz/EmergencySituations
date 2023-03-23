import { defineStore } from 'pinia'
import { ref, watch } from 'vue'

export const useAuthStore = defineStore('auth', () => {
    const token = ref('')
    const isAuth = ref(false)

    const local = localStorage.getItem("token")
    if (local) {
        token.value = local
        isAuth.value = token.value != ''
    }

    watch(() => token, (state) => {
        localStorage.setItem('token', state.value)
        isAuth.value = token.value != ''
    }, { deep: true })

    const logout = () => {
        token.value = ''
    }



    return { token, isAuth, logout }
})