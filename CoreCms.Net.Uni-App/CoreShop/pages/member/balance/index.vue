<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="我的账户"></u-navbar>
        <view class="content">
            <view class='withdrawcash-top'>
                <text class='withdrawcash-title'>账户余额（元）</text>
                <text class='withdrawcash-num'>{{ userInfo.balance }}</text>
            </view>

            <view class="cu-list menu sm-border card-menu margin-top">

                <view class="cu-item arrow" v-if="platform != 'ios' && showRecharge" @click="navigateToHandle('/pages/member/balance/recharge')">
                    <view class="content">
                        <text class="cuIcon-recharge text-grey"></text>
                        <text class="text-grey">账户充值</text>
                    </view>
                </view>
                <view class="cu-item arrow" @click="navigateToHandle('/pages/member/balance/withdrawCash')">
                    <button class="cu-btn content">
                        <text class="cuIcon-moneybag text-yellow"></text>
                        <text class="text-grey">余额提现</text>
                    </button>
                </view>
                <view class="cu-item arrow" @click="navigateToHandle('/pages/member/balance/details')">
                    <button class="cu-btn content">
                        <text class="cuIcon-refund text-olive"></text>
                        <text class="text-grey">余额明细</text>
                    </button>
                </view>
                <view class="cu-item arrow">
                    <navigator class="content" @click="navigateToHandle('/pages/member/balance/cashlist')">
                        <text class="cuIcon-list text-orange"></text>
                        <text class="text-grey">提现记录</text>
                    </navigator>
                </view>
                <view class="cu-item arrow">
                    <navigator class="content" @click="navigateToHandle('/pages/member/balance/bankcard')">
                        <text class="cuIcon-card text-blue"></text>
                        <text class="text-grey">我的银行卡</text>
                    </navigator>
                </view>
            </view>

        </view>
    </view>

</template>

<script>
    export default {
        data() {
            return {
                userInfo: {},
                platform: 'ios',
            }
        },
        computed: {
            showRecharge() {
                return this.$store.state.config.showStoreBalanceRechargeSwitch === 1;
            }
        },
        onShow() {
            this.getUserInfo();
        },
        methods: {
            // 获取用户信息
            getUserInfo() {
                let _this = this;
                uni.getSystemInfo({
                    success: function (res) {
                        _this.platform = res.platform;
                    }
                });

                this.$u.api.userInfo().then(res => {
                    if (res.status) {
                        this.userInfo = res.data
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
            // 页面跳转
            navigateToHandle(pageUrl) {
                this.$u.route(pageUrl)
            }
        }
    }
</script>

<style lang="scss" scoped>
    .withdrawcash-top { padding: 40upx 26upx; background-color: #FF7159; color: #fff; }
    .withdrawcash-title { font-size: 28upx; display: block }
    .withdrawcash-num { font-size: 70upx; display: block; margin-top: 20upx; margin-left: 50upx; }
</style>
