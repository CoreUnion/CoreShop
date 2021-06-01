<template>
    <view class="my-box">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :is-back="true" back-icon-name="scan" back-icon-color="#000" title="会员中心"></u-navbar>

        <view class="head-box bg-red">
            <!--标题栏-->
            <!--小程序端不显示-->
            <!--用户信息-->
            <view class="user-info-box">
                <!--未登陆-->
                <view class="login-user-view" v-if="!hasLogin">
                    <!-- #ifdef H5 || APP-PLUS -->
                    <view class="login-user-avatar-view">
                        <u-avatar :src="$store.state.config.shopLogo" size="large"></u-avatar>
                    </view>
                    <u-button type="default" size="mini" @click="toLogin()">立即登录</u-button>
                    <!-- #endif -->
                    <!-- #ifdef MP-WEIXIN -->
                    <view class="login-user-avatar-view">
                        <view class="account-face grace-box-shadow">
                            <open-data type="userAvatarUrl"></open-data>
                        </view>
                    </view>
                    <u-button type="default" size="mini" @click="goLogin()">立即登录</u-button>
                    <!-- #endif -->
                    <!-- #ifdef MP-ALIPAY -->
                    <view class="login-user-avatar-view">
                        <u-avatar :src="userInfo.avatarImage" size="large"></u-avatar>
                    </view>
                    <u-button type="default" size="mini" @click="goLogin()">立即登录</u-button>
                    <!-- #endif -->
                    <!-- #ifdef MP-TOUTIAO -->
                    <view class="login-user-avatar-view">
                        <u-avatar :src="$store.state.config.shopLogo" size="large"></u-avatar>
                    </view>
                    <u-button type="default" size="mini" @click="goLogin()">立即登录</u-button>
                    <!-- #endif -->
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
                        <!-- #ifndef MP-WEIXIN -->
                        <view class="u-m-l-10 u-p-10">
                            <u-icon name="scan" color="#969799" size="28"></u-icon>
                        </view>
                        <view class="u-m-l-10 u-p-10" @tap="navigateToHandle('/pages/member/setting/userInfo/index')">
                            <u-icon name="arrow-right" color="#969799" size="28"></u-icon>
                        </view>
                        <!-- #endif -->
                        <!-- #ifdef MP-WEIXIN -->
                        <view class="u-m-l-10 u-m-r-20 u-p-10" @click="syncWeChatInfo()">
                            <u-icon name="reload" color="#fff" size="32"></u-icon>
                        </view>
                        <view class="u-m-l-20 u-p-10" @tap="navigateToHandle('/pages/member/setting/userInfo/index')">
                            <u-icon name="arrow-right" color="#fff" size="32"></u-icon>
                        </view>
                        <!-- #endif -->

                    </view>
                </view>
            </view>

            <!--用户数据-->

            <view class="user-info-num-box">
                <u-grid :col="4" :border="false" :bg-color="transparent">
                    <!--<u-grid-item bg-color="transparent" @tap="orderNavigateHandle('/pages/member/order/orderlist', 0)" :custom-style="{padding: '5rpx 0'}">
                        <view class="u-font-36" v-if="!hasLogin">-</view>
                        <view class="u-font-36" v-else>{{ userInfo.orderCount }}</view>
                        <text class="u-font-22">总订单</text>
                    </u-grid-item>-->
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
                    <u-grid-item bg-color="transparent" @tap="navigateToHandle('/pages/member/afterSales/list')" :custom-style="{padding: '5rpx 0'}">
                        <view class="u-font-36" v-if="!hasLogin">-</view>
                        <view class="u-font-36" v-else>{{afterSaleNums || 0}}</view>
                        <text class="u-font-22">售后</text>
                    </u-grid-item>
                </u-grid>
            </view>
        </view>

        <view class="coreshop-view-content">
            <!--用户数据-->
            <view class="padding-xs bg-white coreshop-user-info-order-box">
                <view class="text-black text-lg text-bold padding-sm">我的交易</view>
                <view class="cu-list grid col-5 no-border">
                    <view class="cu-item" v-for="(item, index) in orderItems" :key="index" @click="orderNavigateHandle('/pages/member/order/orderlist', index)">
                        <view class="text-xxl text-red" v-if="!hasLogin">
                            <text :class="['cuIcon-' + item.icon]"></text>
                        </view>
                        <view class="text-xxl text-black" v-else>{{item.nums}}</view>
                        <text class="text-sm">{{ item.name }}</text>
                    </view>
                </view>
            </view>

            <!--天天有钱-->
            <view class="padding-xs bg-white margin-top coreshop-user-info-money-box" v-if="order.invite.showItem">
                <view class="text-black text-lg text-bold padding-sm">天天有钱</view>
                <view class="grid col-2 money-col">
                    <view class="money-item">
                        <view class="money-item-view" @click="navigateToHandle('/pages/member/invite/index')">
                            <view class="cu-avatar lg yqhy" />
                            <view class="money-content">
                                <view class="text-black text-cut">边逛边赚钱</view>
                                <view class="text-gray text-sm text-cut">最高提现20元</view>
                            </view>
                        </view>
                    </view>
                    <view class="money-item">
                        <view class="money-item-view" @click="navigateToHandle('/pages/member/invite/index')">
                            <view class="cu-avatar lg yqhy2" />
                            <view class="money-content">
                                <view class="text-black text-cut">邀请好友</view>
                                <view class="text-gray text-sm text-cut">最高分红50000</view>
                            </view>
                        </view>
                    </view>
                </view>
            </view>

            <!--我的服务-->
            <view class="padding-xs bg-white margin-top coreshop-user-info-tools-box">
                <view class="padding-sm tools-view">
                    <view class="text-black text-bold text-lg tools-title">我的服务</view>
                </view>
                <view class="coreshop-tools-list-box">
                    <view class="cu-list grid col-4 no-border">

                        <view class="cu-item" @click="navigateToHandle('/pages/member/merchant/index/index')" v-if="isClerk">
                            <view class="text-black cuIcon-shop" />
                            <text>商家管理</text>
                        </view>

                        <view class="cu-item" @tap="$u.throttle(goDistributionPanel, 500)" v-if="isDistribution">
                            <view class="text-black cuIcon-all" />
                            <text>分销中心</text>
                        </view>

                        <view class="cu-item" @tap="$u.throttle(goAgentPanel, 500)" v-if="isAgent">
                            <view class="text-black cuIcon-link" />
                            <text>代理中心</text>
                        </view>


                        <block v-for="(item,i) in utilityMenus" :key="i" v-if="(item.showItem && i != 'invoice') || (item.showItem && i == 'invoice' && InvoiceSwitch == 1)">
                            <view class="cu-item" @click="navigateToHandle(item.router)">
                                <view class="text-black" :class="['cuIcon-' + item.icon]" />
                                <text>{{item.name}}</text>
                            </view>
                        </block>
                    </view>
                </view>
            </view>

            <!--增值业务-->
            <view class="padding-xs bg-white margin-top coreshop-user-info-tools-box">
                <view class="padding-sm tools-view">
                    <view class="text-black text-bold text-lg tools-title">增值业务</view>
                </view>
                <view class="coreshop-tools-list-box">
                    <view class="cu-list grid col-4 no-border">
                        <block v v-for="(item,i) in vas" :key="i" v-if="item.showItem">
                            <view class="cu-item" @click="goRoute(item.router)">
                                <view class="text-black" :class="['cuIcon-' + item.icon]" />
                                <text>{{item.name}}</text>
                            </view>
                        </block>
                    </view>
                </view>
            </view>

            <!--其他-->
            <view class="padding-xs bg-white margin-top coreshop-user-info-tools-box">
                <view class="padding-sm tools-view">
                    <view class="text-black text-bold text-lg tools-title">其他</view>
                </view>
                <view class="coreshop-tools-list-box">
                    <view class="cu-list grid col-4 no-border">
                        <view class="cu-item" @click="goArticleList()">
                            <view class="text-black cuIcon-question" />
                            <text>帮助中心</text>
                        </view>
                        <view class="cu-item" v-for="(item,i) in order" :key="i" v-if="item.showItem" @click="navigateToHandle(item.router)">
                            <view class="text-black" :class="['cuIcon-' + item.icon]" />
                            <text>{{item.name}}</text>
                        </view>
                        <!-- #ifdef H5 || APP-PLUS || APP-PLUS-NVUE -->
                        <view class="cu-item" @click="showChat">
                            <view class="text-black cuIcon-service" />
                            <text>联系客服</text>
                        </view>
                        <!-- #endif -->
                        <!-- #ifdef MP-WEIXIN -->
                        <!-- todo:: 微信客服 -->
                        <!-- #endif -->
                        <!-- #ifdef MP-TOUTIAO -->
                        <!-- todo:: 头条客服 -->
                        <!-- #endif -->
                        <!-- #ifdef MP-ALIPAY -->
                        <!-- todo:: 支付宝客服 -->
                        <!-- #endif -->
                    </view>
                </view>
            </view>

        </view>

        <!-- 登录提示 -->
        <corecms-login-modal></corecms-login-modal>
    </view>
