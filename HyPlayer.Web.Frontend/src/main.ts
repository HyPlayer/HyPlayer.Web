import { createApp } from 'vue'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import 'element-plus/theme-chalk/dark/css-vars.css'
import './style.css'
import App from './App.vue'
import {router} from "./configs/vue-router"
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
//import { createPinia } from 'pinia'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import {FaLibrary} from "./configs/font-awesome"
// @ts-ignore
import VMdPreview from '@kangc/v-md-editor/lib/preview';
import '@kangc/v-md-editor/lib/style/preview.css';
// @ts-ignore
import githubTheme from '@kangc/v-md-editor/lib/theme/github.js';
import '@kangc/v-md-editor/lib/theme/style/github.css';

// @ts-ignore
import hljs from 'highlight.js';

VMdPreview.use(githubTheme, {
    Hljs: hljs,
});


let app = createApp(App);
/*
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}
 */
FaLibrary.add()
app.component('font-awesome-icon', FontAwesomeIcon)
app.use(router).use(VMdPreview).use(ElementPlus).mount('#app')
