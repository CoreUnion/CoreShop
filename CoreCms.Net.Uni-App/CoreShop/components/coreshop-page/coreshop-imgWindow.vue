<template>
    <view class="u-margin-bottom-20">
        <u-toast ref="uToast" />
        <u-grid :border="false" :align="center" :col="coreshopdata.parameters.style" v-if="coreshopdata.parameters.style == '2' ||coreshopdata.parameters.style == '3' ||coreshopdata.parameters.style == '4'">
            <u-grid-item bg-color="transparent" v-for="(item, index) in coreshopdata.parameters.list" :key="index" :custom-style="{padding:coreshopdata.parameters.margin+'rpx'}">
                <image :src="item.image" mode="widthFix" @click="showSliderInfo(item.linkType, item.linkValue)" style="width:100%;"></image>
            </u-grid-item>
        </u-grid>
        <u-grid :border="false" :align="center" :col="2" v-if="coreshopdata.parameters.style == '0'">
            <u-grid-item bg-color="transparent" :custom-style="{padding:coreshopdata.parameters.margin+'rpx'}">
                <image :src="coreshopdata.parameters.list[0].image" mode="widthFix" v-if="coreshopdata.parameters.list[0]" @click="showSliderInfo(coreshopdata.parameters.list[0].linkType, coreshopdata.parameters.list[0].linkValue)" class="w100"></image>
            </u-grid-item>
            <u-grid-item bg-color="transparent" :custom-style="{padding:coreshopdata.parameters.margin+'rpx'}">
                <image :src="coreshopdata.parameters.list[1].image" mode="widthFix" v-if="coreshopdata.parameters.list[1]" @click="showSliderInfo(coreshopdata.parameters.list[1].linkType, coreshopdata.parameters.list[1].linkValue)" class="w100"></image>
                <u-grid :border="false" :align="center" :col="2">
                    <u-grid-item bg-color="transparent" :custom-style="{padding:coreshopdata.parameters.margin+'rpx'}">
                        <image :src="coreshopdata.parameters.list[2].image" mode="widthFix" v-if="coreshopdata.parameters.list[2]" @click="showSliderInfo(coreshopdata.parameters.list[2].linkType, coreshopdata.parameters.list[2].linkValue)" class="w100"></image>
                    </u-grid-item>
                    <u-grid-item bg-color="transparent" :custom-style="{padding:coreshopdata.parameters.margin+'rpx'}">
                        <image :src="coreshopdata.parameters.list[3].image" mode="widthFix" v-if="coreshopdata.parameters.list[3]" @click="showSliderInfo(coreshopdata.parameters.list[3].linkType, coreshopdata.parameters.list[3].linkValue)" class="w100"></image>
                    </u-grid-item>
                </u-grid>
            </u-grid-item>
        </u-grid>

    </view>
</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [goods],
        name: "coreshopimgwindow",
        props: {
            coreshopdata: {
                // type: Object,
                required: true,
            }
        },
        data() {
            return {
                padding: '3'
            }
        },
        methods: {
            showSliderInfo(type, val) {
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
                        if (val == '/pages/index/default/default' || val == '/pages/category/index/index' || val == '/pages/index/cart/cart' || val ==
                            '/pages/index/member/member') {
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
        }
    }
</script>
<style lang="scss" scoped>
    
</style>
