<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="提交申请"></u-navbar>
        <!-- 标题栏 -->
        <view class="head-box">
            <view class="head-img-wrap"><image class="head-img" :src="$globalConstVars.apiFilesUrl + '/static/images/agent/applyBg.jpg'" mode="widthFix"></image></view>
        </view>
        <!-- 表单 -->
        <view class="apply-form">
            <u-form :model="form" :rules="rules" ref="uForm" :errorType="errorType">
                <u-form-item label="姓名" label-width="150" prop="name">
                    <u-input type="text" placeholder='请填您的姓名' v-model="form.name" />
                </u-form-item>
                <u-form-item label="微信" label-width="150" prop="weixin">
                    <u-input type="text" placeholder='请填您的微信' v-model="form.weixin" />
                </u-form-item>
                <u-form-item label="QQ" label-width="150" prop="qq">
                    <u-input type="number" placeholder='请填您的QQ' v-model="form.qq" />
                </u-form-item>
                <u-form-item label="手机" label-width="150" prop="mobile">
                    <u-input type="number" placeholder='请填写您的手机号码' v-model="form.mobile" />
                </u-form-item>

                <view class="u-padding-30 flex u-flex-nowrap">
                    <u-checkbox-group>
                        <u-checkbox v-model="form.checked">我已经阅读并接受</u-checkbox>
                        <text class="coreshop-text-orange u-padding-left-20  u-padding-right-20 u-padding-top-8 u-font-30 coreshop-vertical-align-sub" @click="goAgreement()">"代理协议"</text>
                    </u-checkbox-group>
                </view>
            </u-form>
            <view class="u-padding-30">
                <u-button :custom-style="customStyle" class="save-btn" type="primary" size="medium" @click="submit()">申请成为代理商</u-button>
            </view>
        </view>
    </view>
</template>
<script>
    export default {
        data() {
            return {
                customStyle: {
                    width: '100%',
                },
                form: {
                    name: '',
                    weixin: '',
                    qq: '',
                    mobile: '',
                    checked: false,
                    isAgreement: 'off'
                },
                errorType: ['message'],
                rules: {
                    name: [
                        {
                            required: true,
                            message: '请输入姓名',
                            trigger: ['blur', 'change']
                        },
                        {
                            min: 2,
                            max: 4,
                            message: '长度在2-4个字符之间'
                        }
                    ],
                    weixin: [
                        {
                            required: true,
                            message: '请输入微信',
                            trigger: ['blur', 'change']
                        }
                    ],
                    qq: [
                        {
                            required: true,
                            message: '请输入QQ',
                            trigger: ['blur', 'change']
                        },
                        {
                            type: "number",
                            message: 'QQ必须为数字',
                            trigger: ['change', 'blur']
                        },
                    ],
                    mobile: [
                        {
                            required: true,
                            message: '请输入手机号码',
                            trigger: ['blur', 'change']
                        },
                        {
                            validator: (rule, value, callback) => {
                                return this.$u.test.mobile(value);
                            },
                            message: '手机号码不正确',
                            trigger: ['change', 'blur'],
                        }
                    ]
                }
            }
        },
        onReady() {
            this.$refs.uForm.setRules(this.rules);
        },
        methods: {
            submit() {
                this.$refs.uForm.validate(valid => {
                    if (valid) {
                        console.log('验证通过');
                        if (this.form.checked == false) {
                            this.$u.toast('请勾选代理协议')
                            return false;
                        }
                        this.form.isAgreement = "on";

                        // 提交审核
                        let data = {
                            name: this.form.name,
                            weixin: this.form.weixin,
                            qq: this.form.qq,
                            mobile: this.form.mobile,
                            agreement: this.form.isAgreement,
                        }
                        this.$u.api.applyAgent(data).then(res => {
                            if (res.status) {
                                this.$refs.uToast.show({ title: res.msg, type: 'success', url: '/pages/member/agent/applyState/applyState' })
                            } else {
                                this.$u.toast(res.msg);
                            }
                        });

                    } else {
                        console.log('验证失败');
                    }
                });
            },
            goAgreement() {
                uni.navigateTo({
                    url: '/pages/member/agent/agreement/agreement'
                })
            }
        }
    }
</script>
<style lang="scss" scoped>
    @import 'apply.scss';
</style>
