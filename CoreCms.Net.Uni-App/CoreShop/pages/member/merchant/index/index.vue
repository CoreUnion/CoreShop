<!-- 商家中心 -->
<template>
    <view class="">
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
                    <text class="nav-title x-f">商家中心</text>
                </view>
                <view class="user-head x-bc">
                    <view class="shop-info">
                        <view class="x-f u-m-b-30" @tap="goStoreList">
                            <text class="shop-title">{{ storeDetail.storeName }}</text>
                            <text class="cuIcon-roundrightfill"></text>
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
            <view class="cu-list grid col-4 no-border">
                <block v v-for="(item,i) in icons" :key="i">
                    <view class="cu-item" @click="goRoute(item.router)">
                        <view class="text-black" :class="item.icon" />
                        <text>{{item.name}}</text>
                    </view>
                </block>
            </view>
        </view>

        <!-- 统计及切换信息 -->
        <view class="statistics-box">
            <view class="statistics-nav x-f">
                <view class="nav-item x-f" v-for="nav in cancelTypeList" :key="nav.id" @tap="onNav(nav.type)">
                    <view class="y-f" style="width: 100%;">
                        <view class="item-title x-f" :class="{ 'title-active': cancelType === nav.type }">
                            <text class="u-font-13">{{ nav.title }}</text>
                            <view :class="{ 'icon-active': cancelType === nav.type }">
                                <text class="cuIcon-triangledownfill" :class="{ 'icon-active': cancelType === nav.type }"></text>
                            </view>
                        </view>
                        <text class="nav-line" :class="{ 'line-active': cancelType === nav.type }"></text>
                    </view>
                </view>
                <!-- 下拉窗 -->
                <view class="drop-down-box">
                    <view class="drop-down-item x-bc" v-for="(item, index) in dropDown[cancelType]" :key="index" @tap="onFilter(item.value, item.title)">
                        <text class="item-title">{{ item.title }}</text>
                        <text class="cuIcon-check" v-if="filter[cancelType] == item.value"></text>
                    </view>
                </view>
            </view>
            <!-- 销量 -->
            <view class="sales-volume-box x-bc pa30">
                <view class="sales-volume x-c">订单量(单)：{{ orderTotalCount || 0 }}</view>
                <view class="sales-volume x-c">交易额(元)：{{ orderTotalMoney || 0 }}</view>
            </view>
        </view>
        <!-- 订单列表 -->
        <view class="order-list" v-for="order in storeOrderList" :key="order.orderId" @tap.stop="goOrderDetail(order.orderId)">
            <view class="order-head x-bc">
                <text class="no">订单编号：{{ order.orderId }}</text>
                <text class="state">{{ order.shipStatusText }}</text>
            </view>
            <view class="goods-order" v-for="item in order.items" :key="item.id">
                <view class="order-content">
                    <view class="goods-box x-start">
                        <view class="order-goods__tag">
                            <image v-if="detail.activity_type" class="tag-img" :src="orderStatus[detail.activity_type]" mode=""></image>
                            <image v-if="orderType === 'score'" class="tag-img" :src="orderStatus[orderType]" mode=""></image>
                        </view>
                        <image class="goods-img" :src="item.imageUrl || ''" mode="aspectFill"></image>
                        <view class="y-start order-right">
                            <view class="goods-title more-t u-line-2">{{ item.name || '' }}</view>
                            <view class="order-tip one-t">
                                <text class="order-num">数量:{{ item.nums || 0 }};</text>
                                {{ item.addon ? item.addon : '' }}
                            </view>
                            <view class="order-goods x-f ">
                                <text class="order-price">￥{{ item.amount || 0 }}</text>
                                <!--<button class="cu-btn status-btn" v-if="detail.status_name">{{ item.status_name }}</button>-->
                            </view>
                        </view>
                    </view>
                </view>
            </view>

            <view class="order-bottom x-bc u-border-bottom">
                <view>
                    <text class="u-font-24 tips-color">{{$u.timeFormat(order.createTime, 'yyyy-mm-dd hh:MM:ss')}}</text>
                </view>
                <view>
                    <text class="u-font-24 tips-color">{{ order.shipName }}【{{ order.shipMobile }}】</text>
                </view>
            </view>

            <view class="order-bottom x-bc">
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
            <u-empty :src="$apiFilesUrl+'/static/images/empty/order.png'" icon-size="300" text="当前时间暂无订单数据" mode="list"></u-empty>
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
                        { title: '待收货', value: '3', isChecked: true },
                        { title: '待评价', value: '4', isChecked: true },
                        { title: '已完成', value: '6', isChecked: true },
                        { title: '已取消', value: '7', isChecked: true }
                    ]
                },
                loadStatus: 'loadmore', //loadmore-加载前的状态，loading-加载中的状态，nomore-没有更多的状态
                currentPage: 1,
                lastPage: 1,
                icons: [{
                    name: '提货单列表',
                    icon: 'cuIcon-pick text-red',
                    router: '/pages/member/merchant/takeDelivery/list'

                },
                {
                    name: '提货单核销',
                    icon: 'cuIcon-qr_code text-orange',
                    router: '/pages/member/merchant/takeDelivery/index'
                },
                {
                    name: '服务券列表',
                    icon: 'cuIcon-list text-yellow',
                    router: '/pages/member/merchant/serviceVerification/list'

                },
                {
                    name: '核验服务券',
                    icon: 'cuIcon-qr_code text-green',
                    router: '/pages/member/merchant/serviceVerification/index'
                }],
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
            // 选择门店
            goStoreList() {
                this.$u.route('/pages/member/merchant/storeList/storeList');
            },
            // 选择门店
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
    @import '../../../../static/style/merchant.scss';
</style>
