import {createRouter, createWebHistory} from 'vue-router';
import NotFoundPage from '../Pages/NotFoundPage.vue'
import DbfShowPage from '../Pages/DbfShowPage.vue'
import AboutPage from '../Pages/AboutPage.vue'
import FirstPage from '../Pages/FirstPage.vue'

const routes = [
    { path: '/', component: FirstPage },
    { path: '/about', component: AboutPage },
    { path: '/dbfshow', component: DbfShowPage },
    { path: '/:pathMatch(.*)*', component: NotFoundPage },
];

const router = new createRouter({
    history: createWebHistory(),
    routes
});

export default router;