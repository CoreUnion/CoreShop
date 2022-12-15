<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :is-back="false" :background="background" title="拼团详情" title-color="#fff">
            <coreshopNavbarSlot titleColor="#fff" backgroundColor="#e03997" leftIconColor="#fff" :leftIconSize="33"></coreshopNavbarSlot>
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
        <view class="coreshop-limited-seckill-box coreshop-bg-pink">
            <text class="coreshop-text-price u-font-40">{{ price || '0.00' }}</text>
            <view class="u-font-xs coreshop-cost-price-num price-4">
                <view class="coreshop-text-through">已售{{ goodsInfo.buyPinTuanCount || '0' }}件/剩余{{ product.stock || '0' }}件</view>
                <view>累计销售{{ goodsInfo.buyCount || '0' }}件</view>
            </view>
            <view class="u-text-right coreshop-time-right" v-if="goodsInfo.pinTuanRule.pinTuanStartStatus == 1">
                <view>距结束仅剩</view>
                <view class="u-font-xs u-margin-top-10">
                    <u-count-down :timestamp="goodsInfo.pinTuanRule.lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="24" separator-size="24"></u-count-down>
                </view>
            </view>
            <view class="u-text-right coreshop-time-right" v-if="goodsInfo.pinTuanRule.pinTuanStartStatus == 2">
                <view>即将开团</view>
                <view class="u-font-xs u-margin-top-10">
                    <u-count-down :timestamp="goodsInfo.pinTuanRule.lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="24" separator-size="24"></u-count-down>
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
            <view class="light coreshop-bg-pink radius u-margin-top-20 coreshop-title-tip-box">
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
            <view class="coreshop-flex coreshop-flex-wrap u-font-sm u-padding-top-20" v-if="isSpes">
                <view class="coreshop-basis-1">
                    <text class="coreshop-text-gray">规格</text>
                </view>
                <view class="coreshop-basis-8">
                    <text class="u-font-sm">{{ product.spesDesc || ''}}</text>
                </view>
                <view class="coreshop-basis-1">
                    <!--<view class="coreshop-text-gray u-text-right coreshop-justify-end"  @tap="selectTap(0)">
                        <u-icon name="arrow-right-double"></u-icon>
                    </view>-->
                </view>
            </view>
        </view>

        <!-- 团购拼单 -->
        <view class="tuan">
            <view class="coreshop-cell-group u-margin-top-20 u-margin-bottom-20" v-if="pinTuanRecord.length > 0">
                <view class="coreshop-cell-item right-img">
                    <view class="coreshop-cell-item-hd">
                        <view class="coreshop-cell-hd-title">{{ teamCount || '0' }}人在拼单，可直接参与</view>
                    </view>
                </view>
                <view class="group-swiper">
                    <swiper class="group-swiper-c" :class="swiperSet.groupHeight" :indicator-dots="swiperSet.indicatorDots" :autoplay="swiperSet.autoplay" vertical="true" circular="true" :interval="swiperSet.interval" :duration="swiperSet.duration">
                        <swiper-item v-for="(item, index) in pinTuanRecord" :key="index">
                            <view class="swiper-item">

                                <view class="coreshop-cell-item" :class="item[0].isOverdue?'coreshop-lower-shelf':''">
                                    <view class="img-lower-box" :class="item[0].isOverdue?'coreshop-lower-box':''" v-if="item[0].isOverdue">已结束</view>
                                    <view class="coreshop-cell-item-hd">
                                        <u-image width="80rpx" height="80rpx" :src="item[0].userAvatar" shape="circle" class="u-margin-right-8"></u-image>
                                        <view class="coreshop-cell-hd-title">{{ item[0].nickName || '' }}</view>
                                    </view>
                                    <view class="coreshop-cell-item-bd">
                                        <view class="coreshop-cell-bd-view">
                                            <text class="coreshop-cell-bd-text u-font-xs">
                                                还差
                                                <text class="coreshop-text-red">{{ item[0].teamNums || '' }}人</text>
                                                拼成
                                            </text>
                                        </view>
                                        <view class="coreshop-cell-bd-view">
                                            <view class="commodity-day">
                                                <text class="u-font-xs">剩余：</text>
                                                <u-count-down :timestamp="item[0].lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="22" separator-size="22" color="#ff7300" bg-color="#ffd4b0" @end="end(index,0)"></u-count-down>
                                            </view>
                                        </view>
                                    </view>
                                    <view class="coreshop-cell-item-ft">
                                        <u-button type="success" size="mini" @click="selectTap(2, item[0].teamId)" v-if="!item[0].isOverdue">去拼单</u-button>
                                        <u-button type="default" size="mini" v-if="item[0].isOverdue">已结束</u-button>
                                    </view>
                                </view>
                                <view class="coreshop-cell-item" v-if="item[1]" :class="item[1].isOverdue?'coreshop-lower-shelf':''">
                                    <view class="img-lower-box" :class="item[1].isOverdue?'coreshop-lower-box':''" v-if="item[1].isOverdue">已结束</view>
                                    <view class="coreshop-cell-item-hd">
                                        <u-image width="80rpx" height="80rpx" :src="item[1].userAvatar" shape="circle" class="u-margin-right-8"></u-image>
                                        <view class="coreshop-cell-hd-title">{{ item[1].nickName || '' }}</view>
                                    </view>
                                    <view class="coreshop-cell-item-bd">
                                        <view class="coreshop-cell-bd-view">
                                            <text class="coreshop-cell-bd-text u-font-xs">
                                                还差
                                                <text class="coreshop-text-red">{{ item[1].teamNums || '' }}人</text>
                                                拼成
                                            </text>
                                        </view>
                                        <view class="coreshop-cell-bd-view">
                                            <view class="commodity-day">
                                                <text class="u-font-xs">剩余：</text>
                                                <u-count-down :timestamp="item[1].lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="22" separator-size="22" color="#ff7300" bg-color="#ffd4b0" @end="end(index,1)"></u-count-down>
                                            </view>
                                        </view>
                                    </view>
                                    <view class="coreshop-cell-item-ft">
                                        <u-button type="success" size="mini" @click="selectTap(2, item[1].teamId)" v-if="!item[1].isOverdue">去拼单</u-button>
                                        <u-button type="default" size="mini" v-if="item[1].isOverdue">已结束</u-button>
                                    </view>
                                </view>
                            </view>
                        </swiper-item>
                    </swiper>
                </view>
            </view>
            <view class="coreshop-cell-group u-margin-top-20 u-margin-bottom-20" v-else>
                <view class="coreshop-cell-item right-img">
                    <view class="coreshop-cell-item-hd"><view class="coreshop-cell-hd-title">暂无开团信息</view></view>
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
            <view class="u-padding-10">
                <u-parse :html="goodsInfo.intro" :selectable="true" v-if="goodsInfo.intro"></u-parse>
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

        <!--单个拼团数据展示-->
        <u-popup mode="bottom" v-model="pinTuanpop">
            <view class="ig-top" v-if="teamInfo.teams.length > 0">
                <view class="ig-top-t">
                    <view class="">
                        剩余时间：
                        <u-count-down :timestamp="teamInfo.lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="24" separator-size="24" color="#666"></u-count-down>
                    </view>
                </view>
                <view class="ig-top-m">
                    <view class="user-head-img-c" v-for="(item, index) in teamInfo.teams" :key="index">
                        <view class="user-head-img-tip" v-if="item.recordId == item.teamId">拼主</view>
                        <image class="user-head-img coreshop-head-icon " :src="item.userAvatar" mode=""></image>
                    </view>
                    <view class="user-head-img-c uhihn" v-if="teamInfo.teamNums" v-for="n in teamInfo.teamNums" :key="n"><text>?</text></view>
                </view>
                <view class="ig-top-b">
                    <view class="igtb-top">
                        还差
                        <text class="red-price">{{ teamInfo.teamNums || '' }}</text>
                        人，赶快拼单吧
                    </view>
                    <view class="igtb-mid"><button class="coreshop-btn" @click="selectTap(2, teamInfo.id)">参与拼团</button></view>
                </view>
            </view>
        </u-popup>

        <!-- 分享弹窗 -->
        <view class="u-padding-10">
            <u-popup mode="bottom" v-model="shareBox" ref="share">
                <shareByWx :shareType="3" :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByWx>
            </u-popup>
            <div id="qrCode" ref="qrCodeDiv"></div>
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
        <view class="coreshop-good-footer-fixed">
            <view class="tabbar">
                <!-- 客服按钮 -->
                <view class="action">
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
                <view class="btn-group">

                    <u-button type="error" size="medium" @click="selectTap(1)" shape="square">
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-center coreshop-line-height-initial">
                            <text class="price">￥{{ product.price || '0' }}</text>
                            <text>单独购买</text>
                        </view>
                    </u-button>
                    <u-button type="success" size="medium" @click="selectTap(2)" shape="square" v-if="goodsInfo.pinTuanRule.pinTuanStartStatus == 1">
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-center coreshop-line-height-initial">
                            <text class="price">￥{{ pinTuanPrice || '0' }}</text>
                            <text>发起拼团</text>
                        </view>
                    </u-button>
                    <u-button type="primary" size="medium" shape="square" v-if="goodsInfo.pinTuanRule.pinTuanStartStatus == 2">
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-center coreshop-line-height-initial">
                            <text class="price">￥{{ pinTuanPrice || '0' }}</text>
                            <text>即将开团</text>
                        </view>
                    </u-button>
                    <u-button type="default" size="medium" shape="square" v-if="goodsInfo.pinTuanRule.pinTuanStartStatus == 3">
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-center coreshop-line-height-initial">
                            <text class="price">￥{{ pinTuanPrice || '0' }}</text>
                            <text>拼团已结束</text>
                        </view>
                    </u-button>
                </view>
            </view>
        </view>

        <!--弹出框-->
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
                            <text class="u-margin-left-20 u-line-1 coreshop-text-black">{{item.name}}</text>
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
                                        <text class="coreshop-text-price coreshop-text-red u-margin-right-20">{{ price || ''}}</text>
                                        <text class="u-font-sm coreshop-text-gray coreshop-text-through">￥{{ product.mktprice || ''}}</text>
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
                    <view class="coreshop-modal-footer-fixed" v-if="modalType=='select'">
                        <u-button type="error" :custom-style="customStyle" size="medium"  @click="clickHandle()" :disabled='submitStatus' :loading='submitStatus' v-if="product.stock>0">确定</u-button>
                        <u-button type="default" size="medium" v-else>已售罄</u-button>
                    </view>
                </view>
            </view>
        </u-popup>
        <!-- 右边浮动球 -->
        <coreshop-fab horizontal="right" vertical="bottom" direction="vertical"></coreshop-fab>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>

