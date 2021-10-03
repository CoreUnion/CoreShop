<template>
    <view class="clearfix">
        <view class="textarea u-margin-bottom-20">
            <coreshopContent :content="coreshopdata.parameters.value" v-if="coreshopdata.parameters.value"></coreshopContent>
        </view>
    </view>
</template>

<script>
    import coreshopContent from '@/components/coreshop-page/coreshop-content.vue'//视频和文本解析组件
    export default {
        components: {
            coreshopContent
        },
        name: "coreshoptextarea",
        props: {
            coreshopdata: {
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
    .textarea { width: 100%; background-color: #fff; padding: 10upx 26upx;
        p {
            img { width: 100% !important; background-color: #000; }
        }
        div { background-color: #000; }
    }
    .clearfix:after { content: "."; display: block; height: 0; clear: both; visibility: hidden; }
</style>
