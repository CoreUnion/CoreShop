<template>
    <view class="pageBox commission-order-wrap">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <view class="head_box bg-blue">
            <!-- 标题 -->
            <cu-custom isBack>
                <block slot="backText">代理订单</block>
            </cu-custom>

            <!-- 团队数据总览 -->
            <view class="team-data-box x-bc">
                <view class="data-card">
                    <view class="total-item">
                        <view class="item-title">总订单数量（单）</view>
                        <view class="total-num">{{ orderInfo.allOrder }}</view>
                    </view>
                    <view class="category-item x-f">
                        <view class="y-start flex-sub">
                            <view class="item-title">代购订单</view>
                            <view class="category-num">{{ orderInfo.procurementServiceOrder }}</view>
                        </view>
                        <view class="y-start flex-sub">
                            <view class="item-title">客户订单</view>
                            <view class="category-num">{{ orderInfo.customerOrder }}</view>
                        </view>
                    </view>
                </view>
                <view class="data-card">
                    <view class="total-item">
                        <view class="item-title">总订单金额（元）</view>
                        <view class="total-num">{{ orderInfo.allOrderMoney || '0.00' }}</view>
                    </view>
                    <view class="category-item x-f">
                        <view class="y-start flex-sub">
                            <view class="item-title">代购订单</view>
                            <view class="category-num">{{ orderInfo.procurementServiceOrderMoney || '0.00' }}</view>
                        </view>
                        <view class="y-start ">
                            <view class="item-title">客户订单</view>
                            <view class="category-num">{{ orderInfo.customerOrderMoney || '0.00' }}</view>
                        </view>
                    </view>
                </view>
            </view>

            <!-- 直推 -->
            <view class="direct-box x-bc">
                <view class="direct-item ">
                    <view class="item-title">本月订单总数（单）</view>
                    <view class="item-value">{{ orderInfo.monthOrder || '0'  }}</view>
                </view>
                <view class="direct-item">
                    <view class="item-title">本月订单金额（元）</view>
                    <view class="item-value">{{ orderInfo.monthOrderMoney || '0'  }}</view>
                </view>
            </view>

            <!-- 状态分类 -->
            <view class="x-f nav-box">
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
                    <view class="order-head x-bc">
                        <text class="order-code">订单编号：{{ item.orderId }}</text>
                        <text class="order-state">{{ item.userId==item.buyUserId?'代购订单':'推广订单' }}</text>
                    </view>
                    <view class="order-from x-bc">
                        <view class="from-user x-f">
                            <text>下单人：</text>
                            <image class="user-avatar" :src="item.buyer.avatar" mode=""></image>
                            <text class="user-name">{{ item.buyUserNickName }}</text>
                        </view>
                        <view class="order-time">{{ $u.timeFormat(item.createTime, ' yyyy.mm.dd hh:MM ') }}</view>
                    </view>
                    <view class="total-box x-bc px20">
                        <view class="num-price">提成:￥{{ item.amount || '0'}}</view>
                        <view class="x-f">{{item.isSettlement==1?'已结算':item.isSettlement==2?'未结算':'已退款'}}</view>
                    </view>
                </view>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="当前日明细" mode="list"></u-empty>
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
                    procurementServiceOrder: 0,
                    customerOrder: 0,
                    monthOrder: 0,
                    allOrderMoney: 0,
                    procurementServiceOrderMoney: 0,
                    customerOrderMoney: 0,
                    monthOrderMoney: 0,
                },
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
            this.getAgentOrderSum();
            this.getAgentOrder();
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
                this.$u.debounce(this.getAgentOrder);
            },
            getAgentOrderSum() {
                this.$u.api.getAgentOrderSum(null).then(res => {
                    if (res.status) {
                        this.orderInfo.allOrder = res.data.allOrder;
                        this.orderInfo.procurementServiceOrder = res.data.procurementServiceOrder;
                        this.orderInfo.customerOrder = res.data.customerOrder;
                        this.orderInfo.monthOrder = res.data.monthOrder;
                        this.orderInfo.allOrderMoney = res.data.allOrderMoney;
                        this.orderInfo.procurementServiceOrderMoney = res.data.procurementServiceOrderMoney;
                        this.orderInfo.customerOrderMoney = res.data.customerOrderMoney;
                        this.orderInfo.monthOrderMoney = res.data.monthOrderMoney;
                    } else {
                        this.$u.toast(res.msg)
                    }
                });
            },
            getAgentOrder() {
                let data = {
                    page: this.page,
                    limit: this.limit,
                    id: this.stateCurrent
                }
                this.status = 'loading'

                this.$u.api.getAgentOrder(data).then(res => {
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
                })
                uni.hideLoading();
            },
        }
    };
