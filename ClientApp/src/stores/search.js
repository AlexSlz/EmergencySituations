import { ref } from 'vue'

export const useSearch = (startOrder = 'DateAndTime') => {
    const filter = ref({})
    const temp = ref({ level: { id: 0 }, type: { id: 0 } })
    const maxPage = ref(1)
    const notEmpty = ref(false)
    const result = ref([])
    const order = ref(startOrder)
    const type = ref('DESC')

    const GetOrder = () => {
        return `${order.value} ${type.value}`
    }

    const CheckFilter = () => {
        Object.keys(temp.value).forEach((i) => {
            if (temp.value[i].id != 0) filter.value[i] = temp.value[i]
        })
        Object.keys(filter.value).forEach((i) => {
            if (filter.value[i].hasOwnProperty('id'))
                if (filter.value[i].id == 0) delete filter.value[i]
            if (filter.value[i] == null || filter.value[i] == '') delete filter.value[i]
        })
        console.log(filter.value)
        if (Object.keys(filter.value).length <= 0) {
            return false
        }
        return true
    }

    const clear = () => {
        filter.value = {}
        temp.value = { level: { id: 0 }, type: { id: 0 } }
        result.value = []
        notEmpty.value = false
    }

    return { filter, temp, maxPage, result, notEmpty, order, type, GetOrder, CheckFilter, clear }
}