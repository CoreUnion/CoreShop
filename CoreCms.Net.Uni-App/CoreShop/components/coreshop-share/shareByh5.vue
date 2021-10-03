<template>
    <view class="coreshop-share-Box">
        <u-toast ref="uToast" />
        <view class="coreshop-share-pop">
            <view class="coreshop-share-item" @click="copyUrl()" v-show="!ifwx">
                <button class="coreshop-btn">
                    <u-icon label="复制链接" label-pos="bottom" size="80" name="/static/images/common/share-friend.png"></u-icon>
                </button>
            </view>
            <view class="coreshop-share-item" @click="createPoster()">
                <button class="coreshop-btn">
                    <u-icon label="生成海报" label-pos="bottom" size="80" name="/static/images/common/share-poster.png"></u-icon>
                </button>
            </view>
        </view>
        <view class="coreshop-bottomBox">
            <button class="coreshop-btn coreshop-btn-w coreshop-btn-square" @click="close()">关闭</button>
        </view>
    </view>
</template>

<script>
    import { h5Url } from '@/common/setting/constVarsHelper.js'
    export default {
        props: {
            // 商品id
            goodsId: {
                type: Number,
                default: 0
            },
            // 分享的图片
            shareImg: {
                type: String,
                default: ''
            },
            // 分享标题
            shareTitle: {
                type: String,
                default: ''
            },
            // 分享内容
            shareContent: {
                type: String,
                default: ''
            },
            // 分享链接
            shareHref: {
                type: String,
                default: ''
            },
            //分享类型
            shareType: {
                type: Number,
                default: 1
            },
            //拼团id
            groupId: {
                type: Number,
                default: 0
            },
            //拼团的团队id
            teamId: {
                type: Number,
                default: 0
            },
            ifwx: {
                type: Boolean
            }
        },
        mounted() {
            /**
             *
             * H5端分享两种 (微信浏览器内引导用户去分享, 其他浏览器)
             *
             */
        },
        methods: {
            // 关闭弹出层
            close() {
                this.$emit('close')
            },
            // 生成海报
            createPoster() {
                let data = {};
                if (this.shareType == 1) {
                    //商品
                    data = {
                        page: 2, //商品
                        url: h5Url + 'pages/share/jump/jump',
                        params: {
                            goodsId: this.goodsId
                        },
                        type: 3,//海报
                        client: 1
                    }
                    let userToken = this.$db.get('userToken')
                    if (userToken) {
                        data.token = userToken
                    }
                } else if (this.shareType == 3) {
                    //拼团
                    data = {
                        page: 3, //商品
                        url: h5Url + 'pages/share/jump/jump',
                        params: {
                            goodsId: this.goodsId,
                            groupId: this.groupId,
                            teamId: this.teamId
                        },
                        type: 3,//海报
                        client: 1
                    }
                    let userToken = this.$db.get('userToken')
                    if (userToken) {
                        data.token = userToken
                    }
                }
                this.$u.api.share(data).then(res => {
                    if (res.status) {
                        this.close()
                        this.$u.route('/pages/share/sharePoster/sharePoster?poster=' + encodeURIComponent(res.data))
                    } else {
                        this.$u.toast(res.msg)
                    }
                });
            },
            copyUrl() {
                let data = {};
                if (this.shareType == 1) {
                    data = {
                        page: 2, //商品
                        url: h5Url + 'pages/share/jump/jump',
                        params: {
                            goodsId: this.goodsId
                        },
                        type: 1,//URL
                        client: 1
                    }
                    let userToken = this.$db.get('userToken')
                    if (userToken) {
                        data.token = userToken
                    }
                } else if (this.shareType == 3) {
                    data = {
                        page: 3, //拼团
                        url: h5Url + 'pages/share/jump/jump',
                        params: {
                            goodsId: this.goodsId,
                            groupId: this.groupId,
                            teamId: this.teamId
                        },
                        type: 1,//URL
                        client: 1
                    }
                    let userToken = this.$db.get('userToken')
                    if (userToken) {
                        data.token = userToken
                    }
                }
                let _this = this;
                _this.$u.api.share(data).then(res => {
                    if (res.status) {
                        uni.setClipboardData({
                            data: res.data,
                            success: function (data) {
                                _this.$refs.uToast.show({ title: '复制成功', type: 'success' })
                            },
                            fail: function (err) {
                                _this.$u.toast('复制分享URL失败');
                            }
                        })
                    } else {
                        _this.$u.toast('复制分享URL失败');
                    }
                });
            },
            // 分享操作
            share() {
                // h5分享 判断是否是微信浏览器 引导用户完成分享操作

                // 其他浏览器的分享
            }
        }
    }
</script>
