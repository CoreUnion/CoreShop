<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="邀请拼单"></u-navbar>
        <view class="content">
            <view class="u-text-center coreshop-bg-white u-padding-20 u-flex u-flex-wrap">
                <view class="ig-top-t u-margin-bottom-20 w100">
                    剩余： <u-count-down :timestamp="teamInfo.lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="24" separator-size="24" @end="end(key)"></u-count-down>
                </view>
                <view class="u-margin-bottom-20 w100 u-text-center u-flex u-flex-nowrap u-row-center">
                    <view class="user-head-img-c" v-for="(item, index) in teamInfo.list" :key="index">
                        <view class="user-head-img-tip" v-if="item.recordId == teamInfo.teamId">拼主</view>
                        <image class="user-head-img coreshop-head-icon" :src='item.userAvatar' mode=""></image>
                    </view>
                    <view class="user-head-img-c no-head-icon" v-if="teamInfo.teamNums" v-for="n in teamInfo.teamNums" :key="n"><text>?</text></view>
                </view>
                <view class="u-margin-bottom-20 w100">
                    <view class="u-font-32 coreshop-text-black u-margin-bottom-20">
                        还差<text class="coreshop-text-red">{{ teamInfo.teamNums }}</text>人，赶快邀请好友来拼单吧
                    </view>
                    <view class="u-margin-bottom-20">
                        <u-button :custom-style="customStyle" type="success" size="medium" @click="goShare">邀请好友拼单</u-button>
                    </view>
                    <view class="u-font-24 coreshop-text-gray">
                        分享好友越多，成团越快
                    </view>
                </view>
            </view>

            <!-- 弹出层 -->
            <view class="u-padding-10">
                <u-popup mode="bottom" v-model="shareBox" ref="share">
                    <shareByWx :shareType='3' :goodsId="goodsInfo.goodsId" :teamId="teamInfo.teamId" :groupId="teamInfo.ruleId"
                               :shareImg="goodsInfo.image_url" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref"
                               @close="closeShare()"></shareByWx>
                </u-popup>
            </view>

            <view class="coreshop-cell-group u-margin-top-20 u-margin-bottom-20">
                <view class='coreshop-cell-item'>
                    <view class='coreshop-cell-item-hd'>
                        <view class='coreshop-cell-hd-title'>商品名称</view>
                    </view>
                    <view class='coreshop-cell-item-ft'>
                        <text class="coreshop-cell-ft-text">{{ goodsInfo.name }}</text>
                    </view>
                </view>
                <view class='coreshop-cell-item'>
                    <view class='coreshop-cell-item-hd'>
                        <view class='coreshop-cell-hd-title'>拼单时间</view>
                    </view>
                    <view class='coreshop-cell-item-ft'>
                        <text class="coreshop-cell-ft-text">{{ orderInfo.createTime }}</text>
                    </view>
                </view>
                <view class='coreshop-cell-item'>
                    <view class='coreshop-cell-item-hd'>
                        <view class='coreshop-cell-hd-title'>拼单须知</view>
                    </view>
                    <view class='coreshop-cell-item-ft group-notice'>
                        <text class="coreshop-cell-ft-text">* 好友拼单 </text>
                        <text class="coreshop-cell-ft-text">* 人满发货 </text>
                        <text class="coreshop-cell-ft-text">* 人不满退款 </text>
                    </view>
                </view>
            </view>
        </view>
    </view>
</template>

<script>
    import shareByWx from '@/components/coreshop-share/shareByWx.vue'

    export default {
        components: {
            shareByWx
        },
        data() {
            return {
                customStyle: {
                    width: '100%',
                },
                shareType: 3,
                goodsInfo: [],
                teamInfo: [],
                query: '', // query参数登录跳转回来使用
                userToken: 0,
                orderId: '',//订单号
                orderInfo: {},
                shareUrl: '/pages/share/jump/jump',
                shareBox: false,
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
                teamInfo = pre.$vm.teamInfo
                orderInfo = pre.$vm.orderInfo
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
                return this.$globalConstVars.apiBaseUrl + 'wap/' + page.route + '?scene=' + this.query;
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
                            status: res.data.status,
                            lastTime: res.data.lastTime
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
                this.shareBox = true;
            },
            closeShare() {
                this.shareBox = false;
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
    @import "invitationGroup.scss";
</style>