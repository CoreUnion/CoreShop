<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="我的积分"></u-navbar>
        <view class="content">
            <view class="integral-top bg-red">
                <view class="u-font-md text-white  u-margin-bottom-20">
                    可用积分
                </view>
                <view class="text-xxl  text-white u-margin-bottom-20">
                    {{ pointList.length ? pointList[0].balance : 0}}
                </view>
                <view class="u-font-xs text-gray">
                    截止{{ nowDate }}可用积分
                </view>
            </view>
            
            <view class="item-box">
                <!-- 积分记录列表 -->
                <view v-if="pointList.length>0">
                    <view class="log-item x-bc" v-for="item in pointList" :key="item.id">
                        <view class="item-left x-f">
                            <!--<image class="log-img" :src="item.buyer.avatar" mode=""></image>-->
                            <view class="y-start">
                                <view class="log-name text-black">{{ item.typeName }}</view>
                                <view class="log-notice text-grey">{{ item.remarks }}</view>
                            </view>
                        </view>
                        <view class="item-right y-end">
                            <view class="log-num text-red">{{ item.num > 0 ? '+' + item.num : item.num }}</view>
                            <view class="log-date text-grey">{{ $u.timeFormat(item.createTime, 'yyyy.mm.dd') }}</view>

                        </view>
                    </view>
                    <!-- 更多 -->
                    <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
                </view>

                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无积分记录" mode="list"></u-empty>
                </view>
            </view>

        </view>
    </view>

</template>
<script>
    export default {
        data() {
            return {
                page: 1,
                limit: 10,
                pointList: [], // 积分记录
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                }
            }
        },
        onLoad() {
            this.userPointLog()
        },
        computed: {
            nowDate() {
                return this.$u.timeFormat(Math.round(new Date().getTime() / 1000))
            }
        },
        methods: {
            userPointLog() {
                let _this = this
                let data = {
                    page: _this.page,
                    limit: _this.limit
                }
                _this.status = 'loading'
                _this.$u.api.pointLog(data).then(res => {
                    if (res.status) {
                        _this.pointList = [..._this.pointList, ...res.data]
                        // 判断数据是否加载完毕
                        if (_this.page < res.otherData.totalPages) {
                            _this.page++
                            _this.status = 'loadmore'
                        } else {
                            _this.status = 'nomore'
                        }
                    } else {
                        // 接口請求出錯
                        _this.$u.toast(res.msg)
                        _this.status = 'loadmore'
                    }
                })
            }
        },
        // 页面滚动到底部触发事件
        onReachBottom() {
            let _this = this
            if (_this.status === 'loadmore') {
                _this.userPointLog()
            }
        }
    }
</script>
<style lang="scss">
    page { background-color: #fff; }
    .integral-top { width: 690rpx; height: 301rpx; background-image: url('/static/images/common/bg.png'); background-size: cover; background-position: center; text-align: center; width: 698rpx; margin: 10rpx auto 20rpx; border-radius: 12rpx; padding: 40rpx 0; box-shadow: 1rpx 5rpx 16rpx 0px rgba(229,77,66, 0.81); }

</style>
