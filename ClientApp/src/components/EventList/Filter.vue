<template>
  <div class="p-4">
    <h1>Опис</h1>
    <input v-model="params.description" type="text" />
    <h1>Рівень</h1>
    <my-combo v-model="params.level" tableName="EmergencyLevel" firstElement="true" />
    <h1>Тип</h1>
    <my-combo v-model="params.type" tableName="EmergencyType" firstElement="true" />
    <h1>Сортувати</h1>
    <span class="flex">
      <select v-model="params.sort">
        <option value="dateAndTime">За датою та часом</option>
        <option value="name">За назвою</option>
      </select>
      <select class="w-16" v-model="params.orderBy">
        <option :value="true">↑</option>
        <option :value="false">↓</option>
      </select>
    </span>
  </div>
</template>

<script>
export default {
  props: {
    params: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      type: 'text',
    }
  },
  methods: {},
}
</script>
