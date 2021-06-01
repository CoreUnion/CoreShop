<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="售后详情"></u-navbar>
        <view class="u-padding-bottom-20 bg-white">

            <!--状态图标-->
            <view class="bg-white padding solid-top text-center coreshop-status-img-view " v-if="status==2 && refundStatus==2">
                <view class="are-img-view">
                    <image class="are-img" src="/static/images/common/are.png" mode="widthFix" />
                </view>
                <view class="text-sm text-black">退款成功</view>
            </view>


            <!--状态图标-->
            <view class="bg-white padding solid-top text-center coreshop-status-img-view" v-if="status==3">
                <view class="are-img-view">
                    <image class="are-img" src="/static/images/common/arg.png" mode="widthFix" />
                </view>
                <view class="text-sm text-black">申请驳回</view>
                <view class="text-sm text-black">驳回原因：{{mark}}</view>
            </view>

            <!--退款单状态-->
            <view class="bg-white coreshop-card-box">
                <view class="coreshop-card-view coreshop-price-view">
                    <view class="text-lg text-bold text-black">售后单进度</view>
                    <view class="solid-line"></view>
                    <view class="text-center text-black">
                        <view class="cu-btn round bg-yellow u-margin-left-20 u-margin-right-20" v-if="statusName">{{statusName}}</view>
                        <view class="cu-btn round bg-yellow u-margin-left-20 u-margin-right-20" v-if="reshipName">{{reshipName}}</view>
                        <view class="cu-btn round bg-yellow u-margin-left-20 u-margin-right-20" v-if="refundName">{{refundName}}</view>
                    </view>
                </view>
            </view>

            <!--物流信息-->
            <view class="bg-white coreshop-card-box" v-if="status == 2 && reshipStatus == 1 && refundStatus!=2">
                <view class="coreshop-card-view coreshop-address-view">
                    <view class="text-lg text-bold text-black">退货邮寄信息</view>
                    <view class="solid-line"></view>
                    <view class="cu-list menu-avatar">
                        <view class="cu-item">
                            <view class="bg-grey icon-view">
                                <text class="cuIcon-locationfill" />
                            </view>
                            <view class="content">
                                <view class="text-black">
                                    <text>收货人：</text>
                                    <text>{{ reshipInfo.reshipName }}</text>
                                    <text class="margin-left">{{ reshipInfo.reshipMobile }}</text>
                                </view>
                                <view class="text-gray text-sm flex">
                                    <view class="text-cut">{{ reshipInfo.reshipArea + reshipInfo.reshipAddress }}</view>
                                </view>
                            </view>
                        </view>
                    </view>
                </view>
            </view>

            <!--商品信息-->
            <view class="bg-white coreshop-card-box" v-if="goodsInfo.length>0">
                <view class="coreshop-card-view coreshop-shop-view">
                    <view class="text-black text-bold text-lg title-view">商品信息</view>
                    <view class="solid-line"></view>
                    <view class="goods-list-view" v-for="(item, key) in goodsInfo" :key="key">
                        <view class="cu-avatar radius" :style="[{backgroundImage:'url('+ item.imageUrl +')'}]" />
                        <view class="goods-info-view">
                            <view class="text-black text-cut name">{{item.name}} </view>
                            <view class="text-gray text-sm text-cut introduce">售后单号：{{item.aftersalesId}}</view>
                            <view class="text-cut tag-view" v-if="item.addon">
                                <text class="cu-tag sm line-blue radius">{{item.addon}}</text>
                            </view>
                            <view class="text-red text-df">退货数量:{{item.nums}}</view>
                        </view>
                    </view>

                </view>
            </view>

            <!--商品状态-->
            <view class="bg-white coreshop-card-box">
                <view class="coreshop-card-view coreshop-price-view">
                    <view class="text-lg text-bold text-black">状态</view>
                    <view class="solid-line"></view>
                    <view class="text-black title-view">
                        <view class="title">商品状态</view>
                        <view class="text-right">
                            <text>{{typeName}}</text>
                        </view>
                    </view>
                    <view class="text-black title-view">
                        <view class="title">退款金额</view>
                        <view class="text-right">
                            <text>{{refund}}元</text>
                        </view>
                    </view>
                    <view class="solid-line"></view>
                    <view class="text-center text-black">联系客服</view>
                </view>
            </view>
            <!--图片凭证-->
            <view class="bg-white coreshop-card-box" v-if="images.length > 0">
                <view class="coreshop-card-view coreshop-order-view">
                    <view class="text-lg text-bold text-black">图片凭证</view>
                    <view class="solid-line"></view>
                    <view class="text-black title-view">
                        <view class="goods-img-item" v-for="(item, key) in images" :key="key">
                            <image :src="item.imageUrl" mode="aspectFit" @click="clickImg(item.imageUrl)"></image>
                        </view>
                    </view>
                </view>
            </view>

            <!--订单信息-->
            <view class="bg-white coreshop-card-box">
                <view class="coreshop-card-view coreshop-order-view">
                    <view class="text-lg text-bold text-black">问题描述</view>
                    <view class="solid-line"></view>
                    <view class="text-black title-view">
                        <text v-if="reason">{{reason}}</text>
                        <text v-else>暂无描述</text>
                    </view>
                </view>
            </view>

            <!--请填写回邮商品物流信息-->
            <view class="bg-white coreshop-card-box" v-if="status == 2 && reshipStatus == 1">
                <view class="coreshop-card-view coreshop-order-view">
                    <view class="text-lg text-bold text-black">请填写回邮商品物流信息</view>
                    <view class="solid-line"></view>

                    <view class='cell-group margin-cell-group'>
                        <view class='cell-item'>
                            <view class='cell-item-hd'>
                                <view class='cell-hd-title fsz26'>快递公司</view>
                            </view>
                            <view class='cell-item-bd'>
                                <input class='cell-bd-input fsz26' type="text" v-model="logiCode" placeholder="请填写快递公司名称" />
                            </view>
                        </view>
                        <view class='cell-item'>
                            <view class='cell-item-hd'>
                                <view class='cell-hd-title fsz26'>物流单号</view>
                            </view>
                            <view class='cell-item-bd'>
                                <input class='cell-bd-input fsz26' type="text" v-model="logiNo" placeholder="请填写物流单号" />
                            </view>
                        </view>
                    </view>

                </view>
            </view>

            <!--快递公司-->
            <view class="bg-white coreshop-card-box" v-if="status == 2 && reshipStatus > 1">
                <view class="coreshop-card-view coreshop-order-view">
                    <view class="text-lg text-bold text-black">快递公司</view>
                    <view class="solid-line"></view>
                    <view class="text-black title-view">
                        <view class="title">快递公司</view>
                        <view class="text-right">
                            <text>{{logiCode}}</text>
                        </view>
                    </view>
                    <view class="text-black title-view">
                        <view class="title">物流单号</view>
                        <view class="text-right">
                            <text class="margin-right-xs">{{logiNo}}</text>
                            <button class="cu-btn sm line-black">复制</button>
                        </view>
                    </view>
                </view>
            </view>


        </view>

        <!--底部-->
        <view class="foot-hight-view" />


        <view class="coreshop-bottomBox" v-show="status == 2 && reshipStatus == 1">
            <button class="coreshop-btn coreshop-btn-b" @click="submitBtn" :disabled='submitStatus' :loading='submitStatus'>提交</button>
        </view>
        <view class="coreshop-bottomBox" v-show="(orderStatus == 1 && status == 2 && refundStatus != 1 && refundStatus != 0) || (orderStatus == 1 && status == 2 && reshipStatus == 3)">
            <button class="coreshop-btn coreshop-btn-b" @click="repeat">再次申请售后</button>
        </view>
    </view>

