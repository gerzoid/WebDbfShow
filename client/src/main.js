/*import { createApp } from 'vue'
import { createPinia } from 'pinia'
import './style.css'
import Antd from 'ant-design-vue';
import 'ant-design-vue/dist/antd.css';
import App from './App.vue'

import FirstPage from './FirstPage.vue'

let Pinia = createPinia();
createApp(App).use(Pinia).use(Antd).mount('#app');

createApp(FirstPage).use(Pinia).use(Antd).mount('#app2');
*/

import { createApp } from 'vue'
import App from './App.vue'
import  router from './plugins/router'
import Antd from 'ant-design-vue';
import { createPinia } from 'pinia'
import './style.css'

import 'ant-design-vue/dist/antd.css';

let Pinia = createPinia();
createApp(App).use(router).use(Pinia).use(Antd).mount('#app');

//createApp(FirstPage).use(Pinia).use(Antd).mount('#app2')