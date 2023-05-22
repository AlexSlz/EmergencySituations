<template>
  <div>
    <div class="p-2">
      <p class="text-3xl py-3">
        {{ element.name }}
        <span class="text-sm tracking-normal whitespace-nowrap text-mySecondText">{{ info.GetTime(element.dateAndTime) }}</span>
      </p>

      <div class="border-y-2">
        <p>Тип - {{ element.type.name }} <i class="fas" :class="icon[0].element[element.type.name]"></i></p>
        <p>Рівень - {{ element.level.name }} <i class="fas" :class="icon[1].element[element.level.name]"></i></p>
        <p>Розташування - {{ info.GetLocation(element) }}</p>
      </div>

      <p class="text-xl p-2">{{ element.description }}</p>
      <img :src="info.GetImage(element.image)" />
      <p class="px-2 text-2xl">Втрати:</p>

      <my-table v-if="showTable" :headers="['Назва', 'Кількість']" :body="body" />
      <p v-else class="p-2">Інформації немає</p>
    </div>
  </div>
</template>
<script>
import { useAuthStore } from '@/stores/auth'
import info from '@/main/infoManager'
import icon from '@/main/label'
export default {
  props: {
    element: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      icon: icon,
      info: info,
      authStore: useAuthStore(),
      showTable: true,
      body: [],
    }
  },
  mounted() {
    this.getBody()
  },
  methods: {
    getBody() {
      var temp = Object.values(this.element.losses)
      temp[0] = 0
      this.showTable = temp.reduce((a, b) => a + b) > 0
      this.body.push(['Кількість осіб з втратою працездатності до 9 днів', this.element.losses.easyAccident])
      this.body.push(['Кількість осіб з втратою працездатності понад 9 днів', this.element.losses.hardAccident])
      this.body.push(['Кількість осіб з отриманою інвалідністю', this.element.losses.disabilityPerson])
      this.body.push(['Кількість загиблих до 16 років', this.element.losses.deathPersonUndersSixteen])
      this.body.push(['Кількість загиблих від 16 до 60 років', this.element.losses.deathPersonUnderSixty])
      this.body.push(['Збитки від пошкоджених або зруйнованих будівель', this.element.losses.destroyedBuildings])
      this.body.push(['Збитки від пошкоджених або знищених особистих речей', this.element.losses.damagedPersonalItems])
      this.body.push(['Збитки', this.element.losses.costs])
    },
  },
}
</script>
