<!-- 我的团队 -->
<template>
    <view>
        <u-navbar title="我的推广"></u-navbar>
        <view class="head_box coreshop-bg-blue u-padding-top-20  u-padding-bottom-20">
            <!-- 推荐人 -->
            <view class="referrer-box coreshop-flex coreshop-align-center u-padding-left-30  u-padding-right-30" v-if="referrerInfo && referrerInfo.avatarImage">
                上级推荐人：
                <image class="referrer-avatarImage" :src="referrerInfo.avatarImage" mode=""></image>
                {{ referrerInfo.nickName }}
            </view>
            <view class="referrer-box coreshop-flex coreshop-align-center  u-padding-left-30  u-padding-right-30" v-else>
                上级推荐人：无
            </view>

            <!-- 团队数据总览 -->
            <view class="team-data-box u-flex u-row-between">
                <view class="data-card">
                    <view class="total-item">
                        <view class="item-title">团队总人数(人)</view>
                        <view class="total-num">{{ userInfo.count || 0 }}</view>
                    </view>
                    <view class="category-item u-flex u-row-between">
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-start flex-sub">
                            <view class="item-title">总人数</view>
                            <view class="category-num">{{ userInfo.first || 0 }}</view>
                        </view>
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-start flex-sub">
                            <view class="item-title">总订单</view>
                            <view class="category-num">{{ userInfo.second || 0 }}</view>
                        </view>
                    </view>
                </view>
                <view class="data-card">
                    <view class="total-item">
                        <view class="item-title">本月推广人数(人)</view>
                        <view class="total-num">{{ userInfo.monthCount || 0 }}</view>
                    </view>
                    <view class="category-item u-flex u-row-between">
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-start flex-sub">
                            <view class="item-title">人数</view>
                            <view class="category-num">{{ userInfo.monthFirst || 0 }}</view>
                        </view>
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-start ">
                            <view class="item-title">订单</view>
                            <view class="category-num">{{ userInfo.monthSecond || 0 }}</view>
                        </view>
                    </view>
                </view>
            </view>

            <!-- 筛选 TODO -->
            <!--<view class="filter-box coreshop-flex coreshop-align-center">
                <view class="filter-item flex-sub" v-for="(filter, index) in filterList" :key="index" @tap="onFilter(index)">
                    <view class="coreshop-flex coreshop-align-center">
                        <text class="filter-title" :class="{ 'title-active': filterCurrent == index }">{{ filter.title }}</text>
                        <text v-show="index" class="cuIcon-unfold" :class="{ 'icon-active': filter.isUnfold }"></text>
                    </view>
                    <text class="underline" :class="{ 'underline-active': filterCurrent == index }"></text>
                </view>
            </view>-->
        </view>
        <scroll-view :style="'height:'+viewHeight+'px'" scroll-y="true" class="scroll-Y">
            <!-- 团队列表 -->
            <view class="coreshop-team-box">
                <view class="coreshop-team-list">
                    <view v-if="list.length">
                        <view class="coreshop-team-children coreshop-flex coreshop-align-center" v-for="children in list" :key="children.id">
                            <image class="head-img" :src="children.avatarImage" mode=""></image>
                            <view class="head-info">
                                <view class="name-box u-flex u-row-between">
                                    <view class="name-text">{{ children.nickName }}</view>
                                    <view class="coreshop-flex coreshop-align-center">
                                        <text class="cu-tag bg-grey sm radius">{{  children.mobile }}</text>
                                    </view>
                                </view>
                                <view class="u-flex u-row-between">
                                    <view class="head-time">{{ $u.timeFormat(children.createTime, 'yyyy年mm月dd日') }}</view>
                                    <!--<view class="child-num u-margin-left-30">下级成员：{{ childNum || 0 }}人</view>-->
                                </view>
                            </view>
                        </view>
                        <u-loadmore :status="loadStatus" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
                    </view>
                    <!-- 无数据时默认显示 -->
                    <view class="coreshop-emptybox" v-else>
                        <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无邀请列表" mode="list"></u-empty>
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
                userInfo: {
                    count: 0,
                    first: 0,
                    second: 0,
                    monthCount: 0,
                    monthFirst: 0,
                    monthSecond: 0,
                },
                referrerInfo: {}, //推荐人信息
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
            // 点击筛选项
            onFilter(index) {
                this.filterCurrent = index;
                if (this.filterCurrent == index) {
                    this.filterList[index].isUnfold = !this.filterList[index].isUnfold;
                }
            },
            getReferrerInfo() {
                this.$u.api.getMyInvite(null).then(res => {
                    if (res.status) {
                        this.referrerInfo = res.data;
                    } else {
                        this.$u.toast(res.msg)
                    }
                });
                this.$u.api.getAgentTeamSum(null).then(res => {
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
    @import "team.scss";
</style>
