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
  wordWrap: false,
  autoColumnSize: true,
  afterChange: afterChange,
  beforeOnCellMouseDown: beforeOnCellMouseDown,
  hiddenColumns: { columns: [fileStore.fileInfo.countColumns] }, //последняя колонка всегда _IS_DELETED_, всегда скрыта
  cells: function (row, col, prop) {
    var cellProperties = {};
    if (hot.value.hotInstance.getDataAtCell(row, fileStore.fileInfo.countColumns) == true)
      cellProperties.className = "deleted";
    return cellProperties;
  },
  contextMenu: {
    items: {
      stat: {
        name() {
          return "Статистика";
        },
        submenu: {
          items: [
            {
              key: "stat:cm_group",
              name: "Группировать",
              callback(key, selection, clickEvent) {
                fileStore.modal.dopInfo = {
                  column: fileStore.fileInfo.columns[selection[0].start.col].data,
                };
                fileStore.activeModalComponent = "Group";
              },
            },
            {
              key: "stat:cm_countunique",
              name: "Количество уникальных записей по столбцу",
              callback(key, selection, clickEvent) {
                fileStore.modal.dopInfo = {
                  column: fileStore.fileInfo.columns[selection[0].start.col].data,
                };
                fileStore.activeModalComponent = "CountUnique";
              },
            },
            {
              key: "stat:cm_countvalue",
              name: "Количество записей со значением в выделеннной ячейке",
              callback(key, selection, clickEvent) {
                fileStore.modal.dopInfo = {
                  value: hot.value.hotInstance.getDataAtCell(
                    selection[0].start.row,
                    selection[0].start.col
                  ),
                  column: fileStore.fileInfo.columns[selection[0].start.col].data,
                };
                fileStore.activeModalComponent = "CountValue";
              },
            },
          ],
        },
      },
    },
  },
});

function beforeOnCellMouseDown(event, coords, TD, controller) {
  fileStore.selectedColumnType =
    fileStore.fileInfo.columns[coords.col].dbType +
    "(" +
    fileStore.fileInfo.columns[coords.col].dbSize +
    ")";
}

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

  let result = {
    FileName: fileStore.fileName,
    data: changes.map((c) => ({
      field: c[1],
      row: c[0],
      old: c[2],
      value: c[3],
    })),
  };
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
