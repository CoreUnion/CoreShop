<!-- 商家中心 -->
<template>
    <view class="">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <view class="mask" v-if="isShowDropDown" cathctouchmove @tap.stop="onHideDropDown"></view>
        <!-- 商家信息 -->
        <view class="head-box">
            <view class="head-wrap">
                <view class="titleNav u-padding-left-30 u-padding-right-30">
                    <!-- #ifndef H5 -->
                    <view class="status-bar"></view>
                    <!-- #endif -->
                    <!-- #ifdef H5 -->
                    <view class="status-H5bar"></view>
                    <!-- #endif -->
                    <text class="nav-title coreshop-flex coreshop-align-center">商家中心</text>
                </view>
                <view class="user-head u-flex u-row-between">
                    <view class="shop-info">
                        <view class="coreshop-flex coreshop-align-center u-m-b-30" @tap="goStoreList">
                            <text class="shop-title">{{ storeDetail.storeName }}</text>
                            <!--<text class="cuIcon-roundrightfill"></text>-->
                            <u-icon name="arrow-down" color="#FFF" size="28" class="u-margin-left-20"></u-icon>
                        </view>
                        <view class="shop-address" @tap="goMapDetails(storeDetail.id, storeDetail.latitude, storeDetail.longitude)">
                            {{ storeDetail.address || '' }}
                        </view>
                    </view>
                    <button @tap="goUserCenter()" class="cu-btn merchant-btn">切换个人版</button>
                </view>
            </view>
        </view>

        <!--业务列表-->
        <view class="coreshop-tools-list-box">

            <u-grid :col="4">
                <u-grid-item @click="goRoute('/pages/member/merchant/takeDelivery/list')">
                    <u-icon name="order" :size="46"></u-icon>
                    <view class="grid-text">提货单列表</view>
                </u-grid-item>
                <u-grid-item @click="goRoute('/pages/member/merchant/takeDelivery/index')">
                    <u-icon name="fingerprint" :size="46"></u-icon>
                    <view class="grid-text">提货单核销</view>
                </u-grid-item>
                <u-grid-item @click="goRoute('/pages/member/merchant/serviceVerification/list')">
                    <u-icon name="coupon" :size="46"></u-icon>
                    <view class="grid-text">服务券列表</view>
                </u-grid-item>
                <u-grid-item @click="goRoute('/pages/member/merchant/serviceVerification/index')">
                    <u-icon name="grid" :size="46"></u-icon>
                    <view class="grid-text">核验服务券</view>
                </u-grid-item>
            </u-grid>
        </view>

        <view class="u-padding-top-10 u-padding-bottom-10 u-padding-left-15 u-padding-right-15 u-margin-top-15 u-margin-bottom-15 coreshop-bg-white" v-bind:class="coreshopdata.parameters.style">
            <u-search placeholder="请输订单号、收货人手机号、收货人姓名" v-model="keyword" shape="square" :show-action="true" action-text="搜索" @custom="goSearch" @search="goSearch"></u-search>
        </view>

        <!-- 统计及切换信息 -->
        <view class="statistics-box">
            <view class="statistics-nav coreshop-flex coreshop-align-center">
                <view class="nav-item coreshop-flex coreshop-align-center" v-for="nav in cancelTypeList" :key="nav.id" @tap="onNav(nav.type)">
                    <view class="coreshop-flex coreshop-flex-direction coreshop-align-center w100">
                        <view class="item-title coreshop-flex coreshop-align-center" :class="{ 'title-active': cancelType === nav.type }">
                            <text class="u-font-13 u-margin-right-10">{{ nav.title }}</text>
                            <view :class="{ 'icon-active': cancelType === nav.type }">
                                <u-icon name="arrow-down-fill" size="22"></u-icon>
                            </view>
                        </view>
                        <text class="nav-line" :class="{ 'line-active': cancelType === nav.type }"></text>
                    </view>
                </view>
                <!-- 下拉窗 -->
                <view class="drop-down-box">
                    <view class="drop-down-item u-flex u-row-between" v-for="(item, index) in dropDown[cancelType]" :key="index" @tap="onFilter(item.value, item.title)">
                        <text class="item-title">{{ item.title }}</text>
                        <text class="cuIcon-check" v-if="filter[cancelType] == item.value"></text>
                    </view>
                </view>
            </view>
            <!-- 销量 -->
            <view class="sales-volume-box u-flex u-row-between pa30">
                <view class="sales-volume x-c">订单量(单)：{{ orderTotalCount || 0 }}</view>
                <view class="sales-volume x-c">交易额(元)：{{ orderTotalMoney || 0 }}</view>
            </view>
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

        <!-- 日期选择 -->
        <u-calendar v-model="showCalendar"
                    :mode="mode"
                    :start-text="startText"
                    :end-text="endText"
                    :range-color="rangeColor"
                    :range-bg-color="rangeBgColor"
                    :active-bg-color="activeBgColor"
                    btnType="success"
                    @change="selDate"></u-calendar>
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
                keywords: '',
                storeId: 0,
                storeOrderList: [], //订单商品列表
                orderTotalCount: 0, //订单统计信息
                orderTotalMoney: 0, //订单金额统计信息
                storeDetail: {}, //门店信息
                cancelType: '', //分类
                cancelTypeList: [
                    {
                        id: 0,
                        title: '全部',
                        type: 'date'
                    },
                    {
                        id: 1,
                        title: '全部',
                        type: 'status'
                    },
                    {
                        id: 2,
                        title: '全部',
                        type: 'receiptType'
                    }
                ],
                showCalendar: false, //日期选择
                mode: 'range',
                result: '请选择日期',
                startText: '开始',
                endText: '结束',
                rangeColor: '#4CB89D',
                rangeBgColor: 'rgba(76,184,157,0.13)',
                activeBgColor: '#4CB89D',
                filter: {
                    date: 'all',
                    status: '0',
                    receiptType: '0',
                    custom: []
                },
                dropDown: {
                    date: [
                        { title: '全部', value: 'all', isChecked: false },
                        { title: '今日', value: 'today', isChecked: false },
                        { title: '昨日', value: 'yesterday', isChecked: false },
                        { title: '本周', value: 'week', isChecked: false },
                        { title: '本月', value: 'month', isChecked: false },
                        { title: '自定义', value: 'custom', isChecked: false }
                    ],
                    status: [
                        { title: '全部', value: '0', isChecked: false },
                        { title: '待付款', value: '1', isChecked: false },
                        { title: '待发货', value: '2', isChecked: false },
                        { title: '待收货', value: '3', isChecked: false },
                        { title: '待评价', value: '4', isChecked: false },
                        { title: '已完成', value: '6', isChecked: false },
                        { title: '已取消', value: '7', isChecked: false }
                    ],
                    receiptType: [
                        { title: '全部', value: '0', isChecked: false },
                        { title: '物流快递', value: '1', isChecked: false },
                        { title: '同城配送', value: '2', isChecked: false },
                        { title: '门店自提', value: '3', isChecked: true },
                    ]
                },
                loadStatus: 'loadmore', //loadmore-加载前的状态，loading-加载中的状态，nomore-没有更多的状态
                currentPage: 1,
                lastPage: 1,
            };
        },
        computed: {},
        onLoad(options) {
            if (options.storeId) {
                //uni.setStorageSync('storeId', options.storeId);
                this.storeId = options.storeId;
                console.log("获取到storeId：" + this.storeId);
            }
            this.getStoreDetail();
            this.getStoreOrder();
        },
        onPullDownRefresh() {
            this.storeOrderList = [];
            this.currentPage = 1;
            this.getStoreOrder();
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
                    this.$u.route('/pages/member/merchant/search/index?keyword=' + this.keyword);
                } else {
                    this.$refs.uToast.show({
                        title: '请输订单号、收货人手机号、收货人姓名',
                        type: 'warning',
                    })
                }
            },
            // 选择门店
            goStoreList() {
                this.$u.route('/pages/member/merchant/storeList/storeList');
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
            // 获取门店信息
            getStoreDetail() {
                let that = this;
                let data = {
                    id: this.storeId
                }
                if (this.storeId > 0) {
                    this.$u.api.getStoreById(data).then(e => {
                        if (e.status) {
                            that.storeDetail = e.data;
                        } else {
                            that.$refs.uToast.show({
                                title: e.msg,
                                type: 'error',
                            })
                        }
                    });
                } else {
                    this.$u.api.getStoreByUserId(data).then(e => {
                        if (e.status) {
                            that.storeDetail = e.data;
                        } else {
                            that.$refs.uToast.show({
                                title: e.msg,
                                type: 'error',
                            })
                        }
                    });
                }
            },
            // 门店订单列表
            getStoreOrder() {
                let that = this;
                that.loadStatus = 'loading';

                let data = {
                    dateType: that.filter.date,
                    date: that.filter.custom,
                    status: that.filter.status,
                    receiptType: that.filter.receiptType,
                    page: that.currentPage,
                    limit: 10,
                    storeId: that.storeId
                }

                this.$u.api.getOrderPageByMerchant(data).then(res => {
                    if (res.status) {
                        uni.stopPullDownRefresh();
                        that.storeOrderList = [...that.storeOrderList, ...res.data.pages];
                        that.orderTotalCount = res.data.totalCount;
                        that.orderTotalMoney = res.data.totalMoney;
                        that.lastPage = res.data.totalPages;
                        if (that.currentPage < res.data.totalPages) {
                            that.loadStatus = 'loadmore';
                        } else {
                            that.loadStatus = 'nomore';
                        }
                    }
                });
            },

            // 切换核销分类
            onNav(type) {
                this.isShowDropDown = this.cancelType == type ? false : true;
                this.cancelType = this.cancelType == type ? '' : type;
                console.log('cancelType：' + this.cancelType);
            },
            // 选择日期
            selDate(e) {
                this.filter.custom.push(e.startDate);
                this.filter.custom.push(e.endDate);
                this.cancelTypeList[0].title = `${e.startDate.replace(/-/g, ':')}-${e.endDate.replace(/-/g, ':')}`;
                this.storeOrderList = [];
                this.currentPage = 1;
                this.getStoreOrder();
            },
            // 下拉筛选
            onHideDropDown() {
                this.isShowDropDown = false;
                this.cancelType = '';
            },
            // 选择筛选
            onFilter(val, title) {
                if (val == 'custom') {
                    this.showCalendar = true;
                }
                if (this.cancelType == 'date') {
                    this.cancelTypeList[0].title = title;
                }
                if (this.cancelType == 'status') {
                    this.cancelTypeList[1].title = title;
                }
                if (this.cancelType == 'receiptType') {
                    this.cancelTypeList[2].title = title;
                }
                this.filter[this.cancelType] = val;
                this.storeOrderList = [];
                this.currentPage = 1;
                this.getStoreOrder();
                this.cancelType = '';
            },
        }
    };
</script>

<style lang="scss" scoped>
    @import 'index.scss';
</style>
