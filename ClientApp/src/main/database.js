import axios from 'axios'

const apiName = 'api/tables'

async function getKeys(tableName) {
    const res = await axios(`${apiName}/${tableName}/getKey`)
    return await res.data
}

async function getTableNameList() {
    const res = await axios(`${apiName}/getTableNameList`)
    return await res.data
}

function getLastIdFromTable(tableName) {
    return axios(`${apiName}/${tableName}/getLastId`).then((res) => res.data)
}

function getDataFromTable(tableName) {
    return axios(`${apiName}/${tableName}`).then((res) => res.data)
}


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

export default {
    getKeys, getTableNameList, getDataFromTable, getLastIdFromTable,
    getEmergencyData, getToken, addToTable, getUser, editTable, deleteData
}