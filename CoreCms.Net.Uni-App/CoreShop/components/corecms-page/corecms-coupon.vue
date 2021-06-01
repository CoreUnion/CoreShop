<template>
    <view class="coupon u-padding-left-15  u-padding-right-15" v-if="count">
        <u-toast ref="uToast" />
        <view class="coreshop-sponsored-card-view u-margin-bottom-10"  :class="item.maxRecevieNums > 0 && item.getNumber >= item.maxRecevieNums ?'coreshop-lower-shelf':''" v-for="(item, key) in corecmsdata.parameters.list" :key="key">
            <view class="img-lower-box" v-if="item.maxRecevieNums > 0 && item.getNumber >= item.maxRecevieNums ">已领完</view>
            <view class="card-price-view">
                <view class="text-red price-left-view">
                    <image class="icon" src="/static/images/coupon/coupon-element.png" mode=""></image>
                </view>
                <view class="name-content-view">
                    <view class="text-cut text-red"> {{item.name}}</view>
                    <view class="text-xs">
                        优惠方式：<text v-for="(itemResult, index) in item.results" :key="index">{{itemResult}}</text>
                    </view>
                    <view class="text-xs">领取时间：{{$u.timeFormat(item.startTime, 'yyyy-mm-dd')}} 至 {{$u.timeFormat(item.endTime, 'yyyy-mm-dd')}}</view>
                </view>
                <view class="btn-right-view">
                    <button class="cu-btn bg-red round sm" @click="receiveCoupon(item.id)">立即领取</button>
                </view>
            </view>
            <view class="card-num-view">
                <view class="text-xs conditions">
                    <text v-for="(itemCondition, index) in item.conditions" :key="index">【{{itemCondition}}】</text>
                </view>
                <text class="cuIcon-unfold btnUnfold" />
            </view>
        </view>
    </view>
</template>

<script>
    export default {
        name: "corecmscoupon",
        components: {
        },
        props: {
            corecmsdata: {
                // type: Array,
                required: true,
            }
        },
        computed: {
            count() {
                if (!this.corecmsdata) {
                    return false;
                }
                if (!this.corecmsdata.parameters) {
                    return false;
                }
                if (!this.corecmsdata.parameters.list) {
                    return false;
                }
                return (this.corecmsdata.parameters.list.length > 0)
            }
        },
        data() {
            return {
                coupons: [
                    {
                        color: '#9F6DFA', ltBg: "#FFFFFF", height: '180rpx',
                        unit: "￥", number: 5, txt: "满50元可用", title: "全场通用券", desc: "有效期至 2018-05-20",
                        btn: "领取", drawed: "已抢2100张"
                    }
                ]
            }
        },
        created() {
        },
        methods: {
            // 用户领取优惠券
            receiveCoupon(couponId) {
                let _this = this;
                let corecmsdata = {
                    id: couponId
                }
                this.$u.api.getCoupon(corecmsdata).then(res => {
                    if (res.status) {
                        _this.$refs.uToast.show({ title: res.msg, type: 'success', back: false })
                    } else {
                        _this.$u.toast(res.msg)
                    }
                })
            },
        }
    }
</script>

<style lang="scss" scoped>

    .icon { width: 130rpx; height: 100rpx; top: 50%; transform: translateY(-50%); }
    .coreshop-sponsored-card-view { position: relative; }
        .coreshop-sponsored-card-view .card-price-view { position: relative; background: #FFF5F5; border-radius: 14.54rpx 14.54rpx 0 0; padding: 18.18rpx; }
            .coreshop-sponsored-card-view .card-price-view .price-left-view { position: absolute; height: 125.45rpx; width: 135.45rpx; text-align: center; line-height: 125.45rpx; }
                .coreshop-sponsored-card-view .card-price-view .price-left-view .price { font-size: 45.45rpx; font-weight: 400; }
            .coreshop-sponsored-card-view .card-price-view .name-content-view { position: relative; padding-left: 163.63rpx; padding-right: 145.45rpx; height: 125.45rpx; line-height: 1.8; color: #999898; }
                .coreshop-sponsored-card-view .card-price-view .name-content-view::before { content: ''; position: absolute; top: -18.18rpx; bottom: -18.18rpx; margin-left: -18.18rpx; border-left: 2rpx dashed #fdbabc; }
            .coreshop-sponsored-card-view .card-price-view .btn-right-view { position: absolute; right: 27.27rpx; top: 18.18rpx; height: 125.45rpx; line-height: 125.45rpx; }
        .coreshop-sponsored-card-view .card-num-view { position: relative; background: #FFECED; border-radius: 0 0 14.54rpx 14.54rpx; border-top: 2rpx dashed #dedbdb; padding: 10.9rpx 27.27rpx; color: #999898; }
            /*.coreshop-sponsored-card-view .card-num-view::before { content: ''; position: absolute; width: 36.36rpx; height: 36.36rpx; background: #ffffff; border-radius: 50%; top: -18.18rpx; left: -18.18rpx; }
            .coreshop-sponsored-card-view .card-num-view::after { content: ''; position: absolute; width: 36.36rpx; height: 36.36rpx; background: #ffffff; border-radius: 50%; top: -18.18rpx; right: -18.18rpx; }*/
            .coreshop-sponsored-card-view .card-num-view .conditions { position: relative; padding-right: 72.72rpx; }
            .coreshop-sponsored-card-view .card-num-view .btnUnfold { position: absolute; right: 27.27rpx; top: 14.54rpx; }

    .coreshop-lower-shelf { }
        .coreshop-lower-shelf .card-price-view { opacity: 0.5; }
        .coreshop-lower-shelf .card-num-view { opacity: 0.5; }
        .coreshop-lower-shelf .img-lower-box { position: absolute; height: 100.9rpx; width: 100.9rpx; background-color: rgba(0, 0, 0, 0.6); border-radius: 181.81rpx; text-align: center; line-height: 100.9rpx; font-size: 24rpx; color: #fff; top: 25.45rpx; left: 35rpx; -webkit-transition: left .15s; transition: left .15s; }


</style>
