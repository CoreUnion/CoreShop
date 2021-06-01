<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <view class="nav-back">
            <view class="back-btn" @click="toBackBtn()">
                <image class="icon" src="/static/images/common/back-black.png" mode=""></image>
            </view>
        </view>
        <!--幻灯片-->
        <view class="coreshop-banner-swiper-box">
            <swiper class="screen-swiper" circular autoplay @change="bannerSwiper">
                <swiper-item v-for="(item,index) in goodsInfo.album" :key="index" @click="clickImg(item)">
                    <u-image width="100%" height="750rpx" :src="item">
                        <u-loading slot="loading"></u-loading>
                    </u-image>
                </swiper-item>
            </swiper>
            <!--页码-->
            <text class="tag bg-grey round sm coreshop-page">{{bannerCur + 1}} / {{goodsInfo.album.length}}</text>
        </view>


        <!--限时秒杀-->
        <view class="coreshop-limited-seckill-box  bg-orange">
            <text class="text-price text-xxl">{{ product.price || '0.00' }}</text>
            <view class="text-xs coreshop-cost-price-num price-4">
                <view>已售{{ goodsInfo.buyCount || '0' }}件/剩余{{ product.stock || '0' }}件</view>
                <view>累计销售{{ goodsInfo.buyCount || '0' }}件</view>
            </view>
            <view class="text-right coreshop-time-right">
                <view>距结束仅剩</view>
                <view class="text-xs u-margin-top-10">
                    <u-count-down :timestamp="goodsInfo.groupTimestamp" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="24" separator-size="24"></u-count-down>
                </view>
            </view>
            <view class="text-right coreshop-share-right">
                <u-icon name="share" @click="goShare()" label="分享" size="40" label-size="22" label-color="#fff" label-pos="bottom"></u-icon>
            </view>
        </view>


        <!--标题-->
        <view class="bg-white coreshop-view-box coreshop-title-view-box">
            <view class="title-view">
                <text class="cu-tag bg-red radius sm" v-if="goodsInfo.brand">{{goodsInfo.brand.name}}</text>
                <text class="text-black text-lg text-bold">{{ goodsInfo.name || '' }}</text>
            </view>
            <view class="light bg-orange radius margin-top-sm coreshop-title-tip-box">
                <view>
                    <text class="text-sm">{{ goodsInfo.brief || '' }}</text>
                </view>
            </view>
        </view>

        <!--促销（团购秒杀本就是促销的一种，所以没必要显示）-->
        <!--<view class="margin-top bg-white coreshop-view-box coreshop-promotion-view-box" v-if="promotion.length > 0" @tap="promotionTap">
            <view class="flex flex-wrap text-sm" @tap="promotionTap">
                <view class="basis-1">
                    <text class="text-gray">促销</text>
                </view>
                <view class="basis-7">
                    <view v-for="(item, index) in promotion" :key="index" :class="index> 1 ? 'u-margin-top-10':''">
                        <text class="cu-tag line-orange sm radius">{{item.name}}</text>
                    </view>
                </view>
                <view class="basis-2">
                    <view class="text-gray text-right icon-view">
                        <text class="cuIcon-right icon" />
                    </view>
                </view>
            </view>
        </view>-->
        <!--服务-->
        <!--服务-->
        <view class="margin-top bg-white coreshop-view-box coreshop-service-view-box" @tap="serviceTap" v-if="serviceDescription.service.length>0">
            <view class="flex flex-wrap text-sm">
                <view class="basis-1">
                    <text class="text-gray">服务</text>
                </view>
                <view class="basis-7">
                    <view class="tag-view-box">
                        <text class="cu-tag bg-white tag-view" v-for="(item, index) in serviceDescription.service" :key="index">
                            <text class="cuIcon-roundcheck text-red" />
                            <text class="margin-left-xs">{{item.title}}</text>
                        </text>
                    </view>
                </view>
                <view class="basis-2">
                    <view class="text-gray text-right icon-view">
                        <text class="cuIcon-right icon" />
                    </view>
                </view>
            </view>
        </view>


        <!--发货/规格-->
        <view class="margin-top bg-white coreshop-view-box coreshop-select-view-box">
            <view class="flex flex-wrap text-sm" v-if="serviceDescription.delivery.length>0">
                <view class="basis-1">
                    <text class="text-gray">发货</text>
                </view>
                <view class="basis-9" v-for="(item, index) in serviceDescription.delivery" :key="index">
                    <text class="text-sm">{{item.description}}</text>
                </view>
            </view>

            <view class="coreshop-border-view" v-if="serviceDescription.delivery.length>0" />

            <view class="flex flex-wrap text-sm" @tap="selectTap()" v-if="isSpes">
                <view class="basis-1">
                    <text class="text-gray">规格</text>
                </view>
                <view class="basis-8">
                    <text class="text-sm">{{ product.spesDesc || ''}}</text>
                </view>
                <view class="basis-1">
                    <view class="text-gray text-right">
                        <text class="cuIcon-right icon" />
                    </view>
                </view>
            </view>
        </view>

        <!--评论-->
        <view class="margin-top bg-white coreshop-comment-view-box" v-if="goodsComments.length">
            <view class="cu-bar bg-white">
                <view class="action">
                    <text class="text-black text-df">评价（{{goodsComments.length}}）</text>
                </view>
                <view class="action">
                    <view class="text-sm">
                        <text class="cuIcon-right icon margin-left-xs" @tap="goGoodComments(goodsInfo.id)" />
                    </view>
                </view>
            </view>
            <view v-for="(item, index) in goodsComments" :key="index">
                <view class="coreshop-border-view" />
                <view class="coreshop-view-box">
                    <view class="flex flex-wrap text-sm">
                        <view class="basis-1">
                            <view class="cu-avatar sm round" :style="[{backgroundImage:'url('+ item.avatarImage +')'}]" />
                        </view>
                        <view class="basis-9 text-sm">
                            <view>{{ (item.nickName && item.nickName != '')?item.nickName:item.mobile }}</view>
                            <view class="margin-top-xs">{{ item.contentBody || ''}}</view>
                            <view class="text-gray margin-top-sm">
                                <u-rate :current="item.score" :disabled="true" size="26"></u-rate>
                            </view>
                            <view class="text-gray margin-top-sm">{{ item.createTime || ''}} {{ item.addon || ''}}</view>
                        </view>
                    </view>
                </view>
            </view>
        </view>

        <!--内容区-->
        <view class="margin-top bg-white coreshop-details-view-box">
            <view class="text-df title-view">
                <text class="cuIcon-titles text-red" />
                <text class="text-black">商品详情</text>
            </view>
            <!--参数-->
            <view class="grid col-2">
                <view class="col-item" v-for="(item, index) in goodsParams" :key="index">
                    <text class="text-gray">{{ item.name || ''}}：</text>
                    <text class="text-black">{{ item.value || ''}}</text>
                </view>
            </view>
            <!--<view class="text-cut col-item">
                <text class="text-gray">详细信息：</text>
                <text class="text-black">有发票，有配件，有包装</text>
            </view>-->
            <view class="u-padding-top-10">
                <u-parse :html="goodsInfo.intro" :selectable="true" v-if="goodsInfo.intro"></u-parse>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="详情为空" mode="list"></u-empty>
                </view>
            </view>
        </view>

        <!-- 分享弹窗 -->
        <view class="u-padding-10">
            <lvv-popup position="bottom" ref="share">
                <!-- #ifdef H5 -->
                <shareByH5 :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByH5>
                <!-- #endif -->
                <!-- #ifdef MP-WEIXIN -->
                <shareByWx :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByWx>
                <!-- #endif -->
                <!-- #ifdef MP-ALIPAY -->
                <shareByAli :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByAli>
                <!-- #endif -->
                <!-- #ifdef MP-TOUTIAO -->
                <shareByTt :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByTt>
                <!-- #endif -->
                <!-- #ifdef APP-PLUS || APP-PLUS-NVUE -->
                <shareByApp :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByApp>
                <!-- #endif -->
            </lvv-popup>
        </view>


        <!--常见问题-->
        <view class="margin-top bg-white margin-bottom coreshop-view-box coreshop-goods-help-view-box">
            <view class="text-black text-lg margin-bottom-sm">常见说明</view>

            <view class="flex flex-wrap margin-bottom" v-for="(item, index) in serviceDescription.commonQuestion" :key="index">
                <view class="basis-2">
                    <text class="cu-tag bg-grey radius sm">{{item.title}}</text>
                </view>
                <view class="basis-8">
                    <view class="text-sm">{{item.description}}</view>
                </view>
            </view>

            <view class="coreshop-border-view" />
            <view class="text-center text-blue" @tap="goArticleList()">查看更多问题</view>
        </view>


        <!--商家及推荐-->
        <view class="margin-top bg-white coreshop-view-box coreshop-goods-info-view-box">
            <view class="coreshop-shop-view">
                <view class="cu-avatar lg round" :style="[{backgroundImage:'url('+ shopLogo +')'}]" />
                <view class="text-view">
                    <view class="margin-bottom-xs">{{shopName}}</view>
                    <view class="text-sm text-cut">{{shareTitle}}</view>
                </view>
                <button class="cu-btn radius sm line-red" @tap="doPhoneCall">联系商家</button>
            </view>
            <view class="coreshop-border-view" />
            <view class="live-tag-view">
                <view class="text-view">
                    <text class="cu-tag bg-orange radius sm">
                        <text class="cuIcon-title" />
                        <text>已定位</text>
                    </text>
                    <text class="margin-left-xs text-cut">可直接获取商家地理位置信息</text>
                </view>
                <view class="text-sm text-red text-right-view" @tap="goShopMap">
                    <text class="margin-right-xs">去地图</text>
                    <text class="cuIcon-right" />
                </view>
            </view>
            <view class="coreshop-border-view" />
            <view class="coreshop-recommend-list-box">
                <view class="text-sm">本店推荐</view>
                <!--滑动列表-->
                <view class="recommend-scroll-box">
                    <scroll-view class="recommend-scroll" scroll-x>
                        <block v-for="(items,indexs) in shopRecommendData" :key="indexs">
                            <view :id="['scroll' + (indexs + 1 )]" class="recommend-scroll-item" @tap="goGoodsDetail(items.id)">
                                <view class="cu-avatar xl radius" :style="[{backgroundImage:'url('+ items.image +')'}]" />
                                <view class="text-cut-2 text-sm text-black margin-tb-sm u-line-2" style="height:64rpx;">{{items.name}}</view>
                                <view class="text-red text-price margin-tb-sm text-df">
                                    {{items.price}}元
                                </view>
                            </view>
                        </block>
                    </scroll-view>
                </view>
            </view>
        </view>


        <!--其他推荐-->
        <view class="margin-top coreshop-view-box coreshop-recommend-list-view-box">
            <view class="flex flex-wrap">
                <view class="basis-sm text-right">
                    <image class="img-aau" src="/static/images/common/aau.png" lazy-load mode="widthFix" />
                </view>
                <view class="basis-xs text-center">
                    <text class="text-black text-lg">其他推荐</text>
                </view>
                <view class="basis-sm text-left">
                    <image class="img-aau" src="/static/images/common/aau.png" lazy-load mode="widthFix" />
                </view>
            </view>

            <view class="margin-bottom coreshop-goods-list-box">
                <view class="grid col-2">
                    <block v-for="(items,indexs) in otherRecommendData" :key="indexs">
                        <view class="list-itme" @click="goGoodsDetail(items.id)">
                            <view class="bg-white list-radius">
                                <view class="goods-img">
                                    <view class="cu-avatar" :style="[{backgroundImage:'url('+ items.image +')'}]" />
                                    <view class="mold-view" v-if="items.mold">
                                        <text class="cu-tag radius sm bg-orange">自营</text>
                                    </view>
                                </view>
                                <view class="view-goods-info">
                                    <view class="text-cut text-black text-sm margin-bottom-sm">{{items.name}}</view>
                                    <view class="text-price text-red text-lg">
                                        {{items.price}}元  <span class="u-font-xs color-9 linethrough u-margin-left-15 text-gray">{{items.mktprice}}元</span>
                                    </view>
                                </view>
                            </view>
                        </view>
                    </block>
                </view>
            </view>
        </view>




        <view class="cu-modal bottom-modal coreshop-bottom-modal-box" :class="bottomModal?'show':''">
            <view class="cu-dialog bg-white">
                <!--标题-->
                <view class="text-black text-center margin-tb text-lg coreshop-title-bar">
                    <text>{{modalTitle}}</text>
                    <text class="cuIcon-close close-icon" @tap="hideModal"></text>
                </view>

                <!--内容区域-->
                <view class="coreshop-modal-content">

                    <!--服务区域-->
                    <view class="coreshop-view-box service" v-if="modalType=='service'">
                        <view v-for="(item, index) in serviceDescription.service" :key="index">
                            <view class="text-view">
                                <text class="cuIcon-title text-red" />
                                <text class="text-cut text-black">{{item.title}}</text>
                            </view>
                            <view class="text-sm text-list-view">
                                <view class="margin-left text-cut text-gray">{{item.description}}</view>
                            </view>
                        </view>
                    </view>

                    <!--促销区域-->
                    <view class="coreshop-view-box promotion" v-if="modalType=='promotion'">
                        <view class="text-view" v-for="(item, index) in promotion" :key="index">
                            <text class="cu-tag line-orange radius sm">{{item.name}}</text>
                            <text class="margin-left-xs text-cut text-black">{{item.name}}</text>
                        </view>
                    </view>

                    <!--选择规格-->
                    <view class="coreshop-view-box select hide" :class="modalType=='select' ?'show':''">
                        <!--商品信息-->
                        <view class="cu-list menu-avatar">
                            <view class="cu-item">
                                <view class="cu-avatar radius lg" :style="[{backgroundImage:'url('+ product.images +')'}] " />
                                <view class="content">
                                    <view class="text-price-view">
                                        <text class="text-price text-red margin-right-xs">{{ product.price || ''}}</text>
                                        <text class="text-sm text-gray text-through">￥{{ product.mktprice || ''}}</text>
                                        <text class="cu-tag bg-gradual-red radius sm">
                                            <text class="cuIcon-hotfill" />
                                            <text>秒杀中</text>
                                        </text>
                                    </view>
                                    <view class="text-black text-sm flex">
                                        <view class="text-cut">已选: {{ product.spesDesc || '无'}}</view>
                                    </view>
                                </view>
                            </view>
                        </view>
                        <!--规格数据-->
                        <view class="coreshop-select-btn-list-box">
                            <spec :spesData="defaultSpesDesc" ref="spec" @changeSpes="changeSpes"></spec>
                            <view class="select-item">
                                <view class="text-black">数量</view>
                                <view class="select-btn">
                                    <u-number-box v-model="buyNum" :min="minNums" :max="product.stock"></u-number-box>
                                </view>
                            </view>
                        </view>
                    </view>

                    <!--公共按钮-->
                    <view class="coreshop-modal-footer-fixed" v-if="modalType=='select'">
                        <view class="flex flex-direction">
                            <u-button type="error" size="medium" hover-class="btn-hover2" @click="clickHandle()" :disabled='submitStatus' :loading='submitStatus' v-if="product.stock>0">确定</u-button>
                            <u-button type="default" size="medium" v-else>已售罄</u-button>
                        </view>
                    </view>
                </view>
            </view>
        </view>
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
                <view class="action" @click="redirectCart">
                    <view class="cuIcon-cart">
                        <view class="cu-tag badge" v-if="cartNums">{{cartNums}}</view>
                    </view>
                    <text>购物车</text>
                </view>
                <view class="btn-group">
                    <button class="cu-btn bg-orange radius shadow-blur  y-f buyBtn" @tap="selectTap()">立即{{ typeName || '' }}</button>
                </view>
            </view>
        </view>

        <!-- 右边浮动球 -->
        <corecms-fab horizontal="right" vertical="bottom" direction="vertical"></corecms-fab>
        <!-- 登录提示 -->
        <corecms-login-modal></corecms-login-modal>

    </view>
