<!-- 我的团队 -->
<template>
    <view class="pageBox team-wrap">
        <view class="head_box bg-red shadow-warp">
            <cu-custom isBack bgColor="text-white">
                <block slot="backText">我的团队</block>
            </cu-custom>
            <!-- 推荐人 -->
            <view class="referrer-box x-f u-padding-left-30  u-padding-right-30" v-if="referrerInfo && referrerInfo.avatarImage">
                推荐人：
                <image class="referrer-avatarImage" :src="referrerInfo.avatarImage" mode=""></image>
                {{ referrerInfo.nickName }}
            </view>
            <view class="referrer-box x-f  u-padding-left-30  u-padding-right-30" v-else>
                推荐人：无
            </view>
            <!-- 团队数据总览 -->
            <view class="team-data-box x-bc">
                <view class="data-card">
                    <view class="total-item">
                        <view class="item-title">团队总人数(人)</view>
                        <view class="total-num">{{ userInfo.count || 0 }}</view>
                    </view>
                    <view class="category-item x-f">
                        <view class="y-start flex-sub">
                            <view class="item-title">一级成员</view>
                            <view class="category-num">{{ userInfo.first || 0 }}</view>
                        </view>
                        <view class="y-start flex-sub">
                            <view class="item-title">二级成员</view>
                            <view class="category-num">{{ userInfo.second || 0 }}</view>
                        </view>
                    </view>
                </view>
                <view class="data-card">
                    <view class="total-item">
                        <view class="item-title">本月推广人数(人)</view>
                        <view class="total-num">{{ userInfo.monthCount || 0 }}</view>
                    </view>
                    <view class="category-item x-f">
                        <view class="y-start flex-sub">
                            <view class="item-title">一级成员</view>
                            <view class="category-num">{{ userInfo.monthFirst || 0 }}</view>
                        </view>
                        <view class="y-start ">
                            <view class="item-title">二级成员</view>
                            <view class="category-num">{{ userInfo.monthSecond || 0 }}</view>
                        </view>
                    </view>
                </view>
            </view>
            <!-- 筛选 TODO -->
            <view class="filter-box x-f" v-if="false">
                <view class="filter-item flex-sub" v-for="(filter, index) in filterList" :key="index" @tap="onFilter(index)">
                    <view class="x-f">
                        <text class="filter-title" :class="{ 'title-active': filterCurrent == index }">{{ filter.title }}</text>
                        <text v-show="index" class="cuIcon-unfold" :class="{ 'icon-active': filter.isUnfold }"></text>
                    </view>
                    <text class="underline" :class="{ 'underline-active': filterCurrent == index }"></text>
                </view>
            </view>
        </view>
        <scroll-view :style="'height:'+viewHeight+'px'" scroll-y="true" class="scroll-Y">
            <!-- 团队列表 -->
            <view class="team-box">
                <view class="team-list">
                    <view v-if="list.length">
                        <view class="team-children x-f" v-for="children in list" :key="children.id">
                            <image class="head-img" :src="children.avatarImage" mode=""></image>
                            <view class="head-info">
                                <view class="name-box x-bc">
                                    <view class="name-text">{{ children.nickName }}</view>
                                    <view class="x-f">
                                        <text class="cu-tag bg-grey sm radius">{{  children.mobile }}</text>
                                    </view>
                                </view>
                                <view class="x-bc">
                                    <view class="head-time">{{ $u.timeFormat(children.createTime, 'yyyy年mm月dd日') }}</view>
                                    <view class="child-num u-margin-left-30">下级成员：{{ children.childNum || 0 }}人</view>
                                </view>
                            </view>
                        </view>

                        <u-loadmore :status="loadStatus" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
                    </view>

                    <!-- 无数据时默认显示 -->
                    <view class="coreshop-emptybox" v-else>
                        <u-empty :src="$apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无邀请列表" mode="list"></u-empty>
                    </view>

                </view>
            </view>
        </scroll-view>
    </view>
</template>

