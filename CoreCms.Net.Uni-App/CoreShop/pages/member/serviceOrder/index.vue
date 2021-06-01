<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="我购买的服务"></u-navbar>
        <view>
            <view class="cu-list menu-avatar" v-if="list.length>0">
                <view class="cu-item" v-for="(item, index) in list" :key="index" @click="goServicesUserDetail(item.serviceOrderId)">
                    <view class="cu-avatar radius lg">
                        <image class='services-img' :src="item.service.thumbnail" mode="widthFix"></image>
                    </view>
                    <view class="content">
                        <view>
                            <view class="text-cut">{{ item.service.title }}</view>
                            <view class="cu-tag round bg-orange sm">{{ item.statusStr }}</view>
                        </view>
                        <view class="text-gray text-sm flex">
                            <view class="text-cut">{{ item.service.description }}</view>
                        </view>
                        <view class="text-gray text-sm flex">
                            <view class="text-cut">购买于：{{item.payTime}}</view>
                        </view>
                    </view>
                    <view class="action">
                        <view class="cu-tag round bg-blue light">立即使用</view>
                    </view>
                </view>
                <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$apiFilesUrl+'/static/images/empty/order.png'" icon-size="300" text="暂无服务明细" mode="list"></u-empty>
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
    page { background: #f4f4f4; }
    .cu-list.menu-avatar > .cu-item .action { width: 150rpx; text-align: center; }
    .cu-list.menu-avatar > .cu-item { height: 170rpx; }
</style>