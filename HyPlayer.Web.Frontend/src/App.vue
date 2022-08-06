<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
import Header from './components/Header.vue'
import Footer from './components/Footer.vue'
import Main from "./views/Main.vue";
</script>

<template>
  <div>
    <el-container class="page-container">
      <el-header>
        <Header/>
      </el-header>
      <el-main>
        <Main/>
      </el-main>
      <el-footer class="hy-footer">
        <Footer/>
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

.hy-footer{
  max-height: 35px;
}
</style>
