<script setup>
import { ref, watch } from "vue";
import { useFileStore } from "../stores/filestore";

const fileStore = useFileStore();
var selectedKeys = ref([]);

function onClick(e) {
  switch (e.key) {
    case "close":
      fileStore.closeFile();
      break;
    case "about":
      fileStore.activeModalComponent = "About";
      break;
    case "codepage":
      fileStore.activeModalComponent = "Codepage";
      break;
    case "statistics":
      fileStore.activeModalComponent = "Statistics";
      break;
    case "message":
      fileStore.activeModalComponent = "MessageAuthor";
      break;
  }
}
</script>

<template>
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
      >
        Скачать
      </a-menu-item>
    </a-sub-menu>
    <a-menu-item disabled key="2">Правка</a-menu-item>
    <a-sub-menu key="3">
      <template #title>Разное</template>
      <a-menu-item :disabled="!fileStore.isLoading" key="codepage">Кодировка</a-menu-item>
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
</template>
