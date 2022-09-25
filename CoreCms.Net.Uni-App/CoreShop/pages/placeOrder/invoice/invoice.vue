<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="发票设置"></u-navbar>

        <view class="coreshop-bg-white">
            <u-form :model="model" :rules="rules" ref="uForm" :errorType="errorType">
                <view class="u-padding-20">
                    <u-form-item label="发票类型" label-width="150" prop="typeName">
                        <u-radio-group v-model="model.typeName">
                            <u-radio @change="radioChange" v-for="(item, index) in radioItems" :key="index" :name="item.name">
                                {{item.name}}
                            </u-radio>
                        </u-radio-group>
                    </u-form-item>

                    <u-form-item label="发票抬头" label-width="150" class="cheque" prop="name">
                        <u-input v-model="model.name" placeholder='抬头名称' @input="getCheque" />
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
                    </u-form-item>
                    <u-form-item label="税务编号" label-width="150" prop="code" v-show="type === '3'">
                        <u-input v-model="model.code" placeholder='纳税人识别号' />
                    </u-form-item>
                    <u-form-item label="发票内容" label-width="150" v-show="type === '3'">
                        <view>明细</view>
                    </u-form-item>

                </view>

                <view class="coreshop-bottomBox">
                    <button class="coreshop-btn coreshop-btn-square coreshop-btn-w" @click="saveInvoice">保存发票</button>
                    <button class="coreshop-btn coreshop-btn-square coreshop-btn-b" @click="notNeedInvoice">本次不开具发票</button>
                </view>

            </u-form>
        </view>

        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>
<script>
    export default {
        data() {
            return {
                radioItems: [{
                    name: '个人发票',
                    value: '2'
                },
                {
                    name: '企业发票',
                    value: '3'
                }],
                type: '1', // 发票类型 1不开发票、2个人发票、3公司发票
                model: {
                    typeName: '', // 发票类型 2个人 3企业
                    name: '', // 抬头名称
                    code: '', // 税号
                },
                errorType: ['message'],
                isShow: false,
                chequeLisit: [],
                rules: {
                    typeName: [
                        {
                            required: true,
                            message: '请选择发票类型或点击不开具',
                            trigger: ['change', 'blur'],
                        }
                    ],
                    name: [
                        {
                            required: true,
                            message: '请输入发票抬头',
                            trigger: ['change', 'blur'],
                        }
                    ],
                    code: [
                        {
                            validator: (rule, value, callback) => {
                                if (this.type === '3' && !this.model.code) {
                                    return false;
                                }
                                return true;
                            },
                            message: '请输入发票税务编号'
                        }
                    ],
                },
            }
        },
        onReady() {
            this.$refs.uForm.setRules(this.rules);
        },
        onLoad() {
            let invoice
            let pages = getCurrentPages()
            let pre = pages[pages.length - 2]
            console.log(pre);
            if (pre != undefined) {
                invoice = pre.$vm.invoice
                if (invoice && invoice.hasOwnProperty('type') && invoice.type !== '1') {
                    // 发票不是未使用, 展示发票信息
                    this.model.name = invoice.name;
                    this.model.code = invoice.code;
                    this.type = invoice.type;
                    if (invoice.type == 2) {
                        this.model.typeName = this.radioItems[0].name;
                    } else if (invoice.type == 3) {
                        this.model.typeName = this.radioItems[1].name;
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
                this.$refs.uForm.validate(valid => {
                    if (valid) {
                        console.log('验证通过');

                        let data = {
                            type: this.type,
                            name: this.model.name
                        }
                        // 不是企业类型不需要税号
                        data['code'] = this.type === '3' ? this.model.code : ''

                        this.setPageData(data)

                    } else {
                        console.log('验证失败');
                    }
                });


            },
            // 向上个页面赋值并返回
            setPageData(data) {
                let pages = getCurrentPages(); //当前页
                let beforePage = pages[pages.length - 2]; //上个页面
                if (beforePage != undefined) {
                    beforePage.$vm.invoice = data;
                    uni.navigateBack({
                        delta: 1
                    })
                }
            },
            getCheque(event) {
                let name = event;
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
                this.model.name = item.name;
                this.model.code = item.code;
                this.isShow = false;
                this.type = '3';
                this.model.typeName = this.radioItems[1].name;
            }
        }
    }
</script>
<style scoped lang="scss">
    @import "invoice.scss";
</style>
