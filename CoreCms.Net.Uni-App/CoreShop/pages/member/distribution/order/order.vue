<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="分销订单"></u-navbar>

        <view class="head_box coreshop-bg-red">
            <!-- 团队数据总览 -->
            <view class="team-data-box u-flex u-row-between">
                <view class="data-card">
                    <view class="total-item">
                        <view class="item-title">团队订单数量（单）</view>
                        <view class="total-num">{{ orderInfo.allOrder || 0 }}</view>
                    </view>
                    <view class="category-item u-flex u-row-between">
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-start flex-sub">
                            <view class="item-title">一级订单</view>
                            <view class="category-num">{{ orderInfo.firstOrder || 0 }}</view>
                        </view>
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-start flex-sub">
                            <view class="item-title">二级订单</view>
                            <view class="category-num">{{ orderInfo.secondOrder || 0 }}</view>
                        </view>
                    </view>
                </view>
                <view class="data-card">
                    <view class="total-item">
                        <view class="item-title">团队订单金额（元）</view>
                        <view class="total-num">{{ orderInfo.allOrderMoney || '0.00' }}</view>
                    </view>
                    <view class="category-item u-flex u-row-between">
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-start flex-sub">
                            <view class="item-title">一级订单</view>
                            <view class="category-num">{{ orderInfo.firstOrderMoney || '0.00' }}</view>
                        </view>
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-start">
                            <view class="item-title">二级订单</view>
                            <view class="category-num">{{ orderInfo.secondOrderMoney || '0.00' }}</view>
                        </view>
                    </view>
                </view>
            </view>

            <!-- 直推 -->
            <view class="direct-box u-flex u-row-between">
                <view class="direct-item ">
                    <view class="item-title">本月分销订单数量（单）</view>
                    <view class="item-value">{{ orderInfo.monthOrder || '0'  }}</view>
                </view>
                <view class="direct-item">
                    <view class="item-title">本月分销订单金额（元）</view>
                    <view class="item-value">{{ orderInfo.monthOrderMoney || '0'  }}</view>
                </view>
            </view>

            <!-- 状态分类 -->
            <view class="u-flex u-row-between nav-box">
                <view class="state-item flex-sub " v-for="(state, index) in statusList" :key="state.value" @tap="onTab(state.value)">
                    <text class="state-title" :class="{ 'title-active': stateCurrent === state.value }">{{ state.name }}</text>
                    <text class="underline" :class="{ 'underline-active': stateCurrent === state.value }"></text>
                </view>
            </view>
        </view>

        <scroll-view :style="'height:'+viewHeight+'px'" scroll-y="true" class="scroll-Y" @scrolltolower="bottomOut()">
            <view class="content_box">
                <!-- 订单列表 -->
                <view class="order-list" v-for="item in list" :key="item.id" v-if="list.length > 0">
                    <view class="order-head u-flex u-row-between">
                        <text class="order-code">订单编号：{{ item.orderId }}</text>
                        <text class="order-state">{{ item.statusName }}</text>
                    </view>
                    <view class="order-from u-flex u-row-between">
                        <view class="from-user coreshop-flex coreshop-align-center">
                            <text>下单人：</text>
                            <image class="user-avatar" :src="item.buyer.avatar" mode=""></image>
                            <text class="user-name">{{ item.buyUserNickName }}</text>
                        </view>
                        <view class="order-time">{{ $u.timeFormat(item.createTime, ' yyyy.mm.dd hh:MM ') }}</view>
                    </view>
                    <view class="total-box u-flex u-row-between px20">
                        <view class="num-price">佣金：￥{{ item.amount || '0'}}</view>
                        <view class="coreshop-flex coreshop-align-center">{{item.isSettlement==1?'已结算':item.isSettlement==2?'未结算':'已退款'}}</view>
                    </view>
                </view>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="当前日明细" mode="list"></u-empty>
                </view>
                <!-- 更多 -->
                <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" v-if="list.length" />
            </view>
        </scroll-view>
    </view>
</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [goods],
        data() {
            return {
                orderInfo: {
                    allOrder: 0,
                    firstOrder: 0,
                    secondOrder: 0,
                    monthOrder: 0,
                    allOrderMoney: 0,
                    firstOrderMoney: 0,
                    secondOrderMoney: 0,
                    monthOrderMoney: 0
                },
                startTime: 0,
                screenName: '',
                page: 1,
                limit: 10,
                list: [], // 商品浏览足迹
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                },
                stateCurrent: '0', //默认
                statusList: [
                    {
                        name: '全部',
                        value: '0'
                    },
                    {
                        name: '已结算',
                        value: '1'
                    },
                    {
                        name: '未结算',
                        value: '2'
                    },
                    {
                        name: '已退款',
                        value: '3'
                    }
                ],
                viewHeight: 0,
            };
        },
        onLoad() {
            this.getDistributionOrderSum();
            this.getDistributionOrder()
        },
        onShow() {
            var _this = this;
            uni.getSystemInfo({
                success: function (res) { // res - 各种参数
                    console.log(res); // 屏幕的宽度
                    var windowHeight = res.windowHeight;

                    let info = uni.createSelectorQuery().select(".head_box");
                    info.boundingClientRect(function (data) { //data - 各种参数
                        var headHeight = data.height;
                        _this.viewHeight = windowHeight - headHeight;

                    }).exec()
                }
            });
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getDistributionOrder()
            }
        },
        methods: {
            //触底加载数据
            bottomOut() {
                if (this.status == 'nomore') {
                    return;
                }
                if (this.status == 'loadmore') {
                    this.getDistributionOrder();//调用数据请求
                }
            },
            // 切换分类
            onTab(state) {
                uni.showLoading({
                    title: '加载中'
                });
                this.list = [];
                this.stateCurrent = state;
                this.page = 1;
                this.$u.debounce(this.getDistributionOrder);
            },
            getDistributionOrderSum() {
                this.$u.api.getDistributionOrderSum(null).then(res => {
                    if (res.status) {
                        this.orderInfo.allOrder = res.data.allOrder;
                        this.orderInfo.firstOrder = res.data.firstOrder;
                        this.orderInfo.secondOrder = res.data.secondOrder;
                        this.orderInfo.monthOrder = res.data.monthOrder;
                        this.orderInfo.allOrderMoney = res.data.allOrderMoney;
                        this.orderInfo.firstOrderMoney = res.data.firstOrderMoney;
                        this.orderInfo.secondOrderMoney = res.data.secondOrderMoney;
                        this.orderInfo.monthOrderMoney = res.data.monthOrderMoney;
                    } else {
                        this.$u.toast(res.msg)
                    }
                });
            },
            getDistributionOrder() {
                let data = {
                    page: this.page,
                    limit: this.limit,
                    id: this.stateCurrent
                }
                this.status = 'loading'
                this.$u.api.getDistributionOrder(data).then(res => {
                    //console.log(res);
                    uni.hideLoading();
                    if (res.status) {
                        let _list = res.data
                        if (_list) {
                            _list.forEach(item => {
                                this.$set(item, 'slide_x', 0)
                            })
                            this.list = [...this.list, ..._list]
                        }
                        if (res.code > this.list.length) {
                            this.page++
                            this.status = 'loadmore'
                        } else {
                            this.status = 'nomore'
                        }
                    } else {
                        this.$u.toast(res.msg)
                    }
                });
                uni.hideLoading();
            },
        }
    };
</script>

<style lang="scss">
    @import "order.scss";
</style>
