<script setup>
import { ref, onMounted } from "vue";
import Api from "../../plugins/api";
import { showNotification } from "../../plugins/notification";

const textarea = ref(null);
var message = ref("");

onMounted(() => {
  textarea.value.$el.focus();
});

function onClick() {
  Api.SendMessage(message)
    .then((result) => {
      showNotification("success", "", "Сообщение успешно отправлено", 2);
      message.value = "";
    })
    .catch((e) => {
      console.log(e);
      showNotification(
        "error",
        "Ошибка",
        "Сообщение не отправлено. Пдробности в консоли",
        2
      );
    });
}
</script>
<template>
  <div class="card">
    <header class="card-header">
      <p class="card-header-title">Отправка сообщения разработчику</p>
    </header>
    <div class="content" style="padding: 10px"></div>
    <a-textarea ref="textarea" v-model:value="message" style="margin-bottom: 5px" />
    <a-button @click="onClick()" block>Отправить</a-button>
  </div>
</template>
