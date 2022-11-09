<template>
    <!-- 页面主体 -->
    <view>
        <!--提示框组件-->
        <u-toast ref="uToast" />
        <!--无网络组件-->
        <u-no-network></u-no-network>
        <!--头部组件-->
        <u-navbar :is-back="false" :title="homeTitle" :background="background" :title-color="titleColor" :custom-back="about"></u-navbar>

        <!--首页模块化组合组件-->
        <coreshopPage :coreshopdata="pageData"></coreshopPage>
        <!--版权组件-->
        <copyright></copyright>
        <!--客服组件-->
        <button class="floatingButton" hover-class="none" open-type="contact" bindcontact="showChat" :session-from="kefupara">
            <u-icon name="server-man" color="#e54d42" size="60"></u-icon>
        </button>
        <!--返回顶部组件-->
        <u-back-top :scroll-top="scrollTop" :duration="500"></u-back-top>

        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>
<script>
    import { mapMutations, mapActions, mapState } from 'vuex';
    import coreshopPage from '@/components/coreshop-page/coreshop.vue';
    import copyright from '@/components/coreshop-copyright/coreshop-copyright.vue';
    import modalImg from '@/components/coreshop-modal-img/coreshop-modal-img.vue';
    import { goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [goods],
        components: {
            copyright,
            coreshopPage,
            modalImg
        },
        data() {
            return {
                background: this.$coreTheme.mainNabBar.background,
                titleColor: this.$coreTheme.mainNabBar.titleColor,

                swiperItems: [],
                currentIndex: 0,
                opacity: 0,
                scrollTop: 0,
                imageUrl: '/static/images/ShareImage.png', //店铺分享图片
                pageData: [],
                pageCode: 'mobile_home', //页面布局编码
                //userInfo: {}, // 用户信息
                kefupara: '', //客服传递资料
                shareUrl: '/pages/share/jump/jump',
                modalShow: true,
                homeTitle: '',
            };
        },
        computed: {
            ...mapState({
                hasLogin: state => state.hasLogin,
                userInfo: state => state.userInfo,
            }),
            hasLogin: {
                get() {
                    return this.$store.state.hasLogin;
                },
                set(val) {
                    this.$store.commit('hasLogin', val);
                }
            },
            userInfo: {
                get() {
                    return this.$store.state.userInfo;
                },
                set(val) {
                    this.$store.commit('userInfo', val);
                }
            },
            appTitle() {
                this.homeTitle = this.$store.state.config.shopName;
                return this.homeTitle;
            },
            // 获取店铺联系人手机号
            shopMobile() {
                return this.$store.state.config.shopMobile || 0;
            }
        },
        onLoad(e) {
            this.initData();
            console.log("数据：" + this.$globalConstVars.apiFilesUrl);
        },
        methods: {
            about() {

            },
            goSearch() {
                uni.navigateTo({
                    url: '/pages/search/search'
                });
            },
            // 首页初始化获取数据
            initData() {
                uni.showLoading({
                    title: '加载中'
                });
                //获取首页配置
                this.$u.api.getPageConfig({ code: this.pageCode }).then(res => {
                    if (res.status == true) {
                        this.pageData = res.data.items;

                        if (res.data.items.length > 0) {
                            for (var i = 0; i < res.data.items.length; i++) {
                                if (res.data.items[i].widgetCode == 'topImgSlide') {
                                    var data = res.data.items[i].parameters.list;
                                    for (var i = 0; i < data.length; i++) {
                                        let moder = {
                                            image: data[i].image == '/images/empty-banner.png' ? '/static/images/common/empty-banner.png' : data[i].image,
                                            bg: data[i].bg == '/images/empty-banner.png' ? '/static/images/common/empty-banner.png' : data[i].bg,
                                            opentype: 'click',
                                            url: '',
                                            title: data[i].linkType,
                                            linkType: data[i].linkType,
                                            linkValue: data[i].linkValue,
                                        }
                                        this.swiperItems.push(moder);
                                    }
                                }
                            }
                        }
                    }
                    setTimeout(function () {
                        uni.hideLoading();
                    }, 1000);
                });

                var _this = this;
                if (this.$db.get('userToken')) {
                    this.$u.api.userInfo().then(res => {
                        if (res.status) {
                            _this.userInfo = res.data;
                            _this.hasLogin = true;
                            // #ifdef MP-WEIXIN
                            //微信小程序打开客服时，传递用户信息
                            var kefupara = {};
                            kefupara.nickName = res.data.nickName;
                            kefupara.tel = res.data.mobile;
                            _this.kefupara = JSON.stringify(kefupara);
                            //console.log(_this.kefupara);
                            // #endif
                        }
                    });
                }
                this.getShareUrl();
            },
            swiperChange(e) {
                this.currentIndex = e;
            },
            //在线客服,只有手机号的，请自己替换为手机号
            showChat() {
            },
            //获取分享URL
            getShareUrl() {
                let data = {
                    client: 2,
                    url: "/pages/share/jump/jump",
                    type: 1,
                    page: 1,
                };
                let userToken = this.$db.get('userToken');
                if (userToken && userToken != '') {
                    data['token'] = userToken;
                }
                this.$u.api.share(data).then(res => {
                    this.shareUrl = res.data
                });
            },
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
                            this.$u.route({
                                type: 'switchTab',
                                url: val
                            });
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

        },
        onPullDownRefresh() {
            this.swiperItems = [];
            this.initData();
            uni.stopPullDownRefresh();
        },
        //分享
        onShareAppMessage(res) {
            return {
                title: this.$store.state.config.shareTitle,
                imageUrl: this.$store.state.config.shareImage,
                path: this.shareUrl
            }
        },
        onShareTimeline(res) {
            return {
                title: this.$store.state.config.shareTitle,
                imageUrl: this.$store.state.config.shareImage,
                path: this.shareUrl
            }
        },
        onPageScroll: function (e) {
            this.scrollTop = e.scrollTop;
            if (e.scrollTop <= 100) {
                this.opacity = e.scrollTop / 100;
            } else if (this.scrollTop > 100) {
                this.opacity = 1;
            }
        },
    };
</script>

<style lang="scss">
</style>
