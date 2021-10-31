<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="提货单核销"></u-navbar>
        <view class="content">

            <view class="u-padding-15 u-margin-bottom-15 coreshop-bg-white u-border-bottom">
                <u-search placeholder="请输入完整手机号、订单号" v-model="key" :show-action="true" action-text="搜索" :animation="false" @search="search" @custom="search"></u-search>
            </view>

            <button class="floatingButton" hover-class="none" @click="Qrcode">
                <u-icon name="scan" color="#e54d42" size="60"></u-icon>
            </button>

            <view v-if="allData.length">
                <checkbox-group @change="checkboxChange">
                    <view class="orderList" :class="item.status?' grayscale':''" v-for="(item, index) in allData" :key="index">
                        <view class="top">
                            <view class="left">
                                <u-icon name="order" :size="30" color="rgb(94,94,94)"></u-icon>
                                <view class="store">订单号：{{item.orderId}}</view>
                                <!--<u-icon name="arrow-right" color="rgb(203,203,203)" :size="26"></u-icon>-->
                            </view>
                            <view class="right">{{item.statusName}}</view>
                        </view>
                        <view class="item" v-for="(v, key) in item.orderItems" :key="key">
                            <view class="left"><image :src="v.imageUrl && v.imageUrl!='null' ?  v.imageUrl : '/static/images/common/empty-banner.png'" mode="aspectFill"></image></view>
                            <view class="content">
                                <view class="title u-line-2">{{v.name}}</view>
                                <view class="type">{{v.addon}}</view>
                                <view class="delivery-time">SN码：{{v.sn}}</view>
                                <view class="delivery-time">BN码：{{v.bn}}</view>
                            </view>
                            <view class="right">
                                <view class="price">￥{{ v.price }}</view>
                                <view class="number">x{{ v.nums }}</view>
                            </view>
                        </view>
                        <view class="bottom u-margin-top-20">
                            <view class="more u-font-xs" v-if="item.status">
                                下单时间：{{ $u.timeFormat(item.createTime, 'mm-dd hh:MM:ss') }}
                            </view>
                            <view class="more" v-else>
                                下单时间：{{ $u.timeFormat(item.createTime, 'mm-dd hh:MM:ss') }}
                            </view>
                            <view class="more u-font-xs" v-if="item.status">
                                提货时间：{{ $u.timeFormat(item.pickUpTime, 'mm-dd hh:MM:ss') }}
                            </view>
                            <view v-if="!item.disabled">
                                <checkbox color="#FF7159" :value="item.id" :checked="item.checked" :disabled="item.disabled" v-if="item.disabled" class="checkboxNo" />
                                <checkbox color="#FF7159" :value="item.id" :checked="item.checked" :disabled="item.disabled" v-else />
                            </view>

                        </view>
                    </view>
                </checkbox-group>
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="未查询到数据" mode="list"></u-empty>
            </view>

            <view class="coreshop-bottomBox" v-if="allData.length">
                <button class="coreshop-btn coreshop-btn-b coreshop-btn-square" @click="write" v-if="checkedIds.length">确认核销</button>
                <button class="coreshop-btn coreshop-btn-b coreshop-btn-square completed" v-else>请选择待核销订单</button>
            </view>
        </view>
    </view>

</template>

<script>
    export default {
        data() {
            return {
                key: '', // 筛选条件
                isgo: false,
                isgotext: '确认核销',
                allData: [] // 提货单列表
            }
        },
        onLoad(e) {
            if (e.id) {
                this.key = e.id;
            }
            this.getLadingInfo();
        },
        computed: {
            // 获取选中的提货单id
            checkedIds() {
                let ids = []
                this.allData.forEach(item => {
                    // 判断不是禁用状态 并且是选中状态 并且是未核销状态
                    if (!item.disabled && item.checked && item.status === false) {
                        ids.push(item.id)
                    }
                })
                return ids
            },
        },
        methods: {
            // 多选框点击事件处理
            checkboxChange(e) {
                var values = e.detail.value;
                this.allData.forEach(item => {
                    if (values.includes(item.id)) {
                        item.checked = true
                    } else {
                        item.checked = false
                    }
                })
            },
            //获取提货单详情
            getLadingInfo() {
                let _this = this;
                if (this.key) {
                    let data = {
                        'id': this.key
                    }
                    this.$u.api.ladingInfo(data).then(e => {
                        if (e.status) {
                            console.log("获取数据");
                            if (e.data.length > 0) {
                                console.log("去数据转化");
                                _this.allData = _this.formatData(e.data);
                            } else {
                                console.log("清空数据");
                                _this.allData = []; // 清空数据
                                _this.$u.toast('未查询到相关信息');
                            }
                        } else {
                            console.log("数据获取一样");
                            _this.allData = []; // 清空数据
                            _this.$refs.uToast.show({
                                title: e.msg,
                                type: 'success',
                            })
                        }
                    });
                }
            },

            //搜索
            search() {
                if (this.key != '') {
                    this.getLadingInfo();
                } else {
                    this.$u.toast('请输入查询关键字');
                    return false;
                }
            },
            //扫码
            Qrcode() {
                let _this = this;
                // 只允许通过相机扫码
                uni.scanCode({
                    onlyFromCamera: true,
                    success: function (res) {
                        //console.log('条码类型：' + res.scanType);
                        //console.log('条码内容：' + res.result);
                        _this.key = res.result;
                        _this.getLadingInfo();
                    }
                });
            },
            //查询判断是否可以核销
            isGoWrite(data) {
                let isgo = false;
                if (data.order_info.pay_status == 2 && data.order_info.ship_status == 3) {
                    isgo = true;
                    this.lading_id = data.id;
                    this.goodsList = data.goods;
                    this.allData = data;
                } else {
                    this.$common.modelShow('无法核销', '订单必须支付并已发货才可以核销', function () { });
                }
                this.isgo = isgo;
            },
            // 数据转化
            formatData(data) {
                console.log("数据转化");
                data.forEach(item => {
                    if (item.status === true) {
                        // 已提货
                        this.$set(item, 'checked', false)
                        this.$set(item, 'disabled', true)
                    } else {
                        // 未提货
                        this.$set(item, 'checked', true)
                        this.$set(item, 'disabled', false)
                    }
                })
                return data
            },
            //去核销
            write() {
                let _this = this;
                this.$common.modelShow('提示', '您确认提货核销吗？', function (res) {
                    //去核销
                    let data = {
                        id: _this.checkedIds.join()
                    }
                    _this.$u.api.ladingExec(data).then(res => {
                        if (res.status) {
                            _this.$refs.uToast.show({
                                title: res.msg, type: 'success', callback: function () {
                                    _this.afterChangeDataStatus()
                                }
                            })
                        }
                    });
                });
            },
            // 核销完成后更改数据状态
            afterChangeDataStatus() {
                let _this = this;
                this.allData.forEach(item => {
                    if (_this.checkedIds.indexOf(item.id) > -1) {
                        item.status = true;
                        item.checked = false;
                        item.disabled = true;
                        _this.getLadingInfo();
                    }
                })
            }
        }
    }
</script>

<style scoped lang="scss">
</style>
