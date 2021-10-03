<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="消息订阅"></u-navbar>
        <view class="content">
            <view class="coreshop-content-top">
                <view class='coreshop-cell-group right-img'>
                    <view class='coreshop-cell-item' v-for="(item, i) in msgList" :key="i" v-if="item.status">
                        <view class='coreshop-cell-item-hd'>
                            <view class='coreshop-cell-hd-title'>{{item.name}}</view>
                            <view class='cell-hd-desc'>{{item.desc}}</view>
                        </view>
                        <view class='coreshop-cell-item-ft'>
                            <view v-if="!item.is" class='subscription-btn' @click="subscription(item.func, item.tmpl)">添加通知</view>
                            <view v-if="item.is" class='subscription-btn isTrue' @click="subscription(item.func, item.tmpl)">已加通知</view>
                        </view>
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
                msgList: [
                    {
                        name: '下单通知',
                        desc: '商城下单成功后通知我',
                        func: 'order',
                        tmpl: '',
                        status: false,
                        is: false
                    },
                    {
                        name: '支付通知',
                        desc: '订单支付后通知我',
                        func: 'pay',
                        tmpl: '',
                        status: false,
                        is: false
                    },
                    {
                        name: '待付通知',
                        desc: '未支付订单取消前通知我',
                        func: 'cancel',
                        tmpl: '',
                        status: false,
                        is: false
                    },
                    {
                        name: '发货通知',
                        desc: '订单发货后通知我',
                        func: 'ship',
                        tmpl: '',
                        status: false,
                        is: false
                    },
                    {
                        name: '售后通知',
                        desc: '订单售后结果通知我',
                        func: 'aftersale',
                        tmpl: '',
                        status: false,
                        is: false
                    },
                    {
                        name: '退款通知',
                        desc: '售后退款结果通知我',
                        func: 'refund',
                        tmpl: '',
                        status: false,
                        is: false
                    },
                ]
            }
        },
        onShow() {
            this.getSubscriptionTmplIds();
        },
        methods: {
            //获取模板
            getSubscriptionTmplIds: function () {
                this.$u.api.getSubscriptionTmplIds(null).then(res => {
                    if (res.status) {
                        for (let i = 0; i < this.msgList.length; i++) {
                            for (var j = 0; j < res.data.length; j++) {
                                if (this.msgList[i].func == res.data[j].templateTitle) {
                                    this.msgList[i].tmpl = res.data[j].templateId;
                                    this.msgList[i].is = res.data[j].is;
                                }
                            }
                            if (this.msgList[i].tmpl != '') {
                                this.msgList[i].status = true;
                            }
                        }
                    } else {
                        this.$u.toast('消息订阅配置信息获取失败');
                    }
                });
            },
            //发起订阅
            subscription: function (func, tmpl) {
                console.log('进入订阅发起');
                let _this = this;
                uni.requestSubscribeMessage({
                    tmplIds: [tmpl],
                    success(res) {
                        if (res.errMsg == "requestSubscribeMessage:ok") {
                            let data = {
                                'templateId': tmpl,
                                'status': res[tmpl]
                            }
                            _this.$u.api.setSubscriptionStatus(data).then(res => {
                                _this.getSubscriptionTmplIds();
                            });
                        } else {
                            _this.$refs.uToast.show({
                                title: '操作失败，请稍候重试！', type: 'error', callback: function () {
                                    _this.getSubscriptionTmplIds();
                                }
                            });
                        }
                    }, fail(res) {
                        _this.$u.toast(res.errMsg);
                    }
                });
            }
        }
    }
</script>
<style lang="scss" scoped>
    view { box-sizing: border-box; }
    .agreement { position: fixed; bottom: 30rpx; width: 100%; margin: 20rpx 0; text-align: center; }
    .coreshop-cell-hd-title { font-size: 30rpx; /* color: #000000; */ display: block; width: 180rpx; }
    .cell-hd-desc { font-size: 24rpx; /* display: block; */ width: 200rpx; color: #888888; /* margin-top: 50rpx; */ }
    .subscription-btn { background-color: #333333; color: #ffffff; padding: 0 20rpx; line-height: 46rpx; margin-right: 20rpx; border: 1px solid #333333; }
    .isTrue { background-color: #ffffff; border: 1px solid #333333; color: #333333; line-height: 46rpx; }
</style>