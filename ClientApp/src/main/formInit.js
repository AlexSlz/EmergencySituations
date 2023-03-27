const RelatedItemsInTables = { 'Рівень': 'Рівень нс', 'Тип': 'Тип нс' }

const HideVisual = ['Код', 'Додав']
const disabledVisual = []
const customElements = {
    'Зображення': { element: 'input', type: 'file', disabled: false },
    'Опис': { element: 'textarea', disabled: false }
}

function setupObject(data, customValue) {

    var dataVisual = { ...data }
    var dataValue = { ...data }

    Object.keys(data).forEach(name => {
        switch (data[name]) {
            case 'Double':
            case 'Int32':
            case 'Int64':
                dataVisual[name] = setupNumber(name)
                dataValue[name] = 1
                break;
            case 'String':
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
        if (disabledVisual.includes(name)) {
            dataVisual[name].disabled = true
        }
        if (name in customElements) {
            dataVisual[name] = customElements[name]
        }
        if (name in customValue) {
            dataValue[name] = customValue[name]
        }
    })
    return { dataVisual, dataValue }
}

function setupNumber(name) {
    let result

    if (name in RelatedItemsInTables) {
        result = { element: 'combo', table: RelatedItemsInTables[name] }
    } else {
        result = { element: 'input', type: 'number', disabled: false }
    }

    return result
}

function getTime() {
    var date = new Date().toJSON().slice(0, 10);

    var hours = new Date().getHours()
    if (hours < 10)
        hours = `0${hours}`
    var minutes = new Date().getMinutes()
    if (minutes < 10)
        minutes = `0${minutes}`
    var result = `${date} ${hours}:${minutes}`
    return result
}



export default {
    setupObject
}