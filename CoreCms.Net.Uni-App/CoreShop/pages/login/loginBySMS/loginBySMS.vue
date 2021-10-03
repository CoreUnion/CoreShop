<template>
    <view class="wrap">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="短信登录"></u-navbar>
        <view v-if="!showCodeBox">
            <view class="top"></view>
            <view class="content">
                <view class="title">欢迎登录{{appTitle}}</view>
                <input class="u-border-bottom" type="number" v-model="mobile" placeholder="请输入手机号" />
                <view class="tips">未注册的手机号验证后自动创建平台账号</view>
                <button @tap="submit" :style="[inputStyle]" class="getCaptcha">获取短信验证码</button>
                <view class="alternative">
                    <!--<view class="password">密码登录</view>-->
                    <view class="issue">遇到问题</view>
                </view>
            </view>
            <view class="buttom">
                <view class="loginType">
                    <!--#ifdef MP-WEIXIN-->
                    <view class="wechat item" @tap="goLoginByGetPhoneNumber()">
                        <view class="icon"><u-icon size="70" name="weixin-fill" color="rgb(83,194,64)"></u-icon></view>
                        微信手机号绑定
                    </view>
                    <!--#endif-->
                </view>
                <view class="hint">
                    登录即代表你同意
                    <text @click="goUserAgreementPage()">用户协议</text>
                    和
                    <text @click="goUserPrivacyPolicy()">隐私政策</text>，
                    并授权使用您的{{appTitle}}账号信息（如昵称、头像、收获地址）以便您统一管理
                </view>
            </view>
        </view>
        <view class="wrapkey" v-if="showCodeBox">
            <view class="key-input">
                <view class="title">输入验证码</view>
                <view class="tips">验证码已发送至 +{{mobile}}</view>
                <u-message-input :focus="true" :value="value" @change="change" @finish="finish" mode="bottomLine" :maxlength="maxlength"></u-message-input>
                <text :class="{ error: error }">{{errorMsg}}</text>
                <view class="captcha">
                    <text :class="{ noCaptcha: verification }" @tap="noCaptcha">收不到验证码点这里</text>
                    <text :class="{ regain: !verification }">{{ timer }}秒后重新获取验证码</text>
                </view>
            </view>
        </view>

    </view>
</template>

