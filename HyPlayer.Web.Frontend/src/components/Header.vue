<template>
  <div class="header">
    <el-menu
        :ellipsis="false"
        mode="horizontal">
      <el-menu-item>
        <el-image fit="scale-down" class="header-logo" src="https://s2.loli.net/2022/07/24/vwmY7t19uXLHPOr.png"
                  alt="HyPlayer"/>
      </el-menu-item>
      <div>
        <div class="hy-title">HyPlayer</div>
        <div class="hy-subtitle">第三方网易云音乐 UWP 播放器</div>
      </div>
      <div class="flex-grow"/>
      <el-menu-item>
        登入 (暂不支持)
      </el-menu-item>
    </el-menu>
  </div>
</template>

<script lang="ts">
export default {
  name: "Header"
}
</script>

<style scoped>
.header-logo {
  max-height: 60px;
  max-width: 60px;
}

.flex-grow {
  flex-grow: 1;
}

.header {
  width: 100%;
}

.hy-title {
  font-size: var(--el-font-size-extra-large);
}

.hy-subtitle{
  color: #909399;
}
</style>