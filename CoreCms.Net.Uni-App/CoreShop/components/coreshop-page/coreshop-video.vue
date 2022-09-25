<template>
    <view class="video u-margin-15">
        <video class="videoCT" id="myVideo" :src="coreshopdata.parameters.list[0].url" :autoplay="autoplay" :poster="coreshopdata.parameters.list[0].image"></video>
    </view>
</template>

<script>
    export default {
        name: "coreshopvideo",
        props: {
            coreshopdata: {
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
            if (this.coreshopdata.parameters.autoplay == 'true' || this.coreshopdata.parameters.autoplay == true) {
                this.autoplay = true;
            }

        },
        onReady: function (res) {

        },
        methods: {

        }
    }
</script>

<style lang="scss" scoped>
    .video { border-radius: 8px;
        .videoCT { width: 100%; min-height: 200upx; }
    }
</style>
