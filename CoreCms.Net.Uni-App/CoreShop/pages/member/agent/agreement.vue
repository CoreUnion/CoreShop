<template>
    <view>
        <u-navbar title="代理协议"></u-navbar>
        <view class="page-body">
            <view class="u-content">
                <u-parse :html="content" :selectable="true"></u-parse>
            </view>
        </view>
    </view>

</template>
<script>
    export default {
        data() {
            return {
                content: this.$store.state.config.agentAgreement,
            }
        },
        onLoad(e) {
        },
        computed: {
        },
        methods: {
        }
    }
</script>
<style lang="scss" scoped>
    .page-body { background: #FFFFFF; border-top-left-radius: 38rpx; border-top-right-radius: 38rpx; margin: 25rpx; padding: 25rpx; }
    .u-content { margin-top: 20rpx; color: $u-content-color; font-size: 28rpx; line-height: 1.8; }
        .u-content p { color: $u-tips-color; }
</style>
