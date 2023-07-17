import { createApp } from 'vue'
import App from './App.vue'
import  router from './plugins/router'
import Antd from 'ant-design-vue';
import { createPinia } from 'pinia'
import './style.css'

import 'ant-design-vue/dist/antd.css';

let Pinia = createPinia();
createApp(App).use(router).use(Pinia).use(Antd).mount('#app');