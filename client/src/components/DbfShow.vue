<script setup>

import { HotTable } from '@handsontable/vue3';
import 'handsontable/dist/handsontable.full.min.css';
import axios from 'axios'
import { registerAllModules } from 'handsontable/registry';
import { ref, watch, onMounted  } from 'vue';

registerAllModules();
const props = defineProps(['columns']);
var hot = ref(null);

var data= props.columns;

watch(props, (columns)=>{
  data= props.columns;
  console.log(columns);
console.log(this);
});

onMounted(()=>{
    hot = getCurrentInstance().ctx.$refs.hot;
    console.log('onMounted');
});

var settings = {
  licenseKey: 'non-commercial-and-evaluation',
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