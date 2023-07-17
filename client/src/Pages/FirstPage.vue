<script setup>
import { ref } from "vue";
import TopMenu from "../components/TopMenu.vue";
import UploadFile from "../components/UploadFile.vue";
import ListUploadFiles from "../components/ListUploadFiles.vue";
import { showNotification } from "../plugins/notification";
import { useFileStore } from "../stores/filestore";
import Api from "../plugins/api";
import { HideFirstPage } from "../plugins/utils";
import { useRouter } from "vue-router";
import { useHead } from "@unhead/vue";

useHead({
  title: "DBF online просмотр и редактирование",
  meta: [
    {
      name: "description",
      content: "Онлайн просмотр DBase DB (.dbf) файлов и их редактирование. ",
    },
    {
      name: "keywords",
      content:
        "dbf, просмотр dbf online, редактирование dbf online,foxpro,dbase,структура dbf online,кодировка dbf online, онлайн",
    },
  ],
});

const fileStore = useFileStore();
const router = useRouter();

var listUploadedFiles = ref(null);

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
      router.push("dbfshow");
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
  router.push("dbfshow");
};
</script>

<template>
  <top-menu></top-menu>
  <main class="ant-layout-content" id="content">
    <div class="subcontent">
      <div class="page">
        <upload-file @upload-completed="onUploadCompleted"></upload-file>
        <list-upload-files @selectedFile="onSelectedFile"></list-upload-files>
        <p>
          Открыть dbf файлы онлайн теперь проще простого с помощью нашего сервиса. Больше
          не нужно скачивать специальное программное обеспечение для просмотра этого
          формата данных. Наш онлайн инструмент позволяет открывать и просматривать dbf
          файлы в любом браузере без каких-либо дополнительных установок.
        </p>

        <p>
          С помощью нашего сервиса вы можете легко просматривать и редактировать dbf
          файлы, а также экспортировать их в другие форматы, такие как Excel или CSV. Наш
          инструмент поддерживает все версии dbf файлов и гарантирует быстрый и надежный
          доступ к вашим данным.
        </p>

        <p>
          Кроме того, наш сервис полностью бесплатный и не требует регистрации. Вы можете
          начать использовать его прямо сейчас и получить доступ к своим dbf файлам в
          любое время и из любого места. Не откладывайте на потом, откройте dbf файлы
          онлайн уже сегодня с помощью нашего удобного инструмента!
        </p>
        В настоящий момент из дополнительных возможностей доступно:
        <ul>
          <li>Статистика файла</li>
          <li>
            Группировка по полю (в контекстном меню) - группирует по значению выбранной
            колонки, и дополнительно суммирует все числовые колонки
          </li>
        </ul>
        После внесения изменений в файл вы можете его скачать обратно к себе на компьютер
        через меню <b>Файл\Скачать</b>
      </div>
    </div>
  </main>
</template>
