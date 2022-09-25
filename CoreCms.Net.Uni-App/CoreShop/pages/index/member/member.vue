<template>
    <view class="memberBox">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :is-back="true" back-icon-name="scan" back-icon-color="#000" title="会员中心"></u-navbar>

        <view class="headBox coreshop-bg-red">
            <!--标题栏-->
            <!--小程序端不显示-->
            <!--用户信息-->
            <view class="user-info-box">
                <!--未登陆-->
                <view class="login-user-view" v-if="!hasLogin">
                    <view class="login-user-avatar-view">
                        <view class="account-face grace-box-shadow">
                            <open-data type="userAvatarUrl"></open-data>
                        </view>
                    </view>
                    <u-button type="default" size="mini" @click="goLogin()">立即登录</u-button>
                </view>

                <!--已登陆-->
                <view v-else>
                    <view class="u-flex user-box u-p-l-30 u-p-r-20 u-p-b-30">
                        <view class="u-m-r-10">
                            <u-avatar :src="userInfo.avatarImage" size="96"></u-avatar>
                        </view>
                        <view class="u-flex-1 userItem">
                            <view class="u-font-34 u-p-b-20">
                                <text class="u-margin-right-30">{{ userInfo.nickName }}</text>
                                <text class="u-font-24">{{ userInfo.gradeName }}</text>
                            </view>
                            <view class="u-font-24">
                                <text class="text-border-x">积分 {{ userInfo.point }}</text>
                                <text>余额 {{ userInfo.balance }}</text>
                            </view>
                        </view>
                        <view class="u-m-l-10 u-m-r-20 u-p-10" @click="syncWeChatInfo()">
                            <u-icon name="reload" color="#fff" size="32"></u-icon>
                        </view>
                        <view class="u-m-l-20 u-p-10" @tap="navigateToHandle('/pages/member/setting/userInfo/index')">
                            <u-icon name="arrow-right" color="#fff" size="32"></u-icon>
                        </view>
                    </view>
                </view>
            </view>

            <!--用户数据-->

            <view class="user-info-num-box">
                <u-grid :col="4" :border="false" :bg-color="transparent">
                    <u-grid-item bg-color="transparent" @tap="navigateToHandle('/pages/member/history/index')" :custom-style="{padding: '5rpx 0'}">
                        <view class="u-font-36" v-if="!hasLogin">-</view>
                        <view class="u-font-36" v-else>{{ userInfo.footPrintCount }}</view>
                        <text class="u-font-22">足迹</text>
                    </u-grid-item>
                    <u-grid-item bg-color="transparent" @tap="navigateToHandle('/pages/member/coupon/index')" :custom-style="{padding: '5rpx 0'}">
                        <view class="u-font-36" v-if="!hasLogin">-</view>
                        <view class="u-font-36" v-else>{{ userInfo.userCouponCount }}</view>
                        <text class="u-font-22">优惠券</text>
                    </u-grid-item>
                    <u-grid-item bg-color="transparent" @tap="navigateToHandle('/pages/member/collection/index')" :custom-style="{padding: '5rpx 0'}">
                        <view class="u-font-36" v-if="!hasLogin">-</view>
                        <view class="u-font-36" v-else>{{ userInfo.collectionCount }}</view>
                        <text class="u-font-22">收藏</text>
                    </u-grid-item>
                    <u-grid-item bg-color="transparent" @tap="navigateToHandle('/pages/member/afterSales/list/list')" :custom-style="{padding: '5rpx 0'}">
                        <view class="u-font-36" v-if="!hasLogin">-</view>
                        <view class="u-font-36" v-else>{{afterSaleNums || 0}}</view>
                        <text class="u-font-22">售后</text>
                    </u-grid-item>
                </u-grid>
            </view>
        </view>

        <view class="coreshop-view-content">
            <!--用户数据-->
            <view class="u-padding-10 coreshop-bg-white coreshop-user-info-order-box">
                <view class="coreshop-text-black u-font-lg coreshop-text-bold u-padding-20">我的交易</view>
                <u-grid :col="5" :border="false">
                    <u-grid-item v-for="(item, index) in orderItems" :key="index" @click="orderNavigateHandle('/pages/member/order/index/index', index)">
                        <view class="transactionNumber" v-if="hasLogin">{{ item.nums }}</view>
                        <u-icon :name="item.icon" :size="50" v-else></u-icon>
                        <view class="grid-text">{{ item.name }}</view>
                    </u-grid-item>
                </u-grid>
            </view>

            <!--天天有钱-->
            <view class="u-padding-10 coreshop-bg-white u-margin-top-30 coreshop-user-info-money-box" v-if="other.invite.showItem">
                <view class="coreshop-text-black u-font-lg coreshop-text-bold u-padding-20">天天有钱</view>
                <u-row gutter="16" class="money-col">
                    <u-col span="6">
                        <view class="money-item">
                            <view class="money-item-view" @click="navigateToHandle('/pages/member/invite/index')">
                                <view class="money-avatar coreshop-avatar lg yqhy" />
                                <view class="money-content">
                                    <view class="coreshop-text-black u-line-1">边逛边赚钱</view>
                                    <view class="coreshop-text-gray u-font-sm u-line-1">最高提现20元</view>
                                </view>
                            </view>
                        </view>
                    </u-col>
                    <u-col span="6">
                        <view class="money-item">
                            <view class="money-item-view" @click="navigateToHandle('/pages/member/invite/index')">
                                <view class="money-avatar coreshop-avatar lg yqhy2" />
                                <view class="money-content">
                                    <view class="coreshop-text-black u-line-1">邀请好友</view>
                                    <view class="coreshop-text-gray u-font-sm u-line-1">最高分红50000</view>
                                </view>
                            </view>
                        </view>
                    </u-col>
                </u-row>
            </view>

            <!--我的服务-->
            <view class="u-padding-10 coreshop-bg-white u-margin-top-30 coreshop-user-info-tools-box">
                <view class="u-padding-20 tools-view">
                    <view class="coreshop-text-black coreshop-text-bold u-font-lg tools-title">我的服务</view>
                </view>
                <view class="coreshop-tools-list-box">
                    <u-grid :col="4" :border="false">
                        <u-grid-item @click="navigateToHandle('/pages/member/merchant/index/index')" v-if="isClerk">
                            <u-icon name="calendar" :size="50" color="#666"></u-icon>
                            <view class="grid-text">商家管理</view>
                        </u-grid-item>
                        <u-grid-item @tap="$u.throttle(goDistributionPanel, 500)" v-if="isDistribution">
                            <u-icon name="wifi" :size="50" color="#666"></u-icon>
                            <view class="grid-text">分销中心</view>
                        </u-grid-item>
                        <u-grid-item @tap="$u.throttle(goAgentPanel, 500)" v-if="isAgent">
                            <u-icon name="zhuanfa" :size="50" color="#666"></u-icon>
                            <view class="grid-text">代理中心</view>
                        </u-grid-item>
                        <u-grid-item v-for="(item,i) in utilityMenus" :key="i" v-if="(item.showItem && i != 'invoice') || (item.showItem && i == 'invoice' && InvoiceSwitch == 1)" @click="navigateToHandle(item.router)">
                            <u-icon :name="item.icon" :size="50"  color="#666"></u-icon>
                            <view class="grid-text">{{ item.name }}</view>
                        </u-grid-item>
                    </u-grid>

                </view>
            </view>

            <!--增值业务-->
            <view class="u-padding-10 coreshop-bg-white u-margin-top-30 coreshop-user-info-tools-box">
                <view class="u-padding-20 tools-view">
                    <view class="coreshop-text-black coreshop-text-bold u-font-lg tools-title">增值业务</view>
                </view>
                <view class="coreshop-tools-list-box">
                    <u-grid :col="4" :border="false">
                        <u-grid-item v-for="(item,i) in vas" :key="i" v-if="item.showItem" @click="goRoute(item.router)">
                            <u-icon :name="item.icon" :size="50" color="#666"></u-icon>
                            <view class="grid-text">{{ item.name }}</view>
                        </u-grid-item>
                    </u-grid>
                </view>
            </view>

            <!--其他-->
            <view class="u-padding-10 coreshop-bg-white u-margin-top-30 coreshop-user-info-tools-box">
                <view class="u-padding-20 tools-view">
                    <view class="coreshop-text-black coreshop-text-bold u-font-lg tools-title">其他</view>
                </view>
                <view class="coreshop-tools-list-box">
                    <u-grid :col="4" :border="false">
                        <u-grid-item @click="goArticleList()">
                            <u-icon name="question-circle" :size="50" color="#666"></u-icon>
                            <view class="grid-text">帮助中心</view>
                        </u-grid-item>
                        <u-grid-item v-for="(item,i) in other" :key="i" v-if="item.showItem" @click="navigateToHandle(item.router)">
                            <u-icon :name="item.icon" :size="50" color="#666"></u-icon>
                            <view class="grid-text">{{ item.name }}</view>
                        </u-grid-item>
                    </u-grid>
                </view>
            </view>

        </view>
         <!--版权组件-->
        <copyright></copyright>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>

