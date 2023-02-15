<script setup>
    import { ref, watch } from 'vue';
    import { useFileStore } from '../stores/filestore'

    const fileStore = useFileStore();

    const pageSize = ref(fileStore.pageSize);
    const current1 = ref(fileStore.page);
    const totalRecord = ref(fileStore.fileInfo.countRows);

    console.log(fileStore.fileInfo.countRows);
    watch(pageSize, () => {
        fileStore.page = pageSize.value;
    });

    watch(current1, () => {
        fileStore.page = current1.value;
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