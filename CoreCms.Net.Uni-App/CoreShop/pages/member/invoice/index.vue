<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="我的发票"></u-navbar>
        <view v-if="listData.length > 0">
            <view class="invoiceBox" v-for="(item, index) in listData" :key="index">
                <view class="invoiceLeft">
                    <image src="/static/images/common/invoice.png" class="leftIco"></image>
                </view>
                <view class="invoiceRight">
                    <view class="invoiceAmount u-flex u-row-between"><text class="coreshop-text-price coreshop-text-red">{{item.amount}}</text> <text :class="item.status == 1?'status_no':'status_yes'">{{item.statusName || ''}}</text></view>
                    <view class="invoiceTitle">发票抬头：{{item.title || ''}}</view>
                    <view class="invoiceTaxNumber" v-if="item.taxNumber">发票税号：{{item.taxNumber}}</view>
                    <view class="invoiceTitle">开票备注：{{item.remarks || ''}}</view>

                    <view class="invoiceTime  u-flex u-row-between">{{item.createTime}} <text class="coreshop-text-grey">{{item.typeName || ''}}</text></view>
                </view>
            </view>
            <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
        </view>
        <!-- 无数据时默认显示 -->
        <view class="coreshop-emptybox" v-else>
            <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/coupon.png'" icon-size="300" text="暂无发票明细" mode="list"></u-empty>
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
    @import "index.scss";
</style>