<script setup>
import { codepages } from "../../plugins/codepages";
import { useFileStore } from "../../stores/filestore";
import { ref } from "vue";
import Api from "../../plugins/api";

const fileStore = useFileStore();
var codepage = ref(fileStore.fileInfo.codePageId);

const handleChange = (value) => {
  Api.SetEncoding(value)
    .then((result) => {
      fileStore.fileInfo.codePageId = value;
      fileStore.needReload = true;
    })
    .catch((e) => {
      console.log(e);
    });
};
</script>
<template>
  <div class="card">
    <div class="content" style="padding: 10px">
      Текущая кодировка файла:
      {{ codepages[fileStore.fileInfo.codePageId] }}<br />

      <a-select
        ref="select"
        v-model:value="codepage"
        style="width: 100%"
        @change="handleChange"
      >
        <a-select-option v-for="(value, key, index) in codepages" v-bind:value="key"
          >{{ value }}
        </a-select-option>
      </a-select>
    </div>
  </div>
</template>
