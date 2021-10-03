<!-- 分享记录 -->
<template>
    <view>
        <!-- 标题栏 -->
        <u-navbar title="分享记录"></u-navbar>
        <!-- 分类tab -->
        <view class="tab-box u-flex u-row-between u-padding-left-20 u-padding-right-20">
            <view class="tab-item flex-sub " v-for="(tab, index) in tabsList" :key="tab.value" @tap="onTab(tab.value)">
                <text class="tab-title" :class="{ 'title-active': tabCurrent === tab.value }">{{ tab.name }}</text>
                <text class="underline" :class="{ 'underline-active': tabCurrent === tab.value }"></text>
            </view>
        </view>

        <view class="content_box">
            <scroll-view scroll-y="true" @scrolltolower="loadMore" class="scroll-box">
                <!-- 分享记录列表 -->
                <view class="log-list coreshop-flex coreshop-align-center" v-for="item in shareLogList" :key="item.id">
                    <view class="log-avatar-wrap"><image class="log-avatar" :src="item.user.avatar" mode=""></image></view>

                    <view class="item-right">
                        <view class="name">{{ item.user.nickname }}</view>
                        <view class="content coreshop-flex coreshop-align-center" v-if="item.type_data">
                            <view class="content-img-wrap"><image class="content-img" :src="item.type_data.image" mode=""></image></view>

                            <view class="content-text">通过{{ typeObj[item.type] }}访问了商品“{{ item.type_data.title }}”, 进入商城</view>
                        </view>
                        <view class="item-bottom u-flex u-row-between">
                            <view class="time">{{ $u.timeFormat(item.createtime, 'yyyy年mm月dd日 hh:MM') }}</view>
                            <view class="from-text">来自{{ typeObj[item.type] }}分享</view>
                        </view>
                    </view>
                </view>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-if="shareLogList.length<=0">
                    <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无分享记录" mode="list"></u-empty>
                </view>
                <!-- 更多 -->
                <u-loadmore v-if="shareLogList.length" :status="loadStatus" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
            </scroll-view>
        </view>
        <view class="foot_box"></view>
    </view>
</template>

<script>
    export default {
        components: {},
        data() {
            return {
                shareLogList: [], //分享记录
                tabCurrent: 'all', //默认
                tabsList: [
                    {
                        name: '全部',
                        value: 'all'
                    },
                    {
                        name: '名片',
                        value: 'index'
                    },
                    {
                        name: '商品',
                        value: 'goods'
                    },
                    {
                        name: '拼团',
                        value: 'groupon'
                    }
                ],
                typeObj: {
                    index: '名片',
                    goods: '商品',
                    groupon: '拼团'
                },
                loadStatus: 'loadmore', //loadmore-加载前的状态，loading-加载中的状态，nomore-没有更多的状态
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                },
                currentPage: 1,
                lastPage: 1,
            };
        },
        computed: {},
        onLoad() {
            this.getShareLog();
        },
        methods: {
            // 切换分类
            onTab(type) {
                this.tabCurrent = type;
                this.currentPage = 1;
                this.lastPage = 1;
                this.shareLogList = [];
                this.$u.debounce(this.getShareLog);
            },

            // 分享记录数据
            getShareLog() {
                let that = this;
                that.loadStatus = 'loading';
                //that.$api('commission.share', {
                //    type: that.tabCurrent,
                //    page: that.currentPage
                //}).then(res => {
                //    if (res.code === 1) {
                //        that.shareLogList = [...that.shareLogList, ...res.data.data];
                //        that.lastPage = res.data.last_page;
                //        if (that.currentPage < res.data.last_page) {
                //            that.loadStatus = 'loadmore';
                //        } else {
                //            that.loadStatus = 'nomore';
                //        }
                //    }
                //});
            },

            // 加载更多
            loadMore() {
                if (this.currentPage < this.lastPage) {
                    this.currentPage += 1;
                    this.getShareLog();
                }
            }
        }
    };
</script>

<style lang="scss">
    @import "shareLog.scss";
</style>
