<template>
    <view class="coreshop-header-slot-wrap u-row-center" :style="[navbarStyle]">
        <view class="coreshop-slot-btns u-flex u-flex-nowrap u-col-top" :style="{
							background: backgroundColor,
                            borderColor :leftIconColor
						}">
            <u-icon name="home-fill" :color="leftIconColor" :size="leftIconSize"  @tap="isGoBack" v-if="showHomeIcon && showLeftOneBtn"></u-icon>
            <u-icon name="arrow-left-double" :color="leftIconColor" :size="leftIconSize"  @tap="isGoBack"  v-if="!showHomeIcon && showLeftOneBtn"></u-icon>
            <view class="coreshop-slot-cut-off"  :style="{
							borderLeftColor: leftIconColor,
						}" v-if="showLeftOneBtn"></view>
            <u-icon name="list-dot" color="#fff" :color="leftIconColor" size="36" @click="doShowPopup"></u-icon>
        </view>
        <view class="coreshop-text-white u-font-32 coreshop-header-title">
            <view class="u-title u-line-1"
                  :style="{
							color: titleColor,
							fontSize: titleSize + 'rpx',
							fontWeight: titleBold ? 'bold' : 'normal'
						}">
                {{ title }}
            </view>
        </view>
        
        <view v-show="isShowPopup" class="coreshop-mark">
            <u-mask :maskClickAble="true" :show="isShowPopup" @click="maskClick"></u-mask>
			<view class="poptip">
                <view class="poptip-arrow poptip-arrow-top"><em>◆</em><i>◆</i></view>
                
                <!--增值业务-->
                <view class="u-padding-5 coreshop-bg-white u-margin-top-10 coreshop-user-info-tools-box">
                    <view class="u-padding-10 tools-view">
                        <view class="coreshop-text-black coreshop-text-bold u-font-lg tools-title">增值业务</view>
                    </view>
                    <view class="coreshop-tools-list-box">
                        <u-grid :col="4" :border="false">
                            <u-grid-item v-for="(item,i) in vas" :key="i" v-if="item.showItem" @click="goRoute(item.router)">
                                <u-icon :name="item.icon" :size="50" color="#666"></u-icon>
                                <view class="grid-text">{{ item.name }}</view>
                            </u-grid-item>
                        </u-grid>
                    </view>
                </view>
                <!--我的服务-->
                <view class="u-padding-5 coreshop-bg-white u-margin-top-10 coreshop-user-info-tools-box">
                    <view class="u-padding-10 tools-view">
                        <view class="coreshop-text-black coreshop-text-bold u-font-lg tools-title">我的服务</view>
                    </view>
                    <view class="coreshop-tools-list-box">
                        <u-grid :col="4" :border="false">
                            <u-grid-item v-for="(item,i) in utilityMenus" :key="i" @click="navigateToHandle(item.router)">
                                <u-icon :name="item.icon" :size="50"  color="#666"></u-icon>
                                <view class="grid-text">{{ item.name }}</view>
                            </u-grid-item>
                        </u-grid>
                    </view>
                </view>

            </view>
		</view>
    </view>
</template>

