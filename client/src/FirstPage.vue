<script setup>
import { ref } from "vue";
import UploadFile from "./components/UploadFile.vue";
import ListUploadFiles from "./components/ListUploadFiles.vue";
import { showNotification } from "./plugins/notification";
import { useFileStore } from "./stores/filestore";
import Api from "./plugins/api";
import {HideFirstPage} from "./plugins/utils";

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

<style>
.site-layout-content {
  min-height: 280px;
  padding: 24px;
  background: #fff;
}
#components-layout-demo-top .logo {
  float: left;
  width: 100%;
  height: 32px;
  margin: 16px 24px 16px 0;
  background: rgba(255, 255, 255, 0.3);
}

.upload {
  max-width: 50%;
  margin: 0 auto;
}

[data-theme="dark"] .site-layout-content {
  background: #141414;
}
</style>
