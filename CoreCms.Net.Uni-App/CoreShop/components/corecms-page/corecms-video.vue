<template>
    <view class="video u-margin-15">
        <video class="videoCT" id="myVideo" :src="corecmsdata.parameters.list[0].url" :autoplay="autoplay" :poster="corecmsdata.parameters.list[0].image"></video>
    </view>
</template>

<script>
    export default {
        name: "corecmsvideo",
        props: {
            corecmsdata: {
                type: Object,
                required: true,
            }
        },
        data() {
            return {
                autoplay: false
            }
        },
        created() {
            if (this.corecmsdata.parameters.autoplay == 'true' || this.corecmsdata.parameters.autoplay == true) {
                this.autoplay = true;
            }

        },
        onReady: function (res) {
            // #ifndef MP-ALIPAY
            this.videoContext = uni.createVideoContext('myVideo')
            // #endif
        },
        methods: {

        }
    }
</script>

<style lang="scss" scoped>
    .video { border-radius: 8px; }
        .videoCT { width: 100%; min-height: 200upx;}
</style>
