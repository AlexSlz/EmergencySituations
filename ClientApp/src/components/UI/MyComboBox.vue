<template>
  <select :value="modelValue" @change="changeCombo" :disabled="disabled">
    <option class="text-mySecond" :value="getIndex(index)" v-for="(item, index) in itemsVisual">{{ item }}</option>
  </select>
</template>
<script>
import database from '@/main/database'
export default {
  name: 'my-combo',
  props: {
    disabled: {
      type: Boolean,
      default: false,
    },
    modelValue: {
      required: true,
    },
    tableName: {
      type: String,
    },
    items: {
      type: Array,
    },
    firstElement: {
      default: false,
    },
  },
  data() {
    return {
      itemsVisual: [],
    }
  },
  methods: {
    changeCombo(e) {
      this.$emit('onChange', e.target.value)
      this.$emit('update:modelValue', e.target.value)
    },
    getIndex(index) {
      return this.firstElement ? index : index + 1
    },
  },
  emits: ['onChange', 'update:modelValue'],
  watch: {
    tableName: {
      immediate: true,
      handler(val) {
        if (val != undefined) {
          database.GetData(this.tableName).then((i) => {
            this.itemsVisual = i.data.map((a) => a.name)
            if (this.modelValue === undefined && !this.firstElement) this.$emit('update:modelValue', 1)
            if (this.firstElement) this.itemsVisual.unshift('')
          })
        }
      },
    },
    items: {
      immediate: true,
      handler(val) {
        if (val != undefined) {
          this.itemsVisual = val
        }
      },
    },
  },
}
</script>
