<template>
  <div class="w-full p-3 overflow-auto">
    <table class="w-full text-center">
      <thead>
        <tr class="bg-mySecond">
          <th class="p-3 border-x-2" v-for="(item, i) in headers">
            {{ item }}
          </th>
        </tr>
      </thead>
      <tbody>
        <tr class="hover:bg-mySecondActive transition-colors" v-for="item in body">
          <td :class="first ? 'first:bg-mySecond' : ''" class="border-x-2 p-2" v-for="(i, index) in item">
            <p
              @click="onLinkClick(index)"
              :class="i.custom ? 'text-myActive cursor-pointer' : ''"
              class="line-clamp-2 hover:line-clamp-none"
            >
              {{ customData(i, index) }}
            </p>
          </td>
          <!-- <td class="w-56"></td> -->
          <td v-if="addon" class="w-64"><slot :item="item"></slot></td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: 'myTable',
  props: {
    addon: {
      type: Boolean,
      default: false,
    },
    headers: {
      type: Array,
      required: true,
    },
    body: {
      type: Array,
      required: true,
    },
    first: {
      type: Boolean,
    },
  },
  data() {
    return {
      link: [],
    }
  },
  methods: {
    onLinkClick(id) {
      if (this.findIndex(id)) {
        this.$emit('onLink', id)
      }
    },
    findIndex(id) {
      return this.link.find((i) => i == id) != undefined
    },
    customData(item, i) {
      if (typeof item == 'object') {
        if ('name' in item) {
          this.link.push(i)
          item.custom = true
          return item.name
        }
        if ('id' in item) {
          this.link.push(i)
          item.custom = true
          return item.id
        }
      }
      if (Array.isArray(item)) {
        this.link.push(i)
        item.custom = true
        return item.map((p) => p.location).join(', ')
      }
      return item

      this.table[1].forEach((i, index) => {
        Object.keys(i).forEach((j) => {
          if (typeof i[j] == 'object') {
            if ('name' in i[j]) {
              i[j] = i[j].name
              this.link.push({ name: j })
              return
            }
            if ('id' in i[j]) {
              i[j] = i[j].id
              this.link.push({ name: j })
              return
            }
          }
          if (Array.isArray(i[j])) {
            i[j] = i[j].map((p) => p.location).join(', ')
            this.link.push({ name: j })
            return
          }
        })
      })
    },
  },
  emits: ['onLink'],
}
</script>
