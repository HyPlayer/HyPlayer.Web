<template>
  <el-row justify="center">
    <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="8">
      <el-card class="box-card">
        <template #header>
          <div class="card-header">
            需要执行一些操作<br/>
            请输入您的微软账号邮箱
          </div>
        </template>
        后续更新可在此页面获取
        <br /><br />
        <el-input v-model="email" placeholder="请输入微软账户邮箱" clearable/>
        <el-button type="primary" @click="submitEmail" class="hy-btn-submit">提交</el-button>
        <el-alert class="hy-alert" v-if="error" :title="errorMsg" type="error" show-icon/>
      </el-card>
    </el-col>
  </el-row>
</template>

<script lang="ts">
import axios from 'axios'
import {defineComponent} from "vue";

export default defineComponent({
  name: "VersionAuth",
  data: () => ({
    email: '',
    errorMsg: '',
    error: false
  }),
  methods: {
    submitEmail() {
      axios.get('/user/email/' + this.email)
          .then(data => {
            this.$router.push('/channel/' + this.$route.params.channel + '/latest/' + data.data.id)
          })
          .catch(error => {
            this.error = true
            this.errorMsg = error.message + ': ' + error.response.data
            this.errorMsg += error.response.data.detail
          });
    }
  }
})
</script>

<style scoped>
.card-header {
  text-align: center;
  width: 100%;
}

.hy-btn-submit {
  width: 100%;
  margin-top: 13px;
}

.hy-alert {
  margin-top: 24px;
}
</style>