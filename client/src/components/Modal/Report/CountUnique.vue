<script setup>
import Api from "../../../plugins/api";
import { useFileStore } from "../../../stores/filestore";
import { ref, onMounted } from "vue";
import { showNotification } from "../../../plugins/notification";

const fileStore = useFileStore();
var countUnique = ref(0);
var spinning = ref(true);

onMounted(() => {
  Api.GetMiniStatCount("countunique")
    .then((result) => {
      countUnique.value = result.data;
      spinning.value = false;
    })
    .catch((e) => {
      console.log(e);
      showNotification(
        "error",
        "Внимание",
        "Ошибка при формировании данных на севрере. Подробнее в консоли",
        5
      );
    });
});
</script>
<template>
  <div class="card">
    <a-spin :spinning="spinning" size="large">
      <div class="content" style="padding: 10px">
        Количество уникальных записей по столбцу:
        <b>{{ countUnique }}</b
        ><br />
      </div>
    </a-spin>
  </div>
</template>
