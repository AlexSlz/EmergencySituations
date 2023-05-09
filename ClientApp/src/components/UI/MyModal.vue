<template>
  <div v-if="show" class="z-[999] top-0 left-0 fixed flex w-full h-full">
    <div class="bg-myMain m-auto text-center w-96 z-[99999]">
      <slot></slot>
      <span>
        <button @click="show = false">Close</button>
      </span>
    </div>
    <div @click="show = false" class="fixed w-full h-full bg-[rgba(0,0,0,0.4)]"></div>
  </div>
</template>

<script>
export default {
  name: 'my-modal',
  data() {
    return {
      show: false,
    }
  },
  methods: {
    Open() {
      this.show = true
    },
  },
}
</script>
