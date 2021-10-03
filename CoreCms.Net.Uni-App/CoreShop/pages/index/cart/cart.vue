<template>
    <view class="wrap">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :is-back="false" :background="background">
            <view class="slot-wrap">
                <u-search :show-action="true" shape="round" v-model="searchKey" action-text="搜索" placeholder="请输入搜索内容" @custom="goSearch" @search="goSearch" :action-style="actionStyle"></u-search>
            </view>
        </u-navbar>
        <!-- 购物车为空 -->
        <view v-if="!shoppingCard.list || shoppingCard.list.length < 1">
            <view class="coreshop-emptybox">
                <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/cart.png'" icon-size="300" text="您的购物车空空如也" mode="list"></u-empty>
                <navigator class="coreshop-btn" url="/pages/category/index/index"  open-type="switchTab">随便逛逛</navigator>
            </view>
        </view>
        <!-- 购物车主结构  -->
        <view v-else class="cart-shoppingcard" v-for="(item, index) in shoppingCard.list" :key="index">
            <view class="cart-space-between cart-flex-vcenter" @click="goGoodsDetail(item.products.goodsId)">
                <view class="cart-shopp-name">
                    <text class="cart-h5 cart-bold">{{item.products.name}}</text>
                </view>
                <view class="cart-shopp-go cart-nowrap cart-flex-end">
                    <u-button class="cart-text" :plain="true" size="mini" @click="goGoodsDetail(item.products.goodsId)">浏览</u-button>
                </view>
            </view>
            <view style="height:25rpx;"></view>
            <view class="cart-shoppingcard-goods">
                <view class="cart-shoppingcard-goods-checkbtn">
                    <u-checkbox @change="itemChange" v-model="item.isSelect" active-color="red" :name="index"></u-checkbox>
                </view>
                <image class="cart-shoppingcard-goods-image" :src="item.products.images && item.products.images!='null' ?  item.products.images+'?x-oss-process=image/resize,m_lfit,h_320,w_240' : '/static/images/common/empty-banner.png'" mode="widthFix"></image>
                <view class="cart-shoppingcard-goods-body">
                    <!--<view class="cart-shoppingcard-goods-title">{{item.products.name}}</view>-->
                    <view class="cart-shoppingcard-goods-title" v-if="item.products.spesDesc">{{ item.products.spesDesc }}</view>
                    <view class="cart-shoppingcard-goods-title" v-if="item.products.promotionList">
                        <text class="cart-badge cart-bg-gray" v-for="(v, k) in item.products.promotionList" :key="k" :class="v.type !== 2 ? 'bg-gray' : ''"> {{ v.name }}</text>
                    </view>

                    <view class="cart-space-between" style="margin-top:20rpx;">
                        <text class="cart-shoppingcard-goods-price">￥{{item.products.price}}</text>
                        <view class="cart-shoppingcard-goods-number">
                            <u-number-box :disabled="false" :index="index" v-model="item.nums" @change="numberChange" :step="1" :min="1" :max="item.products.stock"></u-number-box>
                        </view>
                    </view>

                    <view class="cart-space-between">
                        <text class="cart-shoppingcard-remove cart-icons icon-msg" v-if="item.stockNo">库存不足</text>
                        <text class="cart-shoppingcard-remove cart-icons icon-msg" v-else-if="item.stockTension">库存紧张</text>
                        <text class="cart-shoppingcard-remove cart-icons" v-else=""></text>
                        <u-icon class="cart-shoppingcard-remove cart-icons icon-remove" name="trash" size="28" @click="removeGoods" :index="index" label="删除"></u-icon>
                    </view>
                </view>
            </view>


        </view>

        <view class="nobox"></view>

        <view class="cart-nowrap cart-flex-vcenter cart-border-t" style="background-color: #FFFFFF; width: 100%; position: fixed; left: 0; bottom: 0; z-index: 99;" v-if="shoppingCard.list.length >= 1">
            <view class="cart-shoppingcard-checkbtn cart-nowrap cart-flex-vcenter">
                <u-checkbox @change="itemChangeAll" v-model="selectAll" active-color="red">{{selectText}}</u-checkbox>
            </view>
            <view class="cart-shoppingcard-count cart-nowrap cart-flex-vcenter">
                <text class="cart-text">合计 :</text>
                <text class="u-font-26 coreshop-text-red">￥{{totalprice}}</text>
            </view>
            <view class="cart-shoppingcard-checkout cart-coreshop-bg-red" @tap="checkout">立即结算</view>
        </view>

        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>

