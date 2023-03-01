<script setup>
import { ref, onMounted, watch } from "vue";
import About from "./Modal/About.vue";
import Codepage from "./Modal/Codepage.vue";
import Statistics from "./Modal/Report/Statistics.vue";

const props = defineProps({ activeComponentName: null });
const emit = defineEmits(["closed"]);

var visible = ref(false);

const componentList = { About: About, Codepage: Codepage, Statistics: Statistics };

const componentTitle = {
  About: "О сервисе",
  Codepage: "Изменить кодировку",
  Statistics: "Статистика",
};

var selectedComponent = "About";

var activeComponent = componentList[selectedComponent];
var activeTittle = componentTitle[selectedComponent];

onMounted(() => {});

var componentKey=0;

watch(
  () => props.activeComponentName,
  () => {
    if (props.activeComponentName != null) {
      activeComponent = componentList[props.activeComponentName];
      activeTittle = componentTitle[props.activeComponentName];
      componentKey+=1;
      visible.value = true;
    } else visible.value = false;
  }
);

function handleOk() {
  emit("closed");
}
</script>
<template>
  <a-modal
    v-model:visible="visible"
    :title="activeTittle"
    @ok="handleOk"
    @cancel="handleOk"
  >
    <component :is="activeComponent" :key="componentKey"></component>
    <template #footer>
      <a-button key="submit" type="primary" @click="handleOk">Закрыть</a-button>
    </template>
  </a-modal>
</template>
