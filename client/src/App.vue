<script setup>
import { ref, onMounted } from "vue";
import ModalComponent from "./components/ModalComponent.vue";
import Pagination from "./components/Pagination.vue";
import UploadFile from "./components/UploadFile.vue";
import ListUploadFiles from "./components/ListUploadFiles.vue";
import Dbfshow from "./components/DbfShow.vue";
import { setCookie } from "./plugins/cookies";
import { showNotification } from "./plugins/notification";
import { useFileStore } from "./stores/filestore";
import Api from "./plugins/api";

var selectedKeys = ref([]);
const fileStore = useFileStore();
var listUploadedFiles = ref(null);
var activeModalComponent = ref(null);
var spinnerCheckFiles = ref(false);

//Проверитьь список загруженных файлов по юзеру
function CheckUploadedFiles() {
  spinnerCheckFiles.value = true;
  Api.CheckUploadedFiles()
    .then((result) => {
      let date = new Date();
      date = new Date(date.setMonth(date.getMonth() + 8));
      setCookie("dbfshowuser", result.data.usersId, { expiries: date.toUTCString() });
      fileStore.userId = result.data.usersId;
      listUploadedFiles.value = result.data.files;
    })
    .catch((e) => {
      console.log(e);
    })
    .finally(() => (spinnerCheckFiles.value = false));
}

onMounted(() => {
  CheckUploadedFiles();
});

var onClosedModal = () => {
  activeModalComponent.value = null;
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
  //Set custom renderer
  //data.columns.map((d) => (d.renderer = "myrenderer"));

  fileStore.fileInfo = data;
  fileStore.fileName = data.name;
  fileStore.isLoading = true;
  showNotification("success", "Загрузка файлов", "Файл успешно загружен");
};

function onClick(e) {
  switch (e.key) {
    case "close":
      fileStore.closeFile();
      CheckUploadedFiles();
      break;
    case "about":
      activeModalComponent.value = "About";
      break;
    case "codepage":
      activeModalComponent.value = "Codepage";
      break;
    case "statistics":
      activeModalComponent.value = "Statistics";
      break;
    case "message":
      activeModalComponent.value = "MessageAuthor";
      break;
  }
}
</script>

<template>
  <a-layout class="layout">
    <a-layout-header>
      <div class="logo" />
      <a-menu
        class="main-menu"
        v-model:selectedKeys="selectedKeys"
        theme="dark"
        mode="horizontal"
        @click="onClick"
      >
        <a-sub-menu key="1">
          <template selected #title>Файл</template>
          <a-menu-item :disabled="!fileStore.isLoading" key="close">Закрыть</a-menu-item>
          <a-menu-item disabled key="struct">Структура файла</a-menu-item>
          <a-menu-item disabled key="export">Экспорт</a-menu-item>
          <a-menu-item
            :disabled="!fileStore.isLoading"
            @click="Api.DownloadFile()"
            key="save"
            >Скачать</a-menu-item
          >
          <!--<a-menu-item-group title="Item 2">
              <a-menu-item key="setting:3">Option 3</a-menu-item>
              <a-menu-item key="setting:4">Option 4</a-menu-item>
            </a-menu-item-group> --->
        </a-sub-menu>
        <a-menu-item disabled key="2">Правка</a-menu-item>
        <a-sub-menu key="3">
          <template #title>Разное</template>
          <a-menu-item :disabled="!fileStore.isLoading" key="codepage"
            >Кодировка</a-menu-item
          >
        </a-sub-menu>
        <a-sub-menu key="4">
          <template #title>Статистика</template>
          <a-menu-item :disabled="!fileStore.isLoading" key="statistics"
            >Статистика...</a-menu-item
          >
        </a-sub-menu>
        <a-sub-menu key="5">
          <template #title>Помощь</template>
          <a-menu-item key="about">О сервисе</a-menu-item>
          <a-menu-item key="message">Сообщение автору</a-menu-item>
        </a-sub-menu>
      </a-menu>
      <!-- <div class="menu-right">
        <div>Вход</div>
      </div> -->
    </a-layout-header>
    <modal-component
      ref="modal"
      :activeComponentName="activeModalComponent"
      @closed="onClosedModal"
    ></modal-component>
    <a-layout-content id="content">
      <div class="subcontent">
        <dbfshow v-if="fileStore.isLoading == true"></dbfshow>
        <div v-else class="upload">
          <upload-file @upload-completed="onUploadCompleted"></upload-file>
          <a-spin :spinning="spinnerCheckFiles" size="large">
            <list-upload-files
              @selectedFile="onSelectedFile"
              :files="listUploadedFiles"
            ></list-upload-files>
          </a-spin>
        </div>
        <pagination v-if="fileStore.isLoading == true"></pagination>
      </div>
    </a-layout-content>
    <a-layout-content v-if="fileStore.isLoading == true" id="dopinfo">
      <div>
        <b>Колонок:</b> {{ fileStore.getCountColumns }} <b>Строк:</b>
        {{ fileStore.fileInfo.countRows }} <b> Кодировка:</b>
        <a @click="onClick({ key: 'codepage' })">{{ fileStore.getCodePage }}</a>
        <b> Формат:</b> {{ fileStore.fileInfo.version }}
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
