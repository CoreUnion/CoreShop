<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="支付"></u-navbar>
        <view class="cu-bar bg-white solid-bottom">
            <view class="action">
                <text class="cuIcon-title text-orange"></text> 支付信息
            </view>
            <view class="action">
            </view>
        </view>
        <!--商品信息-->
        <view class="cu-list menu">
            <view class="cu-item">
                <view class="content">
                    <text class="text-grey">订单类型</text>
                </view>
                <view class="action">
                    <text class="text-sm text-gray" v-if="type == 1">商品订单</text>
                    <text class="text-sm text-gray" v-if="type == 2" @click="toRecharge()">充值订单</text>
                    <text class="text-sm text-gray" v-if="type == 3">表单订单</text>
                    <text class="text-sm text-gray" v-if="type == 4">付款码</text>
                    <text class="text-sm text-gray" v-if="type == 5">服务订单</text>
                </view>
            </view>
            <template v-if="type == 1">
                <view class="cu-item">
                    <view class="content">
                        <text class="text-grey">订单编号</text>
                    </view>
                    <view class="action">
                        <text class="text-grey text-sm" v-for="(item, index) in orderInfo.rel" :key="index" @click="goOrderDetail(item.sourceId)">{{ item.sourceId || '' }}</text>
                    </view>
                </view>
                <view class="cu-item">
                    <view class="content">
                        <text class="text-grey">订单金额</text>
                    </view>
                    <view class="action">
                        <text class="text-price text-red text-lg">{{ orderInfo.money || '' }}</text>
                    </view>
                </view>
            </template>
            <template v-else-if="type == 2">
                <view class="cu-item">
                    <view class="content">
                        <text class="text-grey">充值金额</text>
                    </view>
                    <view class="action">
                        <text class="text-price text-red text-lg">{{ recharge || '' }}</text>
                    </view>
                </view>
            </template>
            <template v-else-if="type == 5">
                <view class="cu-item">
                    <view class="content">
                        <text class="text-grey">购买服务</text>
                    </view>
                    <view class="action">
                        <text class="text-sm text-gray">{{ serviceInfo.title || '' }}</text>
                    </view>
                </view>
                <view class="cu-item">
                    <view class="content">
                        <text class="text-grey">服务金额</text>
                    </view>
                    <view class="action">
                        <text class="text-price text-red text-lg">{{ serviceInfo.money || '' }}</text>
                    </view>
                </view>
            </template>
            <template v-else>
                <view class="cu-item">
                    <view class="content">
                        <text class="text-grey">支付金额</text>
                    </view>
                    <view class="action">
                        <text class="text-price text-red text-lg">{{ recharge || '' }}</text>
                    </view>
                </view>
            </template>

        </view>

        <view class="cu-bar bg-white solid-bottom margin-top">
            <view class="action">
                <text class="cuIcon-title text-orange"></text> 请点击选择以下支付方式
            </view>
            <view class="action">
            </view>
        </view>
        <!--支付方式-->
        <view class="content">
            <!-- #ifdef H5 -->
            <payments-by-h5 :orderId="orderId" :recharge="recharge" :type="type" :uid="userInfo.id"></payments-by-h5>
            <!-- #endif -->
            <!-- #ifdef MP-WEIXIN -->
            <payments-by-wx :orderId="orderId" :recharge="recharge" :type="type" :uid="userInfo.id"></payments-by-wx>
            <!-- #endif -->
            <!-- #ifdef MP-ALIPAY -->
            <payments-by-ali :orderId="orderId" :recharge="recharge" :type="type" :uid="userInfo.id"></payments-by-ali>
            <!-- #endif -->
            <!-- #ifdef APP-PLUS || APP-PLUS-NVUE -->
            <payments-by-app :orderId="orderId" :recharge="recharge" :type="type" :uid="userInfo.id"></payments-by-app>
            <!-- #endif -->
            <!-- #ifdef MP-TOUTIAO -->
            <payments-by-tt :orderId="orderId" :recharge="recharge" :type="type" :uid="userInfo.id"></payments-by-tt>
            <!-- #endif -->
        </view>

        <!--提示信息-->
        <view class="text-gray padding-sm text-sm">
            注：如果您在支付中选择的支付方式不适合或异常，请再次选择其他支付方式。
        </view>
        <!-- 登录提示 -->
		<corecms-login-modal></corecms-login-modal>
    </view>
