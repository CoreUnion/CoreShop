<template>
    <!-- 单图 -->
    <view class="coreshop-adpop" v-if="coreshopdata.parameters.list && coreshopdata.parameters.list.length > 0">
        <u-toast ref="uToast" />
        <view class="adpop" v-if="closeAd">
            <view class="adpop-center">
                <view class="adpop-img">
                    <!-- #ifdef MP-WEIXIN -->
                    <view @click="showSliderInfo2" :data-type="coreshopdata.parameters.list[0].linkType" :data-val="coreshopdata.parameters.list[0].linkValue">
                        <image class="ad-img" :src="coreshopdata.parameters.list[0].image" mode="widthFix"></image>
                    </view>
                    <!-- #endif -->
                    <!-- #ifndef MP-WEIXIN -->
                    <image class="ad-img" :src="coreshopdata.parameters.list[0].image" mode="widthFix" @click="showSliderInfo(coreshopdata.parameters.list[0].linkType, coreshopdata.parameters.list[0].linkValue)"></image>
                    <!-- #endif -->
                </view>
                <image class="close-btn" src="/static/images/close-pink.png" mode="" @click="closePop"></image>
            </view>
        </view>
    </view>
</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [goods],
        name: "coreshopadpop",
        props: {
            coreshopdata: {
                // type: Object,
                required: true,
            }
        },
        data() {
            return {
                closeAd: true
            }
        },
        computed: {
            count() {
                // console.log(coreshopdata)
                return (this.coreshopdata.parameters.list.length > 0)
            }

        },
        methods: {
            closePop() {
                this.closeAd = false
            },
            showSliderInfo(type, val) {
                console.log(type);
                if (!val) {
                    return;
                }
                if (type == 1) {
                    if (val.indexOf('http') != -1) {
                        // #ifdef H5
                        window.location.href = val
                        // #endif
                    } else {
                        // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE || MP
                        if (val == '/pages/index/default/default' || val == '/pages/category/index/index' || val == '/pages/index/cart/cart' || val == '/pages/index/member/member') {
                            this.$u.route({ type: 'switchTab', url: val });
                            return;
                        } else if (val.indexOf('/pages/coupon/coupon') > -1) {
                            var id = val.replace('/pages/coupon/coupon?id=', "");
                            this.receiveCoupon(id)
                        } else {
                            this.$u.route(val);
                            return;
                        }
                        // #endif
                    }
                } else if (type == 2) {
                    // 商品详情
                    this.goGoodsDetail(val)
                } else if (type == 3) {
                    // 文章详情
                    this.$u.route('/pages/article/details/details?id=' + val + '&idType=1')
                } else if (type == 4) {
                    // 文章列表
                    this.$u.route('/pages/article/list/list?cid=' + val)
                } else if (type == 5) {
                    //智能表单
                    this.$u.route('/pages/form/details/details?id=' + val)
                }
            },
            // 用户领取优惠券
            receiveCoupon(couponId) {
                let data = {
                    promotion_id: couponId
                }
                this.$u.api.getCoupon(data).then(res => {
                    if (res.status) {
                        this.$refs.uToast.show({ title: res.msg, type: 'success', back: false })
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
            // #ifdef MP-WEIXIN
            showSliderInfo2: function (e) {
                let type = e.currentTarget.dataset.type;
                let val = e.currentTarget.dataset.val;
                console.log(type);
                console.log(val)
                console.log(type);
                if (!val) {
                    return;
                }
                if (type == 1) {
                    if (val.indexOf('http') != -1) {
                        // #ifdef H5
                        window.location.href = val
                        // #endif
                    } else {
                        // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE || MP
                        if (val == '/pages/index/default/default' || val == '/pages/category/index/index' || val == '/pages/index/cart/cart' || val == '/pages/index/member/member') {
                            this.$u.route({ type: 'switchTab', url: val });
                            return;
                        } else if (val.indexOf('/pages/coupon/coupon') > -1) {
                            var id = val.replace('/pages/coupon/coupon?id=', "");
                            this.receiveCoupon(id)
                        } else {
                            this.$u.route(val);
                            return;
                        }
                        // #endif
                    }
                } else if (type == 2) {
                    // 商品详情
                    this.goGoodsDetail(val)
                } else if (type == 3) {
                    // 文章详情
                    this.$u.route('/pages/article/details/details?id=' + val + '&idType=1')
                } else if (type == 4) {
                    // 文章列表
                    this.$u.route('/pages/article/list/list?cid=' + val)
                } else if (type == 5) {
                    //智能表单
                    this.$u.route('/pages/form/details/details?id=' + val)
                }
            }
            // #endif
        },
    }
</script>

<style scoped lang="scss">
    .coreshop-adpop {
        .adpop { position: fixed; background: rgba(0,0,0,.5); width: 100%; height: 100vh; z-index: 999; top: 0; left: 0;
            .adpop-center { position: absolute; left: 50%; top: 50%; transform: translate(-50%,-50%); width: 70%; text-align: center;
                .adpop-img { width: 100%; max-height: 1000rpx; margin-bottom: 50rpx;
                    .ad-img { width: 100%; max-height: 1000rpx; }
                }
                .close-btn { width: 80rpx; height: 80rpx; }
            }
        }
    }
</style>
