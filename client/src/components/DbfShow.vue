<script setup>
import { HotTable } from "@handsontable/vue3";
import Handsontable from "handsontable";
import axios from "axios";
import { useFileStore } from "../stores/filestore";
import { storeToRefs } from "pinia";
import "handsontable/dist/handsontable.full.min.css";
import { registerAllModules } from "handsontable/registry";
import { ref, watch, toRaw, onMounted } from "vue";

const fileStore = useFileStore();

var hot = ref(null);
registerAllModules();

function onModifyRowData(row) {
  console.log("edit row");
}
//Custom renderer
/*Handsontable.renderers.registerRenderer("myrenderer", (hotInstance, TD, ...rest) => {
  Handsontable.renderers.TextRenderer(hotInstance, TD, ...rest);
  if (
    hot.value.hotInstance.getDataAtCell(rest[0], fileStore.fileInfo.countColumns) == true
  ) {
    TD.classList.add("deleted");
  }
});*/

var settings = ref({
  licenseKey: "non-commercial-and-evaluation",
  columns: toRaw(fileStore.fileInfo.columns),
  rowHeaders(index) {
    return (fileStore.page - 1) * fileStore.pageSize + index + 1;
  },
  colHeaders: true,
  width: "100%",
  height: "100%",
  manualColumnResize: true,
  columnSorting: true,
  stretchH: "all",
  afterChange: afterChange,
  hiddenColumns: { columns: [fileStore.fileInfo.countColumns] }, //последняя колонка всегда _IS_DELETED_, всегда скрыта
  cells: function (row, col, prop) {
    var cellProperties = {};
    if (hot.value.hotInstance.getDataAtCell(row, fileStore.fileInfo.countColumns) == true)
      cellProperties.className = "deleted";
    return cellProperties;
  },
});

watch(
  () => [fileStore.page, fileStore.pageSize],
  () => {
    getData();
  }
);

//Получение данных с сервера
function getData() {
  const data = new FormData();
  data.append("FileName", fileStore.fileInfo.name);
  data.append("PageSize", fileStore.pageSize);
  data.append("Page", fileStore.page);
  axios
    .post("http://localhost:5149/api/editor/getData", data)
    .then((result) => {
      hot.value.hotInstance.updateData(result.data);
    })
    .catch((e) => {
      console.log(e);
    });
}

//Изменение данных в таблице
function afterChange(changes) {
  if (changes == null) return;
  //const data = new FormData();
  let result = {};
  result["data"] = [];
  let cnt = 0;
  result["FileName"] = fileStore.fileName;
  for (var i = 0; i < changes.length; i++) {
    result["data"][cnt] = {
      field: changes[i][1],
      row: changes[i][0],
      old: changes[i][2],
      value: changes[i][3],
    };
    cnt++;
  }
  axios
    .post("http://localhost:5149/api/editor/modify", result)
    .then((result) => {
      console.log("update data post result = ", result);
    })
    .catch((e) => {
      console.log(e);
    });
}

getData();
</script>

<template>
  <hot-table ref="hot" :settings="settings"></hot-table>
</template>

<style scoped>
.upload-dbf {
  width: 400px;
  display: block;
  margin-left: auto;
  margin-right: auto;
  background-color: white;
  padding-top: 50px;
}
</style>
