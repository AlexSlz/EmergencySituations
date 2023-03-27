<template>
  <select :value="modelValue" @change="changeCombo" class="combo">
    <option class="text-neutral-700" :value="i + 1" v-for="(item, i) in itemsVisual">{{ item }}</option>
  </select>
</template>
<script>
import database from '@/main/database'
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
    checkValue() {
      if (typeof this.modelValue != 'number') {
        this.$emit('update:modelValue', this.itemsVisual.indexOf(this.modelValue) + 1)
      }
    },
  },
  watch: {
    tableName: {
      immediate: true,
      handler(val, oldVal) {
        if (val != undefined) {
          database.getDataFromTable(this.tableName).then((i) => {
            this.itemsVisual = i.map((a) => a.Назва)
            this.checkValue()
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
