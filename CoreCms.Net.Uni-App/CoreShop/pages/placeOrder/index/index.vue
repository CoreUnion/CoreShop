<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="提交订单"></u-navbar>
        <div class="content">
            <view>
                <view class="u-margin-left-20 u-margin-right-20 u-margin-top-20">
                    <u-subsection :list="typeItems" mode="subsection" :current="typeCurrent" @change="onTypeItem" bg-color="#ffffff" active-color="#e54d42" v-if="storeSwitch == 1" style="border-radius:0;"></u-subsection>
                </view>
                <view class="content">
                    <view v-show="typeCurrent === 0 || typeCurrent === 1">
                        <!-- 收货地址信息 -->
                        <view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box" v-if="userShip && userShip.id" @click="showAddressList">
                            <view class="coreshop-bg-white coreshop-card address-view">
                                <view class="coreshop-list menu-avatar">
                                    <view class="coreshop-list-item">
                                        <view class="coreshop-bg-grey icon-view">
                                            <u-icon name="map"></u-icon>
                                        </view>
                                        <view class="content">
                                            <view class="coreshop-text-black">
                                                <text>收货人：{{ userShip.name || '' }}</text>
                                                <text class="u-margin-left-20">{{ userShip.mobile || ''}}</text>
                                            </view>
                                            <view class="coreshop-text-gray u-font-sm flex">
                                                <view class="u-line-1">{{ userShip.areaName || ''}} {{userShip.address || ''}}</view>
                                            </view>
                                        </view>
                                        <view class="action coreshop-text-gray">
                                            <u-icon name="arrow-right"  @click="goAddress()"></u-icon>
                                        </view>
                                    </view>
                                </view>
                                <view class="address-line" />
                            </view>
                        </view>
                        <view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box" v-else>
                            <view class="coreshop-bg-white coreshop-card address-view u-text-center u-padding-bottom-30">
                                <u-button type="error" size="mini" @click="goAddress()">添加收货地址</u-button>
                                <view class="address-line" />
                            </view>
                        </view>
                    </view>
                    <view v-show="typeCurrent === 2">
                        <!-- 门店信息 -->
                        <view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box" v-if="store && store.id" @click="goStorelist()">
                            <view class="coreshop-bg-white coreshop-card address-view">
                                <view class="coreshop-list menu-avatar">
                                    <view class="coreshop-list-item">
                                        <view class="coreshop-bg-grey icon-view">
                                            <u-icon name="map"></u-icon>
                                        </view>
                                        <view class="content">
                                            <view class="coreshop-text-black">
                                                <text>{{store.name|| ''}}</text>
                                                <text class="u-margin-left-20">{{store.mobile|| ''}}</text>
                                            </view>
                                            <view class="coreshop-text-gray u-font-sm flex">
                                                <view class="u-line-1">{{store.address|| ''}}</view>
                                            </view>
                                        </view>
                                        <view class="action coreshop-text-gray">
                                            <u-icon name="arrow-right"  @click="goStorelist()"></u-icon>
                                        </view>
                                    </view>
                                </view>
                                <view class="address-line" />
                            </view>
                        </view>
                        <view v-else class='u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box' @click="goStorelist()">
                            <view class="coreshop-bg-white coreshop-card address-view">
                                    <view class='u-padding-20 u-text-center'>暂无门店</view>
                                <view class="address-line" />
                            </view>
                        </view>
                    </view>
                </view>
                <view class='u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box' v-if="storeSwitch == 1 && typeCurrent === 2">
                    <view class="coreshop-form-group">
                        <view class="title">姓名</view>
                        <input class='coreshop-cell-bd-input' placeholder='请输入提货人姓名' v-model="storePick.name" style="width: 100%;"></input>
                    </view>
                    <view class="coreshop-form-group">
                        <view class="title">电话</view>
                        <input class='coreshop-cell-bd-input' placeholder='请输入提货人电话' v-model="storePick.mobile" style="width: 100%;"></input>
                    </view>
                </view>
                <!-- 商品列表信息 -->
                <!--商品信息-->
                <view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box">
                    <view class="coreshop-bg-white coreshop-card goods-view">
                        <view class="goods-info-view-box coreshop-solid-bottom" v-if="item.isSelect == true" v-for="(item, index) in products" :key="index">
                            <view class="coreshop-avatar radius lg" :style="[{backgroundImage:'url('+ item.products.images+'?x-oss-process=image/resize,m_lfit,h_200,w_200' +')'}]" />
                            <view class="goods-info-view">
                                <view class="coreshop-text-black u-line-2">{{ item.products.name || ''}}</view>
                                <view class="u-font-sm coreshop-text-gray" v-if="item.products.spesDesc !== null">{{ item.products.spesDesc || ''}}</view>
                                <view class="coreshop-tag-view" v-if="item.products.promotionList">
                                    <u-tag :text="v.name" mode="light" size="mini" v-for="(v, k) in item.products.promotionList" :key="k"/>
                                </view>
                                <view class="goods-price-view">
                                    <text class="coreshop-text-price coreshop-text-red u-font-lg">{{ item.products.price || ''}}</text>
                                    <view class='u-text-right goods-num'>× {{ item.nums || ''}}</view>
                                </view>
                            </view>
                        </view>
                    </view>
                </view>
            </view>

            <!--商品信息-->
            <view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box" v-if="userCoupons.length>0">
                <view class="coreshop-bg-white coreshop-card goods-view">
                    <view class="coreshop-text-black coreshop-gift-list">
                        <view class="u-line-1 title">优惠券</view>
                        <u-tag class="u-text-right" text="取消选择" closeable type="error" shape="circle" mode="dark" size="mini" @click="notUseCoupon()" v-if="usedCoupons.length>0" />
                    </view>
                    <!--滑动列表-->
                    <view class="coupon-scroll-box">
                        <scroll-view class="coupon-scroll" scroll-x>
                            <block v-for="(item, index) in userCoupons" :key="index">
                                <view :id="['scroll' + (index + 1 )]" class="coupon-scroll-item u-margin-top-20 flex coreshop-service-view couponBox" @click="couponHandle(index)">
                                    <view class="flex-sub bg-page u-padding-20 u-margin-10 radius ">
                                        <view class="coreshop-text-black u-line-1">{{ item.couponName || ''}}</view>
                                        <view class="coreshop-text-gray u-font-sm u-line-1"> {{ item.expression2 }}</view>
                                        <view class="coreshop-text-black">
                                            <text class="line-red u-font-20 radius">有效期：{{ item.stime + ' 至 ' + item.etime }}</text>
                                        </view>
                                        <view v-if="!item.checked && item.disabled">
                                            <view class="coreshop-corner-mark" />
                                            <u-icon name="checkmark" class="check-icon"></u-icon>
                                        </view>
                                        <view v-else-if="item.checked && item.disabled">
                                            <view class="coreshop-corner-mark check" />
                                            <u-icon name="checkmark" class="check-icon check"></u-icon>
                                        </view>
                                    </view>
                                </view>
                            </block>
                        </scroll-view>
                    </view>
                </view>
            </view>
            <!--支付方式-->
            <view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box">
                <view class="coreshop-bg-white coreshop-card pay-view">
                    <view class="coreshop-list menu">
                        <!-- 商户开启积分 并且用户有积分情况下 -->
                        <view class="coreshop-list-item" v-if="isOpenPoint === 1 && userPointNums > 0">
                            <!--<view class="coreshop-list-item arrow">-->
                            <view class="content">
                                <text class="coreshop-text-black">积分抵扣</text>
                                <view class="coreshop-text-gray u-font-xs flex">
                                    <text class="u-line-1">
                                        可用{{ canUsePoint}}积分，可抵扣{{ pointMoney}}元，共有{{ userPointNums}}积分
                                    </text>
                                </view>
                            </view>
                            <view class="action" @click="changePointHandle">
                                <view class="coreshop-text-gray fr">
                                    <switch :class="isUsePoint?'checked':''" :checked="isUsePoint"></switch>
                                </view>
                            </view>
                        </view>
                        <view class="coreshop-list-item" v-if="invoiceSwitch == 1" @click="goInvoice()">
                            <view class="content">
                                <text class="coreshop-text-black">发票</text>
                            </view>
                            <view class="action">
                                <text class="coreshop-text-gray">{{invoice.name|| '无'}}</text>
                                <u-icon name="arrow-right"></u-icon>
                            </view>
                        </view>
                    </view>
                </view>
            </view>
            <!--商品价格计算-->
            <view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box">
                <view class="coreshop-bg-white coreshop-card coreshop-price-view">

                    <view class="coreshop-text-black item-view">
                        <view class="u-line-1 title">商品总额</view>
                        <text class="coreshop-text-red coreshop-text-price u-text-right">{{ cartData.goodsAmount }}</text>
                    </view>

                    <view class="coreshop-text-black item-view">
                        <view class="u-line-1 title">商品优惠</view>
                        <text class="u-text-right">- {{ cartData.goodsPromotionMoney || '0'}}</text>
                    </view>
                    <view class="coreshop-text-black item-view">
                        <view class="u-line-1 title">订单优惠</view>
                        <text class="u-text-right">- {{ cartData.orderPromotionMoney || '0' }}</text>
                    </view>
                    <view class="coreshop-text-black item-view">
                        <view class="u-line-1 title">优惠券抵扣</view>
                        <text class="u-text-right">- {{ cartData.couponPromotionMoney  || '0'}}</text>
                    </view>
                    <view class="coreshop-text-black item-view">
                        <view class="u-line-1 title">积分抵扣</view>
                        <text class="u-text-right">- {{ cartData.pointExchangeMoney || '0'}}</text>
                    </view>
                    <view class="coreshop-text-black item-view">
                        <view class="u-line-1 title">
                            <text class="u-margin-right-20">运费</text>
                            <text class="cuIcon-question icon" />
                        </view>
                        <text class="u-text-right">{{ cartData.costFreight || '0'}}</text>
                    </view>
                </view>
            </view>
            <!--买家留言-->
            <view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box">
                <view class="coreshop-bg-white">
                    <view class="u-margin-top-20 u-padding-20">
                        <view class="title">买家留言</view>
                        <!--<input v-model="memo" placeholder="50字以内(选填)" name="input" />-->
                    </view>
                    <view class="u-padding-20">
                        <textarea class="memoBox" maxlength="50" @input="memoChange" placeholder="50字以内(选填)"></textarea>
                    </view>
                </view>
            </view>
            <!--占位底部距离-->
            <view class="coreshop-tabbar-height" />
            <!--底部操作-->
            <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
                <view class="u-flex u-flex-nowrap u-row-between  u-padding-20 w100">
                    <view class="coreshop-text-black coreshop-text-bold price-view">
                        <text class="u-margin-right-20">共 {{ productNums}} 件商品</text>
                        <text>合计<text class="coreshop-text-price coreshop-text-red u-font-lg u-margin-left-20"> {{ cartData.amount}}</text></text>
                    </view>
                    <u-button  size="medium"  type="error" @click="toPay" :disabled='submitStatus' :loading='submitStatus'>确认下单</u-button>
                </view>
            </view>
        </div>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>
