function GetLocation(element) {
    var a = [...new Set(element.positions.map((i) => i.location))]
    var text = 'Інформації немає'
    if (a.length > 0) text = a.join(', ')
    return text
}
function GetImage(id) {
    if (id != '') return `${window.location.origin}/api/file/Emergency/${id}`
}
function GetTime(time = '') {
    var result = ''
    if (time == '') {
        return new Date().toLocaleDateString('fr-CA') + ' ' + new Date().toLocaleTimeString('ua')
    } else {
        result = time.replace('T', ' ')
    }
    var temp = result.split(' ', 2)
    if (temp[1] === '00:00:00') return temp[0]
    return result
}


export default {
    GetLocation, GetImage, GetTime
}