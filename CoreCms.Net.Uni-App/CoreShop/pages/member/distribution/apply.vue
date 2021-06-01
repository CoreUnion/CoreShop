<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="提交申请"></u-navbar>
        <view class="contentBody">
            <view class="apply-content">
                <view class="apply-form">
                    <view class="cu-form-group">
                        <view class="title">姓名</view>
                        <input type="text" placeholder='请填您的姓名' v-model="name" />
                    </view>
                    <view class="cu-form-group">
                        <view class="title">微信</view>
                        <input type="text" placeholder='请填您的微信' v-model="weixin" />
                    </view>
                    <view class="cu-form-group">
                        <view class="title">QQ</view>
                        <input type="number" placeholder='请填您的QQ' v-model="qq" />
                    </view>
                    <view class="cu-form-group">
                        <view class="title">手机</view>
                        <input type="number" placeholder='请填写您的手机号码' v-model="mobile" />
                    </view>

                    <view class="apply-tip">
                        <label class="radio u-padding-left-20  u-padding-right-20" @click="agreeAgreement"><radio value="1" :checked="checked" color="#FF7159" /> &nbsp;</label> 我已经阅读并接受 <text class="agreement u-padding-left-20  u-padding-right-20" @click="goAgreement()">"分销协议"</text>
                    </view>
                </view>
                <view class="apply-bottom">
                    <button class="coreshop-btn coreshop-btn-square coreshop-btn-o coreshop-btn-all" hover-class="btn-hover" @click="goApplyState()">申请成为分销</button>
                </view>
            </view>
        </view>
    </view>
</template>
<script>
    export default {
        data() {
            return {
                name: '',
                weixin: '',
                qq: '',
                mobile: '',
                checked: false,
                isAgreement: 'off'
            }
        },
        methods: {
            // 是否同意协议
            agreeAgreement() {
                if (this.checked) {
                    this.checked = false;
                    this.isAgreement = 'off';
                } else {
                    this.checked = true;
                    this.isAgreement = 'on';
                }
            },
            // 信息验证
            checkData(data) {
                if (!data.name) {
                    this.$u.toast('请输入您的姓名')
                    return false
                } else if (!data.weixin) {
                    this.$u.toast('请输入您的微信')
                    return false
                } else if (!data.qq) {
                    this.$u.toast('请输入您的QQ')
                    return false
                } else if (!data.mobile) {
                    this.$u.toast('请输入您的手机号')
                    return false
                } else if (data.mobile.length !== 11) {
                    this.$u.toast('手机号格式不正确')
                    return false
                } else if (data.agreement != 'on') {
                    //console.log(data)
                    this.$u.toast('请勾选分销协议')
                    return false
                } else {
                    return true
                }
            },
            // 提交审核
            goApplyState() {
                let data = {
                    name: this.name,
                    weixin: this.weixin,
                    qq: this.qq,
                    mobile: this.mobile,
                    agreement: this.isAgreement,
                }
                if (this.checkData(data)) {
                    this.$u.api.applyDistribution(data).then(res => {
                        if (res.status) {
                            this.$refs.uToast.show({ title: res.msg, type: 'success', url: '/pages/member/distribution/applyState' })
                        } else {
                            this.$u.toast(res.msg);
                        }
                    });
                }
            },
            goAgreement() {
                uni.navigateTo({
                    url: '/pages/member/distribution/agreement'
                })
            }
        }
    }
</script>
<style lang="scss" scoped>
    .contentBody { background-color: #FF7159; height: calc(100vh - 100px); padding-top: 50rpx; }
    .cu-form-group .title { min-width: calc(4em + 15px); }
    .apply-content { margin: 40rpx auto; padding: 26rpx 0; border-radius: 30rpx; box-shadow: 0 0 10px #aaa; width: 670rpx; min-height: 400rpx; background-color: #fff; }
    .apply-tip { padding: 26rpx; }
    .apply-bottom { width: 100%; text-align: center; }
        .apply-bottom .coreshop-btn { border-radius: 50rpx; width: 90%; margin: 40rpx auto 0; }
    .agreement { text-decoration: underline; color: #FF7159; }
</style>
