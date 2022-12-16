<template>
  <el-row justify="center">
    <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="8">
      <el-card class="box-card">
        <template #header>
          <div class="card-header">
            内测申请 / 修改
          </div>
        </template>
        <el-input class="hy-form-item" v-model="userData.username" placeholder="请输入用户名" clearable/>
        <el-input class="hy-form-item" v-model="userData.email" type="email" placeholder="请输入微软账号" clearable/>
        <el-input class="hy-form-item" v-model="userData.contact" type="email" placeholder="请输入联系邮箱" clearable/>
        <el-row class="hy-form-item" style="width: 100%">
          <el-col :xs="24" :sm="24" :lg="12" :xl="12">
            <div>更新通道:</div>
          </el-col>
          <el-col :xs="24" :sm="24" :lg="12" :xl="12">
            <el-select v-model="userData.channel" placeholder="更新通道">
              <el-option
                  v-for="item in options"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                  :disabled="item.disabled"
              />
            </el-select>
          </el-col>
        </el-row>
        <el-switch
            v-model="userData.subscribe"
            class="mb-2"
            active-text="接受邮箱更新通知"
        />
        <el-button type="primary" v-if="!loading" @click="submit" class="hy-btn-submit">提交</el-button>
        <el-alert v-if="!loading" class="hy-alert" title="如需修改信息, 请保持微软账号不变, 而后可修改其他信息" show-icon/>
        <el-alert class="hy-alert" title="请勿使用 QQ 邮箱作为联系邮箱, 否则将无法收到相关邮件" show-icon/>
        <el-progress v-if="loading" :show-text="false" :percentage="100" :indeterminate="true" />
        <el-alert class="hy-alert" v-if="error" :title="errorMsg" type="error" show-icon/>
        <el-alert class="hy-alert" v-if="success" title="申请成功: 点击下方链接获取最新版本" type="success" show-icon/>
        <el-button type="primary" v-if="success" class="hy-form-item" @click="getLatest">获取最新版本</el-button>
      </el-card>
    </el-col>
  </el-row>
</template>

<script lang="ts">
import {defineComponent} from "vue";
import axios from "axios"

export default defineComponent({
  name: "Insider",
  data: () => ({
    success: false,
    loading: false,
    error: false,
    errorMsg: '',
    userData: {
      username: '',
      email: '',
      contact: '',
      channel: 2,
      subscribe: true
    },
    options: [
      {
        value: 0,
        label: 'Microsoft Store',
        disabled: true
      },
      {
        value: 1,
        label: 'Microsoft Store (Beta)',
        disabled: true
      },
      {
        value: 2,
        label: 'App Center (Canary)',
      },
      {
        value: 3,
        label: 'App Center (Release)',
      },
      {
        value: 4,
        label: 'Github Nightly',
      },
    ]
  }),
  methods: {
    submit() {
      this.error = false
      this.loading = true
      axios.post('/user', this.userData)
          .then(data => {
            this.success = true
            this.loading = false
          })
          .catch(error => {
            this.error = true
            this.loading = false
            this.errorMsg = error.message + ': ' + error.response.data
          })
    },
    getLatest() {
      this.$router.push("/channel/" + this.userData.channel + "/latest")
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

.hy-form-item {
  margin-top: 24px;
}
</style>