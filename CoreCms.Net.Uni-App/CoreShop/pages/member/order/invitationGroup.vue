<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="邀请拼单"></u-navbar>
        <view class="content">
            <view class="ig-top">
                <view class="ig-top-t">
                    <view class="">
                        剩余： <u-count-down :timestamp="item.lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="24" separator-size="24" @end="end(key)"></u-count-down>
                    </view>
                </view>
                <view class="ig-top-m">
                    <view class="user-head-img-c" v-for="(item, index) in teamInfo.list" :key="index">
                        <view class="user-head-img-tip" v-if="item.recordId == teamInfo.teamId">拼主</view>
                        <image class="user-head-img cell-hd-icon have-none" :src='item.userAvatar' mode=""></image>
                    </view>
                    <view class="user-head-img-c uhihn" v-if="teamInfo.teamNums" v-for="n in teamInfo.teamNums" :key="n"><text>?</text></view>
                </view>
                <view class="ig-top-b">
                    <view class="igtb-top">
                        还差<text class="red-price">{{ teamInfo.teamNums }}</text>人，赶快邀请好友来拼单吧
                    </view>
                    <view class="igtb-mid">
                        <button class="coreshop-btn" @click="goShare()">邀请好友拼单</button>
                    </view>
                    <view class="igtb-bot">
                        分享好友越多，成团越快
                    </view>
                </view>
            </view>
            <!-- 弹出层 -->
            <lvv-popup position="bottom" ref="share">

                <!-- #ifdef H5 -->
                <shareByH5 :shareType='3' :goodsId="goodsInfo.goodsId" :teamId="teamInfo.teamId" :groupId="teamInfo.ruleId"
                           :shareImg="goodsInfo.image_url" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref"
                           @close="closeShare()"></shareByH5>
                <!-- #endif -->
                <!-- #ifdef MP-WEIXIN -->
                <shareByWx :shareType='3' :goodsId="goodsInfo.goodsId" :teamId="teamInfo.teamId" :groupId="teamInfo.ruleId"
                           :shareImg="goodsInfo.image_url" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref"
                           @close="closeShare()"></shareByWx>
                <!-- #endif -->
                <!-- #ifdef MP-ALIPAY -->
                <shareByAli :shareType='3' :goodsId="goodsInfo.goodsId" :teamId="teamInfo.teamId" :groupId="teamInfo.ruleId"
                            :shareImg="goodsInfo.image_url" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref"
                            @close="closeShare()"></shareByAli>
                <!-- #endif -->
                <!-- #ifdef MP-TOUTIAO -->
                <shareByTt :shareType='3' :goodsId="goodsInfo.goodsId" :teamId="teamInfo.teamId" :groupId="teamInfo.ruleId"
                           :shareImg="goodsInfo.image_url" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref"
                           @close="closeShare()"></shareByTt>
                <!-- #endif -->
                <!-- #ifdef APP-PLUS || APP-PLUS-NVUE -->
                <shareByApp :shareType='3' :goodsId="goodsInfo.goodsId" :teamId="teamInfo.teamId" :groupId="teamInfo.ruleId"
                            :shareImg="goodsInfo.image_url" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref"
                            @close="closeShare()"></shareByApp>
                <!-- #endif -->

            </lvv-popup>
            <view class="cell-group margin-cell-group">
                <view class='cell-item'>
                    <view class='cell-item-hd'>
                        <view class='cell-hd-title'>商品名称</view>
                    </view>
                    <view class='cell-item-ft'>
                        <text class="cell-ft-text">{{ goodsInfo.name }}</text>
                    </view>
                </view>
                <view class='cell-item'>
                    <view class='cell-item-hd'>
                        <view class='cell-hd-title'>拼单时间</view>
                    </view>
                    <view class='cell-item-ft'>
                        <text class="cell-ft-text">{{ orderInfo.createTime }}</text>
                    </view>
                </view>
                <view class='cell-item'>
                    <view class='cell-item-hd'>
                        <view class='cell-hd-title'>拼单须知</view>
                    </view>
                    <view class='cell-item-ft group-notice'>
                        <text class="cell-ft-text">* 好友拼单 </text>
                        <text class="cell-ft-text">* 人满发货 </text>
                        <text class="cell-ft-text">* 人不满退款 </text>
                    </view>
                </view>
            </view>
        </view>
    </view>
