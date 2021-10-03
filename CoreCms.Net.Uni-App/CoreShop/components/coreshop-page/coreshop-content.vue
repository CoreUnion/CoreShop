<template>
    <view class="u-content">
        <u-parse :html="content" :selectable="true"></u-parse>
    </view>

</template>
<script>
    //视频和文本解析组件
    export default {
        name: 'coreshopContent',
        props: {
            content: {}
        },
        created() { },
        methods: {
            preview(src, e) {
                // do something
            },
            navigate(href, e) {
                // do something
            }
        }
    }
</script>
<style lang="scss" scoped>
    .u-content { color: $u-content-color; font-size: 26rpx; line-height: 1.8; }
        .u-content p { color: $u-tips-color; }
</style>
