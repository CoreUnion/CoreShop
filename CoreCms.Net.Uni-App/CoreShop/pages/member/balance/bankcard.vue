<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="我的银行卡"></u-navbar>
        <view class="content">
            <view class="content-top" v-if="cards.length">
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
                            <view class="cibr-b color-9">
                                {{ item.cardNumber }}
                            </view>
                        </view>
                    </view>
                    <view class="mr-card" v-if="item.cardNumber === false" @click="setDefault(item.id)">
                        <button class="coreshop-btn coreshop-btn-w" :disabled='submitStatus' :loading='submitStatus'>设为默认</button>
                    </view>
                    <view class="del-card" v-if="mold" @click="selected(index)">
                        <button class="coreshop-btn coreshop-btn-b">选择</button>
                    </view>
                    <view class="del-card" v-else @click="removeCard(item.id)">
                        <button class="coreshop-btn coreshop-btn-b" :disabled='delSubmitStatus' :loading='delSubmitStatus'>删除</button>
                    </view>
                </view>
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$apiFilesUrl+'/static/images/empty/coupon.png'" icon-size="300" text="暂无银行卡" mode="list"></u-empty>
            </view>
            <view class="bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
                <view class="flex padding-sm flex-direction">
                    <button class="cu-btn bg-red" @click="goAddcard()">添加银行卡</button>
                </view>
            </view>

        </view>
    </view>

</template>

<script>
    export default {
        data() {
            return {
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
                this.$u.route({ url: '/pages/member/balance/addBankCard' })
            },
            selected(index) {
                let pages = getCurrentPages();//当前页
                let beforePage = pages[pages.length - 2];//上个页面

                // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE
                beforePage.cardInfo = this.cards[index]
                // #endif

                // #ifdef MP-WEIXIN
                beforePage.$vm.cardInfo = this.cards[index]
                // #endif

                // #ifdef MP-ALIPAY || MP-TOUTIAO
                this.$db.set('userCardInfo', this.cards[index], true);
                // #endif

                uni.navigateBack({
                    delta: 1
                })
            }
        }
    }
</script>

<style lang="scss" scoped>
    .card-item { position: relative; background-color: #fff; margin: 26rpx; border-radius: 10rpx; box-shadow: 0 0 20rpx #ccc; padding: 60rpx 30rpx 80rpx; }
    .card-item-tip { position: absolute; top: 0rpx; left: 0rpx; z-index: 10; border-top-left-radius: 10rpx; overflow: hidden; width: 100rpx; height: 100rpx; }
    .cit-bg { position: absolute; top: 0; left: 0; z-index: 11; color: #ffffff; width: 0rpx; height: 0rpx; border-bottom: solid 100rpx transparent; border-right: solid 100rpx transparent; border-top: solid 100rpx #FF7159; }
    .cit-text { position: absolute; top: 0; left: 0; z-index: 12; color: #ffffff; margin-top: 4rpx; margin-left: 14rpx; font-size: 30rpx; }

    .cib-left { position: absolute; top: 60%; transform: translateY(-50%); width: 250rpx; }
    .bank-logo { width: 240rpx; height: 70rpx; }
    .cib-right { margin-left: 250rpx; }
    .cibr-t { font-size: 30rpx; margin-bottom: 10rpx; text-align: center; }
    .cibr-b { font-size: 26rpx; text-align: center; }
    .mr-card { position: absolute; right: 140rpx; bottom: 0rpx; }
    /* #ifdef MP-TOUTIAO */
    .mr-card { position: absolute; right: 140rpx; bottom: 12rpx; }
    .del-card { position: absolute; right: 30rpx; bottom: 12rpx; }
    /* #endif */
    /* #ifndef MP-TOUTIAO */
    .mr-card { position: absolute; right: 140rpx; bottom: 0rpx; }
    .del-card { position: absolute; right: 30rpx; bottom: 0rpx; }
        /* #endif */
        .del-card .coreshop-btn, .mr-card .coreshop-btn { font-size: 24rpx; height: 48rpx; line-height: 46rpx; padding: 0 16rpx; }

</style>