</template>
<script>
    import { goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [goods],
        data() {
            return {
                background: {
                    backgroundColor: '#e54d42',
                },
                actionStyle: {
                    color: '#ffffff',
                },
                searchKey: '',
                // 总价
                totalprice: '0.00',
                // 选择文本
                selectText: '全选',
                // 购物车数据 可以来自 api 请求或本地数据
                shoppingCard: [],
                cartData: {}, //购物车数据
                cartIds: [], //选中ids
                goSettlement: false, //去结算按钮
                selectAll: true
            }
        },
        //页面加载
        onShow: function () {
            let userToken = this.$db.get('userToken');
            if (userToken) {
                this.getCartData(); //获取购物车数据
            }
        },
        onLoad: function () {
            // 初始化时候计算总价，如果使用api 获取购物车项目在 api 请求完成后执行此函数
            this.countTotoal();
        },
        computed: {
            // 从vuex中获取店铺名称
            shopName() {
                return this.$store.state.config.shopName;
            },
            GoodsStocksWarn() {
                return this.$store.state.config.goodsStocksWarn;
            }
        },
        methods: {
            //数组转字符串
            arrayToStr: function (array) {
                return array.toString();
            },

            //获取购物车数据
            getCartData: function () {
                let _this = this;
                let cartIds = _this.arrayToStr(_this.cartIds);
                let data = {
                    ids: cartIds,
                    display: 'all'
                };
                this.$u.api.cartList(data).then(res => {
                    if (res.status) {
                        _this.shoppingCard = res.data;
                        _this.showHandle(_this.shoppingCard); //数量设置
                        //console.log(_this.shoppingCard);
                    }
                });
            },

            //渲染前配置数据
            showHandle: function (data, flag = true) {
                let _this = this;
                let goSettlement = false;
                for (let i in data.list) {
                    //不可能购买0件
                    if (data.list[i].nums < 1) {
                        data.list[i].nums = 1;
                    }
                    //不能买大于库存的数量(库存不足)
                    let stockNo = false;
                    let maxStock = data.list[i].products.stock;
                    if (data.list[i].nums > data.list[i].products.stock) {
                        stockNo = true;
                        maxStock = data.list[i].nums;
                    }
                    data.list[i].maxStock = maxStock;
                    data.list[i].stockNo = stockNo;

                    //库存紧张
                    let stockTension = false;
                    if (_this.GoodsStocksWarn >= data.list[i].products.stock) {
                        stockTension = true;
                    }
                    data.list[i].stockTension = stockTension;

                    //设置样式
                    data.list[i].minStatus = 'normal';
                    data.list[i].maxStatus = 'normal';
                    if (data.list[i].nums == 1) {
                        data.list[i].minStatus = 'disabled';
                    }
                    if (data.list[i].nums == data.list[i].products.stock) {
                        data.list[i].maxStatus = 'disabled';
                    }

                    //设置规格参数
                    data.list[i].spes = [];
                    if (data.list[i].products.spesDesc != null) {
                        let spesArray = data.list[i].products.spesDesc.split(',');
                        for (let key in spesArray) {
                            let spesOne = spesArray[key].split(':');
                            data.list[i].spes.push(spesOne[1]);
                        }
                    }
                    //添加左滑效果
                    data.list[i].isTouchMove = false;
                    //是否可以去支付
                    if (data.list[i].isSelect) {
                        goSettlement = true;
                    }
                    //id转换为字符串
                    data.list[i].id = _this.arrayToStr(data.list[i].id);
                    //选中状态
                    if (flag) {
                        data.list[i].isSelect = true;
                        if (data.list[i].isSelect) {
                            if (_this.cartIds.indexOf(data.list[i].id) < 0) {
                                _this.cartIds.push(data.list[i].id);
                            }
                        }
                    }
                }
                data.goodsPromotionMoney = _this.$common.formatMoney(data.goodsPromotionMoney, 2, '');
                data.orderPromotionMoney = _this.$common.formatMoney(data.orderPromotionMoney, 2, '');
                data.amount = _this.$common.formatMoney(data.amount, 2, '');
                let isLoad = false;
                if (data.list.length < 1) {
                    isLoad = true;
                }
                let n = 0;
                for (let i in data.promotionList) {
                    n++;
                }

                _this.goSettlement = goSettlement;
                _this.isLoad = isLoad;
                _this.cartNums = n;

                if (flag) {
                    _this.cartData = data;
                } else {
                    _this.getCartData();
                }
                _this.countTotoal();
            },

            //计算总计函数
            countTotoal: function () {
                let _that = this;
                var total = 0;
                for (let i in _that.shoppingCard.list) {
                    if (_that.shoppingCard.list[i].isSelect) {
                        total += Number(_that.shoppingCard.list[i].products.price) * Number(_that.shoppingCard.list[i].nums);
                    }
                }
                _that.totalprice = _that.$common.formatMoney(total, 2, '');
            },
            numberChange: function (d) {
                var id = this.shoppingCard.list[d.index].productId;
                var nums = d.value;
                let _this = this;
                let data = {
                    id: id,
                    nums: nums
                };
                _this.$u.api.setCartNum(data).then(res => {
                    if (res.status) {
                        _this.shoppingCard.list[d.index].nums = d.value;
                    } else {
                        _this.$u.toast(res.msg);
                    }
                });
                //重新计算总价
                this.countTotoal();
            },
            removeGoods: function (index) {
                console.log(index);
                let _this = this;
                var index = index;
                uni.showModal({
                    title: '确认提醒',
                    content: '您确定要移除此商品吗？',
                    success: (e) => {
                        if (e.confirm) {
                            //移除数据库
                            let data = {
                                id: _this.shoppingCard.list[index].id
                            };
                            _this.$u.api.removeCart(data).then(res => {
                                if (res.status) {
                                    //清除已经勾选的商品和购物车数据
                                    var idIndex = _this.cartIds.indexOf(_this.shoppingCard.list[index].id);
                                    if (idIndex > -1) {
                                        _this.cartIds.splice(index, 1);
                                    }
                                    _this.shoppingCard.list.splice(index, 1);
                                    //计算总价
                                    _this.countTotoal();
                                    _this.$refs.uToast.show({ title: res.msg, type: 'success', back: false });
                                }
                            });

                        }
                    }
                })
            },
            checkout: function () {
                let _this = this;
                let cartData = _this.shoppingCard.list;
                let newData = '';
                for (let key in cartData) {
                    if (cartData[key].isSelect == true) {
                        newData += ',' + cartData[key].id;
                        _this.goSettlement = true;
                    }
                }
                if (newData.substr(0, 1) == ',') {
                    newData = newData.substr(1);
                }
                if (newData.length > 0) {
                    _this.$u.route('/pages/placeOrder/index/index?cartIds=' + JSON.stringify(newData));
                    return true;
                } else {
                    //没有选择不跳转
                }

            },
            // 商品选中
            itemChange: function (e) {
                var _this = this;
                var index = Number(e.name);
                var bl = e.value;
                let cartData = _this.shoppingCard;
                cartData.list[index].isSelect = bl;
                if (bl) {
                    if (_this.cartIds.indexOf(cartData.list[index].id) < 0) {
                        _this.cartIds.push(cartData.list[index].id);
                    }
                } else {
                    var idIndex = _this.cartIds.indexOf(cartData.list[index].id);
                    if (idIndex > -1) {
                        _this.cartIds.splice(index, 1);
                    }
                }
                _this.shoppingCard = cartData;
                this.countTotoal();
                if (_this.cartIds.length == _this.shoppingCard.list.length) {
                    this.selectAll = true;
                    let e = { value: true };
                    this.itemChangeAll(e);
                } else {
                    this.selectAll = false;
                }

            },
            itemChangeAll: function (e) {
                let _this = this;
                var bl = e.value;
                this.selectText = bl ? '全选' : '全不选';
                var ids = [];
                for (var i = 0; i < this.shoppingCard.list.length; i++) {
                    this.shoppingCard.list[i].isSelect = bl;
                    if (bl) {
                        ids.push(this.shoppingCard.list[i].id);
                    }
                }
                if (bl) {
                    _this.cartIds = ids;
                } else {
                    _this.cartIds = [];
                }

                this.countTotoal();
            },
            goSearch() {
                if (this.searchKey != '') {
                    this.$u.route('/pages/category/list/list?key=' + this.searchKey);
                } else {
                    this.$refs.uToast.show({
                        title: '请输入查询关键字',
                        type: 'warning',
                    })
                }
            }
        }
    }
</script>
<style lang="scss" scoped>
    @import "cart.scss";
</style>