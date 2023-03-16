<template>
  <input
    class="input"
    :value="modelValue"
    @input="updateInput"
    :type="type"
    :disabled="disabled"
    placeholder="Введіть значення"
    accept="image/*"
    required
  />
</template>
<script>
export default {
  name: 'my-input',
  props: {
    modelValue: {
      required: true,
    },
    type: {
      type: String,
      default: 'text',
    },
    disabled: {
      type: Boolean,
    },
  },
  methods: {
    updateInput(e) {
      this.$emit('update:modelValue', e.target.value)
    },
  },
}
</script>
