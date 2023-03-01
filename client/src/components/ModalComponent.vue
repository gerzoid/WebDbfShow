<script setup>
import { ref, onMounted, watch } from "vue";
import About from "./Modal/About.vue";
import Codepage from "./Modal/Codepage.vue";
import Statistics from "./Modal/Report/Statistics.vue";

const props = defineProps({ activeComponentName: null });
const emit = defineEmits(["closed"]);

var visible = ref(false);

const componentList = {
  About: {
    name: About,
    title: "О сервисе",
    width: "500px",
  },
  Codepage: {
    name: Codepage,
    title: "Изменить кодировку",
    width: "500px",
  },
  Statistics: {
    name: Statistics,
    title: "Статистика",
    width: "600px",
  },
};

var selectedComponent = "About";
var activeComponent = componentList[selectedComponent].name;
var activeTittle = componentList[selectedComponent].title;
var componentWidth = componentList[selectedComponent].width;

onMounted(() => {});

var componentKey = 0;

watch(
  () => props.activeComponentName,
  () => {
    if (props.activeComponentName != null) {
      activeComponent = componentList[props.activeComponentName].name;
      activeTittle = componentList[props.activeComponentName].title;
      componentWidth = componentList[props.activeComponentName].width;
      componentKey += 1;
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
    :width="componentWidth"
  >
    <!-- <component :is="activeComponent" :key="componentKey" ></component> -->
    <component :is="activeComponent" :key="componentKey"></component>
    <template #footer>
      <a-button key="submit" type="primary" @click="handleOk">Закрыть</a-button>
    </template>
  </a-modal>
</template>
