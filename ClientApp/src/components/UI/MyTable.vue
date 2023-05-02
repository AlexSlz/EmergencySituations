<template>
  <div class="w-full p-3 overflow-auto">
    <table class="w-full text-center">
      <thead>
        <tr class="bg-mySecond">
          <th class="p-3 border-x-2" v-for="(item, i) in headers">
            {{ item }}
          </th>
          <!-- <th>Дії</th> -->
        </tr>
      </thead>
      <tbody>
        <tr class="hover:bg-mySecondActive transition-colors" v-for="item in body">
          <td :class="first ? 'first:bg-mySecond' : ''" class="border-x-2 p-2" v-for="(i, index) in item">
            <p
              class="line-clamp-2 hover:line-clamp-none text-myActive hover: cursor-pointer"
              v-if="findIndex(index)"
              @click="$emit('onLink', index)"
            >
              {{ i }}
            </p>
            <p v-else class="line-clamp-2 hover:line-clamp-none">{{ i }}</p>
          </td>
          <!-- <td class="w-56"></td> -->
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: 'myTable',
  props: {
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
    link: {
      type: Array,
      default: [],
    },
  },
  methods: {
    findIndex(id) {
      return this.link.find((i) => i.name == id) != undefined
    },
  },
  emits: ['onLink'],
}
</script>
