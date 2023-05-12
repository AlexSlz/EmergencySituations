<template>
  <h1>Назва</h1>
  <input v-model="element.name" />
  <h1>Опис</h1>
  <textarea v-model="element.description"></textarea>
  <h1>Дата і час</h1>
  <input v-model="element.dateAndTime" type="datetime-local" />
  <h1>Рівень надзвичайної ситуації</h1>
  <my-combo v-model="element.level.id" tableName="EmergencyLevel" />
  <h1>Тип надзвичайної ситуації</h1>
  <my-combo v-model="element.type.id" tableName="EmergencyType" />
  <h1>Зображення</h1>
  <file-input v-model="element.image" tableName="Emergency" />

  <h1>Втрати</h1>
  <div class="px-5 py-2">
    <LossesTable :element="element.losses" />
  </div>
  <h1>Позиції</h1>
  <pos-list :items="element.positions" />
</template>
<script>
import info from '@/main/infoManager'
import LossesTable from './LossesTable.vue'

export default {
  props: {
    element: {
      type: Object,
      default: {},
    },
  },
  beforeMount() {
    if (this.element.dateAndTime == undefined) this.element.dateAndTime = info.GetTime().replace(' ', 'T')
    if (this.element.positions == undefined) this.element.positions = []
    if (this.element.losses == undefined) this.element.losses = {}
    if (this.element.type == undefined) this.element.type = { id: 1 }
    if (this.element.level == undefined) this.element.level = { id: 1 }
  },
  components: { LossesTable },
}
</script>
