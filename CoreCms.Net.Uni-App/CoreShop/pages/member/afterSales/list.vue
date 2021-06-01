<template>
    <view class="wrap">
        <u-navbar title="售后列表"></u-navbar>

        <view v-if="order.length>0">
            <view class="order" v-for="(item, key) in order" :key="key">
                <view class="top">
                    <view class="left" @click="doCopyData(item.aftersalesId)">
                        <u-icon name="home" :size="30" color="rgb(94,94,94)"></u-icon>
                        <view class="store">售后单号：{{item.aftersalesId}}</view>
                        <u-icon name="arrow-right" color="rgb(203,203,203)" :size="26"></u-icon>
                    </view>
                    <view class="right" v-if="item.status == 1">待审核</view>
                    <view class="right" v-else-if="item.status == 2">审核通过</view>
                    <view class="right" v-else-if="item.status == 3">审核拒绝</view>
                </view>
                <view class="item" v-for="(v, k) in item.items" :key="k" @click="showOrder(item.aftersalesId)">
                    <view class="left"><image :src="v.imageUrl && v.imageUrl!='null' ?  v.imageUrl+'?x-oss-process=image/resize,m_lfit,h_320,w_240' : '/static/images/common/empty-banner.png'" mode="aspectFill"></image></view>
                    <view class="content">
                        <view class="title u-line-2">{{v.name}}</view>
                        <view class="type">{{v.addon}}</view>
                    </view>
                    <view class="right">
                        <view class="number">x{{ v.nums }}</view>
                    </view>
                </view>
                <!-- 订单底部 -->
                <view class="bottom">
                    <view class="more">退款金额：￥{{item.refundAmount}}</view>
                    <view class='logistics coreshop-btn' hover-class="btn-hover" @click="showOrder(item.aftersalesId)">查看详情</view>
                </view>
            </view>
            <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
        </view>

        <!-- 无数据时默认显示 -->
        <view class="coreshop-emptybox" v-else>
            <u-empty :src="$apiFilesUrl+'/static/images/empty/order.png'" icon-size="300" text="暂无提现明细" mode="list"></u-empty>
        </view>

    </view>
</template>

<script>
    import { tools } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [tools],
        data() {
            return {
                order: [], //订单列表
                page: 1, //当前页
                limit: 10, //每页显示几条
                orderId: "",
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                }
            }
        },
        onShow() {
            this.getOrderList();
        },
        onLoad(e) {
            this.order = [];
            this.orderId = e.orderId;
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getOrderList()
            }
        },
        methods: {
            //获取订单数据
            getOrderList() {
                let data = {};
                this.status = 'loading'
                data['page'] = this.page;
                data['limit'] = this.limit;
                if (this.orderId) {
                    data['id'] = this.orderId;
                }
                this.$u.api.afterSalesList(data).then(res => {
                    let orderList = this.dataFormat(res.data.list);
                    this.order = this.order.concat(orderList);
                    this.page = res.data.page * 1 + 1;
                    let allpage = res.data.totalPage;
                    if (allpage < this.page) {
                        this.status = 'nomore'
                    } else {
                        this.status = 'loadmore'
                    }
                });
            },

            //数据格式处理
            dataFormat(data) {
                for (var i = 0; i < data.length; i++) {
                    let countnum = 0
                    if (data[i].order && data[i].order.items) {
                        for (var j = 0; j < data[i].order.items.length; j++) {
                            countnum += data[i].order.items[j].nums;
                        }
                        data[i].countnum = countnum;
                    }
                }
                return data;
            },

            //查看详情
            showOrder(aftersalesId) {
                this.$u.route('/pages/member/afterSales/detail?aftersalesId=' + aftersalesId);
            }
        },
    }
</script>

<style lang="scss" scoped>
    .order { width: 710rpx; background-color: #ffffff; margin: 20rpx auto; border-radius: 20rpx; box-sizing: border-box; padding: 20rpx; font-size: 28rpx; }
        .order .top { display: flex; justify-content: space-between; }
            .order .top .left { display: flex; align-items: center; }
                .order .top .left .store { margin: 0 10rpx; font-size: 32rpx; font-weight: bold; }
            .order .top .right { color: $u-type-warning-dark; }
        .order .item { display: flex; margin: 20rpx 0 0; }
            .order .item .left { margin-right: 20rpx; }
                .order .item .left image { width: 200rpx; height: 200rpx; border-radius: 10rpx; }
            .order .item .content { }
                .order .item .content .title { font-size: 28rpx; line-height: 50rpx; }
                .order .item .content .type { margin: 10rpx 0; font-size: 24rpx; color: $u-tips-color; }
                .order .item .content .delivery-time { color: #e5d001; font-size: 24rpx; }
            .order .item .right { margin-left: 10rpx; padding-top: 20rpx; text-align: right; }
                .order .item .right .decimal { font-size: 24rpx; margin-top: 4rpx; }
                .order .item .right .number { color: $u-tips-color; font-size: 24rpx; }
        .order .total { margin-top: 20rpx; text-align: right; font-size: 24rpx; }
            .order .total .total-price { font-size: 32rpx; }
        .order .bottom { display: flex; margin-top: 40rpx; padding: 0 10rpx; justify-content: space-between; align-items: center; }
            .order .bottom .coreshop-btn { line-height: 52rpx; width: 160rpx; border-radius: 26rpx; border: 2rpx solid $u-border-color; font-size: 26rpx; text-align: center; color: $u-type-info-dark; }
            .order .bottom .evaluate { color: $u-type-warning-dark; border-color: $u-type-warning-dark; }

    .centre { text-align: center; margin: 200rpx auto; font-size: 32rpx; }
        .centre image { width: 164rpx; height: 164rpx; border-radius: 50%; margin-bottom: 20rpx; }
        .centre .tips { font-size: 24rpx; color: #999999; margin-top: 20rpx; }
        .centre .coreshop-btn { margin: 80rpx auto; width: 200rpx; border-radius: 32rpx; line-height: 64rpx; color: #ffffff; font-size: 26rpx; background: linear-gradient(270deg, rgba(249, 116, 90, 1) 0%, rgba(255, 158, 1, 1) 100%); }
    .wrap { display: flex; flex-direction: column; height: calc(100vh - var(--window-top)); width: 100%; }
</style>