</script>

<style lang="scss">
    .direct-box { margin: 20rpx; }
        .direct-box .direct-item { width: 341rpx; height: 117rpx; background: #ffffff; border-radius: 20rpx; padding: 20rpx; }
            .direct-box .direct-item .item-title { font-size: 22rpx; font-weight: 500; color: #999999; margin-bottom: 6rpx; }
            .direct-box .direct-item .item-value { font-size: 38rpx; font-weight: 600; color: #333333; }

    .team-data-box { margin: 20rpx; }
        .team-data-box .data-card { width: 340rpx; background: #ffffff; border-radius: 20rpx; padding: 20rpx; }
            .team-data-box .data-card .item-title { font-size: 22rpx; font-weight: 500; color: #999999; line-height: 30rpx; margin-bottom: 10rpx; }
            .team-data-box .data-card .total-item { margin-bottom: 20rpx; }
            .team-data-box .data-card .total-num { font-size: 38rpx; font-weight: 600; color: #333333; }
            .team-data-box .data-card .category-num { font-size: 26rpx; font-weight: 600; color: #333333; }

    .head_box { width: 750rpx; background-image: url('/static/images/common/bg.png'); background-size: cover; background-position: center; }
        .head_box .cu-back { color: #fff; font-size: 40rpx; }
        .head_box .head-title { font-size: 38rpx; color: #fff; }

    .nav-box { background-color: #fff; }

    .state-item { height: 100%; display: flex; flex-direction: column; align-items: center; justify-content: center; }
        .state-item .state-title { color: #666; font-weight: 500; font-size: 28rpx; line-height: 90rpx; }
        .state-item .title-active { color: #333; }
        .state-item .underline { display: block; width: 68rpx; height: 4rpx; background: #fff; border-radius: 2rpx; }
        .state-item .underline-active { background: #e54d42; display: block; width: 68rpx; height: 4rpx; border-radius: 2rpx; }

    .order-list { background-color: #fff; margin-top: 20rpx; }
        .order-list .order-head { padding: 20rpx; }
            .order-list .order-head .order-code { font-size: 26rpx; font-weight: 400; color: #999999; }
            .order-list .order-head .order-state { font-size: 26rpx; font-weight: 500; color: #05c3a1; }
        .order-list .order-from { background-color: #f9f9f9; padding: 20rpx; }
            .order-list .order-from .from-user { font-size: 24rpx; font-weight: 400; color: #666666; }
                .order-list .order-from .from-user .user-avatar { width: 26rpx; height: 26rpx; border-radius: 50%; margin-right: 8rpx; }
                .order-list .order-from .from-user .user-name { font-size: 24rpx; font-weight: 400; color: #999999; }
            .order-list .order-from .order-time { font-size: 24rpx; font-weight: 400; color: #999999; }
        .order-list .goods-card { padding: 30rpx 20rpx; }
            .order-list .goods-card .goods-img-box { margin-right: 30rpx; }
                .order-list .goods-card .goods-img-box .goods-img { width: 160rpx; height: 160rpx; background-color: #ccc; }
            .order-list .goods-card .goods-info { height: 160rpx; width: 600rpx; align-items: flex-start; }
                .order-list .goods-card .goods-info .goods-title { font-size: 28rpx; font-weight: 500; color: #333333; }
                .order-list .goods-card .goods-info .goods-sku { font-size: 24rpx; font-weight: 400; color: #666666; }
                .order-list .goods-card .goods-info .goods-price { font-size: 30rpx; font-weight: 500; color: #333333; }
                    .order-list .goods-card .goods-info .goods-price .goods-state { line-height: 30rpx; padding: 0 10rpx; background: #f1eeff; border: 1rpx solid #e54d42; border-radius: 30rpx; margin-left: 20rpx; font-size: 20rpx; color: #e54d42; }
                    .order-list .goods-card .goods-info .goods-price::before { content: '￥'; font-size: 20rpx; }
        .order-list .total-box { height: 80rpx; padding: 0 20rpx; }
            .order-list .total-box .num-price { font-size: 24rpx; font-weight: 400; color: #999999; }
            .order-list .total-box .name { font-size: 24rpx; font-weight: 400; color: #999999; }
            .order-list .total-box .commission-num { font-size: 30rpx; font-weight: 400; color: #eb2b3d; }
                .order-list .total-box .commission-num::before { content: '￥'; font-size: 22rpx; }
</style>
