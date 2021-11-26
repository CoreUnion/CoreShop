<template>
    <view>

        <u-tabs :list="nameList" :current="current" @change="change"></u-tabs>

        <view v-for="(child,indexP) in newData.parameters.list" :key="indexP">
            <view class="goodsBox" v-show="child.isShow==true">
                <!-- 列表平铺两列三列 -->
                <view v-if="child.column == '2' || child.column == '3' " v-bind:class="'column'+child.column">
                    <view class="" v-if="child.list">
                        <u-grid :col="child.column" :border="false" :align="center">
                            <u-grid-item bg-color="transparent" :custom-style="{padding: '0rpx'}" v-for="(item, index) in child.list" :key="index" @click="goGoodsDetail(item.id)">
                                <view class="good_box">
                                    <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                                    <u-lazy-load threshold="-150" border-radius="10" :image="item.image" :index="index"></u-lazy-load>
                                    <view class="good_title u-line-2">
                                        {{item.name}}
                                    </view>
                                    <view class="good-price">
                                        {{item.price}}元 <span class="u-font-xs  coreshop-text-through u-margin-left-15 coreshop-text-gray">{{item.mktprice}}元</span>
                                    </view>
                                    <view class="good-tag-recommend" v-if="item.isRecommend">
                                        推荐
                                    </view>
                                    <view class="good-tag-hot" v-if="item.isHot">
                                        热门
                                    </view>
                                </view>
                            </u-grid-item>
                        </u-grid>
                    </view>
                    <view v-else-if="!count && !child.listAjax">
                        <u-grid col="3" border="false" align="center">
                            <u-grid-item bg-color="transparent" :custom-style="{padding: '0rpx'}" v-for="item in 3" :key="item">
                                <view class="good_box">
                                    <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                                    <!--<u-lazy-load threshold="-450" border-radius="10" image="/static/images/common/empty.png"></u-lazy-load>-->
                                    <view class="good_title u-line-2">
                                        无
                                    </view>
                                    <view class="good-price">
                                        0元
                                    </view>
                                    <view class="good-tag-recommend">
                                        推荐
                                    </view>
                                    <view class="good-tag-hot">
                                        热门
                                    </view>
                                </view>
                            </u-grid-item>
                        </u-grid>
                    </view>
                </view>

                <!-- 列表平铺单列 -->
                <view v-if="child.column == '1'">
                    <view v-if="child.list">
                        <u-grid :col="1" :border="false" :align="center">
                            <u-grid-item bg-color="transparent" :custom-style="{padding: '0rpx'}" v-for="item in child.list" :key="item.id" @click="goGoodsDetail(item.id)">
                                <view class="good_box">
                                    <u-row gutter="5" justify="space-between">
                                        <u-col span="4">
                                            <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                                            <u-lazy-load threshold="-150" border-radius="10" :image="item.image" :index="item.id"></u-lazy-load>
                                            <view class="good-tag-recommend2" v-if="item.isRecommend">
                                                推荐
                                            </view>
                                            <view class="good-tag-hot" v-if="item.isHot">
                                                热门
                                            </view>
                                        </u-col>
                                        <u-col span="8">
                                            <view class="good_title-xl u-line-3 u-padding-10">
                                                {{item.name}}
                                            </view>
                                            <view class="good-price u-padding-10">
                                                {{item.price}}元 <span class="u-font-xs  coreshop-text-through u-margin-left-15 coreshop-text-gray">{{item.mktprice}}元</span>
                                            </view>
                                        </u-col>
                                    </u-row>
                                </view>
                            </u-grid-item>
                        </u-grid>
                    </view>
                    <view class="order-none" v-else>
                        <image class="order-none-img" src="/static/images/order.png" mode=""></image>
                    </view>
                </view>
            </view>
        </view>
    </view>
</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [goods],
        data() {
            return {
                current: 0,
                nameList: [],
                newData: {}
            };
        },
        name: "coreshopgoodTabBar",
        props: {
            coreshopdata: {
                // type: Array,
                required: true,
            }
        },
        mounted() {
            this.newData = this.coreshopdata;
            for (var i = 0; i < this.newData.parameters.list.length; i++) {
                this.newData.parameters.list[i].isShow = i == 0;
                let item = {
                    name: this.newData.parameters.list[i].title
                }
                this.nameList.push(item);
            }
        },
        computed: {
        },
        methods: {
            change(index) {
                var _this = this;
                this.current = index;
                for (var i = 0; i < this.newData.parameters.list.length; i++) {
                    if (this.current == i) {
                        this.newData.parameters.list[i].isShow = true;
                    } else {
                        this.newData.parameters.list[i].isShow = false;
                    }
                }
            }
        },
    }
</script>

<style scoped lang="scss">
    .goodsBox { border-radius: 16rpx; color: #333333 !important; margin: 10rpx 10rpx;
        .good_box { border-radius: 8px; margin: 3px; background-color: #ffffff; padding: 5px; position: relative; width: calc(100% - 6px);
            .good_title { font-size: 26rpx; margin-top: 5px; color: $u-main-color; }
            .good_title-xl { font-size: 28rpx; margin-top: 5px; color: $u-main-color; }
            .good-tag-hot { display: flex; margin-top: 5px; position: absolute; top: 15rpx; left: 15rpx; background-color: $u-type-error; color: #FFFFFF; display: flex; align-items: center; padding: 4rpx 14rpx; border-radius: 50rpx; font-size: 20rpx; line-height: 1; }
            .good-tag-recommend { display: flex; margin-top: 5px; position: absolute; top: 15rpx; right: 15rpx; background-color: $u-type-primary; color: #FFFFFF; margin-left: 10px; border-radius: 50rpx; line-height: 1; padding: 4rpx 14rpx; display: flex; align-items: center; border-radius: 50rpx; font-size: 20rpx; }
            .good-tag-recommend2 { display: flex; margin-top: 5px; position: absolute; bottom: 15rpx; left: 15rpx; background-color: $u-type-primary; color: #FFFFFF; border-radius: 50rpx; line-height: 1; padding: 4rpx 14rpx; display: flex; align-items: center; border-radius: 50rpx; font-size: 20rpx; }
            .good-price { font-size: 30rpx; color: $u-type-error; margin-top: 5px; }
        }

        .indicator-dots { margin-top: 20rpx; margin-bottom: 20rpx; display: flex; justify-content: center; align-items: center;
            .indicator-dots-item { background-color: $u-tips-color; height: 6px; width: 6px; border-radius: 10px; margin: 0 3px; }
            .indicator-dots-active { background-color: $u-type-primary; }
        }
    }
</style>