</template>
<script>
    import { mapMutations, mapActions, mapState } from 'vuex';
    import lvvPopup from '@/components/corecms-lvv-popup/corecms-lvv-popup.vue';
    import corecmsFab from '@/components/corecms-fab/corecms-fab.vue';
    import { get } from '@/common/utils/dbHelper.js';
    import { apiBaseUrl } from '@/common/setting/constVarsHelper.js';
    import { goods, articles, commonUse, tools } from '@/common/mixins/mixinsHelper.js'
    import spec from '@/components/corecms-spec/corecms-spec.vue';
    // #ifdef H5
    import shareByH5 from '@/components/corecms-share/shareByh5.vue';
    // #endif
    // #ifdef MP-WEIXIN
    import shareByWx from '@/components/corecms-share/shareByWx.vue';
    // #endif
    // #ifdef MP-TOUTIAO
    import shareByTt from '@/components/corecms-share/shareByTt.vue';
    // #endif
    // #ifdef MP-ALIPAY
    import shareByAli from '@/components/corecms-share/shareByAli.vue';
    // #endif
    // #ifdef APP-PLUS || APP-PLUS-NVUE
    import shareByApp from '@/components/corecms-share/shareByApp.vue';
    // #endif
    export default {
        mixins: [goods, articles, commonUse, tools],
        components: {
            lvvPopup,
            corecmsFab,
            spec,
            // #ifdef H5
            shareByH5,
            // #endif
            // #ifdef MP-WEIXIN
            shareByWx,
            // #endif
            // #ifdef MP-TOUTIAO
            shareByTt,
            // #endif
            // #ifdef MP-ALIPAY
            shareByAli,
            // #endif
            // #ifdef APP-PLUS || APP-PLUS-NVUE
            shareByApp
            // #endif
        },
        data() {
            return {
                bannerCur: 0,
                current: 0, // init tab位
                goodsId: 0, // 商品id
                groupId: 0, // 团购ID
                goodsInfo: {}, // 商品详情
                cartNums: 0, // 购物车数量
                product: {}, // 规格详情
                goodsParams: [], // 商品参数信息
                goodsComments: [], // 商品评论信息
                shopRecommendData: [], // 本店推荐数据
                otherRecommendData: [], // 其他数据
                buyNum: 1, // 选定的购买数量
                minBuyNum: 1, // 最小可购买数量
                type: 1,
                isfav: false, // 商品是否收藏
                //拼团列表滑动数据
                swiperSet: {
                    indicatorDots: false,
                    autoplay: false,
                    interval: 2000,
                    duration: 500,
                    groupHeight: '',
                },
                bottomModal: false,
                modalTitle: '',
                modalType: 'promotion',
                selectType: '',
                shareUrl: '/pages/share/jump/jump',
                serviceDescription: {
                    commonQuestion: [],
                    delivery: [],
                    service: [],
                }
            };
        },
        onLoad(e) {
            this.goodsId = e.id;
            this.groupId = e.groupId;
            if (this.goodsId && this.groupId) {
                this.getServiceDescription();
                this.getGoodsInfo();
                this.getGoodsParams();
                this.getGoodsComments();
            } else {
                this.$refs.uToast.show({ title: '获取失败', type: 'error', back: true });
            }
            // 获取购物车数量
            this.getCartNums();
            //获取推荐数据
            this.getGoodsRecommendList();
        },
        computed: {
            ...mapState({
                hasLogin: state => state.hasLogin,
                userInfo: state => state.userInfo,
            }),
            hasLogin: {
                get() {
                    return this.$store.state.hasLogin;
                },
                set(val) {
                    this.$store.commit('hasLogin', val);
                }
            },
            shopName() {
                return this.$store.state.config.shopName;
            },
            shareTitle() {
                return this.$store.state.config.shareTitle;
            },
            shopLogo() {
                return this.$store.state.config.shopLogo;
            },
            // 获取店铺联系人手机号
            shopMobile() {
                return this.$store.state.config.shopMobile || 0;
            },
            // 规格切换计算规格商品的 可购买数量
            minNums() {
                var num = this.product.stock > 0 ? this.product.stock : 0;
                return num > this.minBuyNum ? this.minBuyNum : num;
            },
            // 判断商品是否是多规格商品  (为了兼容小程序 只能写在计算属性里面了)
            isSpes() {
                if (this.product.hasOwnProperty('defaultSpecificationDescription') && this.product.defaultSpecificationDescription != null && Object.keys(this.product.defaultSpecificationDescription).length) {
                    if (this.product.defaultSpecificationDescription != null && this.product.defaultSpecificationDescription != "" && this.product.defaultSpecificationDescription != undefined) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            },
            // 优惠信息重新组装
            promotion() {
                let arr = [];
                if (this.product.promotionList) {
                    for (let k in this.product.promotionList) {
                        arr.push(this.product.promotionList[k]);
                    }
                }
                return arr;
            },
            typeName() {
                return this.goodsInfo.groupType == 3 ? '团购' : '秒杀';
            },
            shareHref() {
                let pages = getCurrentPages();
                let page = pages[pages.length - 1];
                // #ifdef H5 || MP-WEIXIN || APP-PLUS || APP-PLUS-NVUE
                return apiBaseUrl + 'wap/' + page.route + '?id=' + this.goodsId + '&groupId=' + this.groupId;
                // #endif
                // #ifdef MP-ALIPAY
                return apiBaseUrl + 'wap/' + page.__proto__.route + '?id=' + this.goodsId + '&groupId=' + this.groupId;
                // #endif
            },
            defaultSpesDesc() {
                return this.product.defaultSpecificationDescription;
            }
        },
        methods: {
            bannerSwiper(e) {
                this.bannerCur = e.detail.current;
            },
            getServiceDescription() {
                let _this = this;
                this.$u.api.getServiceDescription().then(res => {
                    console.log(res);
                    if (res.status == true) {
                        _this.serviceDescription.commonQuestion = res.data.commonQuestion;
                        _this.serviceDescription.delivery = res.data.delivery;
                        _this.serviceDescription.service = res.data.service;

                    } else {
                        _this.$refs.uToast.show({ title: res.msg, type: 'error', back: true })
                    }
                })
            },
            getGoodsInfo() {
                let data = {
                    id: this.goodsId,
                    groupId: this.groupId
                };
                // 如果用户已经登录 要传用户token
                let userToken = get('userToken');
                if (userToken) {
                    data['token'] = userToken;
                }
                let _this = this;
                this.$u.api.groupInfo(data).then(res => {
                    if (res.status) {
                        if (res.data.length < 1) {
                            _this.$refs.uToast.show({ title: '该商品不存在，请返回重新选择商品。', type: 'error', back: true });
                        } else if (res.data.isMarketable == false) {
                            _this.$refs.uToast.show({ title: '该商品已下架，请返回重新选择商品。', type: 'error', back: true });
                        } else {
                            let info = res.data;
                            let products = res.data.product;

                            _this.goodsInfo = info;
                            _this.isfav = _this.goodsInfo.isfav;
                            _this.type = _this.goodsInfo.groupType;
                            _this.product = _this.spesClassHandle(products);

                            _this.buyNum = _this.product.stock >= _this.minBuyNum ? _this.minBuyNum : 0;

                            // 判断如果登录用户添加商品浏览足迹
                            if (userToken) {
                                _this.goodsBrowsing();
                            }
                        }
                    }
                });
            },
            // 获取推荐商品信息
            getGoodsRecommendList() {
                let _this = this;
                let recommenddata = {
                    id: 10,
                    data: true
                }
                _this.$u.api.getGoodsRecommendList(recommenddata).then(res => {
                    if (res.status) {
                        _this.shopRecommendData = _this.$u.randomArray(res.data);
                    } else {
                        _this.$u.toast(res.msg)
                    }
                });

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


            // 获取购物车数量
            getCartNums() {
                let userToken = this.$db.get('userToken');
                if (userToken && userToken != '') {
                    // 获取购物车数量
                    this.$u.api.getCartNum().then(res => {
                        if (res.status) {
                            this.cartNums = res.data;
                        }
                    });
                }
            },
            // 切换商品规格
            changeSpes(obj) {
                let index = obj.v;
                let key = obj.k;
                let tmp_defaultSpecificationDescription = JSON.parse(this.product.defaultSpecificationDescription);
                if (tmp_defaultSpecificationDescription[index][key].hasOwnProperty('productId') && tmp_defaultSpecificationDescription[index][key].productId) {
                    let data = {
                        id: tmp_defaultSpecificationDescription[index][key].productId,
                        type: 'group', //商品类型
                        groupId: this.groupId
                    };
                    let userToken = this.$db.get('userToken');
                    if (userToken) {
                        data['token'] = userToken;
                    }
                    this.$u.api.getProductInfo(data).then(res => {
                        if (res.status == true) {
                            // 切换规格判断可购买数量
                            this.buyNum = res.data.stock > this.minBuyNum ? this.minBuyNum : res.data.stock;
                            this.product = this.spesClassHandle(res.data);
                        }
                    });
                    uni.showLoading({
                        title: '加载中'
                    });
                    setTimeout(function () {
                        uni.hideLoading();
                    }, 1000);
                }
            },
            // 多规格样式统一处理
            spesClassHandle(products) {
                // 判断是否是多规格 (是否有默认规格)
                if (products.hasOwnProperty('defaultSpecificationDescription')) {
                    let spes = products.defaultSpecificationDescription;
                    for (let key in spes) {
                        for (let i in spes[key]) {
                            if (spes[key][i].hasOwnProperty('isDefault') && spes[key][i].isDefault === true) {
                                this.$set(spes[key][i], 'cla', 'selected');
                            } else if (spes[key][i].hasOwnProperty('productId') && spes[key][i].productId) {
                                this.$set(spes[key][i], 'cla', 'not-selected');
                            } else {
                                this.$set(spes[key][i], 'cla', 'none');
                            }
                        }
                    }
                    spes = JSON.stringify(spes)
                    products.defaultSpecificationDescription = spes;
                }
                return products;
            },
            // 购买数量加减操作
            bindChange(e) {
                this.buyNum = e.val;
            },
            // 商品收藏/取消
            collection() {
                let data = {
                    id: this.goodsInfo.id
                };
                this.$u.api.goodsCollection(data).then(res => {
                    if (res.status) {
                        this.isfav = !this.isfav;
                        this.$refs.uToast.show({ title: res.msg, type: 'success', back: false });
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            },
            // 获取商品参数信息
            getGoodsParams() {
                this.$u.api.goodsParams(
                    {
                        id: this.goodsId
                    }).then(res => {
                        if (res.status == true) {
                            this.goodsParams = res.data;
                        }
                    }
                    );
            },
            // 获取商品评论信息
            getGoodsComments() {
                let data = {
                    page: 1,
                    limit: 5,
                    id: this.goodsId,
                }
                this.$u.api.goodsComment(data).then(res => {
                    if (res.status == true) {
                        let _list = res.data.list;
                        // 如果评论没有图片 在这块作处理否则控制台报错
                        _list.forEach(item => {
                            if (!item.hasOwnProperty('images')) {
                                this.$set(item, 'images', [])
                            }
                        });
                        this.goodsComments = [...this.goodsComments, ..._list];
                    } else {
                        console.log("错误2");
                        this.$u.toast(res.msg);
                    }
                })
            },
            // 添加商品浏览足迹
            goodsBrowsing() {
                let data = {
                    id: this.goodsInfo.id
                };
                this.$u.api.addGoodsBrowsing(data).then(res => { });
            },
            // 点击弹出框确定按钮事件处理
            clickHandle() {
                if (!this.hasLogin) {
                    this.$store.commit('showLoginTip', true);
                    return false;
                }
                this.submitStatus = true;
                this.buyNow();
            },
            // 立即购买
            buyNow() {
                if (this.buyNum > 0) {
                    let data = {
                        ProductId: this.product.id,
                        Nums: this.buyNum,
                        cartType: this.type,
                        groupId: this.groupId
                    };
                    this.$u.api.addCart(data).then(res => {
                        if (res.status) {
                            this.hideModal(); // 关闭弹出层
                            let cartIds = res.data;
                            this.$u.route('/pages/placeOrder/index/index?cartIds=' + JSON.stringify(cartIds) + '&orderType=' + this.type + '&groupId=' + this.groupId);

                        } else {
                            this.$u.toast(res.msg);
                        }
                    });
                }
            },
            // 购物车页面跳转
            redirectCart() {
                this.$u.route({
                    type: 'switchTab',
                    url: '/pages/index/cart/cart'
                });
            },
            trigger(e) {
                this.content[e.index].active = !e.item.active;
                this.$u.route({
                    type: 'switchTab',
                    url: e.item.url
                });
            },
            // 跳转到h5分享页面
            goShare() {
                this.$refs.share.show();
            },
            closeShare() {
                this.$refs.share.close();
            },
            // 图片点击放大
            clickImg(imgs) {
                // 预览图片
                uni.previewImage({
                    urls: imgs.split()
                });
            },
            //在线客服,只有手机号的，请自己替换为手机号
            showChat() {
                // #ifdef H5
                let _this = this;
                window._AIHECONG('ini', {
                    entId: this.config.entId,
                    button: false,
                    appearance: {
                        panelMobile: {
                            tone: '#FF7159',
                            sideMargin: 30,
                            ratio: 'part',
                            headHeight: 50
                        }
                    }
                });
                //传递客户信息
                window._AIHECONG('customer', {
                    head: _this.userInfo.avatar,
                    名称: _this.userInfo.nickname,
                    手机: _this.userInfo.mobile
                });
                window._AIHECONG('showChat');
                // #endif
                // 客服页面
                // #ifdef APP-PLUS || APP-PLUS-NVUE
                this.$u.route('/pages/member/customerService/index');
                // #endif
                // 头条系客服
                // #ifdef MP-TOUTIAO
                if (this.shopMobile != 0) {
                    let _this = this;
                    tt.makePhoneCall({
                        phoneNumber: this.shopMobile.toString(),
                        success(res) { },
                        fail(res) { }
                    });
                } else {
                    _this.$u.toast('暂无设置客服电话');
                }
                // #endif
            },
            //获取分享URL
            getShareUrl() {
                let data = {
                    client: 2,
                    url: "/pages/share/jump/jump",
                    type: 1,
                    page: 9,
                    params: {
                        goodsId: this.goodsId,
                        groupId: this.groupId
                    }
                };
                let userToken = this.$db.get('userToken');
                if (userToken && userToken != '') {
                    data['token'] = userToken;
                }
                this.$u.api.share(data).then(res => {
                    this.shareUrl = res.data
                });
            },
            serviceTap() {
                this.modalTitle = "说明";
                this.modalType = 'service';
                this.showModal();
            },
            promotionTap() {
                this.modalTitle = "促销优惠";
                this.modalType = 'promotion';
                this.showModal();
            },
            // 显示modal弹出框
            selectTap() {
                this.modalTitle = "选择规格";
                this.modalType = 'select';
                this.showModal();
            },
            showModal() {
                this.bottomModal = true;
            },
            hideModal(e) {
                this.bottomModal = false;
                this.modalTitle = "";
                this.modalType = '';
            },
        },
        watch: {
            goodsId: {
                handler() {
                    this.getShareUrl();
                },
                deep: true
            }
        },
        //分享
        onShareAppMessage(res) {
            return {
                title: this.goodsInfo.name,
                imageUrl: this.goodsInfo.image,
                path: this.shareUrl
            }
        },
        onShareTimeline(res) {
            return {
                title: this.goodsInfo.name,
                imageUrl: this.goodsInfo.image,
                path: this.shareUrl
            }
        },
    };
</script>
<style lang="scss">
    @import '../../../../static/style/goodDetails.scss';
    .buyBtn { height: 74rpx; width: 90%; }
</style>
