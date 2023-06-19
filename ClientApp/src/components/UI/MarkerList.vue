<template>
  <div class="px-5 overflow-auto max-h-72">
    <div v-for="(item, i) in items">
      <input v-model="item.name" />
      <p>{{ item.location }}</p>
      <p>{{ item.x.toFixed(3) }} | {{ item.y.toFixed(3) }}</p>
      <button @click="RemoveItem(i)" type="button">Видалити</button>
    </div>
    <button @click="AddItem" type="button">Додати</button>
  </div>
</template>
<script>
export default {
  name: 'pos-list',
  props: {
    items: {
      type: Object,
      required: true,
      default: [],
    },
  },
  methods: {
    AddItem() {
      this.items.push({ name: 'Новий маркер', location: 'Україна', emergencyId: 0, x: -1, y: -1 })
    },
    RemoveItem(id) {
      this.items.splice(id, 1)
    },
  },
}
</script>
<style></style>