</template>
<script>
    import { mapMutations, mapActions, mapState } from 'vuex';
    import coreshopFab from '@/components/coreshop-fab/coreshop-fab.vue';
    import coreshopNavbarSlot from '@/components/coreshop-navbar-slot/coreshop-navbar-slot.vue';
    import { goods, articles, commonUse, tools } from '@/common/mixins/mixinsHelper.js'
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
                    backgroundColor: '#e03997'
                },
                customStyle: {
                    width: '100%',
                },
                bannerCur: 0,
                current: 0, // init tab位
                goodsId: 0, // 商品id
                pinTuanId: 0, // 团购ID
                goodsInfo: {}, // 商品详情
                cartNums: 0, // 购物车数量
                product: {}, // 货品详情
                shopRecommendData: [], // 本店推荐数据
                otherRecommendData: [], // 其他数据
                goodsParams: [], // 商品参数信息
                goodsComments: [], // 商品评论信息
                buyNum: 1, // 选定的购买数量
                minBuyNum: 1, // 最小可购买数量
                pinTuanType: 2, // 1单独购买 2拼团
                type: 2,
                cartType: 2,
                isfav: false, // 商品是否收藏
                //拼团列表滑动数据
                swiperSet: {
                    indicatorDots: false,
                    autoplay: false,
                    interval: 2000,
                    duration: 500,
                    groupHeight: '',
                },
                pinTuanpop: false,
                pinTuanPrice: 0,
                discountAmount: 0, //拼团优惠金额
                price: 0,
                teamCount: 0, //已经有多少人拼团
                pinTuanRecord: [], //拼团列表

                teamId: 0, //去参团的teamid
                teamInfo: {
                    teams: [],
                    lastTime: 0 //被邀请拼团倒计时
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
            console.log(e);
            this.goodsId = e.id;
            this.pinTuanId = e.pinTuanId;
            if (e.teamId) {
                this.teamId = e.teamId;
                this.getTeam(this.teamId);
            }
            if (this.goodsId) {
                this.getServiceDescription();
                this.getGoodsInfo();
                this.getGoodsParams();
                this.getGoodsComments();
            } else {
                this.$refs.uToast.show({ title: '获取失败', type: 'error', back: true });
            }
            // 获取购物车数量
            this.getCartNums();
            //获取随机推荐数据
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
                if (this.product.hasOwnProperty('defaultSpecificationDescription') && this.product.defaultSpecificationDescription && Object.keys(this.product.defaultSpecificationDescription).length) {
                    return true;
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
            shareHref() {
                let pages = getCurrentPages();
                let page = pages[pages.length - 1];
                return this.$globalConstVars.apiBaseUrl + 'wap/' + page.route + '?id=' + this.goodsId + '&pinTuanId=' + this.pinTuanId;
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
            // 获取商品详情
            getGoodsInfo() {
                let data = {
                    id: this.goodsId,
                    data: 1
                };
                // 如果用户已经登录 要传用户token
                let userToken = this.$db.get('userToken');
                if (userToken) {
                    data['token'] = userToken;
                }
                let _this = this;
                _this.$u.api.pinTuanGoodsInfo(data).then(res => {
                    //console.log(res);
                    if (res.status) {
                        if (res.data.length < 1) {
                            _this.$refs.uToast.show({ title: '该商品不存在，请返回重新选择商品。', type: 'error', back: true });
                        } else if (res.data.isMarketable == false) {
                            _this.$refs.uToast.show({ title: '该商品已下架，请返回重新选择商品。', type: 'error', back: true });
                        } else {
                            let info = res.data;
                            let products = res.data.product;

                            _this.goodsInfo = info;
                            _this.discountAmount = parseFloat(info.pinTuanRule.discountAmount).toFixed(2);
                            _this.product = _this.spesClassHandle(products);

                            _this.buyNum = _this.product.stock >= _this.minBuyNum ? _this.minBuyNum : 0;

                            _this.isfav = _this.goodsInfo.isfav;
                            _this.price = _this.pinTuanPrice = _this.$common.moneySub(_this.product.price, _this.discountAmount);

                            // 获取拼团记录
                            let pinTuanData = info.pinTuanRecord;
                            let newData = new Array();
                            for (var k = 0; k < pinTuanData.length; k++) {
                                if (k == 0 || k % 2 == 0) {
                                    if (k + 1 < pinTuanData.length) {
                                        var a = [pinTuanData[k], pinTuanData[k + 1]];
                                    } else {
                                        var a = [pinTuanData[k]];
                                    }
                                    newData.push(a);
                                }
                            }
                            pinTuanData.length < 2 ? (_this.swiperSet.groupHeight = 'groupHeight') : (_this.swiperSet.groupHeight = '');
                            _this.pinTuanRecord = newData;
                            _this.teamCount = info.pinTuanRecordNums;
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
            // 获取通过分享进来的拼团数据
            getTeam(id) {
                this.$u.api.getOrderPinTuanTeamInfo(
                    {
                        teamId: id
                    }).then(res => {
                        if (res.status) {
                            this.teamInfo = res.data;
                            if (res.data.status == 1) {
                                this.pinTuanShow();
                            } else {
                                this.teamId = 0;
                            }
                        } else {
                            this.$u.toast(res.msg);
                        }
                    }
                    );
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
            bindChange(val) {
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
                        objectId: this.pinTuanId,
                        teamId: this.teamId
                    };
                    this.$u.api.addCart(data).then(res => {
                        if (res.status) {
                            this.hideModal(); // 关闭弹出层
                            let cartIds = res.data;
                            if (this.teamId == 0) {
                                this.$u.route('/pages/placeOrder/index/index?cartIds=' + JSON.stringify(cartIds) + '&orderType=' + this.cartType + '&objectId=' + this.pinTuanId);
                            } else {
                                this.$u.route('/pages/placeOrder/index/index?cartIds=' + JSON.stringify(cartIds) + '&orderType=' + this.cartType + '&objectId=' + this.pinTuanId + '&teamId=' + this.teamId);
                            }
                        } else {
                            this.hideModal(); // 关闭弹出层
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
            end(index, number) {
                this.pinTuanRecord[index][number].isOverdue = true;
            },
            // 跳转到h5分享页面
            goShare() {
                this.shareBox = true;
            },
            closeShare() {
                this.shareBox = false;
            },
            // 拼团弹出层
            pinTuanShow() {
                this.pinTuanpop = true;
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
                    page: 3,
                    params: {
                        goodsId: this.goodsId,
                        teamId: this.teamId
                    }
                };
                //let userToken = this.$db.get('userToken');
                //if (userToken && userToken != '') {
                //    data['token'] = userToken;
                //}
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
            // 切换商品规格
            changeSpes(obj) {
                let index = obj.v;
                let key = obj.k;
                console.log(obj);
                let tmp_defaultSpecificationDescription = JSON.parse(this.product.defaultSpecificationDescription);
                //type = 1是立即购买，2是拼团购买
                if (tmp_defaultSpecificationDescription[index][key].hasOwnProperty('productId') && tmp_defaultSpecificationDescription[index][key].productId) {
                    let data = {
                        id: tmp_defaultSpecificationDescription[index][key].productId,
                        type: 'pinTuan' //商品类型
                    };
                    let userToken = this.$db.get('userToken');
                    if (userToken) {
                        data['token'] = userToken;
                    }
                    this.$u.api.pinTuanProductInfo(data).then(res => {
                        if (res.status == true) {
                            // 切换规格判断可购买数量
                            this.buyNum = res.data.stock > this.minBuyNum ? this.minBuyNum : res.data.stock;
                            this.product = this.spesClassHandle(res.data);
                            //products.price = _this.$common.moneySub(products.price,_this.discountAmount);
                            //this.pinTuanPrice = this.$common.moneySub(this.product.price, this.discountAmount);
                            console.log("type=" + this.pinTuanType);
                            if (this.pinTuanType == 2) {
                                //拼团
                                this.product.mktprice = this.product.price;//原价
                                this.price = this.pinTuanPrice = this.$common.moneySub(this.product.price, this.discountAmount);
                            } else {
                                this.price = this.product.price;
                                this.pinTuanPrice = this.$common.moneySub(this.product.price, this.discountAmount);
                            }
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
            // 显示modal弹出框
            selectTap(type, teamId) {
                console.log("pinTuanPrice：" + this.pinTuanPrice);
                console.log("product.price：" + this.product.price);
                console.log("price：" + this.price);
                this.pinTuanType = type;
                if (teamId) {
                    this.teamId = teamId;
                } else {
                    this.teamId == 0;
                }
                if (this.pinTuanType == 2) {
                    this.price = this.pinTuanPrice;
                } else {
                    this.price = this.product.price;
                }
                this.selectType = type;
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
            },
            teamId: {
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
    @import "details.scss";
</style>
