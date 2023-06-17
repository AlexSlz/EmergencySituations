import axios from 'axios'
import { useAuthStore } from '@/stores/auth'
import { useNotify } from '@/stores/Notify'


let limit = 35

async function GetData(tableName, page = 1, order = 'id') {
    let authStore = useAuthStore()
    var data = await axios(`api/${tableName}`, {
        params: { limit, page, order },
        headers: { 'token': authStore.userData.token }
    }).then((res) => {
        return { data: res.data, maxPage: res.headers.maxpage, totalCount: res.headers.totaldata }
    }).catch((err) => {
        throw new Error(err.response.data || 'Server Error')
    })
    return await data
}

async function Search(tableName, filter, page = 1, order = 'id') {
    let authStore = useAuthStore()
    var data = await axios.post(`api/${tableName}/search`, filter, {
        params: { limit, page, order },
        headers: { 'token': authStore.userData.token }
    }).then((res) => {
        return { data: res.data, maxPage: res.headers.maxpage, totalCount: res.headers.totaldata }
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
        console.log(err)
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
    if (!confirm(`Ви точно хочете видалити дані з таблиці ${tableName}?`)) {
        console.log('a')
        return null
    }
    if (!authStore.isAuth) return 'NotAuth'
    let result = await axios.delete(`api/${tableName}/${data.id}`, {
        headers: { 'token': authStore.userData.token },
    }).then(res => {
        useNotify().Open('Запис видалено.', 'success')
        return res.data
    }).catch((err) => {
        console.log(err)
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

async function UploadBackUp(file) {
    let authStore = useAuthStore()
    if (!authStore.isAuth) return 'NotAuth'
    let form = new FormData()
    form.append('file', file)
    let result = await axios.post(`api/file/backup`, form, {
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

async function GetStatistic(year) {
    var api = 'api/statistic/'
    if (year > 0)
        api += `?year=${year}`
    return await axios(api).then((res) => {
        return res.data
    }).catch((err) => {
        throw new Error(err.response.data || 'Server Error')
    })
}

async function GetYears(year = 0) {
    let params = {}
    if (year > 0) {
        params = { year }
    }
    return await axios('api/statistic/year', { params }).then((res) => {
        return res.data
    }).catch((err) => {
        throw new Error(err.response.data || 'Server Error')
    })
}

async function GetTableNameList() {
    let authStore = useAuthStore()
    const res = await axios(`api/tables`, {
        headers: { 'token': authStore.userData.token }
    })
    return await res.data
}

async function CreateReport(year, month = 0) {
    let authStore = useAuthStore()
    return await axios({
        url: `api/file/report?year=${year}&month=${month}`,
        method: 'GET',
        responseType: 'blob', // important
        headers: {
            'token': authStore.userData.token,
        }
    }).then((res) => {
        const href = URL.createObjectURL(res.data);
        const link = document.createElement('a');
        let fileName = res.headers['content-disposition'].split("UTF-8''")[1]
        fileName = decodeURIComponent(fileName)
        link.href = href;
        link.setAttribute('download', fileName);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(href);
        return 'ok'
    }).catch((err) => {
        console.log(err)
        throw new Error(err.response.statusText || 'Server Error')
    })
}

async function GetKeys(tableName) {
    let authStore = useAuthStore()
    const res = await axios(`api/${tableName}/getKeys`, {
        headers: { 'token': authStore.userData.token }
    })
    return await res.data
}

async function GetBackUpList() {
    let authStore = useAuthStore()
    return await axios(`api/file/backup/list`, {
        headers: { 'token': authStore.userData.token }
    }).then((res) => {
        return res.data
    }).catch((err) => {
        throw new Error(err.response.data || 'Server Error')
    })
}

async function CreateBackup() {
    let authStore = useAuthStore()
    return await axios(`api/file/backup/create`, {
        headers: { 'token': authStore.userData.token }
    }).then((res) => {
        return res.data
    }).catch((err) => {
        throw new Error(err.response.data || 'Server Error')
    })
}

async function LoadFileBackup(name, delet) {
    var del = (delet) ? '/delete' : ''
    var msg = `Ви хочете завантажити файл ${name}? Усі незбережені дані будуть видалені.`
    if (delet)
        msg = `Ви точно хочете видалити файл ${name}?`
    if (!confirm(msg)) {
        return
    }
    let authStore = useAuthStore()
    return await axios(`api/file/backup/${name}${del}`, {
        headers: { 'token': authStore.userData.token }
    }).then((res) => {
        return res.data
    }).catch((err) => {
        throw new Error(err.response.data || 'Server Error')
    })
}


export default {
    GetData, AddToTable, EditTable, DeleteData, GetToken, GetUserByToken, CheckUser, UploadFile, DeleteFiles, UploadBackUp,
    GetStatistic, GetYears, GetTableNameList, CreateReport, Search, GetKeys, GetBackUpList, CreateBackup, LoadFileBackup
}
