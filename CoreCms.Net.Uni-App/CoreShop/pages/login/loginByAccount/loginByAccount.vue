<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="账户登录"></u-navbar>
        <view class="content">
            <view class="login-t"><image class="login-logo" :src="logoImage" mode="aspectFill"></image></view>
            <view>
                <view class="login-m">
                    <view class="login-item">
                        <input type="number" v-model="mobile" :maxlength="maxMobile" placeholder="请输入手机号码" placeholder-class="login-item-i-p fsz26" />
                    </view>
                    <view class="login-item flc">
                        <input class="login-item-input" :password="true" placeholder-class="login-item-i-p fsz26" type="text" v-model="pwd" placeholder="请输入密码" />
                    </view>
                    <view class="login-item" v-if="isCaptcha">
                        <input class="login-item-input" placeholder-class="login-item-i-p fsz26" type="text" v-model="captcha" placeholder="输入验证码" />
                        <img class="codeimg" :src="captchaUrl" alt="" />
                    </view>
                    <view class="goforgetpwd color-9 fsz24" @click="goForgetpwd()">忘记密码？</view>
                </view>
                <view class="login-b">
                    <button :class="loginButtonClass" @click="loginHandler" hover-class="coreshop-btn-hover">登录</button>
                    <view class="color-9 fsz24 agreement">
                        登录即代表你同意
                        <text @click="goUserAgreementPage()" class="color-o">用户协议</text>
                        和
                        <text @click="goUserPrivacyPolicy()" class="color-o">隐私政策</text>
                    </view>
                    <view class="login-other flc">
                        <view class="fz12 item" @click="selectLoginType">验证码登录</view>
                    </view>
                </view>
            </view>
            <!--  微信浏览器里 -->
            <!-- #ifdef H5 -->
            <template v-if="weixinBrowser">
                <view class="fz12 g5">第三方账号登录：</view>
                <view class="flc third-block">
                    <view class="third-item" v-for="(item, key, index) in thirdPartyLogins" :key="index" @click="handleThirdLogin(item)">
                        <image class="third-item-img" :src="getThirdLoginImg(key)" mode="aspectFill"></image>
                    </view>
                </view>
            </template>
            <!-- #endif -->
            <!-- #ifdef APP-PLUS || APP-PLUS-NVUE -->
            <view class="fz12 g5">第三方账号登录：</view>
            <view class="flc third-block" v-if="thirdPartyLogins.length > 0">
                <view class="third-item" v-for="(item, key, index) in thirdPartyLogins" :key="key" @click="handleThirdLoginApp(item)">
                    <image class="third-item-img" src="/static/images/ic-wechat.png" mode="aspectFill" v-if="item == 'weixin'"></image>
                </view>
            </view>
            <!-- #endif -->
        </view>
    </view>
</template>

