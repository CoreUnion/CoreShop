<template>
    <view>
        <u-navbar title="分销申请"></u-navbar>
        <view v-if="show">
            <u-toast ref="uToast" />
            <u-no-network></u-no-network>
            <view class="coreshop-bg-red coreshop-solid-top u-padding-left-80 u-padding-right-80 u-padding-top-45 u-padding-bottom-45">
                <view class="u-text-center u-margin-bottom-20">
                    <text class="coreshop-text-white" v-if="condition.conditionStatus">已达标</text>
                    <text class="coreshop-text-white" v-if="!condition.conditionStatus">未达标</text>
                    <text class="coreshop-text-white u-font-40">{{condition.conditionProgress}}%</text>
                </view>
                <progress class="coreshop-progress-radius" percent="100" active stroke-width="10" activeColor="#fbbd08" />
                <view class="u-text-center u-margin-bottom-20 u-margin-top-20">
                    <text class="coreshop-text-white">{{condition.conditionMsg}}</text>
                </view>
                <view class="u-text-left u-margin-bottom-20  u-margin-top-40">
                    <text class="coreshop-text-white u-font-xs">注：消费金额只算实付金额部分，储值抵扣/退款退货金额不算在内。</text>
                </view>
            </view>
            <!--标题-->
            <view class="coreshop-text-black u-font-md  u-padding-20 coreshop-solid-bottom">分销商须知</view>
            <!--内容-->
            <view class="coreshop-text-gray u-margin-20">
                <u-parse :html="distributionNotes"></u-parse>
            </view>
            <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
                <view class="flex u-padding-20 flex-direction">
                    <u-button :custom-style="customStyle" size="medium" type="error" v-if="condition.conditionStatus" @click="goApply()">申请</u-button>
                    <u-button :custom-style="customStyle" size="medium" type="primary" v-else>您的条件暂不满足</u-button>
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
                show: false,
                condition: {}
            }
        },
        methods: {
            goApply() {
                this.$u.route('/pages/member/distribution/apply/apply');
            }
        },
        computed: {
            distributionNotes() {
                return this.$store.state.config.distributionNotes
            }
        },
        onLoad: function () {
            var _this = this;
            _this.$u.api.getDistributionInfo().then(res => {
                if (res.status) {
                    _this.condition = res.data;
                    if (_this.condition.hasOwnProperty('verifyStatus')) {
                        if (_this.condition.verifyStatus == 1 || (!_this.condition.needApply && _this.conditionStatus)) {
                            _this.$u.route({ type: 'redirectTo', url: '/pages/member/distribution/panel/panel' });
                        } else if (_this.condition.verifyStatus > 1) {
                            _this.$u.route({ type: 'redirectTo', url: '/pages/member/distribution/applyState/applyState' });
                        } else {
                            _this.show = true;
                        }
                    }
                } else {
                    //报错了
                    _this.$u.toast(res.msg);
                }
            });
        }
    }
</script>
<style lang="scss" scoped>
    @import "index.scss";
</style>
