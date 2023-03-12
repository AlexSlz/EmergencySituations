<template lang="">
  <div v-for="(item, i) in items">
    <my-input v-model="item.Назва" />
    <p>{{ item.X.toFixed(3) }} | {{ item.Y.toFixed(3) }}</p>
    <my-button @click="RemoveItem(i)">Remove</my-button>
  </div>
  <my-button @click="AddItem">Add</my-button>
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
  data() {
    return {}
  },
  methods: {
    AddItem() {
      console.log(this.currentId)
      this.items.push({ Назва: 'Новий маркер', 'Код нс': this.currentId, X: -1, Y: -1 })
      this.$emit('UpdateItems', this.items)
    },
    RemoveItem(id) {
      this.items.splice(id, 1)
      this.$emit('UpdateItems', this.items)
    },
  },
  emits: ['UpdateItems'],
}
</script>
<style></style>
