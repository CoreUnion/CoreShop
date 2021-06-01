<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="我的发票"></u-navbar>
        <view class="content">
            <view v-if="listData.length > 0">
                <view class="invoiceBox" v-for="(item, index) in listData" :key="index">
                    <view class="invoiceLeft">
                        <image src="/static/images/common/invoice.png" class="leftIco"></image>
                    </view>
                    <view class="invoiceRight">
                        <view class="invoiceAmount x-bc"><text class="text-price text-red">{{item.amount}}</text> <text :class="item.status == 1?'status_no':'status_yes'">{{item.statusName || ''}}</text></view>
                        <view class="invoiceTitle">发票抬头：{{item.title || ''}}</view>
                        <view class="invoiceTaxNumber" v-if="item.taxNumber">发票税号：{{item.taxNumber}}</view>
                        <view class="invoiceTitle">开票备注：{{item.remarks || ''}}</view>

                        <view class="invoiceTime  x-bc">{{item.createTime}} <text class="text-grey">{{item.typeName || ''}}</text></view>
                    </view>
                </view>
                <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$apiFilesUrl+'/static/images/empty/coupon.png'" icon-size="300" text="暂无发票明细" mode="list"></u-empty>
            </view>
        </view>
    </view>

</template>

<script>
    export default {
        data() {
            return {
                id: 0,
                page: 1,
                limit: 10,
                listData: [],
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                }
            }
        },
        onLoad(e) {
            if (e.id) {
                this.id = e.id;
            }
            this.getData();
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getData();
            }
        },
        methods: {
            //获取我的发票列表
            getData() {
                this.status = 'loading'
                let data = {
                    page: this.page,
                    limit: this.limit
                }
                if (this.id != 0) {
                    data.id = this.id;
                }
                this.$u.api.myInvoiceList(data).then(res => {
                    if (res.status) {
                        let newList = this.listData.concat(res.data);
                        this.listData = newList;

                        if (res.otherData.totalPages > this.listData.length) {
                            this.page++
                            this.status = 'loadmore'
                        } else {
                            this.status = 'noMore'
                        }
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            }
        }
    }
</script>

<style lang="scss" scoped>
    .invoiceBox { margin: 30rpx 20rpx; margin-bottom: 20rpx; background-color: #ffffff; padding: 30rpx; border-radius: 10rpx; box-shadow: 0 0 10rpx #eeeeee; overflow: auto; }
    .invoiceLeft { height: 90rpx; width: 90rpx; overflow: hidden; float: left; }
    .leftIco { height: 100%; width: 100%; }
    .invoiceRight { width: calc(100% - 120rpx); float: right; }
    .invoiceAmount { font-size: 32rpx; margin-bottom: 20rpx; }
    .invoiceTitle { font-size: 24rpx; color: #888888; }
    .invoiceTaxNumber { font-size: 24rpx; color: #888888; }
    .invoiceTime { border-top: 2rpx #eeeeee dashed; margin-top: 20rpx; padding-top: 20rpx; font-size: 24rpx; color: #F44336; }
    .status_no { margin-left: 20rpx; font-size: 24rpx; color: #F44336; }
    .status_yes { margin-left: 20rpx; font-size: 24rpx; color: #0d9e13; }
    .invoice-none { text-align: center; padding: 200rpx 0; }
    .invoice-none-img { width: 274rpx; height: 274rpx; }
</style>