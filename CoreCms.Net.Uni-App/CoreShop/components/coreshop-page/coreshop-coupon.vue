<template>
    <view class="coreshop-coupon u-padding-left-15  u-padding-right-15" v-if="count">
        <u-toast ref="uToast" />
        <view class="coreshop-coupon-card-view u-margin-bottom-10" :class="item.maxRecevieNums > 0 && item.getNumber >= item.maxRecevieNums ?'coreshop-lower-shelf':''" v-for="(item, key) in coreshopdata.parameters.list" :key="key">
            <view class="img-lower-box" v-if="item.maxRecevieNums > 0 && item.getNumber >= item.maxRecevieNums ">已领完</view>
            <view class="card-price-view">
                <view class="price-left-view">
                    <image class="icon" src="/static/images/coupon/coupon-element.png" mode=""></image>
                </view>
                <view class="name-content-view">
                    <view class="u-line-1 coreshop-text-red"> {{item.name}}</view>
                    <view class="u-font-xs">
                        优惠方式：<text v-for="(itemResult, index) in item.results" :key="index">{{itemResult}}</text>
                    </view>
                    <view class="u-font-xs">领取时间：{{$u.timeFormat(item.startTime, 'yyyy-mm-dd')}} 至 {{$u.timeFormat(item.endTime, 'yyyy-mm-dd')}}</view>
                </view>
                <view class="btn-right-view">
                    <u-button type="success" shape="circle" size="mini" @click="receiveCoupon(item.id)">立即领取</u-button>
                </view>
            </view>
            <view class="card-num-view">
                <view class="u-font-xs conditions">
                    <text v-for="(itemCondition, index) in item.conditions" :key="index">【{{itemCondition}}】</text>
                </view>
                <u-icon name="arrow-down-fill" class="btnUnfold" size="28"></u-icon>
            </view>
        </view>
    </view>
</template>

<script>
    export default {
        name: "coreshopcoupon",
        components: {
        },
        props: {
            coreshopdata: {
                // type: Array,
                required: true,
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
                let coreshopdata = {
                    id: couponId
                }
                this.$u.api.getCoupon(coreshopdata).then(res => {
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
</style>
