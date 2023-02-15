<script setup>

import { InboxOutlined } from '@ant-design/icons-vue';
import axios from 'axios'
import { message } from 'ant-design-vue';
import { ref } from 'vue';
import { useFileStore } from '../stores/filestore'

const emit = defineEmits(['upload-completed']);

var fileList =  ref([]);
const fileStore = useFileStore();

var uploadFiles = ({ onSuccess, onError, file })=>
{
  var formData = new FormData();
    formData.append('formfile', file);
    formData.append('filename', file.name);
    fileStore.originalFileName = file.name;
    axios.post('http://localhost:5149/api/Files', formData, {
      headers: {
        'Content-Type' : 'multipart/form-data'
      }
    }).then((data)=>{
      onSuccess(null, file);
      emit('upload-completed', data.data);
    }).catch(e=>{
    message.error(e);
  });
}
</script>

<template>

 <a-upload-dragger
    v-model:fileList="fileList"
    name="file"
    accept=".dbf"
    :multiple="false"
    :customRequest="uploadFiles">

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