<script>
    import { commonUse } from '@/common/mixins/mixinsHelper.js';
    import { mapMutations, mapActions, mapState } from 'vuex';
    export default {
        mixins: [commonUse],
        name: "coreshop-navbar-slot",
        props: {
            // 左侧按钮组背景颜色
            backgroundColor: {
                type: String,
                default: '#fff'
            },
            // 左侧按钮组边框颜色
            borderColor: {
                type: String,
                default: '#fff'
            },
            // 是否显示第一个按钮
            showLeftOneBtn: {
                type: Boolean,
                default: true
            },
            // 返回箭头的颜色
            leftIconColor: {
                type: String,
                default: '#fff'
            },
            // 左边返回的图标
            leftIconName: {
                type: String,
                default: 'nav-back'
            },
            // 左边返回图标的大小，rpx
            leftIconSize: {
                type: [String, Number],
                default: '44'
            },
            // 导航栏标题
            title: {
                type: String,
                default: ''
            },
            // 标题的宽度，如果需要自定义右侧内容，且右侧内容很多时，可能需要减少这个宽度，单位rpx
            titleWidth: {
                type: [String, Number],
                default: '250'
            },
            // 标题的颜色
            titleColor: {
                type: String,
                default: '#606266'
            },
            // 标题字体是否加粗
            titleBold: {
                type: Boolean,
                default: false
            },
            // 标题的字体大小
            titleSize: {
                type: [String, Number],
                default: 32
            },
            // 对象形式，因为用户可能定义一个纯色，或者线性渐变的颜色
            background: {
                type: Object,
                default() {
                    return {
                    }
                }
            }
        },
        data() {
            return {
                isShowPopup: false,
                showHomeIcon: false,
                utilityMenus: {
                    myCoupon: {
                        name: '我的优惠券',
                        icon: 'coupon',
                        router: '/pages/member/coupon/index',
                        showItem: true
                    },
                    myBalance: {
                        name: '我的余额',
                        icon: 'rmb-circle',
                        router: '/pages/member/balance/index/index',
                        showItem: true
                    },
                    myInvoice: {
                        name: '我的发票',
                        icon: 'calendar',
                        router: '/pages/member/invoice/index',
                        showItem: true
                    },
                    myServices: {
                        name: '我的服务卡',
                        icon: 'bell',
                        router: '/pages/member/serviceOrder/index/index',
                        showItem: true
                    },
                    myIntegral: {
                        name: '我的积分',
                        icon: 'integral',
                        router: '/pages/member/integral/index',
                        showItem: true
                    },
                    myAddress: {
                        name: '地址管理',
                        icon: 'map',
                        router: '/pages/member/address/list/list',
                        showItem: true
                    },
                    myCollection: {
                        name: '我的收藏',
                        icon: 'bookmark',
                        router: '/pages/member/collection/index',
                        showItem: true
                    },
                    myHistory: {
                        name: '我的足迹',
                        icon: 'bag',
                        router: '/pages/member/history/index',
                        showItem: true
                    },
                },
                vas: {
                    storeMap: {
                        name: '门店列表',
                        icon: 'home',
                        router: '/pages/storeMap/storeMap',
                        showItem: false
                    },
                    servicePackage: {
                        name: '服务商品',
                        icon: 'list-dot',
                        router: '/pages/serviceGoods/index/index',
                        showItem: true
                    },
                    coupons: {
                        name: '优惠券',
                        icon: 'red-packet',
                        router: '/pages/coupon/coupon',
                        showItem: true
                    },
                    pinTuan: {
                        name: '拼团',
                        icon: 'grid',
                        router: '/pages/activity/pinTuan/list/list',
                        showItem: true
                    },
                    seckill: {
                        name: '秒杀',
                        icon: 'clock',
                        router: '/pages/activity/seckill/list/list',
                        showItem: true
                    },
                    groupBuying: {
                        name: '团购',
                        icon: 'trash',
                        router: '/pages/activity/groupBuying/list/list',
                        showItem: true
                    },
                    solitaire: {
                        name: '接龙',
                        icon: 'bag',
                        router: '/pages/activity/solitaire/list/list',
                        showItem: true
                    },
                },
            };
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
            // 整个导航栏的样式
            navbarStyle() {
                let style = {};
                // 合并用户传递的背景色对象
                Object.assign(style, this.background);
                return style;
            },
        },
        mounted() {
            let pages = getCurrentPages();
            var page = pages[pages.length - 2];
            console.log(page);
            if (!page) {
                this.showHomeIcon = true;
            }
            console.log(this.showHomeIcon);
        },
        methods: {
            navigateToHandle(pageUrl) {
                this.isShowPopup = false;
                uni.showLoading({
                    title: '跳转中...'
                });
                if (!this.hasLogin) {
                    this.$store.commit('showLoginTip', true);
                    uni.hideLoading();
                    return false;
                }
                uni.hideLoading();
                this.$u.route(pageUrl)
            },
            isGoBack() {
                this.isShowPopup = false;
                if (this.showHomeIcon) {
                    uni.switchTab({
                        url: '/pages/index/default/default'
                    });
                } else {
                    uni.navigateBack();
                }
            },
            doShowPopup() {
                this.isShowPopup = true;
            },
            // 遮罩被点击
            maskClick() {
                this.isShowPopup = false;
            },
        }
    };
</script>

<style scoped lang="scss">
    .coreshop-mark { position: fixed; top: 90px; left: 0; right: 0; z-index: 10074; width: 100%;
        .poptip { position: absolute; top: 0px; left: 0px; line-height: 16px; color: #db7c22; font-size: 12px; background: #fff; border: solid 1px #ffbb76; border-radius: 20rpx; box-shadow: 0 0 3px #ddd; margin: 0 20rpx; width: calc(100% - 40rpx); padding: 20rpx; z-index: 10072; }
        .poptip-arrow { position: absolute; overflow: hidden; font-style: normal; font-family: simsun; font-size: 12px; text-shadow: 0 0 2px #ccc;
            em { color: #ffbb76; }
            i { color: #fffcef; text-shadow: none; }
        }
        .poptip-arrow em, .poptip-arrow i { position: absolute; left: 0; top: 0; font-style: normal; }
        .poptip-arrow-top { height: 12px; width: 22px; left: 66px; margin-left: -12px; }
        .poptip-arrow-bottom { height: 6px; width: 12px; left: 12px; margin-left: -6px; }
        .poptip-arrow-left, .poptip-arrow-right { height: 12px; width: 6px; top: 12px; margin-top: -6px; }
        .poptip-arrow-top { top: -8px;
            em { top: -1px; }
            i { top: 0px; }
        }
        .poptip-arrow-bottom { bottom: -6px;
            em { top: -8px; }
            i { top: -9px; }
        }
        .poptip-arrow-left { left: -6px;
            em { left: 1px; }
            i { left: 2px; }
        }
        .poptip-arrow-right { right: -6px;
            em { left: -6px; }
            i { left: -7px; }
        }
    }
    </style>
