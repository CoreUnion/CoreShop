<template>
    <view>
        <u-toast ref="uToast" />
        <u-popup class="coreshop-bottom-popup-box" v-if="showLogin" v-model="showLogin" mode="center">
            <view class="radius coreshop-bg-white" @tap.stop style="background: none;overflow: visible;">
                <view class="modal-box">
                    <view class="detail">
                        <view class="shopDesc">
                            <view class="coreshop-avatar sm round margin-left">
                                <u-image width="48rpx" height="48rpx" :src="shopLogo"></u-image>
                            </view>
                            <text class="shopName">
                                {{shopName||'登录授权'}}
                            </text>
                            <text class="get">
                                申请
                            </text>
                        </view>
                        <view class="title3">获取以下权限为您提供服务</view>
                        <view class="desc">
                            1、获取你的手机号提供更好的账户安全，物流，订单状态提醒等服务（在接下来微信授权手机号的弹窗中选择“允许”）<br />
                            2、使用我们的相关服务，需要将您的手机号授权给我们。
                        </view>
                        <!--服务协议-->
                        <view class="u-margin-top-30 u-margin-bottom-30 agreement-checked-view">
                            <u-checkbox v-model="agreement" @change="checkboxChange" size="30" active-color="#19be6b"></u-checkbox>
                            <view class="coreshop-text-black-view">
                                <text class="coreshop-text-black">同意</text>
                                <text class="text-blue" @tap="goUserAgreementPage">《服务协议》</text>
                                <text class="coreshop-text-black">与</text>
                                <text class="text-blue" @tap="goUserPrivacyPolicy">《隐私协议》</text>
                            </view>
                        </view>
                    </view>
                    <view class="u-flex u-row-between u-padding-left-30 u-padding-right-30">
                        <u-button @click="closeAuth">暂不授权</u-button>
                        <u-button type="success" :disabled="isDisabled" v-if="isDisabled">确定授权</u-button>
                        <u-button type="success" open-type="getPhoneNumber" @getphonenumber="getPhoneNumber" v-else>确定授权</u-button>
                    </view>
                </view>
            </view>
        </u-popup>
    </view>
</template>

