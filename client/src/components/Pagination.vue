<script setup>
import { ref, watch } from "vue";
import { useFileStore } from "../stores/filestore";

const fileStore = useFileStore();

const pageSize = ref(fileStore.options.pageSize);
const current1 = ref(fileStore.options.page);
const totalRecord = ref(fileStore.fileInfo.countRows);

watch(pageSize, () => {
  fileStore.options.pageSize = pageSize.value;
  fileStore.options.page = 1;
  current1.value = 1;
});

watch(current1, () => {
  fileStore.options.page = current1.value;
});
</script>

<template>
  <div>
    <a-pagination
      show-size-changer
      v-model:current="current1"
      v-model:pageSize="pageSize"
      v-model:total="totalRecord"
    />
  </div>
</template>
