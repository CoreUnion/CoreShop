<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :title="storeName"></u-navbar>
        <view ref="myStore" class="myStore">
            <view class="mst-top">
                <u-swiper :list="storeBanner"></u-swiper>
            </view>
            <view class="mst-bot">
                <view class='coreshop-grid'>
                    <view class='coreshop-item'>
                        <image class='coreshop-item-img' :src='storeLogo'></image>
                    </view>
                    <view class='coreshop-item'>
                        <text class="u-font-xl">{{totalGoods}}</text>
                        <text class='coreshop-item-text'>全部宝贝</text>
                    </view>
                    <view class='coreshop-item' @click="createPoster()">
                        <u-icon name="grid" size="36" label="二维码" label-pos="bottom" margin-top="15"></u-icon>
                    </view>
                </view>
            </view>

            <view class="u-padding-15 u-margin-bottom-15 coreshop-bg-white u-border-bottom" @tap="goSearch()">
                <u-search placeholder="请输入关键字搜索" :show-action="true" action-text="搜索" :animation="false"></u-search>
            </view>
            <!-- 收藏弹出窗 -->
            <u-popup mode="top" v-model="lvvpopref">
                <view class="collect-pop">
                    <image v-if="isWeixinBrowser" src="/static/images/distribution/wxh5.png" mode="widthFix"></image>
                    <image src="/static/images/distribution/wxxcx.png" mode="widthFix"></image>
                </view>
            </u-popup>
        </view>
        <!-- 商品列表 -->
        <view>
            <view class="coreshop-goods-group" v-if="goodsData.length>0">
                <u-grid col="2" :border="false" :align="center">
                    <u-grid-item bg-color="transparent" :custom-style="{padding: '0rpx'}" v-for="(item, index) in goodsData" :key="index" @click="goGoodsDetail(item.id)">
                        <view class="good_box">
                            <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                            <u-lazy-load threshold="-150" border-radius="10" :image="item.image" :index="index"></u-lazy-load>
                            <view class="good_title u-line-2">
                                {{item.name}}
                            </view>
                            <view class="good-price">
                                {{item.price}}元 <span class="u-font-xs  coreshop-text-through u-margin-left-15 coreshop-text-gray">{{item.mktprice}}元</span>
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
                <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/order.png'" icon-size="300" text="暂无商品列表" mode="list"></u-empty>
            </view>
        </view>
        <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
    </view>
</template>
<script>
    import { goods, articles, commonUse } from '@/common/mixins/mixinsHelper.js'

    export default {
        mixins: [goods, articles, commonUse],
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
                storeBanner: [],
                isWeixinBrowser: this.$common.isWeiXinBrowser(), //判断是否是微信浏览器
                totalGoods: 0,
                page: 1, //默认页码
                searchKey: '请输入关键字搜索',
                shareUrl: '/pages/share/jump/jump',
                lvvpopref: false
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
        },
        updated() {
        },
        methods: {
            // 显示modal弹出框
            openPopup() {
                this.lvvpopref = true;
            },
            // 关闭modal弹出框
            closePopup() {
                this.lvvpopref = false;
            },
            //去搜索
            goSearch() {
                let pages = getCurrentPages();
                let prevPage = pages[pages.length - 2];
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

                        if (res.data.storeBanner) {
                            _this.storeBanner = res.data.storeBanner.split(',');
                        } else {
                            _this.storeBanner.push('/static/images/common/empty-banner.png');
                        }

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
                data.client = 2;

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
    @import "myStore.scss";
</style>