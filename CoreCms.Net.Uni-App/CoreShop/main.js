import Vue from 'vue'
import App from './App'
//上传方法
import * as Upload from '@/common/utils/uploadHelper.js'
//常用方法库
import * as Common from '@/common/utils/commonHelper.js'
//本地存储封装
import * as Db from '@/common/utils/dbHelper.js'
//全局常量配置
import * as GlobalConstVars from '@/common/setting/constVarsHelper.js'

//全局常量配置
import * as CoreTheme from '@/common/setting/coreThemeHelper.js'

import store from '@/common/store'

//引入全局uView
import uView from 'uview-ui';
Vue.use(uView);


// 引入uView对小程序分享的mixin封装
let mpShare = require('@/uview-ui/libs/mixin/mpShare.js');
Vue.mixin(mpShare)

//全局引用常量配置文件,用于template内代码使用
Vue.mixin({
    data() {
        return {
            $globalConstVars: GlobalConstVars
        }
    }
})

Vue.config.productionTip = false
Vue.prototype.$upload = Upload;
Vue.prototype.$common = Common;
Vue.prototype.$db = Db;
Vue.prototype.$globalConstVars = GlobalConstVars;
Vue.prototype.$coreTheme = CoreTheme;
Vue.prototype.$store = store;

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
