<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :title="storeName"></u-navbar>
        <view class="content my-store">
            <view class="" ref="myStore">
                <view class="my-store-t">
                    <view class="mst-top">
                        <image :src="storeBanner" mode="widthFix"></image>
                    </view>
                    <view class="mst-bot">
                        <view class='coreshop-grid'>
                            <view class='coreshop-item'>
                                <image class='coreshop-item-img' :src='storeLogo'></image>
                            </view>
                            <view class='coreshop-item'>
                                <view class="color-o fsz36">{{totalGoods}}</view>
                                <text class='coreshop-item-text'>全部宝贝</text>
                            </view>
                            <!-- #ifdef MP-TOUTIAO -->
                            <view class='coreshop-item' @click="createPoster()">
                                <u-icon name="star" size="36" label="分享店铺" label-pos="bottom" margin-top="15"></u-icon>
                            </view>
                            <!-- #endif -->
                            <!-- #ifndef MP-TOUTIAO -->
                            <view class='coreshop-item' @click="openPopup()">
                                <u-icon name="star" size="36" label="收藏本店" label-pos="bottom" margin-top="15"></u-icon>
                            </view>
                            <!-- #endif -->
                            <view class='coreshop-item'>
                                <!-- #ifdef MP-WEIXIN -->
                                <!--<button class='share coreshop-btn' open-type="share">-->
                                <button class='share coreshop-btn' @click="createPoster()">
                                    <u-icon name="grid" size="36" label="二维码" label-pos="bottom" margin-top="15"></u-icon>
                                </button>
                                <!-- #endif -->
                                <!-- #ifndef MP-WEIXIN -->
                                <button class='share coreshop-btn' @click="createPoster()">
                                    <u-icon name="grid" size="36" label="二维码" label-pos="bottom" margin-top="15"></u-icon>
                                </button>
                                <!-- #endif -->
                            </view>
                        </view>
                    </view>
                </view>
                <view class="my-store-m">
                    <view class="u-padding-15 u-margin-bottom-15 bg-white u-border-bottom" @tap="goSearch()">
                        <u-search placeholder="请输入关键字搜索" :show-action="true" action-text="搜索" :animation="false"></u-search>
                    </view>
                </view>
                <!-- 收藏弹出窗 -->
                <lvv-popup position="bottom" ref="lvvpopref" @click="closePopup()">
                    <view class="collect-pop" @click="closePopup()">
                        <image v-if="isWeixinBrowser" src="/static/images/distribution/wxh5.png" mode="widthFix"></image>
                        <!-- #ifdef MP-WEIXIN -->
                        <image src="/static/images/distribution/wxxcx.png" mode="widthFix"></image>
                        <!-- #endif -->
                        <!-- #ifdef H5 -->
                        <view class="h5-tip color-f fsz38">
                            <view>请将此页面添加浏览器书签</view>
                            <view>方便下次浏览</view>
                        </view>
                        <!-- #endif -->
                    </view>
                </lvv-popup>
            </view>
            <!-- 商品列表 -->
            <view>
                <view class="goodsBox" v-if="goodsData.length>0">
                    <u-grid col="2" :border="false" :align="center">
                        <u-grid-item bg-color="transparent" :custom-style="{padding: '0rpx'}" v-for="(item, index) in goodsData" :key="index" @click="goGoodsDetail(item.id)">
                            <view class="good_box">
                                <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                                <u-lazy-load threshold="-150" border-radius="10" :image="item.image" :index="index"></u-lazy-load>
                                <view class="good_title u-line-2">
                                    {{item.name}}
                                </view>
                                <view class="good-price">
                                    {{item.price}}元 <span class="u-font-xs color-9 linethrough u-margin-left-15 text-gray">{{item.mktprice}}元</span>
                                </view>
                                <view class="good-tag-recommend" v-if="item.isRecommend">
                                    推荐
                                </view>
                                <view class="good-tag-hot" v-if="item.isHot">
                                    热门
                                </view>
                            </view>
                        </u-grid-item>
                    </u-grid>
                </view>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$apiFilesUrl+'/static/images/empty/order.png'" icon-size="300" text="暂无商品列表" mode="list"></u-empty>
                </view>
            </view>
            <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
        </view>
    </view>