<script>
    import { commonUse, articles } from '@/common/mixins/mixinsHelper.js';
    import { mapMutations, mapActions, mapState } from 'vuex';
    import copyright from '@/components/coreshop-copyright/coreshop-copyright.vue';
    export default {
        mixins: [commonUse, articles],
        components: {
            copyright
        },
        data() {
            return {
                afterSaleNums: 0, //售后数量
                isClerk: false,//显示商家管理
                isDistribution: false,//显示分销中心
                isAgent: false,//显示代理中心

                config: '',//配置信息
                orderItems: [
                    {
                        name: '全部',
                        icon: 'order',
                        nums: 0
                    }, {
                        name: '待付款',
                        icon: 'order',
                        nums: 0
                    },
                    {
                        name: '待发货',
                        icon: 'order',
                        nums: 0
                    },
                    {
                        name: '待收货',
                        icon: 'order',
                        nums: 0
                    },
                    {
                        name: '待评价',
                        icon: 'order',
                        nums: 0
                    }
                ],
                utilityMenus: {
                    myCoupon: {
                        name: '我的优惠券',
                        icon: 'coupon',
                        router: '/pages/member/coupon/index',
                        showItem: true
                    },
                    myBalance: {
                        name: '我的余额',
                        icon: 'rmb-circle',
                        router: '/pages/member/balance/index/index',
                        showItem: true
                    },
                    myInvoice: {
                        name: '我的发票',
                        icon: 'calendar',
                        router: '/pages/member/invoice/index',
                        showItem: true
                    },
                    myServices: {
                        name: '我的服务卡',
                        icon: 'bell',
                        router: '/pages/member/serviceOrder/index/index',
                        showItem: true
                    },
                    myIntegral: {
                        name: '我的积分',
                        icon: 'integral',
                        router: '/pages/member/integral/index',
                        showItem: true
                    },
                    myAddress: {
                        name: '地址管理',
                        icon: 'map',
                        router: '/pages/member/address/list/list',
                        showItem: true
                    },
                    myCollection: {
                        name: '我的收藏',
                        icon: 'bookmark',
                        router: '/pages/member/collection/index',
                        showItem: true
                    },
                    myHistory: {
                        name: '我的足迹',
                        icon: 'bag',
                        router: '/pages/member/history/index',
                        showItem: true
                    },
                },
                vas: {
                    storeMap: {
                        name: '门店列表',
                        icon: 'home',
                        router: '/pages/storeMap/storeMap',
                        showItem: false
                    },
                    servicePackage: {
                        name: '服务商品',
                        icon: 'list-dot',
                        router: '/pages/serviceGoods/index/index',
                        showItem: true
                    },
                    coupons: {
                        name: '优惠券',
                        icon: 'red-packet',
                        router: '/pages/coupon/coupon',
                        showItem: true
                    },
                    pinTuan: {
                        name: '拼团',
                        icon: 'grid',
                        router: '/pages/activity/pinTuan/list/list',
                        showItem: true
                    },
                    seckill: {
                        name: '秒杀',
                        icon: 'clock',
                        router: '/pages/activity/seckill/list/list',
                        showItem: true
                    },
                    groupBuying: {
                        name: '团购',
                        icon: 'trash',
                        router: '/pages/activity/groupBuying/list/list',
                        showItem: true
                    },
                },
                other: {
                    invite: {
                        name: '邀请好友',
                        icon: 'man-add',
                        router: '/pages/member/invite/index',
                        showItem: false
                    },
                    search: {
                        name: '商品检索',
                        icon: 'search',
                        router: '/pages/search/search',
                        showItem: true
                    },
                    setting: {
                        name: '系统设置',
                        icon: 'setting',
                        router: '/pages/member/setting/index/index',
                        showItem: true
                    }
                },
                list: 2,
                suTipStatus: false,
                opacity: 0,
            }
        },
        onShow() {
            this.initData()
        },
        computed: {
            ...mapState({
                hasLogin: state => state.hasLogin,
                userInfo: state => state.userInfo,
            }),
            shopMobile() {
                return this.$store.state.config.shopMobile || 0;
            },
            InvoiceSwitch() {
                return this.$store.state.config.invoiceSwitch || 2;
            },
            StoreSwitch() {
                return this.$store.state.config.storeSwitch || 0;
            },
            hasLogin: {
                get() {
                    return this.$store.state.hasLogin;
                },
                set(val) {
                    this.$store.commit('hasLogin', val);
                }
            },
            userInfo: {
                get() {
                    return this.$store.state.userInfo;
                },
                set(val) {
                    this.$store.commit('userInfo', val);
                }
            }
        },
        methods: {
            goAgentPanel() {
                var _this = this;
                uni.showLoading({
                    title: '跳转中...'
                });
                _this.$u.api.getAgentInfo().then(res => {
                    if (res.status) {
                        _this.condition = res.data;
                        if (_this.condition.verifyStatus == 1 || (!_this.condition.needApply && _this.conditionStatus)) {
                            _this.$u.route({ url: '/pages/member/agent/panel/panel' });
                        } else if (_this.condition.verifyStatus > 1) {
                            _this.$u.route({ url: '/pages/member/agent/applyState/applyState' });
                        } else {
                            _this.$u.route({ url: '/pages/member/agent/index/index' });
                        }
                    } else {
                        //报错了
                        _this.$u.toast(res.msg);
                    }
                });
                uni.hideLoading();
            },
            goDistributionPanel() {
                var _this = this;
                uni.showLoading({
                    title: '跳转中...'
                });
                _this.$u.api.getDistributionInfo().then(res => {
                    if (res.status) {
                        _this.condition = res.data;
                        if (_this.condition.hasOwnProperty('verifyStatus')) {
                            if (_this.condition.verifyStatus == 1 || (!_this.condition.needApply && _this.conditionStatus)) {
                                _this.$u.route({ url: '/pages/member/distribution/panel/panel' });
                            } else if (_this.condition.verifyStatus > 1) {
                                _this.$u.route({ url: '/pages/member/distribution/applyState/applyState' });
                            } else {
                                _this.$u.route({ url: '/pages/member/distribution/index/index' });
                            }
                        }
                    } else {
                        //报错了
                        _this.$u.toast(res.msg);
                    }
                });
                uni.hideLoading();
            },
            goLogin() {
                this.$store.commit('showLoginTip', true);
            },
            initData() {
                var _this = this
                //判断是开启分销还是原始推广
                this.config = this.$store.state.config;
                if (this.config.openDistribution == 2) {
                    //屏蔽分销按钮
                    _this.isDistribution = false
                } else if (this.config.openDistribution == 1) {
                    _this.isDistribution = true
                }
                if (this.config.isOpenAgent == 1 && this.config.isShowAgentPortal == 1) {
                    _this.isAgent = true
                } else if (this.config.openDistribution == 1) {
                    //屏蔽代理中心入库
                    _this.isAgent = false
                }
                if (this.config.showInviter == 1) {
                    //不显示-邀请好友
                    _this.other.invite.showItem = true;
                } else if (this.config.showInviter == 2) {
                    //显示-邀请好友
                    _this.other.invite.showItem = false;
                }
                if (this.config.showStoresSwitch == 1) {
                    //不显示-门店展示列表
                    _this.vas.storeMap.showItem = true;
                } else if (this.config.showStoresSwitch == 2) {
                    //显示-门店展示列表
                    _this.vas.storeMap.showItem = false;
                }
                this.getUserInfo();
            },
            getUserInfo() {
                var _this = this
                if (this.$db.get('userToken')) {
                    this.hasLogin = true
                    this.$u.api.userInfo().then(res => {
                        if (res.status) {
                            _this.userInfo = res.data
                            // #ifdef MP-WEIXIN
                            // #endif
                            // 获取订单不同状态的数量
                            let data = {
                                ids: '0,1,2,3,4',
                                isAfterSale: true
                            }
                            _this.$u.api.getOrderStatusSum(data).then(res => {
                                if (res.status) {
                                    _this.orderItems.forEach((item, key) => {
                                        item.nums = res.data[key]
                                    })
                                    _this.afterSaleNums = res.data['isAfterSale'];
                                }
                            })
                            //判断是否是店员
                            _this.$u.api.isStoreUser().then(res => {
                                this.isClerk = res.data
                            })
                        }
                    })
                } else {
                    this.hasLogin = false
                }
            },
            navigateToHandle(pageUrl) {
                uni.showLoading({
                    title: '跳转中...'
                });
                if (!this.hasLogin) {
                    this.$store.commit('showLoginTip', true);
                    uni.hideLoading();
                    return false;
                }
                uni.hideLoading();
                this.$u.route(pageUrl)
            },
            orderNavigateHandle(url, tab = 0) {
                if (!this.hasLogin) {
                    this.$store.commit('showLoginTip', true);
                    return false;
                }
                this.$store.commit('orderTab', tab)
                this.$u.route(url + '?swiperCurrentIndexId=' + tab);
            },
            //在线客服,只有手机号的，请自己替换为手机号
            showChat() {
            },
            //同步微信昵称数据
            syncWeChatInfo() {
                let _this = this
                wx.getUserProfile({
                    desc: "获取你的昵称、头像、地区及性别",
                    success: e => {
                        console.log(e)
                        if (e.errMsg == 'getUserProfile:ok') {
                            _this.$u.api.syncWeChatInfo(e.userInfo).then(res => {
                                console.log(res);
                                if (res.status) {
                                    _this.$refs.uToast.show({ title: '同步成功', type: 'success', });
                                    if (res.data) {
                                        _this.hasLogin = true
                                        //_this.$store.commit('userInfo', res.data);
                                    }
                                } else {
                                    _this.$u.toast('登录失败，请重试')
                                }
                            })
                        }
                    },
                    fail: res => {
                        //拒绝授权
                        this.$refs.uToast.show({ title: '您拒绝了请求', type: 'error' })
                        return;
                    }
                })
            },
        },
        watch: {
            hasLogin() {
                this.getUserInfo();
            }
        },
        //分享
        onShareAppMessage(res) {
            return {
                title: this.$store.state.config.shareTitle,
                imageUrl: this.$store.state.config.shareImage,
                path: this.shareUrl
            }
        },
        onShareTimeline(res) {
            return {
                title: this.$store.state.config.shareTitle,
                imageUrl: this.$store.state.config.shareImage,
                path: this.shareUrl
            }
        },
    }
</script>
<style lang="scss" scoped>
    @import 'member.scss';
</style>
