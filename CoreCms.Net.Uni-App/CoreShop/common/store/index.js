import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

const store = new Vuex.Store({
    state: {
        config: {}, // 店铺配置信息
        orderTab: 0, // 选中的订单tab页
        redirectPage: '',
        uuid: '',//当前客户端
        searchStyle: '',
        sessionAuthId: '', //微信缓存授权信息
        searchFixed: false,//搜索框样式
        showLoginTip: false,//显示登录框
        hasLogin: false,//存储用户当前是否登录，作为切换特效使用
        userShip: {}, //地区信息
        userInfo: {}, //用户信息存储
        invoice: {}, //发票信息
    },
    mutations: {
        config(state, payload) {
            state.config = payload
        },
        orderTab(state, tab) {
            state.orderTab = tab
        },
        redirect(state, payload) {
            state.redirectPage = payload.page
        },
        searchStyle(state, style) {
            state.searchStyle = style
        },
        sessionAuthId(state, payload) {
            state.sessionAuthId = payload
        },
        searchFixed(state, payload) {
            state.searchFixed = payload
        },
        showLoginTip(state, payload) {
            state.showLoginTip = payload
        },
        hasLogin(state, payload) {
            state.hasLogin = payload
        },
        userShip(state, userShip) {
            state.userShip = userShip
        },
        userInfo(state, userInfo) {
            state.userInfo = userInfo
        },
        invoice(state, invoice) {
            state.invoice = invoice
        }
    },
    actions: {

    },
    getters: {
        shopConfig: state => state.config,
        userInfo: state => state.userInfo,
        uuid: state => state.uuid,
        hasLogin: state => state.hasLogin,
        sessionAuthId: state => state.sessionAuthId,
    }
})

export default store