</template>

<script>
    import lvvPopup from '@/components/corecms-lvv-popup/corecms-lvv-popup.vue';
    import { get } from '@/common/utils/dbHelper.js';
    import { apiBaseUrl } from '@/common/setting/constVarsHelper.js';
    // #ifdef H5
    import shareByH5 from '@/components/corecms-share/shareByh5.vue'
    // #endif
    // #ifdef MP-WEIXIN
    import shareByWx from '@/components/corecms-share/shareByWx.vue'
    // #endif
    // #ifdef MP-ALIPAY
    import shareByAli from '@/components/corecms-share/shareByAli.vue'
    // #endif
    // #ifdef MP-TOUTIAO
    import shareByTt from '@/components/corecms-share/shareByTt.vue'
    // #endif
    // #ifdef APP-PLUS || APP-PLUS-NVUE
    import shareByApp from '@/components/corecms-share/shareByApp.vue'
    // #endif

    export default {
        components: {
            lvvPopup,
            // #ifdef H5
            shareByH5,
            // #endif
            // #ifdef MP-WEIXIN
            shareByWx,
            // #endif
            // #ifdef MP-ALIPAY
            shareByAli,
            // #endif
            // #ifdef MP-TOUTIAO
            shareByTt,
            // #endif
            // #ifdef APP-PLUS || APP-PLUS-NVUE
            shareByApp,
            // #endif
            // spec
        },
        data() {
            return {
                shareType: 3,
                goodsInfo: [],
                teamInfo: [],

                query: '', // query参数登录跳转回来使用

                lasttime: {
                    day: 0,
                    hour: 0,
                    minute: 0,
                    second: 0
                }, //购买倒计时
                userToken: 0,
                time: 0,
                orderId: '',//订单号
                orderInfo: {},
                shareUrl: '/pages/share/jump/jump'
            }
        },
        onLoad(options) {
            if (options.orderId) {
                this.orderId = options.orderId;
            } else {
                this.$u.toast('参数错误');
            }
            let teamInfo, orderInfo, goodsInfo

            let pages = getCurrentPages()
            let pre = pages[pages.length - 2]
            if (typeof pre != 'undefined') {
                // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE
                teamInfo = pre.teamInfo
                orderInfo = pre.orderInfo
                // #endif
                // #ifdef MP-WEIXIN
                teamInfo = pre.$vm.teamInfo
                orderInfo = pre.$vm.orderInfo
                // #endif
                // #ifdef MP-ALIPAY || MP-TOUTIAO
                teamInfo = pre.data.teamInfo;
                orderInfo = pre.data.orderInfo
                // #endif
            }
            if (teamInfo && orderInfo) {
                this.teamInfo = teamInfo;
                this.orderInfo = orderInfo;
                this.goodsInfo = orderInfo.items[0];

            } else {
                this.orderDetail();
                this.getTeam();
            }
        },
        computed: {
            shareHref() {
                let pages = getCurrentPages()
                let page = pages[pages.length - 1]
                // #ifdef H5 || MP-WEIXIN || APP-PLUS || APP-PLUS-NVUE || MP-TOUTIAO
                return apiBaseUrl + 'wap/' + page.route + '?scene=' + this.query;
                // #endif

                // #ifdef MP-ALIPAY
                return apiBaseUrl + 'wap/' + page.__proto__.route + '?scene=' + this.query;
                // #endif
            }
        },
        methods: {
            //拼团信息
            getTeam() {
                this.$u.api.getOrderPinTuanTeamInfo({ orderId: this.orderId }).then(res => {
                    if (res.status) {
                        this.teamInfo = {
                            list: res.data.teams,
                            userAvatar: res.data.userAvatar,
                            currentCount: res.data.teams.length,
                            peopleNumber: res.data.peopleNumber,
                            teamNums: res.data.teamNums, //剩余
                            closeTime: res.data.closeTime, //关闭时间
                            id: res.data.id, //拼团id
                            teamId: res.data.teamId, //拼团团队id
                            ruleId: res.data.ruleId,
                            status: res.data.status
                        };
                    } else {
                        this.$u.toast(res.msg)
                    }

                });
            },
            //获取订单详情
            orderDetail() {
                let _this = this
                let data = {
                    id: _this.orderId
                }
                _this.$u.api.orderDetail(data).then(res => {
                    if (res.status) {
                        let data = res.data

                        _this.orderInfo = data
                        _this.goodsInfo = data.items[0];

                    } else {
                        _this.$u.toast(res.msg)
                    }
                })
            },
            // 关闭弹出层
            close() {
                this.$emit('close')
            },
            // 点击操作
            clickHandler(e) {
                if (e.cate === 'poster') {
                    this.createPoster()
                } else {
                    // 去分享
                    this.share(e)
                }
            },
            // 显示modal弹出框
            toshow(type, teamId = 0) {
                if (type == 1) {
                    this.lvvpopref_type = 1;
                }
                if (teamId !== 0) {
                    this.teamId = teamId;
                }
                this.$refs.lvvpopref.show();
            },
            // 关闭modal弹出框
            toclose() {
                this.$refs.lvvpopref.close();
            },
            // 跳转到h5分享页面
            goShare() {
                this.$refs.share.show();
            },
            closeShare() {
                this.$refs.share.close();
            },
            //获取分享URL
            getShareUrl() {
                let data = {
                    client: 2,
                    url: "/pages/share/jump/jump",
                    type: 1,
                    page: 3,
                    params: {
                        goodsId: this.goodsInfo.goodsId,
                        teamId: this.teamInfo.list[0].teamId
                    }
                };
                let userToken = this.$db.get('userToken');
                if (userToken && userToken != '') {
                    data['token'] = userToken;
                }
                this.$u.api.share(data).then(res => {
                    this.shareUrl = res.data
                });
            }
        },
        watch: {
            goodsInfo: {
                handler() {
                    this.getShareUrl();
                },
                deep: true
            },
            teamInfo: {
                handler() {
                    this.getShareUrl();
                },
                deep: true
            }
        },
        //分享
        onShareAppMessage(res) {
            return {
                title: this.$store.state.config.shareTitle,
                imageUrl: this.$store.state.config.shareImage,
                path: this.shareUrl
            }
        },
        onShareTimeline(res) {
            return {
                title: this.$store.state.config.shareTitle,
                imageUrl: this.$store.state.config.shareImage,
                path: this.shareUrl
            }
        },
    }
