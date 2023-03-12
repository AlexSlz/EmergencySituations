import axios from 'axios'

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
    var dataValue = { ...data }
    Object.keys(data).forEach(name => {
        switch (data[name]) {
            case 'Int32':
                dataVisual[name] = setupNumber(name)
                dataValue[name] = 0
                break;
            case 'String':
                dataVisual[name] = { element: 'input', type: 'text', disabled: false }
                dataValue[name] = '123'
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

function getTime() {
    var date = new Date().toJSON().slice(0, 10);

    var result = `${date}T${new Date().getHours()}:${new Date().getMinutes()}`

    return result
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

export default {
    getKeys, getTableNameList, setupObject, getDataFromTable, getLastIdFromTable
}