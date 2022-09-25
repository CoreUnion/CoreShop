<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="提交售后"></u-navbar>
        <view class="content">
            <view class="coreshop-solid-bottom u-font-lg u-padding-30">
                <uni-text class="coreshop-text-black">
                    <span>请选择退货商品和数量</span>
                </uni-text>
            </view>

            <form @submit="submit">
                <view class="coreshop-content-top">
                    <view class="img-list">
                        <checkbox-group class="cart-checkbox" v-for="(item, key) in items" :key="key" @change="checkboxChange">
                            <view class="cart-checkbox-item">
                                <label class="uni-list-cell uni-list-cell-pd">
                                    <view class="cart-checkbox-c">
                                        <checkbox :value='item.id' :checked="item.checked" color="#FF7159" v-if="item.disabled" :disabled="item.disabled" class="checkboxNo" />
                                        <checkbox :value='item.id' :checked="item.checked" color="#FF7159" v-else />
                                    </view>
                                    <view class="img-list-item">
                                        <u-avatar :src="item.imageUrl" mode="square" size="150" class="img-list-item-l"></u-avatar>
                                        <view class="img-list-item-r">
                                            <view class="list-goods-name">
                                                {{item.name}}
                                            </view>
                                            <view class="goods-item-c u-margin-top-10 u-margin-bottom-10" v-if="item.addon">
                                                <view class="goods-buy">
                                                    <!-- 商品规格 -->
                                                    <view class="goods-salesvolume u-margin-right-10 ">
                                                        {{item.addon}}
                                                    </view>
                                                </view>
                                            </view>
                                            <view class="goods-item-c">
                                                <view class="goods-buy">
                                                    <!-- 商品规格 -->
                                                    <view class="goods-salesvolume u-margin-right-10">
                                                        购买数：{{item.nums}}
                                                    </view>
                                                    <view class="goods-salesvolume u-margin-right-10">
                                                        发货数量：{{item.sendNums}}
                                                    </view>
                                                    <view class="goods-salesvolume u-margin-right-10" v-show="item.reshipNums!=0">
                                                        已退数量：{{item.reshipNums}}
                                                    </view>
                                                    <view class="goods-salesvolume u-margin-top-10 u-margin-bottom-10" v-if="!item.disabled">
                                                        <label>您要退货数量：</label>
                                                        <input type="number" v-model="item.returnNums" @focus="onFocus(item,key)" @blur="updateNum(item,key)" class="inputStyle" ref="input" @click.stop />
                                                    </view>
                                                </view>
                                            </view>
                                        </view>
                                    </view>
                                </label>
                            </view>
                        </checkbox-group>
                    </view>
                    <view class='coreshop-cell-group u-margin-top-20 u-margin-bottom-20'>
                        <view class='coreshop-cell-item'>
                            <view class='coreshop-cell-item-hd'>
                                <view class='coreshop-cell-hd-title'>是否发货</view>
                            </view>
                            <view class='coreshop-cell-item-ft'>
                                <view class="uni-form-item uni-column invoice-type">
                                    <radio-group class="uni-list" @change="radioChange">
                                        <label class="uni-list-cell uni-list-cell-pd" v-for="(item, index) in typeList" :key="index">
                                            <view class="invoice-type-icon">
                                                <radio class="a-radio radioNo" v-if="item.disabled" :id="item.name" :value="item.value" :checked="item.checked"
                                                       :disabled="item.disabled"></radio>
                                                <radio class="a-radio " v-else :id="item.name" :value="item.value" :checked="item.checked" :disabled="item.disabled"></radio>
                                            </view>
                                            <view class="invoice-type-c">
                                                <label :for="item.name">
                                                    <text>{{item.name}}</text>
                                                </label>
                                            </view>
                                        </label>
                                    </radio-group>
                                </view>
                            </view>
                        </view>
                        <view class='coreshop-cell-item refund-price'>
                            <view class='coreshop-cell-item-hd'>
                                <view class='coreshop-cell-hd-title'>退款金额</view>
                            </view>
                            <view class='coreshop-cell-item-ft'>
                                <input type="digit" class='coreshop-cell-bd-input red-price' v-model="refund" @focus="refundFocus" ref="refund"></input>
                            </view>

                        </view>
                        <view class=" u-font-24 refund-tip">
                            可修改，最多￥{{maxRefund}}，含发货邮费￥{{costFreight}}
                        </view>
                    </view>
                    <view class='coreshop-cell-group u-margin-top-20 u-margin-bottom-20'>
                        <view class='coreshop-cell-item right-img'>
                            <view class='coreshop-cell-item-hd'>
                                <view class='coreshop-cell-hd-title'>上传凭证</view>
                            </view>
                        </view>
                        <view class="">
                            <view class="evaluate-c-b">
                                <view class="goods-img-item" v-for="(item, key) in images" :key="key">
                                    <image class="del" src="/static/images/common/del.png" mode="" @click="delImage(item)"></image>
                                    <image class="" :src="item" mode="" @click="clickImg(item)"></image>
                                </view>
                                <view class="upload-img" v-show="isImage" @click="upImage">
                                    <image class="icon" src="/static/images/common/camera.png" mode=""></image>
                                    <view class="">上传照片</view>
                                </view>
                            </view>
                        </view>
                    </view>
                    <view class='coreshop-cell-group u-margin-top-20 u-margin-bottom-20'>
                        <view class='coreshop-cell-item right-img'>
                            <view class='coreshop-cell-item-hd'>
                                <view class='coreshop-cell-hd-title'>问题描述</view>
                            </view>
                        </view>
                        <view class="cell-textarea">
                            <input v-model="reason" placeholder="请您在此描述问题(最多200字)" maxlength="200" />
                        </view>
                    </view>
                    <view class="coreshop-bottomBox">
                        <button class="coreshop-btn coreshop-btn-b coreshop-btn-square" formType="submit" :disabled='submitStatus' :loading='submitStatus'>提交</button>
                    </view>
                </view>
            </form>
        </view>
    </view>

