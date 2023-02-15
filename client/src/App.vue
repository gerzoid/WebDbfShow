<script setup>
import { defineComponent, ref, watch } from 'vue';
import UploadFile from './components/UploadFile.vue'
import Dbfshow from './components/DbfShow.vue'
import { storeToRefs } from 'pinia'
import { message} from 'ant-design-vue';
import { showNotification } from './plugins/notification'
import { useFileStore } from './stores/filestore'

var selectedKeys= ref([]);
const fileStore = useFileStore();

let isLoaded =ref(false);

var onUploadCompleted =(data)=>{
  fileStore.fileInfo = data;
  fileStore.fileName = data.name;
  isLoaded.value = true;
  showNotification('success', 'Загрузка файлов', 'Файл успешно загружен')
}
</script>

<template>
    <a-layout class="layout">
      <a-layout-header>
        <div class="logo" />
        <a-menu
          v-model:selectedKeys="selectedKeys"
          theme="dark"
          mode="horizontal"
          :style="{ lineHeight: '64px' }"
        >
      <a-sub-menu key="sub1">
          <template #icon>
              <setting-outlined />
          </template>
          <template selected #title>Файл</template>
          <a-menu-item key="1">Закрыть</a-menu-item>
          <a-menu-item disabled key="1-1">Структура файла</a-menu-item>
          <a-menu-item disabled key="1-2">Экспорт</a-menu-item>
          <!--<a-menu-item-group title="Item 2">
            <a-menu-item key="setting:3">Option 3</a-menu-item>
            <a-menu-item key="setting:4">Option 4</a-menu-item>
          </a-menu-item-group> --->
          </a-sub-menu>
          <a-menu-item disabled key="2">Правка</a-menu-item>
          <a-menu-item disabled key="3">Статистика</a-menu-item>
        </a-menu>
      </a-layout-header>
      <a-layout-content id='content'>
        <div class="subcontent">
            <dbfshow v-if="isLoaded==true"></dbfshow>
            <div v-else class="upload">
                <upload-file @upload-completed="onUploadCompleted"></upload-file>
            </div>
        </div>
      </a-layout-content>
      <a-layout-content v-if="isLoaded==true" id='dopinfo'>
      <div><b>Колонок:</b>  {{ fileStore.fileInfo.countColumns }} <b>Строк:</b> {{ fileStore.fileInfo.countRows }} <b>Кодировка:</b> {{ fileStore.fileInfo.codePage }} <b>Формат:</b> {{ fileStore.fileInfo.version }}</div>
    </a-layout-content>
      <a-layout-footer style="text-align: center">
        jobtools.ru ©2023
      </a-layout-footer>
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

  .upload{
    max-width: 50%;
    margin: 0 auto;
  }

  [data-theme='dark'] .site-layout-content {
    background: #141414;
  }
  </style>