<template>
    <!-- 页面主体 -->
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <view class="nav-back">
            <view class="back-btn" @click="toBackBtn()">
                <image class="icon" src="/static/images/common/back-black.png" mode=""></image>
            </view>
        </view>

        <view class="coreshop-banner-swiper-box">
            <u-image width="100%" height="750rpx" :src="info.thumbnail && info.thumbnail!='null' ?  info.thumbnail+'?x-oss-process=image/resize,m_lfit,h_320,w_240' : '/static/images/common/empty-banner.png'">
                <u-loading slot="loading"></u-loading>
            </u-image>
        </view>

        <!--限时秒杀-->
        <view class="coreshop-limited-seckill-box bg-green">
            <text class="text-price text-xxl">{{ info.money || '0.00' }}</text>
            <view class="text-xs coreshop-cost-price-num price-4">
                <view>单包量：{{ info.ticketNumber }}</view>
                <view>剩余：{{ info.amount || '0' }} </view>
            </view>
            <view class="text-right coreshop-time-right">
                <view>限时秒杀</view>
                <view class="text-xs">
                    距结束剩余 <u-count-down :timestamp="timestamp" separator="zh" separator-size="22" separator-color="#ffffff" font-size="22" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true"></u-count-down>
                </view>
            </view>
            <view class="text-right coreshop-share-right">
                <u-icon name="share" @click="goShare()" label="分享" size="40" label-size="22" label-color="#fff" label-pos="bottom"></u-icon>
            </view>
        </view>

        <!--标题-->
        <view class="bg-white coreshop-view-box coreshop-title-view-box">
            <view class="title-view">
                <text class="text-black text-lg text-bold">{{ info.title || '' }}</text>
            </view>
            <view class="light bg-red radius margin-top-sm coreshop-title-tip-box">
                <view>
                    <text class="text-sm">{{ info.description || '' }}</text>
                </view>
            </view>
        </view>

        <!--会员级别-->
        <view class="margin-top bg-white coreshop-view-box coreshop-promotion-view-box" v-if="info.allowedMemberships">
            <view class="flex flex-wrap text-sm">
                <view class="basis-2">
                    <text class="text-gray">会员级别</text>
                </view>
                <view class="basis-8">
                    <text class="cu-tag bg-olive sm radius" v-for="(item, index) in info.allowedMemberships" :key="index">{{item}}</text>
                </view>
            </view>
        </view>

        <!--服务网点-->
        <view class="bg-white coreshop-view-box coreshop-select-view-box">
            <view class="flex flex-wrap text-sm">
                <view class="basis-2">
                    <text class="text-gray">服务网点</text>
                </view>
                <view class="basis-8">
                    <text class="cu-tag bg-orange sm radius" v-for="(item, index) in info.consumableStores" :key="index">{{item}}</text>
                </view>
            </view>
            <view class="coreshop-border-view" />
            <view class="flex flex-wrap text-sm">
                <view class="basis-2">
                    <text class="text-gray">有效期</text>
                </view>
                <view class="basis-8">
                    <text class="text-sm">
                        {{info.validityStartTime}} 至 {{info.validityEndTime}}
                    </text>
                </view>
            </view>
        </view>

        <!--内容区-->
        <view class="margin-top bg-white coreshop-details-view-box">
            <view class="text-df title-view">
                <text class="cuIcon-titles text-red" />
                <text class="text-black">服务详情</text>
            </view>
            <view class="u-padding-10">
                <u-parse :html="info.contentBody" :selectable="true" v-if="info.contentBody"></u-parse>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="详情为空" mode="list"></u-empty>
                </view>
            </view>
        </view>

        <!--常见问题-->
        <view class="margin-top bg-white margin-bottom coreshop-view-box coreshop-goods-help-view-box">
            <view class="text-black text-lg margin-bottom-sm">常见说明</view>
            <view class="flex flex-wrap margin-bottom">
                <view class="basis-2">
                    <text class="cu-tag bg-grey radius sm">服务说明</text>
                </view>
                <view class="basis-8">
                    <view class="text-sm">服务是对商品概念的扩展，商品订单为单次交易，但很多是否需要购买的商品或产品能够进行多次次数消费，特推出此服务包。</view>
                </view>
            </view>
            <view class="flex flex-wrap margin-bottom">
                <view class="basis-2">
                    <text class="cu-tag bg-grey radius sm">权利声明</text>
                </view>
                <view class="basis-8">
                    <view class="text-sm">本站商品信息均来自于合作方，其真实性、准确性和合法性由信息拥有者（合作方）负责。本站不提供任何保证，并不承担任何法律责任。</view>
                </view>
            </view>
            <view class="flex flex-wrap margin-bottom">
                <view class="basis-2">
                    <text class="cu-tag bg-grey radius sm">销售价</text>
                </view>
                <view class="basis-8">
                    <view class="text-sm">销售价为商品的销售价格，是您最终决定是否购买商品的依据。</view>
                </view>
            </view>
            <view class="flex flex-wrap margin-bottom">
                <view class="basis-2">
                    <text class="cu-tag bg-grey radius sm">划线价</text>
                </view>
                <view class="basis-8">
                    <view class="text-sm">商品展示的划横线价格为参考价，并非原价，该价格可能是品牌专柜标价、商品吊牌价或由品牌供应商提供的正品零售价（如厂商指导价、建议零售价等）或该商品在平台上曾经展示过的销售价；由于地区、时间的差异性和市场行情波动，品牌专柜标价、商品吊牌价等可能会与您购物时展示的不一致，该价格仅供您参考。</view>
                </view>
            </view>
            <view class="flex flex-wrap margin-bottom">
                <view class="basis-2">
                    <text class="cu-tag bg-grey radius sm">折扣</text>
                </view>
                <view class="basis-8">
                    <view class="text-sm">如无特殊说明，折扣指销售商在原价、或划线价（如品牌专柜标价、商品吊牌价、厂商指导价、厂商建议零售价）等某一价格基础上计算出的优惠比例或优惠金额；如有疑问，您可在购买前联系销售商进行咨询。</view>
                </view>
            </view>
            <view class="flex flex-wrap margin-bottom">
                <view class="basis-2">
                    <text class="cu-tag bg-grey radius sm">异常问题</text>
                </view>
                <view class="basis-8">
                    <view class="text-sm">商品促销信息以商品详情页“促销”栏中的信息为准；商品的具体售价以订单结算页价格为准；如您发现活动商品售价或促销信息有异常，建议购买前先联系销售商咨询。</view>
                </view>
            </view>
            <view class="coreshop-border-view" />
            <view class="text-center text-blue" @tap="goArticleList()">查看更多问题</view>
        </view>

        <!--按钮-->
        <!--<view class="bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
            <view class="flex padding-sm flex-direction basis-10">
                <button class="cu-btn bg-red" @tap="$u.debounce(toAddOrder, 500)">立刻购买</button>
            </view>
        </view>-->

         <!--占位底部距离-->
        <view class="cu-tabbar-height" />
        <!--底部操作-->
        <view class="coreshop-footer-fixed">
            <view class="cu-bar bg-white tabbar border shop">
                <!-- 客服按钮 -->
                <!-- #ifdef H5 || APP-PLUS-NVUE || APP-PLUS -->
                <view class="action" @click="showChat()">
                    <view class="cuIcon-service" />
                    <view>客服</view>
                </view>
                <!-- #endif -->
                <!-- #ifdef MP-WEIXIN -->
                <button class="action" hover-class="none" open-type="contact" bindcontact="showChat" :session-from="kefupara">
                    <view class="cuIcon-service" />
                    <view>客服</view>
                </button>
                <!-- #endif -->
                <!-- #ifdef MP-ALIPAY -->
                <view class="action">
                    <contact-button class="goods-bottom-ic icon" icon="/static/images/customerservice.png" size="80rpx*80rpx" tnt-inst-id="WKPKUZXG" scene="SCE00040186" hover-class="none" />
                </view>
                <!-- #endif -->
                <!-- #ifdef MP-TOUTIAO -->
                <view class="action" @click="showChat()">
                    <view class="cuIcon-service" />
                    <view>客服</view>
                </view>
                <!-- #endif -->
                <button class="action" @click="collection">
                    <view :class="isfav ? 'cuIcon-favorfill':'cuIcon-favor'" />
                    <view v-if="!isfav">收藏</view>
                    <view v-if="isfav">已收藏</view>
                </button>
                
                <view class="btn-group">
                    <button class="cu-btn bg-green radius shadow-blur  y-f buyBtn" style="width: 90%;" @tap="$u.debounce(toAddOrder, 500)">立刻购买</button>
                </view>
            </view>
        </view>



        <!-- 登录提示 -->
        <corecms-login-modal></corecms-login-modal>
    </view>
</template>

<script>
    import { commonUse, articles } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [commonUse, articles],
        data() {
            return {
                id: 0,
                info: {},
                timestamp: 86400,
                orderType: 5	// 订单类型
            }
        },
        onLoad(e) {
            this.id = e.id;
            console.log(this.id);
            this.getDetail();
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
    @import '../../../static/style/goodDetails.scss';
</style>