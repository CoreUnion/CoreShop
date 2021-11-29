<template>
    <view v-show="showPage">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :custom-back="goHome" back-icon-name="home" :title="form.name"></u-navbar>
        <form @submit="formSubmit" bindreset="formReset">
                <view v-if="form.headType==1">
                    <view class="banner">
                        <image :src='slideImg[0]' mode='widthFix'></image>
                    </view>
                </view>
                <!-- 轮播图 -->
                <view v-else-if="form.headType == 2">
                    <view>
                        <view class='sw'>
                            <swiper>
                                <swiper-item v-for="(item,itemIndex) in slideImg" :key="itemIndex">
                                    <image :src="item" class="slide-image" mode='widthFix' />
                                </swiper-item>
                            </swiper>
                        </view>
                    </view>
                </view>
                <view v-else-if="form.headType==3">
                    <view class='video'>
                        <video :src='form.headTypeVideo' :poster="form.headTypeValue"></video>
                    </view>
                </view>
                <!-- 纯文字 -->
                <view v-if="form.description !=''">
                    <view class='plaintext'>
                        <text>{{form.description}}</text>
                    </view>
                </view>
            <view class="u-margin-bottom-20 u-margin-top-20 u-padding-10 coreshop-bg-white">
                <view v-for="(item,index) in form.items" :key="index">
                    <view class='goods-box-item' v-if="item.type=='goods'">
                        <view class='input-box-item-left u-padding-bottom-20'>
                            <text>{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必选）</text>：</text>
                        </view>
                        <image class='goods-img' :src='item.good.image' mode='aspectFit'></image>
                        <view class='goods-right'>
                            <view class='goods-name'>{{item.good.name}}</view>
                            <view class='goods-mid'>
                                <text>已售{{item.good.buyCount}}</text>
                            </view>
                            <view class='goods-buttom'>
                                <view class="goods-price">￥{{item.good.price}}</view>
                                <view class='choose-specs' @click="specifications($event,item)" data-type='1' :data-goods="item.good.id" :data-id="item.id" data-statu="openspecs">
                                    选规格
                                </view>
                                <text class='order-num' v-if="item.cartCount> 0">{{item.cartCount || 0}}</text>
                            </view>
                        </view>
                    </view>
                    <!-- 文本框 -->
                    <view class='form-input-box-item' v-if="item.type=='text'">
                        <view class='input-box-item-left'>
                            <text>{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必填）</text>：</text>
                        </view>
                        <view class='input-box-item-right'>
                            <input class='ib-item-input' type="text" :name="'objName'+item.id" :data-id="item.id" v-model="item.defaultValue" :placeholder="'请输入'+item.name"></input>
                        </view>
                    </view>
                    <!-- 日期 -->
                    <view class='form-input-box-item' v-if="item.type=='date'">
                        <view class='input-box-item-left'>
                            <text>{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必选）</text>：</text>
                        </view>
                        <view class='input-box-item-right'>
                            <view class="ib-item-mid">
                                <picker mode="date" :name="'objName'+item.id" :value="item.defaultValue" @change="bindDateChange($event,item)" :data-id='item.id'>
                                    <view>{{item.defaultValue}}</view>
                                </picker>
                                <image class='icon-img-right' :src="$globalConstVars.apiFilesUrl+'/static/images/common/ic-unfold.png'"></image>
                            </view>
                        </view>
                    </view>
                    <!-- 时间 -->
                    <view class='form-input-box-item' v-if="item.type=='time'">
                        <view class='input-box-item-left'>
                            <text>{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必选）</text>：</text>
                        </view>
                        <view class='input-box-item-right'>
                            <view class="ib-item-mid">
                                <picker class="weui-btn" :name="'objName'+item.id" mode="time" :value="item.defaultValue" @change="bindTimeChange($event,item)" :data-id='item.id'>
                                    <view>{{item.defaultValue}}</view>
                                </picker>
                                <image class='icon-img-right' :src="$globalConstVars.apiFilesUrl+'/static/images/common/ic-unfold.png'"></image>
                            </view>
                        </view>
                    </view>
                    <!-- 范围选择 -->
                    <!-- 多选 -->
                    <view class='form-input-box-item' v-if="item.type=='checbox'">
                        <view class='input-box-item-left'>
                            <text>{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必选）</text>：</text>
                        </view>
                        <view class='input-box-item-right'>
                            <view class='checkout-list'>
                                <checkbox-group @change="checkboxChange($event,item)" :data-value="item.id" :name="'objName'+item.id">
                                    <label class="checkout-item" v-for="(checkboxItem,itemIndex) in item.checkboxValue" :key="itemIndex">
                                        <view class="checkout-item-c">
                                            <checkbox class="" :value="checkboxItem.value" :checked="checkboxItem.checked" /> {{checkboxItem.value}}
                                        </view>
                                    </label>
                                </checkbox-group>
                            </view>
                        </view>
                    </view>
                    <!-- radio时处理 -->
                    <view class='form-input-box-item' v-if="item.type=='radio'">
                        <view class='input-box-item-left'>
                            <text>{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必选）</text>:</text>
                        </view>
                        <view class='input-box-item-right'>
                            <radio-group class="uni-list" @change="radioChange($event,item)" :data-value="item.id" :name="'objName'+item.id">
                                <label class="u-margin-right-40" v-for="(radioItem, itemIndex) in  item.radioValue" :key="itemIndex">
                                    <view class="coreshop-display-inline-block">
                                        <radio class="a-radio" :id="radioItem" :value="radioItem" checked=true v-if="radioItem==item.defaultValue"></radio>
                                        <radio class="a-radio" :id="radioItem" :value="radioItem" v-if="radioItem!=item.defaultValue"></radio>
                                    </view>
                                    <view class="coreshop-display-inline-block">
                                        <label class="label-2-text" :for="radioItem">
                                            <text>{{radioItem}}</text>
                                        </label>
                                    </view>
                                </label>
                            </radio-group>
                        </view>
                    </view>
                    <!-- 省市区选择 -->
                    <view class='form-input-box-item' v-if="item.type=='area'">
                        <view class='input-box-item-left'>
                            <text>{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必选）</text>：</text>
                        </view>
                        <view class='input-box-item-right'>
                            <view class="ib-item-mid">
                                <input class="ib-item-input" :value="pickerValue" @focus="showThreePicker" :name="'objName'+item.id" style="width: 100%;" />
                                <u-select v-model="show" mode="mutil-column-auto" :list="pickerList" :default-value="pickerIndex" @confirm="onConfirm"></u-select>
                            </view>
                        </view>
                    </view>
                    <!-- 金额 -->
                    <view class='form-input-box-item' v-if="item.type=='money'">
                        <view class='input-box-item-left'>
                            <text>{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必填）</text>：</text>
                        </view>
                        <view class='input-box-item-right'>
                            <view class="ib-item-mid">
                                <input class='ib-item-input' type="digit" :name="'objName'+item.id" v-model="item.defaultValue" 
                                        :placeholder="'请输入'+item.name"></input>
                            </view>
                        </view>
                    </view>
                    <!-- 密码 -->
                    <view class='form-input-box-item' v-if="item.type=='password'">
                        <view class='input-box-item-left'>
                            <text>{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必填）</text>：</text>
                        </view>
                        <view class='input-box-item-right'>
                            <view class="ib-item-mid">
                                <input class='ib-item-input' type='password' :name="'objName'+item.id" v-model="item.defaultValue" 
                                        :placeholder="'请输入'+item.name"></input>
                            </view>
                        </view>
                    </view>
                    <!-- 图片 -->
                    <view class='form-input-box-item' v-if="item.type=='image'">
                        <view class='u-font-28'>上传{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必传）</text>（限制3张）</view>
                        <view class='u-margin-top-15'>
                            <view class='upload-img-list'>
                                <view class='upload-img-bd'>
                                    <view class='upload-img' v-for="(picItem, i) in item.pics" :key="i">
                                        <image @click='picDel(item,index,i)' :data-index="i" class='del-img' :src="$globalConstVars.apiFilesUrl+'/static/images/common/del.png'"></image>
                                        <image class='upload-camera' :src="picItem" mode='aspectFit'></image>
                                        <input type='text' hidden='hidden' :name="'objName'+item.id" v-model="item.pics" />
                                    </view>
                                </view>
                                <view class='upload-img-hd'>
                                    <image class='upload-camera' :src="$globalConstVars.apiFilesUrl+'/static/images/common/camera.png'" @click="picChoose($event,item,index)"
                                            :data-id="item.id"></image>
                                </view>
                            </view>
                        </view>
                    </view>
                    <!-- 文本域 -->
                    <view class='form-input-box-item' v-if="item.type=='textarea'">
                        <view class='u-font-28'>{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必填）</text>：</view>
                        <view class='u-margin-top-15'>
                            <textarea :name="'objName'+item.id" class='ib-item-textarea' :placeholder="'请输入'+item.name" ></textarea>
                        </view>
                    </view>
                    <!-- 定位 -->
                    <view class='form-input-box-item' v-if="item.type=='coordinate'">
                        <view class='input-box-item-left'>
                            <text>{{item.name}}<text v-if="item.required" class="coreshop-bg-red u-font-22">（必选）</text>：</text>
                        </view>
                        <view class='input-box-item-right'>
                            <view class="ib-item-mid coreshop-justify-start">
                                <image class='icon-img' :src="$globalConstVars.apiFilesUrl+'/static/images/common/ic-location.png'"></image>
                                <input class='ib-item-input u-margin-right-40' :name="'objName'+item.id" :value="item.defaultValue"
                                        disabled='disabled' placeholder="点击获取位置信息" @click="chooseLocation($event,item,index)" :data-id='item.id' />
                            </view>
                        </view> 
                    </view>
                </view>
            </view>
            <view class='goods-bottom' v-if="form.type==1">
                <text class='coreshop-float-right u-font-28 coreshop-text-black'>
                    合计
                    <text class='coreshop-text-red u-font-30'>￥{{goodsTotalMoney}}</text>
                </text>
            </view>
            <view class="coreshop-tabbar-height"></view>
            <!-- 底部按钮 -->
            <view class='coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom'>
                <view class="u-padding-10">
                    <button :style='{backgroundColor:form.buttonColor}' class="bottom-btn" data-statu="open" form-type="submit" :disabled='submitStatus' :loading='submitStatus'>
                    {{buttonName}}
                    </button>
                </view>
            </view>
        </form>

        <!--弹出框-->
        <u-popup class="coreshop-bottom-popup-box" v-model="bottomModal" mode="bottom"  border-radius="14" closeable="true" height="60%">
            <!-- 多规格商品弹出 -->
            <block v-if="showSpecs">
                <view class="coreshop-bg-white">
                    <!--标题-->
                    <view class="coreshop-text-black u-text-center u-margin-top-30 u-margin-bottom-30 u-font-lg coreshop-title-bar">
                        <text>选择商品</text>
                    </view>
                    <!--内容区域-->
                    <view class="coreshop-modal-content">
                        <!--选择规格-->
                        <view class="coreshop-common-view-box select">
                            <!--商品信息-->
                            <view class="coreshop-list menu-avatar">
                                <view class="coreshop-list-item">
                                    <view class="coreshop-avatar radius lg" :style="[{backgroundImage:'url('+ goodsInfoImage +')'}] " />
                                    <view class="content">
                                        <view class="coreshop-text-price-view">
                                            <text class="coreshop-text-price coreshop-text-red u-margin-right-20">{{goodsInfoName}}</text>
                                            <text class="u-font-sm coreshop-text-gray">￥{{goodsInfoPrint}}</text>
                                        </view>
                                    </view>
                                </view>
                            </view>
                            <!--规格数据-->
                            <view class="coreshop-select-btn-list-box">
                                <view class="select-item" v-for="(value,key) in goodsSpesDesc" :key="key">
                                    <text class='coreshop-text-black u-margin-10  coreshop-solid-bottom u-padding-bottom-20'>{{key}}</text>
                                    <view class='select-btn'>
                                        <button class="sku-btn u-margin-10"  :class='i.isDefault ? "selected" : "not-selected"' v-for="(i,itemIndex) in value" :key="itemIndex"  :data-key="i.productId" :data-id="i.name"  @click="selectSku">
                                            <u-avatar :src="i.image" size="50" class="u-margin-right-10 u-margin-top-10" style="margin-left: -20rpx;"></u-avatar>
                                            {{ i.name }}
                                        </button>
                                    </view>
                                </view>
                                <!-- 库存 -->
                                <view class="select-item">
                                    <view class="coreshop-text-black">数量</view>
                                    <view class="select-btn">
                                        <u-number-box v-model="goodsNums" :min="0"  :max="goodsInfoNumber" @change="valChange"></u-number-box>
                                    </view>
                                </view>
                            </view>
                        </view>
                        <!--公共按钮-->
                        <view class="u-padding-30 u-text-center">
                            <u-button type="error" size="medium" @click='goodsAddCart' v-if="status">确定</u-button>
                            <u-button type="default" size="medium" v-else>已售罄</u-button>
                        </view>
                    </view>
                </view>
            </block>
        </u-popup>


        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>
<script>
    import { goods, articles, commonUse, tools } from '@/common/mixins/mixinsHelper.js'
    import { mapMutations, mapActions, mapState } from 'vuex';
    export default {
        mixins: [goods, articles, commonUse, tools],
        data() {
            return {
                formBoxId: '',
                form: {
                    headType: 1,
                },
                showPage: true,
                slideImg: [], //幻灯片广告数据
                region: ['湖南省', '怀化市', '鹤城区'],
                areaId: 431202,
                pickerValue: '',
                show: false,
                pickerList: this.$db.get("areaList"),
                province: this.$db.get("areaList"),
                pickerIndex: [0, 0, 0], // picker索引值
                provinceKey: -1,//省份id
                cityKey: -1,//市id
                areaKey: -1,//区域id
                pics: [], //图片
                goodsNums: 0,
                cart: [],
                currentKey: 0, //当前下单的商品的Key
                currentGoodsId: 0, //当前选中的商品ID
                goodsTotalMoney: '0.00', //商品总额
                originForm: [], //原始表单
                paymentType: '', //支付类型
                paymentType: '', //表单付款码||表单订单
                /** 商品信息*/
                goodsSpesDesc: '',
                productId: '',
                status: '',
                goodsInfoName: '',
                goodsInfoImage: '',
                goodsInfoPrint: '',
                goodsInfoNumber: '',
                selectGoodsId: '',
                selectId: '',
                showSpecs: false,
                submitStatus: false, //按钮状态
                shareUrl: '/pages/share/jump/jump',
                bottomModal: false,
                buttonName: '',
            }
        },
        onLoad(options) {
            var id = options.id
            if (!id) {
                this.$u.toast('路径错误')
                return false
            }
            this.formBoxId = id
            this.$db.set('formBoxId', id)
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
            }
        },
        onShow() {
            this.getFormDetail()
        },
        methods: {
            goHome() {
                console.log("回主页");
                uni.switchTab({
                    url: '/pages/index/default/default'
                });
            },
            //倒查城市信息
            getFullPath(id, data) {
                for (var i = 0; i < data.length; i++) {
                    if (id == data[i].value) {
                        if (!data[i].children) {
                            this.areaKey = i;
                            return true;
                        } else if (data[i].hasOwnProperty("children")) {
                            if (data[i].children[0] && !data[i].children[0].children) {
                                this.cityKey = i;
                                return true;
                            } else {
                                this.provinceKey = i;
                                return true;
                            }
                        }
                    } else {
                        if (data[i].hasOwnProperty("children")) {
                            if (data[i].children[0] !== undefined) {
                                if (data[i].children[0].hasOwnProperty("children")) {
                                    this.provinceKey = i;
                                } else {
                                    this.cityKey = i;
                                }
                            }
                            if (typeof data[i].children != 'undefined') {
                                var res = this.getFullPath(id, data[i].children);
                                if (res) {
                                    return true;
                                }
                            }
                        }
                    }
                }
            },
            init() {
                console.log(this.areaId);
                this.getFullPath(this.areaId, this.province);
                this.pickerIndex = [this.provinceKey, this.cityKey, this.areaKey];
                console.log(this.pickerIndex);
            },
            // 省市区联动初始化
            showThreePicker() {
                this.pickerValue = this.region[0] + ' ' + this.region[1] + ' ' + this.region[2];
                this.init();
                this.show = true;
            },
            onConfirm(e) {
                console.log(e);
                var that = this
                let provinceName = e[0].label;
                let cityName = e[1].label;
                let countyName = e[2].label;
                this.pickerValue = e[0].label + " " + e[1].label + " " + e[2].label
                let data = {
                    provinceName: provinceName,
                    cityName: cityName,
                    countyName: countyName
                }
                let regionName = [provinceName, cityName, countyName];
                this.region = regionName;
                this.$u.api.getAreaId(data).then(res => {
                    if (res.status) {
                        that.areaId = res.data;
                        that.init();
                    } else {
                        uni.showModal({
                            title: '提示',
                            content: '地区选择出现问题，请重新选择地区',
                            showCancel: false
                        });
                    }
                });
                console.log(this.areaId);
            },
            getFormDetail() {
                var _this = this;
                var data = {
                    id: this.formBoxId,
                    token: this.$db.get('userToken')
                }
                var that = this
                this.$u.api.getFormDetial(data).then(res => {
                    if (res.status) {
                        this.form = res.data
                        this.originForm = res.data
                        this.buttonName = res.data.buttonName
                        if (res.data.images) {
                            this.slideImg = res.data.images.split(',');
                        }
                        if (res.data.type == '1' || res.data.type == '2') {
                            if (res.data.type == '1') {
                                //订单
                                that.paymentType = this.$globalConstVars.paymentType.formPay
                            } else if (res.data.type == '2') {
                                //付款码
                                that.paymentType = this.$globalConstVars.paymentType.formOrder
                            }
                        }

                    } else {
                        this.showPage = false;
                        if (res.data.isExpires) {
                            uni.showModal({
                                title: '提示',
                                content: '表单已过期，请扫描新的二维码',
                                showCancel: false,
                                success: function (res) {
                                    if (res.confirm) {
                                        _this.$u.route({ type: 'switchTab', url: '/pages/index/default/default' })
                                    }
                                }
                            })
                        } else if (res.data.needLogin) {
                            console.log("要登录权限了");
                            this.$u.route({ type: 'switchTab', url: '/pages/index/default/default' })
                            //if (this.$db.get('userToken')) {
                            //    this.hasLogin = true
                            //    this.$u.api.userInfo().then(res => {
                            //        if (res.status) {

                            //        }
                            //    })
                            //} else {
                            //    this.hasLogin = false
                            //}

                            if (!this.hasLogin) {
                                this.$store.commit('showLoginTip', true);
                                return false;
                            }
                        }
                    }
                })
            },
            // 选择日期
            bindDateChange(e, item) {
                item.defaultValue = e.target.value
            },
            // 选择时间
            bindTimeChange(e, item) {
                item.defaultValue = e.target.value
            },
            // 单选
            radioChange(e, item) {
                item.defaultValue = e.detail.value
            },
            // 多选
            checkboxChange(e, item) {
                var values = e.detail.value
                for (var i = 0; i < item.checkboxValue.length; ++i) {
                    const checkboxItem = item.checkboxValue[i]
                    if (values.includes(checkboxItem.value)) {
                        this.$set(checkboxItem, 'checked', true)
                    } else {
                        this.$set(checkboxItem, 'checked', false)
                    }
                }
            },
            /* 输入框事件 */
            valChange(e) {
                this.num = e.value;
            },
            //选择位置
            chooseLocation(e, item, index) {
                var pages = getCurrentPages()
                var items = pages[0].$vm.form.items;
                var that = this;
                uni.chooseLocation({
                    success(e) {
                        item.defaultValue = e.latitude + ',' + e.longitude
                        items[index] = item;
                        setTimeout(() => {
                            that.form.items = items;
                        }, 500)
                    },
                    fail(e) {
                        uni.getSetting({
                            success(res) {
                                if (!res.authSetting['scope.userLocation']) {
                                    uni.openSetting()
                                }
                            }
                        })
                    }
                })
            },
            picChoose(e, item, index) {
                var that = this
                //var pages = getCurrentPages()
                //if (pages.length > 1) {
                //    var items = pages[1].$vm.form.items;
                //} else {
                //    var items = pages[0].$vm.form.items;
                //}
                var items = this.form.items;
                if (!item.pics) {
                    item.pics = []
                }
                if (item.pics.length >= 3) {
                    that.$refs.uToast.show({ title: "最多允许上传3张图片", type: 'error' })
                    return false;
                }

                this.$upload.uploadImage(1, res => {
                    if (res.status) {
                        item.pics.push(res.data.src);
                        // #ifdef H5
                        that.$set(that.form.items, index, item)
                        // #endif
                        // #ifndef H5
                        items[index] = item;
                        setTimeout(() => {
                            that.form.items = items;
                        }, 500)
                        // #endif
                        that.$refs.uToast.show({ title: res.msg, type: 'success' })
                    } else {
                        that.$refs.uToast.show({ title: res.msg, type: 'error' })
                    }
                })

                console.log(item.pics);

            },
            //删除图片
            picDel(item, index, i) {
                item.pics.splice(i, 1)
                this.$set(this.form.items, index, item)
            },
            //表单提交
            formSubmit(e) {
                var that = this
                var data = e.detail.value
                var keys = Object.keys(data);
                var postData = [];
                keys.forEach(function (value, index) {
                    var name = value;
                    var id = name.replace('objName', '');
                    let obj = {
                        key: Number(id),
                        value: data[name].toString()
                    };
                    postData.push(obj);
                })
                //订单时需要合并购物车信息

                console.log(this.cart);


                if (this.form.type == 1) {
                    if (this.cart.length < 1) {
                        this.$u.toast('请先选择商品')
                        return true
                    }
                    var tempArray = []
                    this.cart.map((mapItem) => {
                        if (tempArray.length == 0) {
                            let pushData = { key: mapItem.key, value: [] }
                            pushData.value.push(mapItem);
                            tempArray.push(pushData)
                        } else {
                            //判断是否存在同名key的数据，进行累加
                            let res = tempArray.some((item) => {
                                if (item.key == mapItem.key) {
                                    item.value.push(mapItem)
                                    return true
                                }
                            })
                            if (!res) {
                                let pushData = { key: mapItem.key, value: [] }
                                pushData.value.push(mapItem);
                                tempArray.push(pushData)
                            }
                        }
                    })
                    tempArray.forEach(function (item, index) {
                        item.value = JSON.stringify(item.value);
                        postData.push(item);
                    })
                }
                console.log(data);
                console.log(postData);

                let userToken = this.$db.get('userToken')
                let obj = {
                    data: postData,
                    id: this.form.id,
                    token: userToken
                }
                this.submitStatus = true;
                this.$u.api.addSubmitForm(obj).then(res => {
                    this.submitStatus = false;
                    if (res.status) {
                        //表单类型判断是否需要支付，支付金额多少
                        if (that.form.type == '1' || that.form.type == '2') {
                            that.$refs.uToast.show({ title: res.msg, type: 'success' });
                            //跳转首页
                            setTimeout(function () {
                                //出来支付按钮
                                that.$u.route({
                                    type: 'redirectTo', url: '/pages/payment/pay/pay?formId=' + res.data.formSubmitId + '&type=' + that.paymentType +
                                        '&recharge=' + res.data.money
                                });
                            }, 1000)
                        } else {
                            that.formReset()
                            that.$refs.uToast.show({ title: res.msg, type: 'success' })
                            //跳转首页
                            setTimeout(function () {
                                wx.switchTab({
                                    url: '/pages/index/default/default'
                                })
                            }, 1500)
                        }
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            },
            //表单清空
            formReset(e) {
                this.$db.set('formBoxId', '')
                this.cart = [] //初始化，刷新当前页面
                this.form = this.originForm
            },
            closeModal() {
                this.bottomModal = false;
                this.showSpecs = false
            },
            //选择规格弹出
            specifications(e, item) {
                this.bottomModal = true;
                this.showSpecs = true
                this.selectId = e.target.dataset.id
                this.selectGoodsId = e.target.dataset.goods
                this.currentKey = e.target.dataset.id //当前选中的key
                this.currentGoodsId = e.target.dataset.goods //当前选中的商品ID
                this.getGoodsInfo(item)
            },
            //获取商品详情
            getGoodsInfo(item) {
                let goods = item.good
                this.goodsSpesDesc = this.$u.deepClone(goods.product.defaultSpecificationDescription);
                this.productId = goods.product.id
                this.goodsInfoName = goods.product.name
                this.goodsInfoImage = goods.product.images
                this.goodsInfoPrint = goods.product.price
                this.goodsInfoNumber = goods.product.stock
                this.goodsNums = this.getNumsByKey(this.currentKey, goods.product.id)
                this.status = goods.product.stock < 1 ? false : true
            },
            /*获取key的数量 */
            getNumsByKey(key, productId) {
                var that = this
                if (that.cart.length < 1) {
                    return 0
                } else {
                    for (var i = 0; i < that.cart.length; i++) {
                        if (that.cart[i].key == key && that.cart[i].productId == productId) {
                            return that.cart[i].nums
                        }
                    }
                    return 0
                }
            },
            //加入购物车
            goodsAddCart: function () {
                var productId = this.productId
                var currentKey = this.currentKey
                if (this.cart.length < 1) {
                    this.cart.push({
                        key: currentKey,
                        productId: productId,
                        goodsId: this.selectGoodsId,
                        nums: this.goodsNums,
                        price: this.goodsInfoPrint
                    })
                } else {
                    var isIn = false
                    for (var i = 0; i < this.cart.length; i++) {
                        if (this.cart[i].key == currentKey && this.cart[i].productId == productId) {
                            this.cart[i] = {
                                key: currentKey,
                                productId: productId,
                                goodsId: this.selectGoodsId,
                                nums: this.goodsNums,
                                price: this.goodsInfoPrint
                            }
                            isIn = true
                        }
                    }
                    if (!isIn) {
                        this.cart.push({
                            key: currentKey,
                            productId: productId,
                            goodsId: this.selectGoodsId,
                            nums: this.goodsNums,
                            price: this.goodsInfoPrint
                        })
                    }
                }

                this.closeModal();
                this.getCartNums()
            },
            getCartNums() {
                var items = this.form.items
                var itemKey = ''
                for (var i = 0, len = items.length; i < len; ++i) {
                    if (items[i].id == this.currentKey) {
                        itemKey = i
                    }
                }
                var that = this
                if (this.form.items[itemKey].good.id == this.currentGoodsId) {
                    if (this.form.items[itemKey].cartCount > 0) {
                        var cartCount = 0
                        var currentKey = this.currentKey
                        this.cart.forEach(function (item, index, input) {
                            if (item.key == currentKey) {
                                cartCount += item.nums
                            }
                            that.form.items[itemKey].cartCount = cartCount
                        })
                    } else {
                        this.form.items[itemKey].cartCount = this.goodsNums
                    }
                } else {
                    this.form.items[itemKey].cartCount = this.goodsNums
                }
                this.getGoodsTotalMoney()
            },
            //获取商品总额
            getGoodsTotalMoney() {
                var that = this
                var goodsTotalMoney = 0
                this.cart.forEach(function (item, index, input) {
                    goodsTotalMoney += item.price * item.nums
                })
                this.goodsTotalMoney = this.$common.formatMoney(goodsTotalMoney, 2, '')
            },
            getSpes: function (product) {
                if (!product.defaultSpecificationDescription) {
                    return []
                }
                return product.defaultSpecificationDescription
            },
            //获取规格信息
            selectSku(e) {
                var id = e.target.dataset.key
                this.$u.api.getProductInfo({
                    id
                }).then(res => {
                    if (res.status) {
                        this.goodsSpesDesc = this.$u.deepClone(res.data.defaultSpecificationDescription);
                        this.productId = res.data.id
                        this.goodsInfoName = res.data.name
                        this.goodsInfoImage = res.data.images
                        this.goodsInfoPrint = res.data.price
                        this.goodsInfoNumber = res.data.stock
                        this.goodsNums = this.getNumsByKey(this.currentKey, res.data.id)
                        this.status = res.data.stock < 1 ? false : true
                    }
                })
            },
            //获取分享URL
            getShareUrl() {
                let data = {
                    client: 2,
                    url: "/pages/share/jump/jump",
                    type: 1,
                    page: 8,
                    params: {
                        id: this.formBoxId
                    }
                };
                let userToken = this.$db.get('userToken');
                if (userToken && userToken != '') {
                    data['token'] = userToken;
                }
                this.$u.api.share(data).then(res => {
                    this.shareUrl = res.data
                });
            }
        },
        watch: {
            formBoxId: {
                handler() {
                    this.getShareUrl();
                },
                deep: true
            }
        },
        //分享
        onShareAppMessage(res) {
            return {
                title: this.form.name,
                path: this.shareUrl
            }
        },
        onShareTimeline(res) {
            return {
                title: this.form.name,
                path: this.shareUrl
            }
        },
    }
</script>
<style lang="scss" scoped>
    @import 'details.scss';
</style>
