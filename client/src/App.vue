<script setup>
import { ref, onMounted } from "vue";
import ModalComponent from "./components/ModalComponent.vue";
import Pagination from "./components/Pagination.vue";
import UploadFile from "./components/UploadFile.vue";
import FileMenu from "./components/Menu.vue";
import ListUploadFiles from "./components/ListUploadFiles.vue";
import Dbfshow from "./components/DbfShow.vue";
import { showNotification } from "./plugins/notification";
import { useFileStore } from "./stores/filestore";
import Api from "./plugins/api";

//const els = document.querySelectorAll(".intro");
//els.forEach((el) => {
//  el.style.display = "none";
//});

const fileStore = useFileStore();
var listUploadedFiles = ref(null);

var onClosedModal = () => {
  fileStore.activeModalComponent = null;
};
var onSelectedFile = (id, originalName) => {
  Api.OpenFile(id)
    .then((result) => {
      fileStore.fileInfo = result.data;
      fileStore.fileName = result.data.name;
      fileStore.originalFileName = result.data.originalFileName;
      fileStore.isLoading = true;
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
};
</script>

<template>
  <a-layout class="layout">
    <file-menu></file-menu>
    <modal-component
      ref="modal"
      :activeComponentName="fileStore.activeModalComponent"
      @closed="onClosedModal"
    ></modal-component>
    <a-layout-content id="content">
      <div class="subcontent">
        <dbfshow v-if="fileStore.isLoading == true"></dbfshow>
        <div v-else class="upload">
          <upload-file @upload-completed="onUploadCompleted"></upload-file>
          <list-upload-files @selectedFile="onSelectedFile"></list-upload-files>
        </div>
        <pagination v-if="fileStore.isLoading == true"></pagination>
      </div>
    </a-layout-content>
    <a-layout-content v-if="fileStore.isLoading == true" id="dopinfo">
      <div>
        <b>Колонок:</b> {{ fileStore.getCountColumns }} <b>Строк:</b>
        {{ fileStore.fileInfo.countRows }} <b> Кодировка:</b>
        <a @click="onClick({ key: 'codepage' })">{{ fileStore.getCodePage }}</a>
        <b> Формат:</b> {{ fileStore.fileInfo.version }} |
        {{ fileStore.selectedColumnType }}
      </div>
    </a-layout-content>
    <a-layout-footer style="text-align: center"> jobtools.ru ©2023 </a-layout-footer>
  </a-layout>
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
.ant-row-rtl #components-layout-demo-top .logo {
  float: right;
  margin: 16px 0 16px 24px;
}

.upload {
  max-width: 50%;
  margin: 0 auto;
}

[data-theme="dark"] .site-layout-content {
  background: #141414;
}
.main-menu {
  float: left;
  width: 70%;
  line-height: "64px";
}
.menu-right {
  width: 20%;
  float: right;
  color: white;
  text-align: right;
}
</style>
