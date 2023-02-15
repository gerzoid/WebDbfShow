<script setup>

import { HotTable } from '@handsontable/vue3';
import axios from 'axios'
import { useFileStore } from '../stores/filestore'
import { storeToRefs } from 'pinia'
import 'handsontable/dist/handsontable.full.min.css';
import { ref, watch, toRaw, onMounted  } from 'vue';

const fileStore = useFileStore();

var hot = ref(null);

var dataRow= ref([]);

var settings = ref({
  licenseKey: 'non-commercial-and-evaluation',
  columns: toRaw(fileStore.fileInfo.columns),
  stretchH: 'all',
})

onMounted(()=>{
  //console.log(hot.value.hotInstance.loadData(['1','2','4']));
});

//Не работает так как v-if на компоненте, а не v-show
/*const { fileName } = storeToRefs(fileStore)
watch(fileName, () => {
  settings.columns = toRaw(fileStore.fileInfo.columns);
  console.log('some changed', fileName)
})*/

function getData(){
  const data = new FormData();
  data.append('FileName', fileStore.fileInfo.name);
  data.append('PageSize', 10);
  data.append('Page', 1);
  axios.post('http://localhost:5149/api/Files/getData', data)
    .then(result=>{
      dataRow = result; 
      console.log(result.data);
      hot.value.hotInstance.loadData(result.data);
  });
}

getData();

</script>

<template>
  <hot-table ref='hot' :data="dataRow" :rowHeaders="true" :colHeaders="true" :settings="settings"></hot-table>
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