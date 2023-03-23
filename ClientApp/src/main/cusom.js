function getTime(time) {
    var date = new Date(time).toJSON().slice(0, 10);

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
    getTime
}