<template>
  <select :value="modelValue" @change="changeCombo" class="combo">
    <option :value="i" v-for="(item, i) in itemsVisual">{{ item }}</option>
  </select>
</template>
<script>
import database from '../../main/database'
export default {
  name: 'my-combo',
  props: {
    modelValue: {
      required: true,
    },
    tableName: {
      type: String,
    },
    items: {
      type: Array,
    },
  },
  data() {
    return {
      itemsVisual: [],
    }
  },
  methods: {
    changeCombo(e) {
      this.$emit('update:modelValue', e.target.value)
    },
  },
  watch: {
    tableName: {
      immediate: true,
      handler(val, oldVal) {
        if (val != undefined) {
          database.getDataFromTable(this.tableName).then((i) => {
            this.itemsVisual = i.map((a) => a.Назва)
          })
        }
      },
    },
    items: {
      immediate: true,
      handler(val, oldVal) {
        if (val != undefined) {
          this.itemsVisual = val
        }
      },
    },
  },
}
</script>
