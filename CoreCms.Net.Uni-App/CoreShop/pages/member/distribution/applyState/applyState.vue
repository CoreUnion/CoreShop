<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="申请状态"></u-navbar>

        <!--状态图标-->
        <view class="coreshop-status-image-view">
            <image :src="$globalConstVars.apiFilesUrl+'/static/images/common/non_real_name.png'" mode="widthFix" v-if="info.verifyStatus==3" />
            <image :src="$globalConstVars.apiFilesUrl+'/static/images/common/real_name.png'" mode="widthFix" v-if="info.verifyStatus==2 || info.verifyStatus==1" />
        </view>


        <view class="coreshop-text-black u-font-lg u-text-center u-margin-bottom-20" v-if="status">
            <text class="u-margin-right-20">{{info.name}}</text>
            <text>{{info.mobile}}</text>
        </view>

        <!--状态信息-->
        <view class="coreshop-text-black u-font-xl u-text-center u-margin-bottom-20" v-if="info.verifyStatus==2">
            恭喜，您的申请已提交！
        </view>
        <view class="coreshop-text-black u-font-xl u-text-center u-margin-bottom-20" v-if="info.verifyStatus==3">
            抱歉，您的申请被驳回！
        </view>
        <view class="coreshop-text-black u-font-xl u-text-center u-margin-bottom-20" v-if="info.verifyStatus==1">
            恭喜，您的申请已通过！
        </view>

        <view class="coreshop-text-gray u-text-center u-padding-70" v-if="info.verifyStatus==2">
            您于{{info.createTime}}提交的申请已经提交成功，当前状态：<text v-if="info.verifyStatus==2">等待审核</text><text v-if="info.verifyStatus==3">审核驳回</text><text v-if="info.verifyStatus==1">审核通过</text>
        </view>

    </view>

</template>
<script>
    export default {
        data() {
            return {
                info: {}
            }
        },
        onLoad: function () {
            var _this = this;
            _this.$u.api.getDistributionInfo({ check_condition: true }).then(res => {
                if (res.status) {
                    if (res.data.needApply && res.data.conditionStatus == false) {
                        _this.$u.route({ type: 'redirectTo', url: '/pages/member/distribution/index/index' });
                    }
                    if (res.data.verifyStatus == 1) {//审核通过
                        _this.$u.route({ type: 'redirectTo', url: '/pages/member/distribution/index/index' });
                    }
                    _this.info = res.data;
                } else {
                    //报错了
                    _this.$u.toast(res.msg);
                }
            });
        },
        methods: {
        }
    }
</script>
<style lang="scss">
    @import "applyState.scss";
</style>

