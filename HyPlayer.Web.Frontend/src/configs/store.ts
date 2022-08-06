import {defineStore} from 'pinia'
import md5 from "md5"

export const useStore = defineStore('hyplayer',
    {
        state: () => ({
            user: {
                isLogin: false,
                userName: "未登录",
                email: ''
            }
        }),
        getters: {
            userAvatar: (state) => 'https://cn.gravatar.com/avatar/' + md5(state.user.email.trim().toLowerCase())
        }
    });