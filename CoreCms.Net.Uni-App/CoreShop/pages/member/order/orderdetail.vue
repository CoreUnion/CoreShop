<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="订单详情"></u-navbar>

        <!--步骤条区域-->
        <view class="bg-white padding solid-top" v-if="basics < 9">
            <!--步骤条-->
            <view class="cu-steps" v-if="basics < 5">
                <block v-for="(item,index) in basicsList" :key="index">
                    <view class="cu-item" :class="index>basics?'':'select'">
                        <view class="icon-view" v-if="index>basics">
                            <text class="text-red" :class="'cuIcon-' + item.cuIcon"></text>
                        </view>
                        <view class="bg-red icon-view" v-else>
                            <text :class="'cuIcon-' + item.cuIcon"></text>
                        </view>
                        <view class="text-sm text-black" v-if="index>basics">{{item.name}}</view>
                        <view class="text-sm text-black" v-else>{{item.name_s}}</view>
                    </view>
                </block>
            </view>

            <!--提示-->
            <view class="text-sm text-center margin-top" v-if="basics == 0">
                <view class="text-black">拍下成功，待买家支付。</view>
                <view class="text-black">
                    <text class="text-red">拍下{{orderCancelTime}}分后</text>
                    <text>未支付，自动取消订单。</text>
                </view>
            </view>
            <view class="text-sm text-center margin-top" v-if="basics == 1">
                <view class="text-black">支付成功，待卖家发货.</view>
            </view>
            <view class="text-sm text-center margin-top" v-if="basics == 2">
                <view class="text-black">已发货，快递正在路上，务必在收到商品后再确认收货。</view>
                <view class="text-black">
                    <text class="text-red">发货20天</text>
                    <text>后将自动确认收货</text>
                </view>
            </view>
            <view class="text-sm text-center margin-top" v-if="basics == 3">
                <view class="text-black">已收货，请您对此次购物体检进行评价。</view>
                <view class="text-black">
                    <text class="text-red">收货30天</text>
                    <text>后将自动评价</text>
                </view>
            </view>
            <!--状态图标-->
            <view class="bg-white padding solid-top text-center coreshop-status-img-view u-margin-top-20" v-if="basics == 4">
                <view class="are-img-view">
                    <image class="are-img" src="/static/images/common/are.png" mode="widthFix" />
                </view>
                <view class="text-sm text-black">交易成功，感谢您的评价</view>
            </view>
            <!--状态图标-->
            <view class="bg-white padding solid-top text-center coreshop-status-img-view u-margin-top-20" v-if="basics == 6">
                <view class="are-img-view">
                    <image class="are-img" src="/static/images/common/are.png" mode="widthFix" />
                </view>
                <view class="text-sm text-black">交易成功.期待下次服务。</view>
            </view>
            <!--状态图标-->
            <view class="bg-white padding solid-top text-center coreshop-status-img-view u-margin-top-20" v-if="basics == 7">
                <view class="are-img-view">
                    <image class="are-img" src="/static/images/common/arg.png" mode="widthFix" />
                </view>
                <view class="text-sm text-black">订单已取消</view>
            </view>
        </view>



        <!-- 团购分享拼单 -->
        <view class="bg-white coreshop-card-box" v-if="orderInfo.orderType==2 && orderInfo.status != 3 && orderInfo.payStatus!=1">
            <view class="coreshop-card-view coreshop-address-view">
                <view v-if="teamInfo.status==1" class="text-lg text-bold text-black">待拼团，还差{{ teamInfo.teamNums || ''}}人</view>
                <view v-else-if="teamInfo.status==2" class="text-lg text-bold text-black">拼团成功，待发货</view>
                <view v-else-if="teamInfo.status==3" class="text-lg text-bold text-black">拼团失败</view>
                <view class="solid-line"></view>
                <view class="cell-group margin-cell-group">
                    <view class="group-swiper">
                        <view class='cell-item' v-if="teamInfo.currentCount">
                            <view class='cell-item-hd'>
                                <view class="user-head-img-c" v-for="(item, index) in teamInfo.list" :key="index">
                                    <view class="user-head-img-tip" v-if="item.recordId == teamInfo.teamId">拼主</view>
                                    <image class="user-head-img cell-hd-icon have-none" :src='item.userAvatar' mode=""></image>
                                </view>
                                <view v-if="teamInfo.teamNums > 3">
                                    <view class="uhihn" v-for="n in 3" :key="n">?</view>
                                    <view class="uhihn">···</view>
                                </view>
                                <view v-else>
                                    <view class="uhihn" v-for="n in teamInfo.teamNums" :key="n">?</view>
                                </view>
                            </view>
                            <view class="cell-item-ft" v-if="teamInfo.status==1">
                                <button class="cu-btn round bg-yellow sm" @click="goInvition()">邀请拼单</button>
                            </view>
                        </view>
                    </view>
                </view>
            </view>
        </view>




        <!--物流信息-->
        <view class="bg-white coreshop-card-box" v-if="basics != 0 && !orderInfo.store">
            <view class="coreshop-card-view coreshop-address-view">
                <view class="text-lg text-bold text-black">物流信息</view>
                <view class="solid-line"></view>
                <view class="cu-list menu-avatar">
                    <view class="cu-item">
                        <view class="bg-grey icon-view">
                            <text class="cuIcon-locationfill" />
                        </view>
                        <view class="content">
                            <view class="text-black">
                                <text>收货人：</text>
                                <text>{{ orderInfo.shipName || ''}}</text>
                                <text class="margin-left">{{ orderInfo.shipMobile || ''}}</text>
                            </view>
                            <view class="text-gray text-sm flex">
                                <view class="text-cut">{{ orderInfo.shipAreaName|| ''}} {{orderInfo.shipAddress || ''}}</view>
                            </view>
                        </view>
                    </view>
                </view>

                <view class="delivery" v-if="isDelivery">
                    <view class="cell-item">
                        <view class="cell-bd-view black-text">
                            <text class="cell-bd-text">已发货，请注意查收</text>
                        </view>
                    </view>
                    <view class='cell-item add-title-item' v-for="(v, k) in orderInfo.delivery" :key="k" @click="logistics(k)">
                        <view class='cell-item-bd'>
                            <view class="cell-bd-view">
                                <text class="cell-bd-text">{{v.logiName|| ''}} : {{v.logiNo|| ''}}</text>
                            </view>

                        </view>
                        <view class="cell-item-ft">
                            <view class="cell-ft-view"> {{ v.createTime || ''}}</view>
                            <text class="cuIcon-right icon" />
                        </view>
                    </view>
                </view>

            </view>
        </view>


        <!--提货信息-->
        <view class="bg-white coreshop-card-box" v-if="orderInfo.store">
            <view class="coreshop-card-view coreshop-order-view">
                <view class="text-lg text-bold text-black">提货信息</view>
                <view class="solid-line"></view>
                <view class="text-black text-bold title-left-view u-margin-buttom-20">
                    <text>{{orderInfo.store.storeName|| ''}}</text>
                </view>

                <view class="text-black title-view">
                    <view class="title">门店电话</view>
                    <view class="text-right">
                        <text>{{orderInfo.store.mobile|| '无'}}</text>
                    </view>
                </view> <view class="text-black title-view">
                    <view class="title">门店地址</view>
                    <view class="text-right">
                        <text> {{orderInfo.store.address|| '无'}}</text>
                    </view>
                </view>
                <view class="text-black title-view">
                    <view class="title">提货人信息</view>
                    <view class="text-right">
                        <text>{{orderInfo.shipName|| ''}} - {{orderInfo.shipMobile|| ''}}</text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="lading.status">
                    <view class="title">提货码：</view>
                    <view class="text-right">
                        <text class="red-price">{{lading.code|| ''}}</text>
                    </view>
                </view>

            </view>
        </view>

        <!--商品信息-->
        <view class="bg-white coreshop-card-box">
            <view class="coreshop-card-view coreshop-shop-view">

                <view class="goods-list-view" v-for="item in orderInfo.items" :key="item.id">
                    <view class="cu-avatar radius" :style="[{backgroundImage:'url('+ item.imageUrl +')'}]" />
                    <view class="goods-info-view">
                        <view class="text-black u-line-2" @click="goGoodsDetail(item.goodsId)" v-if="orderInfo.orderType == 1">{{ item.name }}</view>
                        <view class="text-black u-line-2" @click="goPinTuanDetail(item.goodsId)" v-else-if="orderInfo.orderType == 2">{{ item.name }}</view>
                        <view class="text-gray text-sm text-cut introduce" v-if="item.addon !== null">{{ item.addon || ''}}</view>
                        <view class="text-cut tag-view">
                            <!--<text class="cu-tag sm line-blue radius" v-if="item.promotionObj">{{ item.promotionObj.name}}</text>-->
                            <text class="cu-tag line-blue sm radius" v-for="(v, k) in item.promotionList" :key="k"> {{ v.name || ''}}</text>
                        </view>
                        <view class="text-price text-red text-lg priceBox">{{ item.price }} <view class="text-black text-sm nums">× {{ item.nums }}</view></view>

                    </view>
                </view>

            </view>
        </view>



        <!--发票信息-->
        <view class="bg-white coreshop-card-box" v-if="orderInfo.invoice && orderInfo.invoice.type != 1">
            <view class="coreshop-card-view coreshop-order-view">
                <view class="text-lg text-bold text-black">发票信息</view>
                <view class="solid-line"></view>

                <view class="text-black title-view">
                    <view class="title">发票抬头</view>
                    <view class="text-right">
                        <text> {{orderInfo.invoice.title|| '无'}}</text>
                    </view>
                </view>
                <view class="text-black title-view">
                    <view class="title">发票税号</view>
                    <view class="text-right">
                        <text>{{orderInfo.invoice.taxNumber|| '无'}}</text>
                    </view>
                </view>

            </view>
        </view>


        <!--商品金额-->
        <view class="bg-white coreshop-card-box">
            <view class="coreshop-card-view coreshop-price-view">

                <view class="text-black title-view" v-if="orderInfo.promotionObj && orderInfo.promotionObj.length > 0">
                    <view class="title">订单优惠</view>
                    <view class="text-right">
                        <text class="text-price" v-for="(item, key) in orderInfo.promotionObj" :key="key" v-show="item.type == 2">{{ item.name}}</text>
                    </view>
                </view>

                <view class="text-black title-view">
                    <view class="title">商品总额</view>
                    <view class="text-right">
                        <text class="text-price">{{ orderInfo.goodsAmount}}</text>
                    </view>
                </view>
                <view class="text-black title-view">
                    <view class="title">运费</view>
                    <view class="text-right">
                        <text class="margin-right-xs">+</text>
                        <text class="text-price">{{ orderInfo.costFreight}}</text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="orderInfo.goodsDiscountAmount > 0">
                    <view class="title">商品优惠</view>
                    <view class="text-right">
                        <text class="margin-right-xs">-</text>
                        <text class="text-price">{{ orderInfo.goodsDiscountAmount }}</text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="orderInfo.pointMoney > 0">
                    <view class="title">积分优惠</view>
                    <view class="text-right">
                        <text class="margin-right-xs">-</text>
                        <text class="text-price">{{ orderInfo.pointMoney }}</text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="orderInfo.orderDiscountAmount > 0">
                    <view class="title">订单优惠</view>
                    <view class="text-right">
                        <text class="margin-right-xs">-</text>
                        <text class="text-price">{{ orderInfo.orderDiscountAmount }}</text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="orderInfo.couponDiscountAmount > 0">
                    <view class="title">优惠券优惠</view>
                    <view class="text-right">
                        <text class="margin-right-xs">-</text>
                        <text class="text-price">{{ orderInfo.couponDiscountAmount }}</text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="orderInfo.payStatus > 1">
                    <view class="title">支付方式</view>
                    <view class="text-right">
                        <text>{{ orderInfo.paymentName }}</text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="orderInfo.payStatus > 1">
                    <view class="title">支付时间</view>
                    <view class="text-right">
                        <text>{{ orderInfo.paymentTime}}</text>
                    </view>
                </view>
                <view class="text-black text-bold title-right-view">
                    <text class="margin-right-xs">应付款：</text>
                    <text class="text-price">{{ orderInfo.orderAmount}}</text>
                </view>

                <view class="solid-line"></view>

                <view class="text-center text-black">联系客服</view>
            </view>
        </view>

        <!--订单信息-->
        <view class="bg-white coreshop-card-box">
            <view class="coreshop-card-view coreshop-order-view">
                <view class="text-lg text-bold text-black">订单信息（{{ orderInfo.globalStatusText || ''}}）</view>
                <view class="solid-line"></view>
                <view class="text-black title-view">
                    <view class="title">订单编号</view>
                    <view class="text-right" hover-class="btn-hover" @click="doCopyData(orderInfo.orderId)">
                        <text class="margin-right-xs">{{ orderInfo.orderId || ''}}</text>
                        <button class="cu-btn sm line-black">复制</button>
                    </view>
                </view>
                <view class="text-black title-view">
                    <view class="title">订单类型</view>
                    <view class="text-right">
                        <text>
                            {{ orderInfo.typeText || ''}}
                        </text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="orderInfo.paymentName && orderInfo.payStatus > 1">
                    <view class="title">支付方式</view>
                    <view class="text-right">
                        <text>{{ orderInfo.paymentName || ''}} </text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="orderInfo.createTime">
                    <view class="title">下单时间</view>
                    <view class="text-right">
                        <text>{{ orderInfo.createTime || ''}}</text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="basics > 0 && orderInfo.paymentTime">
                    <view class="title">支付时间</view>
                    <view class="text-right">
                        <text>{{ orderInfo.paymentTime || ''}}</text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="basics > 1 && delivery && delivery.createTime">
                    <view class="title">发货时间</view>
                    <view class="text-right">
                        <text>{{ delivery.createTime || ''}}</text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="basics > 2 && orderInfo.confirmTime">
                    <view class="title">确认时间</view>
                    <view class="text-right">
                        <text>{{ orderInfo.confirmTime || ''}}</text>
                    </view>
                </view>
                <view class="text-black title-view" v-if="basics > 3 && orderInfo.updateTime">
                    <view class="title" v-if="basics >= 7">取消时间</view>
                    <view class="title" v-else>完成时间</view>
                    <view class="text-right">
                        <text>{{ orderInfo.updateTime || ''}}</text>
                    </view>
                </view>
            </view>
        </view>

        <view class="bg-white coreshop-card-hight-box" />

        <!--为您推荐-->
        <view class="coreshop-title-view" v-if="otherData.length>0">
            <view class="flex flex-wrap">
                <view class="basis-sm text-right">
                    <image class="img-anc" src="/static/images/common/anc.png" mode="widthFix" />
                </view>
                <view class="basis-xs text-center">
                    <text class="text-black text-lg">为您推荐</text>
                </view>
                <view class="basis-sm text-left">
                    <image class="img-anc" src="/static/images/common/anc.png" mode="widthFix" />
                </view>
            </view>
        </view>

        <!--推荐列表-->
        <view class="coreshop-recommend-goods-list-view" v-if="otherData.length>0">
            <view class="flex flex-wrap">
                <view class="basis-df padding-sm padding-right-xs" v-for="(item, index) in otherData" :key="index" @click="goGoodsDetail(item.id)">
                    <view class="bg-white margin-bottom-sm list-itme">
                        <view class="cu-avatar" :style="[{backgroundImage:'url('+ item.image +')'}]" />
                        <view class="goods-info-view">
                            <view class="text-cut text-black">{{item.name}}</view>
                            <view class="text-price text-red text-lg">{{item.price}}</view>
                            <view class="foot-box">
                                <view class="text-gray text-sm address">{{item.buyCount}}人购买</view>
                                <text class="cu-tag line-red sm radius">找相似</text>
                            </view>
                        </view>
                    </view>
                </view>

            </view>
        </view>

        <!--底部-->
        <view class="foot-hight-view" />

        <view class="bg-white coreshop-footer-fixed coreshop-foot-padding-bottom" v-if="orderInfo.status == 1 || orderInfo.status == 2">
            <button class='cu-btn line-black radius' hover-class="btn-hover" v-if="orderInfo.status == 1 && orderInfo.payStatus == 1 && orderInfo.shipStatus == 1" @click="cancelOrder(orderInfo.orderId)">取消订单</button>
            <button class='cu-btn bg-red' hover-class="btn-hover" v-if="orderInfo.status == 1 && orderInfo.payStatus == 1" @click="goToPay(orderInfo.orderId)">立即支付</button>
            <button class='cu-btn bg-red' hover-class="btn-hover" v-if="orderInfo.status == 1 && orderInfo.payStatus >= 2 && orderInfo.shipStatus >= 3 && orderInfo.confirmStatus == 1" @click="tackDeliery(orderInfo.orderId)">确认收货</button>
            <button class='cu-btn bg-red' hover-class="btn-hover" v-if="orderInfo.status === 1 && orderInfo.payStatus >= 2 && orderInfo.shipStatus >= 3 && orderInfo.confirmStatus >= 2 && orderInfo.isComment == false" @click="toEvaluate(orderInfo.orderId)">立即评价</button>
            <button class='cu-btn bg-red' hover-class="btn-hover" @click="customerService(orderInfo.orderId)" v-if="orderInfo.addAftersalesStatus">申请售后</button>
            <button class='cu-btn bg-red' hover-class="btn-hover" @click="showCustomerService(orderInfo)" v-if="orderInfo.billAftersalesId && orderInfo.billAftersalesId != false">查看售后</button>
        </view>

    </view>

