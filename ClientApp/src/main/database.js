import axios from 'axios'
import color from '@/main/color'

const apiName = 'api/tables'

const RelatedItemsInTables = { 'Рівень': 'Рівень нс', 'Тип': 'Тип нс' }

const HideVisual = ['Додав']

async function getKeys(tableName) {
    const res = await axios(`${apiName}/getKey/${tableName}`)
    return await res.data
}

async function getTableNameList() {
    const res = await axios(`${apiName}/getTableNameList`)
    return await res.data
}

function setupObject(data, customData) {
    var dataVisual = { ...data }
    console.log(data)
    var dataValue = { ...data }
    Object.keys(data).forEach(name => {
        switch (data[name]) {
            case 'Double':
            case 'Int32':
                dataVisual[name] = setupNumber(name)
                dataValue[name] = 0
                break;
            case 'String':
                if (name == 'Зображення')
                    dataVisual[name] = { element: 'input', type: 'file', disabled: false }
                else if (name == 'Опис')
                    dataVisual[name] = { element: 'textarea', disabled: false }
                else
                    dataVisual[name] = { element: 'input', type: 'text', disabled: false }
                dataValue[name] = ''
                break

            case 'DateTime':
                dataValue[name] = getTime()
                dataVisual[name] = { element: 'input', type: 'datetime-local', disabled: false }
                break
            default:
                dataVisual[name] = { element: dataVisual[name] }
                dataValue[name] = []
                break
        }
        if (HideVisual.includes(name)) {
            delete dataVisual[name]
        }
        if (name in customData) {
            dataValue[name] = customData[name]
        }
    })
    return { dataVisual, dataValue }
}

function setupNumber(name) {
    let result

    if (name in RelatedItemsInTables) {
        result = { element: 'combo', table: RelatedItemsInTables[name] }
    } else {
        result = { element: 'input', type: 'number', disabled: name == 'Код' }
    }

    return result
}

function getLastIdFromTable(tableName) {
    return axios(`${apiName}/getLastId/${tableName}`).then((res) => res.data)
}

function getDataFromTable(tableName) {
    return axios(`${apiName}/${tableName}`).then((res) => res.data)
}


async function getEmergencyData() {
    var result = await axios(`${apiName}/main`).then((eme) => {
        console.log(eme.data)
        return eme.data
    })

    result = await loadPointsList(result)

    return await result
}

async function loadPointsList(emergencyList) {
    let result = await axios(`${apiName}/Позиції НС`)
        .then((points) => {
            emergencyList.forEach((element) => {
                var result = points.data.filter((i) => i['Код нс'] == element.Код)
                element.Points = result.map(p => ({ ...p, X: p.X, Y: p.Y }))
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

export default {
    getKeys, getTableNameList, setupObject, getDataFromTable, getLastIdFromTable, getEmergencyData, getToken
}