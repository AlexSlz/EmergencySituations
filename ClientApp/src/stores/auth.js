import { defineStore } from 'pinia'
import { ref, watch } from 'vue'

export const useAuthStore = defineStore('auth', () => {
    const isAuth = ref(false)
    const userData = ref({})

    const local = localStorage.getItem("user")
    if (local) {
        userData.value = JSON.parse(local)
        isAuth.value = userData.value.token != ''
    }

    function auth(data, token) {
        userData.value = data
        userData.value.token = token
        localStorage.setItem('user', JSON.stringify(userData.value))
        isAuth.value = true
    }

    const logout = () => {
        userData.value = {}
        localStorage.removeItem('user')
        isAuth.value = false
    }

    return { userData, isAuth, logout, auth }
})