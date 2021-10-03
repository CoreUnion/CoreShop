<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="服务核销码"></u-navbar>
        <view class="coreshop-list menu-avatar">
            <view class="coreshop-bg-white u-padding-20 coreshop-solid-bottom">
                <view class="u-line-1">{{ info.title }}</view>
            </view>
            <view class="coreshop-list-item">
                <view class="coreshop-avatar radius lg">
                    <image class='services-img' :src="info.thumbnail" mode="widthFix"></image>
                </view>
                <view class="content">
                    <view>
                    </view>
                    <view class="coreshop-text-gray u-font-sm u-flex u-flex-nowrap">
                        <view class="u-line-3">{{ info.description }}</view>
                    </view>
                </view>
                <view class="action" @click="goServicesDetail(info.id)">
                    <u-button  size="mini">说明</u-button>
                </view>
            </view>
            <view class="coreshop-bg-white u-padding-20 coreshop-solid-bottom">
                <view class="coreshop-text-gray u-font-xs u-flex u-flex-nowrap u-padding-10">
                    订单编号：
                </view>
                <view class="coreshop-text-gray u-font-xs u-flex u-flex-nowrap u-padding-10">
                    兑换级别：
                    <view v-if="info.allowedMemberships && info.allowedMemberships.length>0">
                        <u-tag :text="item" mode="light" size="mini" class="u-margin-right-5" v-for="(item, index) in info.allowedMemberships" :key="index"/>
                    </view>
                </view>
                <view class="coreshop-text-gray u-font-xs u-flex u-flex-nowrap u-padding-10">
                    兑换门店：
                    <view v-if="info.consumableStores && info.consumableStores.length>0">
                            <view class="coreshop-bg-orange light sm  u-padding-4 u-margin-right-5" v-for="(item, index) in info.consumableStores" :key="index">{{item}}</view>
                    </view>
                </view>
            </view>
        </view>

        <view class="taobao">
            <view class="ticket" :class="item.status==3?'grayscale':''" v-if="list.length" v-for="(item, index) in list" :key="index" @click="showQrcodeBox(index)">
                <view class="left">
                    <view class="introduce">
                        <view class="top">
                            核销码：
                            <text class="big">{{item.redeemCode}}</text>
                        </view>
                        <view class="type">{{item.validityType==1?'长期有效':'限时间段内消费'}}</view>
                        <view class="date u-line-1" v-if="item.validityStartTime && item.validityEndTime">{{item.validityStartTime}} 至 {{item.validityEndTime}}</view>
                    </view>
                </view>
                <view class="right">
                    <view class="use immediate-use" :round="true">{{item.statusStr}}</view>
                </view>
            </view>
            <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
        </view>
        <u-popup v-model="show" mode="center" width="180px" height="180px" closeable="true">
            <canvas canvas-id="qrcode" style="width: 180px; height: 180px; " />
        </u-popup>
    </view>
</template>

<script>
    import uQRCode from '@/common/utils/uqrcode.js'
    import { services } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [services],
        computed: {
        },
        data() {
            return {
                page: 1,
                limit: 10,
                list: [],
                serviceOrderId: '',
                info: {},
                show: false,
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                }
            };
        },
        onLoad(e) {
            this.serviceOrderId = e.id;
            this.getServicesTickets()
        },
        onShow() {
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getServicesTickets()
            }
        },
        methods: {
            getServicesTickets() {
                let _this = this;
                let data = {
                    id: this.serviceOrderId,
                    page: this.page,
                    limit: this.limit
                }
                this.status = 'loading'

                this.$u.api.getServicesTickets(data).then(res => {
                    if (res.status) {
                        this.info = res.data.model;
                        let _list = res.data.list
                        this.list = [...this.list, ..._list]

                        if (res.data.count > _this.list.length) {
                            _this.page++
                            _this.status = 'loadmore'
                        } else {
                            _this.status = 'nomore'
                        }
                    } else {
                        _this.$u.toast(res.msg)
                    }
                })
            },
            showQrcodeBox(index) {
                let _this = this;
                var item = _this.list[index];
                if (item.status == 0) {
                    _this.make(item.redeemCode);
                    _this.show = true;
                }
            },
            make(textStr) {
                uQRCode.make({
                    canvasId: 'qrcode',
                    componentInstance: this,
                    text: textStr,
                    size: 180,
                    margin: 30,
                    backgroundColor: '#ffffff',
                    foregroundColor: '#000000',
                    fileType: 'jpg',
                    correctLevel: uQRCode.errorCorrectLevel.H,
                    success: res => {
                        console.log(res)
                    }
                })
            }
        }
    };
</script>

<style lang="scss" scoped>
    @import "details.scss";
</style>
