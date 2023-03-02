<script setup>
import { useFileStore } from "../../../stores/filestore";
import { ref, onMounted, onRenderTriggered } from "vue";
import { showNotification } from "../../../plugins/notification";
import Api from "../../../plugins/api";

const fileStore = useFileStore();
var spisok = ref([]);
var spinning = ref(true);

const columns = [
  {
    title: "Имя",
    dataIndex: "name",
  },
  {
    title: "Тип",
    dataIndex: "type",
  },
  {
    title: "Min",
    dataIndex: "min",
    align: "center",
  },
  {
    title: "Max",
    dataIndex: "max",
    align: "center",
  },
  {
    title: "Сумма",
    dataIndex: "sum",
    align: "center",
  },
];

onMounted(() => {
  Api.GetStatistics()
    .then((result) => {
      spisok.value = result.data;
      spinning.value= false;
    })
    .catch((e) =>{
      console.log(e.response.data);
      showNotification("error", "Внимание", "Ошибка при формировании данных на севрере. Подробнее в консоли", 5);
    });
});
</script>
<template>
  <div class="card">
    <div class="content" style="padding: 10px">
      <h4>Статистическая информация по полям файла</h4>
      <a-spin :spinning="spinning" size="large">
        <a-table
          class="stat-table"
          :columns="columns"
          :data-source="spisok"
          :bordered="true"
          :pagination="{ pageSize: 15 }"
          size="small"
        />
      </a-spin>
    </div>
  </div>
</template>

<style>
div.stat-table .ant-table-small {
  font-size: 13px !important;
}
div.stat-table .ant-table-cell {
  padding: 4px 4px !important;
}
</style>