</template>
<script>
    import lvvPopup from '@/components/corecms-lvv-popup/corecms-lvv-popup.vue';
    import { goods, articles, commonUse } from '@/common/mixins/mixinsHelper.js'

    import {
        apiBaseUrl
    } from '@/common/setting/constVarsHelper.js'
    export default {
        mixins: [goods, articles, commonUse],
        components: {
            lvvPopup,
        },
        data() {
            return {
                goodsData: [],
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                },
                orderItems: [{
                    name: '全部宝贝',
                    nums: '115'
                },
                {
                    name: '收藏本店',
                    icon: '/static/images/ic-me-collect.png',
                },
                {
                    name: '二维码',
                    icon: '/static/images/qr_code.png',
                }
                ],
                storeCode: '',
                storeName: '', //店铺名称
                storeLogo: '',
                store_banner: '',
                storeDesc: '', //店铺介绍
                storeBanner: '',
                isWeixinBrowser: this.$common.isWeiXinBrowser(), //判断是否是微信浏览器
                totalGoods: 0,
                page: 1, //默认页码
                searchKey: '请输入关键字搜索',
                shareUrl: '/pages/share/jump/jump'
            }
        },
        onShow: function () {
            if (this.$store.state.config.distributionStore == '2') {
                //跳转到首页
                this.$u.route({
                    type: 'switchTab',
                    url: '/pages/index/default/default'
                });
            }
        },
        //加载执行
        onLoad: function (options) {
            let store = options.store;
            this.storeCode = store;
            this.getDistribution(store);
            this.getGoods();
        },
        mounted() {
            // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE
            window.addEventListener('scroll', this.handleScroll)
            // #endif
        },
        updated() {
            // #ifndef MP-WEIXIN || MP-TOUTIAO || MP-ALIPAY
            // 获取上半部分的整体高度
            this.$nextTick(() => {
                let h = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight; //浏览器高度
                this.top_height = this.$refs.myStore.$el.clientHeight;
            })
            // #endif
        },
        methods: {
            // 显示modal弹出框
            openPopup() {
                this.$refs.lvvpopref.show();
            },
            // 关闭modal弹出框
            closePopup() {
                this.$refs.lvvpopref.close();
            },
            //去搜索
            goSearch() {
                let pages = getCurrentPages();
                let prevPage = pages[pages.length - 2];
                // #ifdef H5 || MP-WEIXIN || APP-PLUS || APP-PLUS-NVUE || MP-TOUTIAO
                if (prevPage && prevPage.route) {
                    let search_flag = prevPage.route;
                    if (search_flag == 'pages/search/search') {
                        uni.navigateBack({
                            delta: 1
                        });
                    } else {
                        this.$u.route('/pages/search/search');
                    }
                } else {
                    this.$u.route('/pages/search/search');
                }
                // #endif
                // #ifdef MP-ALIPAY
                if (prevPage && prevPage.__proto__.route) {
                    let search_flag = prevPage.__proto__.route;
                    if (search_flag == 'pages/search/search') {
                        uni.navigateBack({
                            delta: 1
                        });
                    } else {
                        this.$u.route('/pages/search/search');
                    }
                } else {
                    this.$u.route('/pages/search/search');
                }
                // #endif
            },

            //取得商品数据
            getGoods: function () {
                let data = {
                    page: this.page,
                    limit: 10
                }
                this.status = 'loading'
                this.$u.api.goodsList(data).then(res => {
                    if (res.status) {
                        if (this.page >= res.data.totalPages) {
                            // 没有数据了
                            this.status = 'nomore'
                        } else {
                            // 未加载完毕
                            this.status = 'loadmore'
                            this.page++
                        }
                        this.goodsData = [...this.goodsData, ...res.data.list]
                    } else {
                        this.$u.toast(res.msg)
                    }
                });
            },
            //获取分销商信息
            getDistribution: function (store) {
                let _this = this;
                _this.$u.api.getDistributionStoreInfo({
                    id: store
                }).then(res => {
                    if (res.status) {
                        _this.storeName = res.data.storeName ? res.data.storeName : "我的店铺";
                        _this.storeDesc = res.data.storeDesc;
                        _this.storeLogo = res.data.storeLogo ? res.data.storeLogo : '/static/images/common/empty.png';
                        _this.storeBanner = res.data.storeBanner ? res.data.storeBanner : '/static/images/common/empty-banner.png';
                        _this.totalGoods = res.data.totalGoods;
                        uni.setNavigationBarTitle({
                            title: _this.storeName
                        });
                    } else {
                        //报错了
                        _this.$u.toast(res.msg);
                    }
                });
            },
            // 生成邀请海报
            createPoster() {
                let data = {
                    type: 3,
                    params: {
                        store: this.storeCode
                    },
                    page: 4,
                }
                let pages = getCurrentPages()
                let page = pages[pages.length - 1]
                let pageUrl = 'pages/share/jump/jump';
                // #ifdef H5
                data.client = 1;
                data.url = apiBaseUrl + 'wap/' + pageUrl;
                // #endif
                // #ifdef MP-WEIXIN
                data.client = 2;
                data.url = pageUrl;
                // #endif
                // #ifdef MP-ALIPAY
                data.client = 3;
                data.url = pageUrl;
                // #endif
                // #ifdef APP-PLUS || APP-PLUS-NVUE
                data.client = 5;
                data.url = apiBaseUrl + 'wap/' + pageUrl;
                // #endif
                // #ifdef MP-TOUTIAO
                data.client = 6;
                // #endif
                let userToken = this.$db.get('userToken')
                if (userToken && userToken != '') {
                    data.token = userToken
                }
                this.$u.api.share(data).then(res => {
                    if (res.status) {
                        this.$u.route('/pages/share/sharePoster/sharePoster?poster=' + encodeURIComponent(res.data))
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
            //获取分享URL
            getShareUrl() {
                let data = {
                    client: 2,
                    url: "/pages/share/jump/jump",
                    type: 1,
                    page: 4,
                    params: {
                        store: this.storeCode
                    }
                };
                let userToken = this.$db.get('userToken');
                if (userToken && userToken != '') {
                    data['token'] = userToken;
                }
                this.$u.api.share(data).then(res => {
                    this.shareUrl = res.data
                });
            }
        },
        watch: {
            storeCode: {
                handler() {
                    this.getShareUrl();
                },
                deep: true
            }
        },
        //上拉加载
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getGoods();
            }
        },
        //分享
        onShareAppMessage(res) {
            return {
                title: this.storeName ? this.storeName : this.$store.state.config.shareTitle,
                imageUrl: this.storeLogo ? this.storeLogo : this.$store.state.config.shareImage,
                path: this.shareUrl
            }
        },
        onShareTimeline(res) {
            return {
                title: this.storeName ? this.storeName : this.$store.state.config.shareTitle,
                imageUrl: this.storeLogo ? this.storeLogo : this.$store.state.config.shareImage,
                path: this.shareUrl
            }
        },
    }
</script>
<style lang="scss" scoped>
    view { box-sizing: border-box; }
    .mst-top { width: 100%; }
        .mst-top image { width: 100%; max-height: 450rpx; }
    .coreshop-grid { padding: 20rpx 26rpx; width: 100%; display: flex; border-top: 2rpx solid #ddd; background-color: #fff; margin-bottom: 20rpx; }
    .coreshop-item { flex: 1; text-align: center; position: relative; border-right: 2rpx solid #eee; height: 90rpx; }
        .coreshop-item:last-child { border-right: none; }
        .coreshop-item:active { transform: scale(.90); transition: all .5s; opacity: .8; }
    .coreshop-item-icon { width: 50rpx; height: 50rpx; display: block; margin: 0 auto; }
    .coreshop-item-text { font-size: 28rpx; color: #666; display: block; margin-top: 15rpx; }
    .coreshop-item-img { width: 150rpx; height: 150rpx; top: -70rpx; position: absolute; left: 42%; transform: translateX(-50%); border-radius: 10rpx; background-color: #fff; border-radius: 6rpx; box-shadow: 0 0 10rpx #ccc; }

    .collect-pop { width: 100%; height: 100%; position: absolute; left: 0; bottom: 0; }
        .collect-pop image { width: 100%; }
    .h5-tip { text-align: center; margin-top: 300rpx; }

    .coreshop-item .share { background: none !important; line-height: normal; }


    .goodsBox { border-radius: 16rpx; /*padding: 0rpx 10rpx; background: #FFFFFF !important;*/ color: #333333 !important; margin: 0 10rpx; }
    .u-cell { padding: 15rpx 25rpx; }
    .u-close { position: absolute; top: 30rpx; right: 30rpx; }
    .good_box { border-radius: 8px; margin: 3px; background-color: #ffffff; padding: 5px; position: relative; width: calc(100% - 6px); }
    .good_image { width: 100%; border-radius: 4px; }
    .good_title { font-size: 26rpx; margin-top: 5px; color: $u-main-color; }
    .good_title-xl { font-size: 28rpx; margin-top: 5px; color: $u-main-color; }
    .good-tag-hot { display: flex; margin-top: 5px; position: absolute; top: 15rpx; left: 15rpx; background-color: $u-type-error; color: #FFFFFF; display: flex; align-items: center; padding: 4rpx 14rpx; border-radius: 50rpx; font-size: 20rpx; line-height: 1; }
    .good-tag-recommend { display: flex; margin-top: 5px; position: absolute; top: 15rpx; right: 15rpx; background-color: $u-type-primary; color: #FFFFFF; margin-left: 10px; border-radius: 50rpx; line-height: 1; padding: 4rpx 14rpx; display: flex; align-items: center; border-radius: 50rpx; font-size: 20rpx; }
    .good-tag-recommend2 { display: flex; margin-top: 5px; position: absolute; bottom: 15rpx; left: 15rpx; background-color: $u-type-primary; color: #FFFFFF; border-radius: 50rpx; line-height: 1; padding: 4rpx 14rpx; display: flex; align-items: center; border-radius: 50rpx; font-size: 20rpx; }
    .good-price { font-size: 30rpx; color: $u-type-error; margin-top: 5px; }
    .good-des { font-size: 20rpx; color: $u-tips-color; margin-top: 5px; }

    .grid-text { font-size: 28rpx; margin-top: 4rpx; color: $u-type-info; }
</style>