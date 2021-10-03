<template>
    <view class="u-wrap">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :is-back="false" :background="background">
            <view class="slot-wrap">
                <u-search :show-action="true" shape="round" v-model="searchKey" action-text="搜索" placeholder="请输入搜索内容" @custom="goSearch" @search="goSearch" :action-style="actionStyle"></u-search>
            </view>
        </u-navbar>

        <view class="u-padding-20" v-if="CateStyle==1">
            <u-row gutter="16">
                <u-col span="12" v-for="(item,index) in tabbar" :key="index" @click="goClass(item.id)">
                    <u-image width="100%" height="300rpx" :src="item.imageUrl">
                        <u-loading slot="loading"></u-loading>
                    </u-image>
                    <view class="u-text-center u-padding-top-30 u-padding-bottom-30">
                        {{item.name}}
                    </view>
                </u-col>
            </u-row>
        </view>
        <view class="u-padding-top-20 u-padding-bottom-20" v-if="CateStyle==2">
            <u-row gutter="16">
                <u-col span="4" v-for="(item,index) in tabbar" :key="index" @click="goClass(item.id)">
                    <u-image width="100%" height="210rpx" :src="item.imageUrl">
                        <u-loading slot="loading"></u-loading>
                    </u-image>
                    <view class="u-text-center u-padding-top-30 u-padding-bottom-30">
                        {{item.name}}
                    </view>
                </u-col>
            </u-row>
        </view>
        <view class="u-menu-wrap" v-if="CateStyle==3">
            <scroll-view scroll-y scroll-with-animation class="u-tab-view menu-scroll-view" :scroll-top="scrollTop">
                <view v-for="(item,index) in tabbar" :key="index" class="u-tab-item" :class="[current==index ? 'u-tab-item-active' : '']"
                      :data-current="index" @tap.stop="swichMenu(index)">
                    <text class="u-line-1">{{item.name}}</text>
                </view>
            </scroll-view>
            <block v-for="(item,index) in tabbar" :key="index">
                <scroll-view scroll-y class="right-box" v-if="current==index">
                    <view class="page-view">
                        <view class="class-item">
                            <view class="item-title">
                                <text>{{item.name}}</text>
                            </view>
                            <view class="item-container">
                                <view class="thumb-box" v-for="(item1, index1) in item.child" :key="index1" @click="goClass(item1.id)">
                                    <u-image width="120rpx" height="120rpx" :src="item1.imageUrl">
                                        <u-loading slot="loading"></u-loading>
                                    </u-image>
                                    <view class="item-menu-name">{{item1.name}}</view>
                                </view>
                            </view>
                        </view>
                    </view>
                </scroll-view>
            </block>
        </view>

        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>
<script>
    import { goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [goods],
        data() {
            return {
                background: {
                    backgroundColor: '#e54d42',
                },
                actionStyle: {
                    color: '#ffffff',
                },
                tabbar: [],
                scrollTop: 0, //tab标题的滚动条位置
                current: 0, // 预设当前项的值
                menuHeight: 0, // 左边菜单的高度
                menuItemHeight: 0, // 左边菜单item的高度,
                beans: [],
                advert: {},
                isChild: false,
                searchKey: ''
            }
        },
        computed: {
            CateStyle() {
                return this.$store.state.config.cateStyle ? this.$store.state.config.cateStyle : 3;
            }
        },
        onShow() {
            this.categories();
            this.getBanner();
        },
        methods: {
            categories() {
                this.$u.api.categories().then(res => {
                    if (res.status) {
                        this.tabbar = res.data;
                    }
                });
            },
            getImg() {
                return Math.floor(Math.random() * 35);
            },
            // 点击左边的栏目切换
            async swichMenu(index) {
                if (index == this.current) return;
                this.current = index;
                // 如果为0，意味着尚未初始化
                if (this.menuHeight == 0 || this.menuItemHeight == 0) {
                    await this.getElRect('menu-scroll-view', 'menuHeight');
                    await this.getElRect('u-tab-item', 'menuItemHeight');
                }
                // 将菜单菜单活动item垂直居中
                this.scrollTop = index * this.menuItemHeight + this.menuItemHeight / 2 - this.menuHeight / 2;
            },
            // 获取一个目标元素的高度
            getElRect(elClass, dataVal) {
                new Promise((resolve, reject) => {
                    const query = uni.createSelectorQuery().in(this);
                    query.select('.' + elClass).fields({ size: true }, res => {
                        // 如果节点尚未生成，res值为null，循环调用执行
                        if (!res) {
                            setTimeout(() => {
                                this.getElRect(elClass);
                            }, 10);
                            return;
                        }
                        this[dataVal] = res.height;
                    }).exec();
                })
            },
            goClass(catId) {
                uni.navigateTo({
                    url: '/pages/category/list/list?id=' + catId
                });
            },
            getBanner() {
                this.$u.api.advert(
                    {
                        codes: 'tpl1_class_banner1'
                    }).then(res => {
                        this.advert = res.data;
                    }
                    );
            },
            // 广告点击查看详情
            showSliderInfo(type, val) {
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
                    this.goGoodsDetail(val);
                } else if (type == 3) {
                    // 文章详情
                    this.$u.route('/pages/article/details/details?id=' + val + '&idType=1');
                } else if (type == 4) {
                    // 文章列表
                    this.$u.route('/pages/article/list/list?cid=' + val);
                }
            },
            goSearch() {
                if (this.searchKey != '') {
                    this.$u.route('/pages/category/list/list?key=' + this.searchKey);
                } else {
                    this.$refs.uToast.show({
                        title: '请输入查询关键字',
                        type: 'warning',
                    })
                }
            }
        }
    }
</script>
<style lang="scss" scoped>
    @import "index.scss";
</style>
