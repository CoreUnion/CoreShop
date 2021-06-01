<template>
    <view>
        <u-toast ref="uToast" />
        <view v-for="item in payments" :key="item.code" @click="toPayHandler(item.code)" v-if="!(type == 2 && item.code == 'balancepay')">
            <view class="u-reset-button cu-list menu-avatar  padding-xl radius shadow shadow-lg bg-blue margin-top">
                <view class="cu-item">
                    <view class="cu-avatar radius">
                        <u-image width="68rpx" height="68rpx" :src="item.icon" :lazy-load="false" :fade="false" duration="0"></u-image>
                    </view>
                    <view class="content">
                        <view class="text-grey">{{ item.name }}</view>
                        <view class="text-gray text-sm flex">
                            <view class="text-cut">
                                {{ item.memo }}
                            </view>
                        </view>
                    </view>
                    <view class="action">
                        <view class="cu-tag round bg-red sm">选择此支付方式</view>
                    </view>
                </view>
            </view>
        </view>

        <view class="payment-pop" v-show="popShow">
            <view class="payment-pop-c">
                <image src="/static/images/payments/wait-pay.png" style="width: 30px;height: 30px;"></image>
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
    import { apiBaseUrl } from '@/common/setting/constVarsHelper.js';
    export default {
        props: {
            // 如果是商品订单此参数必须
            orderId: {
                type: String,
                default() {
                    return '';
                }
            },
            // 如果是充值订单此参数必须
            recharge: {
                type: Number,
                default() {
                    return 0;
                }
            },
            // 用户id
            uid: {
                type: Number,
                default() {
                    return 0;
                }
            },
            // 订单类型
            type: {
                type: Number,
                default() {
                    return 1;
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
            this.getPayments();
        },
        methods: {
            // 获取可用支付方式列表
            getPayments() {
                this.$u.api.paymentList().then(res => {
                    if (res.status) {
                        this.payments = this.formatPayments(res.data);
                    }
                })
            },
            // 支付方式处理
            formatPayments(payments) {
                payments = payments.filter(item => item.code !== 'wechatpay');
                // 如果是充值订单 过滤余额支付 过滤非线上支付方式
                if (this.type === 2) {
                    payments = payments.filter(item => item.code !== 'balancepay' || item.isOnline === false)
                }
                // 设置logo图片
                payments.forEach(item => {
                    this.$set(item, 'icon', '/static/images/payments/' + item.code + '.png')
                });

                return payments;
            },
            // 用户点击支付方式处理
            toPayHandler(code) {
                this.popShow = true;
                let data = {
                    payment_code: code,
                    payment_type: this.type
                }
                data['ids'] = (this.type == 1 || this.type == 5 || this.type == 6) ? this.orderId : this.uid
                // 判断订单支付类型
                if (this.type == 2 && this.recharge) {
                    data['params'] = {
                        money: this.recharge,
                        trade_type: 'JSAPI'
                    }
                } else if ((this.type == 5 || this.type == 6) && this.recharge) {
                    data['params'] = {
                        trade_type: 'JSAPI',
                    }
                } else {
                    data['params'] = {
                        trade_type: 'JSAPI'
                    }
                }
                let _this = this;
                switch (code) {
                    case 'alipay':
                        this.$u.api.pay(data).then(res => {
                            if (res.status) {
                                uni.requestPayment({
                                    provider: 'alipay',
                                    tradeNO: res.data.trade_no,
                                    success: function (e) {
                                        if (e.errMsg === 'requestPayment:ok') {
                                            _this.$refs.uToast.show({
                                                title: res.msg, type: 'success', callback: function () {
                                                    _this.$u.route({ type: 'redirectTo', url: '/pages/payment/result/result?id=' + res.data.paymentId });
                                                }
                                            })
                                        }
                                    },
                                    fail: function (res) {
                                        console.log(res);
                                    }
                                });
                            } else {
                                this.$u.toast(res.msg);
                                this.popHide();
                            }
                        })
                        break
                    case 'balancepay':
                        //用户余额支付
                        this.$u.api.pay(data).then(res => {
                            if (res.status) {
                                this.$u.route({ type: 'redirectTo', url: '/pages/payment/result/result?id=' + res.data.paymentId });
                            } else {
                                this.$u.toast(res.msg);
                                this.popHide();
                            }
                        })
                        break;
                    case 'offline':
                        //线下支付
                        this.$common.modelShow('线下支付说明', '请联系客服进行线下支付', () => { }, false, '取消', '确定')
                        break;
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
    .cu-list.menu-avatar { display: block; overflow: hidden; border-radius: 10upx; margin: 20rpx; }
        .cu-list.menu-avatar > .cu-item .content { position: absolute; left: 116rpx; width: calc(100% - 66rpx - 60rpx - 120rpx - 20rpx); line-height: 1.6em; }
        .cu-list.menu-avatar > .cu-item .action { width: 200rpx; text-align: center; }

    .no-show { display: none; }
    .payment-wx .coreshop-btn { background-color: #fff; line-height: 1.7; padding: 0; width: 724upx; position: relative; display: flex; align-items: center; }
    .payment-pop { position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); width: 400rpx; height: 272rpx; background-color: #fff; text-align: center; box-shadow: 0 0 20rpx #ccc; }

    .payment-pop-c { padding: 50rpx 30rpx; font-size: 32rpx; color: #999; }
        .payment-pop-c image { width: 60upx; height: 60upx; }
    .payment-pop-b { position: absolute; bottom: 0; display: flex; width: 100%; justify-content: space-between; }
        .payment-pop-b .coreshop-btn { flex: 1; justify-content: center; }
        .payment-pop-b .coreshop-btn-o { background-color: #FF7159; }
    .payment-pop .text { font-size: 24upx; }
</style>
