<template>
  <h1>Кількість осіб з втратою працездатності до 9 днів</h1>
  <input v-model="element.easyAccident" type="number" />
  <h1>Кількість осіб з втратою працездатності понад 9 днів</h1>
  <input v-model="element.hardAccident" type="number" />
  <h1>Кількість осіб з отриманою інвалідністю</h1>
  <input v-model="element.disabilityPerson" type="number" />
  <h1>Кількість загиблих до 16 років</h1>
  <input v-model="element.deathPersonUndersSixteen" type="number" />
  <h1>Кількість загиблих від 16 до 60 років</h1>
  <input v-model="element.deathPersonUnderSixty" type="number" />

  <h1>Збитки від пошкоджених або зруйнованих будівель</h1>
  <input v-model="element.destroyedBuildings" type="number" />

  <h1>Збитки від пошкоджених або знищених особистих речей</h1>
  <input v-model="element.damagedPersonalItems" type="number" />

  <h1>Збитки (тис. гривень)</h1>
  <input v-model="element.costs" type="number" />
  <button @click="CalculateLoss">Підрахувати збитки</button>
</template>
<script>
export default {
  props: {
    element: {
      type: Object,
      default: {},
    },
  },

  methods: {
    CalculateLoss() {
      this.element.costs =
        (this.element.easyAccident * 0.28 || 0) +
        (this.element.hardAccident * 6.5 || 0) +
        (this.element.disabilityPerson * 37 || 0) +
        (this.element.deathPersonUndersSixteen * 47 || 0) +
        (this.element.deathPersonUnderSixty * 22 || 0) +
        (this.element.destroyedBuildings || 0) +
        (this.element.damagedPersonalItems || 0)
    },
  },
}
</script>
