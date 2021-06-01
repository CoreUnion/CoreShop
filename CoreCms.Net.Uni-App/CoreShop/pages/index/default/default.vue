<template>
    <!-- 页面主体 -->
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
         <!-- #ifdef H5 || APP-PLUS-NVUE || APP-PLUS ||  MP-ALIPAY || MP-TOUTIAO -->
        <u-navbar :is-back="true" back-icon-name="scan" back-icon-color="#fff" :title="appTitle" :background="background" title-color="#fff" :custom-back="about"></u-navbar>
        <!-- #endif -->
        <!-- #ifdef MP-WEIXIN -->
        <view class="swiper-background-box">
            <view class="swiper-background"></view>
        </view>
        <view class="head-bg-box" :style="[{backgroundColor:'rgba(229, 77, 66,'+ opacity +')'}]">
            <view class="indexHeaderLogoBox">
                <u-image src="/static/images/logo/logo2.png" width="65%" mode="widthFix" :lazy-load="false" :show-loading="false"></u-image>
            </view>
        </view>
        <view class="view-content"></view>
        <!-- #endif -->

        <!--调用组件-->
        <corecmsPage :corecmsdata="pageData"></corecmsPage>
        <copyright v-if="copy"></copyright>
        <!-- #ifdef H5 || APP-PLUS-NVUE || APP-PLUS -->
        <view class="floatingButton" @click="showChat()">
            <u-icon name="server-man" color="#e54d42" size="60"></u-icon>
        </view>
        <!-- #endif -->
        <!-- #ifdef MP-WEIXIN -->
        <button class="floatingButton" hover-class="none" open-type="contact" bindcontact="showChat" :session-from="kefupara">
            <u-icon name="server-man" color="#e54d42" size="60"></u-icon>
        </button>
        <!-- #endif -->
        <!-- #ifdef MP-ALIPAY -->
        <contact-button class="floatingButton icon" icon="/static/images/common/seller-content.png" size="80rpx*80rpx" tnt-inst-id="" scene="" hover-class="none" />
        <!-- #endif -->
        <!-- #ifdef MP-TOUTIAO -->
        <!-- 头条客服 -->
        <!-- #endif -->
        <u-back-top :scroll-top="scrollTop" :duration="500"></u-back-top>

        <!--弹出框-->
        <!--<corecms-modal-img :show="modalShow"  :src="$apiFilesUrl+'/static/images/empty/reward.png'" @imgTap="imgTap" @closeTap="closeTap" />-->
        <!-- 登录提示 -->
        <corecms-login-modal></corecms-login-modal>
    </view>
</template>
<script>
    import { mapMutations, mapActions, mapState } from 'vuex';
    import corecmsPage from '@/components/corecms-page/corecms.vue';
    import copyright from '@/components/corecms-copyright/corecms-copyright.vue';
    import modalImg from '@/components/corecms-modal-img/corecms-modal-img.vue';
    import { goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [goods],
        components: {
            copyright,
            corecmsPage,
            modalImg
        },
        data() {
            return {
                background: {
                    backgroundColor: '#e54d42',
                },
                swiperItems: [],
                currentIndex: 0,
                opacity: 0,
                scrollTop: 0,
                imageUrl: '/static/images/ShareImage.png', //店铺分享图片
                pageData: [],
                pageCode: 'mobile_home', //页面布局编码
                //userInfo: {}, // 用户信息
                kefupara: '', //客服传递资料
                copy: false,
                shareUrl: '/pages/share/jump/jump',
                modalShow: true,
            };
        },
        updated() {
            this.copy = true;
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
                return this.$store.state.config.shopName;
            },
            // 获取店铺联系人手机号
            shopMobile() {
                return this.$store.state.config.shopMobile || 0;
            }
        },
        onLoad(e) {
            this.initData();
        },
        methods: {
            about() {

            },
            imgTap() {
                this.modalShow = false;
                uni.navigateTo({
                    url: "/pages/reward/reward"
                });
            },
            closeTap() {
                this.modalShow = false;
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
                // #ifdef H5
                let _this = this;
                window._AIHECONG('ini', {
                    entId: this.$store.state.config.entId,
                    button: false,
                    appearance: {
                        panelMobile: {
                            tone: '#FF7159',
                            sideMargin: 30,
                            ratio: 'part',
                            headHeight: 50
                        }
                    }
                });
                //传递客户信息
                window._AIHECONG('customer', {
                    head: _this.userInfo.avatarImage,
                    名称: _this.userInfo.nickName,
                    手机: _this.userInfo.mobile
                });
                window._AIHECONG('showChat');
                // #endif

                // 客服页面
                // #ifdef APP-PLUS || APP-PLUS-NVUE
                this.$u.route('/pages/member/customerService/index');
                // #endif

                // 头条系客服
                // #ifdef MP-TOUTIAO
                if (this.shopMobile != 0) {
                    let _this = this;
                    tt.makePhoneCall({
                        phoneNumber: this.shopMobile.toString(),
                        success(res) { },
                        fail(res) { }
                    });
                } else {
                    _this.$u.toast('暂无设置客服电话');
                }
                // #endif
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
    .indexHeaderLogoBox { width: 100%; height: 120rpx; }
    .swiper-background-box { position: absolute; height: 348rpx; width: 100%; top: 0; display: block; transition: 0s; z-index: -1 }
        .swiper-background-box .swiper-background { position: absolute; height: 100%; width: 100%; top: 0; background-size: cover; opacity: 1; transition: opacity 0.25s; background-image: url($apiFilesUrl+'/static/images/default/swiper-background-2.png'); }
        .swiper-background-box.welcome { top: calc(var(--status-bar-height) + 101rpx); transition: top 0.25s; }

    .head-bg-box { position: fixed; width: 100%; top: 0; z-index: 9999; background-color: rgba(229, 77, 66,0); padding-top: calc(var(--status-bar-height) + 15rpx); transition: top .25s; }

    /*占位*/
    .view-content { width: 100%; /* #ifdef APP-PLUS */ margin-top: calc(var(--status-bar-height) + 70rpx); /* #endif */ /* #ifdef H5 */ margin-top: calc(var(--status-bar-height) + 110rpx); /* #endif */ /* #ifdef MP */ margin-top: calc(var(--status-bar-height) + 135rpx); /* #endif */ }
</style>
