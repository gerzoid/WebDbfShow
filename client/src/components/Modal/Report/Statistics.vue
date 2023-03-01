<script setup>
import { useFileStore } from "../../../stores/filestore";
import { ref, onMounted, onRenderTriggered } from "vue";
import Api from "../../../plugins/api";

const fileStore = useFileStore();
var spisok =ref([]);

onMounted(() => {
  Api.GetStatistics()
  .then(result=>{
    spisok = result.data;
    console.log(spisok);

  }).catch((e)=> console.log(e));
});
</script>
<template>
  <div class="card">
    <div class="content" style="padding: 10px">
      Статистическая информация по полям файла
      <ul>
        <li v-for="item in spisok"> {{item.name}} : {{ item.type }} : {{ item.max }} : {{ item.min }}</li>
      </ul>
    </div>
  </div>
</template>
