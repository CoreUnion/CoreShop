<template>
    <view>
        <u-navbar title="模板"></u-navbar>
        <view class="content">
            <image class="logo" src="/static/images/logo/logo.png"></image>
            <view class="text-area">
                <text class="title">
                    CoreShop - Net开源商城系统
                </text>
            </view>
            <view class="button-demo">
                <u-button :ripple="true">按钮组件演示</u-button>
            </view>
            <view class="link-demo">
                <u-link :color="$u.color['primary']" :under-line="true" href="http://www.corecms.net">跳转CoreCms官网：www.coreshop.cn</u-link>
            </view>
        </view>
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
    .content { display: flex; flex-direction: column; align-items: center; justify-content: center; padding: 40rpx; }
    .logo { height: 200rpx; width: 200rpx; margin-top: 100rpx; margin-left: auto; margin-right: auto; margin-bottom: 50rpx; }
    .text-area { display: flex; justify-content: center; }
    .title { font-size: 28rpx; color: $u-content-color; }
    .button-demo { margin-top: 80rpx; }
    .link-demo { margin-top: 80rpx; }
</style>
