<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="售后详情"></u-navbar>
        <view class="u-padding-bottom-20 coreshop-bg-white">

            <!--状态图标-->
            <view class="coreshop-bg-white u-padding-20 coreshop-solid-top u-text-center coreshop-status-img-view" v-if="status==2 && refundStatus==2">
                <view class="are-img-view">
                    <image class="are-img" src="/static/images/common/are.png" mode="widthFix" />
                </view>
                <view class="u-font-sm coreshop-text-black">退款成功</view>
            </view>


            <!--状态图标-->
            <view class="coreshop-bg-white  u-padding-20 coreshop-solid-top u-text-center coreshop-status-img-view" v-if="status==3">
                <view class="are-img-view">
                    <image class="are-img" src="/static/images/common/arg.png" mode="widthFix" />
                </view>
                <view class="u-font-sm coreshop-text-black">申请驳回</view>
                <view class="u-font-sm coreshop-text-black">驳回原因：{{mark}}</view>
            </view>

            <!--退款单状态-->
            <view class="coreshop-bg-white coreshop-card-box">
                <view class="coreshop-card-view coreshop-price-view">
                    <view class="u-font-lg coreshop-text-bold coreshop-text-black">售后单进度</view>
                    <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                    <view class="u-text-center coreshop-text-black">
                        <u-button type="warning" shape="square" size="mini" v-if="statusName" class="u-margin-left-20 u-margin-right-20">{{statusName}}</u-button>
                        <u-button type="warning" shape="square" size="mini" v-if="reshipName" class="u-margin-left-20 u-margin-right-20">{{reshipName}}</u-button>
                        <u-button type="warning" shape="square" size="mini" v-if="refundName" class="u-margin-left-20 u-margin-right-20">{{refundName}}</u-button>
                    </view>
                </view>
            </view>

            <!--物流信息-->
            <view class="coreshop-bg-white coreshop-card-box" v-if="status == 2 && reshipStatus == 1 && refundStatus!=2">
                <view class="coreshop-card-view coreshop-address-view">
                    <view class="u-font-lg coreshop-text-bold coreshop-text-black">退货邮寄信息</view>
                    <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                    <view class="wrap">
                        <u-row gutter="16">
                            <u-col span="1">
                                <u-icon name="map-fill" size="38"></u-icon>
                            </u-col>
                            <u-col span="11">
                                <view class="coreshop-text-black">
                                    <text>收货人：</text>
                                    <text>{{ reshipInfo.reshipName }}</text>
                                    <text class="u-margin-left-10">{{ reshipInfo.reshipMobile }}</text>
                                </view>
                                <view class="coreshop-text-gray u-font-sm flex">
                                    <view class="u-line-1">{{ reshipInfo.reshipArea + reshipInfo.reshipAddress }}</view>
                                </view>
                            </u-col>
                        </u-row>
                    </view>

                </view>
            </view>

            <!--商品信息-->
            <view class="coreshop-bg-white coreshop-card-box" v-if="goodsInfo.length>0">
                <view class="coreshop-card-view coreshop-shop-view">
                    <view class="coreshop-text-black coreshop-text-bold u-font-lg title-view">商品信息</view>
                    <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                    <view class="goods-list-view" v-for="(item, key) in goodsInfo" :key="key">
                        <image class="coreshop-avatar radius" :src="item.imageUrl" mode="aspectFill"></image>
                        <view class="goods-info-view">
                            <view class="coreshop-text-black u-line-2 name">{{item.name}} </view>
                            <view class="coreshop-text-gray u-font-sm u-line-1 introduce u-margin-top-15">售后单号：{{item.aftersalesId}}</view>
                            <view class="u-line-1  u-margin-top-15" v-if="item.addon">
                                <u-tag :text="item.addon" type="success" shape="circle" />
                            </view>
                            <view class="coreshop-text-red u-font-md u-margin-top-15">退货数量:{{item.nums}}</view>
                        </view>
                    </view>

                </view>
            </view>

            <!--商品状态-->
            <view class="coreshop-bg-white coreshop-card-box">
                <view class="coreshop-card-view coreshop-price-view">
                    <view class="u-font-lg coreshop-text-bold coreshop-text-black">状态</view>
                    <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                    <view class="coreshop-text-black title-view">
                        <view class="title">商品状态</view>
                        <view class="u-text-right">
                            <text>{{typeName}}</text>
                        </view>
                    </view>
                    <view class="coreshop-text-black title-view">
                        <view class="title">退款金额</view>
                        <view class="u-text-right">
                            <text>{{refund}}元</text>
                        </view>
                    </view>
                    <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                    <view class="coreshop-text-black u-text-center">
                        <u-button type="primary" size="mini" open-type="contact" bindcontact="showChat">联系客服</u-button>
                    </view>
                </view>
            </view>
            <!--图片凭证-->
            <view class="coreshop-bg-white coreshop-card-box" v-if="images.length > 0">
                <view class="coreshop-card-view coreshop-order-view">
                    <view class="u-font-lg coreshop-text-bold coreshop-text-black">图片凭证</view>
                    <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                    <view class="coreshop-text-black title-view">
                        <view class="goods-img-item" v-for="(item, key) in images" :key="key">
                            <image :src="item.imageUrl" mode="aspectFit" @click="clickImg(item.imageUrl)"></image>
                        </view>
                    </view>
                </view>
            </view>

            <!--订单信息-->
            <view class="coreshop-bg-white coreshop-card-box">
                <view class="coreshop-card-view coreshop-order-view">
                    <view class="u-font-lg coreshop-text-bold coreshop-text-black">问题描述</view>
                    <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                    <view class="coreshop-text-black title-view">
                        <text v-if="reason">{{reason}}</text>
                        <text v-else>暂无描述</text>
                    </view>
                </view>
            </view>

            <!--请填写回邮商品物流信息-->
            <view class="coreshop-bg-white coreshop-card-box" v-if="status == 2 && reshipStatus == 1">
                <view class="coreshop-card-view coreshop-order-view">
                    <view class="u-font-lg coreshop-text-bold coreshop-text-black">请填写回邮商品物流信息</view>
                    <u-line color="#eee" border-style="dashed" margin="20rpx 0" />

                    <view class='coreshop-cell-group u-margin-top-20 u-margin-bottom-20'>
                        <view class='coreshop-cell-item'>
                            <view class='coreshop-cell-item-hd'>
                                <view class='coreshop-cell-hd-title fsz26'>快递公司</view>
                            </view>
                            <view class='coreshop-cell-item-bd'>
                                <input class='coreshop-cell-bd-input fsz26' type="text" v-model="logiCode" placeholder="请填写快递公司名称" />
                            </view>
                        </view>
                        <view class='coreshop-cell-item'>
                            <view class='coreshop-cell-item-hd'>
                                <view class='coreshop-cell-hd-title fsz26'>物流单号</view>
                            </view>
                            <view class='coreshop-cell-item-bd'>
                                <input class='coreshop-cell-bd-input fsz26' type="text" v-model="logiNo" placeholder="请填写物流单号" />
                            </view>
                        </view>
                    </view>

                </view>
            </view>

            <!--快递公司-->
            <view class="coreshop-bg-white coreshop-card-box" v-if="status == 2 && reshipStatus > 1">
                <view class="coreshop-card-view coreshop-order-view">
                    <view class="u-font-lg coreshop-text-bold coreshop-text-black">快递公司</view>
                    <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                    <view class="coreshop-text-black title-view">
                        <view class="title">快递公司</view>
                        <view class="u-text-right">
                            <text>{{logiCode}}</text>
                        </view>
                    </view>
                    <view class="coreshop-text-black title-view">
                        <view class="title">物流单号</view>
                        <view class="u-text-right">
                            <text class="u-margin-right-20">{{logiNo}}</text>
                            <u-tag text="复制" type="success" mode="dark" @click="doCopyData(logiNo)" />
                        </view>
                    </view>
                </view>
            </view>

        </view>

        <!--底部-->
        <view class="coreshop-foot-hight-view" />

        <view class="coreshop-bottomBox" v-show="status == 2 && reshipStatus == 1">
            <button class="coreshop-btn coreshop-btn-b" @click="submitBtn" :disabled='submitStatus' :loading='submitStatus'>提交</button>
        </view>
        <view class="coreshop-bottomBox" v-show="(orderStatus == 1 && status == 2 && refundStatus != 1 && refundStatus != 0) || (orderStatus == 1 && status == 2 && reshipStatus == 3)">
            <button class="coreshop-btn coreshop-btn-b" @click="repeat">再次申请售后</button>
        </view>
    </view>

</template>

<script>
    import { orders, goods, tools } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [orders, goods, tools],
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
                this.$u.route('/pages/member/afterSales/submit/submit?orderId=' + this.orderId);
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
    @import "detail.scss";
</style>
