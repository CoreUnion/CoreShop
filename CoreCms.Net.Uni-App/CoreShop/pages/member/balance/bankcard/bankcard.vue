<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="我的银行卡"></u-navbar>
        <view class="coreshop-content-top" v-if="cards.length">
            <view class="card-item" v-for="(item, index) in cards" :key="index">
                <view class="card-item-tip" v-if="item.isdefault">
                    <view class="cit-bg"></view>
                    <view class="cit-text">默</view>
                </view>
                <view class="card-item-body">
                    <view class="cib-left">
                        <image class="bank-logo" :src="item.bankLogo" mode=""></image>
                    </view>
                    <view class="cib-right">
                        <view class="cibr-t color-3">
                            {{ item.bankName }} - {{ item.cardTypeName }}
                        </view>
                        <view class="cibr-b ">
                            {{ item.cardNumber }}
                        </view>
                    </view>
                </view>
                <u-button class="mr-card" v-if="item.isdefault === false" @click="setDefault(item.id)" size="mini" :disabled='submitStatus' :loading='submitStatus'  type="success">设为默认</u-button>
                <u-button class="del-card" v-if="mold" @click="selected(index)" size="mini" type="primary">选择</u-button>
                <u-button class="del-card" v-else @click="removeCard(item.id)" size="mini" type="error" :disabled='delSubmitStatus' :loading='delSubmitStatus'>删除</u-button>
            </view>
        </view>
        <!-- 无数据时默认显示 -->
        <view class="coreshop-emptybox" v-else>
            <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/coupon.png'" icon-size="300" text="暂无银行卡" mode="list"></u-empty>
        </view>

        <!--按钮-->
        <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
            <view class="flex u-padding-20 flex-direction">
                <u-button :custom-style="customStyle" type="error" size="medium" @click="goAddcard()">添加银行卡</u-button>
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
                mold: '',
                cards: [],// 我的银行卡列表
                submitStatus: false,
                delSubmitStatus: false
            }
        },
        onLoad(options) {
            if (options.mold && options.mold == 'select') {
                this.mold = options.mold
            }
        },
        onShow() {
            this.getBankCards()
        },
        methods: {
            // 获取我的银行卡列表
            getBankCards() {
                this.$u.api.getBankCardList().then(res => {
                    if (res.status) {
                        this.cards = res.data
                    }
                })
            },
            // 删除银行卡
            removeCard(cardId) {
                let _that = this;
                this.$common.modelShow('提示', '确定删除该银行卡?', res => {
                    _that.delSubmitStatus = true;
                    let data = {
                        id: cardId
                    }
                    _that.$u.api.removeBankCard(data).then(res => {
                        if (res.status) {
                            _that.$refs.uToast.show({
                                title: res.msg, type: 'success', callback: function () {
                                    _that.delSubmitStatus = false;
                                    _that.getBankCards();
                                }
                            })
                        } else {
                            _that.$u.toast(res.msg);
                            _that.delSubmitStatus = false;
                        }
                    })
                })
            },
            // 设置默认卡
            setDefault(id) {
                let _that = this;
                _that.submitStatus = true;
                let data = {
                    id: id
                }
                _that.$u.api.setDefaultBankCard(data).then(res => {
                    _that.submitStatus = false;
                    if (res.status) {
                        _that.$refs.uToast.show({
                            title: res.msg, type: 'success', callback: function () {
                                _that.getBankCards();
                            }
                        })
                    } else {
                        _that.$u.toast(res.msg);
                    }
                });
            },
            // 添加新的银行卡
            goAddcard() {
                this.$u.route({ url: '/pages/member/balance/addBankCard/addBankCard' })
            },
            selected(index) {
                let pages = getCurrentPages();//当前页
                let beforePage = pages[pages.length - 2];//上个页面

                beforePage.$vm.cardInfo = this.cards[index]

                uni.navigateBack({
                    delta: 1
                })
            }
        }
    }
</script>

<style lang="scss" scoped>
    @import "bankcard.scss";
</style>