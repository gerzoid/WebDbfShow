<script setup>
import { ref } from "vue";
import UploadFile from "./components/UploadFile.vue";
import ListUploadFiles from "./components/ListUploadFiles.vue";
import { showNotification } from "./plugins/notification";
import { useFileStore } from "./stores/filestore";
import Api from "./plugins/api";
import { HideFirstPage } from "./plugins/utils";

const fileStore = useFileStore();
var listUploadedFiles = ref(null);

var onSelectedFile = (id, originalName) => {
  Api.OpenFile(id)
    .then((result) => {
      fileStore.fileInfo = result.data;
      fileStore.fileName = result.data.name;
      fileStore.originalFileName = result.data.originalFileName;
      fileStore.isLoading = true;
      HideFirstPage();
    })
    .catch((e) => {
      console.log(e);
    });
};
var onUploadCompleted = (data) => {
  fileStore.fileInfo = data;
  fileStore.fileName = data.name;
  fileStore.isLoading = true;
  showNotification("success", "Загрузка файлов", "Файл успешно загружен");
  HideFirstPage();
};
</script>

<template>
  <div class="subcontent">
    <upload-file @upload-completed="onUploadCompleted"></upload-file>
    <list-upload-files @selectedFile="onSelectedFile"></list-upload-files>
  </div>
</template>
