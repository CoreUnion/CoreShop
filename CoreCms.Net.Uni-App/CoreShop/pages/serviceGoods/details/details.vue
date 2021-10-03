<template>
    <!-- 页面主体 -->
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <view class="coreshop-full-screen-nav-back">
            <view class="back-btn" @click="toBackBtn()">
                <u-icon name="arrow-left" size="40" top="12"></u-icon>
            </view>
        </view>

        <view class="coreshop-full-screen-banner-swiper-box">
            <u-image width="100%" height="750rpx" :src="info.thumbnail && info.thumbnail!='null' ?  info.thumbnail+'?x-oss-process=image/resize,m_lfit,h_320,w_240' : '/static/images/common/empty-banner.png'">
                <u-loading slot="loading"></u-loading>
            </u-image>
        </view>

        <!--限时秒杀-->
        <view class="coreshop-limited-seckill-box coreshop-bg-green">
            <text class="coreshop-text-price u-font-40">{{ info.money || '0.00' }}</text>
            <view class="u-font-xs coreshop-cost-price-num price-4">
                <view>单包量：{{ info.ticketNumber }}</view>
                <view>剩余：{{ info.amount || '0' }} </view>
            </view>
            <view class="u-text-right coreshop-time-right">
                <view>限时秒杀</view>
                <view class="u-font-xs">
                    距结束剩余 <u-count-down :timestamp="timestamp" separator="zh" separator-size="22" separator-color="#ffffff" font-size="22" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true"></u-count-down>
                </view>
            </view>
        </view>

        <!--标题-->
        <view class="coreshop-bg-white coreshop-common-view-box coreshop-good-title-view-box">
            <view class="title-view">
                <text class="coreshop-text-black u-font-lg coreshop-text-bold">{{ info.title || '' }}</text>
            </view>
            <view class="light coreshop-bg-red radius u-margin-top-20 coreshop-title-tip-box">
                <text class="u-font-sm">{{ info.description || '' }}</text>
            </view>
        </view>

        <!--会员级别-->
        <view class="u-margin-top-20 coreshop-bg-white coreshop-common-view-box" v-if="info.allowedMemberships">
            <view class="coreshop-flex coreshop-flex-wrap u-font-sm">
                <view class="coreshop-basis-2">
                    <text class="coreshop-text-gray">会员级别</text>
                </view>
                <view class="coreshop-basis-8">
                    <u-tag :text="item" mode="light" size="mini" class="u-margin-right-5" v-for="(item, indexChild) in info.allowedMemberships" :key="indexChild" />
                </view>
            </view>
        </view>

        <!--服务网点-->
        <view class="coreshop-bg-white coreshop-common-view-box ">
            <view class="coreshop-flex coreshop-flex-wrap u-font-sm">
                <view class="coreshop-basis-2">
                    <text class="coreshop-text-gray">服务网点</text>
                </view>
                <view class="coreshop-basis-8">
                    <u-icon name="checkmark-circle" size="20" label-size="24" color="#e54d42" :label="item" v-for="(item, index) in info.consumableStores" :key="index" class="u-margin-right-10"></u-icon>
                </view>
            </view>
            <view class="coreshop-solid-bottom u-margin-top-20 u-margin-bottom-20 u-padding-top-20" />
            <view class="coreshop-flex coreshop-flex-wrap u-font-sm">
                <view class="coreshop-basis-2">
                    <text class="coreshop-text-gray">有效期</text>
                </view>
                <view class="coreshop-basis-8">
                    <text class="u-font-24">
                        {{info.validityStartTime}} 至 {{info.validityEndTime}}
                    </text>
                </view>
            </view>
        </view>

        <!--内容区-->
        <view class="u-margin-top-20 u-padding-top-10 coreshop-bg-white">
            <view class="u-font-md u-padding-top-10 u-padding-bottom-10 u-padding-left-20 u-padding-right-10">
                <u-icon name="more-circle" color="#e54d42"></u-icon>
                <text class="u-margin-left-10 coreshop-text-black">服务详情</text>
            </view>
            <view class="u-padding-10 coreshop-good-rich-text-view">
                <u-parse :html="info.contentBody" :selectable="true" v-if="info.contentBody"></u-parse>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="详情为空" mode="list"></u-empty>
                </view>
            </view>
        </view>

        <!--常见问题-->
        <view class="u-margin-top-20 coreshop-bg-white u-margin-bottom-30 coreshop-common-view-box">
            <view class="u-font-md u-padding-bottom-20">
                <u-icon name="question-circle" color="#e54d42"></u-icon>
                <text class="u-margin-left-10 coreshop-text-black">常见说明</text>
            </view>
            <view class="coreshop-flex coreshop-flex-wrap u-margin-bottom-20">
                <view class="coreshop-basis-2">
                    <text class="cu-tag bg-grey radius sm">服务说明</text>
                </view>
                <view class="coreshop-basis-8">
                    <view class="u-font-sm">服务是对商品概念的扩展，商品订单为单次交易，但很多是否需要购买的商品或产品能够进行多次次数消费，特推出此服务包。</view>
                </view>
            </view>
            <view class="coreshop-flex coreshop-flex-wrap u-margin-bottom-20">
                <view class="coreshop-basis-2">
                    <text class="cu-tag bg-grey radius sm">权利声明</text>
                </view>
                <view class="coreshop-basis-8">
                    <view class="u-font-sm">本站商品信息均来自于合作方，其真实性、准确性和合法性由信息拥有者（合作方）负责。本站不提供任何保证，并不承担任何法律责任。</view>
                </view>
            </view>
            <view class="coreshop-flex coreshop-flex-wrap u-margin-bottom-20">
                <view class="coreshop-basis-2">
                    <text class="cu-tag bg-grey radius sm">销售价</text>
                </view>
                <view class="coreshop-basis-8">
                    <view class="u-font-sm">销售价为商品的销售价格，是您最终决定是否购买商品的依据。</view>
                </view>
            </view>
            <view class="coreshop-flex coreshop-flex-wrap u-margin-bottom-20">
                <view class="coreshop-basis-2">
                    <text class="cu-tag bg-grey radius sm">划线价</text>
                </view>
                <view class="coreshop-basis-8">
                    <view class="u-font-sm">商品展示的划横线价格为参考价，并非原价，该价格可能是品牌专柜标价、商品吊牌价或由品牌供应商提供的正品零售价（如厂商指导价、建议零售价等）或该商品在平台上曾经展示过的销售价；由于地区、时间的差异性和市场行情波动，品牌专柜标价、商品吊牌价等可能会与您购物时展示的不一致，该价格仅供您参考。</view>
                </view>
            </view>
            <view class="coreshop-flex coreshop-flex-wrap u-margin-bottom-20">
                <view class="coreshop-basis-2">
                    <text class="cu-tag bg-grey radius sm">折扣</text>
                </view>
                <view class="coreshop-basis-8">
                    <view class="u-font-sm">如无特殊说明，折扣指销售商在原价、或划线价（如品牌专柜标价、商品吊牌价、厂商指导价、厂商建议零售价）等某一价格基础上计算出的优惠比例或优惠金额；如有疑问，您可在购买前联系销售商进行咨询。</view>
                </view>
            </view>
            <view class="coreshop-flex coreshop-flex-wrap u-margin-bottom-20">
                <view class="coreshop-basis-2">
                    <text class="cu-tag bg-grey radius sm">异常问题</text>
                </view>
                <view class="coreshop-basis-8">
                    <view class="u-font-sm">商品促销信息以商品详情页“促销”栏中的信息为准；商品的具体售价以订单结算页价格为准；如您发现活动商品售价或促销信息有异常，建议购买前先联系销售商咨询。</view>
                </view>
            </view>
            <view class="coreshop-solid-bottom u-margin-top-20 u-margin-bottom-20" />
            <view class="u-text-center coreshop-text-blue  u-padding-top-20" @tap="goArticleList()">查看更多问题</view>
        </view>

        <!--为您推荐-->
        <view class="coreshop-recommended-title-view">
            <view class="u-flex u-flex-wrap">
                <view class="u-flex-4 u-text-right">
                    <image class="img-anc" src="/static/images/common/anc.png" mode="widthFix" />
                </view>
                <view class="u-flex-4 u-text-center">
                    <text class="coreshop-text-black u-font-lg">为您推荐</text>
                </view>
                <view class="u-flex-4 u-text-left">
                    <image class="img-anc" src="/static/images/common/anc.png" mode="widthFix" />
                </view>
            </view>
        </view>

        <!--推荐列表-->
        <view class="coreshop-goods-group" v-if="otherRecommendData.length>0">
            <u-grid col="2" :border="false" :align="center">
                <u-grid-item bg-color="transparent" :custom-style="{padding: '0rpx'}" v-for="(item, index) in otherRecommendData" :key="index" @click="goGoodsDetail(item.id)">
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

        <!--占位底部距离-->
        <view class="coreshop-tabbar-height" />
        <!--底部操作-->
        <!--按钮-->
        <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
            <view class="flex u-padding-20 flex-direction">
                <u-button :custom-style="customStyle" type="success" size="medium" @click="toAddOrder">立刻购买</u-button>
            </view>
        </view>

        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>

