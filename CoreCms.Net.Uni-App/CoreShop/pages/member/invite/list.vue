<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="邀请列表"></u-navbar>
        <!-- 邀请列表 -->
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
                                <view class="child-num u-margin-left-30">下级成员：{{ childNum || 0 }}人</view>
                            </view>
                        </view>
                    </view>

                    <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
                </view>

                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无邀请列表" mode="list"></u-empty>
                </view>

            </view>
        </view>
    </view>
</template>

<script>
    export default {
        data() {
            return {
                list: [],
                page: 1, //当前页
                limit: 20, //每页显示几条
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                }
            };
        },
        onLoad() {
            this.getDataList();
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getDataList()
            }
        },
        methods: {
            getDataList() {
                this.status = 'loading'
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
                            this.status = 'loadmore'
                        } else {
                            this.status = 'nomore'
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
    page { background: #fff; }
</style>
