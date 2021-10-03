<template>
    <view>
        <u-navbar title="分销协议"></u-navbar>
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
                content: this.$store.state.config.distributionAgreement,
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
    @import "agreement.scss";
</style>
