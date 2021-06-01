<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="添加银行卡"></u-navbar>
        <form>
            <view class="cu-form-group">
                <view class="title">银行卡号</view>
                <input type="number" v-model="cardNumber" focus @blur="checkCard()" placeholder='请输入银行卡号'></input>
            </view>
            <view class="cu-form-group">
                <view class="title">持卡人</view>
                <input type="text" v-model="name" placeholder='请输入持卡人姓名'></input>

            </view>
            <view class="cu-form-group">
                <view class="title">银行名称</view>
                <input type="text" :disabled="true" v-model="bankName"></input>
            </view>
            <view class="cu-form-group">
                <view class="title">银行卡类型</view>
                <input type="text" :disabled="true" v-model='cardTypeName'></input>
            </view>
            <view class="cu-form-group">
                <view class="title">开户行名</view>
                <input type="text" v-model="accountBank" placeholder='请输入开户银行名'></input>
            </view>
            <view class="cu-form-group">
                <view class="title">开户行地址</view>
                <input :value="pickerValue" @focus="showThreePicker"></input>
                <u-select v-model="show" mode="mutil-column-auto" :list="pickerList" :default-value="pickerIndex" @confirm="onConfirm"></u-select>
                <text class='cuIcon-locationfill text-orange' @click="showThreePicker"></text>

            </view>
            <view class="cu-form-group" @click="defaultChange">
                <view class="title">设为默认</view>
                <radio value="0" :checked="checked" color="#333" />
            </view>

        </form>

        <view class="bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
            <view class="flex padding-sm flex-direction">
                <button class="cu-btn bg-red" @click="addCard" hover-class="btn-hover2" :disabled='submitStatus' :loading='submitStatus'>保存</button>
            </view>
        </view>
    </view>

</template>

