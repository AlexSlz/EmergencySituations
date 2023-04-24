import axios from 'axios'
import { useAuthStore } from '@/stores/auth'

async function GetData(tableName) {
    var data = await axios(`api/${tableName}`).then((res) => {
        return res.data
    }).catch((err) => {
        throw new Error(err.response.data || 'Server Error')
    })
    return await data
}

async function AddToTable(tableName, data) {
    let authStore = useAuthStore()
    if (!authStore.isAuth) return 'NotAuth'
    let result = await axios.post(`api/${tableName}`, data, {
        headers: { 'token': authStore.userData.token }
    }).then(res => {
        return res.data
    }).catch((err) => {
        throw new Error(err.response.data || 'Server Error')
    })
    return await result
}

async function EditTable(tableName, data) {
    let authStore = useAuthStore()
    if (!authStore.isAuth) return 'NotAuth'
    let result = await axios.put(`api/${tableName}`, data, {
        headers: { 'token': authStore.userData.token }
    }).then(res => {
        return res.data
    }).catch((err) => {
        throw new Error(err.response.data || 'Server Error')
    })
    return await result
}


async function DeleteData(tableName, data) {
    let authStore = useAuthStore()
    if (!authStore.isAuth) return 'NotAuth'
    let result = await axios.delete(`api/${tableName}`, {
        headers: { 'token': authStore.userData.token },
        data: data
    }).then(res => {
        return res.data
    }).catch((err) => {
        throw new Error(err.response.data || 'Server Error')
    })
    return await result
}

async function GetToken(Login, Password) {
    let result = await axios.post('api/auth', { Login, Password }).then(res => {
        return res.data
    }).catch(e => {
        throw new Error(e.response.data || 'Server Error')
    })
    return await result
}

async function GetUserByToken(token) {
    let result = await axios.get(`api/auth/${token}`).then(res => {
        return res.data
    }).catch(e => {
        throw new Error(e.response.data || 'Server Error')
    })
    return await result
}

async function CheckUser() {
    const auth = useAuthStore()
    if (auth.isAuth)
        axios(`api/auth/${auth.userData.token}/check`)
            .then((res) => {
                if (!res.data) auth.logout()
            })
            .catch((e) => {
                auth.logout()
            })
}

async function UploadFile(tableName, file, id) {
    let authStore = useAuthStore()
    if (!authStore.isAuth) return 'NotAuth'
    let form = new FormData()
    form.append('file', file)
    let result = await axios.post(`api/file/${tableName}/${id}`, form, {
        headers: {
            'token': authStore.userData.token,
            'Content-Type': 'multipart/form-data'
        }
    }).then(res => {
        return res.data
    }).catch(e => {
        throw new Error(e.response.data || 'Server Error')
    })
    return await result
}

async function DeleteFiles(tableName) {
    let authStore = useAuthStore()
    if (!authStore.isAuth) return 'NotAuth'
    await axios(`api/file/${tableName}`, {
        headers: {
            'token': authStore.userData.token,
        }
    }).catch((err) => {
        console.log(err)
    })
}

async function GetStatistic(type) {
    return await axios(`api/statistic/${type}`).then((res) => {
        return res.data
    }).catch((err) => {
        throw new Error(err.response.data || 'Server Error')
    })
}


export default {
    GetData, AddToTable, EditTable, DeleteData, GetToken, GetUserByToken, CheckUser, UploadFile, DeleteFiles,
    GetStatistic
}


// async function getKeys(tableName) {
//     let authStore = useAuthStore()
//     const res = await axios(`${apiName}/${tableName}/getKey`, {
//         headers: { 'token': authStore.userData.token }
//     })
//     return await res.data
// }

// async function getTableNameList() {
//     let authStore = useAuthStore()
//     const res = await axios(`${apiName}/getTableNameList`, {
//         headers: { 'token': authStore.userData.token }
//     })
//     return await res.data
// }