</script>

<style lang="scss" scoped>
    .ig-top { text-align: center; background-color: #fff; padding: 20upx 26upx; }
    .ig-top-t,
    .ig-top-m { margin-bottom: 20upx; }
        .ig-top-t > view { display: inline-block; padding: 0 10upx; color: #999; }
    .user-head-img-c { position: relative; width: 80upx; height: 80upx; border-radius: 50%; margin-right: 20upx; box-sizing: border-box; display: inline-block; border: 1px solid #f3f3f3; }
    .user-head-img-tip { position: absolute; top: -6upx; left: -10upx; display: inline-block; background-color: #FF7159; color: #fff; font-size: 22upx; z-index: 98; padding: 0 10upx; border-radius: 10upx; transform: scale(.8); }
    .user-head-img-c .user-head-img { width: 100%; height: 100%; border-radius: 50%; }
    .user-head-img-c:first-child { border: 1px solid #FF7159; }
    .uhihn { width: 80upx; height: 80upx; border-radius: 50%; display: inline-block; border: 2upx dashed #e1e1e1; text-align: center; color: #d1d1d1; font-size: 40upx; box-sizing: border-box; position: relative; }
        .uhihn > text { position: absolute; left: 50%; top: 50%; transform: translate(-50%, -50%); }
    .igtb-top { font-size: 32upx; color: #333; margin-bottom: 16upx; }
    .igtb-mid { margin-bottom: 16upx; }
        .igtb-mid .coreshop-btn { width: 100%; background-color: #FF7159; color: #fff; }
    .igtb-bot { font-size: 24upx; color: #666; }
    .cell-ft-text { max-width: 520upx; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .group-notice .cell-ft-text { color: #999; margin-left: 20upx; font-size: 26upx; }
</style>