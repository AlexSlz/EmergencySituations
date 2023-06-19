<template>
  <input type="file" ref="input" accept="image/png, image/gif, image/jpeg" class="hidden" @change="onFileImport" />
  <button @click="onButtonClick" type="button">{{ btnMassage }}</button>
  <label>{{ getFileName() }}</label>
</template>
<script>
import database from '@/main/database'
export default {
  name: 'file-input',
  props: {
    tableName: String,
    modelValue: {
      required: true,
    },
  },
  data() {
    return {
      btnMassage: 'Імпортувати',
      fileId: this.getRandomId(),
    }
  },
  methods: {
    getRandomId() {
      return this.modelValue != undefined && this.modelValue != '' ? this.modelValue : Math.random().toString().slice(2, 11)
    },
    getFileName() {
      if (this.modelValue != undefined && this.modelValue != '') {
        this.btnMassage = 'Очистити'
        return 'Файл'
      } else {
        return 'Виберіть файл'
      }
    },
    onButtonClick() {
      var myInput = this.$refs.input
      if (myInput.files.length >= 1 || (this.modelValue != '' && this.modelValue != undefined)) {
        myInput.type = 'text'
        myInput.type = 'file'
        this.$emit('update:modelValue', '')
        this.btnMassage = 'Імпортувати'
        return
      }
      myInput.click()
    },
    onFileImport(event) {
      let temp = event.target.files[0]
      if (temp != null) {
        database.UploadFile(this.tableName, temp, this.fileId.split('.')[0]).then((res) => {
          this.$emit('update:modelValue', res)
        })
      }
    },
  },
}
</script>
<style></style>
