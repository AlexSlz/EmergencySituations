<template>
  <div iv class="item">
    <p class="text-2xl py-3">
      {{ data.Назва }}
      <span class="text-sm tracking-normal whitespace-nowrap text-myMain">{{ getTime(data['Дата та час']) }}</span>
    </p>
    <p class="line-clamp-3 text-mySecondText">{{ data.Опис }}</p>
    <img v-show="imgLoad" :src="getImage(data.Зображення)" @load="imgLoad = true" />
  </div>
</template>
<script>
export default {
  props: {
    data: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      imgLoad: false,
    }
  },
  methods: {
    getImage(id) {
      if (id != '') return `${window.location.origin}/api/file/Надзвичайні ситуації/${id}`
    },
    getTime(time) {
      var result = time.replace('T', ' ')
      var temp = result.split(' ', 2)
      if (temp[1] === '00:00:00') return temp[0]
      return result
    },
  },
}
</script>
<style>
.item {
  @apply px-7 py-3 border-b-2 border-mySecond hover:border-myMain hover:cursor-pointer;
}
img {
  @apply max-w-xs p-1 m-auto;
}
</style>
