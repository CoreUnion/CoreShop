<template>
    <view style="width:100%;height: 100%;">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="代理面板"></u-navbar>
        <view>
            <!-- 用户资料 -->
            <view class="user-card coreshop-bg-blue">
                <view class="card-top u-flex u-row-between">
                    <view class="coreshop-flex coreshop-align-center">
                        <view class="head-img-box"><image class="head-img" :src="userInfo.avatarImage" mode=""></image></view>
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-start">
                            <view class="user-name">{{ userInfo.nickName }}</view>
                            <view class="user-info-box coreshop-flex coreshop-align-center">
                                <view class="coreshop-flex coreshop-align-center" v-if="info.gradeName">
                                    <text class="cu-tag bg-purple sm radius">{{ info.gradeName }}</text>
                                </view>
                            </view>
                        </view>
                    </view>
                    <view class="coreshop-flex coreshop-flex-direction coreshop-align-start">
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-center">
                            <view class="log-btn" @click="navigateToHandle(utilityMenus.balance.router)">明细</view>
                            <view class="look-btn" @click="onEye">
                                <u-icon class="eye" v-if="showMoney" name="eye-fill"></u-icon>
                                <u-icon class="eye" v-else name="eye-off"></u-icon>
                            </view>
                        </view>
                    </view>
                </view>
                <view class="card-bottom u-flex u-row-between">
                    <view class="flex-sub coreshop-flex coreshop-flex-direction coreshop-align-start">
                        <view class="item-title">已结算金额</view>
                        <view class="item-detail">{{ showMoney ? info.settlementAmount || '0.00' : '***' }}</view>
                    </view>
                    <view class="flex-sub coreshop-flex coreshop-flex-direction coreshop-align-start" style="align-items: center;">
                        <view class="item-title">待入账佣金</view>
                        <view class="item-detail">{{ showMoney ? info.freezeAmount || '0.00' : '***' }}</view>
                    </view>
                    <view class="flex-sub coreshop-flex coreshop-flex-direction coreshop-align-start" style="align-items: flex-end;">
                        <view class="item-title">本月订单数</view>
                        <view class="item-detail">{{ showMoney ? info.currentMonthOrder || '0.00' : '***' }}</view>
                    </view>
                </view>
            </view>

            <view class="coreshop-list menu card-menu">
                <view class="coreshop-list-item arrow" v-for="(menu, index) in utilityMenus" :key="index" @tap="navigateToHandle(menu.router)">
                    <view class="content">
                        <image :src="menu.icon" class="png" mode="aspectFit"></image>
                        <text class="coreshop-text-grey">{{ menu.name }}</text>
                    </view>
                </view>
                <!-- 分享二维码 -->
                <view class="coreshop-list-item arrow"  @click="createPoster()">
                    <view class="content">
                        <image src="/static/images/distribution/distribution_icon7.png" class="png" mode="aspectFit"></image>
                        <text class="coreshop-text-grey">邀请海报</text>
                    </view>
                </view>
            </view>
            
        </view>
    </view>
</template>
<script>
    import { commonUse } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [commonUse],
        data() {
            return {
                showMoney: true, //是否显示金额
                siteName: this.$store.state.config.shopName,
                utilityMenus: {
                    invite: {
                        name: '我的推广',
                        icon: '/static/images/distribution/distribution_icon1.png',
                        router: '/pages/member/agent/team/team'
                    },
                    balance: {
                        name: '佣金明细',
                        icon: '/static/images/distribution/distribution_icon2.png',
                        router: '/pages/member/agent/commissionDetails/commissionDetails'
                    },
                    order: {
                        name: '代理订单',
                        icon: '/static/images/distribution/distribution_icon3.png',
                        router: '/pages/member/agent/order/order'
                    },
                    myStore: {
                        name: '我的店铺',
                        icon: '/static/images/distribution/distribution_icon4.png',
                        router: '/pages/member/agent/myStore/myStore'
                    },
                    storeSetting: {
                        name: '店铺设置',
                        icon: '/static/images/distribution/distribution_icon5.png',
                        router: '/pages/member/agent/storeSetting/storeSetting'
                    },
                    ranking: {
                        name: '代理排行',
                        icon: '/static/images/distribution/distribution_icon6.png',
                        router: '/pages/member/agent/rankings/rankings'
                    },
                    shareLog: {
                        name: '分享记录',
                        icon: '/static/images/distribution/distribution_icon8.png',
                        router: '/pages/member/agent/shareLog/shareLog'
                    },
                },
                info: {}, //分销商信息
                userInfo: {}, // 用户信息
                shareUrl: '/pages/share/jump/jump'
            }
        },
        onShow() {
            var _this = this;
            if (_this.$store.state.config.distributionStore != '1') {
                delete this.utilityMenus.myStore;
                delete this.utilityMenus.storeSetting;
            }
            _this.$u.api.getAgentInfo().then(res => {
                if (res.status) {
                    _this.info = res.data;
                    if (res.data.verifyStatus != 1) { //审核通过
                        _this.$u.route({ type: 'redirectTo', url: '/pages/member/agent/index/index' });
                    }
                    if (_this.$store.state.config.distributionStore == '1') {
                        _this.utilityMenus.myStore.router = '/pages/member/agent/myStore/myStore?store=' + _this.info.store;
                    }
                } else {
                    //报错了
                    _this.$u.toast(res.msg);
                }
            });
        },
        onLoad() {
            this.initData()
        },
        methods: {
            // 是否显示金额
            onEye() {
                this.showMoney = !this.showMoney;
            },
            navigateToHandle(pageUrl) {
                this.$u.route({ url: pageUrl })
            },
            initData() {
                // 获取用户信息
                var _this = this
                this.$u.api.userInfo().then(res => {
                    if (res.status) {
                        _this.userInfo = res.data
                    }
                    this.getShareUrl();
                })
            },
            //去提现
            goWithdraw() {
                this.$u.route('/pages/member/balance/withdrawCash/withdrawCash')
            },
            //获取分享URL
            getShareUrl() {
                let data = {
                    client: 2,
                    url: "/pages/share/jump/jump",
                    type: 1,
                    page: 1,
                };
                let userToken = this.$db.get('userToken');
                if (userToken && userToken != '') {
                    data['token'] = userToken;
                }
                this.$u.api.share(data).then(res => {
                    this.shareUrl = res.data
                });
            },
            // 生成邀请海报
            createPoster() {
                let data = {
                    type: 3,
                    params: {
                        store: this.info.store
                    },
                    page: 11,
                }
                let pageUrl = 'pages/share/jump/jump';
                data.client = 2;

                let userToken = this.$db.get('userToken')
                if (userToken && userToken != '') {
                    data.token = userToken
                }
                this.$u.api.share(data).then(res => {
                    if (res.status) {
                        this.$u.route('/pages/share/sharePoster/sharePoster?poster=' + encodeURIComponent(res.data))
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
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
    @import "panel.scss";
</style>