<script>
    import { commonUse, articles } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [commonUse, articles],
        data() {
            return {
                maxMobile: 11,
                mobile: '', // 用户手机号
                code: '', // 短信验证码
                sessionAuthId: '', //授权id
                verification: true, // 通过v-show控制显示获取还是倒计时
                timer: 60, // 定义初始时间为60s
                btnb: 'coreshop-btn coreshop-btn-square coreshop-btn-c coreshop-btn-all', //按钮背景
                type: '', // 有值是第三方登录账号绑定
                isWeixinBrowser: this.$common.isWeiXinBrowser(),
                showCodeBox: false,

                maxlength: 6,
                value: '',
                error: false,
                errorMsg: '验证码错误，请重新输入',
            };
        },
        onLoad(option) {
            if (option.sessionAuthId) {
                this.sessionAuthId = option.sessionAuthId;
            }
        },
        computed: {
            logoImage() {
                return this.$store.state.config.shopLogo
            },
            appTitle() {
                return this.$store.state.config.shopName;
            },
            inputStyle() {
                let style = {};
                if (this.mobile) {
                    style.color = "#fff";
                    style.backgroundColor = this.$u.color['warning'];
                }
                return style;
            },
            // 验证手机号
            rightMobile() {
                let res = {};
                if (!this.mobile) {
                    res.status = false;
                    res.msg = '请输入手机号';
                } else if (!this.$u.test.mobile(this.mobile)) {
                    res.status = false;
                    res.msg = '手机号格式不正确';
                } else {
                    res.status = true;
                }
                return res;
            },
            // 动态计算发送验证码按钮样式
            sendCodeBtn() {
                let btn = 'coreshop-btn coreshop-btn-g';
                if (this.mobile.length === this.maxMobile && this.rightMobile.status) {
                    return btn + ' coreshop-btn-b';
                } else {
                    return btn;
                }
            },
            // 动态更改登录按钮bg
            regButtonClass() {
                return this.mobile && this.mobile.length === this.maxMobile && this.code ? this.btnb + ' coreshop-btn-b' : this.btnb;
            },
            logoImage() {
                return this.$store.state.config.shopLogo;
            }
        },
        onShow() {
            let _this = this;
            let userToken = _this.$db.get('userToken');
            if (userToken) {
                this.$u.route({
                    type: 'switchTab',
                    url: '/pages/index/member/member'
                });
                return true;
            }
            _this.timer = parseInt(_this.$db.get('timer'));
            if (_this.timer != null && _this.timer > 0) {
                _this.countDown();
                _this.verification = false;
            }
        },
        methods: {
            goLoginByGetPhoneNumber() {
                this.$u.route({ type: 'redirectTo', url: '/pages/login/loginByGetPhoneNumber/loginByGetPhoneNumber?sessionAuthId=' + this.sessionAuthId });
            },
            submit() {
                if (this.$u.test.mobile(this.mobile)) {
                    this.sendCode();
                } else {
                    this.$u.toast('请输入合法的手机号码!');
                }
            },
            // 收不到验证码选择时的选择
            noCaptcha() {
                var _this = this;
                uni.showActionSheet({
                    itemList: ['重新获取验证码'],
                    success: function (res) {
                        _this.sendCode();
                    },
                    fail: function (res) {
                        this.$u.toast('重发失败!');
                    }
                });
            },
            // change事件侦听
            change(value) {
                console.log('change', value);
            },
            // 输入完验证码最后一位执行
            finish(value) {
                this.code = value;
                // #ifdef H5|APP-PLUS|APP-PLUS-NVUE
                if (this.sessionAuthId) {
                    this.toBind();
                } else {
                    this.login();
                }
                // #endif
                // #ifdef MP
                this.showTopTips();
                // #endif
                console.log('finish', value);
            },
            // 发送短信验证码
            sendCode() {
                if (!this.rightMobile.status) {
                    this.$u.toast(this.rightMobile.msg);
                } else {
                    uni.showToast({ title: '发送中...', icon: 'loading' })
                    setTimeout(() => {
                        uni.hideToast();
                        this.$u.api.sms({ mobile: this.mobile, code: 'login' }).then(res => {
                            if (res.status) {
                                this.showCodeBox = true;
                                this.timer = 60;
                                this.verification = false;
                                this.$refs.uToast.show({ title: res.msg, type: 'success' });
                                this.countDown(); // 执行验证码计时
                            } else {
                                this.$u.toast(res.msg);
                            }
                        });
                    }, 1000);
                }
            },
            // 验证码倒计时
            countDown() {
                let auth_timer = setInterval(() => {
                    // 定时器设置每秒递减
                    this.timer--; // 递减时间
                    uni.setStorage({
                        key: 'timer',
                        data: this.timer,
                        success: function () { }
                    });
                    if (this.timer <= 0) {
                        this.verification = true; // 60s时间结束还原v-show状态并清除定时器
                        clearInterval(auth_timer);
                    }
                }, 1000);
            },
            // 登录
            login() {
                var _this = this;
                if (!_this.rightMobile.status) {
                    _this.$u.toast(_this.rightMobile.msg);
                } else {
                    // 短信验证码登录
                    if (!_this.code) {
                        _this.$u.toast('请输入短信验证码!');
                    } else {
                        let data = {
                            mobile: _this.mobile,
                            code: _this.code
                        };

                        let invicode = _this.$db.get('invitecode');
                        if (invicode) {
                            data.invitecode = invicode;
                        }

                        _this.$u.api.smsLogin(data).then(res => {
                            if (res.status) {
                                this.$db.set('userToken', res.data.token);
                                _this.redirectHandler(res.msg);
                            } else {
                                _this.$u.toast(res.msg);
                                _this.error = true;
                                _this.errorMsg = res.msg;
                            }
                        });
                    }
                }
            },
            // 重定向跳转 或者返回上一个页面
            redirectHandler(msg) {
                let _this = this;
                this.$refs.uToast.show({
                    title: msg, type: 'success', callback: function () {
                        _this.$db.set('timer', 0);
                        _this.$db.del('invitecode');
                        _this.toLoginSuccessHandleBack();
                    }
                })
            },
            // 跳转到普通登录
            toLogin() {
                uni.navigateTo({
                    url: '../../login/login/loginByAccount'
                });
            },
            //提交按钮
            showTopTips: function () {
                let _this = this;
                if (_this.mobile == '') {
                    _this.$u.toast('请输入手机号码');
                    return false;
                }
                if (this.code == '') {
                    _this.$u.toast('请输入验证码');
                    return false;
                }
                //if (_this.sessionAuthId == 0) {
                //    this.$refs.uToast.show({ title: '登录失败，请稍后再试', type: 'error', back: true });
                //    return false;
                //}
                var platform = 2;
                //1就是h5登陆（h5端和微信公众号端），2就是微信小程序登陆，3是支付宝小程序，4是app，5是pc
                // #ifdef MP-ALIPAY
                platform = 3;
                // #endif
                // #ifdef APP-PLUS || APP-PLUS-NVUE
                platform = 4;
                // #endif
                var data = {
                    mobile: _this.mobile,
                    code: _this.code,
                    platform: platform, //平台id，标识是小程序登陆的
                    sessionAuthId: _this.sessionAuthId //微信小程序接口存不了session，所以要绑定的id只能传到前台
                };
                //有推荐码的话，带上
                var invitecode = _this.$db.get('invitecode');
                if (invitecode) {
                    data.invitecode = invitecode;
                }
                _this.$u.api.smsLogin(data).then(res => {
                    if (res.status) {
                        _this.$db.set('userToken', res.data.token);
                        _this.redirectHandler(res.msg);
                    } else {
                        //报错了
                        _this.$u.toast(res.msg);
                        _this.error = true;
                        _this.errorMsg = res.msg;
                    }
                });
            },
            // 公众号第三方登录账号绑定
            toBind() {
                if (this.mobile == '') {
                    this.$u.toast('请输入手机号码');
                    return false;
                }
                if (this.code == '') {
                    this.$u.toast('请输入验证码');
                    return false;
                }

                let data = {
                    mobile: this.mobile,
                    code: this.code,
                    sessionAuthId: this.sessionAuthId
                };

                // 获取邀请码
                let invicode = this.$db.get('invitecode');
                if (invicode) {
                    data.invitecode = invicode;
                }

                this.$u.api.smsLogin(data).then(res => {
                    if (res.status) {
                        this.$db.set('userToken', res.data);
                        this.redirectHandler(res.msg);
                    } else {
                        this.$u.toast(res.msg);
                        this.error = true;
                        this.errorMsg = res.msg;
                    }
                });
            },
            // 切换登录方式
            selectLoginType() {
                this.$u.route({ type: 'redirectTo', url: '/pages/login/loginByAccount/loginByAccount' });
            },
            goForgetpwd() {
                this.$u.route('/pages/login/forget/forget');
            }
        }
    };
</script>

<style lang="scss" scoped>
    @import 'loginBySMS.scss';
</style>
