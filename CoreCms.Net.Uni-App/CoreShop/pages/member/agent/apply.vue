<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <!--<u-navbar title="提交申请"></u-navbar>-->
        <!-- 标题栏 -->
        <view class="head-box">
            <view class="nav-box"><cu-custom isBack></cu-custom></view>
            <view class="head-img-wrap"><image class="head-img" :src="$apiFilesUrl + '/static/images/agent/applyBg.jpg'" mode="widthFix"></image></view>
        </view>

        <!-- 表单 -->
        <view class="apply-form">
            <u-form :model="model" :rules="rules" ref="uForm" :errorType="errorType">

                <u-form-item label="姓名" type="text" label-width="150"><u-input placeholder='请填您的姓名' v-model="form.name" /></u-form-item>
                <u-form-item label="微信" type="text" label-width="150"><u-input placeholder='请填您的微信' v-model="form.weixin" /></u-form-item>
                <u-form-item label="QQ" type="number" label-width="150"><u-input placeholder='请填您的QQ' v-model="form.qq" /></u-form-item>
                <u-form-item label="手机" type="number" label-width="150"><u-input placeholder='请填写您的手机号码' v-model="form.mobile" /></u-form-item>


                <view class="apply-tip">
                    <label class="radio u-padding-left-20  u-padding-right-20" @click="agreeAgreement"><radio value="1" :checked="checked" color="#FF7159" /> &nbsp;</label> 我已经阅读并接受 <text class="agreement u-padding-left-20  u-padding-right-20" @click="goAgreement()">"代理商协议"</text>
                </view>

                <button class="cu-btn save-btn" @tap="goApplyState" :disabled="isFormEnd">
                    <text v-if="isFormEnd" class="cuIcon-loading2 cuIconfont-spin"></text>
                    申请成为代理商
                </button>
            </u-form>
        </view>

        <!--<view class="contentBody bg-blue">
            <view class="apply-content">
                <view class="apply-form">


                </view>
            </view>
        </view>-->
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
                isFormEnd: false, //提交成功
                form: {
                    name: '',
                    weixin: '',
                    qq: '',
                    mobile: '',
                    agreement: 'off'
                },
            }
        },
        methods: {
            // 是否同意协议
            agreeAgreement() {
                if (this.checked) {
                    this.checked = false;
                    this.form.agreement = 'off';
                } else {
                    this.checked = true;
                    this.form.agreement = 'on';
                }
            },
            // 信息验证
            checkData(data) {
                console.log(data);
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
                let that = this;
                that.isFormEnd = true;
                let data = {
                    name: that.form.name,
                    weixin: that.form.weixin,
                    qq: that.form.qq,
                    mobile: that.form.mobile,
                    agreement: that.form.agreement,
                }
                if (this.checkData(data)) {
                    this.$u.api.applyAgent(data).then(res => {
                        that.isFormEnd = false;
                        if (res.status) {
                            this.$refs.uToast.show({ title: res.msg, type: 'success', url: '/pages/member/agent/applyState' })
                        } else {
                            this.$u.toast(res.msg);
                        }
                    });
                } else {
                    that.isFormEnd = false;
                }
            },
            goAgreement() {
                uni.navigateTo({
                    url: '/pages/member/agent/agreement'
                })
            }
        }
    }
</script>
<style lang="scss" scoped>
    @import '../../../static/style/agent.scss';
</style>
