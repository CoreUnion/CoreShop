<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :is-back="false" :background="background" title="团购详情" title-color="#fff">
            <coreshopNavbarSlot titleColor="#fff" backgroundColor="#f37b1d" leftIconColor="#fff" :leftIconSize="33"></coreshopNavbarSlot>
        </u-navbar>
        <!--幻灯片-->
        <view class="coreshop-full-screen-banner-swiper-box">
            <swiper class="screen-swiper" circular autoplay @change="bannerSwiper">
                <swiper-item v-for="(item,index) in goodsInfo.album" :key="index" @click="clickImg(item)">
                    <u-image width="100%" height="750rpx" :src="item">
                        <u-loading slot="loading"></u-loading>
                    </u-image>
                </swiper-item>
            </swiper>
            <!--页码-->
            <text class="coreshop-bg-grey tag round coreshop-page">{{bannerCur + 1}} / {{goodsInfo.album.length}}</text>
        </view>

        <!--限时秒杀-->
        <view class="coreshop-limited-seckill-box coreshop-bg-orange">
            <text class="coreshop-text-price u-font-40">{{ product.price || '0.00' }}</text>
            <view class="u-font-xs coreshop-cost-price-num price-4">
                <view>已售{{ goodsInfo.buyCount || '0' }}件/剩余{{ product.stock || '0' }}件</view>
                <view>累计销售{{ goodsInfo.buyCount || '0' }}件</view>
            </view>
            <view class="u-text-right coreshop-time-right">
                <view>距结束仅剩</view>
                <view class="u-font-xs u-margin-top-10">
                    <u-count-down :timestamp="goodsInfo.groupTimestamp" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="24" separator-size="24"></u-count-down>
                </view>
            </view>
            <view class="u-text-right coreshop-share-right">
                <u-icon name="share" @click="goShare()" label="分享" size="40" label-size="22" label-color="#fff" label-pos="bottom"></u-icon>
            </view>
        </view>


        <!--标题-->
        <view class="coreshop-bg-white coreshop-common-view-box coreshop-good-title-view-box">
            <view class="title-view">
                <u-tag :text="goodsInfo.brand.name" type="error"  mode="dark" v-if="goodsInfo.brand" size="mini"/>
                <text class="coreshop-text-black u-font-lg coreshop-text-bold">{{ goodsInfo.name || '' }}</text>
            </view>
            <view class="light coreshop-bg-orange radius u-margin-top-20 coreshop-title-tip-box">
                <text class="u-font-sm">{{ goodsInfo.brief || '' }}</text>
            </view>
        </view>

         <!--服务-->
        <view class="u-margin-top-20 coreshop-bg-white coreshop-common-view-box" @tap="serviceTap" v-if="serviceDescription.service.length>0">
            <view class="coreshop-flex coreshop-flex-wrap u-font-sm">
                <view class="coreshop-basis-1">
                    <text class="coreshop-text-gray">服务</text>
                </view>
                <view class="coreshop-basis-7">
                    <view class="tag-view-box">
                        <u-icon name="checkmark-circle" size="20" label-size="24" color="#e54d42" :label="item.title"  v-for="(item, index) in serviceDescription.service" :key="index" class="u-margin-right-10"></u-icon>
                    </view>
                </view>
                <view class="coreshop-basis-2">
                    <view class="coreshop-text-gray u-text-right coreshop-justify-end">
                        <u-icon name="arrow-right-double"></u-icon>
                    </view>
                </view>
            </view>
        </view>

        <!--发货/规格-->
        <view class="u-margin-top-20 coreshop-bg-white coreshop-common-view-box" v-if="serviceDescription.delivery.length>0">
            <view class="coreshop-flex coreshop-flex-wrap u-font-sm">
                <view class="coreshop-basis-1">
                    <text class="coreshop-text-gray">发货</text>
                </view>
                <view class="coreshop-coreshop-basis-9" v-for="(item, index) in serviceDescription.delivery" :key="index">
                    <text class="u-font-sm">{{item.description}}</text>
                </view>
            </view>
            <view class="coreshop-solid-bottom u-margin-top-20 u-margin-bottom-20" v-if="serviceDescription.delivery.length>0" />
            <view class="coreshop-flex coreshop-flex-wrap u-font-sm u-padding-top-20" @tap="selectTap(0)" v-if="isSpes">
                <view class="coreshop-basis-1">
                    <text class="coreshop-text-gray">规格</text>
                </view>
                <view class="coreshop-basis-8">
                    <text class="u-font-sm">{{ product.spesDesc || ''}}</text>
                </view>
                <view class="coreshop-basis-1">
                    <view class="coreshop-text-gray u-text-right coreshop-justify-end">
                        <u-icon name="arrow-right-double"></u-icon>
                    </view>
                </view>
            </view>
        </view>

        <!--评论-->
        <view class="u-margin-top-20 coreshop-bg-white  u-padding-left-20 u-padding-right-20 u-padding-bottom-20" v-if="goodsComments.length">
            <view class="coreshop-flex coreshop-flex-wrap u-font-sm u-padding-top-20">
                <view class="coreshop-basis-2">
                    <text class="coreshop-text-black u-font-md">评价（{{goodsComments.length}}）</text>
                </view>
                <view class="coreshop-basis-7"></view>
                <view class="coreshop-basis-1">
                    <view class="coreshop-text-gray u-text-right coreshop-justify-end">
                        <u-icon name="arrow-right"  @tap="goGoodComments(goodsInfo.id)"></u-icon>
                    </view>
                </view>
            </view>
            <view v-for="(item, index) in goodsComments" :key="index">
                <view class="coreshop-solid-bottom u-margin-top-20 u-margin-bottom-20" />
                <view class="coreshop-flex coreshop-flex-wrap u-font-sm u-padding-20">
                    <view class="coreshop-basis-1">
                        <view class="coreshop-avatar sm round" :style="[{backgroundImage:'url('+ item.avatarImage +')'}]" />
                    </view>
                    <view class="coreshop-basis-9 u-font-sm">
                        <view>{{ (item.nickName && item.nickName != '')?item.nickName:item.mobile }}</view>
                        <view class="u-margin-top-20">{{ item.contentBody || ''}}</view>
                        <view class="coreshop-text-gray u-margin-top-10">
                            <u-rate :current="item.score" :disabled="true" size="26"></u-rate>
                        </view>
                        <view class="coreshop-text-gray u-margin-top-10">{{ item.createTime || ''}} {{ item.addon || ''}}</view>
                    </view>
                </view>
            </view>
        </view>

        <!--内容区-->
        <view class="u-margin-top-20 u-padding-top-10 coreshop-bg-white">
            <view class="u-font-md u-padding-top-10 u-padding-bottom-10 u-padding-left-20 u-padding-right-10">
                <u-icon name="more-circle" color="#e54d42"></u-icon>
                <text class="u-margin-left-10 coreshop-text-black">商品详情</text>
            </view>
            <!--参数-->
            <view class="grid col-2">
                <view class="col-item" v-for="(item, index) in goodsParams" :key="index">
                    <text class="coreshop-text-gray">{{ item.name || ''}}：</text>
                    <text class="coreshop-text-black">{{ item.value || ''}}</text>
                </view>
            </view>
            <view class="u-padding-top-10">
                <u-parse :html="goodsInfo.intro" :selectable="true" v-if="goodsInfo.intro"></u-parse>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="详情为空" mode="list"></u-empty>
                </view>
            </view>
        </view>

        <!-- 分享弹窗 -->
        <view class="u-padding-10">
            <u-popup  mode="bottom" v-model="shareBox" ref="share">
                <shareByWx :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByWx>
            </u-popup>
        </view>

        <!--常见问题-->
        <view class="u-margin-top-20 coreshop-bg-white u-margin-bottom-30 coreshop-common-view-box">
            <view class="u-font-md u-padding-bottom-20">
                <u-icon name="question-circle" color="#e54d42"></u-icon>
                <text class="u-margin-left-10 coreshop-text-black">常见说明</text>
            </view>

            <view class="coreshop-flex coreshop-flex-wrap u-margin-bottom-20" v-for="(item, index) in serviceDescription.commonQuestion" :key="index">
                <view class="coreshop-basis-2">
                    <u-tag :text="item.title" type="info" size="mini" />
                </view>
                <view class="coreshop-basis-8">
                    <view class="u-font-sm">{{item.description}}</view>
                </view>
            </view>

            <view class="coreshop-solid-bottom u-margin-top-20 u-margin-bottom-20" />
            <view class="u-text-center coreshop-text-blue  u-padding-top-20" @tap="goArticleList()">查看更多问题</view>
        </view>
        
        <!--商家及推荐-->
        <view class="u-margin-top-20 coreshop-bg-white coreshop-common-view-box coreshop-goods-shop-info-view-box">
            <view class="coreshop-shop-view">
                <view class="u-absolute">
                     <u-avatar :src="shopLogo"></u-avatar>
                </view>
                <view class="u-margin-left-20 u-padding-left-80 u-padding-right-80">
                    <view class="u-margin-bottom-10">{{shopName}}</view>
                    <view class="u-font-sm u-line-1">{{shareTitle}}</view>
                </view>
                <u-button type="error" size="mini" :plain="true" @click="doPhoneCall">联系商家</u-button>
            </view>
            <view class="coreshop-solid-bottom u-padding-top-10 u-padding-bottom-20" />
            <view class="live-tag-view u-margin-top-20 u-margin-bottom-20">
                <view class="text-view">
                    <u-tag text="已定位" mode="plain" size="mini" type="warning" />
                    <text class="u-margin-left-10 u-line-1">可直接获取商家地理位置信息</text>
                </view>
                <view class="u-font-sm coreshop-text-red go-map-box" @tap="goShopMap">
                    <text class="u-margin-right-10">去地图</text>
                    <u-icon name="arrow-right"></u-icon>
                </view>
            </view>
            <view class="coreshop-solid-bottom u-margin-top-20 u-margin-bottom-20" />
            <view class="coreshop-good-shop-recommend-list-box">
                <view class="u-font-sm u-padding-top-20 ">本店推荐</view>
                <!--滑动列表-->
                <view class="recommend-scroll-box">
                    <scroll-view class="recommend-scroll" scroll-x>
                        <block v-for="(items,indexs) in shopRecommendData" :key="indexs">
                            <view :id="['scroll' + (indexs + 1 )]" class="recommend-scroll-item" @tap="goGoodsDetail(items.id)">
                                <view class="coreshop-avatar xl radius" :style="[{backgroundImage:'url('+ items.image +')'}]" />
                                <view class="u-line-1-2 u-font-sm coreshop-text-black u-margin-top-20 u-margin-bottom-20 u-line-2" style="height: 64rpx;">{{items.name}}</view>
                                <view class="coreshop-text-red coreshop-text-price u-margin-top-20 u-margin-bottom-20 u-font-md">
                                    {{items.price}}元
                                </view>
                            </view>
                        </block>
                    </scroll-view>
                </view>
            </view>
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


        <u-popup class="coreshop-bottom-popup-box"  v-model="bottomModal" mode="bottom" height="60%">
            <view class="radius coreshop-bg-white">
                <!--标题-->
                <view class="coreshop-text-black u-text-center u-margin-top-30 u-margin-bottom-30 u-font-lg coreshop-title-bar">
                    <text>{{modalTitle}}</text>
                    <u-icon name="close"  @tap="hideModal" class="close-icon"></u-icon>
                </view>
                <!--内容区域-->
                <view class="coreshop-modal-content">
                    <!--服务区域-->
                    <view class="coreshop-common-view-box service" v-if="modalType=='service'">
                        <view v-for="(item, index) in serviceDescription.service" :key="index">
                            <view class="text-view">
                                <u-icon name="more-circle" color="#e54d42" :label="item.title"></u-icon>
                            </view>
                            <view class="u-font-sm text-list-view">
                                <view class="u-margin-left-20 coreshop-text-gray">{{item.description}}</view>
                            </view>
                        </view>
                    </view>

                    <!--促销区域-->
                    <view class="coreshop-common-view-box promotion" v-if="modalType=='promotion'">
                        <view class="text-view" v-for="(item, index) in promotion" :key="index">
                            <text class="cu-tag line-orange radius sm">{{item.name}}</text>
                            <text class="u-margin-left-20 u-line-5 coreshop-text-black">{{item.name}}</text>
                        </view>
                    </view>

                    <!--选择规格-->
                    <view class="coreshop-common-view-box select hide" :class="modalType=='select' ?'show':''">
                        <!--商品信息-->
                        <view class="coreshop-list menu-avatar">
                            <view class="coreshop-list-item">
                                <view class="coreshop-avatar radius lg" :style="[{backgroundImage:'url('+ product.images +')'}] " />
                                <view class="content">
                                    <view class="coreshop-text-price-view">
                                        <text class="coreshop-text-price coreshop-text-red u-margin-right-20">{{ product.price || ''}}</text>
                                        <text class="u-font-sm coreshop-text-gray coreshop-text-through u-margin-right-20">￥{{ product.mktprice || ''}}</text>
                                        <u-tag text="秒杀中" mode="plain" type="error" shape="circle" size="mini"/>
                                    </view>
                                    <view class="coreshop-text-black u-font-sm flex">
                                        <view class="u-line-1">已选: {{ product.spesDesc || '无'}}</view>
                                    </view>
                                </view>
                            </view>
                        </view>
                        <!--规格数据-->
                        <view class="coreshop-select-btn-list-box">
                            <spec :spesData="defaultSpesDesc" ref="spec" @changeSpes="changeSpes"></spec>
                            <view class="select-item">
                                <view class="coreshop-text-black">数量</view>
                                <view class="select-btn">
                                    <u-number-box v-model="buyNum" :min="minNums" :max="product.stock"></u-number-box>
                                </view>
                            </view>
                        </view>
                    </view>

                    <!--公共按钮-->
                    <view class="u-padding-30 u-text-center" v-if="modalType=='select'">
                        <u-button type="error" :custom-style="customStyle" size="medium"  @click="clickHandle()" :disabled='submitStatus' :loading='submitStatus' v-if="product.stock>0" shape="circle">确定</u-button>
                        <u-button type="default" size="medium" v-else>已售罄</u-button>
                    </view>
                </view>
            </view>
        </u-popup>
        <!--占位底部距离-->
        <view class="coreshop-tabbar-height" />
        <!--底部操作-->
        <view class="coreshop-good-footer-fixed">
            <view class="tabbar">
                 <!-- 客服按钮 -->
                <view class="action" >
                    <button open-type="contact" bindcontact="showChat" class="noButtonStyle">
                        <u-icon name="server-fill" :size="40" label="客服" :label-size="22" label-pos="bottom"></u-icon>
                    </button>   
                </view>
                <view class="action"  @click="collection">
                    <u-icon :name="[isfav?'star-fill':'star']" :size="40" :label="isfav?'已收藏':'收藏'" :label-size="22" label-pos="bottom"></u-icon>
                </view>
                <view class="action" @click="redirectCart">
                    <u-badge class="car-num" :count="cartNums?cartNums:0" type="error" :offset="[-3, -6]"></u-badge>
				    <u-icon name="shopping-cart" :size="40"  label="购物车" :label-size="22" label-pos="bottom"></u-icon>
                </view>
                <view class="btn-box">
                    <u-button type="error" :custom-style="customStyle" size="medium" @click="selectTap()" shape="circle">立即{{ typeName || '' }}</u-button>
                </view>
            </view>
        </view>

        <!-- 右边浮动球 -->
        <coreshop-fab horizontal="right" vertical="bottom" direction="vertical"></coreshop-fab>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>

    </view>
