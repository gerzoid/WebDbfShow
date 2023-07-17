<script setup>
import { useRouter } from "vue-router";

const router = useRouter();
</script>
<template>
  <nav class="menu">
    <ul class="menu-list">
      <li class="menu-item"><router-link to="/history">История dbf</router-link></li>
      <li class="menu-item"><router-link to="/formats">Формат dbf</router-link></li>
      <li class="menu-item"><router-link to="/programms">Чем открыть</router-link></li>
      <li class="menu-item special"><router-link to="/dbfshow">Онлайн</router-link></li>
      <li class="menu-item"><router-link to="/about">О нас</router-link></li>
    </ul>
  </nav>
</template>
<style>
.menu {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 8px;
  background-color: black;
}

.menu-list {
  list-style: none;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 0;
  margin: 0;
}

.menu-item {
  margin: 0 10px;
  cursor: pointer;
  font-size: 16px;
}

.menu-item.special {
  font-size: 20px;
}
.menu-item.special > a {
  color: greenyellow;
}

.menu-item > a {
  color: #c7c7c7;
}

a.router-link-active,
li.router-link-active > a {
  color: white;
  font-weight: bold;
}
</style>
