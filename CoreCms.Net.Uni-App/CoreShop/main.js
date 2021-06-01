import Vue from 'vue'
import App from './App'

import * as Upload from '@/common/utils/uploadHelper.js'

import * as Common from '@/common/utils/commonHelper.js'
import * as Db from '@/common/utils/dbHelper.js'
import * as Config from '@/common/setting/constVarsHelper.js'
import store from '@/common/store'

//引入全局uView
import uView from 'uview-ui';
Vue.use(uView);


// 引入uView对小程序分享的mixin封装
let mpShare = require('@/uview-ui/libs/mixin/mpShare.js');
Vue.mixin(mpShare)

//引入全局colorUI头部
import cuCustom from '@/static/colorui/components/cu-custom.vue'
Vue.component('cu-custom', cuCustom)

import { apiFilesUrl } from '@/common/setting/constVarsHelper.js'


Vue.config.productionTip = false
Vue.prototype.$upload = Upload;
Vue.prototype.$common = Common;
Vue.prototype.$db = Db;
Vue.prototype.$config = Config;
Vue.prototype.$store = store;

Vue.prototype.$apiFilesUrl = apiFilesUrl;

App.mpType = 'app'

const app = new Vue({
    ...App
})

// http拦截器
import httpInterceptor from '@/common/request/http.interceptor.js'
// 这里需要写在最后，是为了等Vue创建对象完成，引入"app"对象(也即页面的"this"实例)
Vue.use(httpInterceptor, app)

// http接口API集中管理引入部分
import httpApi from '@/common/request/http.api.js'
Vue.use(httpApi, app)

app.$mount()
