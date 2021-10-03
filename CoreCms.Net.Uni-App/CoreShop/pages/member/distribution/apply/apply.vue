<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="提交申请"></u-navbar>
        <view class="contentBody">
            <view class="apply-content">
                <view class="u-padding-30">
                    <u-form :model="form" :rules="rules" ref="uForm" :errorType="errorType">
                        <u-form-item label="姓名" prop="name">
                            <u-input type="text" placeholder="请输入姓名" v-model="form.name" />
                        </u-form-item>
                        <u-form-item label="微信" prop="weixin">
                            <u-input type="text" placeholder="请输入微信" v-model="form.weixin" />
                        </u-form-item>
                        <u-form-item label="QQ" prop="qq">
                            <u-input type="text" placeholder="请输入QQ" v-model="form.qq" />
                        </u-form-item>
                        <u-form-item label="手机" prop="mobile">
                            <u-input type="number" placeholder="请输入手机" v-model="form.mobile" />
                        </u-form-item>
                    </u-form>

                    <view class="u-padding-30 flex u-flex-nowrap">
                        <u-checkbox-group>
                            <u-checkbox v-model="form.checked">我已经阅读并接受</u-checkbox>
                            <text class="coreshop-text-orange u-padding-left-20  u-padding-right-20 u-padding-top-8 u-font-30 coreshop-vertical-align-sub" @click="goAgreement()">"分销协议"</text>
                        </u-checkbox-group>
                    </view>
                </view>
                <view class="u-padding-30">
                    <u-button :custom-style="customStyle" type="error" size="medium" @click="submit()">申请成为分销</u-button>
                </view>
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
                            this.$u.toast('请勾选分销协议')
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
                        this.$u.api.applyDistribution(data).then(res => {
                            if (res.status) {
                                this.$refs.uToast.show({ title: res.msg, type: 'success', url: '/pages/member/distribution/applyState/applyState' })
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
                    url: '/pages/member/distribution/agreement/agreement'
                })
            }
        }
    }
</script>
<style lang="scss" scoped>
    @import "apply.scss";
</style>
