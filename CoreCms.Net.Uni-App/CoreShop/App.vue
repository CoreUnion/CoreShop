<script>
    import Vue from 'vue'
    export default {
        onLaunch() {
            //版本设置
            const version = '1.5.4'
            // 开发环境才提示，生产环境不会提示
            if (process.env.NODE_ENV === 'development') {
                console.log(`\n %c \u6838\u5fc3\u5546\u57ce\u7cfb\u7edf\u0020\u0043\u006f\u0072\u0065\u0053\u0068\u006f\u0070 V${version} %c \u0068\u0074\u0074\u0070\u0073\u003a\u002f\u002f\u0077\u0077\u0077\u002e\u0063\u006f\u0072\u0065\u0073\u0068\u006f\u0070\u002e\u0063\u006e\u002f \n\n`, 'color: #ffffff; background: #3c9cff; padding:5px 0;', 'color: #3c9cff;background: #f1f1f1; padding:5px 0;');
            }
            // #ifdef MP-WEIXIN
            this.autoUpdate();
            // #endif

            // #ifdef APP-PLUS || APP-PLUS-NVUE
            this.checkVersion()
            // #endif

            // 获取店铺配置信息  全局只请求一次
            this.$u.api.shopConfigV2().then(res => {
                this.$store.commit('config', res.data)
                // #ifdef H5
                //百度统计
                if (res.data.statistics) {
                    var script = document.createElement("script");
                    script.innerHTML = res.data.statistics;
                    document.getElementsByTagName("body")[0].appendChild(script);
                }
                // #endif
            })
            //获取三级联动城市信息
            this.$u.api.getAreaList().then(res => {
                if (res.status) {
                    this.$db.set('areaList', res.data)
                }
            });

        },
        onShow: function (obj) {
            // #ifdef MP-WEIXIN
            this.$store.commit('scene', obj.scene)
            console.log(obj);
            // #endif
            //console.log('App Show')
        },
        onHide: function () {
            //console.log('App Hide')
        },
        methods: {
            // #ifdef MP-WEIXIN
            //微信小程序检测更新措施，方式更新功能后，要等待24小时内才刷新的问题。
            autoUpdate() {
                var self = this
                // 获取小程序更新机制兼容
                if (wx.canIUse('getUpdateManager')) {
                    //console.log("进入小程序自动更新检测");
                    const updateManager = wx.getUpdateManager()
                    //1. 检查小程序是否有新版本发布
                    updateManager.onCheckForUpdate(function (res) {
                        //console.log("进入小程序检测是否需要自动更新");
                        //console.log(res);
                        // 请求完新版本信息的回调
                        if (res.hasUpdate) {
                            //检测到新版本，需要更新，给出提示
                            wx.showModal({
                                title: '更新提示',
                                content: '检测到新版本，是否下载新版本并重启小程序？',
                                success: function (res) {
                                    if (res.confirm) {
                                        //2. 用户确定下载更新小程序，小程序下载及更新静默进行
                                        self.downLoadAndUpdate(updateManager)
                                    } else if (res.cancel) {
                                        //用户点击取消按钮，需要强制更新，二次弹窗
                                        wx.showModal({
                                            title: '温馨提示~',
                                            content: '本次版本更新涉及到新的功能添加，旧版本无法正常访问的哦~',
                                            showCancel: false,
                                            confirmText: "确定更新",
                                            success: function (res) {
                                                if (res.confirm) {
                                                    //下载新版本，并重新应用
                                                    self.downLoadAndUpdate(updateManager)
                                                }
                                            }
                                        })
                                    }
                                }
                            })
                        }
                    })
                } else {
                    // 如果希望用户在最新版本的客户端上体验您的小程序，可以这样子提示
                    wx.showModal({
                        title: '提示',
                        content: '当前微信版本过低，无法使用该功能，请升级到最新微信版本后重试。'
                    })
                }
            },
            downLoadAndUpdate(updateManager) {
                var self = this
                wx.showLoading();
                //静默下载更新小程序新版本
                updateManager.onUpdateReady(function () {
                    wx.hideLoading()
                    //新的版本已经下载好，调用 applyUpdate 应用新版本并重启
                    updateManager.applyUpdate()
                })
                updateManager.onUpdateFailed(function () {
                    // 新的版本下载失败
                    wx.showModal({
                        title: '已经有新版本了哟~',
                        content: '新版本已经上线啦~，请您删除当前小程序，重新搜索打开哟~',
                    })
                })
            },
            // #endif
            // #ifdef APP-PLUS || APP-PLUS-NVUE
            // app更新检测
            checkVersion() {
                // 获取应用版本号
                let version = plus.runtime.version;
                //检测当前平台，如果是安卓则启动安卓更新
                uni.getSystemInfo({
                    success: res => {
                        this.updateHandler(res.platform, version);
                    }
                })
            },
            // 更新操作
            updateHandler(platform, version) {
                let data = {
                    platform: platform,
                    version: version
                }
                let _this = this;
                this.$u.api.getAppVersion(data).then(res => {
                    if (res.status && res.data[0].version) {
                        const info = res.data[0];
                        if (info.version !== '' && info.version > version) {
                            uni.showModal({
                                //提醒用户更新
                                title: '更新提示',
                                content: info.note,
                                success: res => {
                                    if (res.confirm) {
                                        plus.runtime.openURL(info.download_url)
                                    }
                                }
                            })
                        }
                    }
                });
            }
            // #endif
        }
    }
</script>
<style lang="scss">
    /*加载UViewUI*/
    @import "uview-ui/index.scss";
    /*公共css */
    @import 'static/style/coreCommon.scss';
</style>