</template>

<script>
    import { commonUse, articles } from '@/common/mixins/mixinsHelper.js';
    import { mapMutations, mapActions, mapState } from 'vuex';

    export default {
        mixins: [commonUse, articles],
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
                        icon: 'timefill',
                        nums: 0
                    }, {
                        name: '待付款',
                        icon: 'timefill',
                        nums: 0
                    },
                    {
                        name: '待发货',
                        icon: 'deliver_fill',
                        nums: 0
                    },
                    {
                        name: '待收货',
                        icon: 'cartfill',
                        nums: 0
                    },
                    {
                        name: '待评价',
                        icon: 'commentfill',
                        nums: 0
                    }
                ],
                utilityMenus: {
                    myCoupon: {
                        name: '我的优惠券',
                        icon: 'ticket',
                        router: '/pages/member/coupon/index',
                        showItem: true
                    },
                    myBalance: {
                        name: '我的余额',
                        icon: 'recharge',
                        router: '/pages/member/balance/index',
                        showItem: true
                    },
                    myInvoice: {
                        name: '我的发票',
                        icon: 'news',
                        router: '/pages/member/invoice/index',
                        showItem: true
                    },
                    myServices: {
                        name: '我的服务卡',
                        icon: 'vipcard',
                        router: '/pages/member/serviceOrder/index',
                        showItem: true
                    },
                    myIntegral: {
                        name: '我的积分',
                        icon: 'medal',
                        router: '/pages/member/integral/index',
                        showItem: true
                    },
                    myAddress: {
                        name: '地址管理',
                        icon: 'location',
                        router: '/pages/member/address/list',
                        showItem: true
                    },
                    myCollection: {
                        name: '我的收藏',
                        icon: 'favor',
                        router: '/pages/member/collection/index',
                        showItem: true
                    },
                    myHistory: {
                        name: '我的足迹',
                        icon: 'footprint',
                        router: '/pages/member/history/index',
                        showItem: true
                    },
                },
                vas: {
                    storeMap: {
                        name: '门店列表',
                        icon: 'shop',
                        router: '/pages/storeMap/storeMap',
                        showItem: false
                    },
                    servicePackage: {
                        name: '服务包',
                        icon: 'send',
                        router: '/pages/serviceGoods/index/index',
                        showItem: true
                    },
                    coupons: {
                        name: '优惠券',
                        icon: 'ticket',
                        router: '/pages/coupon/coupon',
                        showItem: true
                    },
                    pinTuan: {
                        name: '拼团',
                        icon: 'cascades',
                        router: '/pages/activity/pinTuan/list/list',
                        showItem: true
                    },
                    seckill: {
                        name: '秒杀',
                        icon: 'remind',
                        router: '/pages/activity/seckill/list/list',
                        showItem: true
                    },
                    groupBuying: {
                        name: '团购',
                        icon: 'goods',
                        router: '/pages/activity/groupBuying/list/list',
                        showItem: true
                    },
                },
                order: {
                    invite: {
                        name: '邀请好友',
                        icon: 'friendadd',
                        router: '/pages/member/invite/index',
                        showItem: false
                    },
                    setting: {
                        name: '系统设置',
                        icon: 'repair',
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
                            _this.$u.route({ url: '/pages/member/agent/panel' });
                        } else if (_this.condition.verifyStatus > 1) {
                            _this.$u.route({ url: '/pages/member/agent/applyState' });
                        } else {
                            _this.$u.route({ url: '/pages/member/agent/index' });
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
                                _this.$u.route({ url: '/pages/member/distribution/panel' });
                            } else if (_this.condition.verifyStatus > 1) {
                                _this.$u.route({ url: '/pages/member/distribution/applyState' });
                            } else {
                                _this.$u.route({ url: '/pages/member/distribution/index' });
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
            toLogin() {
                this.$u.route('/pages/member/login/login/loginByAccount')
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
                    _this.order.invite.showItem = true;
                } else if (this.config.showInviter == 2) {
                    //显示-邀请好友
                    _this.order.invite.showItem = false;
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
                // #ifdef H5
                // #endif
                // #ifdef APP-PLUS || APP-PLUS-NVUE
                this.$u.route('/pages/member/customerService/index');
                // #endif
                // 头条系客服
                // #ifdef MP-TOUTIAO
                if (this.shopMobile != 0) {
                    let _this = this;
                    tt.makePhoneCall({
                        phoneNumber: this.shopMobile.toString(),
                        success(res) { },
                        fail(res) { }
                    });
                } else {
                    _this.$u.toast('暂无设置客服电话');
                }
                // #endif
            },
            //同步微信昵称数据
            syncWeChatInfo() {
                let _this = this
                wx.getUserProfile({
                    desc: "获取你的昵称、头像、地区及性别",
                    success: e => {
                        console.log(e)
                        if (e.errMsg == 'getUserProfile:ok') {
                            //var data = {
                            //    avatarUrl: e.avatarUrl,
                            //    city: e.city,
                            //    country: e.country,
                            //    gender: e.gender,
                            //    language: e.language,
                            //    nickName: e.nickName,
                            //    province: e.province
                            //}
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
    @import '../../../static/style/member.scss';
</style>
