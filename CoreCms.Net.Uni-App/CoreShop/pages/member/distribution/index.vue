<template>
    <view>
        <view v-if="show">
            <u-toast ref="uToast" /><u-no-network></u-no-network>
            <u-navbar title="分销申请"></u-navbar>

            <view class="content">
                <view class="bg-red solid-top coreshop-head-box">
                    <view class="text-center margin-bottom-sm">
                        <text class="text-white" v-if="condition.conditionStatus">已达标</text>
                        <text class="text-white" v-if="!condition.conditionStatus">未达标</text>
                        <text class="text-white text-xxl">{{condition.conditionProgress}}%</text>
                    </view>
                    <progress class="coreshop-progress-radius" percent="100" active stroke-width="10" activeColor="#fbbd08" />
                    <view class="text-center margin-bottom-sm margin-top-sm">
                        <text class="text-white">{{condition.conditionMsg}}</text>
                    </view>
                    <view class="text-left margin-bottom-sm  u-margin-top-40">
                        <text class="text-white text-xs">注：消费金额只算实付金额部分，储值抵扣/退款退货金额不算在内。</text>
                    </view>
                </view>
                <!--标题-->
                <view class="text-black text-df coreshop-title-view solid-bottom">分销商须知</view>
                <!--内容-->
                <view class="text-gray coreshop-content-view u-margin-top-10">
                    <u-parse :html="distributionNotes"></u-parse>
                </view>
                <view class="bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
                    <view class="flex padding-sm flex-direction">
                        <button class="cu-btn bg-red" v-if="condition.conditionStatus" @click="goApply()">申请</button>
                        <button class="cu-btn bg-grey" v-else>您的条件暂不满足</button>
                    </view>
                </view>
            </view>
        </view>
    </view>

</template>
<script>
    export default {
        data() {
            return {
                show: false,
                condition: {}
            }
        },
        methods: {
            goApply() {
                this.$u.route('/pages/member/distribution/apply');
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
                            _this.$u.route({ type: 'redirectTo', url: '/pages/member/distribution/panel' });
                        } else if (_this.condition.verifyStatus > 1) {
                            _this.$u.route({ type: 'redirectTo', url: '/pages/member/distribution/applyState' });
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
    page { background: #FFFFFF; }

    .coreshop-title-view { position: relative; padding: 18.18rpx; }
    .coreshop-content-view { position: relative; padding: 0 18.18rpx; }
    .coreshop-head-box { position: relative; padding: 45.45rpx 90.9rpx 10.45rpx 90.9rpx; }
    .content { background-color: #fff; height: calc(100vh - 100px); }
</style>
