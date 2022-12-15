<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="余额提现"></u-navbar>
        <view class="content">
            <view class="coreshop-content-top">
                <!-- 我的银行卡信息 -->
                <view class="coreshop-list menu sm-border margin-top" v-if="userbankCard" @click="toBankCardList">
                    <view class="coreshop-list-item arrow">
                        <view class="content">
                            <image :src="cardInfo.bankLogo" class="png" mode="aspectFit" style="width:6em;height:2em;"></image>
                            <text class="coreshop-text-grey">{{ cardInfo.cardNumber || ''}}</text>
                        </view>
                    </view>
                </view>

                <view class="coreshop-list menu sm-border margin-top" v-else @click="toBankCardList">
                    <view class="coreshop-list-item arrow">
                        <view class="content">
                            <image src="/static/images/common/yl.png" class="png" mode="aspectFit" style="width:6em;height:2em;"></image>
                            <text class="coreshop-text-grey">{{ cardInfo.cardNumber || ''}}</text>
                        </view>
                    </view>
                </view>

                <!-- 提现金额手续费 提现金额input -->
                <view class='coreshop-cell-group u-margin-top-20 u-margin-bottom-20'>
                    <view class='coreshop-cell-item'>
                        <view class='coreshop-cell-item-bd' v-if="tocashExplain">
                            <view class='coreshop-cell-hd-title coreshop-text-grey'>{{ tocashExplain || ''}}</view>
                        </view>
                    </view>
                    <view class='coreshop-cell-item'>
                        <view class='coreshop-cell-item-bd withdrawcash-input'>
                            <text>￥</text><input type="number" focus v-model="money" />
                        </view>
                    </view>
                    <view class='coreshop-cell-item'>
                        <view class='coreshop-cell-item-bd'>
                            <view class='coreshop-cell-hd-title coreshop-text-grey' v-show="!isError">可用余额 {{ user.balance}} 元</view>
                            <view class='coreshop-cell-hd-title coreshop-text-red' v-show="isError">提现金额超过可用余额</view>
                        </view>
                    </view>
                </view>
            </view>

            <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
                <view class="flex u-padding-20 flex-direction">
                    <u-button :custom-style="customStyle" size="medium"  type="success" v-if="isSubmit" @click="toCash" :disabled='submitStatus' :loading='submitStatus'>确认提现</u-button>
                    <u-button :custom-style="customStyle" size="medium" v-else-if="!isSubmit" disabled>确认提现</u-button>
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
                cardInfo: {}, // 我的银行卡信息
                user: {}, // 用户信息
                isError: false, // 当提现金额大于可用余额 显示错误提示
                isSubmit: false, // 提现点击
                money: '', // 用户输入的提现金额
                submitStatus: false
            }
        },
        onLoad() {
            this.userBankCard()
            this.userInfo()
        },
        onShow() {
        },
        computed: {
            userbankCard() {
                if (Object.keys(this.cardInfo).length) {
                    return true
                } else {
                    return false
                }
            },
            // 店铺提现手续费
            tocashMoneyRate() {
                return this.$store.state.config.toCashMoneyRate
            },
            // 店铺提现最低金额
            tocashMoneyLow() {
                return this.$store.state.config.toCashMoneyLow
            },
            // 提现文字说明
            tocashExplain() {
                if (this.tocashMoneyRate && this.tocashMoneyLow) {
                    return '最低提现金额 ' + this.tocashMoneyLow + ' 元（收取 ' + this.tocashMoneyRate + ' %服务费）'
                } else if (this.tocashMoneyLow) {
                    return '最低提现金额 ' + this.tocashMoneyLow + ' 元'
                } else if (this.tocashMoneyRate) {
                    return '收取 ' + this.tocashMoneyRate + ' %服务费'
                } else {
                    return ''
                }
            }
        },
        methods: {
            // 获取我的默认银行卡信息
            userBankCard() {
                this.$u.api.getDefaultBankCard().then(res => {
                    if (res.status) {
                        this.cardInfo = res.data
                    }
                })
            },
            // 获取用户信息
            userInfo() {
                // 获取我的余额信息
                // 获取用户的可用余额
                this.$u.api.userInfo().then(res => {
                    this.user = res.data
                    console.log(this.user);
                })
            },
            // 去提现
            toCash() {
                if (!Object.keys(this.cardInfo).length) {
                    this.$u.toast('请选择要提现的银行卡')
                    return false
                } else if (!this.money) {
                    this.$u.toast('请输入要提现的金额')
                    return false
                } else if (Number(this.money) === 0) {
                    this.$u.toast('提现金额不能为0')
                } else {
                    this.submitStatus = true;
                    var postMoney = Math.floor(this.money * 100) / 100
                    this.$u.api.userToCash({
                        data: postMoney,
                        id: this.cardInfo.id
                    }).then(res => {
                        this.submitStatus = false;
                        if (res.status) {
                            this.$refs.uToast.show({ title: res.msg, type: 'success', back: true })
                        } else {
                            this.$u.toast(res.msg);
                        }
                    });
                }
            },
            // 跳转我的银行卡列表
            toBankCardList() {
                this.$u.route('/pages/member/balance/bankcard/bankcard?mold=select')
            }
        },
        watch: {
            money() {
                // 比较用户的输入金额 如果大于可用金额
                if (this.money === '' || Number(this.money) <= 0) {
                    this.isSubmit = false
                } else if (Number(this.money) > Number(this.user.balance)) {
                    this.isError = true
                    this.isSubmit = false
                } else if (Number(this.money) < Number(this.tocashMoneyLow)) {
                    this.isError = false
                    this.isSubmit = false
                } else {
                    this.isError = false
                    this.isSubmit = true
                }
            }
        }
    }
</script>

<style lang="scss" scoped>
    @import "withdrawCash.scss";
</style>
