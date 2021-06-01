<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="编辑地址"></u-navbar>
        <view class="content">
            <view class="content-top">

                <view class="cu-form-group">
                    <view class="title">收货人</view>
                    <input placeholder="请填写收货人姓名" v-model="name" />
                </view>
                <view class="cu-form-group">
                    <view class="title">手机号</view>
                    <input placeholder="请填写收货人手机号" v-model="mobile" />
                </view>

                <view class="cu-form-group">
                    <view class="title">省市区</view>
                    <input :value="pickerValue" @focus="showThreePicker" />
                    <u-select v-model="show" mode="mutil-column-auto" :list="pickerList" :default-value="pickerIndex" @confirm="onConfirm"></u-select>
                <text class='cuIcon-locationfill text-orange' @click="showThreePicker"></text>
                </view>

                <view class="cu-form-group align-start">
                    <view class="title">详细地址</view>
                    <textarea maxlength="-1" v-model="address" placeholder="请填写收货详细地址"></textarea>
                </view>

                <radio-group class="block" @click="defaultChange">
                    <view class="cu-form-group margin-top">
                        <view class="title">设为默认</view>
                        <radio value="1" :checked="checked" color="#FF7159" />
                    </view>
                </radio-group>

            </view>
            <view class="coreshop-bottomBox">
                <button class="coreshop-btn coreshop-btn-square coreshop-btn-w" @click="delShip" v-if="id && id != 0" hover-class="btn-hover2" :disabled='submitStatus' :loading='submitStatus'>删除</button>
                <button class="coreshop-btn coreshop-btn-square coreshop-btn-b" @click="saveShip" hover-class="btn-hover2" :disabled='submitStatus' :loading='submitStatus'>保存</button>
            </view>
        </view>
    </view>
</template>

<script>
    export default {
        data() {
            return {
                id: 0,
                name: '',
                mobile: '',
                region: ['湖南省', '怀化市', '鹤城区'],
                areaId: 431202,
                address: '',
                isDefault: 2,
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
            // 信息验证
            checkData(data) {
                this.submitStatus = false;
                if (!data.name) {
                    this.$u.toast('请输入收货人姓名')
                    return false
                } else if (!data.mobile) {
                    this.$u.toast('请输入收货人手机号')
                    return false
                } else if (data.mobile.length !== 11) {
                    this.$u.toast('收货人手机号格式不正确')
                    return false
                } else if (!data.areaId) {
                    this.$u.toast('请选择地区信息')
                    return false
                } else if (!data.address) {
                    this.$u.toast('请输入收货地址详细信息')
                    return false
                } else {
                    return true
                }
            },
            //默认
            defaultChange() {
                if (this.checked) {
                    this.checked = false;
                    this.isDefault = 2;
                } else {
                    this.checked = true;
                    this.isDefault = 1;
                }
            },
            //编辑获取收货地址信息
            getShipInfo() {
                let data = {
                    'id': this.id
                }
                this.$u.api.shipDetail(data).then(res => {
                    if (res.status) {
                        let region = res.data.areaName.split(" ");
                        this.name = res.data.name;
                        this.mobile = res.data.mobile;
                        this.region = region;
                        this.areaId = res.data.areaId;
                        this.init();
                        this.pickerValue = this.region[0] + " " + this.region[1] + " " + this.region[2]
                        this.address = res.data.address;
                        this.isDefault = res.data.isDefault;
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
                this.submitStatus = true;
                let data = {
                    name: this.name,
                    address: this.address,
                    mobile: this.mobile,
                    isDefault: this.isDefault,
                    areaId: this.areaId
                }

                if (this.id && this.id != 0) {
                    //编辑存储
                    data.id = this.id

                    if (this.checkData(data)) {
                        this.$u.api.editShip(data).then(res => {
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
                    //添加
                    if (this.checkData(data)) {
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
                }
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
                //添加
                this.pickerValue = this.region[0] + " " + this.region[1] + " " + this.region[2];
                this.init();
            }

        }
    }
</script>
