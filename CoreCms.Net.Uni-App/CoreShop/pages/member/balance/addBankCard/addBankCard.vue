<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="添加银行卡"></u-navbar>

        <view class="u-padding-20">
            <u-form :model="form" :rules="rules" ref="uForm" :errorType="errorType">
                <u-form-item label="银行卡号" label-width="180" prop="cardNumber">
                    <u-input type="text" v-model="form.cardNumber" placeholder="请输入银行卡号" @blur="checkCard()"></u-input>
                </u-form-item>
                <u-form-item label="持卡人姓名" label-width="180" prop="name">
                    <u-input type="text" v-model="form.name" placeholder="请输入持卡人姓名"></u-input>
                </u-form-item>
                <u-form-item label="银行名称" label-width="180">
                    <u-input type="text" :disabled="true" v-model="form.bankName"></u-input>
                </u-form-item>
                <u-form-item label="银行卡类型" label-width="180">
                    <u-input type="text" :disabled="true" v-model="form.cardTypeName"></u-input>
                </u-form-item>
                <u-form-item label="开户行名" label-width="180">
                    <u-input type="text" v-model="form.accountBank" placeholder="请输入开户银行名"></u-input>
                </u-form-item>
                <u-form-item label="开户行地址" label-width="180">
                    <u-input :value="pickerValue" @click="showThreePicker" type="select" placeholder="请选择开户行区域"></u-input>
                    <u-select v-model="show" mode="mutil-column-auto" :list="pickerList" :default-value="pickerIndex" @confirm="onConfirm"></u-select>
                </u-form-item>
                <u-form-item label="设为默认" label-width="180">
                    <u-checkbox v-model="checked" @change="defaultChange">勾选将设为默认提现账户</u-checkbox>
                </u-form-item>
            </u-form>
        </view>
        <!--按钮-->
        <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
            <view class="flex u-padding-20 flex-direction">
                <u-button :custom-style="customStyle" type="error" size="medium" @click="addCard" :disabled='submitStatus' :loading='submitStatus'>保存</u-button>
            </view>
        </view>

    </view>

</template>

<script>
    export default {
        data() {
            return {
                customStyle: {
                    width: '100%',
                },
                form: {
                    cardNumber: '', // 银行卡号
                    name: '',	// 开户人姓名
                    bankName: '',	// 银行名称
                    bankCode: '', // 银行缩写码
                    accountBank: '', // 开户行
                    cardType: 1, // 卡类型
                    cardTypeName: '', // 卡片类型
                    isDefault: 0,
                },
                errorType: ['message'],
                region: ['湖南省', '怀化市', '鹤城区'],
                areaId: 0,
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
                rules: {
                    cardNumber: [
                        {
                            required: true,
                            message: '请输入密码',
                            trigger: ['change', 'blur'],
                        },
                        {
                            type: 'number',
                            message: '请输入数字银行卡号',
                            trigger: ['change', 'blur'],
                        }
                    ],
                    name: [
                        {
                            required: true,
                            message: '请输入姓名',
                            trigger: 'blur',
                        },
                        {
                            min: 2,
                            max: 4,
                            message: '姓名长度在2到4个字符',
                            trigger: ['change', 'blur'],
                        },
                        {
                            validator: (rule, value, callback) => {
                                return this.$u.test.chinese(value);
                            },
                            message: '必须为中文',
                            trigger: ['change', 'blur'],
                        }
                    ],
                },
            }
        },
        computed: {},
        onReady() {
            this.$refs.uForm.setRules(this.rules);
        },
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
                //let regionName = [provinceName, cityName, countyName];
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
            defaultChange(e) {
                this.form.isDefault = e.value ? 1 : 0;
                console.log(this.form.isDefault);
            },
            // 判断获取银行卡类型
            checkCard() {
                let _this = this;
                if (this.form.cardNumber) {
                    let data = {
                        id: this.form.cardNumber
                    }
                    this.$u.api.getBankCardOrganization(data).then(res => {
                        if (res.status) {
                            let data = res.data
                            _this.form.bankName = data.name
                            _this.form.cardType = data.type
                            _this.form.bankCode = data.bankCode
                            _this.form.cardTypeName = data.typeName
                        } else {
                            _this.$refs.uToast.show({
                                title: res.msg, type: 'error', callback: function () {
                                    _this.form.bankCode = _this.form.bankName = _this.form.cardType = _this.form.cardTypeName = ''
                                }
                            })

                        }
                    })
                } else {
                    this.form.bankCode = this.form.bankName = this.form.cardType = this.form.cardTypeName = ''
                }
            },
            // 添加银行卡
            addCard() {

                this.$refs.uForm.validate(valid => {
                    if (valid) {
                        console.log('验证通过');

                        if (!this.form.cardNumber) {
                            this.$u.toast('请输入银行卡号')
                        } else if (!this.form.bankName || !this.form.cardType || !this.form.bankCode) {
                            this.$u.toast('请输入正确的银行卡号')
                        } else if (!/^[\u4E00-\u9FA5\uf900-\ufa2d·s]{2,30}$/.test(this.form.name)) {
                            this.$u.toast('请输入正确的持卡人名称')
                        } else if (this.areaId <= 0) {
                            this.$u.toast('请选择开户行所在地区')
                        } else if (!this.form.accountBank) {
                            this.$u.toast('请输入开户银行信息')
                        } else {
                            this.submitStatus = true;
                            let data = {
                                bankName: this.form.bankName,
                                bankCode: this.form.bankCode,
                                bankAreaId: this.areaId,
                                accountBank: this.form.accountBank,
                                accountName: this.form.name,
                                cardNumber: this.form.cardNumber,
                                cardType: this.form.cardType,
                                isDefault: this.form.isDefault
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


                    } else {
                        console.log('验证失败');
                    }
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
            //增加默认省市区选中
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
                //添加（取消初始化）
                //this.pickerValue = this.region[0] + " " + this.region[1] + " " + this.region[2];
                //this.init();
            }
        },
    }
</script>

<style lang="scss">
    page { background: #fff; }
</style>