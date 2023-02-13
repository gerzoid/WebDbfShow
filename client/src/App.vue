<script setup>
import { defineComponent, ref, watch } from 'vue';
import UploadFile from './components/UploadFile.vue'
import Dbfshow from './components/DbfShow.vue'
import { storeToRefs } from 'pinia'
import { useFileStore } from './stores/filestore'

var selectedKeys= ref(['2']);
const fileStore = useFileStore();

let isLoaded =ref(false);

var onUploadCompleted =(data)=>{
  fileStore.fileInfo = data;
  fileStore.fileName = data.name;
  isLoaded.value = true;
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
          <a-menu-item key="1">Файл</a-menu-item>
          <a-menu-item key="2">Правка</a-menu-item>
        </a-menu>
      </a-layout-header>
      <a-layout-content id='content'>
        <div class="subcontent">
            <!---<dbfshow v-if="isLoaded==true" :info="fileInfo"></dbfshow>-->
            <dbfshow v-if="isLoaded==true"></dbfshow>
            <div v-else class="upload">
                <upload-file @upload-completed="onUploadCompleted"></upload-file>
            </div>
        </div>
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