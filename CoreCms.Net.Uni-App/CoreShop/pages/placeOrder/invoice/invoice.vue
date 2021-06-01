<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="发票设置"></u-navbar>
        <view class="content">
            <view class="content-top">
                <u-radio-group v-model="typeName">
                    <view class="cu-form-group">
                        <view class="title">发票类型：</view>
                        <view>
                            <u-radio @change="radioChange" v-for="(item, index) in radioItems" :key="index" :name="item.name">
                                {{item.name}}
                            </u-radio>
                        </view>
                    </view>
                </u-radio-group>


                <view class="cu-form-group cheque">
                    <view class="title">发票抬头：</view>
                    <input v-model="name" placeholder='抬头名称' @input="getCheque"></input>
                    <view class="cheque-content" v-show="isShow">
                        <view class="tips-item" v-for="(item,index) in chequeLisit" :key="index" @click="chooseCheque(item)">
                            <view class="tips-name">
                                {{item.name|| ''}}
                            </view>
                            <view class="sub-div">
                                <view class="tax-no">
                                    {{item.code|| ''}}
                                </view>
                                <view class="tips-num">
                                    <view class="num">
                                        {{item.frequency|| ''}}
                                    </view>
                                    人使用过
                                </view>
                            </view>
                        </view>
                    </view>
                </view>
                <view class="cu-form-group" v-show="type === '3'">
                    <view class="title">税务编号：</view>
                    <input v-model="code" placeholder='纳税人识别号'></input>
                </view>

                <view class="cu-form-group">
                    <view class="title">发票内容：</view>
                    <view>明细</view>
                </view>
            </view>
            <view class="coreshop-bottomBox">
                <button class="coreshop-btn coreshop-btn-square coreshop-btn-w" @click="saveInvoice" hover-class="btn-hover2">保存发票</button>
                <button class="coreshop-btn coreshop-btn-square coreshop-btn-b" @click="notNeedInvoice" hover-class="btn-hover2">本次不开具发票</button>
            </view>
        </view>

        <!-- 登录提示 -->
		<corecms-login-modal></corecms-login-modal>
    </view>
</template>
<script>
    export default {
        data() {
            return {
                radioItems: [{
                    name: '个人或事业单位',
                    value: '2'
                },
                {
                    name: '企业',
                    value: '3'
                }],
                type: '1', // 发票类型 2个人 3企业
                typeName: '', // 发票类型 2个人 3企业
                name: '', // 抬头名称
                code: '', // 税号
                isShow: false,
                chequeLisit: []
            }
        },
        onLoad() {
            let invoice
            let pages = getCurrentPages()
            let pre = pages[pages.length - 2]
            console.log(pre);
            if (pre != undefined) {
                // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE
                invoice = pre.invoice
                // #endif
                // #ifdef MP-WEIXIN
                invoice = pre.$vm.invoice
                // #endif
                // #ifdef MP-ALIPAY || MP-TOUTIAO
                invoice = pre.data.invoice;
                // #endif
                if (invoice && invoice.hasOwnProperty('type') && invoice.type !== '1') {
                    // 发票不是未使用, 展示发票信息
                    this.name = invoice.name;
                    this.code = invoice.code;
                    this.type = invoice.type;
                    if (invoice.type == 2) {
                        this.typeName = this.radioItems[0].name;
                    } else if (invoice.type == 3) {
                        this.typeName = this.radioItems[1].name;
                    }
                }
            }
        },
        methods: {
            // 选中某个单选框时，由radio时触发
            radioChange(e) {
                this.radioItems.forEach(item => {
                    if (item.name === e) {
                        this.type = item.value
                    }
                })
            },
            // 不需要发票信息
            notNeedInvoice() {
                let data = {
                    type: '1',
                    name: '',
                    code: ''
                }
                this.setPageData(data)
            },
            // 保存发票信息
            saveInvoice() {
                if (this.type === '1' || this.type === 1) {
                    this.$u.toast('请选择发票类型')
                    return false
                }
                if (!this.name) {
                    this.$u.toast('请输入发票抬头')
                    return false
                }
                // 个人需要输入昵称
                if (this.type === '3' && !this.code) {
                    this.$u.toast('请输入发票税号信息')
                    return false
                }
                // 企业类型需要输入税号
                if (this.type === '3' && !this.code) {
                    this.$u.toast('请输入发票税号信息')
                    return false
                }
                let data = {
                    type: this.type,
                    name: this.name
                }
                // 不是企业类型不需要税号
                data['code'] = this.type === '3' ? this.code : ''
                this.setPageData(data)
            },
            // 向上个页面赋值并返回
            setPageData(data) {
                let pages = getCurrentPages(); //当前页
                let beforePage = pages[pages.length - 2]; //上个页面
                if (beforePage != undefined) {
                    // #ifdef MP-ALIPAY || MP-TOUTIAO
                    this.$db.set('userInvoice', data, true);
                    // #endif
                    // #ifdef MP-WEIXIN
                    beforePage.$vm.invoice = data;
                    // #endif
                    // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE
                    // beforePage.invoice = data;
                    this.$store.commit('invoice', data)
                    // #endif
                    uni.navigateBack({
                        delta: 1
                    })
                }
            },
            getCheque(event) {
                let name = event.detail.value
                if (name != '') {
                    let data = {
                        name: name
                    }
                    this.$u.api.getTaxInfo(data).then(res => {
                        if (res.status) {
                            if (res.data.length != 0) {
                                this.isShow = true
                                this.chequeLisit = res.data
                            } else {
                                // this.isShow = false
                            }
                        } else {
                            this.$u.toast(res.msg)
                        }
                    });
                } else {
                    this.isShow = false
                }
            },
            chooseCheque(item) {
                this.name = item.name;
                this.code = item.code;
                this.isShow = false;
                this.type = '3';
                this.typeName = this.radioItems[1].name;
            }
        }
    }
</script>
<style scoped lang="scss">
    .cu-form-group .title { min-width: calc(4em + 15px); }

    .coreshop-bottomBox .coreshop-btn { width: 100%; }
    .cheque { position: relative; }
    .cheque-content { position: absolute; left: 15rpx; top: 90rpx; z-index: 10; width: calc(100% - 30rpx);; background-color: #fff; box-shadow: 0 0 0.666667vw 0.4vw rgba(0, 0, 0, .13); border-radius: 10rpx; padding: 20rpx; }
    .tips-item { margin-bottom: 20rpx; }
    .tips-name { font-size: 32rpx; line-height: 35rpx; color: #333; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; margin-bottom: 10rpx; }
    .sub-div { display: flex; width: 100%; justify-content: space-between; height: 30rpx; font-size: 24rpx; line-height: 30rpx; color: #999; }
    .num { display: inline-block; }
</style>
