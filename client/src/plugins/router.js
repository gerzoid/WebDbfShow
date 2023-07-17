import {createRouter, createWebHistory} from 'vue-router';
import NotFoundPage from '../pages/NotFoundPage.vue'
import DbfShowPage from '../pages/DbfShowPage.vue'
import AboutPage from '../pages/AboutPage.vue'
import FormatsPage from '../pages/FormatsPage.vue'
import ProgrammsPage from '../pages/ProgrammsPage.vue'
import HistoryPage from '../pages/HistoryPage.vue'
import FirstPage from '../pages/FirstPage.vue'


const routes = [
    { path: '/', component: FirstPage },
    { path: '/about', component: AboutPage },
    { path: '/programms', component: ProgrammsPage },
    { path: '/formats', component: FormatsPage },
    { path: '/history', component: HistoryPage },
    { path: '/dbfshow', component: DbfShowPage },
    { path: '/:pathMatch(.*)*', component: NotFoundPage },
];

const router = new createRouter({
    history: createWebHistory(),
    routes
});

export default router;