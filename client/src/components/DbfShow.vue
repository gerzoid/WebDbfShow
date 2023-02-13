<script setup>

import { HotTable } from '@handsontable/vue3';
import axios from 'axios'
import 'handsontable/dist/handsontable.full.min.css';
import { ref, watch  } from 'vue';

const props = defineProps(['info']);

var hot = ref();
var data= [];

let fileName=ref('');

function getData(){
  const data = new FormData();
  data.append('FileName', this.filename);
  data.append('PageSize', 10);
  data.append('Page', 1);
  axios.post('http://localhost:5149/api/Files/getData', data).then(({ data })=>{
       data = data.data;
  });
}

var settings = {
  licenseKey: 'non-commercial-and-evaluation',
  columns: props.info.columns,
  stretchH: 'all',
}

</script>

<template>
  <hot-table ref='hot' :data="data" :rowHeaders="true" :colHeaders="true" :settings="settings"></hot-table>
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