<template>
  <h1>Назва</h1>
  <input v-model="element.name" />
  <h1>Опис</h1>
  <textarea v-model="element.description"></textarea>
  <h1>Дата і час</h1>
  <input v-model="element.dateAndTime" type="datetime-local" />
  <!-- <h1>Рівень надзвичайної ситуації</h1>
  <my-combo v-model="element.level" tableName="EmergencyLevel" />
  <h1>Тип надзвичайної ситуації</h1>
  <my-combo v-model="element.type" tableName="EmergencyType" /> -->
  <h1>Зображення</h1>
  <file-input v-model="element.image" tableName="Emergency" />

  <h1>Втрати</h1>
  <div class="px-5 overflow-auto max-h-64">
    <LossesTable :element="element.losses" />
  </div>
  <h1>Позиції</h1>
  <pos-list :items="element.positions" />
</template>
<script>
import LossesTable from './LossesTable.vue'

export default {
  props: {
    element: {
      type: Object,
      default: {},
    },
  },
  beforeMount() {
    if (this.element.dateAndTime == undefined) this.element.dateAndTime = new Date().toJSON().slice(0, 19)
    if (this.element.positions == undefined) this.element.positions = []
    if (this.element.losses == undefined) this.element.losses = {}
    this.element.type = { Id: 1 }
    this.element.level = { Id: 1 }
  },
  components: { LossesTable },
}
</script>
