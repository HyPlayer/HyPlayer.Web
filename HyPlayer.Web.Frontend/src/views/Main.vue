<script lang="ts">
import Header from '../components/Header.vue'
import Footer from '../components/Footer.vue'
import { defineComponent } from 'vue';
export default defineComponent({
    name: "Main",
    components: {
        Footer: Footer,
        HeaderComp: Header
    },
})
</script>

<template>
    <div>
        <el-container class="page-container">
            <el-header>
                <HeaderComp />
            </el-header>
            <el-main>
                <RouterView />
            </el-main>
            <el-footer class="hy-footer">
                <component :is="'Footer'" />
            </el-footer>
        </el-container>
    </div>
</template>

<style scoped>
.page-container {
    min-width: 100%;
    max-width: 100%;

    min-height: 98vh;
}

.hy-footer {
    max-height: 35px;
}
</style>
