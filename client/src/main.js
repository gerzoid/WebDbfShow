import { createApp } from 'vue'
import App from './App.vue'
import  router from './plugins/router'
import Antd from 'ant-design-vue';
import { createPinia } from 'pinia'
import { createHead } from '@unhead/vue'
import './style.css'
import 'ant-design-vue/dist/antd.css';

const head = createHead();

let Pinia = createPinia();

createApp(App).
    use(router).
    use(head).
    use(Pinia).
    use(Antd).
    mount('#app');