<template>
    <view class="pageBox">
        <u-navbar title="限时秒杀"></u-navbar>
        <view class="tab-box x-f">
            <view class="tab-item" @tap="onTab(tab.id,tab.status)" :class="{ 'tab-active': tabCurrent === tab.id }" v-for="tab in tabList" :key="tab.id">
                <text class="tab-title">{{ tab.title }}</text>
                <text v-show="tabCurrent === tab.id" class="tab-triangle"></text>
            </view>
        </view>
        <view class="content-box">
            <scroll-view scroll-y="true" enable-back-to-top @scrolltolower="loadMore" class="scroll-box">
                <view class="goods-item" v-for="item in goodsList" :key="item.id" v-if="goodsList.length>0">

                    <view class="activity-goods-box x-bc">
                        <view class="img-box">
                            <slot name="tag"></slot>
                            <image class="img" :src="item.image" mode="aspectFill"></image>
                        </view>
                        <view class="goods-right y-bc">
                            <view class="title u-line-1">{{ item.name }}</view>
                            <view class="tip u-line-1">{{ item.brief }}</view>
                            <view class="slod-end">

                                <view class="x-f">
                                    <view class="cu-progress round sm">
                                        <view class="progress--color" :style="[{ width: loading ? getProgress(item.buyPromotionCount, item.stock) : '' }]"></view>
                                    </view>
                                    <view class="progress-text">已抢{{ getProgress(item.buyPromotionCount, item.stock) }}</view>
                                </view>

                            </view>
                            <view class=" price-box">
                                <view class="x-f">
                                    <view class="current">￥{{ item.price }}</view>
                                    <view class="original">￥{{ item.mktprice }}</view>
                                </view>
                            </view>
                            <button class="cu-btn buy-btn" :class="btnType[tabCurrent].color" v-if="tabCurrent=='ing'" @click="goSeckillDetail(item.id, item.groupId)">{{ btnType[tabCurrent].name }}</button>
                            <button class="cu-btn buy-btn" :class="btnType[tabCurrent].color" v-else>{{ btnType[tabCurrent].name }}</button>
                        </view>
                    </view>

                </view>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="暂无秒杀信息" mode="list"></u-empty>
                </view>
                <!-- 加载更多 -->
                <u-loadmore :status="loadStatus" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
            </scroll-view>
        </view>
        <!-- 登录提示 -->
        <corecms-login-modal></corecms-login-modal>
    </view>
</template>

<script>
    import { commonUse, goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [commonUse, goods],
        data() {
            return {
                loadStatus: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                },
                loading: false,
                page: 1,
                limit: 10,
                lastPage: 1,
                status: 1,
                tabCurrent: 'ing',
                goodsList: [],
                btnType: {
                    ing: {
                        name: '立即抢购',
                        color: 'btn-ing'
                    },
                    nostart: {
                        name: '尚未开始',
                        color: 'btn-nostart'
                    },
                    ended: {
                        name: '已结束',
                        color: 'btn-end',
                    },
                },
                tabList: [
                    {
                        id: 'ing',
                        title: '进行中',
                        status: 1

                    },
                    {
                        id: 'nostart',
                        title: '即将开始',
                        status: 0
                    },
                    {
                        id: 'ended',
                        title: '已结束',
                        status: 2
                    },
                ]
            };
        },
        onLoad() {
            setTimeout(() => {
                this.loading = true;
            }, 500);
            this.getGoodsList();
        },
        //onReachBottom() {
        //    if (this.loadStatus === 'loadmore') {
        //        this.getGoodsList()
        //    }
        //},
        methods: {
            onTab(id, status) {
                this.tabCurrent = id;
                this.status = status;
                this.goodsList = [];
                this.page = 1;
                this.loadStatus = 'loading';
                this.$u.debounce(this.getGoodsList, 500);
            },
            // 百分比
            getProgress(sales, stock) {
                let unit = '';
                if (stock + sales > 0) {
                    let num = (sales / (sales + stock)) * 100;
                    unit = num.toFixed(2) + '%';
                } else {
                    unit = '0%';
                }
                return unit;
            },
            // 加载更多
            loadMore() {
                if (this.page < this.lastPage) {
                    this.page += 1;
                    this.getGoodsList();
                }
            },
            // 秒杀列表
            getGoodsList() {
                let _this = this;
                let data = {
                    page: this.page,
                    limit: this.limit,
                    type: 4, //秒杀
                    status: this.status
                }
                this.loadStatus = 'loading';
                this.$u.api.getGroup(data).then(res => {
                    if (res.status) {
                        if (res.data) {
                            let _goodsList = res.data.goods;
                            _this.goodsList = [..._this.goodsList, ..._goodsList]
                        }
                        _this.lastPage = res.data.totalPages;
                        if (_this.page < res.data.totalPages) {
                            _this.page++
                            _this.loadStatus = 'loadmore'
                        } else {
                            _this.loadStatus = 'nomore'
                        }
                    } else {
                        _this.$u.toast(res.msg)
                    }
                });

            }
        }
    }
</script>

<style lang="scss" scoped>
    @import '../../../../static/style/pinTuan.scss';
    .tab-box .tab-item { flex: 1; line-height: 84rpx; text-align: center; background: #636363; color: #fff; font-size: 28rpx; font-family: PingFang SC; font-weight: 500; color: #ffffff; position: relative; border-right: 1rpx solid #fff; }
        .tab-box .tab-item .tab-triangle { position: absolute; z-index: 2; bottom: -14rpx; left: 50%; transform: translateX(-50%); width: 28rpx; height: 28rpx; background: #e54d42; transform: rotate(45deg); transform-origin: center; }
    .tab-box .tab-active { background: #e54d42; }
    .goods-item { margin-bottom: 2rpx; }
        .goods-item .cu-progress { width: 225rpx; height: 16rpx; }
            .goods-item .cu-progress .progress--color { background: #e54d42; }
        .goods-item .progress-text { color: #999999; font-size: 20rpx; margin-left: 25rpx; }
        .goods-item .buy-btn { position: absolute; right: 0; bottom: -20rpx; width: 140rpx; height: 60rpx; border-radius: 30rpx; font-size: 26rpx; font-family: PingFang SC; font-weight: 400; padding: 0; }
        .goods-item .btn-end,
        .goods-item .btn-nostart { background: #eeeeee; color: #999999; }
        .goods-item .btn-ing { background: linear-gradient(45deg,#f43f3b,#ed1c24); box-shadow: 0px 7rpx 6rpx 0px rgba(229, 138, 0, 0.22); color: #ffffff; }



    .activity-goods-box .goods-right { width: 480rpx; }
</style>
