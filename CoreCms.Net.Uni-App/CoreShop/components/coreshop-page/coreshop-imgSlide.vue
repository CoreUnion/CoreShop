<template>
    <view class="u-margin-20">
        <u-swiper :list="swiperItems" :effect3d="false" :title="false" @click="taped" bg-color="transparency"></u-swiper>
        <u-toast ref="uToast" />
    </view>
</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [goods],
        name: "coreshopimgSlide",
        props: {
            coreshopdata: {
                // type: Object,
                required: true,
            }
        },
        data() {
            return {
                swiperItems: [],
            };
        },
        computed: {
            count() {
                return (this.coreshopdata.parameters.list.length > 0)
            }
        },
        components: {},
        created() {
            var data = this.coreshopdata.parameters.list;
            for (var i = 0; i < data.length; i++) {
                let moder = {
                    image: data[i].image == '/images/empty-banner.png' ? '/static/images/common/empty-banner.png' : data[i].image,
                    opentype: 'click',
                    url: '',
                    title: data[i].linkType,
                    linkType: data[i].linkType,
                    linkValue: data[i].linkValue,
                }
                this.swiperItems.push(moder);
            }
        },
        watch: {},
        methods: {
            taped: function (e) {
                this.showSliderInfo(this.swiperItems[e].linkType, this.swiperItems[e].linkValue);
            },
            // 广告点击查看详情
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
        }
    }
</script>
