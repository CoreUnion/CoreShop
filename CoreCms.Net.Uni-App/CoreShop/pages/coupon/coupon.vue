<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="优惠券"></u-navbar>
        <view class="coreshop-coupon">
            <view v-if="list.length">
                <view v-for="(item, key) in list" :key="key">
                    <view class="coreshop-coupon-card-view" :class="item.maxRecevieNums > 0 && item.getNumber >= item.maxRecevieNums ?'coreshop-lower-shelf':''">
                        <view class="img-lower-box" v-if="item.maxRecevieNums > 0 && item.getNumber >= item.maxRecevieNums ">已领完</view>
                        <view class="card-price-view">
                            <view class="coreshop-text-red price-left-view">
                                <image class="icon" src="/static/images/coupon/coupon-element.png" mode=""></image>
                            </view>
                            <view class="name-content-view">
                                <view class="u-line-1 coreshop-text-red"> {{item.name}}</view>
                                <view class="u-font-xs">
                                    优惠方式：<text v-for="(itemResult, index) in item.results" :key="index">{{itemResult}}</text>
                                </view>
                                <view class="u-font-xs">领取时间：{{$u.timeFormat(item.startTime, 'yyyy-mm-dd')}} - {{$u.timeFormat(item.endTime, 'yyyy-mm-dd')}}</view>
                            </view>
                            <view class="btn-right-view">
                                <u-button type="success" shape="circle" size="mini" @click="receiveCoupon(item.id)">立即领取</u-button>
                            </view>
                        </view>
                        <view class="card-num-view">
                            <view class="u-font-xs">
                                <text v-for="(itemCondition, index) in item.conditions" :key="index">【{{itemCondition}}】</text>
                            </view>
                            <u-icon name="arrow-down-fill" class="btnUnfold" size="28"></u-icon>
                        </view>
                    </view>
                </view>
                <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
            </view>
            <view class="services-none" v-else>
                <view class="page-box">
                    <view>
                        <view class="coreshop-emptybox">
                            <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/coupon.png'" icon-size="300" mode="order" text="暂无优惠券可领取"></u-empty>
                            <navigator class="coreshop-btn" url="/pages/category/index/index"  open-type="switchTab">随便逛逛</navigator>
                        </view>
                    </view>
                </view>
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
            this.getCouponlist()
        },
        onShow() {
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getCouponlist()
            }
        },
        methods: {
            getCouponlist() {
                let _this = this;
                let data = {
                    page: this.page,
                    limit: this.limit
                }
                this.status = 'loading'
                this.$u.api.couponList(data).then(res => {
                    if (res.status) {
                        if (res.data) {
                            let _list = res.data
                            this.list = [...this.list, ..._list]
                        }
                        if (res.code >= _this.list.length) {
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
            // 用户领取优惠券
            receiveCoupon(couponId) {
                let _this = this;
                let coreshopdata = {
                    id: couponId
                }
                this.$u.api.getCoupon(coreshopdata).then(res => {
                    if (res.status) {
                        _this.$refs.uToast.show({ title: res.msg, type: 'success' })
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            }
        }
    };
</script>

<style lang="scss">
    @import 'coupon.scss';
</style>
