<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="我的账户"></u-navbar>
        <view class="content">
            <view class='withdrawcash-top'>
                <text class='withdrawcash-title'>账户余额（元）</text>
                <text class='withdrawcash-num'>{{ userInfo.balance }}</text>
            </view>

            <view class="coreshop-list menu card-menu u-margin-top-30">

                <view class="coreshop-list-item arrow" v-if="platform != 'ios' && showRecharge" @click="navigateToHandle('/pages/member/balance/recharge/recharge')">
                    <view class="content">
                        <u-icon name="gift" size="32" class="coreshop-text-grey"></u-icon>
                        <text class="coreshop-text-grey u-margin-left-20">账户充值</text>
                    </view>
                </view>
                <view class="coreshop-list-item arrow">
                    <view class="content" @click="navigateToHandle('/pages/member/balance/withdrawCash/withdrawCash')">
                        <u-icon name="rmb" size="32" class="coreshop-text-yellow"></u-icon>
                        <text class="coreshop-text-grey u-margin-left-20">余额提现</text>
                    </view>
                </view>
                <view class="coreshop-list-item arrow">
                    <view class="content" @click="navigateToHandle('/pages/member/balance/details/details')">
                        <u-icon name="rmb-circle" size="32" class="coreshop-text-olive"></u-icon>
                        <text class="coreshop-text-grey u-margin-left-20">余额明细</text>
                    </view>
                </view>
                <view class="coreshop-list-item arrow">
                    <view class="content" @click="navigateToHandle('/pages/member/balance/cashlist/cashlist')">
                        <u-icon name="list-dot" size="32" class="coreshop-text-orange"></u-icon>
                        <text class="coreshop-text-grey u-margin-left-20">提现记录</text>
                    </view>
                </view>
                <view class="coreshop-list-item arrow">
                    <view class="content" @click="navigateToHandle('/pages/member/balance/bankcard/bankcard')">
                        <u-icon name="coupon" size="32" class="coreshop-text-blue"></u-icon>
                        <text class="coreshop-text-grey u-margin-left-20">我的银行卡</text>
                    </view>
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
    @import "index.scss";
</style>