<script>

    export default {
        data() {
            return {
                emptyData: {
                    show: false,
                    img: this.$IMG_URL + '/imgs/empty/no_team.png',
                    tip: '暂无团队人员',
                    path: '',
                    pathText: ''
                },
                userInfo: {
                    count: 0,
                    first: 0,
                    second: 0,
                    monthCount: 0,
                    monthFirst: 0,
                    monthSecond: 0,

                },
                referrerInfo: {}, //推荐人信息
                twoTeamCount: 0, //二级成员
                agentInfo: uni.getStorageSync('agentInfo'),
                filterCurrent: 0,
                filterList: [
                    {
                        title: '综合',
                        isUnfold: false
                    },
                    {
                        title: '等级',
                        isUnfold: false
                    },
                    {
                        title: '加入时间',
                        isUnfold: false
                    }
                ],
                list: [],
                page: 1, //当前页
                limit: 10, //每页显示几条
                loadStatus: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                },
                viewHeight: 0,
            };
        },
        onLoad() {
            this.getReferrerInfo();
            this.getDataList();

            var _this = this;
            uni.getSystemInfo({
                success: function (res) { // res - 各种参数
                    console.log(res); // 屏幕的宽度
                    var windowHeight = res.windowHeight;

                    let info = uni.createSelectorQuery().select(".head_box");
                    info.boundingClientRect(function (data) { //data - 各种参数
                        var headHeight = data.height;
                        _this.viewHeight = windowHeight - headHeight;

                    }).exec()
                }
            });


        },
        onReachBottom() {
            if (this.loadStatus === 'loadmore') {
                this.getDataList()
            }
        },
        methods: {
            getReferrerInfo() {
                this.$u.api.getMyInvite(null).then(res => {
                    if (res.status) {
                        this.referrerInfo = res.data;
                    } else {
                        this.$u.toast(res.msg)
                    }
                });
                this.$u.api.getDistributionTeamSum(null).then(res => {
                    if (res.status) {
                        this.userInfo.count = res.data.count;
                        this.userInfo.first = res.data.first;
                        this.userInfo.second = res.data.second;
                        this.userInfo.monthCount = res.data.monthCount;
                        this.userInfo.monthFirst = res.data.monthFirst;
                        this.userInfo.monthSecond = res.data.monthSecond;
                    } else {
                        this.$u.toast(res.msg)
                    }
                });
            },
            getDataList() {
                this.loadStatus = 'loading'
                let data = {
                    page: this.page,
                    limit: this.limit
                }
                this.$u.api.recommendUserList(data).then(res => {
                    if (res.status) {
                        for (let i = 0; i < res.data.length; i++) {
                            if (res.data[i].avatarImage == null) {
                                res.data[i].avatarImage = this.$store.state.config.shopDefaultImage;
                            }
                            if (res.data[i].nickName == null) {
                                res.data[i].nickName = '暂无昵称'
                            }
                        }
                        let list = this.list.concat(res.data);
                        this.list = list;
                        if (res.otherData.totalPages > this.page) {
                            this.page++
                            this.loadStatus = 'loadmore'
                        } else {
                            this.loadStatus = 'nomore'
                        }
                    } else {
                        this.$u.toast(res.msg)
                    }
                });
            }
        },
    };
</script>

<style lang="scss">
    .referrer-box { font-size: 28rpx; font-weight: 500; color: #ffffff; margin-top: 10rpx; }
        .referrer-box .referrer-avatarImage { width: 34rpx; height: 34rpx; border-radius: 50%; }

    .refresh-btn { width: 100%; line-height: 100rpx; background: #ffffff; border-radius: 25rpx; font-size: 22rpx; font-weight: 500; color: #999999; white-space: nowrap; }
        .refresh-btn .cuIcon-refresh { color: #dbdbdb; margin-right: 12rpx; font-size: 32rpx; }

    .refresh-active { transform: rotate(360deg); transition: all linear 0.5s; }

    .head_box { background-image: url('/static/images/common/bg.png'); background-size: cover; background-position: center; background-size: 100% auto; position: relative; }
        .head_box::after { content: ""; position: absolute; z-index: -1; background-color: inherit; width: 100%; height: 100%; left: 0; bottom: -10%; border-radius: 10upx; opacity: 0.2; transform: scale(0.9, 0.9); }
        .head_box .cu-back { color: #fff; font-size: 40rpx; }
        .head_box .head-title { font-size: 38rpx; color: #fff; }

    .team-data-box { margin: 30rpx 20rpx; }
        .team-data-box .data-card { width: 340rpx; background: #ffffff; border-radius: 20rpx; padding: 20rpx; }
            .team-data-box .data-card .item-title { font-size: 22rpx; font-weight: 500; color: #999999; line-height: 30rpx; margin-bottom: 10rpx; }
            .team-data-box .data-card .total-item { margin-bottom: 20rpx; }
            .team-data-box .data-card .total-num { font-size: 38rpx; font-weight: 500; color: #333333; }
            .team-data-box .data-card .category-num { font-size: 26rpx; font-weight: 500; color: #333333; }

    .filter-box { width: 750rpx; height: 95rpx; background: #ffffff; }
        .filter-box .filter-item { height: 100%; display: flex; flex-direction: column; align-items: center; justify-content: center; }
            .filter-box .filter-item .filter-title { color: #666; font-weight: 500; font-size: 28rpx; line-height: 90rpx; }
            .filter-box .filter-item .cuIcon-unfold { font-size: 24rpx; color: #c4c4c4; margin-left: 10rpx; transition: all linear 0.3s; }
            .filter-box .filter-item .icon-active { transform: rotate(180deg); transform-origin: center center; transition: all linear 0.3s; }
            .filter-box .filter-item .title-active { color: #333; }
            .filter-box .filter-item .underline { display: block; width: 68rpx; height: 4rpx; background: #fff; border-radius: 2rpx; }
            .filter-box .filter-item .underline-active { background: #5e49c3; display: block; width: 68rpx; height: 4rpx; border-radius: 2rpx; }
</style>
