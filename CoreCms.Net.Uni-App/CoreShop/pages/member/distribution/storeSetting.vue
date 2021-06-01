<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="店铺设置"></u-navbar>

        <form>
            <view class="cu-form-group">
                <view class="title">名称</view>
                <input placeholder='请输入店铺名称' v-model="storeName"></input>
            </view>
            <view class="cu-form-group">
                <view class="title">图标</view>
                <image class='cu-avatar radius bg-gray' mode="aspectFill" :src="logo" @click="uploadLogo"></image>
            </view>

            <view class="cu-bar bg-white margin-top">
                <view class="action">
                    店招
                </view>
                <view class="action">
                    {{images.length}}/{{imageMax}}
                </view>
            </view>
            <view class="cu-form-group">
                <view class="grid col-4 grid-square flex-sub">
                    <view class="bg-img" v-for="(item,index) in images" :key="index" @tap="clickImg(item.src)" :data-url="images[index]">
                        <image :src="item.src" mode="aspectFill"></image>
                        <view class="cu-tag bg-red" @tap.stop="delImage(item)" :data-index="index">
                            <text class='cuIcon-close'></text>
                        </view>
                    </view>
                    <view class="solids" @tap="upImage" v-if="images.length<imageMax">
                        <text class='cuIcon-cameraadd'></text>
                    </view>
                </view>
            </view>
            <view class="cu-form-group align-start">
                <view class="title">简介</view>
                <textarea v-model="storeDesc" placeholder="请您在此描述问题(最多200字)" maxlength="200"></textarea>
            </view>
        </form>

        <view class="bg-white coreshop-footer-fixed coreshop-foot-padding-bottom" @click="submitHandler()">
            <view class="flex padding-sm flex-direction">
                <button class="cu-btn bg-red">保存</button>
            </view>
        </view>

    </view>

</template>

<script>
    export default {
        data() {
            return {
                title: 'picker',
                logo: '',
                images: [],
                imageMax: 1,
                storeName: '',//店铺名称
                storeLogo: '',
                storeBanner: '',
                storeDesc: '',//店铺介绍
            }
        },
        computed: {
            isImage() {
                let num = this.imageMax - this.images.length;
                if (num > 0) {
                    return true;
                } else {
                    return false;
                }
            }
        },
        methods: {
            // 用户上传头像
            uploadLogo() {
                this.$upload.uploadFiles(null, res => {
                    if (res.status) {
                        this.storeLogo = res.data.src;
                        this.logo = res.data.src;
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
            // 保存资料
            submitHandler() {
                if (!this.storeName || this.storeName == '') {
                    this.$u.toast('请填写店铺名称');
                    return;
                }
                if (this.images.length <= 0) {
                    this.$u.toast('请上传店招');
                    return;
                }
                if (!this.storeLogo) {
                    this.$u.toast('请上传图标');
                    return;
                }
                this.storeBanner = this.images[0].src;

                this.$u.api.setDistributionStore({
                    storeName: this.storeName,
                    storeLogo: this.storeLogo,
                    storeBanner: this.storeBanner,
                    storeDesc: this.storeDesc
                }).then(res => {
                    if (res.status) {
                        this.$refs.uToast.show({ title: res.msg, type: 'success', back: false })
                    } else {
                        this.$u.toast(res.msg);
                    }
                }
                );
            },
            //上传图片
            upImage() {
                let num = this.imageMax - this.images.length;
                if (num > 0) {
                    this.$upload.uploadImage(num, res => {
                        if (res.status) {
                            this.images.push(res.data);
                            this.$refs.uToast.show({ title: res.msg, type: 'success' });
                        } else {
                            this.$u.toast(res.msg);
                        }
                    });
                }
            },
            //删除图片
            delImage(e) {
                let newImages = [];
                for (var i = 0; i < this.images.length; i++) {
                    if (this.images[i].imageId != e.imageId) {
                        newImages.push(this.images[i]);
                    }
                }
                this.images = newImages;
            },
            // 图片点击放大
            clickImg(img) {
                // 预览图片
                uni.previewImage({
                    urls: img.split()
                });
            }
        },
        onLoad: function () {
            var _this = this;
            _this.$u.api.getDistributionInfo({ check_condition: false }).then(res => {
                if (res.status) {
                    _this.storeName = res.data.storeName;
                    _this.storeDesc = res.data.storeDesc;
                    _this.storeLogo = res.data.storeLogo;
                    if (res.data.storeLogo) {
                        _this.logo = res.data.storeLogo;
                    }
                    _this.storeBanner = res.data.storeBanner;
                    if (_this.storeBanner) {
                        _this.images.push({
                            imageId: res.data.storeBanner,
                            url: res.data.storeBanner,
                            src: res.data.storeBanner
                        });
                    }
                } else {
                    //报错了
                    _this.$u.toast(res.msg);
                }
            });
        }
    }
</script>

<style lang="scss">
    .cu-form-group .title { min-width: calc(4em + 15px); }

    .user-head { height: 100upx; }
    .user-head-img { height: 90upx; width: 90upx; border-radius: 50%; }
    .cell-hd-title { color: #333; }
    .cell-item-bd { color: #666; font-size: 26upx; }
    .list-goods-name { width: 100% !important; }
    .cart-checkbox-item { position: relative; }
    .invoice-type .uni-list-cell { display: inline-block; font-size: 26upx; color: #333; position: relative; margin-left: 50upx; }
        .invoice-type .uni-list-cell > view { display: inline-block; }
    .invoice-type-icon { position: absolute; top: 50%; transform: translateY(-50%); }
    .invoice-type-c { margin-left: 50upx; line-height: 2; }
    .cell-item-ft .cell-bd-input { text-align: right; width: 500upx; font-size: 28upx; }
    .right-img { border-bottom: 0; }
    .cell-textarea { padding: 0 26upx 20upx; }
        .cell-textarea textarea { width: 100%; height: 200upx; font-size: 26upx; color: #333; }
    .evaluate-c-b { overflow: hidden; padding: 0 20upx; }
    .upload-img { width: 146upx; height: 146upx; margin: 14upx; text-align: center; color: #999999; font-size: 22upx; border: 2upx solid #E1E1E1; border-radius: 4upx; display: inline-block; float: left; padding: 24upx 0; }
    .goods-img-item { width: 174upx; height: 174upx; padding: 14upx; float: left; position: relative; }
        .goods-img-item:nth-child(4n) { margin-right: 0; }
        .goods-img-item image { width: 100%; height: 100%; }
    .del { width: 30upx !important; height: 30upx !important; position: absolute; right: 0; top: 0; z-index: 999; }
    .cell-textarea textarea { background-color: #f8f8f8; padding: 12upx 20upx; box-sizing: border-box; }
</style>
