<script setup>
import { ref, onMounted, watch } from "vue";
import About from "./Modal/About.vue";
import CountUnique from "./Modal/Report/CountUnique.vue";
import CountValue from "./Modal/Report/CountValue.vue";
import Codepage from "./Modal/Codepage.vue";
import MessageAuthor from "./Modal/MessageAuthor.vue";
import Statistics from "./Modal/Report/Statistics.vue";
import Group from "./Modal/Report/Group.vue";

const props = defineProps({ activeComponentName: null });
const emit = defineEmits(["closed"]);

var visible = ref(false);

const componentList = {
  About: {
    name: About,
    title: "О сервисе",
    width: "500px",
  },
  Group: {
    name: Group,
    title: "Группировка",
    width: "600px",
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
  CountUnique: {
    name: CountUnique,
    title: "Количество уникальных записей",
    width: "500px",
  },
  CountValue: {
    name: CountValue,
    title: "Количество уникальных записей со значением",
    width: "500px",
  },
  MessageAuthor: {
    name: MessageAuthor,
    title: "Сообщение",
    width: "500px",
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
