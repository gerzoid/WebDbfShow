<script setup>
import { ref, onMounted } from "vue";
import ModalComponent from "../components/ModalComponent.vue";
import Pagination from "../components/Pagination.vue";
import UploadFile from "../components/UploadFile.vue";
import FileMenu from "../components/Menu.vue";
import ListUploadFiles from "../components/ListUploadFiles.vue";
import Dbfshow from "../components/DbfShow.vue";
import { showNotification } from "../plugins/notification";
import { useFileStore } from "../stores/filestore";
import Api from "../plugins/api";
import { useHead } from "@unhead/vue";

useHead({
  title: "DBF редактор онлайн: Легкий доступ и редактирование DBF файлов",
  meta: [
    {
      name: "description",
      content:
        "Изучите возможности DBF редактора онлайн, позволяющего легко и удобно редактировать DBF файлы прямо в браузере. Раскройте потенциал онлайн редактора для изменения данных, добавления новых записей, фильтрации и сортировки информации в DBF файлах.",
    },
    {
      name: "keywords",
      content:
        "DBF редактор онлайн, онлайн редактор DBF, редактирование DBF файлов, изменение DBF данных, добавление записей в DBF, фильтрация DBF, сортировка DBF, онлайн инструмент для DBF",
    },
  ],
});

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
      if (fileStore.fileInfo.codePageId == 0)
        showNotification(
          "warning",
          "Кодировка файла",
          "У файла не указана кодировка, данные могут отображаться не корректно. Кодировку можно поментья в меню Разное\\Кодировка",
          5
        );
    })
    .catch((e) => {
      console.log(e);
    });
};
var onUploadCompleted = (data) => {
  fileStore.fileInfo = data;
  fileStore.fileName = data.name;
  showNotification("success", "Загрузка файлов", "Файл успешно загружен");
  fileStore.isLoading = true;
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
        <div v-else class="page">
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
        <a @click="() => (fileStore.activeModalComponent = 'Codepage')">{{
          fileStore.getCodePage
        }}</a>
        <b> Формат:</b> {{ fileStore.fileInfo.version }} |
        {{ fileStore.selectedColumnType }}
      </div>
    </a-layout-content>
    <div style="text-align: center">jobtools.ru ©2023</div>
  </a-layout>
</template>

<style>
.ant-row-rtl #components-layout-demo-top .logo {
  float: right;
  margin: 16px 0 16px 24px;
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
