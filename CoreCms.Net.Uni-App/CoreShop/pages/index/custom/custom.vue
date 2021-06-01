<template>
    <view class="content" style="padding-top: 0upx;">
        <corecmsPage :corecmsdata="pageData"></corecmsPage>
        <copyright></copyright>
        <!-- 登录提示 -->
		<corecms-login-modal></corecms-login-modal>
    </view>
</template>
<script>
    import corecmsPage from '@/components/corecms-page/corecms.vue';
    import copyright from '@/components/corecms-copyright/corecms-copyright.vue';
    import { goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [goods],
        components: {
            copyright,
            corecmsPage
        },
        data() {
            return {
                imageUrl: '/static/images/ShareImage.png', //店铺分享图片
                pageData: [],
                pageCode: 'mobile_home', //页面布局编码
                statusBarHeight: '0',
                customBarOpacity: false,
                scrollTop: 0,
                showLoad: false, //是否显示loading
                shareName: '',
                shareUrl: '/pages/share/jump/jump'
            };
        },
        computed: {
            appTitle() {
                return this.$store.state.config.shopName;
            }
        },
        onLoad(e) {
            //增加页面编码，可自定义编码
            if (e.pageCode) {
                this.pageCode = e.pageCode;
            }
            this.initData();
        },
        // 小程序沉浸式状态栏变色
        onPageScroll(e) {
            // console.log(e);
            e.scrollTop > 50 ? (this.customBarOpacity = true) : (this.customBarOpacity = false);
        },
        mounted() {
            // #ifdef H5
            window.addEventListener('scroll', this.handleScroll);
            // #endif
        },
        methods: {
            // 搜索框滑动变色
            handleScroll() {
                var scrollTop = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop;
                scrollTop > 50 ? (this.searchBarOpacity = true) : (this.searchBarOpacity = false);
            },
            destroyed() {
                window.removeEventListener('scroll', this.handleScroll);
            },
            goSearch() {
                uni.navigateTo({
                    url: '/pages/search/search'
                });
            },
            // 首页初始化获取数据
            initData() {
                this.showLoad = true;
                //获取首页配置
                this.$u.api.getPageConfig(
                    {
                        code: this.pageCode
                    }).then(res => {
                        if (res.status == true) {
                            this.pageData = res.data.items;
                            this.shareName = res.data.name;
                            uni.setNavigationBarTitle({
                                title: res.data.name
                            });
                            //隐藏loading
                            setTimeout(() => {
                                this.showLoad = false;
                            }, 600);
                        }
                    }
                    );
            },
            //获取分享URL
            getShareUrl() {
                let data = {
                    client: 2,
                    url: "/pages/share/jump/jump",
                    type: 1,
                    page: 7,
                    params: {
                        pageCode: this.pageCode
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
            pageCode: {
                handler() {
                    this.getShareUrl();
                },
                deep: true
            }
        },
        onPullDownRefresh() {
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
        // #ifdef MP-WEIXIN || APP-PLUS || APP-PLUS-NVUE
        onPageScroll() {
            var _this = this;
            const query = uni.createSelectorQuery();
            query
                .select('.content >>> .search')
                .boundingClientRect(function (res) {
                    if (res) {
                        if (res.top < 0) {
                            _this.$store.commit('searchFixed', true);
                        } else {
                            _this.$store.commit('searchFixed', false);
                        }
                    }
                })
                .exec();
        }
        //#endif
    };
</script>
<style lang="scss" scoped>
</style>
