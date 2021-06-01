<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar back-text="返回" :is-back="true" title="授权登录"></u-navbar>

        <!--登录图标-->
        <view class="coreshop-user-login-avatar-view">
            <view class="cu-avatar round lg">
                <!-- #ifndef MP-TOUTIAO -->
                <open-data type="userAvatarUrl"></open-data>
                <!-- #endif -->
                <!-- #ifdef MP-TOUTIAO -->
                <image class="toutiao-logo" :src="logoImage"></image>
                <!-- #endif -->
            </view>
        </view>

        <!--按钮-->
        <view class="flex flex-direction coreshop-btn-view">
            <!-- #ifdef MP-WEIXIN -->
            <button class="cu-btn bg-red" shape="circle" open-type="getPhoneNumber" @getphonenumber="getPhoneNumber" hover-class="coreshop-btn-hover">
                <text class="cuIcon-mobilefill icon" />
                <text>微信手机号授权绑定</text>
            </button>
            <!-- #endif -->
        </view>

        <!--协议-->
        <view class="coreshop-agreement-checked-view">
            <!--<u-checkbox v-model="checked" shape="circle" style="position:absolute;" disabled="true" active-color="error"></u-checkbox>-->
            <view class="text-sm text-black-view">
                <view class="text-gray">申请获取以下权限</view>
                <view class="text-red"> 获得你的手机号码 <br />用于下单预留联系人收货信息，接收订单状态</view>
            </view>
        </view>

        <!--底部说明-->
        <view class="text-sm text-gray coreshop-foot-ad-view">{{appTitle}}</view>

        <view class="otherLogin" @click="goSmsLogin">
            <u-icon name="phone" color="#e54d42" size="45" label="手机短信验证绑定" label-pos="bottom" margin-top="20"></u-icon>
        </view>
    </view>
</template>

<script>
    import { off } from "process";

    export default {
        data() {
            return {
                sessionAuthId: '',
                checked: true
            }
        },
        computed: {
            logoImage() {
                return this.$store.state.config.shopLogo
            },
            appTitle() {
                return this.$store.state.config.shopName;
            },
        },
        onLoad(option) {
            const _this = this
            if (option.sessionAuthId) {
                this.sessionAuthId = option.sessionAuthId;
            }
            console.log(option.sessionAuthId);
        },
        methods: {
            handleRefuse() {
                uni.showToast({
                    title: '未登录',
                    icon: 'none',
                    duration: 1000,
                })
                setTimeout(() => {
                    uni.hideToast();
                    uni.navigateBack(-1);
                }, 1000);
            },
            getPhoneNumber: function (e) {
                let _this = this;
                if (e.mp.detail.errMsg == 'getPhoneNumber:ok') {
                    //var sessionAuthId = _this.$db.get('sessionAuthId')
                    var data = {
                        sessionAuthId: _this.sessionAuthId,
                        iv: e.mp.detail.iv,
                        encryptedData: e.mp.detail.encryptedData,
                    }
                    //有推荐码的话，带上
                    var invitecode = _this.$db.get('invitecode')
                    if (invitecode) {
                        data.invitecode = invitecode
                    }
                    _this.toGetPhoneNumber(data);



                } else {
                    _this.$u.toast('如未授权，您可尝试使用手机号+短信验证码登录');
                }
            },
            //实际的去登陆
            toGetPhoneNumber: function (data) {
                let _this = this
                _this.$u.api.loginByGetPhoneNumber(data).then(res => {
                    if (res.status) {
                        //判断是否返回了token，如果没有，就说明没有绑定账号，跳转到绑定页面
                        if (res.data.token) {
                            //登陆成功，设置token，并返回上一页
                            _this.$db.set('userToken', res.data.token)
                            uni.navigateBack({
                                delta: 1
                            })
                            return false
                        }
                    } else {
                        _this.$u.toast('登录失败，请重试')
                    }
                })
            },
            goSmsLogin: function () {
                this.$u.route({ type: 'redirectTo', url: '/pages/login/loginBySMS/loginBySMS?sessionAuthId=' + this.sessionAuthId });
            }
        }
    }
</script>

<style lang="scss" scoped>
    .coreshop-user-login-avatar-view { position: relative; display: flex; align-items: center; justify-content: center; margin-top: 218.18rpx; }
        .coreshop-user-login-avatar-view .cu-avatar { width: 181.81rpx; height: 181.81rpx; }
    .coreshop-btn-view { position: relative; margin-top: 72.72rpx; padding: 0 60rpx; }
        .coreshop-btn-view .cu-btn .icon { position: relative; font-size: 47.27rpx; right: 9.09rpx; top: -3.63rpx; }
    .coreshop-agreement-checked-view { position: relative; padding: 27.27rpx 60rpx; }
        .coreshop-agreement-checked-view .coreshop-checked { position: absolute; transform: scale(0.7); }
        .coreshop-agreement-checked-view .text-black-view { /* padding-left: 54.54rpx; */ line-height: 47.27rpx; }
    .coreshop-foot-ad-view { position: fixed; text-align: center; bottom: 72.72rpx; width: 100%; }
    .otherLogin { margin-top: 200upx; text-align: center; width: 100%; }
</style>
