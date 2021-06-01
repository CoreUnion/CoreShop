<template>
    <!-- 单图 -->
    <view class="ad corecms-imgsingle" v-if="corecmsdata.parameters.list && corecmsdata.parameters.list.length > 0">
        <view class="" v-for="item in corecmsdata.parameters.list" :key="item.id">
            <!-- #ifdef MP-WEIXIN -->
            <view @click="showSliderInfo2" :data-type="item.linkType" :data-val="item.linkValue">
                <image class="ad-img" :src="item.image" mode="widthFix"></image>
            </view>
            <!-- #endif -->
            <!-- #ifndef MP-WEIXIN -->
            <image class="ad-img" :src="item.image" mode="widthFix" @click="showSliderInfo(item.linkType, item.linkValue)"></image>
            <!-- #endif -->
            <view class="imgup-btn" v-if="item.buttonText != ''" @click="showSliderInfo(item.linkType, item.linkValue)">
                <button class="coreshop-btn coreshop-btn-fillet" :style="{background:item.buttonColor,color:item.textColor}">{{item.buttonText}}</button>
            </view>
        </view>
    </view>
</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [goods],
        name: "corecmsimgsingle",
        props: {
            corecmsdata: {
                // type: Object,
                required: true,
            }
        },
        computed: {
            count() {
                // console.log(corecmsdata)
                return (this.corecmsdata.parameters.list.length > 0)
            }

        },
        methods: {
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

<style lang="scss" scoped>
    .corecms-imgsingle.ad { width: 100%; overflow: hidden; position: relative; margin: 20rpx 0rpx; padding:0 15rpx; }
    .corecms-imgsingle .ad-img { width: 100%; float: left; position: relative; z-index: 667; }
        .corecms-imgsingle .ad-img:last-child { margin-bottom: 0; }
    .corecms-imgsingle .imgup-btn { position: absolute; z-index: 668; bottom: 80upx; left: 40upx; }
        .corecms-imgsingle .imgup-btn .coreshop-btn { line-height: 2; font-size: 28upx; padding: 0 50upx; }
</style>
