<!-- 分销排行 -->
<template>
    <view class="rankings-wrap coreshop-bg-blue">
        <u-navbar title="代理排行"></u-navbar>
        <!-- 排行榜 -->
        <view class="rankings-list-box">
            <scroll-view scroll-y="true" @scrolltolower="loadMore" class="scroll-box">
                <view class="ranking-list u-flex u-row-between" v-for="(item, index) in rankingsList" :key="index">
                    <view class="list-left coreshop-flex coreshop-align-center">
                        <view class="tag-box coreshop-flex coreshop-justify-center coreshop-align-center">
                            <text class="tag-text" v-if="index >= 3">{{ index }}</text>
                            <image v-else class="tag-icon" :src="rankingsIcon[index]" mode=""></image>
                        </view>
                        <image class="user-avatar" :src="item.user.avatar" mode=""></image>
                        <view class="user-info">
                            <view class="name mb10">{{ item.nickname }}</view>
                            <view class="date">{{ $u.timeFormat(item.createtime, 'yyyy年mm月dd日') }}</view>
                        </view>
                    </view>
                    <view class="list-right coreshop-flex coreshop-flex-direction coreshop-align-end">
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
                hasNextPage: false
            };
        },
        onShow() {
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
                this.loadStatus = 'loading';
                this.$u.api.getAgentRanking(data).then(res => {
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
    @import "rankings.scss";
</style>
