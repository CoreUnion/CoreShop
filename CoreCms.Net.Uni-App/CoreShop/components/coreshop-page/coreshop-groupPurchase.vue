<template>
    <!-- 团购秒杀 -->
    <view v-if="count">
        <view class="u-margin-left-20 u-margin-right-20 u-margin-top-20 u-margin-bottom-20">
            <u-section font-size="30" :title="coreshopdata.parameters.title" @click="goSeckillList()"></u-section>
        </view>
        <view class="u-margin-left-15  u-margin-right-15" v-if="coreshopdata.parameters.list && count">
            <view class="img-list-item" v-if="item.id !== 'undefined' && item.id" v-for="(item, key) in coreshopdata.parameters.list" :key="key" @click="goSeckillDetail(item.goods.id, item.goods.groupId)">
                <view class="img-list-item-l">
                    <u-lazy-load threshold="-150" border-radius="10" :image="item.goods.image" :index="item.id"></u-lazy-load>
                </view>
                <view class="img-list-item-r">
                    <view class="u-font-28  u-line-1">{{item.name}}</view>
                    <view class="description u-line-2 u-margin-10">{{item.goods.name}}</view>
                    <view class="item-c">
                        <view class="red-price">￥{{item.goods.product.price}} <span class="u-font-xs  coreshop-text-through u-margin-left-15">{{item.goods.product.mktprice}}元</span></view>
                        <view>
                            <view style="float:left;">
                                <view class="goods-salesvolume red-price u-font-24" v-if="(item.startStatus == 1) && item.lastTime">
                                    剩余：
                                    <u-count-down :timestamp="item.lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="24" separator-size="24" @end="end(key)"></u-count-down>
                                </view>
                                <view class="goods-salesvolume red-price" v-if="item.startStatus == 3">已结束</view>
                                <view class="goods-salesvolume red-price" v-if="item.startStatus == 2">即将开始</view>
                            </view>
                            <u-icon name="shopping-cart" color="#2979ff" size="40" class="btnCart"></u-icon>
                        </view>
                    </view>
                </view>
            </view>
            <view class="img-list-item" v-if="!item.id" v-for="(item, key) in coreshopdata.parameters.list" :key="key">
                <image class="img-list-item-l" :src="item.image== '/images/empty-banner.png'? '/static/images/common/empty-banner.png':item.image" mode='aspectFill'></image>
                <view class="img-list-item-r">
                    <view class="goods-name list-goods-name">{{item.name}}</view>
                    <view class="item-c">
                        <view class=" red-price">{{item.price}}</view>
                    </view>
                </view>
            </view>
        </view>

    </view>
</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [goods],
        name: "coreshopgrouppurchase",
        props: {
            coreshopdata: {
                // type: Array,
                required: false,
            }
        },
        computed: {
            count() {
                if (!this.coreshopdata) {
                    return false;
                }
                if (!this.coreshopdata.parameters) {
                    return false;
                }
                if (!this.coreshopdata.parameters.list) {
                    return false;
                }
                return (this.coreshopdata.parameters.list.length > 0)
            }
        },
        methods: {
            end(index) {
                let _that = this;
                _that.list.splice(index, 1)
            }
        },
    }
</script>

<style lang="scss" scoped>
    .img-list-item { border-radius: 8px; margin: 5rpx 5rpx 20rpx 5rpx; background-color: #ffffff; padding: 10rpx; position: relative; overflow: hidden;
        .img-list-item-l { width: 200rpx; height: 200rpx; display: inline-block; float: left; }
        .img-list-item-r { width: calc(100% - 240rpx); display: inline-block; margin-left: 15rpx; float: left; position: relative;
            .description { font-size: 24rpx; color: #929292; }
            .item-c { width: 100%; margin-top: 0;
                .red-price { color: #FF7159 !important; }
                .btnCart { float: right; }
            }
        }
    }
</style>
