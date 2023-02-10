<script setup>
import { defineComponent, ref } from 'vue';
import UploadFile from './components/UploadFile.vue'
    var selectedKeys= ref(['2']);
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
            <div class="upload">
                <upload-file></upload-file>
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