<script>
    /**
     * 登录提示页
     * @property {Boolean} value=showLoginTip - 由v-model控制显示隐藏。
     * @property {Boolean} forceOauth - 小程序端特制的全屏登录提示。
     */
    import { mapMutations, mapActions, mapState } from 'vuex';
    import { goods, articles, commonUse, tools } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [goods, articles, commonUse, tools],
        name: 'coreshopLoginModal',
        components: {},
        data() {
            return {
                agreement: false,
                isDisabled: true,
            };
        },
        props: {
            value: {},
            modalType: {
                type: String,
                default: ''
            }
        },
        computed: {
            ...mapState({
                showLoginTip: state => state.showLoginTip,
                sessionAuthId: state => state.sessionAuthId,
                hasLogin: state => state.hasLogin,
            }),
            shopLogo() {
                return this.$store.state.config.shopLogo
            },
            shopName() {
                return this.$store.state.config.shopName;
            },
            showLogin: {
                get() {
                    return this.showLoginTip;
                },
                set(val) {
                    this.$store.commit('showLoginTip', val);
                }
            },
            sessionAuthIdTool: {
                get() {
                    return this.sessionAuthId;
                },
                set(val) {
                    this.$store.commit('sessionAuthId', val);
                }
            }
        },
        mounted() {
            const _this = this
            // #ifdef MP-WEIXIN
            var userInfo = this.$store.state.userInfo;
            //var token = this.$db.get('userToken');
            if (Object.keys(userInfo).length != 0) {
                console.log("获取到store.state用户数据");
            } else {
                _this.doToken();
            }
            // #endif
        },
        //watch: {
        //    'hasLogin': {
        //        handler(newVal) {
        //            console.log(newVal);
        //            if (newVal == false) {
        //                console.log("watch监听");
        //                this.doToken();
        //            }
        //        },
        //        deep: true,
        //        immediate: true,
        //    }
        //},
        methods: {
            doToken() {
                const _this = this
                console.log("重新获取用户数据");
                _this.getCode(function (code) {
                    var data = {
                        code: code
                    }
                    _this.$u.api.onLogin(data).then(res => {
                        if (res.status) {
                            if (res.data.auth) {
                                _this.$db.set('userToken', res.data.auth.token)
                                _this.$store.commit('hasLogin', true);
                            }
                            if (res.data.user) {
                                _this.$store.commit('userInfo', res.data.user);
                            }
                            _this.sessionAuthIdTool = res.otherData;
                            console.log("成功后获取sessionAuthIdTool：" + _this.sessionAuthIdTool);
                        } else {
                            _this.sessionAuthIdTool = res.otherData;
                            console.log("失败后获取sessionAuthIdTool：" + _this.sessionAuthIdTool);
                        }
                    })
                })
            },
            // 勾选版权协议
            checkboxChange(e) {
                this.agreement = e.value;
                if (e.value == true) {
                    this.isDisabled = false;
                } else {
                    this.isDisabled = true;
                }
                console.log(this.agreement);
            },
            // 隐藏登录弹窗
            hideModal() {
                this.showLogin = false;
            },
            // 小程序，取消登录
            closeAuth() {
                this.showLogin = false;
                this.$u.toast('您已取消授权')
            },
            getCode: function (callback) {
                let _this = this
                uni.login({
                    // #ifdef MP-ALIPAY
                    scopes: 'auth_user',
                    // #endif
                    success: function (res) {
                        console.log(res);
                        if (res.code) {
                            return callback(res.code)
                        } else {
                            //login成功，但是没有取到code
                            _this.$refs.uToast.show({ title: '未取得code，请重试', type: 'error', })
                        }
                    },
                    fail: function (res) {
                        console.log(res);
                        _this.$refs.uToast.show({ title: '用户授权失败wx.login，请重试', type: 'error', })
                    }
                })
            },
            toLogin: function (data) {
                let _this = this
                _this.$u.api.loginByDecodeEncryptedData(data).then(res => {
                    if (res.status) {
                        //判断是否返回了token，如果没有，就说明没有绑定账号，跳转到绑定页面
                        if (res.data.token) {
                            //登陆成功，设置token，并返回上一页
                            _this.$db.set('userToken', res.data.token)
                            _this.$store.commit('hasLogin', true);
                            _this.$refs.uToast.show({ title: '登录成功', type: 'success', })
                            return false
                        } else {
                            _this.sessionAuthIdTool = res.data.sessionAuthId;
                        }
                    } else {
                        _this.$refs.uToast.show({ title: '登录失败，请重试', type: 'error', })
                    }
                })
            },
            async getPhoneNumber(e) {
                let _this = this;
                if (e.mp.detail.errMsg == 'getPhoneNumber:ok') {
                    var data = {
                        sessionAuthId: _this.sessionAuthIdTool,
                        iv: e.mp.detail.iv,
                        encryptedData: e.mp.detail.encryptedData,
                    }
                    //有推荐码的话，带上
                    var invitecode = _this.$db.get('invitecode')
                    if (invitecode) {
                        data.invitecode = invitecode
                    }
                    _this.toGetPhoneNumber(data);
                }
                else if (e.mp.detail.errMsg == 'getPhoneNumber:fail user deny') {
                    _this.$u.toast('您已经取消了授权，将无法进行关键业务功能。');
                }
                else {
                    _this.$u.toast('如未授权，您可尝试使用手机号+短信验证码登录');
                }
                _this.agreement = false;
                _this.isDisabled = true;
                _this.showLogin = false;
            },
            //实际的去登陆
            toGetPhoneNumber: function (data) {
                let _this = this
                _this.$u.api.loginByGetPhoneNumber(data).then(res => {
                    console.log(res);
                    if (res.status) {
                        //判断是否返回了token，如果没有，就说明没有绑定账号，跳转到绑定页面
                        if (res.data.token) {
                            //console.log("登陆成功，设置token，并返回上一页");
                            //登陆成功，设置token，并返回上一页
                            _this.$db.set('userToken', res.data.token)
                            _this.$store.commit('hasLogin', true);
                            _this.showLogin = false;
                            _this.$refs.uToast.show({ title: '登录成功', type: 'success', })
                            return false
                        }
                    } else if (res.status == false && res.code == 500) {
                        console.log("sessionId不正确，重载");
                        _this.$u.route({ type: 'switchTab', url: '/pages/index/default/default' });
                    } else {
                        _this.$u.toast('登录失败，请重试')
                    }
                })
            }
        }
    };
</script>

<style lang="scss">
    .modal-box { width: 610rpx; border-radius: 20rpx; background: #fff; position: relative; left: 50%; transform: translateX(-50%); padding: 30rpx; z-index: 11111;
        .head-bg { width: 100%; height: 210rpx; }
        .detail { width: 100%; text-align: center;
            .title1 { color: #46351b; font-size: 35rpx; font-weight: bold; }
            .title2 { font-size: 28rpx; color: #999; padding-top: 20rpx; }
            .title3 { color: #46351b; font-size: 35rpx; font-weight: bold; text-align: left; line-height: 35rpx; padding: 30rpx 0 30rpx 30rpx; }
            .desc { font-size: 24rpx; line-height: 40rpx; color: #333; background: #f7f8fa; text-align: left; padding: 20rpx; }
            .user-avatar { width: 160rpx; height: 160rpx; overflow: hidden; margin: 0 auto; margin-bottom: 40rpx; }
            .user-name { font-size: 35rpx; font-family: PingFang SC; font-weight: bold; color: #845708; margin-bottom: 30rpx; }
        }
    }
    .shopDesc { padding: 30rpx 0rpx 0rpx 0rpx; text-align: left;
        .shopName { margin-left: 20rpx; line-height: 40rpx; }
        .get { margin-left: 20rpx; line-height: 40rpx; }
    }
    .agreement-checked-view { position: relative; padding: 18.18rpx 0rpx 18.18rpx 30rpx; display: flex; align-items: center; margin: 10rpx 0; font-size: 24rpx;
        .coreshop-checked { transform: scale(0.7); }
    }
</style>