</template>

<script>
    export default {
        data() {
            return {
                typeList: [
                    { value: '1', name: '未发货', checked: true, disabled: false },
                    { value: '2', name: '已发货', checked: false, disabled: false },
                ],
                typeListByAli: [
                    { value: '1', name: '仅退款', checked: true, disabled: false },
                    { value: '2', name: '退货退款', checked: false, disabled: false },
                ],
                orderId: '',
                items: [],   //退货明细
                itemIds: [],  //选择的退货
                aftersaleType: 1,     //售后类型1退款，2退款退货
                refund: 0,   //退款金额，等于已支付的金额减去已退款的金额
                maxRefund: 0,//最大可退款金额
                refundShow: 0,
                images: [],      //图片
                reason: '',      //原因
                imageMax: 5,    //用于前台判断上传图片按钮是否显示
                refundInputNoedit: true,
                mode: 'aspectFill',
                submitStatus: false,
                checkedItems: [],//当前选中的商品
                isFlag: true,
                costFreight: 0//运费
            }
        },
        computed: {
            isImage() {
                let num = this.imageMax - this.images.length;
                if (num > 0) {
                    return true;
                } else {
                    return false;
                }
            },
        },
        methods: {
            // 单选框点击切换
            radioChange: function (evt) {
                this.typeList.forEach(item => {
                    if (item.value === evt.target.value) {
                        item.checked = true;
                        this.aftersaleType = parseInt(evt.target.value);
                    } else {
                        item.checked = false;
                    }
                });
                if (this.typeList[0].checked) {
                    this.refundInputNoedit = true;
                } else {
                    this.refundInputNoedit = false;
                }
            },
            //订单商品信息
            getOrderInfo() {
                let data = {
                    id: this.orderId
                }
                this.$u.api.orderDetail(data).then(res => {
                    if (res.status) {
                        //如果不是未支付的，已取消的，已完成的状态，就都可以售后
                        if (res.data.globalStatus != 1 && res.data.globalStatus != 6 && res.data.globalStatus != 7) {
                            //判断是已付款未发货，如果是，就禁用退货
                            let typeList = this.typeList;
                            if (res.data.globalStatus == 2) {
                                typeList[1].disabled = true;
                            }

                            //设置已选中的商品
                            let nums = 0;
                            let returnNums = {}
                            let returnStatus
                            for (var i = 0; i < res.data.items.length; i++) {
                                if (res.data.items[i].nums >= res.data.items[i].reshipNums) {
                                    returnNums = res.data.items[i].nums - res.data.items[i].reshipNums;
                                }
                                if (returnNums > 0) {
                                    returnStatus = true
                                }
                                res.data.items[i].id = res.data.items[i].id.toString();
                                //this.itemIds = this.itemIds.concat({ id: res.data.items[i].id, nums: returnNums });
                                res.data.items[i].returnNums = returnNums
                                res.data.items[i].returnStatus = returnStatus
                                res.data.items[i].checked = false;
                                if (res.data.items[i].returnNums > 0) {
                                    res.data.items[i].disabled = false;
                                } else {
                                    res.data.items[i].disabled = true;
                                }
                            }
                            this.items = res.data.items;

                            console.log(this.items);

                            this.refund = this.$common.formatMoney((res.data.orderAmount - res.data.refunded), 2, '');
                            this.maxRefund = this.$common.formatMoney((res.data.orderAmount - res.data.refunded), 2, '');
                            this.costFreight = res.data.costFreight;//运费
                            this.refundShow = res.data.payedAmount - res.data.refunded;
                            this.typeList = typeList;
                        } else {
                            this.$common.errorToBack('订单不可以进行售后');
                        }
                    } else {
                        this.$common.errorToBack('没有找到此订单');
                    }
                });
            },

            //退货商品选择
            checkboxChange(e) {
                let nums = 0;
                let id = 0;
                this.itemIds = [];
                for (var i = 0; i < e.detail.value.length; i++) {
                    let k = e.detail.value[i];
                    for (var j = 0; j < this.items.length; j++) {
                        if (this.items[j].id == k) {
                            if (this.items[j].nums > this.items[j].reshipNums) {
                                // nums = this.items[j].sendnums - this.items[j].reshipNums;
                                //nums = this.$refs.input[i].value
                                nums = this.items[j].returnNums;
                                id = parseInt(k);
                                this.itemIds = this.itemIds.concat({ id: id, nums: nums });
                                console.log(this.itemIds)
                            }
                        }
                    }
                }

            },

            // 点击输入框的事件
            onFocus(item, key) {
                item.returnNums = '';
                if (this.checkedItems.indexOf(item.id) == -1) {
                    this.checkedItems.push(item.id)
                }
                this.items[key].checked = true;
                this.getReturnData();
            },
            //处理退款金额光标事件
            refundFocus(e) {
                this.refund = '';
            },

            //数量改变事件
            updateNum(updateNum, key) {
                let nums = 0;
                nums = this.items[key].nums - this.items[key].reshipNums;
                if (nums < updateNum.returnNums) {
                    this.isFlag = false;
                    this.items[key].returnNums = nums;
                    this.$u.toast("您填写的数量不对！")
                    //return false;
                }
                if (updateNum.returnNums == '') {
                    this.items[key].returnNums = nums;
                }
                this.isFlag = true;
                this.items[key].returnNums = updateNum.returnNums;
                this.getReturnData();
            },

            //计算要退货的商品数量
            getReturnData() {
                let nums = 0;
                this.itemIds = [];
                for (var i = 0; i < this.checkedItems.length; i++) {
                    let k = this.checkedItems[i];
                    for (var j = 0; j < this.items.length; j++) {
                        if (this.items[j].id == k) {
                            if (this.items[j].nums >= this.items[j].reshipNums) {

                                nums = this.items[j].nums - this.items[j].reshipNums;
                                if (nums >= this.items[j].returnNums) {
                                    nums = this.items[j].returnNums
                                    this.itemIds = this.itemIds.concat({ id: k, nums: nums });
                                } else {
                                    this.$u.toast("您填写的数量不对！")
                                    return;
                                }
                            }
                        }
                    }
                }
            },

            //提交
            submit(e) {
                let _that = this;
                this.submitStatus = true;

                //判断退款金额
                let reg = /^[0-9]+(.[0-9]{1,2})?$/;
                if (!reg.test(this.refund)) {
                    this.$u.toast('请输入正确金额');
                    this.submitStatus = false;
                    return false;
                } else {
                    if (this.refund > this.refundShow) {
                        this.$u.toast('退款金额过大');
                        this.submitStatus = false;
                        return false;
                    }
                }
                if (!this.isFlag) {
                    this.$u.toast('您填写的数量不对！');
                    this.submitStatus = false;
                    return false;
                }
                console.log(this.itemIds)
                if (this.itemIds.length <= 0) {
                    this.$u.toast('请选择要售后的商品');
                    this.submitStatus = false;
                    return false;
                }
                //组装数据，提交数据
                let data = {
                    orderId: _that.orderId,
                    type: _that.aftersaleType,
                    items: _that.itemIds,
                    images: _that.images,
                    refund: _that.refund,
                    reason: _that.reason
                };
                _that.$u.api.addAfterSales(data).then(res => {
                    _that.submitStatus = false;
                    if (res.status) {
                        _that.$refs.uToast.show({
                            title: '提交成功', type: 'success', callback: function () {
                                _that.$u.route("/pages/member/order/detail/detail?orderId=" + _that.orderId)
                            }
                        })
                    } else {
                        _that.$u.toast(res.msg);
                    }
                });
            },

            //上传图片
            upImage() {
                let num = this.imageMax - this.images.length;
                if (num > 0) {
                    this.$upload.uploadImage(num, res => {
                        if (res.status) {
                            this.images.push(res.data.src);
                            this.$refs.uToast.show({ title: res.msg, type: 'success', back: false });
                        } else {
                            this.$u.toast(res.msg);
                        }
                    });
                }
            },
            //删除图片
            delImage(e) {
                let newImages = [];
                for (var i = 0; i < this.images.length; i++) {
                    if (this.images[i].image_id != e.image_id) {
                        newImages.push(this.images[i]);
                    }
                }
                this.images = newImages;
            },
            // 图片点击放大
            clickImg(img) {
                // 预览图片
                uni.previewImage({
                    urls: img.split()
                });
            }
        },
        onLoad(e) {
            this.orderId = e.orderId;
            this.getOrderInfo();
            this.getReturnData()
        }
    }
</script>

<style lang="scss" scoped>
    @import "submit.scss";
</style>