<script>
    export default {
        data() {
            return {
                bankName: '',	// 银行名称
                cardType: 1, // 卡类型
                cardTypeName: '', // 卡片类型
                bankCode: '', // 银行缩写码
                accountBank: '', // 开户行
                cardNumber: '', // 银行卡号
                name: '',	// 开户人姓名
                mobile: '',	//
                region: ['湖南省', '怀化市', '鹤城区'],
                areaId: 431202,
                address: '',
                isDefault: 0,
                checked: false,
                pickerValue: '',
                submitStatus: false,
                show: false,
                pickerList: this.$db.get("areaList"),
                province: this.$db.get("areaList"),
                pickerIndex: [0, 0, 0], // picker索引值
                provinceKey: -1,//省份id
                cityKey: -1,//市id
                areaKey: -1,//区域id
            }
        },
        computed: {},
        methods: {
            // 省市区联动初始化
            showThreePicker() {
                this.show = true;
            },
            // 选择收货地址
            onConfirm(e) {
                console.log(e);
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
                this.$u.api.getAreaId(data).then(res => {
                    if (res.status) {
                        this.areaId = res.data;
                        this.init();
                    } else {
                        uni.showModal({
                            title: '提示',
                            content: '地区选择出现问题，请重新选择地区',
                            showCancel: false
                        });
                    }
                });
            },
            // 选择/取消默认
            defaultChange() {
                this.checked = !this.checked
                this.isDefault = this.isDefault === 0 ? 1 : 0
            },
            //存储收货地址
            saveShip() {
                if (this.id && this.id != 0) {
                    //编辑存储
                    let data = {
                        'id': this.id,
                        'name': this.name,
                        'address': this.address,
                        'mobile': this.mobile,
                        'isDefault': this.isDefault
                    }

                    data['areaId'] = this.areaId,
                        this.$u.api.editShip(data).then(res => {
                            if (res.status) {
                                this.$refs.uToast.show({ title: '编辑成功', type: 'success', back: true })
                            } else {
                                this.$u.toast(res.msg);
                            }
                        });
                } else {
                    //添加
                    let data = {
                        'areaId': this.areaId,
                        'name': this.name,
                        'address': this.address,
                        'mobile': this.mobile,
                        'isDefault': this.isDefault
                    }
                    this.$u.api.saveUserShip(data).then(res => {
                        if (res.status) {
                            this.$refs.uToast.show({ title: '添加成功', type: 'success', back: true })
                        } else {
                            this.$u.toast(res.msg);
                        }
                    });
                }
            },
            // 判断获取银行卡类型
            checkCard() {
                let _this = this;
                if (this.cardNumber) {
                    let data = {
                        id: this.cardNumber
                    }
                    this.$u.api.getBankCardOrganization(data).then(res => {
                        if (res.status) {
                            let data = res.data
                            _this.bankName = data.name
                            _this.cardType = data.type
                            _this.bankCode = data.bankCode
                            _this.cardTypeName = data.typeName
                        } else {
                            _this.$refs.uToast.show({
                                title: res.msg, type: 'error', callback: function () {
                                    _this.bankCode = _this.bankName = _this.cardType = _this.cardTypeName = ''
                                }
                            })

                        }
                    })
                } else {
                    this.bankCode = this.bankName = this.cardType = this.cardTypeName = ''
                }
            },
            // 添加银行卡
            addCard() {
                if (!this.cardNumber) {
                    this.$u.toast('请输入银行卡号')
                } else if (!this.bankName || !this.cardType || !this.bankCode) {
                    this.$u.toast('请输入正确的银行卡号')
                } else if (!/^[\u4E00-\u9FA5\uf900-\ufa2d·s]{2,30}$/.test(this.name)) {
                    this.$u.toast('请输入正确的持卡人名称')
                } else if (!this.areaId) {
                    this.$u.toast('请选择开户行所在地区')
                } else if (!this.accountBank) {
                    this.$u.toast('请输入开户银行信息')
                } else {
                    this.submitStatus = true;
                    let data = {
                        bankName: this.bankName,
                        bankCode: this.bankCode,
                        bankAreaId: this.areaId,
                        accountBank: this.accountBank,
                        accountName: this.name,
                        cardNumber: this.cardNumber,
                        cardType: this.cardType,
                        isDefault: this.isDefault
                    }

                    this.$u.api.addBankCard(data).then(res => {
                        this.submitStatus = false;
                        if (res.status) {
                            this.$refs.uToast.show({ title: res.msg, type: 'success', back: true })
                        } else {
                            this.$u.toast(res.msg);
                        }
                    });
                }
            },
            // #ifdef MP-ALIPAY
            // alipay bank
            aliPayBank() {
                let _this = this;
                if (this.cardNumber.length >= 16 && this.cardNumber.length <= 19) {
                    let data = {
                        card_code: this.cardNumber
                    }
                    this.$u.api.getBankCardOrganization(data).then(res => {
                        if (res.status) {
                            let data = res.data
                            this.bankName = data.name
                            this.cardType = data.type
                            this.bankCode = data.bank_code
                            this.cardTypeName = data.type_name
                        } else {
                            _this.$refs.uToast.show({
                                title: res.msg, type: 'error', callback: function () {
                                    _this.bankCode = _this.bankName = _this.cardType = _this.cardTypeName = ''
                                }
                            })
                        }
                    })
                } else {
                    this.bankCode = this.bankName = this.cardType = this.cardTypeName = ''
                }
            },
            // #endif
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
                this.getFullPath(this.areaId, this.province);
                this.pickerIndex = [this.provinceKey, this.cityKey, this.areaKey];
                console.log(this.pickerIndex);
            },
        },
        onLoad(e) {
            if (e.ship_id) {
                //编辑
                this.id = e.ship_id;
                this.getShipInfo();
            } else {
                //添加
                this.pickerValue = this.region[0] + " " + this.region[1] + " " + this.region[2];
                this.init();
            }
        },
        // #ifdef MP-ALIPAY
        watch: {
            cardNumber() {
                this.$u.throttle(this.aliPayBank, 500)
            }
        },
        // #endif
    }
</script>

<style lang="scss" scoped>
    .cu-form-group .title { min-width: calc(5em + 15px); }
    /* #ifdef MP-ALIPAY */
    input { font-size: 24upx; }
    /* #endif */
</style>