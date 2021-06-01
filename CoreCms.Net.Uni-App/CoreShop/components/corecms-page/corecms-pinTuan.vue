<template>
    <!-- 拼团 -->
    <view v-if="count">
        <view class="u-margin-left-20 u-margin-right-20 u-margin-top-20 u-margin-bottom-20">
            <u-section font-size="30" :title="corecmsdata.parameters.title" @click="goPinTuanList()"></u-section>
        </view>
        <view class="u-margin-left-15  u-margin-right-15 bg-white u-padding-10" v-if="goodsList && count">
            <view class="goods-box swiper-box x-f">
                <swiper class="carousel" circular @change="swiperChange" :autoplay="true" duration="2000">
                    <swiper-item  v-for="(goods, index) in goodsList" :key="index"  class="carousel-item">
                        <view class="goods-list-box x-f">
                            <view class="goods-item" v-for="item in goods" :key="item.id"  @tap="goPinTuanDetail(item.goodsId)">
                                <view class="min-goods">
                                    <view class="img-box">
                                        <view class="tag">{{item.peopleNumber}}人团</view>
                                        <image class="img" :src="item.goodsImage" mode="widthFix"></image>
                                    </view>
                                    <view class="price-box">
                                        <view class="y-f">
                                            <text class="seckill-current">￥{{item.pinTuanPrice}}</text>
                                            <text class="original">￥{{item.pinTuanPrice + item.discountAmount}}</text>
                                        </view>
                                    </view>
                                    <view class="title"><slot name="titleText"></slot></view>
                                </view>
                            </view>
                        </view>
                    </swiper-item>
                </swiper>
                <view class="swiper-dots" v-if="goodsList.length > 1">
                    <text :class="swiperCurrent === index ? 'dot-active' : 'dot'" v-for="(dot, index) in goodsList.length" :key="index"></text>
                </view>
            </view>
        </view>
    </view>
</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [goods],
        name: "corecmspinTuan",
        props: {
            corecmsdata: {
                required: false,
            }
        },
        data() {
            return {
                goodsList: [],
                swiperCurrent: 0
            };
        },
        created() {
            let that = this;
            if (that.corecmsdata.parameters.list.length > 0) {
                let arr = that.sortData(that.corecmsdata.parameters.list, 4);
                that.goodsList = arr;
            }
        },
        computed: {
            count() {
                return (this.corecmsdata.parameters.list.length > 0)
            }
        },
        methods: {
            swiperChange(e) {
                this.swiperCurrent = e.detail.current;
            },
            // 数据分层
            sortData(oArr, length) {
                let arr = [];
                let minArr = [];
                oArr.forEach(c => {
                    if (minArr.length === length) {
                        minArr = [];
                    }
                    if (minArr.length === 0) {
                        arr.push(minArr);
                    }
                    minArr.push(c);
                });
                return arr;
            }
        },
    }
</script>

<style lang="scss">
    .swiper-box,
    .carousel { width: 700rpx; height: 240upx; position: relative; border-radius: 20rpx; }
        .swiper-box .carousel-item,
        .carousel .carousel-item { width: 100%; height: 100%; }
        .swiper-box .swiper-image,
        .carousel .swiper-image { width: 100%; height: 100%; }
    .swiper-dots { display: flex; position: absolute; left: 50%; transform: translateX(-50%); bottom: 0rpx; z-index: 66; }
        .swiper-dots .dot { width: 45rpx; height: 3rpx; background: #eee; border-radius: 50%; margin-right: 10rpx; }
        .swiper-dots .dot-active { width: 45rpx; height: 3rpx; background: #a8700d; border-radius: 50%; margin-right: 10rpx; }
    .group-goods { background: #fff; border-radius: 20rpx; overflow: hidden; }
        .group-goods .title-box { padding-bottom: 20rpx; }
            .group-goods .title-box .title { font-size: 32rpx; font-weight: bold; }
            .group-goods .title-box .group-people .time-box { font-size: 26rpx; color: #edbf62; }
                .group-goods .title-box .group-people .time-box .count-text-box { width: 30rpx; height: 34rpx; background: #edbf62; text-align: center; line-height: 34rpx; font-size: 24rpx; border-radius: 6rpx; color: rgba(255, 255, 255, 0.9); margin: 0 8rpx; }
            .group-goods .title-box .group-people .head-box .head-img { width: 40rpx; height: 40rpx; border-radius: 50%; background: #ccc; }
            .group-goods .title-box .group-people .tip { font-size: 28rpx; padding-left: 30rpx; color: #666; }
            .group-goods .title-box .group-people .cuIcon-right { font-size: 30rpx; line-height: 28rpx; color: #666; }
    .goods-item { margin-right: 22rpx; }
        .goods-item:nth-child(4n) { margin-right: 0; }



    .min-goods { width: 152rpx; background: #fff; }
        .min-goods .img-box { width: 152rpx; height: 152rpx; overflow: hidden; position: relative; }
            .min-goods .img-box .tag { position: absolute; left: 0; bottom: 0rpx; z-index: 2; line-height: 35rpx; background: linear-gradient(132deg, #f3dfb1, #f3dfb1, #ecbe60); border-radius: 0px 18rpx 18rpx 0px; padding: 0 10rpx; font-size: 24rpx; font-family: PingFang SC; font-weight: bold; color: #784f06; }
            .min-goods .img-box .img { width: 100%; background-color: #ccc; }
        .min-goods .price-box { width: 100%; margin-top: 10rpx; }
            .min-goods .price-box .seckill-current { font-size: 30rpx; font-weight: 500; color: #e1212b; }
            .min-goods .price-box .original { font-size: 20rpx; font-weight: 400; text-decoration: line-through; color: #999999; margin-left: 14rpx; }
        .min-goods .title { font-size: 26rpx; }
</style>
