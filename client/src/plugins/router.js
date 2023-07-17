import {createRouter, createWebHistory} from 'vue-router';
import NotFoundPage from '../Pages/NotFoundPage.vue'
import DbfShowPage from '../Pages/DbfShowPage.vue'
import AboutPage from '../Pages/AboutPage.vue'
import FormatsPage from '../Pages/FormatsPage.vue'
import ProgrammsPage from '../Pages/ProgrammsPage.vue'
import HistoryPage from '../Pages/HistoryPage.vue'
import FirstPage from '../Pages/FirstPage.vue'


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