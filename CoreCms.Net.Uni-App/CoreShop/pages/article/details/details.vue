<template>
    <!-- 页面主体 -->
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :custom-back="goBack" :title="title"></u-navbar>
        <view class="coreshop-bg-white u-padding-20 u-margin-20">
            <u-image width="100%" height="300rpx" v-if="info.coverImage" :src="info.coverImage && info.coverImage!='null' ?  info.coverImage+'?x-oss-process=image/resize,m_lfit,h_320,w_240' : '/static/images/common/empty-banner.png'"></u-image>
            <view class="article-title">
                {{ info.title }}
            </view>
            <view class="article-time" v-if="info.createTime">{{ info.createTime }}</view>
            <u-line color="info" border-style="dashed" margin="20rpx 0" />
            <view class="u-content">
                <u-parse :html="contentBody" :selectable="true"></u-parse>
            </view>
        </view>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>

<script>
    import { tools } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [tools],
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
    @import "details.scss";
</style>