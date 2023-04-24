<template>
  <select :value="modelValue" @change="changeCombo">
    <option class="text-mySecond" :value="item" v-for="item in itemsVisual">{{ item }}</option>
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
      console.log(e.target.value)
      this.$emit('onChange', e.target.value)
      this.$emit('update:modelValue', e.target.value)
    },
    checkValue() {
      console.log(this.modelValue)
      if (typeof this.modelValue != 'number') {
        this.$emit('update:modelValue', this.itemsVisual.indexOf(this.modelValue) + 1)
      }
    },
  },
  emits: ['onChange'],
  watch: {
    tableName: {
      immediate: true,
      handler(val, oldVal) {
        if (val != undefined) {
          database.GetData(this.tableName).then((i) => {
            this.itemsVisual = i.map((a) => a.name)
            if (this.modelValue === undefined) this.$emit('update:modelValue', this.itemsVisual[0])
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
