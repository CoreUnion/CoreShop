<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="设置"></u-navbar>

        <u-cell-group>
            <u-cell-item icon="setting" title="个人资料" @click="navigateToHandle('/pages/member/setting/userInfo/index')"></u-cell-item>
            <!-- #ifdef MP-WEIXIN  -->
            <u-cell-item icon="reload" title="同步微信昵称头像" @click="syncWeChatInfo()"></u-cell-item>
            <!-- #endif  -->
            <u-cell-item icon="warning" title="修改密码" @click="navigateToHandle('/pages/member/setting/userInfo/password')"></u-cell-item>
            <u-cell-item icon="tags" title="关于我们" @click="goAboutUs()"></u-cell-item>
            <u-cell-item icon="order" title="用户协议" @click="goUserAgreementPage()"></u-cell-item>
            <u-cell-item icon="eye-off" title="隐私政策" @click="goUserPrivacyPolicy()"></u-cell-item>
            <u-cell-item icon="trash" title="清除缓存" @click="clearCache"></u-cell-item>
        </u-cell-group>

        <!--按钮-->
        <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
            <view class="flex u-padding-20 flex-direction">
                <u-button :custom-style="customStyle" type="error" size="medium" @click="logOff">重新登录</u-button>
            </view>
        </view>
    </view>
</template>

<script>
    import { commonUse, articles } from '@/common/mixins/mixinsHelper.js';
    export default {
        data() {
            return {
                customStyle: {
                    width: '100%',
                }
            }
        },
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
            },
        }
    }
</script>

<style lang="scss">
    page { background: #fff; }
</style>
