<template>
    <view>
        <view v-if="show">
            <u-toast ref="uToast" /><u-no-network></u-no-network>
            <u-navbar title="代理商申请"></u-navbar>

            <view class="page-body">
                <view class="u-content">
                    <u-parse :html="agentNotes" :selectable="true"></u-parse>
                </view>
                <view class="bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
                    <view class="flex padding-sm flex-direction">
                        <button class="cu-btn bg-blue" v-if="condition.conditionStatus" @click="goApply()">申请</button>
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
                this.$u.route('/pages/member/agent/apply');
            }
        },
        computed: {
            agentNotes() {
                return this.$store.state.config.agentNotes
            }
        },
        onLoad: function () {
            var _this = this;
            _this.$u.api.getAgentInfo().then(res => {
                if (res.status) {
                    _this.condition = res.data;
                    if (_this.condition.verifyStatus == 1 || (!_this.condition.needApply && _this.conditionStatus)) {
                        _this.$u.route({ type: 'redirectTo', url: '/pages/member/agent/panel' });
                    } else if (_this.condition.verifyStatus > 1) {
                        _this.$u.route({ type: 'redirectTo', url: '/pages/member/agent/applyState' });
                    } else {
                        _this.show = true;
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
    .page-body { background: #FFFFFF; border-top-left-radius: 38rpx; border-top-right-radius: 38rpx; margin: 25rpx; padding: 25rpx; margin-bottom: 150rpx; }
    .u-content { margin-top: 20rpx; color: $u-content-color; font-size: 28rpx; line-height: 1.8; }
        .u-content p { color: $u-tips-color; }
</style>