<script>
    import { commonUse, articles } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [commonUse, articles],
        data() {
            return {
                customStyle: {
                    width: '100%',
                },
                id: 0,
                info: {},
                timestamp: 86400,
                orderType: 5,	// 订单类型
                otherRecommendData: [], // 其他数据

            }
        },
        onLoad(e) {
            this.id = e.id;
            //获取服务商品数据
            this.getDetail();
            //获取推荐商品数据
            this.getGoodsRecommendList();
        },
        methods: {
            getDetail() {
                let data = {
                    id: this.id
                };
                this.$u.api.getServiceDetail(data).then(res => {
                    if (res.status) {
                        this.info = res.data;
                        this.timestamp = res.data.timestamp;
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            },
            // 获取推荐商品信息
            getGoodsRecommendList() {
                let _this = this;
                let data = {
                    id: 10
                }
                _this.$u.api.getGoodsRecommendList(data).then(res => {
                    if (res.status) {
                        _this.otherRecommendData = _this.$u.randomArray(res.data);
                    } else {
                        _this.$u.toast(res.msg)
                    }
                });
            },
            toAddOrder() {
                //创建一个服务订单并跳转到支付
                let data = {
                    id: this.id
                };
                this.$u.api.addServiceOrder(data).then(res => {
                    if (res.status) {
                        this.$u.route('/pages/payment/pay/pay?orderId=' + res.data + '&type=' + this.orderType + '&serviceId=' + this.id)
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            }
        }
    }
</script>

<style lang="scss" scoped>
    @import "details.scss";
</style>