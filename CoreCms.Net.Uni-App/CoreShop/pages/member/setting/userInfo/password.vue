<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="设置密码"></u-navbar>
        <view class="u-padding-30 coreshop-bg-white">
            <u-form :model="model" :rules="rules" ref="uForm" :errorType="errorType">
                <u-form-item label="旧密码" label-width="150" prop="password" v-if="oldpassWord">
                    <u-input :password-icon="true" type="password" v-model="model.pwd" placeholder="请输入密码"></u-input>
                </u-form-item>
                <u-form-item label="新密码" label-width="150" prop="password">
                    <u-input :password-icon="true" type="password" v-model="model.newPwd" placeholder="请输入密码"></u-input>
                </u-form-item>
                <u-form-item label="确认密码" label-width="150" prop="rePassword">
                    <u-input :password-icon="true" type="password" v-model="model.rePwd" placeholder="请确认密码"></u-input>
                </u-form-item>
            </u-form>
        </view>

        <!--按钮-->
        <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
            <view class="flex u-padding-20 flex-direction">
                <u-button :custom-style="customStyle" type="error" size="medium" @click="submitHandler" :disabled='submitStatus' :loading='submitStatus'>保存</u-button>
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
                model: {
                    pwd: '',
                    newPwd: '',
                    rePwd: '',
                },
                errorType: ['message'],
                submitStatus: false,
                oldpassWord: true,
                rules: {
                    password: [
                        {
                            required: true,
                            message: '请输入密码',
                            trigger: ['change', 'blur'],
                        },
                        {
                            // 正则不能含有两边的引号
                            pattern: /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]+\S{5,12}$/,
                            message: '需同时含有字母和数字，长度在6-12之间',
                            trigger: ['change', 'blur'],
                        }
                    ],
                    rePassword: [
                        {
                            required: true,
                            message: '请重新输入密码',
                            trigger: ['change', 'blur'],
                        },
                        {
                            validator: (rule, value, callback) => {
                                return value === this.model.newPwd;
                            },
                            message: '两次输入的密码不相等',
                            trigger: ['change', 'blur'],
                        }
                    ],
                },
            }
        },
        computed: {},
        onReady() {
            this.$refs.uForm.setRules(this.rules);
        },
        methods: {
            // 保存资料
            submitHandler() {
                this.submitStatus = true;
                if (this.oldpassWord == true) {
                    if (this.model.pwd === '') {
                        this.$u.toast('请输入旧密码')
                        this.submitStatus = false;
                    } else if (this.model.newPwd === '') {
                        this.$u.toast('请输入新密码')
                        this.submitStatus = false;
                    } else if (this.model.rePwd === '') {
                        this.$u.toast('请输入重复密码')
                        this.submitStatus = false;
                    } else {
                        this.$u.api.editPwd({
                            pwd: this.model.pwd,
                            newpwd: this.model.newPwd,
                            repwd: this.model.rePwd
                        }).then(res => {
                            this.submitStatus = false;
                            if (res.status) {
                                this.$refs.uToast.show({ title: res.msg, type: 'success', back: true })
                            } else {
                                this.$u.toast(res.msg)
                            }
                        });
                    }
                } else {
                    if (this.model.newPwd === '') {
                        this.$u.toast('请输入新密码')
                        this.submitStatus = false;
                    } else if (this.model.rePwd === '') {
                        this.$u.toast('请输入重复密码')
                        this.submitStatus = false;
                    } else {
                        this.$u.api.editPwd({
                            newpwd: this.model.newPwd,
                            repwd: this.model.rePwd,
                        }).then(res => {
                            this.submitStatus = false;
                            if (res.status) {
                                this.$refs.uToast.show({ title: res.msg, type: 'success', back: true })
                            } else {
                                this.$u.toast(res.msg)
                            }
                        });
                    }
                }
            }
        },
        onLoad: function () {
            var _this = this;
            _this.$u.api.userInfo().then(res => {
                if (res.status) {
                    _this.oldpassWord = res.data.passWord ? true : false;;
                } else {
                    //报错了
                    _this.$u.toast(res.msg);
                }
            });
        }
    }
</script>

<style lang="scss">
</style>
