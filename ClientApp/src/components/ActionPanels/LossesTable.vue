<template>
  <h1>Кількість постраждалих осіб</h1>
  <input v-model="element.affectedPerson" type="number" />
  <h1>Кількість постраждалих тварин</h1>
  <input v-model="element.affectedAnimals" type="number" />

  <h1>Кількість зруйнованих будівель</h1>
  <input v-model="element.destroyedBuildings" type="number" />
  <h1>Кількість пошкоджених будівель</h1>
  <input v-model="element.damagedBuildings" type="number" />

  <h1>Кількість знищених особистих речей</h1>
  <input v-model="element.destroyedPersonalItems" type="number" />
  <h1>Кількість пошкоджених особистих речей</h1>
  <input v-model="element.damagedPersonalItems" type="number" />

  <h1>Збитки</h1>
  <input v-model="element.costs" type="number" />
</template>
<script>
export default {
  props: {
    element: {
      type: Object,
      default: {},
    },
  },
}
</script>
