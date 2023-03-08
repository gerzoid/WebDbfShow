<script setup>
import { watch } from "vue";
import { useFileStore } from "../stores/filestore";
import moment from "moment";

const fileStore = useFileStore();

const props = defineProps({ files: null });
const emit = defineEmits(["selectedFile"]);

var hasLoadedFiles = false;

watch(
  () => props.files,
  () => {
    if (props.files.length > 0) hasLoadedFiles = true;
    else hasLoadedFiles = false;
  }
);

function onClick(file) {
  emit("selectedFile", file.filesId, file.originalName);
}
</script>

<template>
  <div class="uploadedfiles" v-if="hasLoadedFiles == true">
    <div class="zagolovok">
      Ранее загруженные файлы, доступные для дальнейшего редактирования
    </div>
    <div :key="file.fileId" v-for="(file, index) in files" class="row">
      <span>
        {{ index + 1 }}. Файл <b> {{ file.originalName }} </b> от
        {{ moment(file.createdAt).format("DD.MM.YYYY hh:mm") }}
      </span>
      <span><a @click="onClick(file)">Открыть</a></span>
    </div>
  </div>
  <div v-else class="zagolovok">Загруженные файлы отсутствуют</div>
</template>

<style>
.uploadedfiles{
  min-height: 130px;
}
.row {
  padding: 0px 0px;
  width: 100%;
}
.row span {
  padding: 0px 10px;
  display: inline-block;
}
.zagolovok {
  text-align: center;
  margin: 30px 0px 15px 0px;
}
</style>
