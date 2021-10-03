<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="在线充值"></u-navbar>
        <view class="coreshop-bg-white u-margin-20 u-padding-20">
            <u-form :model="form" ref="uForm" :rules="rules" :errorType="errorType">
                <u-form-item label="当前金额" label-width="150">
                    <view class="coreshop-text-red coreshop-text-price u-text-lg">{{ user.balance || '0'}}</view>
                </u-form-item>
                <u-form-item label="储值金额" label-width="150" prop="money">
                    <u-input type="number" v-model="form.money" placeholder='请输入要储值的金额' />
                </u-form-item>
            </u-form>
            <!--按钮-->
            <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
                <view class="flex u-padding-20 flex-direction">
                    <u-button :custom-style="customStyle" type="error" size="medium" @click="navigateToHandle">去支付</u-button>
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
                user: {}, // 用户信息
                errorType: ['message'],
                form: {
                    money: ''// 储值的金额
                },
                rules: {
                    money: [
                        {
                            required: true,
                            message: '请输入储值金额',
                            trigger: 'blur,change'
                        }, {
                            type: 'number',
                            message: '请输入整数金额',
                            trigger: 'blur,change'
                        }
                    ]
                },
                orderType: 2	// 储值类型
            }
        },
        onReady() {
            this.$refs.uForm.setRules(this.rules);
        },
        onLoad() {
            this.userInfo()
        },
        methods: {
            // 获取用户信息
            userInfo() {
                this.$u.api.userInfo().then(res => {
                    if (res.status) {
                        this.user = res.data
                    }
                })
            },
            // 去储值
            navigateToHandle() {
                this.$refs.uForm.validate(valid => {
                    if (valid) {
                        console.log('验证通过');
                        if (!Number(this.form.money)) {
                            this.$u.toast('请输入要储值的金额')
                        } else {
                            this.$u.route('/pages/payment/pay/pay?recharge=' + Number(this.form.money) + '&type=' + this.orderType)
                        }
                    } else {
                        console.log('验证失败');
                    }
                });
            }
        }
    }
</script>

<style lang="scss" scoped>
</style>
