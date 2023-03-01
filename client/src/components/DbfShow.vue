<script setup>
import { HotTable } from "@handsontable/vue3";
import Handsontable from "handsontable";
import { useFileStore } from "../stores/filestore";
import { storeToRefs } from "pinia";
import "handsontable/dist/handsontable.full.min.css";
import { registerAllModules } from "handsontable/registry";
import { ref, watch, toRaw, onMounted } from "vue";
import { showNotification } from "../plugins/notification";
import Api from "../plugins/api";

const fileStore = useFileStore();
//Изменения не требующие синхронизации с бэком
var notSyncChanges = false;

var hot = ref(null);
registerAllModules();

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
    return (fileStore.options.page - 1) * fileStore.options.pageSize + index + 1;
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
  () => [fileStore.options.page, fileStore.options.pageSize, fileStore.needReload],
  () => {
    getData();
    fileStore.needReload = false;
  }
);

//Получение данных с сервера
function getData() {
  Api.GetData()
    .then((result) => {
      hot.value.hotInstance.updateData(result.data);
    })
    .catch((e) => {
      console.log(e);
    });
}

//Изменение данных в таблице
function afterChange(changes) {
  if (notSyncChanges) return;
  if (changes == null) return;
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
  Api.Change(result)
    .then((result) => {
      notSyncChanges = true;
      for (var i = 0; i < changes.length; i++) {
        if (result.data[i].result == false)
          hot.value.hotInstance.setDataAtCell(
            changes[i][0],
            hot.value.hotInstance.propToCol(changes[i][1]),
            changes[i][2]
          );
        else
          hot.value.hotInstance.setDataAtCell(
            changes[i][0],
            hot.value.hotInstance.propToCol(changes[i][1]),
            result.data[i].value
          );
      }
      notSyncChanges = false;
    })
    .catch((e) => {
      notSyncChanges = true;
      for (var i = 0; i < changes.length; i++) {
        hot.value.hotInstance.setDataAtCell(
          changes[i][0],
          hot.value.hotInstance.propToCol(changes[i][1]),
          changes[i][2]
        );
      }
      notSyncChanges = false;
      showNotification(
        "error",
        "Обновление данных",
        "Произошла непредвиденная ошибка а сервере, данные не были обновлены.",
        3
      );
    })
    .finally(() => (notSyncChanges = false));
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
