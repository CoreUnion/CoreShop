<template>
    <view class="clearfix">
        <view class="textarea bottom-cell-group">
            <corecmsContent :content="corecmsdata.parameters.value" v-if="corecmsdata.parameters.value"></corecmsContent>
        </view>
    </view>
</template>

<script>
    import corecmsContent from '@/components/corecms-page/corecms-content.vue'//视频和文本解析组件
    export default {
        components: {
            corecmsContent
        },
        name: "corecmstextarea",
        props: {
            corecmsdata: {
                // type: Object,
                required: true,
            }
        },
        created() {
        },
        onLoad() {

        },
        methods: {

        }
    }
</script>

<style lang="scss" scoped>
    .textarea { width: 100%; background-color: #fff; padding: 10upx 26upx; /* height: 40upx; */ }
        .textarea p img { width: 100% !important; }
        .textarea div { background-color: #000; }
        .textarea p { background-color: #000; }
    .clearfix:after { content: "."; display: block; height: 0; clear: both; visibility: hidden; }
</style>
