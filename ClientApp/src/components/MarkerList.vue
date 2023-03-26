<template>
  <div class="p-5">
    <div v-for="(item, i) in items">
      <input class="input" v-model="item.Назва" />
      <p>{{ item.X.toFixed(3) }} | {{ item.Y.toFixed(3) }}</p>
      <button class="input" @click="RemoveItem(i)">Remove</button>
    </div>
    <button class="input" @click="AddItem">Add</button>
  </div>
</template>
<script>
export default {
  props: {
    currentId: {
      type: Number,
      required: true,
    },
    items: {
      type: Object,
      required: true,
    },
  },
  methods: {
    AddItem() {
      this.items.push({ Назва: 'Новий маркер', 'Код нс': this.currentId, X: -1, Y: -1 })
    },
    RemoveItem(id) {
      this.items.splice(id, 1)
    },
  },
}
</script>
<style></style>
