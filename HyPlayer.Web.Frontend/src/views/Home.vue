<template>
  <el-row justify="center" :gutter="8">
    <el-col
        :xs="12"
        :sm="12"
        :md="6"
    >
      <el-image src="https://s2.loli.net/2022/07/24/vwmY7t19uXLHPOr.png"/>
      <div class="hy-main-title">HyPlayer</div>
      <div class="hy-sub-title">
        开源 | 简洁 | 强大<br/>
        仅供学习交流使用
      </div>
      <div class="hy-icon-group">
        <el-button @click="gotoGithub" round>
          <font-awesome-icon size="xl" icon="fa-brands fa-github"/>&nbsp;
          Github
        </el-button>
        <el-button @click="gotoMSStore" round>
          <font-awesome-icon size="xl" icon="fa-brands fa-microsoft"/>&nbsp;
          Microsoft Store
        </el-button>
        <el-button @click="gotoAzure" round>
          <font-awesome-icon size="xl" icon="fa-solid fa-server"/>&nbsp;
          Azure Pipeline
        </el-button>
      </div>
      <RouterLink to="/insider">
        <el-button class="hy-btn-insider-apply" type="primary">申请内测</el-button>
      </RouterLink>
    </el-col>
  </el-row>

</template>

<script lang="ts">
export default {
  name: "Home",
  methods:{
    gotoGithub(){
      window.location.href = "https://github.com/HyPlayer/HyPlayer"
    },
    gotoMSStore(){
      window.location.href = "https://www.microsoft.com/store/productId/9N5TD916686K"
    },
    gotoAzure(){
      window.location.href = "https://dev.azure.com/kengwang/HyPlayer/_build/latest?definitionId=29&branchName=develop"
    }
  }
}
</script>

<style scoped>
.hy-main-title {
  font-weight: 400;
  font-size: 56px;
  text-align: center !important;
}

.hy-sub-title {
  font-weight: 400;
  font-size: 24px;
  text-align: center !important;
}

.hy-icon-group {
  margin-top: 15px;
}

.hy-btn-insider-apply {
  width: 100%;
  margin-top: 15px;
}
</style>