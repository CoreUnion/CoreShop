<template>
    <view>
        <view v-if="show">
            <u-toast ref="uToast" /><u-no-network></u-no-network>
            <u-navbar title="代理商申请"></u-navbar>
            <view class="page-body">
                <view class="u-content">
                    <u-parse :html="agentNotes" :selectable="true"></u-parse>
                </view>
                <!--按钮-->
                <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
                    <view class="flex u-padding-20 flex-direction">
                        <u-button :custom-style="customStyle" size="medium" type="error" v-if="condition.conditionStatus" @click="goApply()">申请</u-button>
                        <u-button :custom-style="customStyle" size="medium" type="primary" v-else>您的条件暂不满足</u-button>
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
                customStyle: {
                    width: '100%',
                },
                show: false,
                condition: {}
            }
        },
        methods: {
            goApply() {
                this.$u.route('/pages/member/agent/apply/apply');
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
                        _this.$u.route({ type: 'redirectTo', url: '/pages/member/agent/panel/panel' });
                    } else if (_this.condition.verifyStatus > 1) {
                        _this.$u.route({ type: 'redirectTo', url: '/pages/member/agent/applyState/applyState' });
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
    @import "index.scss";
</style>
