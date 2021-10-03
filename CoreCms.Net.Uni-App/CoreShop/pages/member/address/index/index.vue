<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="编辑地址"></u-navbar>

        <view class="u-padding-20">
            <u-form :model="form" :rules="rules" ref="uForm" :errorType="errorType">
                <u-form-item label="收货人" label-width="180" prop="name">
                    <u-input v-model="form.name" placeholder="请填写收货人姓名" />
                </u-form-item>
                <u-form-item label="手机号" label-width="180" prop="mobile">
                    <u-input v-model="form.mobile" placeholder="请填写收货人手机号" />
                </u-form-item>

                <u-form-item label="省市区" label-width="180">
                    <u-input :value="pickerValue" @click="showThreePicker" type="select" placeholder="请选择省市区区域"></u-input>
                    <u-select v-model="show" mode="mutil-column-auto" :list="pickerList" :default-value="pickerIndex" @confirm="onConfirm"></u-select>
                </u-form-item>

                <u-form-item label="详细地址" label-width="180" prop="address">
                    <u-input v-model="form.address" placeholder="请填写收货详细地址" type="textarea" />
                </u-form-item>

                <u-form-item label="设为默认" label-width="180">
                    <u-switch slot="right" v-model="checked"></u-switch>
                </u-form-item>
            </u-form>
        </view>

        <view class="coreshop-bottomBox">
            <button class="coreshop-btn coreshop-btn-square coreshop-btn-w" @click="delShip" v-if="id && id != 0" :disabled='submitStatus' :loading='submitStatus'>删除</button>
            <button class="coreshop-btn coreshop-btn-square coreshop-btn-b" @click="saveShip" :disabled='submitStatus' :loading='submitStatus'>保存</button>
        </view>
    </view>
</template>

<script>
    export default {
        data() {
            return {
                errorType: ['message'],
                id: 0,
                form: {
                    name: '',
                    mobile: '',
                    address: '',
                    isDefault: 2,
                },
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
                    address: [
                        {
                            required: true,
                            message: '请输入地址',
                            trigger: 'blur',
                        },
                        {
                            min: 5,
                            max: 30,
                            message: '地址长度在5到30个字符',
                            trigger: ['change', 'blur'],
                        }
                    ],
                    mobile: [
                        {
                            required: true,
                            message: '请输入手机号',
                            trigger: ['change', 'blur'],
                        },
                        {
                            validator: (rule, value, callback) => {
                                return this.$u.test.mobile(value);
                            },
                            message: '手机号码不正确',
                            trigger: ['change', 'blur'],
                        }
                    ]
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
            //编辑获取收货地址信息
            getShipInfo() {
                let data = {
                    'id': this.id
                }
                this.$u.api.shipDetail(data).then(res => {
                    if (res.status) {
                        let region = res.data.areaName.split(" ");
                        this.form.name = res.data.name;
                        this.form.mobile = res.data.mobile;
                        this.region = region;
                        this.areaId = res.data.areaId;
                        this.init();
                        this.pickerValue = this.region[0] + " " + this.region[1] + " " + this.region[2]
                        this.form.address = res.data.address;
                        this.form.isDefault = res.data.isDefault;
                        if (res.data.isDefault) {
                            this.checked = true;
                            this.isDefault = 1;
                        } else {
                            this.checked = false;
                            this.isDefault = 2;
                        }
                    } else {
                        this.$u.toast('获取收货地址信息出现问题');
                        // this.submitStatus = false;
                    }
                    this.submitStatus = false;
                });
            },
            //删除地址
            delShip() {
                this.submitStatus = true;
                this.$u.api.removeShip({ 'id': this.id }).then(res => {
                    this.submitStatus = false;
                    if (res.status) {
                        this.$refs.uToast.show({ title: res.msg, type: 'success', back: true })
                    } else {
                        this.$u.toast(res.msg);
                        // this.submitStatus = false;
                    }
                });
            },
            //存储收货地址
            saveShip() {

                this.$refs.uForm.validate(valid => {
                    if (valid) {
                        console.log('验证通过');

                        if (this.checked) {
                            this.form.isDefault = 1;
                        } else {
                            this.form.isDefault = 2;
                        }

                        this.submitStatus = false;
                        if (!this.form.name) {
                            this.$u.toast('请输入收货人姓名')
                            return false
                        } else if (!this.form.mobile) {
                            this.$u.toast('请输入收货人手机号')
                            return false
                        } else if (this.form.mobile.length !== 11) {
                            this.$u.toast('收货人手机号格式不正确')
                            return false
                        } else if (this.areaId <= 0) {
                            this.$u.toast('请选择地区信息')
                            return false
                        } else if (!this.form.address) {
                            this.$u.toast('请输入收货地址详细信息')
                            return false
                        }

                        let data = {
                            name: this.form.name,
                            address: this.form.address,
                            mobile: this.form.mobile,
                            isDefault: this.form.isDefault,
                            areaId: this.areaId
                        }
                        if (this.id && this.id != 0) {
                            //编辑存储
                            data.id = this.id
                            this.$u.api.editShip(data).then(res => {
                                this.submitStatus = false;
                                if (res.status) {
                                    this.$refs.uToast.show({ title: res.msg, type: 'success', back: true })
                                } else {
                                    this.$u.toast(res.msg);
                                    // this.submitStatus = false;
                                }
                            });
                        } else {
                            //添加
                            this.$u.api.saveUserShip(data).then(res => {
                                this.submitStatus = false;
                                if (res.status) {
                                    this.$refs.uToast.show({ title: res.msg, type: 'success', back: true })
                                } else {
                                    this.$u.toast(res.msg);
                                    // this.submitStatus = false;
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
            init() {
                this.getFullPath(this.areaId, this.province);
                this.pickerIndex = [this.provinceKey, this.cityKey, this.areaKey];
                console.log(this.pickerIndex);
            },
        },
        onLoad(e) {
            if (e.shipId) {
                //编辑
                this.id = e.shipId;
                this.getShipInfo();
            } else {
                //添加（取消初始化）
                //this.pickerValue = this.region[0] + " " + this.region[1] + " " + this.region[2];
                //this.init();
            }

        }
    }
</script>
<style lang="scss">
    page { background: #fff; }
</style>