<template>
    <view class="invite">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="邀请好友"></u-navbar>
        <image class="invite-bg" :src="$globalConstVars.apiFilesUrl+'/static/images/my/invite-bg.png'" mode=""></image>
        <view class="invite-c">
            <view class="invite-w">
                <view class='invite-w-t'>我的专属邀请码</view>
                <text class='invite-w-num'>{{code}}</text>
                <view class='invite-w-detail'>快去分享您的邀请码吧，让更多的好友加入到【{{appTitle}}】，您也可以获得丰厚的奖励！</view>
                <view class='invite-w-bot'>
                    <view bindtap='commission' @click="toMoney">
                        <text class="cuIcon-coin coreshop-text-red" style="font-size: 44rpx;"></text>
                        <text class='invite-w-bot-red'>￥{{money}}元</text>
                        <text class='invite-w-bot-gray'>邀请收益</text>
                    </view>
                    <view bindtap='recommendUserList' @click="toList">
                        <text class="cuIcon-friend coreshop-text-red" style="font-size: 44rpx;"></text>
                        <text class='invite-w-bot-red'>{{number}}人</text>
                        <text class='invite-w-bot-gray'>邀请人数</text>
                    </view>
                </view>
            </view>
            <view class="invite-w" v-if="!isSuperior">
                <text class='invite-w-t-blue'>谁推荐你的？</text>
                <input class='invite-w-input' placeholder='请输入推荐人邀请码' v-model="inviteKey" />
                <view class='invite-w-btn' @click="setMyInvite">提交</view>
            </view>
            <view class='invite-btn'>
                <button class='share coreshop-btn' open-type="share">
                    <u-icon name="weixin-fill" size="90" top="4"></u-icon>
                </button>
                <button class='share coreshop-btn' @click="createPoster()">
                    <u-icon name="moments" size="90" top="4"></u-icon>
                </button>
            </view>
        </view>
    </view>

</template>
<script>

    export default {
        data() {
            return {
                code: '',
                money: 0,
                number: 0,
                isSuperior: false,
                inviteKey: '',
                imageUrl: '/static/images/ShareImage.png',
                shareUrl: '/pages/share/jump/jump'
            }
        },
        computed: {
            appTitle() {
                return this.$store.state.config.shopName;
            }
        },
        onShow() {
            this.getInviteData();
            this.ifwxl()
        },
        methods: {
            // 判断是不是微信浏览器
            ifwxl() {
                this.ifwx = this.$common.isWeiXinBrowser()
            },
            //获取数据
            getInviteData() {
                this.$u.api.myInvite(null).then(res => {
                    this.code = res.data.code;
                    this.money = res.data.money;
                    this.number = res.data.number;
                    this.isSuperior = res.data.isSuperior;
                });
                this.getShareUrl();
            },
            //去佣金明细
            toMoney() {
                this.$u.route('/pages/member/balance/details/details?status=5');
            },
            //去邀请列表
            toList() {
                this.$u.route('/pages/member/invite/list');
            },
            //填写设置要求
            setMyInvite() {
                let data = {
                    id: this.inviteKey
                }
                this.$u.api.setMyInvite(data).then(res => {
                    if (res.status) {
                        this.$refs.uToast.show({ title: '邀请码填写成功', type: 'success' })
                        this.isSuperior = true;
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            },
            // 生成邀请海报
            createPoster() {
                let data = {
                    page: 1,//首页
                    type: 3,//海报
                }
                let userToken = this.$db.get('userToken')
                if (userToken) {
                    data.token = userToken
                }
                data.client = 2;
                data.url = 'pages/share/jump/jump'
                this.$u.api.share(data).then(res => {
                    if (res.status) {
                        this.$u.route('/pages/share/sharePoster/sharePoster?poster=' + encodeURIComponent(res.data))
                    } else {
                        this.$u.toast(res.msg)
                    }
                });
            },
            //复制URL链接
            copyUrl() {
                let data = {
                    page: 1,//首页
                    type: 1,//海报
                }
                let userToken = this.$db.get('userToken')
                if (userToken) {
                    data.token = userToken
                }
                data.client = 2;
                data.url = 'pages/share/jump/jump'
                let _this = this;
                this.$u.api.share(data).then(res => {
                    if (res.status) {
                        uni.setClipboardData({
                            data: res.data,
                            success: function (data) {
                                _this.$refs.uToast.show({ title: '复制成功', type: 'success' })
                            },
                            fail: function (err) {
                                _this.$u.toast('复制分享URL失败');
                            }
                        })
                    } else {
                        _this.$u.toast('复制分享URL失败');
                    }
                });
            },
            //获取分享URL
            getShareUrl() {
                let data = {
                    client: 2,
                    url: "pages/share/jump/jump",
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
<style lang="scss">
    @import "index.scss";
</style>