</template>
<script>
    import { mapMutations, mapActions, mapState } from 'vuex';
    import coreshopFab from '@/components/coreshop-fab/coreshop-fab.vue';
    import { goods, articles, commonUse, tools } from '@/common/mixins/mixinsHelper.js'
    import coreshopNavbarSlot from '@/components/coreshop-navbar-slot/coreshop-navbar-slot.vue';
    import spec from '@/components/coreshop-spec/coreshop-spec.vue';
    import shareByWx from '@/components/coreshop-share/shareByWx.vue';
    export default {
        mixins: [goods, articles, commonUse, tools],
        components: {
            coreshopFab,
            coreshopNavbarSlot,
            spec,
            shareByWx,
        },
        data() {
            return {
                background: {
                    backgroundColor: '#f37b1d'
                },
                customStyle: {
                    width: '100%',
                    borderColor:'#f37b1d',
                    backgroundColor:'#f37b1d',
                },
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
                type: 2,
                cartType: 3,
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
                shareBox: false,
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
                return this.$globalConstVars.apiBaseUrl + 'wap/' + page.route + '?id=' + this.goodsId + '&groupId=' + this.groupId;
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
                let userToken = this.$db.get('userToken');
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
                        type: this.type,
                        cartType: this.cartType,
                        objectId: this.groupId
                    };
                    this.$u.api.addCart(data).then(res => {
                        if (res.status) {
                            this.hideModal(); // 关闭弹出层
                            let cartIds = res.data;
                            this.$u.route('/pages/placeOrder/index/index?cartIds=' + JSON.stringify(cartIds) + '&orderType=' + this.cartType + '&objectId=' + this.groupId);

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
                this.shareBox = true;
            },
            closeShare() {
                this.shareBox = false;
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
<style lang="scss" scoped>
    @import "details.scss";
</style>
