import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { useFileStore } from './stores/filestore';
import './style.css'
import Antd from 'ant-design-vue';
import 'ant-design-vue/dist/antd.css';
import App from './App.vue'
import Router from './router';
import FirstPage from './FirstPage.vue'

let Pinia = createPinia();
createApp(App).use(Router).use(Pinia).use(Antd).mount('#app');

createApp(FirstPage).use(Router).use(Pinia).use(Antd).mount('#app2');

