<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="我购买的服务"></u-navbar>
        <view>
            <view class="page-box" v-if="list.length > 0">
                <view class="orderList" v-for="(item, index) in list" :key="index">
                    <view class="top" @click="goServicesUserDetail(item.serviceOrderId)">
                        <view class="left">
                            <view class="store">订单号 : {{item.serviceOrderId}}</view>
                        </view>
                        <view class="right">{{ item.statusStr }}</view>
                    </view>
                    <view class="item">
                        <view class="left"><image :src="item.service.thumbnail && item.service.thumbnail!='null' ?  item.service.thumbnail : '/static/images/common/empty-banner.png'" mode="aspectFill"></image></view>
                        <view class="content">
                            <view class="title u-line-2">{{item.service.title}}</view>
                            <view class="type">{{item.service.description}}</view>
                            <view class="delivery-time">下单时间：{{ $u.timeFormat(item.payTime, 'yyyy-mm-dd hh:MM:ss') }}</view>
                        </view>
                    </view>
                    <view class="bottom">
                        <view class="more">
                            <u-tag :text="item.statusStr" mode="light" />
                        </view>

                        <view class="u-flex">
                            <view class='coreshop-btn exchange' @click="goServicesUserDetail(item.serviceOrderId)">立即使用</view>
                        </view>
                    </view>
                </view>
                <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/order.png'" icon-size="300" text="暂无购买的服务" mode="list"></u-empty>
            </view>
        </view>
    </view>
</template>

<script>
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
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                }
            };
        },
        onLoad() {
            this.getUserServicesPageList()
        },
        onShow() {
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getUserServicesPageList()
            }
        },
        methods: {
            getUserServicesPageList() {
                let _this = this;
                let data = {
                    page: this.page,
                    limit: this.limit
                }
                this.status = 'loading'

                this.$u.api.getUserServicesPageList(data).then(res => {
                    if (res.status) {

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
        }
    };
</script>

<style lang="scss">
</style>