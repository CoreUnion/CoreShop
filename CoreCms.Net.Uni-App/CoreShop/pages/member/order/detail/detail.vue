<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="订单详情"></u-navbar>

        <!--步骤条区域-->
        <view class="coreshop-bg-white u-padding-20" v-if="basics < 9">
            <!--步骤条-->
            <u-steps :list="numList" active-color="#fa3534" :current="basics" mode="number" v-if="basics < 5"></u-steps>

            <!--提示-->
            <view class="u-font-sm u-text-center u-margin-top-30" v-if="basics == 0">
                <view class="coreshop-text-black">拍下成功，待买家支付。</view>
                <view class="coreshop-text-black">
                    <text class="coreshop-text-red">拍下{{orderCancelTime}}分后</text>
                    <text>未支付，自动取消订单。</text>
                </view>
            </view>
            <view class="u-font-sm u-text-center u-margin-top-30" v-if="basics == 1">
                <view class="coreshop-text-black">支付成功，待卖家发货.</view>
            </view>
            <view class="u-font-sm u-text-center u-margin-top-30" v-if="basics == 2">
                <view class="coreshop-text-black">已发货，快递正在路上，务必在收到商品后再确认收货。</view>
                <view class="coreshop-text-black">
                    <text class="coreshop-text-red">发货20天</text>
                    <text>后将自动确认收货</text>
                </view>
            </view>
            <view class="u-font-sm u-text-center u-margin-top-30" v-if="basics == 3">
                <view class="coreshop-text-black">已收货，请您对此次购物体检进行评价。</view>
                <view class="coreshop-text-black">
                    <text class="coreshop-text-red">收货30天</text>
                    <text>后将自动评价</text>
                </view>
            </view>
            <!--状态图标-->
            <view class="coreshop-bg-white u-padding-20 u-text-center coreshop-status-img-view u-margin-top-20" v-if="basics == 4">
                <view class="are-img-view">
                    <image class="are-img" src="/static/images/common/are.png" mode="widthFix" />
                </view>
                <view class="u-font-sm coreshop-text-black">交易成功，感谢您的评价</view>
            </view>
            <!--状态图标-->
            <view class="coreshop-bg-white u-padding-20 u-text-center coreshop-status-img-view u-margin-top-20" v-if="basics == 6">
                <view class="are-img-view">
                    <image class="are-img" src="/static/images/common/are.png" mode="widthFix" />
                </view>
                <view class="u-font-sm coreshop-text-black">交易成功.期待下次服务。</view>
            </view>
            <!--状态图标-->
            <view class="coreshop-bg-white u-padding-20 u-text-center coreshop-status-img-view u-margin-top-20" v-if="basics == 7">
                <view class="are-img-view">
                    <image class="are-img" src="/static/images/common/arg.png" mode="widthFix" />
                </view>
                <view class="u-font-sm coreshop-text-black">订单已取消</view>
            </view>
        </view>

        <!-- 团购分享拼单 -->
        <view class="coreshop-bg-white coreshop-card-box" v-if="orderInfo.orderType==2 && orderInfo.status != 3 && orderInfo.payStatus!=1">
            <view class="coreshop-card-view coreshop-address-view">
                <view v-if="teamInfo.status==1" class="u-font-lg coreshop-text-bold coreshop-text-black">待拼团，还差{{ teamInfo.teamNums || ''}}人</view>
                <view v-else-if="teamInfo.status==2" class="u-font-lg coreshop-text-bold coreshop-text-black">拼团成功，待发货</view>
                <view v-else-if="teamInfo.status==3" class="u-font-lg coreshop-text-bold coreshop-text-black">拼团失败</view>
                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                <view class="coreshop-cell-group u-margin-top-20 u-margin-bottom-20">
                    <view class="group-swiper">
                        <view class='coreshop-cell-item' v-if="teamInfo.currentCount">
                            <view class='coreshop-cell-item-hd'>
                                <view class="user-head-img-c" v-for="(item, index) in teamInfo.list" :key="index">
                                    <view class="user-head-img-tip" v-if="item.recordId == teamInfo.teamId">拼主</view>
                                    <image class="user-head-img coreshop-head-icon" :src='item.userAvatar' mode=""></image>
                                </view>
                                <view v-if="teamInfo.teamNums > 3">
                                    <view class="uhihn" v-for="n in 3" :key="n">?</view>
                                    <view class="uhihn">···</view>
                                </view>
                                <view v-else>
                                    <view class="uhihn" v-for="n in teamInfo.teamNums" :key="n">?</view>
                                </view>
                            </view>
                            <view class="coreshop-cell-item-ft" v-if="teamInfo.status==1">
                                <u-button type="success" size="mini" @click="goInvition()">邀请拼单</u-button>
                            </view>
                        </view>
                    </view>
                </view>
            </view>
        </view>

        <!--物流信息-->
        <view class="coreshop-bg-white coreshop-card-box" v-if="basics != 0 && !orderInfo.store">
            <view class="coreshop-card-view coreshop-address-view">
                <view class="u-font-lg coreshop-text-bold coreshop-text-black flex justify-between">
                    物流信息
                    <text class="u-font-sm">已发货，请注意查收</text>
                </view>
                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                <view class="wrap">
                    <u-row gutter="16">
                        <u-col span="1">
                            <u-icon name="map-fill" size="38"></u-icon>
                        </u-col>
                        <u-col span="11">
                            <view class="coreshop-text-black">
                                <text>收货人：</text>
                                <text>{{ orderInfo.shipName || ''}}</text>
                                <text class="u-margin-left-10 u-margin-right-10">{{  orderInfo.shipMobile || '' }}</text>
                                <u-tag text="复制" type="success" mode="dark" @click="doCopyData(orderInfo.shipName + ' - ' + orderInfo.shipMobile + ' - ' + orderInfo.shipAreaName + orderInfo.shipAddress)" />
                            </view>
                            <view class="coreshop-text-gray u-font-sm flex">
                                <view class="u-line-2">{{ orderInfo.shipAreaName|| ''}} {{orderInfo.shipAddress || ''}}</view>
                            </view>
                        </u-col>
                    </u-row>
                </view>

                <view class="coreshop-cell-group" v-if="isDelivery">
                    <!--<view class="coreshop-cell-item">
                        <view class="coreshop-cell-bd-view black-text">
                            <text class="coreshop-cell-bd-text">已发货，请注意查收</text>
                        </view>
                    </view>-->
                    <view class='coreshop-cell-item delivery-box' v-for="(v, k) in orderInfo.delivery" :key="k" @click="logistics(k)">
                        <view class='coreshop-cell-item-bd'>
                            <view class="coreshop-cell-bd-view">
                                <text class="coreshop-cell-bd-text">{{v.logiName|| ''}} : {{v.logiNo|| ''}}</text>
                            </view>
                        </view>
                        <view class="coreshop-cell-item-ft">
                            <view class="coreshop-cell-ft-view">{{ v.createTime || ''}}</view>
                            <u-icon name="arrow-right-double u-margin-left-10"></u-icon>
                        </view>
                    </view>
                </view>

            </view>
        </view>

        <!--提货信息-->
        <view class="coreshop-bg-white coreshop-card-box" v-if="orderInfo.store">
            <view class="coreshop-card-view coreshop-order-view">
                <view class="u-font-lg coreshop-text-bold coreshop-text-black">提货信息</view>
                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                <view class="coreshop-text-black coreshop-text-bold title-left-view u-margin-bottom-20">
                    <text>{{orderInfo.store.storeName|| ''}}</text>
                </view>

                <view class="coreshop-text-black title-view">
                    <view class="title">门店电话</view>
                    <view class="u-text-right">
                        <text>{{orderInfo.store.mobile|| '无'}}</text>
                    </view>
                </view> <view class="coreshop-text-black title-view">
                    <view class="title">门店地址</view>
                    <view class="u-text-right">
                        <text> {{orderInfo.store.address|| '无'}}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">提货人信息</view>
                    <view class="u-text-right">
                        <text>{{orderInfo.shipName|| ''}} - {{orderInfo.shipMobile|| ''}}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="lading.status">
                    <view class="title">提货码：</view>
                    <view class="u-text-right">
                        <text class="red-price">{{lading.code|| ''}}</text>
                    </view>
                </view>
            </view>
        </view>

        <!--商品信息-->
        <view class="coreshop-bg-white coreshop-card-box">
            <view class="coreshop-card-view coreshop-shop-view">
                <view class="u-font-lg coreshop-text-bold coreshop-text-black">商品信息</view>
                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                <view class="goods-list-view" v-for="item in orderInfo.items" :key="item.id">
                    <image class="coreshop-avatar radius" :src="item.imageUrl" mode="aspectFill"></image>
                    <view class="goods-info-view">
                        <view class="coreshop-text-black u-line-2" @click="goGoodsDetail(item.goodsId)" v-if="orderInfo.orderType == 1">{{ item.name }}</view>
                        <view class="coreshop-text-black u-line-2" @click="goPinTuanDetail(item.goodsId,orderInfo.objectId)" v-else-if="orderInfo.orderType == 2">{{ item.name }}</view>
                        <view class="coreshop-text-gray u-font-sm u-line-1 introduce" v-if="item.addon">{{ item.addon}}</view>
                        <view class="u-line-1 tag-view">
                            <u-tag :text="v.name" type="success" size="mini" shape="circle" v-for="(v, k) in item.promotionList" :key="k" />
                        </view>
                        <view class="u-flex u-row-between coreshop-order-priceBox">
                            <view class="coreshop-text-red coreshop-text-price u-font-lg">{{ item.price }}</view>
                            <view class="coreshop-text-black u-font-sm coreshop-order-nums">数量：{{ item.nums }}</view>
                        </view>
                    </view>
                </view>

            </view>
        </view>



        <!--发票信息-->
        <view class="coreshop-bg-white coreshop-card-box" v-if="orderInfo.invoice && orderInfo.invoice.type != 1">
            <view class="coreshop-card-view coreshop-order-view">
                <view class="u-font-lg coreshop-text-bold coreshop-text-black">发票信息</view>
                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />

                <view class="coreshop-text-black title-view">
                    <view class="title">发票抬头</view>
                    <view class="u-text-right">
                        <text> {{orderInfo.invoice.title|| '无'}}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">发票税号</view>
                    <view class="u-text-right">
                        <text>{{orderInfo.invoice.taxNumber|| '无'}}</text>
                    </view>
                </view>

            </view>
        </view>


        <!--费用信息-->
        <view class="coreshop-bg-white coreshop-card-box">
            <view class="coreshop-card-view coreshop-price-view">
                <view class="u-font-lg coreshop-text-bold coreshop-text-black">费用信息</view>
                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                <view class="coreshop-text-black title-view" v-if="orderInfo.promotionObj && orderInfo.promotionObj.length > 0">
                    <view class="title">订单优惠</view>
                    <view class="u-text-right">
                        <text class="coreshop-text-price" v-for="(item, key) in orderInfo.promotionObj" :key="key" v-show="item.type == 2">{{ item.name}}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">商品总额</view>
                    <view class="u-text-right">
                        <text class="coreshop-text-price">{{ orderInfo.goodsAmount}}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">运费</view>
                    <view class="u-text-right">
                        <text class="u-margin-right-20">+</text>
                        <text class="coreshop-text-price">{{ orderInfo.costFreight}}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="orderInfo.goodsDiscountAmount > 0">
                    <view class="title">商品优惠</view>
                    <view class="u-text-right">
                        <text class="u-margin-right-20">-</text>
                        <text class="coreshop-text-price">{{ orderInfo.goodsDiscountAmount }}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="orderInfo.pointMoney > 0">
                    <view class="title">积分优惠</view>
                    <view class="u-text-right">
                        <text class="u-margin-right-20">-</text>
                        <text class="coreshop-text-price">{{ orderInfo.pointMoney }}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="orderInfo.orderDiscountAmount > 0">
                    <view class="title">订单优惠</view>
                    <view class="u-text-right">
                        <text class="u-margin-right-20">-</text>
                        <text class="coreshop-text-price">{{ orderInfo.orderDiscountAmount }}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="orderInfo.couponDiscountAmount > 0">
                    <view class="title">优惠券优惠</view>
                    <view class="u-text-right">
                        <text class="u-margin-right-20">-</text>
                        <text class="coreshop-text-price">{{ orderInfo.couponDiscountAmount }}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="orderInfo.payStatus > 1">
                    <view class="title">支付方式</view>
                    <view class="u-text-right">
                        <text>{{ orderInfo.paymentName }}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="orderInfo.payStatus > 1">
                    <view class="title">支付时间</view>
                    <view class="u-text-right">
                        <text>{{ orderInfo.paymentTime}}</text>
                    </view>
                </view>
                <view class="coreshop-text-black coreshop-text-bold title-right-view">
                    <text class="u-margin-right-20">应付款：</text>
                    <text class="coreshop-text-price">{{ orderInfo.orderAmount}}</text>
                </view>

                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />

                <view class="coreshop-text-black u-text-center">
                    <u-button type="primary" size="mini" open-type="contact" bindcontact="showChat">联系客服</u-button>
                </view>
            </view>
        </view>

        <!--订单信息-->
        <view class="coreshop-bg-white coreshop-card-box">
            <view class="coreshop-card-view coreshop-order-view">
                <view class="u-font-lg coreshop-text-bold coreshop-text-black">订单信息（{{ orderInfo.globalStatusText || ''}}）</view>
                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                <view class="coreshop-text-black title-view">
                    <view class="title">订单编号</view>
                    <view class="u-text-right">
                        <text class="u-margin-right-20">{{ orderInfo.orderId || ''}}</text>
                        <u-tag text="复制" type="success" mode="dark" @click="doCopyData(orderInfo.orderId)" />
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">订单类型</view>
                    <view class="u-text-right">
                        <text>
                            {{ orderInfo.typeText || ''}}
                        </text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="orderInfo.paymentName && orderInfo.payStatus > 1">
                    <view class="title">支付方式</view>
                    <view class="u-text-right">
                        <text>{{ orderInfo.paymentName || ''}} </text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="orderInfo.createTime">
                    <view class="title">下单时间</view>
                    <view class="u-text-right">
                        <text>{{ orderInfo.createTime || ''}}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="basics > 0 && orderInfo.paymentTime">
                    <view class="title">支付时间</view>
                    <view class="u-text-right">
                        <text>{{ orderInfo.paymentTime || ''}}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="basics > 1 && delivery && delivery.createTime">
                    <view class="title">发货时间</view>
                    <view class="u-text-right">
                        <text>{{ delivery.createTime || ''}}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="basics > 2 && orderInfo.confirmTime">
                    <view class="title">确认时间</view>
                    <view class="u-text-right">
                        <text>{{ orderInfo.confirmTime || ''}}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view" v-if="basics > 3 && orderInfo.updateTime">
                    <view class="title" v-if="basics >= 7">取消时间</view>
                    <view class="title" v-else>完成时间</view>
                    <view class="u-text-right">
                        <text>{{ orderInfo.updateTime || ''}}</text>
                    </view>
                </view>
            </view>
        </view>

        <view class="coreshop-bg-white coreshop-card-hight-box" />

        <!--为您推荐-->
        <view class="coreshop-recommended-title-view" v-if="otherData.length>0">
            <view class="u-flex u-flex-wrap">
                <view class="u-flex-4 u-text-right">
                    <image class="img-anc" src="/static/images/common/anc.png" mode="widthFix" />
                </view>
                <view class="u-flex-4 u-text-center">
                    <text class="coreshop-text-black u-font-lg">为您推荐</text>
                </view>
                <view class="u-flex-4 u-text-left">
                    <image class="img-anc" src="/static/images/common/anc.png" mode="widthFix" />
                </view>
            </view>
        </view>

        <!--推荐列表-->
        <view class="coreshop-goods-group" v-if="otherData.length>0">
            <u-grid col="2" :border="false" :align="center">
                <u-grid-item bg-color="transparent" :custom-style="{padding: '0rpx'}" v-for="(item, index) in otherData" :key="index" @click="goGoodsDetail(item.id)">
                    <view class="good_box">
                        <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                        <u-lazy-load threshold="-150" border-radius="10" :image="item.image" :index="index"></u-lazy-load>
                        <view class="good_title u-line-2">
                            {{item.name}}
                        </view>
                        <view class="good-price">
                            {{item.price}}元 <span class="u-font-xs  coreshop-text-through u-margin-left-15 coreshop-text-gray">{{item.mktprice}}元</span>
                        </view>
                        <view class="good-tag-recommend" v-if="item.isRecommend">
                            推荐
                        </view>
                        <view class="good-tag-hot" v-if="item.isHot">
                            热门
                        </view>
                    </view>
                </u-grid-item>
            </u-grid>
        </view>


        <!--底部-->
        <view class="coreshop-foot-hight-view" />

        <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom u-text-center u-padding-top-10" v-if="orderInfo.status == 1 || orderInfo.status == 2">
            <u-button class='u-margin-left-10 u-margin-right-10' type="warning" size="mini" v-if="orderInfo.status == 1 && orderInfo.payStatus == 1 && orderInfo.shipStatus == 1" @click="cancelOrder(orderInfo.orderId)">取消订单</u-button>
            <u-button class='u-margin-left-10 u-margin-right-10' type="primary" size="mini" v-if="orderInfo.status == 1 && orderInfo.payStatus == 1" @click="goToPay(orderInfo.orderId)">立即支付</u-button>
            <u-button class='u-margin-left-10 u-margin-right-10' type="success" size="mini" v-if="orderInfo.status == 1 && orderInfo.payStatus >= 2 && orderInfo.shipStatus >= 3 && orderInfo.confirmStatus == 1" @click="tackDeliery(orderInfo.orderId)">确认收货</u-button>
            <u-button class='u-margin-left-10 u-margin-right-10' type="success" size="mini" v-if="orderInfo.status === 1 && orderInfo.payStatus >= 2 && orderInfo.shipStatus >= 3 && orderInfo.confirmStatus >= 2 && orderInfo.isComment == false" @click="toEvaluate(orderInfo.orderId)">立即评价</u-button>
            <u-button class='u-margin-left-10 u-margin-right-10' type="default" size="mini" @click="customerService(orderInfo.orderId)" v-if="orderInfo.addAftersalesStatus">申请售后</u-button>
            <u-button class='coreshop-bg-red u-margin-left-10 u-margin-right-10' size="mini" @click="showCustomerService(orderInfo)" v-if="orderInfo.billAftersalesId && orderInfo.billAftersalesId != false">查看售后</u-button>
        </view>

    </view>

