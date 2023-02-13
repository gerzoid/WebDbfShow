import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { useFileStore } from './stores/filestore';
import './style.css'
import Antd from 'ant-design-vue';
import 'ant-design-vue/dist/antd.css';
import App from './App.vue'

let Pinia = createPinia();
createApp(App).use(Pinia).use(Antd).mount('#app');
