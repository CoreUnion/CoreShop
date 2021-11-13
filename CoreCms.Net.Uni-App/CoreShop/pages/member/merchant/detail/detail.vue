<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="订单详情"></u-navbar>

        <!--步骤条区域-->
        <view class="coreshop-bg-white u-padding-right-20 u-padding-left-20 u-padding-top-30 coreshop-solid-top" v-if="basics < 9">
            <!--步骤条-->
            <u-steps :list="basicsList" :current="basics" mode="number" v-if="basics < 5"></u-steps>
            <!--状态图标-->
            <view class="coreshop-bg-white padding coreshop-solid-top u-text-center coreshop-status-img-view u-margin-top-20" v-if="basics == 7">
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
                                    <image class="user-head-img coreshop-head-icon " :src='item.userAvatar' mode=""></image>
                                </view>
                                <view v-if="teamInfo.teamNums > 3">
                                    <view class="uhihn" v-for="n in 3" :key="n">?</view>
                                    <view class="uhihn">···</view>
                                </view>
                                <view v-else>
                                    <view class="uhihn" v-for="n in teamInfo.teamNums" :key="n">?</view>
                                </view>
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
                <view class="solid-line" v-if="isDelivery"></view>
                <view class="delivery" v-if="isDelivery">
                    <view class='coreshop-cell-item add-title-item' v-for="(v, k) in orderInfo.delivery" :key="k" @click="logistics(k)">
                        <view class='coreshop-cell-item-bd'>
                            <view class="coreshop-cell-bd-view">
                                <text class="coreshop-cell-bd-text">{{v.logiName|| ''}} : {{v.logiNo|| ''}}</text>
                            </view>

                        </view>
                        <view class="coreshop-cell-item-ft">
                            <view class="coreshop-cell-ft-view"> {{ v.createTime || ''}}</view>
                            <u-icon name="arrow-right-double"></u-icon>
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
                <view v-if="ladingItem">
                    <view class="coreshop-text-black title-view">
                        <view class="title">提货码：</view>
                        <view class="u-text-right">
                            <text class="red-price">{{ladingItem.id|| ''}}</text>
                        </view>
                    </view>
                    <view class="coreshop-text-black title-view">
                        <view class="title">提货说明：</view>
                        <view class="u-text-right">
                            <text class="red-price u-margin-right-10">{{ladingItem.statusName|| ''}}</text>
                            <u-button type="success" size="mini" v-if="ladingItem.status == false" @click="ladingWrite(ladingItem.id)">立即核销</u-button>
                        </view>
                    </view>
                    <view class="coreshop-text-black title-view" v-if="ladingItem.pickUpTime">
                        <view class="title">提货时间：</view>
                        <view class="u-text-right">
                            <text class="red-price">{{ladingItem.pickUpTime|| ''}}</text>
                        </view>
                    </view>
                </view>
            </view>
        </view>

        <!--商品信息-->
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
                            <u-tag :text="v.name" type="success" shape="circle" v-for="(v, k) in item.promotionList" :key="k" />
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


        <!--商品金额-->
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
            </view>
        </view>

        <!--订单信息-->
        <view class="coreshop-bg-white coreshop-card-box">
            <view class="coreshop-card-view coreshop-order-view">
                <view class="u-font-lg coreshop-text-bold coreshop-text-black">订单信息（{{ orderInfo.globalStatusText || ''}}）</view>
                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                <view class="coreshop-text-black title-view">
                    <view class="title">订单编号</view>
                    <view class="u-text-right" @click="doCopyData(orderInfo.orderId)">
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


        <!--底部-->
        <view class="coreshop-foot-hight-view" />
        <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom u-text-center u-padding-20" v-if="orderInfo.status == 1 && !isDelivery">
            <u-button class='coreshop-bg-red' type="success" size="default" @click="tackDeliery(orderInfo.orderId)">立即发货</u-button>
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
                    { name: '下单' },
                    { name: '付款' },
                    { name: '发货' },
                    { name: '收货' },
                    { name: '评价' }
                ],
                delivery: {},//发货信息
                orderId: 0,
                orderInfo: {}, // 订单详情
                teamInfo: [], //拼团团信息
                otherData: [], //其他信息
                ladingItem: {},
                lading: {
                    status: false,
                    code: ''
                }, //提货信息
            }
        },
        onLoad(options) {
            this.orderId = options.orderId
            if (this.orderId) {
                this.orderDetail();
            } else {
                this.$refs.uToast.show({ title: '订单获取失败', type: 'error', back: true });
            }
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
                    id: _this.orderId,
                    data: 'merchant'
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
                            _this.ladingItem = data.ladingItem[0];
                        }
                    } else {
                        _this.$u.toast(res.msg)
                    }
                })
            },
            //快递信息
            logistics(key) {
                let address1 = this.orderInfo.shipAreaName ? this.orderInfo.shipAreaName : ''
                let address2 = this.orderInfo.shipAddress ? this.orderInfo.shipAddress : ''
                let address = address1 + address2
                this.goShowExpress(this.orderInfo.delivery[key].logiCode, this.orderInfo.delivery[key].logiNo, address, this.orderInfo.shipMobile)
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
            //提货单核销
            ladingWrite(id) {
                this.$u.route('/pages/member/merchant/takeDelivery/index?id=' + id);
            },
        }
    }
</script>
<style lang="scss" scoped>
    @import "detail.scss";
</style>