<script>
    import { goods, articles } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [goods, articles],
        data() {
            return {
                typeItems: ['快递物流', '同城配送', '门店自提'],
                typeCurrent: 0,
                cartData: {}, // 购物车商品详情
                products: [], // 货品信息
                promotions: [], // 促销信息
                userShip: {}, // 用户收货地址
                receiptType: 1, // 订单类型 1快递物流发货订单，2同城配送，3是门店自提订单
                params: {
                    ids: 0, // 传递过来的购物车id
                    areaId: 0, // 收货地址id
                    couponCode: '', // 优惠券码列表(string)多张逗号分隔
                    point: 0,// 抵扣积分额
                    type: 1,//购物车类型
                    objectId: 0,//关联对象类型
                }, // 监听params参数信息 以重新请求接口
                // 发票信息
                invoice: {
                    type: 1,	// 类型 1不开发票 2个人发票 3公司发票
                    name: '',	// 发票抬头
                    code: ''	// 发票税号
                },
                memo: '', // 买家留言
                items: [
                    '选择优惠券',
                    '输入券码',
                ],
                orderType: 1, // 商品订单类型 1
                current: 0,
                isUsePoint: false,	// 是否勾选使用积分
                userPointNums: 0, // 用户的总积分
                canUsePoint: 0,	// 可以使用的积分
                pointMoney: 0, // 积分抵扣的金额
                userCoupons: [], // 用户的可用优惠券列表
                usedCoupons: {}, // 已经选择使用的优惠券
                inputCouponCode: '', // 输入的优惠券码
                optCoupon: '',// 当前选择使用的优惠券(暂存使用 如果接口返回不可用则剔除优惠券状态)
                store: {
                    id: 0,
                    name: '',
                    mobile: '',
                    address: ''
                },
                storePick: {
                    name: '',
                    mobile: ''
                },
                objectId: 0,//关联对象序列
                teamId: 0,//拼团订单分组序列
                submitStatus: false,
            }
        },
        components: {},
        onLoad(options) {
            console.log(options);
            let cartIds = options.cartIds;
            if (options.orderType) {
                this.params.orderType = options.orderType;
                this.params.type = options.orderType;
            }
            if (options.objectId) {
                this.objectId = options.objectId;
                this.params.objectId = options.objectId;
            }
            if (options.teamId) {
                this.teamId = options.teamId;
            }
            this.params.ids = JSON.parse(cartIds)
            if (!this.params.ids) {
                _this.$refs.uToast.show({ title: '获取失败', type: 'error', back: true })
            }
            // 获取用户的默认收货地址信息
            this.userDefaultShip()
            // 获取用户的可用优惠券信息
            this.getUserCounpons()
            //获取默认门店信息
            this.getDefaultStore();

            console.log("当前scene值:" + this.$store.state.scene);

        },
        onShow() {
            let userShip = this.$store.state.userShip;
            if (userShip) {
                this.userShip = userShip;
                this.params.areaId = userShip.areaId;
            }
            let userInvoice = this.$store.state.invoice;
            if (userInvoice) {
                this.invoice = userInvoice;
            }
        },
        methods: {
            // 切换门店
            onTypeItem(index) {
                if (this.typeCurrent !== index) {
                    this.typeCurrent = index;
                }
                let receiptType = 1;
                if (this.typeCurrent == 0) {
                    receiptType = 1;
                } else if (this.typeCurrent == 1) {
                    receiptType = 2;
                } else if (this.typeCurrent == 2) {
                    receiptType = 3;
                }
                this.receiptType = receiptType;
                this.getCartList();
            },
            // 跳转到门店列表
            goStorelist() {
                console.log("跳转到门店列表");
                this.$u.route('/pages/placeOrder/storeList/storeList')
            },
            // 没有收货地址时跳转
            goAddress() {
                console.log("没有收货地址时跳转");
                this.$u.route('/pages/member/address/list/list?type=order')
            },
            // 获取用户的默认收货地址
            userDefaultShip() {
                this.$u.api.userDefaultShip().then(res => {
                    if (res.status && res.data && Object.keys(res.data).length) {
                        this.userShip = res.data
                        this.params.areaId = this.userShip.areaId

                        this.storePick.name = res.data.name;
                        this.storePick.mobile = res.data.mobile;

                    }
                })
            },
            // 获取购物车商品详情
            getCartList() {
                let _that = this;
                let data = this.params
                data['receiptType'] = this.receiptType	// 区分订单类型  1快递物流,2同城配送，3上门自提订单
                this.$u.api.cartList(data).then(res => {
                    if (res.status) {
                        let data = res.data
                        // 判断是否开启积分抵扣 并且 没有勾选积分使用
                        if (this.isOpenPoint === 1 && !this.isUsePoint) {
                            let money = {
                                orderMoney: data.amount
                            }
                            this.$u.api.usablePoint(money).then(res => {
                                if (res.status) {
                                    this.userPointNums = res.data.point	// 用户总积分
                                    this.canUsePoint = res.data.availablePoint	// 可以使用的积分
                                    this.pointMoney = res.data.pointExchangeMoney	// 积分抵扣的总金额
                                }
                            })
                        }
                        // 所有价格转换
                        data.goodsPromotionMoneyOld = data.goodsPromotionMoney;
                        data.orderPromotionMoneyOld = data.orderPromotionMoney;
                        // 购物车详情
                        this.cartData = data
                        // 商品详情
                        this.products = data.list
                        //判断是否有库存
                        let noStock = true;
                        for (let i = 0; i < data.list.length; i++) {
                            if (data.list[i].isSelect) {
                                noStock = false;
                            }
                        }
                        if (noStock) {
                            this.$u.toast("您所挑选的商品已售罄，请重新添加哦");
                        }
                        // 优惠信息
                        this.promotions = data.promotionList
                        // 使用的优惠券信息
                        this.usedCoupons = data.coupon

                        this.inputCouponCode = ''
                        this.optCoupon = ''
                    } else {

                        this.$refs.uToast.show({
                            title: res.msg, type: 'error', back: false, callback: function () {
                                // 优惠券不可用状态判断
                                // 优惠券号码不存在 			15009
                                // 优惠券未开始				15010
                                // 优惠券已使用				15013
                                // 优惠券不符合使用规则		15014
                                // 优惠券不可使用多张			15015
                                // 优惠券已经过期			15011
                                let errStatus = [15009, 15010, 15011, 15013, 15014, 15015,]
                                if (errStatus.indexOf(res.data) !== -1) {
                                    console.log('删除使用的优惠券号码');
                                    // 删除使用的优惠券号码
                                    if (_that.current === 1) {
                                        _that.removeCouponCode(_that.inputCouponCode, _that.current)
                                    } else {
                                        // 取消选择使用的状态
                                        if (_that.optCoupon) {
                                            _that.userCoupons.forEach(item => {
                                                if (item.couponCode === _that.optCoupon) {
                                                    item.checked = false
                                                }
                                            })
                                        }
                                        _that.removeCouponCode(_that.optCoupon, _that.current)
                                    }
                                } else {
                                    console.log('未判断出内容');
                                }
                            }
                        })
                    }
                })
            },
            // 获取用户可用的优惠券信息
            getUserCounpons() {
                let data = {
                    display: 'no_used',
                    ids: this.params.ids
                }
                this.$u.api.getCartCoupon(data).then(res => {
                    if (res.status) {
                        let _list = res.data.list
                        let nowTime = Math.round(new Date().getTime() / 1000).toString()
                        _list.forEach(item => {
                            this.$set(item, 'checked', false)
                            // 判断优惠券是否有效(开始时间)
                            this.$set(item, 'disabled', item.startTime > nowTime ? true : false)
                            this.$set(item, 'cla', item.disabled ? 'cci-l bg-c' : 'cci-l')	// 绑定相应的class样式
                        })
                        this.userCoupons = _list
                    }
                })
            },
            // 点击使用/取消优惠券操作
            couponHandle(index) {
                // 更改使用/取消状态
                this.userCoupons[index].checked = !this.userCoupons[index].checked
                // 暂存当次选中使用的优惠券key
                this.optCoupon = this.userCoupons[index].couponCode
                let arr = []
                this.userCoupons.forEach(item => {
                    if (item.checked) {
                        arr.push(item.couponCode)
                    }
                })
                if (this.userCoupons[index].checked) {
                    // 使用
                    this.params.couponCode = arr.join()
                } else {
                    // 取消使用
                    let paramsCodes = this.params.couponCode.split(',')
                    let usedIndex = paramsCodes.indexOf(this.userCoupons[index].couponCode)
                    if (usedIndex !== -1) {
                        paramsCodes.splice(usedIndex, 1)
                        this.params.couponCode = paramsCodes.join()
                    }
                }
            },
            // 手输的优惠券码使用
            useInputCouponCode() {
                if (!this.inputCouponCode) {
                    this.$u.toast('请输入优惠券码')
                } else {
                    // 判断是否有使用的优惠券
                    if (this.params.couponCode.length > 0) {
                        this.params.couponCode += ',' + this.inputCouponCode
                    } else {
                        this.params.couponCode = this.inputCouponCode
                    }
                }
            },
            // 不使用优惠券
            notUseCoupon() {
                this.inputCouponCode = ''	// 清空手输的优惠券码
                this.userCoupons.forEach(item => {
                    item.checked = false
                }) // 取消所有选中的使用状态
                this.params.couponCode = ''	// 清空params优惠券码
            },
            // 移除/取消使用中的指定优惠券
            removeCouponCode(code, current) {
                let arr = this.params.couponCode.split(',')
                arr.splice(arr.indexOf(code), 1)
                current === 0 ? this.optCoupon = '' : this.inputCouponCode = ''
                this.params.couponCode = arr.join()
            },
            // 是否使用积分
            changePointHandle(e) {
                //this.switchA = e.detail.value
                if (this.userPointNums > 0) {
                    this.isUsePoint = !this.isUsePoint;
                    this.params.point = this.isUsePoint ? this.canUsePoint : 0;
                }
            },
            // 去支付
            toPay() {
                this.submitStatus = true;
                let receiptType = 1;
                if (this.typeCurrent == 0) {
                    receiptType = 1;
                } else if (this.typeCurrent == 1) {
                    receiptType = 2;
                } else if (this.typeCurrent == 2) {
                    receiptType = 3;
                }
                this.receiptType = receiptType;
                let data = {
                    cartIds: this.params.ids,
                    memo: this.memo,
                    couponCode: this.params.couponCode,
                    point: this.params.point,
                    receiptType: this.receiptType,
                    objectId: this.objectId,
                    teamId: this.teamId,
                    orderType: this.params.orderType, //订单类型
                    scene: this.$store.state.scene //场景值（用于确定小程序是否来源直播和视频号）
                }

                let delivery = {}
                // 判断是快递配送还是门店自提
                if (this.receiptType == 1 || this.receiptType == 2) {
                    if (!this.userShip.id || !this.params.areaId) {
                        this.$u.toast('请选择收货地址');
                        this.submitStatus = false;
                        return false;
                    }
                    // 快递配送
                    delivery = {
                        ushipId: this.userShip.id,
                        areaId: this.params.areaId
                    }
                }
                if (this.receiptType == 3) {
                    if (!this.store.id) {
                        this.$u.toast('请选择自提门店');
                        this.submitStatus = false;
                        return false;
                    }
                    if (!this.storePick.name) {
                        this.$u.toast('请输入提货人姓名');
                        this.submitStatus = false;
                        return false;
                    }
                    if (!this.storePick.mobile) {
                        this.$u.toast('请输入提货人电话');
                        this.submitStatus = false;
                        return false;
                    }
                    // 门店自提
                    delivery = {
                        storeId: this.store.id,
                        ladingName: this.storePick.name,
                        ladingMobile: this.storePick.mobile
                    }
                }

                // 发票信息
                data['taxType'] = this.invoice.type
                data['taxName'] = this.invoice.name
                data['taxCode'] = this.invoice.code
                data['source'] = 3;
                data = Object.assign(data, delivery)
                this.$u.api.createOrder(data).then(res => {
                    this.submitStatus = false;
                    if (res.status) {
                        // 创建订单成功 去支付
                        // 判断是否为0元订单,如果是0元订单直接支付成功
                        if (res.data.payStatus == '2') {
                            this.$u.route({ type: 'redirectTo', url: '/pages/payment/result/result?orderId=' + res.data.orderId });
                        } else {
                            this.$u.route({ type: 'redirectTo', url: '/pages/payment/pay/pay?orderId=' + res.data.orderId + '&type=' + this.orderType });
                        }
                        this.subscription();
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            },
            //发起订阅
            subscription() {
                let _this = this;
                this.$u.api.getSubscriptionTmplIds(null).then(res => {
                    if (res.status) {
                        console.log(res.data);
                        if (res.data.length > 0) {
                            console.log('进入订阅发起');
                            uni.requestSubscribeMessage({
                                tmplIds: res.data,
                                success(ress) {
                                    if (res.errMsg == "requestSubscribeMessage:ok") {
                                        console.log(ress);
                                    }
                                }, fail(ress) {
                                    console.log(ress);
                                }
                            });
                        }
                    } else {
                        this.$u.toast('消息订阅配置信息获取失败');
                    }
                });
            },
            // 跳转发票页面
            goInvoice() {
                this.$u.route('/pages/placeOrder/invoice/invoice')
            },
            // 跳转我的收货地址列表
            showAddressList() {
                this.$u.route('/pages/member/address/list/list?type=order')
            },
            // tab点击切换
            onClickItem(index) {
                if (this.current !== index) {
                    this.current = index;
                }
            },
            // 获取默认店铺
            getDefaultStore() {
                if (this.storeSwitch == 1) {
                    console.log("获取默认店铺");
                    this.$u.api.defaultStore().then(res => {
                        if (res.status) {
                            if (res.data && res.data.id) {
                                let store = {
                                    id: res.data.id || 0,
                                    name: res.data.storeName || '',
                                    mobile: res.data.mobile || '',
                                    address: res.data.address || ''
                                }
                                this.store = store;
                            } else {
                                this.$u.toast('商家未配置默认自提店铺！');
                            }
                        }
                    });
                }
            },
            memoChange(e) {
                //console.log(e);
                this.memo = e.detail.value
            },
        },
        computed: {
            // 计算购物车商品数量
            productNums() {
                let nums = 0
                for (let i in this.cartData.list) {
                    if (this.cartData.list[i].isSelect) {
                        nums += this.cartData.list[i].nums;
                    }
                }
                return nums
            },
            // 判断商户是否开启积分抵扣 1开启 2未开启
            isOpenPoint() {
                return this.$store.state.config.pointSwitch
            },
            // 获取使用的优惠券名称
            usedCouponsCompute() {
                var userCouponsCount = this.userCoupons.length;

                let name = userCouponsCount + '张可用'
                if (Object.keys(this.usedCoupons).length) {
                    let coupons = []
                    for (let i in this.usedCoupons) {
                        coupons.push(this.usedCoupons[i])
                    }
                    name = coupons.join()
                }
                return name
            },
            // 判断是否开启发票功能
            invoiceSwitch() {
                return this.$store.state.config.invoiceSwitch || 2;
            },
            // 判断店铺开关
            storeSwitch() {
                return this.$store.state.config.storeSwitch || 2;
            },
            // 根据接口返回数据判断是否使用优惠券
            couponIsUsed() {
                return this.cartData.coupon && this.cartData.coupon.length > 0;
            },

        },
        watch: {
            // 监听数据状态(切换收货地址, 是否使用优惠券, 是否使用积分) 重新请求订单数据
            params: {
                handler() {
                    this.getCartList();
                },
                deep: true
            }
        }
    }
</script>

<style scoped lang="scss">
    @import 'index.scss';
</style>