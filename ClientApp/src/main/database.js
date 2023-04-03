import axios from 'axios'

const apiName = 'api/tables'

async function getEmergencyData() {
    var data = await axios(`${apiName}/main`).then((eme) => {
        return eme.data
    })

    data = await loadPointsList(data)
    return await data
}

async function loadPointsList(emergencyList) {
    let result = await axios(`${apiName}/Позиції НС`)
        .then((points) => {
            emergencyList.forEach((element) => {
                var result = points.data.filter((i) => i['Код нс'] == element.Код)
                element.Позиції = result.map(p => ({ ...p, X: p.X, Y: p.Y }))
            })
            return emergencyList
        })
    return await result
}

async function getToken(Login, Password) {
    let result = await axios.post('api/auth', { Login, Password }).then(res => {
        return res.data
    }).catch(e => {
        throw new Error(e.response.data || 'Server Error')
    })
    return await result
}

async function getUser(token) {
    let result = await axios.get(`api/auth/${token}`).then(res => {
        return res.data
    }).catch(e => {
        throw new Error(e.response.data || 'Server Error')
    })
    return await result
}

import { useAuthStore } from '@/stores/auth'
async function addToTable(tableName, data) {
    let authStore = useAuthStore()
    if (!authStore.isAuth) return 'NotAuth'
    let result = await axios.post(`api/tables/${tableName}`, data, {
        headers: { 'token': authStore.userData.token }
    }).then(res => {
        return res.data
    })
    return await result
}

async function editTable(tableName, data) {
    let authStore = useAuthStore()
    if (!authStore.isAuth) return 'NotAuth'
    let result = await axios.put(`api/tables/${tableName}`, data, {
        headers: { 'token': authStore.userData.token }
    }).then(res => {
        return res.data
    })
    return await result
}

async function getDataFromTable(tableName) {
    let authStore = useAuthStore()
    return await axios(`${apiName}/${tableName}`, {
        headers: { 'token': authStore.userData.token }
    }).then((res) => res.data)
}

async function getKeys(tableName) {
    let authStore = useAuthStore()
    const res = await axios(`${apiName}/${tableName}/getKey`, {
        headers: { 'token': authStore.userData.token }
    })
    return await res.data
}

async function getTableNameList() {
    let authStore = useAuthStore()
    const res = await axios(`${apiName}/getTableNameList`, {
        headers: { 'token': authStore.userData.token }
    })
    return await res.data
}

async function deleteData(tableName, data) {
    let authStore = useAuthStore()
    if (!authStore.isAuth) return 'NotAuth'
    let result = await axios.delete(`api/tables/${tableName}`, {
        headers: { 'token': authStore.userData.token },
        data: data
    }).then(res => {
        return res.data
    })
    return await result
}

async function loadFile(tableName, file, fileName) {

    let form = new FormData()
    form.append('file', file)
    let result = await axios.post(`api/file/${tableName}/${fileName}`, form, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    }).then(res => {
        editTable(tableName, { Код: fileName, Зображення: res.data })
        return res.data
    })
    return await result
}

export default {
    getKeys, getTableNameList, getDataFromTable,
    getEmergencyData, getToken, addToTable, getUser, editTable, deleteData,
    loadFile
}