</template>
<script>
    import { orders, goods, tools } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [orders, goods, tools],
        data() {
            return {
                numList: [{ name: '下单' }, { name: '付款' }, { name: '发货' }, { name: '收货' }, { name: '评价' }],
                basics: 0,
                delivery: {},//发货信息
                orderId: 0,
                orderInfo: {}, // 订单详情
                teamInfo: [], //拼团团信息
                otherData: [], //其他信息
                lading: {
                    status: false,
                    code: ''
                }, //提货信息
            }
        },
        onLoad(options) {
            this.orderId = options.orderId
            if (this.orderId) {
                //改用show操作
                //this.orderDetail()
            } else {
                this.$refs.uToast.show({ title: '获取失败', type: 'error', back: true });
            }
            this.getGoodsRecommendList();
        },
        onShow() {
            this.orderDetail();
        },
        computed: {
            // 判断是否发货
            isDelivery() {
                if (this.orderInfo && this.orderInfo.delivery && Object.keys(this.orderInfo.delivery).length) {
                    return true
                } else {
                    return false
                }
            },
            orderCancelTime() {
                return this.$store.state.config.orderCancelTime || 60;
            },
        },
        methods: {
            // 获取订单详情
            orderDetail() {
                let _this = this
                let data = {
                    id: _this.orderId
                }
                _this.$u.api.orderDetail(data).then(res => {
                    if (res.status) {
                        let data = res.data;
                        // 订单状态文字转化
                        _this.basics = data.status;
                        console.log(_this.basics);
                        switch (data.status) {
                            case 1:
                                if (data.payStatus === 1) {
                                    _this.$set(data, 'statusName', '待付款')
                                    _this.basics = 0;
                                } else if (data.payStatus >= 2 && data.shipStatus === 1) {
                                    _this.$set(data, 'statusName', '待发货')
                                    _this.basics = 1;
                                } else if (data.payStatus >= 2 && data.shipStatus === 2) {
                                    _this.$set(data, 'statusName', '部分发货')
                                    _this.basics = 2;
                                } else if (data.payStatus >= 2 && data.shipStatus >= 3 && data.confirmStatus === 1) {
                                    _this.$set(data, 'statusName', '已发货')
                                    _this.basics = 2;
                                } else if (data.payStatus >= 2 && data.shipStatus >= 3 && data.confirmStatus >= 2 && data.isComment === false) {
                                    _this.$set(data, 'statusName', '待评价')
                                    _this.basics = 3;
                                } else if (data.payStatus >= 2 && data.shipStatus >= 3 && data.confirmStatus >= 2 && data.isComment === true) {
                                    _this.$set(data, 'statusName', '已评价')
                                    _this.basics = 4;
                                }
                                break
                            case 2:
                                _this.$set(data, 'statusName', data.textStatus)
                                _this.basics = data.globalStatus;
                                break
                            case 3:
                                _this.$set(data, 'statusName', data.textStatus)
                                _this.basics = data.globalStatus;
                                break
                        }
                        // 转换优惠信息
                        for (let i in data.items) {
                            data.items[i].promotionList = JSON.parse(data.items[i].promotionList);
                        }
                        _this.orderInfo = data;

                        if (data.delivery.length > 0) {
                            _this.delivery = data.delivery[0];
                        }

                        //判断是否拼团
                        if (data.orderType == 2) {
                            _this.getTeam(data.orderId);
                        }
                        if (data.ladingItem[0]) {
                            _this.lading = {
                                status: true,
                                code: data.ladingItem[0].id
                            }
                        }
                    } else {
                        _this.$u.toast(res.msg)
                    }
                })
            },
            // 获取推荐商品信息
            getGoodsRecommendList() {
                let _this = this;
                let data = {
                    id: 10
                }
                _this.$u.api.getGoodsRecommendList(data).then(res => {
                    if (res.status) {
                        _this.otherData = res.data;
                    } else {
                        _this.$u.toast(res.msg)
                    }
                })
            },
            // 取消订单
            cancelOrder(orderId) {
                let _this = this;
                _this.$common.modelShow('提示', '确认要取消订单吗?', () => {
                    let data = {
                        id: orderId
                    }
                    _this.$u.api.cancelOrder(data).then(res => {
                        if (res.status) {
                            _this.$refs.uToast.show({
                                title: res.msg, type: 'success', callback: function () {
                                    _this.orderDetail()
                                }
                            })
                        } else {
                            _this.$u.toast(res.msg)
                        }
                    })
                })
            },
            // 确认收货
            tackDeliery(orderId) {
                let _this = this;
                this.$common.modelShow('提示', '确认收货操作吗?', () => {
                    let data = {
                        id: orderId
                    }
                    _this.$u.api.confirmOrder(data).then(res => {
                        if (res.status) {
                            _this.$refs.uToast.show({
                                title: '确认收货成功', type: 'success', callback: function () {
                                    // 更改订单列表页的订单状态
                                    let pages = getCurrentPages(); // 当前页
                                    let beforePage = pages[pages.length - 2]; // 上个页面
                                    if (beforePage !== undefined && beforePage.route === 'pages/member/order/index/index') {
                                        beforePage.$vm.isReload = true
                                    }
                                    _this.orderDetail()
                                }
                            })
                        } else {
                            _this.$u.toast(res.msg)
                        }
                    })
                })
            },
            // 申请售后
            customerService(id) {
                this.$u.route('/pages/member/afterSales/submit/submit?orderId=' + id);
            },
            //快递信息
            logistics(key) {
                let address1 = this.orderInfo.shipAreaName ? this.orderInfo.shipAreaName : ''
                let address2 = this.orderInfo.shipAddress ? this.orderInfo.shipAddress : ''
                let address = address1 + address2
                this.goShowExpress(this.orderInfo.delivery[key].logiCode, this.orderInfo.delivery[key].logiNo, address, this.orderInfo.shipMobile)
            },
            //查看售后
            showCustomerService(info) {
                if (info.aftersalesItem.length == 1) {
                    this.$u.route('/pages/member/afterSales/detail/detail?aftersalesId=' + info.billAftersalesId);
                } else if (info.aftersalesItem.length > 1) {
                    this.$u.route('/pages/member/afterSales/list/list?orderId=' + info.orderId);
                }
            },
            //邀请拼单
            goInvition() {
                uni.navigateTo({
                    url: '/pages/member/order/invitationGroup/invitationGroup?orderId=' + this.orderInfo.orderId + '&closeTime=' + this.teamInfo.closeTime
                })
            },
            //拼团信息
            getTeam(id) {
                this.$u.api.getOrderPinTuanTeamInfo({
                    orderId: id
                }).then(res => {
                    if (res.status) {
                        this.teamInfo = {
                            list: res.data.teams,
                            userAvatar: res.data.userAvatar,
                            currentCount: res.data.teams.length,
                            peopleNumber: res.data.peopleNumber,
                            teamNums: res.data.teamNums, //剩余
                            closeTime: res.data.closeTime, //关闭时间
                            id: res.data.id, //拼团id
                            teamId: res.data.teamId, //拼团团队id
                            ruleId: res.data.ruleId,
                            status: res.data.status
                        };
                    } else {
                        this.$u.toast(res.msg)
                    }
                });
            },
            goTaxList() {
                if (this.orderInfo && this.orderInfo.invoice && this.orderInfo.invoice.id) {
                    uni.navigateTo({
                        url: '/pages/member/invoice/index?id=' + this.orderInfo.invoice.id
                    });
                }
            }
        }
    }
</script>
<style lang="scss" scoped>
    @import "detail.scss";
</style>