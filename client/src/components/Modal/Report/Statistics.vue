<script setup>
import { useFileStore } from "../../../stores/filestore";
import { onMounted } from "vue";
import Api from "../../../plugins/api";

const fileStore = useFileStore();
onMounted(() => {
  Api.GetStatistics();
});
</script>
<template>
  <div class="card">
    <div class="content" style="padding: 10px">
      Статистическая информация по полям файла
    </div>
  </div>
</template>