</template>
<script>
    import { orders, goods, tools } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [orders, goods, tools],
        data() {
            return {
                basics: 0,
                basicsList: [
                    { cuIcon: 'cartfill', name: '未拍下', name_s: '已拍下', nameId: 0 },
                    { cuIcon: 'card', name: '待付款', name_s: '已付款', nameId: 1 },
                    { cuIcon: 'deliver_fill', name: '待发货', name_s: '已发货', nameId: 2 },
                    { cuIcon: 'formfill', name: '待收货', name_s: '已收货', nameId: 3 },
                    { cuIcon: 'presentfill', name: '待评价', name_s: '已评价', nameId: 4 }
                ],
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
                                    if (beforePage !== undefined && beforePage.route === 'pages/member/order/orderlist') {
                                        // #ifdef MP-WEIXIN
                                        beforePage.$vm.isReload = true
                                        // #endif
                                        // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE
                                        beforePage.isReload = true
                                        // #endif
                                        // #ifdef MP-ALIPAY || MP-TOUTIAO
                                        _this.$db.set('orderUserShip', true, true);
                                        // #endif
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
                this.$u.route('/pages/member/afterSales/index?orderId=' + id);
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
                    this.$u.route('/pages/member/afterSales/detail?aftersalesId=' + info.billAftersalesId);
                } else if (info.aftersalesItem.length > 1) {
                    this.$u.route('/pages/member/afterSales/list?orderId=' + info.orderId);
                }
            },
            //邀请拼单
            goInvition() {
                uni.navigateTo({
                    url: '/pages/member/order/invitationGroup?orderId=' + this.orderInfo.orderId + '&closeTime=' + this.teamInfo.closeTime
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
    @import '../../../static/style/orderDetails.scss';

</style>