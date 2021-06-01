<template>
    <view>
        <u-navbar title="打赏"></u-navbar>
        <view class="padding bg-white">
            <image :src="$apiFilesUrl+'/static/images/reward/wx.png'" mode="widthFix" />
            <image :src="$apiFilesUrl+'/static/images/reward/zfb.jpg'" mode="widthFix" />
        </view>
        <!-- 登录提示 -->
		<corecms-login-modal></corecms-login-modal>
    </view>
</template>
<script>
    export default {
        data() {
            return {
                title: 'Hello'
            }
        },
        onLoad() {

        },
        methods: {

        }
    }
</script>

<style lang="scss" scoped>
    image { width: 100%; }
</style>