<script>
    import { baseUrl } from '@/common/setting/constVarsHelper.js';
    import { commonUse, articles } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [commonUse, articles],
        data() {
            return {
                maxMobile: 11,
                mobile: '', // 手机号
                pwd: '', // 密码
                isCaptcha: false, // 是否需要验证码
                captcha: '', // 输入的验证码
                captchaUrl: '', // 验证码图片地址
                btnb: 'coreshop-btn coreshop-btn-square coreshop-btn-c coreshop-btn-all', // 按钮bg
                weixinBrowser: false, // 是否是微信浏览器
                thirdPartyLogins: [] // 第三方登录列表
            };
        },
        onLoad(options) {
            if (options.invitecode) {
                this.$db.set('invitecode', options.invitecode);
            }
            // 判断浏览器环境
            this.weixinBrowser = this.$common.isWeiXinBrowser();

            if (this.weixinBrowser) {
                this.getAuths();
            }
            // #ifdef APP-PLUS || APP-PLUS-NVUE
            this.getAppAuths();
            // #endif
        },
        onShow() {
            //if (this.$db.get('userToken')) {
            //    this.$u.route({
            //        type: 'switchTab',
            //        url: '/pages/index/default/default'
            //    });
            //}
        },
        computed: {
            // 动态更改登录按钮bg
            loginButtonClass() {
                return this.mobile && this.mobile.length === 11 && this.pwd ? this.btnb + ' coreshop-btn-b' : this.btnb;
            },
            logoImage() {
                return this.$store.state.config.shopLogo;
            },
            getThirdLoginImg(key) {
                return key => {
                    if (key == 'Wxofficial') {
                        return '/static/images/ic-wechat.png';
                    } else if (key == 'weixin') {
                        return '/static/images/ic-wechat.png';
                    }
                };
            }
        },
        methods: {
            // 验证手机号
            rightMobile() {
                let res = {};
                if (!this.mobile) {
                    res.status = false;
                    res.msg = '请输入手机号';
                } else if (!/^1[3456789]{1}\d{9}$/gi.test(this.mobile)) {
                    res.status = false;
                    res.msg = '手机号格式不正确';
                } else if (!this.pwd) {
                    res.status = false;
                    res.msg = '请输入密码';
                } else {
                    res.status = true;
                }
                return res;
            },
            // 登录处理
            loginHandler() {
                if (this.mobile && this.mobile.length === 11 && this.pwd) {
                    if (!this.rightMobile().status) {
                        this.$u.toast(this.rightMobile().msg);
                    } else {
                        this.toLogin();
                    }
                }
            },
            // 获取验证码图片地址
            getCaptchaUrl() {
                this.captchaUrl = this.$config.apiBaseUrl + 'captcha.html';
            },
            // 去登录
            toLogin() {
                let _this = this;
                let data = {
                    mobile: this.mobile,
                    password: this.pwd
                };

                if (this.isCaptcha) {
                    data.captcha = this.captcha;
                }

                // 获取邀请码
                let invicode = this.$db.get('invitecode');
                if (invicode) {
                    data.invitecode = invicode;
                }

                this.$u.api.login(data).then(res => {
                    if (res.status) {
                        this.$db.set('userToken', res.data);
                        this.redirectHandler();
                    } else {

                        _this.$refs.uToast.show({
                            title: res.msg, type: 'error', back: true, callback: function () {
                                // 需要输入验证码 或者 验证码错误刷新
                                if (res.data === 10013 || res.data === 10012) {
                                    _this.isCaptcha = true;
                                }
                                // 登录需要验证码
                                if (_this.isCaptcha) {
                                    _this.getCaptchaUrl();
                                }
                            }
                        })
                    }
                });
            },
            // 重定向跳转 或者返回上一个页面
            redirectHandler() {
                this.$db.del('invitecode');
                this.toLoginSuccessHandleBack();
            },
            // 登录方式切换
            selectLoginType() {
                this.$u.route({ type: 'redirectTo', url: '/pages/login/loginBySMS/loginBySMS' });
            },
            // 获取第三方登录列表
            getAuths() {
                let data = {
                    url: baseUrl + 'wap/pages/login/author/author'
                };
                this.$u.api.getTrustLogin(data).then(res => {
                    if (res.status) {
                        this.thirdPartyLogins = res.data;
                    }
                });
            },
            // 第三方登录授权
            handleThirdLogin(url) {
                this.$u.route({ type: 'redirectTo', url: '' });
                let redirect = this.$store.state.redirectPage;
                this.$db.set('redirectPage', redirect);
                window.location.href = url;
            },
            //获取APP信任登录
            getAppAuths() {
                let _this = this;
                _this.thirdPartyLogins = [];
                uni.getProvider({
                    service: 'oauth',
                    success: function (res) {
                        if (res.errMsg == 'getProvider:ok') {
                            _this.thirdPartyLogins = res.provider;
                        }
                    }
                });
            },
            //app第三方登录
            handleThirdLoginApp(type) {
                uni.showLoading({
                    title: '加载中'
                });
                let _this = this;
                uni.login({
                    provider: type,
                    success: function (loginRes) {
                        // 获取用户信息
                        uni.getUserInfo({
                            provider: type,
                            success: function (infoRes) {
                                if (infoRes.errMsg == 'getUserInfo:ok') {
                                    var data = {
                                        user: infoRes.userInfo,
                                        type: type
                                    };
                                    var invitecode = _this.$db.get('invitecode');
                                    if (invitecode) {
                                        data.invitecode = invitecode;
                                    }
                                    _this.$u.api.appTrustLogin(data).then(res => {
                                        uni.hideLoading();
                                        if (res.status) {
                                            //判断是否返回了token，如果没有，就说明没有绑定账号，跳转到绑定页面
                                            if (typeof res.data.token == 'undefined') {
                                                _this.$u.route({ type: 'redirectTo', url: '/pages/login/loginBySMS/loginBySMS?sessionAuthId=' + res.data.sessionAuthId });
                                            } else {
                                                //登陆成功，设置token，并返回上一页
                                                _this.$db.set('userToken', res.data.token);
                                                uni.navigateBack({
                                                    delta: 1
                                                });
                                                return false;
                                            }
                                        } else {
                                            _this.$u.toast('登录失败，请重试');
                                        }
                                    });
                                } else {
                                    uni.hideLoading();
                                    _this.$u.toast('登录失败，请重试');
                                }
                            }
                        });
                    }
                });
            },
            goForgetpwd() {
                this.$u.route('/pages/login/forget/forget');
            }
        }
    };
</script>

<style lang="scss" scoped>
    view { box-sizing: border-box; }
    .content { /*  #ifdef  H5  */ height: calc(100vh - 90upx); /*  #endif  */ /*  #ifndef  H5  */ height: 100vh; /*  #endif  */ background-color: #fff; padding: 0upx 100upx; }
    .login-t { text-align: center; padding: 50upx 0; }
    .login-logo { width: 180upx; height: 180upx; border-radius: 20upx; background-color: #f8f8f8; }
    .login-m { margin-bottom: 100upx; }
    .login-item { border-bottom: 2upx solid #d0d0d0; overflow: hidden; padding: 10upx; font-size: 28upx; color: #333; margin-bottom: 30upx; display: flex; align-items: center; }
    .login-item-input { display: inline-block; flex: 1; box-sizing: border-box; }
    .codeimg { width: 210rpx; }
    .login-item .coreshop-btn { display: inline-block; font-size: 28upx; border: none; width: 40%; padding: 0; line-height: 1.7; float: right; }
    .login-b .coreshop-btn { color: #999; }
    .coreshop-btn-b { color: #fff !important; }
    .registered-item { overflow: hidden; width: 100%; }
    .registered { float: right; }
    .registered-item .coreshop-btn-square { color: #333; }
    .third-block { justify-content: center; padding-top: 40upx; }
    .third-item { width: 80upx; height: 80upx; background: $g2; border-radius: 50%; padding: 16upx; }
        .third-item img { display: block; width: 100%; height: 100%; }

    .login-other { margin-bottom: 40upx; }
    .login-othe .item { padding: 20upx 0; }


    .agreement { margin: 20rpx 0; text-align: center; }
        .agreement .color-o { margin: 0 10rpx; }

    .goforgetpwd { width: 100%; text-align: right; }
</style>
