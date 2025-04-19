<template>
  <el-row justify="center">
    <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="8">
      <el-card class="box-card">
        <template #header>
          <div class="card-header">
            <el-image class="hy-header-image" src="https://s2.loli.net/2022/07/24/vwmY7t19uXLHPOr.png"/>
            <div>
              <div class="hy-card-header-title">
                {{ channelName }}
                <br/>
                {{ update.version }}
              </div>
            </div>
            <el-row>HyPlayer</el-row>
          </div>
        </template>
        <div class="hy-card-content">
          <el-skeleton style="width: 100%" :loading="loading" animated>
            <v-md-preview :text="update.updateLog"></v-md-preview>
            <el-button type="primary" @click="download" class="hy-download-btn">下载版本包</el-button>
            <el-button type="primary" @click="downloadFull" class="hy-download-btn">下载完整包 (新手推荐)</el-button>
            <el-button type="primary" @click="downloadBasic" class="hy-download-btn">下载基础包</el-button>
            <el-alert
                title="温馨提示"
                type="success"
                description="如果您是第一次安装内测通道版本, 建议下载完整包"
                show-icon
            />
          </el-skeleton>
          <el-alert class="hy-alert" v-if="error" :title="errorMsg" type="error" show-icon/>
        </div>
      </el-card>
    </el-col>
  </el-row>
</template>

<script lang="ts">

import axios from 'axios'
import {defineComponent} from "vue";

export default defineComponent({
  name: "Version",
  data: () => ({
    error: false,
    errorMsg: '',
    loading: true,
    update: {
      version: '加载中',
      date: '加载中',
      updateLog: '加载中',
      url: ''
    }
  }),
  computed: {
    channelName() {
      switch (this.$route.params.channel) {
        case '0':
          return "Microsoft Store"
        case '1':
          return "Microsoft Store (Beta 通道)"
        case '2':
          return "App Center (Canary)"
        case '3':
          return "App Center (Release)"
        case '4':
          return "Github Nightly"
	case '5':
	  return "Canary"
	case '6':
	  return "Release"
	case '7':
	  return "Dogfood"
      }
    }
  },
  methods: {
    download() {
      window.location.href = this.update.url
    },
    downloadBasic() {
      window.location.href = "https://install.appcenter.ms/users/kengwang/apps/hyplayer/distribution_groups/base%20packages"
    },
    downloadFull() {
      window.location.href = "https://hyplayer-file.kengwang.com.cn/full/%E5%AE%8C%E6%95%B4%E5%8C%85_latest.zip"
    }
  },
  mounted() {
    axios.get('/channel/' + this.$route.params.channel + '/latest/' + this.$route.params.userId)
        .then(data => {
          this.update.version = data.data.version
          this.update.date = data.data.date
          this.update.updateLog = data.data.updateLog
          this.update.url = data.data.downloadUrl
          this.loading = false
        })
        .catch(error => {
          this.error = true
          this.errorMsg = error.message + ': ' + error.response.data
          this.errorMsg += error.response.data.detail
        })
  }
})
</script>

<style scoped>
.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.hy-card-header-title {
  text-align: center;
}

.hy-header-image {
  max-height: 65px;
  max-width: 65px;
}

.hy-card-content {
  margin-top: 24px;
}

.hy-download-btn {
  width: 100%;
  margin-top: 16px;
}

.hy-alert {
  margin-top: 24px;
}
</style>