</template>
<script>
    // #ifdef H5
    import paymentsByH5 from '@/components/corecms-payments/corecms-paymentsByH5.vue';
    // #endif

    // #ifdef MP-WEIXIN
    import paymentsByWx from '@/components/corecms-payments/corecms-paymentsByWx.vue';
    // #endif

    // #ifdef MP-ALIPAY
    import paymentsByAli from '@/components/corecms-payments/corecms-paymentsByAli.vue';
    // #endif

    // #ifdef APP-PLUS || APP-PLUS-NVUE
    import paymentsByApp from '@/components/corecms-payments/corecms-paymentsByApp.vue';
    // #endif

    // #ifdef MP-TOUTIAO
    import paymentsByTt from '@/components/corecms-payments/corecms-paymentsByTt.vue';
    // #endif

    import { orders } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [orders],
        data() {
            return {
                orderId: 0,
                recharge: 0,
                serviceId: 0, //服务编号
                type: 1, // 订单类型 1商品订单 2充值订单 5服务订单
                orderInfo: {}, // 订单详情
                userInfo: {}, // 用户信息
                serviceInfo: {}, // 服务信息
                formId: 0
            };
        },
        components: {
            // #ifdef H5
            paymentsByH5,
            // #endif
            // #ifdef MP-WEIXIN
            paymentsByWx,
            // #endif
            // #ifdef MP-ALIPAY
            paymentsByAli,
            // #endif
            // #ifdef APP-PLUS || APP-PLUS-NVUE
            paymentsByApp,
            // #endif
            // #ifdef MP-TOUTIAO
            paymentsByTt
            // #endif
        },
        onLoad(options) {
            console.log(options);
            this.orderId = options.orderId;
            this.serviceId = Number(options.serviceId);
            this.recharge = Number(options.recharge);
            this.type = Number(options.type);
            this.formId = Number(options.formId);
            //this.getOrderInfo ()
            if (this.orderId && this.type == 1) {
                // 商品订单
                this.getOrderInfo();
            } else if (this.recharge && this.type == 2) {
                // 充值订单 获取用户id
                this.getUserInfo();
            } else if (this.formId && (this.type == 3 || this.type == 4)) {
                // 表单订单 id传到订单上
                this.orderId = '' + this.formId;
            } else if (this.type == 5) {
                this.getServiceDetail();
            }
            else {
                this.$refs.uToast.show({ title: '订单支付参数错误', type: 'error', back: true });
            }
        },
        methods: {
            // 获取订单详情
            getOrderInfo() {
                let data = {
                    ids: this.orderId,
                    paymentType: this.type
                };
                this.$u.api.paymentsCheckpay(data).then(res => {
                    if (res.status) {
                        this.orderInfo = res.data;
                        /* console.log(this.orderInfo)
                            if(this.orderInfo.pay_status == 2){
                                this.$u.route({ type: 'redirectTo', url: '/pages/payment/result/result?orderId=' + this.orderInfo.orderId });
                            } */
                    }
                });
            },
            //获取服务详情
            getServiceDetail() {
                let data = {
                    id: this.serviceId
                };
                this.$u.api.getServiceDetail(data).then(res => {
                    if (res.status) {
                        this.serviceInfo = res.data;
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            },
            // 获取用户信息
            getUserInfo() {
                this.$u.api.userInfo().then(res => {
                    if (res.status) {
                        this.userInfo = res.data;
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            },
            // 跳转我的余额页面
            toRecharge() {
                this.$u.route('/pages/member/balance/index');
            }
        }
    };
</script>
<style scoped lang="scss">
    view { box-sizing: border-box; }
    .margin-cell-group { margin-bottom: 20upx; }
    .cell-hd-title { color: #999; }
    .payment-method .cell-item-hd { min-width: 70upx; }
    .payment-method .cell-hd-icon { width: 70upx; height: 70upx; }
    .payment-method .cell-item-bd { border-left: 2upx solid #f0f0f0; padding-left: 30upx; }
    .payment-method .cell-bd-text { font-size: 28upx; color: #666; }
    .payment-method .address { font-size: 24upx; color: #999; }
    .flex-item { display: flex; justify-content: space-between; align-items: center; }
        .flex-item .cell-item-ft { position: relative; top: 0; transform: translateY(0); right: 0; }
</style>
