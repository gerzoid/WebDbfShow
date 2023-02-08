<script setup>

import { InboxOutlined } from '@ant-design/icons-vue';
import { message } from 'ant-design-vue';
import { ref } from 'vue';

    var fileList =  ref([]);

    var handleDrop= e => {
        console.log(e);
    };

   var handleChange = info => {
      const status = info.file.status;
      if (status !== 'uploading') {
        console.log(info.file, info.fileList);
      }
      if (status === 'done') {
        message.success(`${info.file.name} file uploaded successfully.`);
      } else if (status === 'error') {
        message.error(`${info.file.name} file upload failed.`);
      }
    };

</script>

<template>
 <a-upload-dragger
    v-model:fileList="fileList"
    name="file"
    :multiple="true"
    action="https://localhost:7036/api/Files"
    @change="handleChange"
    @drop="handleDrop">

    <p class="ant-upload-drag-icon">
      <inbox-outlined></inbox-outlined>
    </p>
    <p class="ant-upload-text">Щелкните мышкой или перетащите файл в эту область для его загрузки</p>
    <p class="ant-upload-hint">
      Поддерживаются dbf, csv файлы и txt с разделителями (аналог csv)
    </p>
  </a-upload-dragger>
</template>

<style scoped>
    .upload-dbf{
        width:400px;
        display: block;
        margin-left: auto;
        margin-right: auto;
        background-color: white;
        padding-top: 50px;
    }
</style>