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
        <view class="coreshop-limited-seckill-box  bg-pink">
            <!--<text class="text-price text-xxl">{{ goodsInfo.pinTuanPrice || '0.00' }}</text>-->
            <text class="text-price text-xxl">{{ price || '0.00' }}</text>
            <view class="text-xs coreshop-cost-price-num price-4">
                <view class="linethrough">已售{{ goodsInfo.buyPinTuanCount || '0' }}件/剩余{{ product.stock || '0' }}件</view>
                <view>累计销售{{ goodsInfo.buyCount || '0' }}件</view>
            </view>
            <view class="text-right coreshop-time-right" v-if="goodsInfo.pinTuanRule.pinTuanStartStatus == 1">
                <view>距结束仅剩</view>
                <view class="text-xs u-margin-top-10">
                    <u-count-down :timestamp="goodsInfo.pinTuanRule.lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="24" separator-size="24"></u-count-down>
                </view>
            </view>
            <view class="text-right coreshop-time-right" v-if="goodsInfo.pinTuanRule.pinTuanStartStatus == 2">
                <view>即将开团</view>
                <view class="text-xs u-margin-top-10">
                    <u-count-down :timestamp="goodsInfo.pinTuanRule.lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="24" separator-size="24"></u-count-down>
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
            <view class="light bg-pink radius margin-top-sm coreshop-title-tip-box">
                <view>
                    <text class="text-sm">{{ goodsInfo.brief || '' }}</text>
                </view>
            </view>
        </view>

        <!--促销-->
        <view class="margin-top bg-white coreshop-view-box coreshop-promotion-view-box" v-if="promotion.length > 0" @tap="promotionTap">
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
        </view>

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

            <view class="flex flex-wrap text-sm" v-if="isSpes">
                <view class="basis-1">
                    <text class="text-gray">规格</text>
                </view>
                <view class="basis-8">
                    <text class="text-sm">{{ product.spesDesc || ''}}</text>
                </view>
                <!--<view class="basis-1">
                    <view class="text-gray text-right">
                        <text class="cuIcon-right icon" />
                    </view>
                </view>-->
            </view>
        </view>

        <!-- 团购拼单 -->
        <view class="tuan">
            <view class="cell-group margin-cell-group" v-if="pinTuanRecord.length > 0">
                <view class="cell-item right-img">
                    <view class="cell-item-hd">
                        <view class="cell-hd-title">{{ teamCount || '0' }}人在拼单，可直接参与</view>
                    </view>
                </view>
                <view class="group-swiper">
                    <swiper class="group-swiper-c" :class="swiperSet.groupHeight" :indicator-dots="swiperSet.indicatorDots" :autoplay="swiperSet.autoplay" vertical="true" circular="true" :interval="swiperSet.interval" :duration="swiperSet.duration">
                        <swiper-item v-for="(item, index) in pinTuanRecord" :key="index">
                            <view class="swiper-item">

                                <view class="cell-item" :class="item[0].isOverdue?'coreshop-lower-shelf':''">
                                    <view class="img-lower-box" :class="item[0].isOverdue?'coreshop-lower-box':''" v-if="item[0].isOverdue">已结束</view>
                                    <view class="cell-item-hd">
                                        <u-image width="80rpx" height="80rpx" :src="item[0].userAvatar" shape="circle" class="u-margin-right-8"></u-image>
                                        <view class="cell-hd-title">{{ item[0].nickName || '' }}</view>
                                    </view>
                                    <view class="cell-item-bd">
                                        <view class="cell-bd-view">
                                            <text class="cell-bd-text u-font-xs">
                                                还差
                                                <text class="text-red">{{ item[0].teamNums || '' }}人</text>
                                                拼成
                                            </text>
                                        </view>
                                        <view class="cell-bd-view">
                                            <view class="commodity-day">
                                                <text class="u-font-xs">剩余：</text>
                                                <u-count-down :timestamp="item[0].lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="22" separator-size="22" color="#ff7300" bg-color="#ffd4b0" @end="end(index,0)"></u-count-down>
                                            </view>
                                        </view>
                                    </view>
                                    <view class="cell-item-ft">
                                        <button class="cu-btn round bg-pink sm" @click="selectTap(2, item[0].teamId)" v-if="!item[0].isOverdue">去拼单</button>
                                        <button class="cu-btn round bg-grey sm" v-if="item[0].isOverdue">已结束</button>
                                    </view>

                                </view>
                                <view class="cell-item" v-if="item[1]" :class="item[1].isOverdue?'coreshop-lower-shelf':''">
                                    <view class="img-lower-box" :class="item[1].isOverdue?'coreshop-lower-box':''" v-if="item[1].isOverdue">已结束</view>
                                    <view class="cell-item-hd">
                                        <u-image width="80rpx" height="80rpx" :src="item[1].userAvatar" shape="circle" class="u-margin-right-8"></u-image>
                                        <view class="cell-hd-title">{{ item[1].nickName || '' }}</view>
                                    </view>
                                    <view class="cell-item-bd">
                                        <view class="cell-bd-view">
                                            <text class="cell-bd-text u-font-xs">
                                                还差
                                                <text class="text-red">{{ item[1].teamNums || '' }}人</text>
                                                拼成
                                            </text>
                                        </view>
                                        <view class="cell-bd-view">
                                            <view class="commodity-day">
                                                <text class="u-font-xs">剩余：</text>
                                                <u-count-down :timestamp="item[1].lastTime" separator="zh" :show-days="true" :show-hours="true" :show-minutes="true" :show-seconds="true" font-size="22" separator-size="22" color="#ff7300" bg-color="#ffd4b0" @end="end(index,1)"></u-count-down>
                                            </view>
                                        </view>
                                    </view>
                                    <view class="cell-item-ft">
                                        <button class="cu-btn round bg-pink sm" @click="selectTap(2, item[1].id)" v-if="!item[1].isOverdue">去拼单</button>
                                        <button class="cu-btn round bg-grey sm" v-if="item[1].isOverdue">已结束</button>
                                    </view>

                                </view>
                            </view>
                        </swiper-item>
                    </swiper>
                </view>
            </view>
            <view class="cell-group margin-cell-group" v-else>
                <view class="cell-item right-img">
                    <view class="cell-item-hd"><view class="cell-hd-title">暂无开团信息</view></view>
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
                    <text class="cu-tag bg-pink radius sm">
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

        <!--单个拼团数据展示-->
        <lvv-popup position="bottom" ref="pinTuanpop">
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
                        <image class="user-head-img cell-hd-icon have-none" :src="item.userAvatar" mode=""></image>
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
        </lvv-popup>

        <!-- 分享弹窗 -->
        <view class="u-padding-10">
            <lvv-popup position="bottom" ref="share">
                <!-- #ifdef H5 -->
                <shareByH5 :shareType="3" :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByH5>
                <!-- #endif -->
                <!-- #ifdef MP-WEIXIN -->
                <shareByWx :shareType="3" :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByWx>
                <!-- #endif -->
                <!-- #ifdef MP-ALIPAY -->
                <shareByAli :shareType="3" :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByAli>
                <!-- #endif -->
                <!-- #ifdef MP-TOUTIAO -->
                <shareByTt :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByTt>
                <!-- #endif -->
                <!-- #ifdef APP-PLUS || APP-PLUS-NVUE -->
                <shareByApp :shareType="3" :goodsId="goodsInfo.id" :shareImg="goodsInfo.image" :shareTitle="goodsInfo.name" :shareContent="goodsInfo.brief" :shareHref="shareHref" @close="closeShare()"></shareByApp>
                <!-- #endif -->
            </lvv-popup>
            <div id="qrCode" ref="qrCodeDiv"></div>
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
                                        <text class="cu-tag radius sm bg-pink">自营</text>
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
                    <button class="cu-btn bg-grey radius shadow-blur  y-f buyBtn" @tap="selectTap(1)">
                        <text class="price">￥{{ product.price || '0' }}</text>
                        <text>单独购买</text>
                    </button>
                    <button class="cu-btn bg-pink radius shadow-blur  y-f buyBtn" v-if="goodsInfo.pinTuanRule.pinTuanStartStatus == 1" @tap="selectTap(2)">
                        <text class="price">￥{{ pinTuanPrice || '0' }}</text>
                        <text>发起拼团</text>
                    </button>
                    <button class="cu-btn bg-pink radius shadow-blur  y-f buyBtn" v-if="goodsInfo.pinTuanRule.pinTuanStartStatus == 2">
                        <text class="price">￥{{ pinTuanPrice || '0' }}</text>
                        <text>即将开团</text>
                    </button>
                    <button class="cu-btn bg-pink radius shadow-blur y-f buyBtn" v-if="goodsInfo.pinTuanRule.pinTuanStartStatus == 3">
                        <text class="price">￥{{ pinTuanPrice || '0' }}</text>
                        <text>拼团已结束</text>
                    </button>
                </view>
            </view>
        </view>

        <!--弹出框-->
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
                                        <text class="text-price text-red margin-right-xs">{{ price || ''}}</text>
                                        <text class="text-sm text-gray text-through">￥{{ product.mktprice || ''}}</text>
                                        <!--<text class="cu-tag bg-gradual-red radius sm">
                                            <text class="cuIcon-hotfill"/>
                                            <text>秒杀中</text>
                                        </text>-->
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
                product: {}, // 货品详情
                shopRecommendData: [], // 本店推荐数据
                otherRecommendData: [], // 其他数据
                goodsParams: [], // 商品参数信息
                goodsComments: [], // 商品评论信息
                buyNum: 1, // 选定的购买数量
                minBuyNum: 1, // 最小可购买数量
                type: 2, // 1单独购买 2拼团
                isfav: false, // 商品是否收藏
                //拼团列表滑动数据
                swiperSet: {
                    indicatorDots: false,
                    autoplay: false,
                    interval: 2000,
                    duration: 500,
                    groupHeight: '',
                },

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
                serviceDescription: {
                    commonQuestion: [],
                    delivery: [],
                    service: [],
                }
            };
        },
        onLoad(e) {
            this.goodsId = e.id;
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
            // 获取商品详情
            getGoodsInfo() {
                let data = {
                    id: this.goodsId,
                    data: 1
                };
                // 如果用户已经登录 要传用户token
                let userToken = get('userToken');
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
                        cartType: this.type,
                        teamId: this.teamId
                    };
                    this.$u.api.addCart(data).then(res => {
                        if (res.status) {
                            this.hideModal(); // 关闭弹出层
                            let cartIds = res.data;
                            if (this.teamId == 0) {
                                this.$u.route('/pages/placeOrder/index/index?cartIds=' + JSON.stringify(cartIds) + '&orderType=' + this.type);
                            } else {
                                this.$u.route('/pages/placeOrder/index/index?cartIds=' + JSON.stringify(cartIds) + '&orderType=' + this.type + '&teamId=' + this.teamId);
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
                this.$refs.share.show();
            },
            closeShare() {
                this.$refs.share.close();
            },
            // 拼团弹出层
            pinTuanShow() {
                this.$refs.pinTuanpop.show();
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
                            console.log("type=" + this.type);
                            if (this.type == 2) {
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
                this.type = type;
                if (teamId) {
                    this.teamId = teamId;
                } else {
                    this.teamId == 0;
                }
                if (this.type == 2) {
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
    @import '../../../../static/style/goodDetails.scss';
    .buyBtn { height: 74rpx; }
        .buyBtn text { width: 100%; line-height: 1.3em; font-size: 24rpx; }
        .buyBtn .price { font-size: 28rpx; }

    .groupHeight { height: 122rpx !important; }

    .group-swiper-c { height: 242rpx; }
        .group-swiper-c .swiper-item .cell-item { height: 50%; }
            .group-swiper-c .swiper-item .cell-item .user-head-img { width: 80rpx; height: 80rpx; border-radius: 50%; }
            .group-swiper-c .swiper-item .cell-item .cell-hd-title { max-width: 200rpx; width: 100%; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; flex: 1; }
            .group-swiper-c .swiper-item .cell-item .cell-item-bd { min-width: 150rpx; text-align: center; display: block; }
                .group-swiper-c .swiper-item .cell-item .cell-item-bd .cell-bd-view { margin-bottom: 0; }
                .group-swiper-c .swiper-item .cell-item .cell-item-bd .cell-bd-text { float: none; }
        .group-swiper-c .commodity-day > text { background: none !important; padding: 0; }
        .group-swiper-c .swiper-item .cell-item .cell-item-ft .btn { font-size: 26rpx; color: #fff; background-color: #ff7159; text-align: center; }


    .ig-top { text-align: center; background-color: #fff; padding: 20upx 26upx; width: 100%; height: 350rpx; background: #FFFFFF; position: absolute; left: 0; bottom: 0; }
    .ig-top-t,
    .ig-top-m { margin-bottom: 20upx; }
        .ig-top-t > view { display: inline-block; padding: 0 10upx; color: #999; }
    .user-head-img-c { position: relative; width: 80upx; height: 80upx; border-radius: 50%; margin-right: 20upx; box-sizing: border-box; display: inline-block; border: 1px solid #f3f3f3; }
    .user-head-img-tip { position: absolute; top: -6upx; left: -10upx; display: inline-block; background-color: #FF7159; color: #fff; font-size: 22upx; z-index: 98; padding: 0 10upx; border-radius: 10upx; transform: scale(.8); }
    .user-head-img-c .user-head-img { width: 100%; height: 100%; border-radius: 50%; }
    .user-head-img-c:first-child { border: 1px solid #FF7159; }
    .uhihn { width: 80upx; height: 80upx; border-radius: 50%; display: inline-block; border: 2upx dashed #e1e1e1; text-align: center; color: #d1d1d1; font-size: 40upx; box-sizing: border-box; position: relative; }
        .uhihn > text { position: absolute; left: 50%; top: 50%; transform: translate(-50%, -50%); }
    .igtb-top { font-size: 32upx; color: #333; margin-bottom: 16upx; }
    .igtb-mid { margin-bottom: 16upx; }
        .igtb-mid .coreshop-btn { width: 100%; background-color: #FF7159; color: #fff; }
    .igtb-bot { font-size: 24upx; color: #666; }
    .cell-ft-text { max-width: 520upx; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .group-notice .cell-ft-text { color: #999; margin-left: 20upx; font-size: 26upx; }

    .coreshop-lower-shelf { }
        .coreshop-lower-shelf .cell-item-hd { opacity: 0.4; }
        .coreshop-lower-shelf .cell-item-bd { opacity: 0.4; }
        .coreshop-lower-shelf .cell-item-ft { opacity: 0.4; }
        .coreshop-lower-shelf .coreshop-lower-box { position: absolute; height: calc(100% - 40rpx); width: calc(100% - 20rpx); background-color: rgba(0, 0, 0, 0.6); text-align: center; font-size: 28rpx; color: #dedede; -webkit-transition: left .15s; transition: left .15s; z-index: 999; -webkit-transition: all .15s; transition: all .15s; line-height: 80rpx; }
</style>
