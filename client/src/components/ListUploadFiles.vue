<script setup>
import Api from "../plugins/api";
import { ref, watch, onMounted } from "vue";
import { useFileStore } from "../stores/filestore";
import { setCookie } from "../plugins/cookies";
import moment from "moment";

const fileStore = useFileStore();

//const props = defineProps({ files: null });
var files = ref(null);
const emit = defineEmits(["selectedFile"]);
var hasLoadedFiles = false;
var spinnerCheckFiles = ref(false);

//Проверитьь список загруженных файлов по юзеру
function CheckUploadedFiles() {
  spinnerCheckFiles.value = true;
  Api.CheckUploadedFiles()
    .then((result) => {
      let date = new Date();
      date = new Date(date.setMonth(date.getMonth() + 8));
      setCookie("dbfshowuser", result.data.usersId, { expiries: date.toUTCString() });
      fileStore.userId = result.data.usersId;
      files.value = result.data.files;
      hasLoadedFiles = true;
    })
    .catch((e) => {
      console.log(e);
    })
    .finally(() => (spinnerCheckFiles.value = false));
}

onMounted(() => {
  CheckUploadedFiles();
});

function onClick(file) {
  emit("selectedFile", file.filesId, file.originalName);
}
</script>

<template>
  <a-spin :spinning="spinnerCheckFiles" size="large">
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
  </a-spin>
</template>

<style>
.uploadedfiles {
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
