<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="服务商品"></u-navbar>
        <view class="content">
            <view v-if="list.length">
                <view class="orderList" v-for="(item, index) in list" :key="index" @click="goServicesDetail(item.id)">
                    <view class="top">
                        <view class="left">
                            <u-icon name="clock" :size="28" color="rgb(94,94,94)"></u-icon>
                            <!--<view class="store">截止购买剩余：</view>-->
                            <u-count-down :timestamp="item.timestamp" separator="zh" :show-days="true" :show-hours="true" :show-seconds="true" :show-minutes="true" font-size="28" :separator-size="28"></u-count-down>

                            <!--<button class="cu-btn sm line-black">复制</button>
                            <u-icon name="arrow-right" color="rgb(203,203,203)" :size="26"></u-icon>-->
                        </view>
                        <view class="right">
                            仅剩{{item.amount}}
                        </view>
                    </view>
                    <view class="item u-padding-20 coreshop-solid-bottom">
                        <view class="left"><image :src="item.thumbnail && item.thumbnail!='null' ?  item.thumbnail : '/static/images/common/empty-banner.png'" mode="aspectFill"></image></view>
                        <view class="content">
                            <view class="title u-line-2">{{item.title}}</view>
                            <view class="type u-line-2">{{item.description}}</view>
                            <view class="coreshop-text-price coreshop-text-red u-margin-bottom-10">{{ item.money }}</view>
                            <view class="delivery-time">最大购买限制：{{item.maxBuyNumber== 0 ? '不限' : item.maxBuyNumber}}</view>
                        </view>
                    </view>

                    <view class="coreshop-text-gray u-font-xs u-flex u-flex-nowrap u-padding-20 coreshop-solid-bottom">
                        兑换级别：
                        <view v-if="item.allowedMemberships && item.allowedMemberships.length>0">
                            <u-tag :text="item" mode="light" size="mini" class="u-margin-right-5" v-for="(item, indexChild) in item.allowedMemberships" :key="indexChild" />
                        </view>
                    </view>
                    <view class="coreshop-text-gray u-font-xs u-flex u-flex-nowrap u-padding-20 coreshop-solid-bottom">
                        兑换门店：
                        <view v-if="item.consumableStores && item.consumableStores.length>0">
                            <view class="coreshop-bg-orange light sm  u-padding-4 u-margin-right-5" v-for="(item, indexChild) in item.consumableStores" :key="indexChild">{{item}}</view>
                        </view>
                    </view>
                    <view class="u-flex u-flex-nowrap u-row-between u-padding-10">
                        <view class="u-font-xs">
                            购买截止：{{ item.endTime }}
                        </view>
                        <u-button type="warning" shape="square"  size="mini" :plain="true">立刻抢购</u-button>
                    </view>
                </view>
                <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
            </view>

            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无服务列表" mode="list"></u-empty>
            </view>
        </view>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>

</template>

<script>

    

    import { services } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [services],
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
            this.getServicelist()
        },
        onShow() {
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getServicelist()
            }
        },
        methods: {
            getServicelist() {
                let _this = this;
                let data = {
                    page: this.page,
                    limit: this.limit
                }
                this.status = 'loading'
                this.$u.api.getServicelist(data).then(res => {
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
    .cell { margin-top: 0; min-height: 60rpx; line-height: 60rpx; }
    .order .top .left .store { margin: 0 10rpx; font-size: 28rpx; font-weight: normal; }
    .order .bottom { margin-top: 0rpx; }

    .order .bottom2 { display: flex; margin-top: 20rpx; padding: 0 10rpx; justify-content: space-between; align-items: center; }
        .order .bottom2 .coreshop-btn { line-height: 52rpx; width: 160rpx; border-radius: 26rpx; border: 2rpx solid $u-border-color; font-size: 26rpx; text-align: center; color: $u-type-info-dark; }
        .order .bottom2 .evaluate { color: $u-type-warning-dark; border-color: $u-type-warning-dark; }
</style>
