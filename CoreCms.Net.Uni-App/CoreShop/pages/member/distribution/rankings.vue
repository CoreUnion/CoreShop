<!-- 分销排行 -->
<template>
    <view class="rankings-wrap bg-red">
        <!-- 标题栏 -->
        <view class="nav-box">
            <cu-custom isBack>
                <block slot="backText">分销排行</block>
            </cu-custom>
        </view>

        <!-- 排行榜 -->
        <view class="rankings-list-box">
            <scroll-view :style="'height:'+viewHeight+'px'" scroll-y="true" @scrolltolower="loadMore" class="scroll-box">
                <view class="ranking-list x-bc" v-for="(item, index) in rankingsList" :key="index">
                    <view class="list-left x-f">
                        <view class="tag-box x-c">
                            <text class="tag-text" v-if="index >= 3">{{ index }}</text>
                            <image v-else class="tag-icon" :src="rankingsIcon[index]" mode=""></image>
                        </view>
                        <image class="user-avatar" :src="item.user.avatar" mode=""></image>
                        <view class="user-info">
                            <view class="name mb10">{{ item.nickname }}</view>
                            <view class="date">{{ $u.timeFormat(item.createtime, 'yyyy年mm月dd日') }}</view>
                        </view>
                    </view>
                    <view class="list-right y-end">
                        <view class="num mb10">{{ item.totalInCome }}</view>
                        <view class="des">累计收益</view>
                    </view>
                </view>
                <!-- 更多 -->
                <u-loadmore v-if="rankingsList.length" height="80rpx" :status="loadStatus" icon-type="flower" color="#ccc" />
            </scroll-view>
        </view>
    </view>
</template>

<script>
    export default {
        components: {},
        data() {
            return {
                rankingsIcon: {
                    0: '/static/images/distribution/01.png',
                    1: '/static/images/distribution/02.png',
                    2: '/static/images/distribution/03.png'
                },
                rankingsList: [], //排行榜
                loadStatus: 'loadmore', //loadmore-加载前的状态，loading-加载中的状态，nomore-没有更多的状态
                currentPage: 1,
                limit: 20,
                viewHeight: 0,
                hasNextPage: false
            };
        },
        onShow() {
            var _this = this;
            uni.getSystemInfo({
                success: function (res) { // res - 各种参数
                    console.log(res); // 屏幕的宽度
                    var windowHeight = res.windowHeight;

                    let info = uni.createSelectorQuery().select(".nav-box");
                    info.boundingClientRect(function (data) { //data - 各种参数
                        var headHeight = data.height;
                        _this.viewHeight = windowHeight - headHeight;

                    }).exec()
                }
            });
        },
        onLoad() {
            this.getRankings();
        },
        methods: {
            getRankings() {
                let that = this;

                let data = {
                    page: this.currentPage,
                    limit: this.limit,
                }
                that.loadStatus = 'loading';
                this.$u.api.getDistributionRanking(data).then(res => {
                    if (res.code === 0) {
                        that.rankingsList = [...that.rankingsList, ...res.data.data];
                        that.hasNextPage = res.data.hasNextPage;
                        if (that.hasNextPage) {
                            that.currentPage++;
                            that.loadStatus = 'loadmore';
                        } else {
                            that.loadStatus = 'nomore';
                        }
                    }
                });
            },

            // 加载更多
            loadMore() {
                if (this.hasNextPage) {
                    this.getRankings();
                }
            }
        }
    };
</script>

<style lang="scss">
    .rankings-wrap { background-image: url('/static/images/common/bg.png'); background-size: cover; background-position: center; overflow: hidden; }
    .nav-box .cu-back { color: #fff; font-size: 40rpx; }
    .nav-box .head-title { font-size: 38rpx; color: #fff; }
    .rankings-list-box { background-color: #fff; border-radius: 20rpx 20rpx 0px 0px; width: 690rpx; margin: 60rpx auto 0; margin-bottom: 60rpx; }
        .rankings-list-box .ranking-list { height: 140rpx; padding: 0 30rpx; border-bottom: 1rpx solid #e5e5e5; }
            .rankings-list-box .ranking-list .list-left .tag-box { width: 50rpx; font-size: 36rpx; font-weight: 500; color: #beb4b3; margin-right: 20rpx; }
                .rankings-list-box .ranking-list .list-left .tag-box .tag-icon { width: 40rpx; height: 60rpx; }
            .rankings-list-box .ranking-list .list-left .user-avatar { width: 66rpx; height: 66rpx; border-radius: 50%; margin-right: 30rpx; }
            .rankings-list-box .ranking-list .list-left .user-info .name { font-size: 28rpx; font-weight: bold; color: #333333; }
            .rankings-list-box .ranking-list .list-left .user-info .date { font-size: 24rpx; font-weight: 400; color: #999999; }
            .rankings-list-box .ranking-list .list-right .num { font-size: 30rpx; font-weight: 500; color: #5e4ddf; }
            .rankings-list-box .ranking-list .list-right .des { font-size: 24rpx; font-weight: 500; color: #a09a98; }
</style>
