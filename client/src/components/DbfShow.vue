<script setup>
import { HotTable } from "@handsontable/vue3";
import axios from "axios";
import { useFileStore } from "../stores/filestore";
import { storeToRefs } from "pinia";
import "handsontable/dist/handsontable.full.min.css";
import { registerAllModules } from 'handsontable/registry';
import { ref, watch, toRaw, onMounted } from "vue";

const fileStore = useFileStore();

var hot = ref(null);
registerAllModules();

function onModifyRowData(row){
  console.log('edit row');
};

var settings = ref({
  licenseKey: "non-commercial-and-evaluation",
  columns: toRaw(fileStore.fileInfo.columns),
  rowHeaders(index) {
    return (fileStore.page-1)*fileStore.pageSize+index+1;
  },
  colHeaders: true,
  width: "100%",
  height: "100%",
  manualColumnResize: true,
  stretchH: "all",
  modifyRowData : "onModifyRowData",
});

watch(()=>([fileStore.page, fileStore.pageSize]), () => {
  getData();
})

function getData() {
  const data = new FormData();
  data.append("FileName", fileStore.fileInfo.name);
  data.append("PageSize", fileStore.pageSize);
  data.append("Page", fileStore.page);
  axios
    .post("http://localhost:5149/api/Files/getData", data)
    .then((result) => {
      hot.value.hotInstance.updateData(result.data);
    })
    .catch((e) => {
      console.log(e);
    });
}

getData();
</script>

<template>
  <hot-table
    ref="hot"
    :settings="settings"
  ></hot-table>
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
