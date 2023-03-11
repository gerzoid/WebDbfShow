<script setup>
import { useFileStore } from "../../../stores/filestore";
import { ref, onMounted, onRenderTriggered } from "vue";
import { showNotification } from "../../../plugins/notification";
import Api from "../../../plugins/api";

const fileStore = useFileStore();
var spisok = ref([]);
var spinning = ref(true);

var columns = ref([
  {
    title: "Значение",
    dataIndex: "value",
    sorter: (a, b) => {
      return a.value.toUpperCase() < b.value.toUpperCase() ? 0 : -1;
    },
  },
  {
    title: "Количество",
    dataIndex: "count",
    sorter: (a, b) => {
      return a.count - b.count;
    },
  },
]);

onMounted(() => {
  Api.GetGroups()
    .then((result) => {
      if (result.data.length > 0) {
        Object.keys(result.data[0].summ).forEach((item) =>
          columns.value.push({
            title: item,
            dataIndex: item,
            sorter: (a, b) => {
              return a.count - b.count;
            },
          })
        );
      }
      var res = [];
      result.data.forEach(function (value) {
        var arr_elem = {};
        for (var prop in value.summ)
          arr_elem[prop] = parseFloat(value.summ[prop].toFixed(2));
        arr_elem["count"] = value["count"];
        arr_elem["value"] = value["value"];
        res.push(arr_elem);
      });
      spisok.value = res;
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
          :pagination="{ pageSize: 15, simple: true }"
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
