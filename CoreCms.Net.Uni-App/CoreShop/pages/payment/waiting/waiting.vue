<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="支付中..."></u-navbar>
        <view class="content">
            <view class="content-c">
                <image class="load-img" src="/static/images/loading.gif" mode=""></image>
                <view class="load-text ">信息加载中.....</view>
            </view>
        </view>
        <!-- 登录提示 -->
		<coreshop-login-modal></coreshop-login-modal>
    </view>
</template>

<script>
    export default {
        data() {
            return {
                type: '',
                openid: '',
                orderId: '',
                state: ''
            }
        },
        onLoad(options) {
            this.orderId = options.order_id
            this.money = Number(options.money)
            this.type = Number(options.type)
            this.uid = Number(options.uid)
            this.state = this.$common.getQueryString('state')
            this.getCode()
        },
        methods: {
            getCode() {
                var code = this.$common.getQueryString('code')
                code && this.getOpenId(code)
            },
            getOpenId(code) {
                let data = {
                    code: code,
                    scope: 2,
                    state: this.state
                }
                //模拟接口
                this.$u.api.getOpenId(data).then(res => {
                    if (res.status) {
                        this.openid = res.data.openid
                        this.toPayHandler('wechatpay')
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
            checkWXJSBridge(data) {
                let that = this
                let interval = setInterval(() => {
                    if (typeof window.WeixinJSBridge != 'undefined') {
                        clearTimeout(interval)
                        that.onBridgeReady(data)
                    }
                }, 200)
            },
            onBridgeReady(data) {
                var _this = this
                window.WeixinJSBridge.invoke(
                    'getBrandWCPayRequest',
                    {
                        appId: data.appid, // 公众号名称，由商户传入
                        timeStamp: data.timeStamp, // 时间戳，自1970年以来的秒数
                        nonceStr: data.nonceStr, // 随机串
                        package: data.package,
                        signType: data.signType, // 微信签名方式：
                        paySign: data.paySign // 微信签名
                    },
                    function (res) {
                        if (res.err_msg === 'get_brand_wcpay_request:ok') {
                            this.$refs.uToast.show({ title: '支付成功', type: 'success' })
                        } else if (res.err_msg === 'get_brand_wcpay_request:cancel') {
                            _this.$u.toast('取消支付')
                        } else {
                            _this.$u.toast('支付失败')
                        }
                        setTimeout(() => {
                            this.$u.route({ type: 'redirectTo', url: '/pages/payment/result/result?id=' + data.payment_id });
                        }, 1000)
                    }
                )
            },
            toPayHandler(code) {
                let data = {
                    payment_code: code,
                    payment_type: this.type
                }
                data['ids'] = (this.type == 1 || this.type == 5 || this.type == 6) ? this.orderId : this.uid
                //data['ids'] = this.type == 1 ? this.orderId : this.uid

                if (this.type == 1 && this.orderId) {
                    // 微信jsapi支付
                    if (this.openid) {
                        data['params'] = {
                            trade_type: 'JSAPI_OFFICIAL',
                            openid: this.openid
                        }
                    }
                } else if (this.type == 2 && this.money) {
                    if (this.openid) {
                        data['params'] = {
                            trade_type: 'JSAPI_OFFICIAL',
                            money: this.money,
                            openid: this.openid
                        }
                    }
                } else if ((this.type == 5 || this.type == 6) && this.recharge) {
                    data['params'] = {
                        trade_type: 'JSAPI_OFFICIAL',
                        openid: this.openid,
                    }
                }
                this.$u.api.pay(data).then(res => {
                    if (res.status) {
                        const data = res.data
                        this.checkWXJSBridge(data)
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            }
        }
    }
</script>

<style scoped lang="scss">
</style>
