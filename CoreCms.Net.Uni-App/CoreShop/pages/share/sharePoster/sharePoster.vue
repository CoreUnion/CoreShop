<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="分享"></u-navbar>
        <view class="share-top"><img class="share-img" :src="poster" mode="widthFix" /></view>
        <view class="share-bot flex flex-direction">
            <view class="u-margin-bottom-40">
                <u-button shape="square" type="primary" v-if="weiXinBrowser">
                    <u-icon name="download" :margin-right="20" label="长按图片保存到手机" label-color="#fff"></u-icon>
                </u-button>
                <u-button shape="square" type="primary" @click="savePoster()" v-else>
                    <u-icon name="download" :margin-right="20" label="保存到本地" label-color="#fff"></u-icon>
                </u-button>
            </view>
            <u-button shape="square" @click="goBack()">
                <u-icon name="arrow-leftward" :margin-right="20" label="返回" label-color="#333"></u-icon>
            </u-button>
        </view>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>
<script>
    export default {
        data() {
            return {
                poster: ''
            };
        },
        onLoad(options) {
            this.poster = decodeURIComponent(options.poster);
        },
        computed: {
            weiXinBrowser() {
                return this.$common.isWeiXinBrowser()
            }
        },
        methods: {
            goBack() {
                uni.navigateBack({
                    delta: 1
                });
            },
            // 保存海报到本地
            savePoster() {
                let _this = this;
                _this.downloadImageOfMp(_this.poster)
            },
            //下载图片地址和图片名
            downloadIamge(imgsrc, name) {
                var image = new Image();
                // 解决跨域 Canvas 污染问题
                image.setAttribute('crossorigin', 'anonymous');
                image.onload = () => {
                    var canvas = document.createElement('canvas');
                    canvas.width = image.width;
                    canvas.height = image.height;
                    var context = canvas.getContext('2d');
                    context.drawImage(image, 0, 0, image.width, image.height);
                    var url = canvas.toDataURL('image/png'); //得到图片的base64编码数据
                    var a = document.createElement('a'); // 生成一个a元素
                    var event = new MouseEvent('click'); // 创建一个单击事件
                    a.download = name || 'photo'; // 设置图片名称
                    a.href = url; // 将生成的URL设置为a.href属性
                    a.dispatchEvent(event); // 触发a的单击事件
                };
                image.src = imgsrc;
            },
            downloadImageOfMp(image) {
                let _this = this

                // #ifdef APP-PLUS || APP-PLUS-NVUE
                uni.downloadFile({
                    url: image,
                    success(res) {
                        uni.saveImageToPhotosAlbum({
                            filePath: res.tempFilePath,
                            success() {
                                _this.$refs.uToast.show({ title: '操作成功', type: 'success' })
                            },
                            fail() {
                                _this.$u.toast('图片保存失败')
                            }
                        });
                    },
                    fail() {
                        _this.$u.toast('下载失败')
                    }
                })
                // #endif

                // #ifdef MP
                uni.authorize({
                    scope: 'scope.writePhotosAlbum',
                    success() {
                        // 先下载到本地
                        uni.downloadFile({
                            url: image,
                            success(res) {
                                uni.saveImageToPhotosAlbum({
                                    filePath: res.tempFilePath,
                                    success() {
                                        _this.$refs.uToast.show({ title: '保存成功', type: 'success' })
                                    },
                                    fail() {
                                        _this.$u.toast('图片保存失败')
                                    }
                                });
                            },
                            fail() {
                                _this.$u.toast('下载失败')
                            }
                        })
                    },
                    fail() {
                        //console.log('授权失败')
                    }
                })
                // #endif
            }
        }
    };
</script>
<style scoped lang="scss">
    @import "sharePoster.scss";
</style>
