<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="订单评价"></u-navbar>
        <view class="content">
            <view class="coreshop-content-top">
                <view class='img-list'>
                    <view v-for="item in info.items" :key="item.id">
                        <view class="coreshop-bg-white coreshop-card-box u-margin-20">
                            <view class="coreshop-card-view coreshop-order-view">
                                <view class="u-font-md coreshop-text-bold coreshop-text-black">商品信息</view>
                                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                                <view class="orderList">
                                    <view class="item" @click="goGoodsDetail(item.goodsId)">
                                        <view class="left"><image :src="item.imageUrl && item.imageUrl!='null' ?  item.imageUrl : '/static/images/common/empty-banner.png'" mode="aspectFill"></image></view>
                                        <view class="content">
                                            <view class="title u-line-4">{{item.name}}</view>
                                        </view>
                                    </view>
                                </view>
                            </view>
                            <view class="coreshop-card-view coreshop-order-view">
                                <view class="u-font-md coreshop-text-bold coreshop-text-black">商品评分</view>
                                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                                <view>
                                    <u-rate v-model="score[item.id]" @change="changeScore"></u-rate>
                                </view>
                            </view>
                            <view class="coreshop-card-view coreshop-order-view">
                                <view class="u-font-md coreshop-text-bold coreshop-text-black">评价内容</view>
                                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                                <view class="evaluate-c-t">
                                    <textarea v-model="textarea[item.id]" placeholder="宝贝满足你的期待吗? 说说你的使用心得" />
                                </view>
                            </view>
                            <view class="coreshop-card-view coreshop-order-view">
                                <view class="u-font-md coreshop-text-bold coreshop-text-black">上传图片（可上传{{maxUploadImg}}张）</view>
                                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                                <view class="evaluate-c-b">
                                    <view class="goods-img-item" v-if="images[item.id].length" v-for="(img, key) in images[item.id]" :key="key">
                                        <image class="del" src="/static/images/common/del.png" mode="" @click="removeImg(item.id, key)"></image>
                                        <image class="" :src="img" mode="" @click="clickImg(img)"></image>
                                    </view>
                                    <view class="upload-img" v-show="isupload[item.id]">
                                        <image class="icon" src="/static/images/common/camera.png" mode="" @click="uploadImg(item.id)"></image>
                                        <view class="">上传照片</view>
                                    </view>
                                </view>
                            </view>
                        </view>
                    </view>
                </view>
            </view>
            <view class="coreshop-foot-hight-view" />
            <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
                <view class="u-padding-20">
                    <u-button :custom-style="customStyle" type="error" size="medium" @click="toEvaluate" :disabled='submitStatus' :loading='submitStatus'>提交评论</u-button>
                </view>
            </view>
        </view>
    </view>
</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [goods],
        data() {
            return {
                customStyle: {
                    width: '100%',
                },
                orderId: 0,
                info: {}, // 订单详情
                images: [],
                score: [], // 商品评价
                textarea: [], // 商品评价信息
                isupload: [], // 启/禁用 图片上传按钮
                rate: 5,
                submitStatus: false
            }
        },
        onLoad(options) {
            this.orderId = options.orderId
            if (this.orderId) {
                this.orderInfo();
            } else {
                this.$refs.uToast.show({ title: '参数获取失败', type: 'error', back: true });
            }
        },
        computed: {
            // 获取vuex中状态
            maxUploadImg() {
                return this.$store.state.config.imageMax
            }
        },
        methods: {
            // 获取订单详情
            orderInfo() {
                let data = {
                    id: this.orderId
                }
                this.$u.api.orderDetail(data).then(res => {
                    if (res.status && res.data.payStatus >= 2 && res.data.shipStatus >= 3 && res.data.confirmStatus >= 2 && res.data.isComment === false) {
                        const _info = res.data
                        let images = []
                        let textarea = []
                        let upload = []
                        let score = []
                        _info.items.forEach(item => {
                            images[item.id] = []
                            textarea[item.id] = ''
                            upload[item.id] = true
                            score[item.id] = 5
                        })
                        this.info = _info
                        this.images = images
                        this.textarea = textarea
                        this.score = score
                        this.isupload = upload
                    } else {
                        this.$u.toast('订单不存在或状态不可评价!')
                    }
                })
            },
            // 上传图片
            uploadImg(key) {
                this.$upload.uploadFiles(null, res => {
                    if (res.status) {
                        //this.images[key].push(res.data.src + '?x-oss-process=image/resize,m_lfit,h_200,w_200')
                        this.images[key].push(res.data.src)
                        this.$refs.uToast.show({ title: res.msg, type: 'success', back: false })
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
            // 删除图片
            removeImg(id, key) {
                this.images[id].splice(key, 1)
            },
            // 图片点击放大
            clickImg(img) {
                // 预览图片
                uni.previewImage({
                    urls: img.split()
                });
            },
            // 改变评分
            changeScore(e) {
                this.score[e.id] = e.value
            },
            // 提交评价
            toEvaluate() {
                this.submitStatus = true;
                let items = [];
                let _this = this;

                this.images.forEach((item, key) => {
                    let model = {
                        orderItemId: key,
                        images: item,
                        score: _this.score[key],
                        textarea: _this.textarea[key]
                    }
                    items.push(model);
                })
                let data = {
                    orderId: _this.orderId,
                    items: items
                }
                this.$u.api.orderEvaluate(data).then(res => {
                    if (res.status) {
                        _this.$refs.uToast.show({
                            title: '评价填写成功', type: 'success', callback: function () {
                                // 更改订单列表页的订单状态
                                let pages = getCurrentPages(); // 当前页
                                let beforePage = pages[pages.length - 2]; // 上个页面

                                if (beforePage !== undefined && beforePage.route === 'pages/member/order/index/index') {
                                    beforePage.$vm.isReload = true
                                }

                                let before = pages[pages.length - 3]; // 上个页面
                                if (before !== undefined && before.route === 'pages/member/order/index/index') {
                                    before.$vm.isReload = true
                                }
                                uni.navigateBack({
                                    delta: 1,
                                    animationType: 'pop-out',
                                    animationDuration: 200
                                });
                            }
                        })
                    } else {
                        _this.$u.toast(res.msg)
                    }
                });
            }
        },
        watch: {
            images: {
                handler() {
                    this.images.forEach((item, key) => {
                        this.isupload[key] = item.length >= this.maxUploadImg ? false : true
                    })
                },
                deep: true
            }
        }
    }
</script>

<style lang="scss" scoped>
    @import "evaluate.scss";
</style>
