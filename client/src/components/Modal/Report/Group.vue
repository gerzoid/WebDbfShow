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
    title: "Значение",
    dataIndex: "value",
    sorter: (a,b)=> {return a.value.toUpperCase() < b.value.toUpperCase() ? 0 : -1 },
  },
  {
    title: "Количество",
    dataIndex: "count",
    sorter: (a,b)=> {return a.count-b.count},
  },
];

onMounted(() => {
  Api.GetGroups()
    .then((result) => {
      spisok.value = result.data;
      spinning.value = false;
    })
    .catch((e) => {
      console.log(e.response.data);
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
    <div class="content" style="padding: 10px">
      <a-spin :spinning="spinning" size="large">
        <a-table
          class="stat-table"
          :columns="columns"
          :data-source="spisok"
          :bordered="true"
          :pagination="{ pageSize: 15, simple:true }"
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
