import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'
const Home = () => import("../views/Home.vue")
const Version = () => import("../views/Version.vue")
const VersionAuth = () => import("../views/VersionAuth.vue")
const Insider = () => import("../views/Insider.vue")


const routes : Array<RouteRecordRaw> = [
    {
      path: '/',
      redirect:'/index'
    },
    {
        path: '/index',
        name: 'home',
        component: Home
    },
    {
        path: '/channel/:channel/latest/:userId',
        name: 'appLatestVersion',
        component: Version
    },
    {
        path: '/channel/:channel/latest',
        name: 'appLatestVersionAuthWithId',
        component: VersionAuth
    },
    {
        path: '/channel/latest',
        name: 'appLatestVersionAuth',
        component: VersionAuth
    },
    {
        path: '/insider',
        name: 'insider',
        component: Insider
    }
]

export const router = createRouter({
    // 4. 内部提供了 history 模式的实现。为了简单起见，我们在这里使用 hash 模式。
    history: createWebHashHistory(),
    routes: routes, // `routes: routes` 的缩写
})