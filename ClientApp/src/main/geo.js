import axios from "axios"

let active = false

export default async (x, y) => {
    if (!active) {
        active = true
        return await axios(`https://nominatim.openstreetmap.org/reverse?format=json&lat=${x}&lon=${y}&addressdetails=1`).then((e) => {
            if ('town' in e.data.address)
                return e.data.address.town
            if ('city' in e.data.address)
                return e.data.address.city
            if ('state' in e.data.address)
                return e.data.address.state
            return e.data.address.country
        }).finally((e) => {
            active = false
        })
    }
}