</template>

<script>
    export default {
        data() {
            return {
                typeName: '',     //售后类型显示
                refund: 0,         //退款金额
                images: [],        //图片
                reason: '暂无',       //问题描述
                ttype: 1,          //售后类型
                status: 1,         //售后单状态
                statusName: '审核中',   //售后单状态文字描述
                reshipStatus: 0,        //退货单状态
                reshipName: '',
                refundStatus: 0,        //退款单状态
                refundName: '',
                reshipInfo: [],         //退货单明细,如果售后单未审核呢，那么显示的是售后单明细，如果售后单审核通过了，显示退款单明细
                items: [],             //退货明细
                mark: "暂无",            //拒绝原因
                logiNo: '',            //回填物流信息
                logiCode: '',          //物流公司
                reshipId: '',
                mode: 'aspectFill',
                orderId: '', //订单号
                orderStatus: '', //订单状态
                submitStatus: false,
                goodsInfo: []
            }
        },
        methods: {
            nextTap() {
                this.basics = this.basics == this.basicsList.length - 1 ? 0 : this.basics + 1;
            },
            //提交按钮
            submitBtn() {
                this.submitStatus = true;
                if (this.logino == '') {
                    this.$u.toast('请输入退货快递信息');
                    this.submitStatus = false;
                    return false;
                }
                let data = {
                    logiNo: this.logiNo,
                    logiCode: this.logiCode,
                    reshipId: this.reshipId,
                };
                this.$u.api.sendShip(data).then(res => {
                    if (res.status) {
                        this.$refs.uToast.show({
                            title: '提交成功', type: 'success', callback: function () {
                                this.submitStatus = false;
                                uni.navigateBack({
                                    delta: 1
                                });
                            }
                        })

                    } else {
                        this.$u.toast(res.msg);
                        this.submitStatus = false;
                    }
                });
            },
            repeat() {
                this.$u.route('/pages/member/afterSales/index?orderId=' + this.orderId);
            },
            // 图片点击放大
            clickImg(img) {
                // 预览图片
                uni.previewImage({
                    urls: img.split()
                });
            }
        },
        //页面加载
        onLoad(options) {
            console.log(options);
            let data = {
                id: options.aftersalesId
            }
            this.$u.api.afterSalesInfo(data).then(res => {
                if (res.status) {
                    let info = res.data.info;
                    if (info.type == 1) {
                        this.ttype = 1;
                        this.typeName = '未收货';
                    } else {
                        this.ttype = 2;
                        this.typeName = '已收货';
                    }
                    this.goodsInfo = info.items;
                    this.refund = info.refundAmount;
                    this.images = info.images;
                    this.reason = info.reason;
                    this.reshipInfo = res.data.reship;
                    this.orderId = info.orderId;
                    this.orderStatus = info.order.status;
                    if (info.mark) {
                        this.mark = info.mark;
                    }
                    if (info.status == 1) {
                        this.status = 1;
                        this.statusName = '审核中';
                    } else if (info.status == 2) {
                        this.status = 2;
                        this.statusName = '申请通过';
                        //退款单状态
                        if (info.billRefund) {
                            if (info.billRefund.status == 1) {
                                this.refundStatus = 1;
                                this.refundName = '未退款';
                            } else if (info.billRefund.status == 2) {
                                this.refundStatus = 2;
                                this.refundName = '退款成功';
                            }
                        }

                        //退货单状态
                        if (info.billReship) {
                            this.reshipId = info.billReship.reshipId
                            if (info.billReship.status == 1) {
                                this.reshipStatus = 1;
                                this.reshipName = '待发退货';
                            } else if (info.billReship.status == 2) {
                                this.reshipStatus = 2;
                                this.reshipName = '待收退货';
                                this.logiNo = info.billReship.logiNo;
                                this.logiCode = info.billReship.logiCode;
                            } else {
                                this.reshipStatus = 3;
                                this.reshipName = '已收退货';
                                this.logiNo = info.billReship.logiNo;
                                this.logiCode = info.billReship.logiCode;
                            }
                        }
                    } else {
                        this.status = 3;
                        this.statusName = '申请驳回';
                    }
                } else {
                    this.$u.toast(res.msg);
                }
            });
        }
    }
</script>

<style lang="scss" scoped>
    .goods-img-item { width: 174rpx; height: 174rpx; padding: 14rpx; position: relative; }
        .goods-img-item:nth-child(4n) { margin-right: 0; }
        .goods-img-item image { width: 100%; height: 100%; }
    @import "../../../static/style/orderDetails.scss";
</style>
