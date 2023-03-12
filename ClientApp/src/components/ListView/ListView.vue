<script setup>
import ItemInfo from './ItemInfo.vue'
import ItemList from './ItemList.vue'
</script>

<template>
  <div class="block">
    <div v-if="selectedElement != null">
      <my-button @click="this.$emit('ClickOnElement', null)">Close</my-button>
      <ItemInfo :selectedElement="selectedElement" />
    </div>
    <ItemList @click="this.$emit('ClickOnElement', item)" v-else :data="item" v-for="item in elementList" />
  </div>
</template>

<script>
export default {
  props: {
    elementList: {
      type: Array,
      required: true,
    },
    selectedElement: {
      type: Object,
    },
  },
  emits: ['ClickOnElement'],
  components: { ItemInfo },
}
</script>
