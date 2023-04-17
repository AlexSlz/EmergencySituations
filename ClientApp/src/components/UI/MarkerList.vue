<template>
  <div class="px-5 overflow-auto max-h-72">
    <div v-for="(item, i) in items">
      <input v-model="item.Назва" />
      <p>{{ item.Розташування }}</p>
      <p>{{ item.X.toFixed(3) }} | {{ item.Y.toFixed(3) }}</p>
      <button @click="RemoveItem(i)">Remove</button>
    </div>
  </div>
  <button @click="AddItem">Add</button>
</template>
<script>
export default {
  props: {
    items: {
      type: Object,
      required: true,
    },
  },
  methods: {
    AddItem() {
      this.items.push({ Назва: 'Новий маркер', Розташування: 'Україна', 'Код нс': 0, X: -1, Y: -1 })
    },
    RemoveItem(id) {
      this.items.splice(id, 1)
    },
  },
}
</script>
<style></style>
