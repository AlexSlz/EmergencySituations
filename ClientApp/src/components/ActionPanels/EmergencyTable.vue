<template>
  <h1>Назва</h1>
  <input v-model="element.name" />
  <h1>Опис</h1>
  <textarea v-model="element.description"></textarea>
  <h1>Дата і час</h1>
  <input v-model="element.dateAndTime" type="datetime-local" />
  <h1>Рівень надзвичайної ситуації</h1>
  <my-combo v-model="element.level" tableName="EmergencyLevel" />
  <h1>Тип надзвичайної ситуації</h1>
  <my-combo v-model="element.type" tableName="EmergencyType" />
  <h1>Зображення</h1>
  <file-input v-model="element.image" tableName="Emergency" />
  <h1>Збитки</h1>
  <input v-model="element.costs" type="number" />

  <h1>Позиції</h1>
  <pos-list :items="element.positions" />
</template>
<script>
import { useEmergencyStore } from '@/stores/emergency'

export default {
  props: {
    element: {
      type: Object,
      default: {},
    },
  },
  data() {
    return {
      emergency: useEmergencyStore(),
    }
  },
  beforeMount() {
    if (this.element.dateAndTime == undefined) this.element.dateAndTime = new Date().toJSON().slice(0, 19)
    if (this.element.positions == undefined) this.element.positions = []
  },
}
</script>
