<template>
    <view>
        <u-toast ref="uToast" />
        <form v-for="item in payments" :key="item.code" @submit="toPayHandler" v-if="!(type == 2 && item.code == 'balancepay')">
            <input name="code" :value="item.code" class="hide">
            <button class="u-reset-button coreshop-list menu-avatar  padding-xl radius shadow shadow-lg bg-blue margin-top" form-type="submit">
                <view class="coreshop-list-item">
                    <view class="coreshop-avatar radius">
                        <u-image width="68rpx" height="68rpx" :src="item.icon" :lazy-load="false" :fade="false" duration="0"></u-image>
                    </view>
                    <view class="content">
                        <view class="coreshop-text-grey">{{ item.name }}</view>
                        <view class="coreshop-text-gray u-font-sm flex u-text-left">
                            <view class="u-line-1">
                                {{ item.memo }}
                            </view>
                        </view>
                    </view>
                    <view class="action">
                        <u-tag text="选择此支付方式" mode="dark" type="error" shape="circle" />
                    </view>
                </view>
            </button>
        </form>
        <view class="payment-pop" v-show="popShow">
            <view class="payment-pop-c">
                <image src="/static/images/payments/wait-pay.png"></image>
                <view class="text">支付中，请稍后...</view>
            </view>
            <view class="payment-pop-b">
                <button class="coreshop-btn coreshop-btn-c" @click="popHide">支付失败</button>
                <button class="coreshop-btn coreshop-btn-o" @click="popHide">支付成功</button>
            </view>
        </view>
    </view>
</template>

<script>
    export default {
        props: {
            // 如果是商品订单此参数必须
            orderId: {
                type: String,
                default() {
                    return ''
                }
            },
            // 如果是充值订单此参数必须
            recharge: {
                type: Number,
                default() {
                    return 0
                }
            },
            // 用户id
            uid: {
                type: Number,
                default() {
                    return 0
                }
            },
            // 订单类型
            type: {
                type: Number,
                default() {
                    return 1
                }
            }
        },
        data() {
            return {
                payments: [],
                popShow: false
            }
        },
        mounted() {
            this.getPayments()
        },
        methods: {
            // 获取可用支付方式列表
            getPayments() {
                this.$u.api.paymentList().then(res => {
                    if (res.status) {
                        this.payments = this.formatPayments(res.data)
                    }
                })
            },
            // 支付方式处理
            formatPayments(payments) {
                // 过滤支付宝支付
                payments = payments.filter(item => item.code !== 'alipay')

                // 如果是充值订单,服务订单 过滤余额支付 过滤非线上支付方式
                if (this.type === 2 || this.type === 5) {
                    payments = payments.filter(item => item.code !== 'balancepay' || item.isOnline === false)
                }

                // 设置logo图片
                payments.forEach(item => {
                    this.$set(item, 'icon', '/static/images/payments/' + item.code + '.png')
                })

                return payments
            },
            // 用户点击支付方式处理
            toPayHandler(e) {
                this.popShow = true;
                let code = e.target.value.code;

                let data = {
                    payment_code: code,
                    payment_type: this.type,
                    params: {}
                }
                data['ids'] = (this.type == 1 || this.type == 3 || this.type == 4 || this.type == 5) ? this.orderId : this.uid

                // 判断订单支付类型
                if (this.type == 2 && this.recharge) {
                    data['params'] = {
                        money: this.recharge
                    }
                } else if ((this.type == 3 || this.type == 4) && this.recharge) {
                    data['params'] = {
                    }
                }
                let _this = this
                switch (code) {
                    case 'wechatpay':
                        this.$u.api.pay(data).then(res => {
                            if (res.status) {
                                if (res.otherData.checkSceneResult && res.otherData.checkSceneResult.is_matched == 1) {
                                    wx.requestOrderPayment({
                                        timeStamp: res.data.timeStamp,
                                        nonceStr: res.data.nonceStr,
                                        package: res.data.package,
                                        signType: res.data.signType,
                                        paySign: res.data.paySign,
                                        orderInfo: res.otherData.orderInfo,
                                        success: function (e) {
                                            console.log(e);
                                            if (e.errMsg === 'requestOrderPayment:ok') {
                                                _this.$u.route({ type: 'redirectTo', url: '/pages/payment/result/result?id=' + res.data.paymentId });
                                            }
                                        },
                                        fail: function (res) {
                                            if (res.errMsg === 'requestPayment:fail cancel') {
                                                _this.popHide();
                                                _this.$u.toast("您已经取消此方式支付,可继续选择其他方式支付。")
                                            }
                                            console.log(res);
                                        }
                                    });
                                } else {
                                    uni.requestPayment({
                                        provider: 'wxpay',
                                        timeStamp: res.data.timeStamp,
                                        nonceStr: res.data.nonceStr,
                                        package: res.data.package,
                                        signType: res.data.signType,
                                        paySign: res.data.paySign,
                                        success: function (e) {
                                            console.log(e);
                                            if (e.errMsg === 'requestPayment:ok') {
                                                _this.$u.route({ type: 'redirectTo', url: '/pages/payment/result/result?id=' + res.data.paymentId });
                                            }
                                        },
                                        fail: function (res) {
                                            if (res.errMsg === 'requestPayment:fail cancel') {
                                                _this.popHide();
                                                _this.$u.toast("您已经取消此方式支付,可继续选择其他方式支付。")
                                            }
                                            console.log(res);
                                        }
                                    });
                                }
                            } else {
                                _this.$u.toast(res.msg)
                                _this.popHide();
                            }
                        })
                        break
                    case 'balancepay':
                        /**
                         *  用户余额支付
                         *
                         */
                        this.$u.api.pay(data).then(res => {
                            if (res.status) {
                                this.$u.route({ type: 'redirectTo', url: '/pages/payment/result/result?id=' + res.data.paymentId });
                            } else {
                                this.$u.toast(res.msg)
                                this.popHide();
                            }
                        })
                        break
                    case 'offline':
                        /**
                         * 线下支付
                         */
                        this.$common.modelShow('线下支付说明', '请联系客服进行线下支付', () => { }, false, '取消', '确定')
                        break
                }

            },
            // 支付中显示隐藏
            popHide() {
                this.popShow = false
            }
        }

    }
</script>

<style lang="scss">
    .coreshop-avatar { background: #fff; }
</style>
