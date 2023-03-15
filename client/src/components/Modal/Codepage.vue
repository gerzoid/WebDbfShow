<script setup>
import { codepages } from "../../plugins/codepages";
import { useFileStore } from "../../stores/filestore";
import { onMounted, computed, ref } from "vue";
import Api from "../../plugins/api";
import { createTokenInstance } from "chevrotain";

const fileStore = useFileStore();

var codepage = Number(fileStore.fileInfo.codePageId);

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
      {{ fileStore.getCodePage }}<br />

      <a-select
        ref="select"
        v-model:value="codepage"
        style="width: 100%"
        @change="handleChange"
      >
        <a-select-option
          v-for="(item, index) in codepages"
          :value="item.code"
          :key="index"
          >{{ item.description }}
        </a-select-option>
      </a-select>
    </div>
  </div>
</template>
