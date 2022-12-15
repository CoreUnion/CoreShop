import { apiBaseUrl } from '@/common/setting/constVarsHelper.js';
import * as db from '@/common/utils/dbHelper.js' //引入common


// 这里的Vue为Vue对象(非创建出来的实例)，vm为main.js中“Vue.use(httpInterceptor, app)”这一句的第二个参数，
// 为一个Vue的实例，也即每个页面的"this"
// 如果需要了解这个install方法是什么，请移步：https://uviewui.com/components/vueUse.html
const install = (Vue, vm) => {
    // 此为自定义配置参数，具体参数见上方说明
    Vue.prototype.$u.http.setConfig({
        baseUrl: apiBaseUrl, // 请求的本域名
        method: 'POST',
        dataType: 'json',// 设置为json，返回后会对数据进行一次JSON.parse()
        showLoading: true, // 是否显示请求中的loading
        loadingText: '请求中...', // 请求loading中的文字提示
        loadingTime: 800, // 在此时间内，请求还没回来的话，就显示加载中动画，单位ms
        originalData: false, // 是否在拦截器中返回服务端的原始数据
        loadingMask: true, // 展示loading的时候，是否给一个透明的蒙层，防止触摸穿透
        // 配置请求头信息
        header: {
            'Content-type': 'application/json'
        },
    });

    // 请求拦截部分，如配置，每次请求前都会执行
    Vue.prototype.$u.http.interceptor.request = (config) => {
        if (config.header.needToken) {
            // 获取用户token
            const userToken = db.get("userToken");
            if (!userToken) {
                console.log("开启弹窗");
                Vue.prototype.$store.commit('showLoginTip', true);
                return false;
            } else {
                config.header.Authorization = 'Bearer ' + userToken;
            }
        }
        //额外需求
        if (config.header.method == 'user.share') {
            const userToken = db.get("userToken");
            config.header.Authorization = 'Bearer ' + userToken;
        }
        return config;
    }
    // 响应拦截，如配置，每次请求结束都会执行本方法
    Vue.prototype.$u.http.interceptor.response = (result) => {
        let pages = getCurrentPages();
        var page = pages[pages.length - 1];

        if (!result.status && page) {
            //console.log(page.route);
            // 登录信息过期或者未登录
            if (result.data === 14007 || result.data === 14006) {
                db.del("userToken");
                console.log("开启登录弹窗");
                //Vue.prototype.$store.commit('showLoginTip', true);
                Vue.prototype.$store.commit('hasLogin', false);
            }
        }

        return result;
        // res为服务端返回值，可能有code，result等字段
        // 这里对res.result进行返回，将会在this.$u.post(url).then(res => {})的then回调中的res的到
        // 如果配置了originalData为true，请留意这里的返回值
    }
}

export default {
    install
}