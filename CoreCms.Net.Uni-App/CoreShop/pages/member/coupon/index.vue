<template>
    <view class="content">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="我的优惠券"></u-navbar>
        <view>
            <view class="u-padding-15">
                <u-subsection :list="items" :current="current" :animation="true" @change="onClickItem" active-color="#ff9900"></u-subsection>
            </view>
            <view class="" v-if="listData.length > 0">
                <view v-for="(item, key) in listData" :key="key">
                    <view class="coreshop-sponsored-card-view" :class="item.isExpire || item.isUsed ?' grayscale':''">
                        <view class="card-price-view">
                            <view class="text-red price-left-view">
                                <text class="text-xs">{{item.couponCode}}</text>
                            </view>
                            <view class="name-content-view">
                                <view class="text-cut text-red">{{item.couponName}}</view>
                                <view class="text-xs">
                                    <text v-for="(itemResult, index) in item.results" :key="index">{{itemResult}}</text>
                                </view>
                                <view class="text-xs">有效期：{{item.stime}} - {{item.etime}}</view>
                            </view>
                            <view class="btn-right-view">
                                <button class="cu-btn bg-red round sm" @click="goIndex" v-if="current == 0">去使用</button>
                                <button class="cu-btn bg-grey round sm" v-if="item.isUsed==true">已使用</button>
                                <button class="cu-btn bg-grey round sm" v-if="item.isExpire==true && item.isUsed==fasle">已失效</button>
                            </view>
                        </view>
                        <view class="card-num-view">
                            <view class="text-xs">
                                <text v-for="(itemCondition, index) in item.conditions" :key="index">【{{itemCondition}}】</text>
                            </view>
                            <text class="cuIcon-unfold btnUnfold" />
                        </view>
                    </view>
                </view>
                <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$apiFilesUrl+'/static/images/empty/coupon.png'" icon-size="300" text="暂无此类优惠券" mode="list"></u-empty>
            </view>
        </view>
    </view>

</template>

<script>
    export default {
        data() {
            return {
                items: ['未使用', '已使用', '已失效'],
                current: 0,
                page: 1,
                limit: 10,
                listData: [],
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
            this.getData();
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getData();
            }
        },
        methods: {
            // tab点击切换
            onClickItem(index) {
                if (this.current !== index) {
                    this.current = index;
                    this.page = 1;
                    this.listData = [];
                    this.getData();
                }
            },
            //获取优惠券列表
            getData() {
                this.status = 'loading'
                let data = {
                    page: this.page,
                    limit: this.limit
                }
                if (this.current == 0) {
                    data['display'] = 'noUsed';
                }
                if (this.current == 1) {
                    data['display'] = 'yesUsed';
                }
                if (this.current == 2) {
                    data['display'] = 'invalid';
                }
                this.$u.api.userCoupon(data).then(res => {
                    if (res.status) {
                        let nowType = 'noUsed';
                        if (this.current == 1) {
                            nowType = 'yesUsed';
                        }
                        if (this.current == 2) {
                            nowType = 'invalid';
                        }
                        if (nowType == res.data.display) {
                            if (res.data.page >= this.page) {
                                let newList = this.listData.concat(res.data.list);
                                this.listData = newList;
                                if (res.data.count > this.listData.length) {
                                    this.page++
                                    this.status = 'loadmore'
                                } else {
                                    this.status = 'nomore'
                                }
                            }
                        }
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            },
            //跳转首页
            goIndex() {
                this.$u.route({
                    type: 'switchTab',
                    url: '/pages/index/default/default'
                });
            }
        }
    }
</script>
<style lang="scss">
    @import '../../../static/style/coupon.scss';
</style>
