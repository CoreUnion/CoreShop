<template>
    <view>
        <view class="u-margin-left-20 u-margin-right-20 u-margin-top-20 u-margin-bottom-20">
            <u-section font-size="30" :title="coreshopdata.parameters.title" @click="goServicesList()" sub-title="查看更多"></u-section>
        </view>
        <view class="u-margin-left-15  u-margin-right-15" v-if="coreshopdata.parameters.list && count">
            <view class="img-list-item" v-if="item.id !== 'undefined' && item.id" v-for="(item, key) in coreshopdata.parameters.list" :key="key" @click="goServicesDetail(item.id)">
                <view class="img-list-item-l">
                    <u-lazy-load threshold="-150" border-radius="10" :image="item.thumbnail" :index="item.id"></u-lazy-load>
                </view>
                <view class="img-list-item-r">
                    <view class="u-font-28 coreshop-text-bold u-line-1">{{item.title}}</view>
                    <view class="description u-line-2 u-margin-top-10 u-margin-bottom-10">{{item.description}}</view>
                    <view class="item-c">
                        <view class="red-price">￥{{item.money}}</view>
                        <view>
                            <view style="float:left;">
                                <view class="goods-salesvolume red-price u-font-24" v-if="item.status == 0 && item.lastTime > 0">
                                    剩余：
                                    <u-count-down :timestamp="item.lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="24" separator-size="24" @end="end(key)"></u-count-down>
                                </view>
                                <view class="goods-salesvolume red-price" v-if="item.status == 1">已结束</view>
                                <view class="goods-salesvolume red-price" v-if="item.status == 2">售罄</view>
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
    import { services } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [services],
        name: "coreshopservice",
        props: {
            coreshopdata: {
                // type: Array,
                required: false,
            }
        },
        computed: {
            count() {
                return (this.coreshopdata.parameters.list.length > 0)
            }
        },
        methods: {
        },
    }
</script>

<style lang="scss" scoped>
    .img-list-item { border-radius: 8px; margin: 5rpx 5rpx 20rpx 5rpx; background-color: #ffffff; padding: 10rpx; position: relative; overflow: hidden;
        .img-list-item-l { width: 200rpx; height: 200rpx; display: inline-block; float: left; }
        .img-list-item-r { width: calc(100% - 240rpx); display: inline-block; margin-left: 15rpx; float: left; position: relative;
            .item-c { width: 100%; margin-top: 0;
                .description { font-size: 24rpx; color: #929292; }
                .red-price { color: #FF7159 !important; }
                .btnCart { float: right; }
            }
        }
    }
</style>
