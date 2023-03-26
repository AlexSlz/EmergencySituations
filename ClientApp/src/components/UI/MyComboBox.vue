<template>
  <select :value="modelValue" @change="changeCombo" class="combo">
    <option class="text-neutral-700" :value="i" v-for="(item, i) in itemsVisual">{{ item }}</option>
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
    checkValue(val) {
      if (typeof val != 'number') {
        return this.itemsVisual.indexOf(val)
      } else {
        return val
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
