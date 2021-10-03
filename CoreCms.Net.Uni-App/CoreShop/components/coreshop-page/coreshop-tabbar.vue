<template>
    <view class="">
        <view class="coreshop-tabbar u-margin-bottom-20" ref="tabBar">
            <scroll-view scroll-x='true' class="tabbar-list">
                <view class="tabbar-item" v-for="(item, index) in coreshopdata.parameters.list" :key="index" @click="showSliderInfo(item.linkType, item.linkValue)">
                    {{item.text}}
                    <view class="active-tabbar"></view>
                </view>
            </scroll-view>
        </view>
        <!-- <view class="coreshop-tabbar u-margin-bottom-20 tabbar-fixed" v-show="tabbarFixed">
            <scroll-view scroll-x='true' class="tabbar-list">
                <view class="tabbar-item" v-for="(item, index) in coreshopdata.parameters.list" :key="index" @click="showSliderInfo(item.linkType, item.linkValue)">
                    {{item.text}}
                    <view class="active-tabbar"></view>
                </view>
            </scroll-view>
        </view> -->
    </view>

</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [goods],
        name: "coreshopTabbar",
        props: {
            coreshopdata: {
                // type: Object,
                required: true,
            }
        },
        data() {
            return {
                searchTop: 0,
                scrollTop: 0,
                tabbarFixed: false
            };
        },
        created() {
            //#ifdef H5
            this.$nextTick(() => {
                this.searchTop = this.$refs.tabBar.$el.offsetTop - 52;
            })
            // #endif
            this.searchStyle()
        },

        mounted() {
            // #ifdef H5
            window.addEventListener('scroll', this.handleScroll)
            // #endif


        },
        methods: {
            searchStyle() {
                this.$store.commit('searchStyle', this.coreshopdata.parameters.style)
                // console.log(this.data.parameters.style)
            },
            handleScroll() {
                this.scrollTop = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop;
                this.scrollTop >= this.searchTop ? this.tabbarFixed = true : this.tabbarFixed = false;
            },
            goClassify() {
                this.$u.route({ type: 'switchTab', url: '/pages/category/index/index' });
            },
            showSliderInfo(type, val) {
                console.log(val)
                if (!val) {
                    return;
                }
                console.log("11")
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
                    // console.log("11")
                    // 文章列表
                    this.$u.route('/pages/article/list/list?cid=' + val)
                } else if (type == 5) {
                    //智能表单
                    this.$u.route('/pages/form/details/details?id=' + val)
                }
            },
        },
        onPageScroll() {
            var _this = this;
            // #ifdef MP-WEIXIN || APP-PLUS || APP-PLUS-NVUE
            const query = uni.createSelectorQuery().in(this)
            query.select('.search').boundingClientRect(function (res) {
                if (res.top < 0) {
                    _this.tabbarFixed = true;
                } else {
                    _this.tabbarFixed = false;
                }
            }).exec()
            // #endif
        }
    }
</script>

<style lang="scss" scoped>
    .tabbar-list { padding: 10rpx 0; background-color: #fff; white-space: nowrap; width: 100%;
        .tabbar-item { display: inline-block; padding: 10rpx 20rpx;
            .active-tabbar { display: none; }
        }
    }
    .tabbar-item:first-of-type { color: #FF7159;
        .active-tabbar { display: block; width: 100%; height: 4rpx; margin: 10rpx auto 0; background-color: #FF7159; }
    }
    .tabbar-fixed { position: fixed; top: 104rpx; transition: all .5s; z-index: 999; background-color: #fff; width: 100%; }
</style>
