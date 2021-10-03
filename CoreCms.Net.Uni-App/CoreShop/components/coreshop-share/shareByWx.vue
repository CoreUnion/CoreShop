<template>
    <view class="coreshop-share-Box">
        <u-toast ref="uToast" />
        <view class="coreshop-share-pop">
            <view class="coreshop-share-item">
                <button class="coreshop-btn" open-type="share">
                    <u-icon label="分享微信好友" label-pos="bottom" size="80" name="/static/images/common/share-friend.png"></u-icon>
                </button>
            </view>
            <view class="coreshop-share-item">
                <button class="coreshop-btn" @click="createPoster()">
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
            }
        },
        data() {
            return {
                providerList: [] // 分享通道 包含生成海报
            }
        },
        mounted() {

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
                        url: 'pages/share/jump/jump',
                        params: {
                            goodsId: this.goodsId
                        },
                        type: 3,//海报
                        client: 2
                    }
                    let userToken = this.$db.get('userToken')
                    if (userToken) {
                        data.token = userToken
                    }
                } else if (this.shareType == 3) {
                    //拼团
                    data = {
                        page: 3, //商品
                        url: 'pages/share/jump/jump',
                        params: {
                            goodsId: this.goodsId,
                            groupId: this.groupId,
                            teamId: this.teamId
                        },
                        type: 3,//海报
                        client: 2
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
            }
        }
    }
</script>
