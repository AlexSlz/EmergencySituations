<template>
  <form class="p-2" @submit.prevent="search.notEmpty ? clear() : Find(1)">
    <div class="flex items-center">
      <h1 class="px-2">Сортувати за</h1>
      <select @change="onSortChange" v-model="search.order" class="max-w-[16rem]" :disabled="loading || search.notEmpty">
        <template v-if="!hideMenu">
          <option value="DateAndTime">Датою</option>
          <option value="name">Назвою</option>
          <option value="level">Рівнем</option>
          <option value="type">Типом</option>
        </template>
        <template v-else>
          <option :value="key" v-for="key in keys">{{ key }}</option>
        </template>
      </select>
      <select @change="onSortChange" v-model="search.type" class="w-12" :disabled="loading || search.notEmpty">
        <option value="DESC">↓</option>
        <option value="ASC">↑</option>
      </select>
    </div>
    <div class="flex">
      <my-combo
        @onChange="search.filter = {}"
        v-model="keyValue"
        class="w-28"
        v-if="keys.length > 1"
        :items="keys"
        :disabled="loading || search.notEmpty"
      />
      <input placeholder="Пошук..." v-model="search.filter[GetMainKey()]" type="search" :disabled="loading || search.notEmpty" />
      <input v-if="!hideMenu" @click="showMore = !showMore" class="w-10" type="button" :value="showMore ? '<-' : '->'" />
    </div>
    <div v-if="showMore" class="block">
      <input
        placeholder="Пошук за описом..."
        v-model="search.filter.description"
        type="search"
        :disabled="loading || search.notEmpty"
      />
      <h1>Рівень</h1>
      <my-combo
        v-model="search.temp.level.id"
        tableName="EmergencyLevel"
        firstElement="true"
        :disabled="loading || search.notEmpty"
      />
      <h1>Тип</h1>
      <my-combo
        v-model="search.temp.type.id"
        tableName="EmergencyType"
        firstElement="true"
        :disabled="loading || search.notEmpty"
      />
      <input v-model="search.filter.dateAndTime" type="date" :disabled="loading || search.notEmpty" />
    </div>
    <button type="submit" class="bg-myElement" :disabled="loading">
      <i v-if="search.notEmpty" class="fas fa-times"></i>
      <i v-else class="fas fa-search"></i>
    </button>
  </form>
</template>

<script>
import database from '@/main/database'
import { useNotify } from '@/stores/Notify'
export default {
  props: {
    keys: {
      type: Array,
      default: [],
    },
    hideMenu: {
      default: false,
    },
    tableName: {
      default: 'Emergency',
    },
    search: {
      required: true,
    },
  },
  data() {
    return {
      keyValue: 2,
      notify: useNotify(),
      showMore: false,
      page: 1,
      loading: false,
    }
  },
  methods: {
    onSortChange() {
      this.$emit('onSortChange')
    },
    GetMainKey() {
      var key = 'name'
      if (this.keys.length > 0) key = this.keys[this.keyValue - 1]
      if (this.keys.length == 1) key = 'id'
      return key
    },
    Find(page) {
      if (!this.search.CheckFilter()) return
      this.loading = true
      database
        .Search(this.tableName, this.search.filter, page, this.search.GetOrder())
        .then((res) => {
          this.search.notEmpty = true
          this.search.maxPage = res.maxPage
          this.search.result = res.data
          this.loading = false
        })
        .catch((err) => {
          this.loading = false
          this.notify.Open('Даних не знайдено.', 'error')
        })
    },
    clear() {
      this.page = 1
      this.search.clear()
    },
  },
  emits: ['onSortChange'],
}
</script>
