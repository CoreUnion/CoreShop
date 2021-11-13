<template>
    <view>
        <u-navbar title="拼团"></u-navbar>
        <view class="content">
            <!-- 列表图片 -->
            <scroll-view class="scroll-box" scroll-y>
                <view class="group-wrap coreshop-bg-red">
                    <view class="group-head u-flex u-row-between">
                        <text class="group-head__title">爆款推荐</text>
                        <text class="group-head__notice">省钱省心限时拼</text>
                    </view>
                    <view class="group-box">
                        <view class="goods-item" v-for="(item, index) in goodsList" :key="item.id" v-if="goodsList.length>0">
                            <view class="activity-goods-box u-flex u-row-between">
                                <view class="img-box">
                                    <view class="tag" v-if="index < 3">TOP{{ index + 1 }}</view>
                                    <image class="img" :src="item.image" mode="aspectFill"></image>
                                </view>
                                <view class="goods-right u-flex u-row-between coreshop-flex-direction">
                                    <view class="title u-line-1">{{ item.name }}</view>
                                    <view class="tip u-line-1">{{ item.brief }}</view>
                                    <view class="slod-end">
                                        <view class="coreshop-flex coreshop-align-center">
                                            <view class="sell-box">
                                                <text class="cuIcon-hotfill"></text>
                                                <text class="sell-num">已拼{{ item.buyPinTuanCount }}件</text>
                                            </view>
                                            <text class="group-num">{{ item.pinTuanRule.peopleNumber || 0 }}人团</text>
                                        </view>
                                    </view>
                                    <view class="price-box">
                                        <view class="coreshop-flex coreshop-align-center">
                                            <view class="current">￥{{ item.pinTuanPrice  }}</view>
                                            <view class="original">￥{{ item.price }}</view>
                                        </view>
                                    </view>
                                    <button class="cu-btn buy-btn" @tap="goPinTuanDetail(item.id,item.pinTuanRule.id)">马上拼</button>
                                </view>
                            </view>
                        </view>
                        <!-- 无数据时默认显示 -->
                        <view class="coreshop-emptybox" v-else>
                            <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无提现明细" mode="list"></u-empty>
                        </view>

                    </view>
                </view>
                <!-- 空白 -->
            </scroll-view>
        </view>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [goods],
        data() {
            return {
                goodsList: [],
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                }
            };
        },
        //加载执行
        onShow: function () {
            this.getGoods();
        },
        methods: {
            //取得列表数据
            getGoods: function () {
                var _this = this;
                let data = {};
                _this.$u.api.pinTuanList(data).then(res => {
                    if (res.status) {
                        _this.goodsList = res.data;
                        if (_this.goodsList) {
                            _this.goodsList.forEach(item => {
                                if (item.pinTuanPrice <= 0) {
                                    item.pinTuanPrice = '0.00';
                                } else {
                                    item.pinTuanPrice = this.$common.moneySub(item.price, item.pinTuanRule.discountAmount);
                                }
                            });
                        }
                    }
                });
            }
        }
    };
</script>

<style lang="scss">
    @import "list.scss";
</style>
