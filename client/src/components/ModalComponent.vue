<script setup>
import { ref, onMounted, watch } from "vue";
import About from "./Modal/About.vue";
import Codepage from "./Modal/Codepage.vue";

const props = defineProps({ activeComponentName: null });
const emit = defineEmits(["closed"]);

var visible = ref(false);

const componentList = { About: About, Codepage: Codepage };
const componentTitle = { About: "О сервисе", Codepage: "Изменить кодировку" };

var selectedComponent = "About";

var activeComponent = componentList[selectedComponent];
var activeTittle = componentTitle[selectedComponent];

onMounted(() => {});

watch(
  () => props.activeComponentName,
  () => {
    if (props.activeComponentName != null) {
      activeComponent = componentList[props.activeComponentName];
      activeTittle = componentTitle[props.activeComponentName];
      visible.value = true;
    } else visible.value = false;
  }
);

function handleOk() {
  emit("closed");
}
</script>
<template>
  <a-modal v-model:visible="visible" :title="activeTittle" @ok="handleOk">
    <component :is="activeComponent"></component>
  </a-modal>
</template>
