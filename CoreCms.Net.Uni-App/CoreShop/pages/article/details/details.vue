<template>
    <!-- 页面主体 -->
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :title="title"></u-navbar>
        <view class="page-body">
            <u-image width="100%" height="300rpx" v-if="info.coverImage" :src="info.coverImage && info.coverImage!='null' ?  info.coverImage+'?x-oss-process=image/resize,m_lfit,h_320,w_240' : '/static/images/common/empty-banner.png'"></u-image>
            <view class="article-title">
                {{ info.title }}
            </view>
            <view class="article-time" v-if="info.createTime">{{ info.createTime }}</view>
            <view class="u-content">
                <u-parse :html="contentBody" :selectable="true"></u-parse>
            </view>
        </view>
        <!-- 登录提示 -->
        <corecms-login-modal></corecms-login-modal>
    </view>
</template>

<script>
    export default {
        components: {
        },
        data() {
            return {
                idType: 1, //1文章 2公告 3微信图文消息
                id: 0,
                info: {},
                contentBody: '',
                shareUrl: '/pages/share/jump/jump',
                title: ''
            };
        },
        onLoad(e) {
            this.idType = e.idType;
            this.id = e.id;

            console.log(e.idType);
            console.log(e.id);

            if (!this.idType && !this.id) {
                this.$refs.uToast.show({ title: '获取失败', type: 'error', isTab: true, url: '/pages/index/default/default' });
            } else if (this.idType == 1) {
                this.title = "文章详情";
                this.articleDetail();
            } else if (this.idType == 2) {
                this.title = "公告详情";
                this.noticeDetail();
            } else if (this.idType == 3) {
                this.title = "图文消息";
                this.messageDetail();
            }
        },
        computed: {
            shopName() {
                return this.$store.state.config.shopName;
            },
            shopLogo() {
                return this.$store.state.config.shopLogo;
            }
        },
        methods: {
            articleDetail() {
                let data = {
                    id: this.id
                };
                this.$u.api.articleInfo(data).then(res => {
                    if (res.status) {
                        this.info = res.data;
                        this.contentBody = res.data.contentBody;
                        this.title = this.info.title;
                    } else {
                        this.$refs.uToast.show({ title: res.msg, type: 'error', back: true })
                    }
                });
            },
            noticeDetail() {
                let data = {
                    id: this.id
                };
                this.$u.api.noticeInfo(data).then(res => {
                    if (res.status) {
                        this.info = res.data;
                        this.contentBody = res.data.contentBody;
                        this.title = this.info.title;
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            },
            //微信图文消息
            messageDetail() {
                let data = {
                    id: this.id
                };
                this.$u.api.messageDetail(data).then(res => {
                    if (res.status) {
                        this.info = res.data;
                        this.contentBody = res.data.contentBody;
                        this.title = this.info.title;
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            },
            //获取分享URL
            getShareUrl() {
                let data = {
                    client: 2,
                    url: "/pages/share/jump/jump",
                    type: 1,
                    page: 5,
                    params: {
                        articleId: this.id,
                        articleType: this.idType
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
            id: {
                handler() {
                    this.getShareUrl();
                },
                deep: true
            }
        },
        //分享
        onShareAppMessage(res) {
            return {
                title: this.info.title,
                path: this.shareUrl
            }
        },
        onShareTimeline(res) {
            return {
                title: this.info.title,
                path: this.shareUrl
            }
        },
    };
</script>

<style lang="scss" scoped>
    .page-body { background: #FFFFFF; border-top-left-radius: 38rpx; border-top-right-radius: 38rpx; margin: 25rpx; padding: 25rpx; }

    .article-title { font-size: 32upx; color: #333; margin: 20upx 0upx; position: relative; text-align: center; }
    .article-time { margin-top: 10rpx; font-size: 22rpx; text-align: center; }

    .u-content { margin-top: 20rpx; color: $u-content-color; font-size: 28rpx; line-height: 1.8; }
        .u-content p { color: $u-tips-color; }
</style>