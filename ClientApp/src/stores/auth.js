import { defineStore } from 'pinia'
import { ref } from 'vue'
import { useNotify } from './Notify'

export const useAuthStore = defineStore('auth', () => {
    let Notify = useNotify()
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
        Notify.Open('Ви вийшли з облікового запису.', 'success')
    }



    return { userData, isAuth, logout, auth }
})