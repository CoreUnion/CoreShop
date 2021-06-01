<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="在线充值"></u-navbar>
        <view class="content">
            <view class="content-top">
                <view class="cu-form-group margin-top">
                    <view class="title">当前金额</view>
                    <view class="text-red text-price text-xxl">{{ user.balance || '0'}}</view>
                </view>
                <view class="cu-form-group">
                    <view class="title">储值金额</view>
                    <input placeholder='请输入要储值的金额' v-model="money" focus type="digit"></input>
                </view>
            </view>

            <view class="bg-white coreshop-footer-fixed coreshop-foot-padding-bottom" @click="navigateToHandle">
                <view class="flex padding-sm flex-direction">
                    <button class="cu-btn bg-red">去支付</button>
                </view>
            </view>

        </view>
    </view>
</template>

<script>
    export default {
        data() {
            return {
                user: {}, // 用户信息
                payments: [], // 可用储值方式列表
                money: '', // 储值的金额
                orderType: 2	// 储值类型
            }
        },
        onLoad() {
            this.userInfo()
        },
        methods: {
            // 获取用户信息
            userInfo() {
                this.$u.api.userInfo().then(res => {
                    if (res.status) {
                        this.user = res.data
                    }
                })
            },
            // 去储值
            navigateToHandle() {
                if (!Number(this.money)) {
                    this.$u.toast('请输入要储值的金额')
                } else {
                    this.$u.route('/pages/payment/pay/pay?recharge=' + Number(this.money) + '&type=' + this.orderType)
                }
            }
        }
    }
</script>

<style lang="scss" scoped>
    .coreshop-bottomBox .coreshop-btn { width: 100%; }
</style>
