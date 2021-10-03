<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="核销服务码"></u-navbar>
        <view class="content">
            <view class="u-padding-15 u-margin-bottom-15 coreshop-bg-white u-border-bottom">
                <u-search placeholder="请输入完整服务券核销码" v-model="key" :show-action="true" action-text="搜索" :animation="false" @search="search" @custom="search"></u-search>
            </view>
            <button class="floatingButton" hover-class="none" @click="Qrcode">
                <u-icon name="scan" color="#e54d42" size="60"></u-icon>
            </button>
            <view v-if="isShow">
                <checkbox-group @change="checkboxChange">

                    <view class="orderList" :class="ticket.status>0?' grayscale':''">
                        <view class="top">
                            <view class="left">
                                <u-icon name="order" :size="30" color="rgb(94,94,94)"></u-icon>
                                <view class="store">服务单号：{{ticket.serviceOrderId}}</view>
                                <u-icon name="arrow-right" color="rgb(203,203,203)" :size="26"></u-icon>
                            </view>
                            <view class="right">
                                <u-tag :text="ticket.statusStr" type="success" v-if="ticket.status==0" /> <!--正常-->
                                <u-tag :text="ticket.statusStr" type="error" v-if="ticket.status==1" /> <!--过期-->
                                <u-tag :text="ticket.statusStr" type="primary" v-if="ticket.status==2" /> <!--作废-->
                                <u-tag :text="ticket.statusStr" type="info" v-if="ticket.status==3" /> <!--已核销-->
                            </view>
                        </view>
                        <view class="item">
                            <view class="left"><image :src="service.thumbnail && service.thumbnail!='null' ?  service.thumbnail : '/static/images/common/empty-banner.png'" mode="aspectFill"></image></view>
                            <view class="content">
                                <view class="title u-line-2">{{service.title}}</view>
                                <view class="type">是否核销：{{ticket.isVerification?'是':'否'}}</view>
                                <view class="type">有效状态：{{ticket.validityType==1?'长期有效':'限定消费时间'}}</view>
                                <view class="delivery-time">服务券状态：{{ ticket.statusStr }}</view>
                            </view>
                        </view>
                        <view class="bottom u-margin-0" v-if="ticket.validityStartTime && ticket.validityEndTime">
                            <view class="u-font-xs">
                                可核销时间：{{ticket.validityStartTime}} 至 {{ticket.validityEndTime}}
                            </view>
                        </view>
                        <view class="bottom u-margin-0">
                            <view class="more u-font-xs" v-if="ticket.createTime && ticket.status>0">
                                下单时间：{{ $u.timeFormat(ticket.createTime, 'mm-dd hh:MM:ss') }}
                            </view>
                            <view class="more" v-else>
                                下单时间：{{ $u.timeFormat(ticket.createTime, 'mm-dd hh:MM:ss') }}
                            </view>
                            <view class="more u-font-xs" v-if="ticket.verificationTime">
                                核销时间：{{ $u.timeFormat(ticket.verificationTime, 'mm-dd hh:MM:ss') }}
                            </view>
                            <view v-if="!ticket.disabled">
                                <checkbox color="#FF7159" :value="ticket.redeemCode" :checked="ticket.checked" :disabled="ticket.disabled" v-if="ticket.disabled" class="checkboxNo" />
                                <checkbox color="#FF7159" :value="ticket.redeemCode" :checked="ticket.checked" :disabled="ticket.disabled" v-else />
                            </view>

                        </view>
                    </view>
                </checkbox-group>
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="未查询到数据" mode="list"></u-empty>
            </view>

            <view class="coreshop-bottomBox" v-if="isShow">
                <button class="coreshop-btn coreshop-btn-b coreshop-btn-square" @click="write" v-if="checkedIds.length">确认核销</button>
                <button class="coreshop-btn coreshop-btn-b coreshop-btn-square completed" v-else>请选择待核销服务券</button>
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
                ticket: {},
                serviceOrder: {},
                service: {},
                isShow: false
            }
        },
        onLoad(e) {
            if (e.id) {
                this.key = e.id;
            }
            this.getServiceVerificationTicketInfo();
        },
        computed: {
            // 获取选中的提货单id
            checkedIds() {
                let ids = []
                if (!this.ticket.disabled && this.ticket.checked && this.ticket.status == 0) {
                    ids.push(this.ticket.redeemCode)
                }
                return ids
            },
        },
        methods: {
            // 多选框点击事件处理
            checkboxChange(e) {
                var values = e.detail.value;
                if (values.includes(this.ticket.redeemCode)) {
                    this.ticket.checked = true
                } else {
                    this.ticket.checked = false
                }
            },
            //获取提货单详情
            getServiceVerificationTicketInfo() {
                let _this = this;
                if (this.key) {
                    let data = {
                        'id': this.key
                    }
                    this.$u.api.getServiceVerificationTicketInfo(data).then(e => {
                        if (e.status) {
                            _this.ticket = _this.formatData(e.data.ticket);
                            _this.service = e.data.service;
                            _this.serviceOrder = e.data.serviceOrder;
                            _this.isShow = true;
                        } else {
                            _this.ticket = {}; // 清空数据
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
                    this.getServiceVerificationTicketInfo();
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
                        _this.getServiceVerificationTicketInfo();
                    }
                });
            },
            // 数据转化
            formatData(data) {
                if (data.isVerification) {
                    // 已提货
                    this.$set(data, 'checked', false)
                    this.$set(data, 'disabled', true)
                } else {
                    // 未提货
                    this.$set(data, 'checked', true)
                    this.$set(data, 'disabled', false)
                }
                return data
            },
            //去核销
            write() {
                let _this = this;
                this.$common.modelShow('提示', '您确认核销吗？', res => {
                    //去核销
                    let data = {
                        id: _this.checkedIds.join()
                    }
                    _this.$u.api.serviceVerificationTicket(data).then(res => {
                        if (res.status) {
                            _this.$refs.uToast.show({
                                title: res.msg, type: 'success', callback: function () {
                                    _this.afterChangeDataStatus()
                                }
                            })

                        } else {
                            _this.$u.toast(res.msg);
                            return false;
                        }
                    });
                });
            },
            // 核销完成后更改数据状态
            afterChangeDataStatus() {
                if (this.checkedIds.indexOf(this.ticket.redeemCode) > -1) {
                    this.ticket.status = true;
                    this.ticket.checked = false;
                    this.ticket.disabled = true;
                    this.getServiceVerificationTicketInfo();
                }
            }
        }
    }
</script>


<style scoped lang="scss">
</style>
