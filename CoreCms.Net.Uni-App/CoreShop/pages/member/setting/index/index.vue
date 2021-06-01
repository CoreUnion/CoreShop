<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="设置"></u-navbar>

        <view class="cu-list menu sm-border">
            <view class="cu-item arrow" @click="navigateToHandle('/pages/member/setting/userInfo/index')">
                <view class="content">
                    <text class="cuIcon-settings text-pink"></text>
                    <text class="text-grey">个人资料</text>
                </view>
            </view>
            <!-- #ifdef MP-WEIXIN  -->
            <view class="cu-item arrow" @click="syncWeChatInfo()">
                <view class="content">
                    <text class="cuIcon-refresh text-pink"></text>
                    <text class="text-grey">同步微信昵称头像</text>
                </view>
            </view>
            <!-- #endif  -->
            <view class="cu-item arrow" @click="navigateToHandle('/pages/member/setting/userInfo/password')">
                <view class="content">
                    <text class="cuIcon-warn text-green"></text>
                    <text class="text-grey">修改密码</text>
                </view>
            </view>
            <view class="cu-item arrow" @click="goAboutUs()">
                <view class="content">
                    <text class="cuIcon-btn text-green"></text>
                    <text class="text-grey">关于我们</text>
                </view>
            </view>
            <view class="cu-item arrow" @click="goUserAgreementPage()">
                <view class="content">
                    <text class="cuIcon-emojiflashfill text-pink"></text>
                    <text class="text-grey">用户协议</text>
                </view>
            </view>
            <view class="cu-item arrow" @click="goUserPrivacyPolicy()">
                <view class="content">
                    <text class="cuIcon-tagfill text-red  margin-right-xs"></text>
                    <text class="text-grey">隐私政策</text>
                </view>
            </view>
            <view class="cu-item" @click="clearCache">
                <view class="content">
                    <text class="cuIcon-circlefill text-grey"></text>
                    <text class="text-grey">清除缓存</text>
                </view>
                <view class="action">
                    <text class="cuIcon-delete text-grey" />
                </view>
            </view>
        </view>


        <!--按钮-->
        <view class="bg-white coreshop-footer-fixed coreshop-foot-padding-bottom" @click="logOff">
            <view class="flex padding-sm flex-direction">
                <button class="cu-btn bg-red">退出登录</button>
            </view>
        </view>

    </view>

</template>

<script>
    import { commonUse, articles } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [commonUse, articles],
        methods: {
            navigateToHandle(pageUrl) {
                this.$u.route(pageUrl)
            },
            //同步微信昵称数据
            syncWeChatInfo() {
                let _this = this
                wx.getUserProfile({
                    desc: "获取你的昵称、头像、地区及性别",
                    success: e => {
                        console.log(e)
                        if (e.errMsg == 'getUserProfile:ok') {
                            //var data = {
                            //    avatarUrl: e.avatarUrl,
                            //    city: e.city,
                            //    country: e.country,
                            //    gender: e.gender,
                            //    language: e.language,
                            //    nickName: e.nickName,
                            //    province: e.province
                            //}
                            _this.$u.api.syncWeChatInfo(e.userInfo).then(res => {
                                console.log(res);
                                if (res.status) {
                                    _this.$refs.uToast.show({ title: '同步成功', type: 'success', });
                                    if (res.data) {
                                        _this.$store.commit('userInfo', res.data);
                                    }
                                } else {
                                    _this.$u.toast('登录失败，请重试')
                                }
                            })
                        }
                    },
                    fail: res => {
                        //拒绝授权
                        this.$refs.uToast.show({ title: '您拒绝了请求', type: 'error' })
                        return;
                    }
                })

            },
            // 清除缓存
            clearCache() {
                // 重新获取统一配置信息
                this.$u.api.shopConfigV2().then(res => {
                    this.$store.commit('config', res.data)
                })
                //获取地区信息
                this.$u.api.getAreaList().then(res => {
                    if (res.status) {
                        // 删除地区缓存信息
                        this.$db.del('areaList');
                        this.$db.set('areaList', res.data)
                    }
                });
                setTimeout(() => {
                    this.$refs.uToast.show({ title: '清除成功', type: 'success' })
                }, 500)
            },

            // 退出登录
            logOff() {
                this.$common.modelShow('退出', '确认退出登录吗?', () => {
                    this.$db.del('userToken')
                    uni.reLaunch({
                        url: '/pages/index/default/default'
                    })
                })
            }
        }
    }
</script>

<style lang="scss">
    page { background: #fff; }
</style>
