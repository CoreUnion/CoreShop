<template>
    <view style="width:100%;height: 100%;">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="代理面板"></u-navbar>
        <view class="commission-wrap">
            <!--<cu-custom isBack="true" bgColor="bg-white">
                <block slot="backText">代理面板</block>
            </cu-custom>-->
            <!-- 用户资料 -->
            <view class="user-card bg-blue">
                <view class="card-top x-bc">
                    <view class="x-f">
                        <view class="head-img-box"><image class="head-img" :src="userInfo.avatarImage" mode=""></image></view>
                        <view class="y-start">
                            <view class="user-name">{{ userInfo.nickName }}</view>
                            <view class="user-info-box x-f">
                                <view class="x-f" v-if="info.gradeName">
                                    <text class="cu-tag bg-purple sm radius">{{ info.gradeName }}</text>
                                </view>
                            </view>
                        </view>
                    </view>
                    <view class="y-start">
                        <view class="y-f">
                            <button class="cu-btn log-btn" @tap="navigateToHandle(utilityMenus.balance.router)">明细</button>
                            <button class="cu-btn look-btn" @tap="onEye">
                                <text v-if="showMoney" class="cuIcon-attentionfill"></text>
                                <text v-else class="cuIcon-attentionforbidfill"></text>
                            </button>
                        </view>
                    </view>
                </view>
                <view class="card-bottom x-f">
                    <view class="flex-sub y-start">
                        <view class="item-title">已结算金额</view>
                        <view class="item-detail">{{ showMoney ? info.settlementAmount || '0.00' : '***' }}</view>
                    </view>
                    <view class="flex-sub y-start" style="align-items: center;">
                        <view class="item-title">待入账佣金</view>
                        <view class="item-detail">{{ showMoney ? info.freezeAmount || '0.00' : '***' }}</view>
                    </view>
                    <view class="flex-sub y-start" style="align-items: flex-end;">
                        <view class="item-title">本月订单数</view>
                        <view class="item-detail">{{ showMoney ? info.currentMonthOrder || '0.00' : '***' }}</view>
                    </view>
                </view>
            </view>

            <view class="cu-list menu sm-border card-menu">
                <view class="cu-item arrow" v-for="(menu, index) in utilityMenus" :key="index" @tap="navigateToHandle(menu.router)">
                    <view class="content">
                        <image :src="menu.icon" class="png" mode="aspectFit"></image>
                        <text class="text-grey">{{ menu.name }}</text>
                    </view>
                </view>
                <!-- 分享二维码 -->
                <view class="cu-item arrow"  @click="createPoster()">
                    <view class="content">
                        <image src="/static/images/distribution/distribution_icon7.png" class="png" mode="aspectFit"></image>
                        <text class="text-grey">邀请海报</text>
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
                        router: '/pages/member/agent/team'
                    },
                    balance: {
                        name: '佣金明细',
                        icon: '/static/images/distribution/distribution_icon2.png',
                        router: '/pages/member/agent/commissionDetails'
                    },
                    order: {
                        name: '代理订单',
                        icon: '/static/images/distribution/distribution_icon3.png',
                        router: '/pages/member/agent/order'
                    },
                    myStore: {
                        name: '我的店铺',
                        icon: '/static/images/distribution/distribution_icon4.png',
                        router: '/pages/member/agent/myStore'
                    },
                    storeSetting: {
                        name: '店铺设置',
                        icon: '/static/images/distribution/distribution_icon5.png',
                        router: '/pages/member/agent/storeSetting'
                    },
                    ranking: {
                        name: '代理排行',
                        icon: '/static/images/distribution/distribution_icon6.png',
                        router: '/pages/member/agent/rankings'
                    }
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
                        _this.$u.route({ type: 'redirectTo', url: '/pages/member/agent/index' });
                    }
                    if (_this.$store.state.config.distributionStore == '1') {
                        _this.utilityMenus.myStore.router = '/pages/member/agent/myStore?store=' + _this.info.store;
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
                this.$u.route('/pages/member/balance/withdrawCash')
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
                // #ifdef H5
                data.client = 1;
                data.url = apiBaseUrl + 'wap/' + pageUrl;
                // #endif
                // #ifdef MP-WEIXIN
                data.client = 2;
                data.url = pageUrl;
                // #endif
                // #ifdef MP-ALIPAY
                data.client = 3;
                data.url = pageUrl;
                // #endif
                // #ifdef APP-PLUS || APP-PLUS-NVUE
                data.client = 5;
                data.url = apiBaseUrl + 'wap/' + pageUrl;
                // #endif
                // #ifdef MP-TOUTIAO
                data.client = 6;
                // #endif
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
<style lang="scss">
    .user-card { width: 690rpx; min-height: 350rpx; border-radius: 14rpx; margin: 30rpx; background-image: url('/static/images/common/bg.png'); background-size: cover; background-position: center; padding-top: 10rpx; position: relative; }
        .user-card::after { content: ""; position: absolute; z-index: -1; background-color: inherit; width: 100%; height: 100%; left: 0; bottom: -10%; border-radius: 10upx; opacity: 0.2; transform: scale(0.9, 0.9); }

        .user-card .card-top { padding: 40rpx 40rpx 30rpx; margin-bottom: 30rpx; border-bottom: 1px solid rgba(255, 255, 255, 0.12); }
            .user-card .card-top .user-name { font-size: 26rpx; font-weight: 500; color: #ffffff; line-height: 30rpx; margin-bottom: 20rpx; }
            .user-card .card-top .cu-btn { padding: 0; background: none; }
            .user-card .card-top .log-btn { width: 83rpx; height: 41rpx; border: 1rpx solid rgba(255, 255, 255, 0.33); border-radius: 21rpx; font-size: 22rpx; font-weight: 400; color: #ffffff; }
            .user-card .card-top .look-btn { color: #fff; font-size: 40rpx; }
                .user-card .card-top .look-btn .cuIcon-attentionfill,
                .user-card .card-top .look-btn .cuIcon-attentionforbidfill { line-height: 50rpx; margin-top: 20rpx; }
                .user-card .card-top .look-btn .cuIcon-attentionfill { line-height: 50rpx; margin-top: 20rpx; }
        .user-card .head-img-box { margin-right: 26rpx; width: 76rpx; height: 76rpx; border-radius: 50px; position: relative; background: #fff; padding: 10rpx; background-clip: padding-box; }
            .user-card .head-img-box .head-img { width: 66rpx; height: 66rpx; border-radius: 50%; position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); }
        .user-card .user-info-box .tag-box { background: rgba(0, 0, 0, 0.2); border-radius: 21rpx; line-height: 38rpx; padding-right: 10rpx; }
            .user-card .user-info-box .tag-box .tag-img { width: 36rpx; height: 36rpx; margin-right: 6rpx; border-radius: 50%; }
            .user-card .user-info-box .tag-box .tag-title { font-size: 20rpx; font-family: PingFang SC; font-weight: 500; color: white; line-height: 20rpx; }
        .user-card .card-bottom { margin: 0 40rpx 40rpx; }
            .user-card .card-bottom .item-title { font-size: 24rpx; font-family: PingFang SC; font-weight: 400; color: #ffffff; line-height: 30rpx; }
            .user-card .card-bottom .item-detail { font-size: 40rpx; font-family: DIN; font-weight: 500; color: #fefefe; line-height: 30rpx; margin-top: 30rpx; }


    .menu-box { flex-wrap: wrap; margin: 30rpx; position: fixed; width: 750rpx; bottom: 0; z-index: 10; }
        .menu-box .menu-item { width: 172.5rpx; margin-bottom: 50rpx; }
            .menu-box .menu-item .item-img { width: 68rpx; height: 68rpx; }
            .menu-box .menu-item .item-title { font-size: 24rpx; font-weight: 600; color: #fff; line-height: 30rpx; margin-top: 16rpx; }
</style>
