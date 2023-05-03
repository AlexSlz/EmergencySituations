<template>
  <div iv class="item">
    <p class="text-2xl py-3">
      {{ data.name }}
      <span class="text-sm tracking-normal whitespace-nowrap text-mySecondText">{{ info.GetTime(data.dateAndTime) }}</span>
    </p>
    <p class="line-clamp-3 text-mySecondText">{{ data.description }}</p>
    <img v-show="imgLoad" :src="info.GetImage(data.image)" @load="imgLoad = true" />
  </div>
</template>
<script>
import info from '@/main/infoManager'
export default {
  props: {
    data: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      info: info,
      imgLoad: false,
    }
  },
}
</script>
<style>
.item {
  @apply px-7 py-3 border-b-2 border-mySecond hover:border-myActive hover:cursor-pointer;
}
img {
  @apply max-w-xs p-1 m-auto;
}
</style>
