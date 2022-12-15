<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="支付"></u-navbar>
        <view class="coreshop-bg-white coreshop-solid-bottom u-padding-30">
            <u-icon name="error-circle" size="28" class="coreshop-text-orange" label="支付信息"></u-icon>
        </view>
        <!--商品信息-->
        <view class="coreshop-list menu">
            <view class="coreshop-list-item">
                <view class="content">
                    <text class="coreshop-text-grey">订单类型</text>
                </view>
                <view class="action">
                    <text class="u-font-sm coreshop-text-gray" v-if="type == 1">商品订单</text>
                    <text class="u-font-sm coreshop-text-gray" v-if="type == 2" @click="toRecharge()">充值订单</text>
                    <text class="u-font-sm coreshop-text-gray" v-if="type == 3">表单订单</text>
                    <text class="u-font-sm coreshop-text-gray" v-if="type == 4">付款码</text>
                    <text class="u-font-sm coreshop-text-gray" v-if="type == 5">服务订单</text>
                </view>
            </view>
            <template v-if="type == 1">
                <view class="coreshop-list-item">
                    <view class="content">
                        <text class="coreshop-text-grey">订单编号</text>
                    </view>
                    <view class="action">
                        <text class="coreshop-text-grey u-font-sm" v-for="(item, index) in orderInfo.rel" :key="index" @click="goOrderDetail(item.sourceId)">{{ item.sourceId || '' }}</text>
                    </view>
                </view>
                <view class="coreshop-list-item">
                    <view class="content">
                        <text class="coreshop-text-grey">订单金额</text>
                    </view>
                    <view class="action">
                        <text class="coreshop-text-price coreshop-text-red u-font-lg">{{ orderInfo.money || '' }}</text>
                    </view>
                </view>
            </template>
            <template v-else-if="type == 2">
                <view class="coreshop-list-item">
                    <view class="content">
                        <text class="coreshop-text-grey">充值金额</text>
                    </view>
                    <view class="action">
                        <text class="coreshop-text-price coreshop-text-red u-font-lg">{{ recharge || '' }}</text>
                    </view>
                </view>
            </template>
            <template v-else-if="type == 5">
                <view class="coreshop-list-item">
                    <view class="content">
                        <text class="coreshop-text-grey">购买服务</text>
                    </view>
                    <view class="action">
                        <text class="u-font-sm coreshop-text-gray">{{ serviceInfo.title || '' }}</text>
                    </view>
                </view>
                <view class="coreshop-list-item">
                    <view class="content">
                        <text class="coreshop-text-grey">服务金额</text>
                    </view>
                    <view class="action">
                        <text class="coreshop-text-price coreshop-text-red u-font-lg">{{ serviceInfo.money || '' }}</text>
                    </view>
                </view>
            </template>
            <template v-else>
                <view class="coreshop-list-item">
                    <view class="content">
                        <text class="coreshop-text-grey">支付金额</text>
                    </view>
                    <view class="action">
                        <text class="coreshop-text-price coreshop-text-red u-font-lg">{{ recharge || '' }}</text>
                    </view>
                </view>
            </template>

        </view>

        <view class="coreshop-bg-white coreshop-solid-bottom u-padding-30  u-margin-top-30">
            <u-icon name="checkmark-circle" size="28" class="coreshop-text-orange" label="请点击选择以下支付方式"></u-icon>
        </view>

        <!--支付方式-->
        <view class="content">
            <payments-by-wx :orderId="orderId" :recharge="recharge" :type="type" :uid="userInfo.id"></payments-by-wx>
        </view>

        <!--提示信息-->
        <view class="coreshop-text-gray u-padding-20 u-font-sm">
            注：如果您在支付中选择的支付方式不适合或异常，请再次选择其他支付方式。
        </view>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>
<script>
    import paymentsByWx from '@/pages/payment/components/coreshop-paymentsByWx.vue';

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
                formId: 0,
            };
        },
        components: {
            paymentsByWx
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
                this.$u.route('/pages/member/balance/index/index');
            }
        }
    };
</script>
<style lang="scss">
    @import "pay.scss";
</style>
