<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="订单查询"></u-navbar>

        <view class="u-padding-top-10 u-padding-bottom-10 u-padding-left-15 u-padding-right-15 u-margin-top-15 u-margin-bottom-15 coreshop-bg-white" v-bind:class="coreshopdata.parameters.style">
            <u-search placeholder="请输订单号、收货人手机号、收货人姓名" v-model="keyword" shape="square" :show-action="true" action-text="搜索" @custom="goSearch" @search="goSearch"></u-search>
        </view>

        <!-- 订单列表 -->
        <view class="order-list" v-for="order in storeOrderList" :key="order.orderId" @tap.stop="goOrderDetail(order.orderId)">
            <view class="order-head u-flex u-row-between">
                <text class="no">
                    编号：{{ order.orderId }}
                </text>
                <text class="state">{{ order.shipStatusText }}</text>
                <u-button type="primary" size="mini" v-if="order.receiptType==1">物流快递</u-button>
                <u-button type="success" size="mini" v-if="order.receiptType==2">同城配送</u-button>
                <u-button type="warning" size="mini" v-if="order.receiptType==3">门店自提</u-button>
            </view>
            <view class="goods-order" v-for="item in order.items" :key="item.id">
                <view class="order-content">
                    <view class="goods-box coreshop-flex coreshop-align-start">
                        <view class="order-goods__tag">
                            <image v-if="detail.activity_type" class="tag-img" :src="orderStatus[detail.activity_type]" mode=""></image>
                            <image v-if="orderType === 'score'" class="tag-img" :src="orderStatus[orderType]" mode=""></image>
                        </view>
                        <image class="goods-img" :src="item.imageUrl || ''" mode="aspectFill"></image>
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-start">
                            <view class="goods-title more-t u-line-2">{{ item.name || '' }}</view>
                            <view class="order-tip one-t">
                                <text class="order-num">数量:{{ item.nums || 0 }};</text>
                                {{ item.addon ? item.addon : '' }}
                            </view>
                            <view class="order-goods coreshop-flex coreshop-align-center ">
                                <text class="order-price">￥{{ item.amount || 0 }}</text>
                                <!--<button class="cu-btn status-btn" v-if="detail.status_name">{{ item.status_name }}</button>-->
                            </view>
                        </view>
                    </view>
                </view>
            </view>

            <view class="order-bottom u-flex u-row-between u-border-bottom">
                <view>
                    <text class="u-font-24 tips-color">{{$u.timeFormat(order.createTime, 'yyyy-mm-dd hh:MM:ss')}}</text>
                </view>
                <view>
                    te
                    <text class="u-font-24 tips-color">{{ order.shipName }}【{{ order.shipMobile }}】</text>
                </view>
            </view>

            <view class="order-bottom u-flex u-row-between">
                <view v-if="order.status === 1">
                    <u-button type="success" size="mini">订单正常</u-button>
                </view>
                <view v-else-if="order.status === 2">
                    <u-button type="primary" size="mini">订单完成</u-button>
                </view>
                <view v-else-if="order.status === 3">
                    <u-button size="mini">订单取消</u-button>
                </view>
                <view v-else></view>
                <view>
                    <text class="u-font-24">{{ order.payStatusText }}</text>
                    <text class="u-font-24" v-if="order.paymentCodeText">【{{order.paymentCodeText}}】</text>
                </view>
                <view>
                    <text class="total-price-title">实付款：</text>
                    <text class="total-price">{{ order.payedAmount }}</text>
                </view>
            </view>

        </view>

        <!-- 无数据时默认显示 -->
        <view class="coreshop-emptybox" v-if="storeOrderList.length<=0">
            <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/order.png'" icon-size="300" text="当前时间暂无订单数据" mode="list"></u-empty>
        </view>

        <!-- 更多 -->
        <u-loadmore v-if="storeOrderList.length" height="80rpx" :status="loadStatus" icon-type="flower" color="#ccc" />


    </view>

</template>

<script>
    import { mapMutations, mapActions, mapState } from 'vuex';
    import { goods, articles, commonUse, tools } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [goods, articles, commonUse, tools],
        components: {},
        data() {
            return {
                keyword: '',
                storeOrderList: [], //订单商品列表
                loadStatus: 'loadmore', //loadmore-加载前的状态，loading-加载中的状态，nomore-没有更多的状态
                currentPage: 1,
                lastPage: 1,
            }
        },
        onLoad(options) {
            this.keyword = options.keyword
            if (this.keyword) {
                this.getStoreOrder();
            }
        },
        onPullDownRefresh() {
            this.storeOrderList = [];
            this.currentPage = 1;
            if (this.keyword) {
                this.getStoreOrder();
            }
        },
        onReachBottom() {
            if (this.currentPage < this.lastPage) {
                this.currentPage += 1;
                this.getStoreOrder();
            }
        },
        methods: {
            goSearch() {
                if (this.keyword) {
                    this.storeOrderList = [];
                    this.currentPage = 1;
                    this.getStoreOrder();
                } else {
                    this.$refs.uToast.show({
                        title: '请输订单号、收货人手机号、收货人姓名',
                        type: 'warning',
                    })
                }
            },
            // 订单详情
            goOrderDetail(id) {
                this.$u.route({
                    url: '/pages/member/merchant/detail/detail',
                    params: {
                        orderId: id
                    }
                });
            },
            // 门店订单列表
            getStoreOrder() {
                let that = this;
                that.loadStatus = 'loading';
                let data = {
                    keyword: this.keyword,
                    page: that.currentPage,
                    limit: 10,
                    storeId: that.storeId
                }
                this.$u.api.getOrderPageByMerchantSearch(data).then(res => {
                    if (res.status) {
                        uni.stopPullDownRefresh();
                        that.storeOrderList = [...that.storeOrderList, ...res.data.pages];
                        that.lastPage = res.data.totalPages;
                        if (that.currentPage < res.data.totalPages) {
                            that.loadStatus = 'loadmore';
                        } else {
                            that.loadStatus = 'nomore';
                        }
                    } else {
                        this.uToast.show({
                            title: res.msg,
                            type: 'warning',
                        })
                    }
                });
            },
        }
    }
</script>

<style lang="scss" scoped>
    @import 'index.scss';
</style>
