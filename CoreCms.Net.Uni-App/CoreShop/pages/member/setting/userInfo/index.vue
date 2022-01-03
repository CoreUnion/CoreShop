<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="个人信息"></u-navbar>

        <view class="coreshop-bg-white coreshop-solid-top u-padding-left-80 u-padding-right-80 u-padding-top-45 u-padding-bottom-45 u-margin-bottom-20">
            <view class="u-text-center u-margin-bottom-20">
                <text class="coreshop-text-black">完成</text>
                <text class="coreshop-text-orange u-font-40">100%</text>
                <text class="coreshop-text-black">，太棒啦！</text>
            </view>
            <progress percent="100" active stroke-width="10" activeColor="#f37b1d" />
        </view>

        <view class="coreshop-list menu coreshop-bg-white u-padding-30">
            <u-form :model="model" :rules="rules" ref="uForm" :errorType="errorType">
                <u-form-item label="头像：" label-width="150">
                    <u-avatar :src="model.avatar" @click="uploadAvatar" size="large"></u-avatar>
                </u-form-item>
                <u-form-item label="昵称：" label-width="150" prop="nickname"><u-input v-model="model.nickname" /></u-form-item>
                <u-form-item label="性别：" label-width="150" prop="sex">
                    <u-input :border="border" type="select" :select-open="actionSheetShow" v-model="model.sex" placeholder="请选择性别" @click="actionSheetShow = true"></u-input>
                </u-form-item>
                <u-form-item label="生日：" label-width="150" prop="birthday">
                    <u-input v-model="model.birthday" />
                    <u-button slot="right" type="success" size="mini" @click="getCalendar">选择日期</u-button>
                </u-form-item>
            </u-form>
        </view>
        <u-calendar v-model="showCalendar" :mode="calendarMode" @change="calendarChange" safe-area-inset-bottom="true"></u-calendar>
        <!--按钮-->
        <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
            <view class="u-padding-20 flex-direction">
                <u-button :custom-style="customStyle" type="error" size="medium" @click="submitHandler()" :disabled='submitStatus' :loading='submitStatus'>保存</u-button>
            </view>
        </view>
        <u-action-sheet :list="actionSheetList" v-model="actionSheetShow" @click="actionSheetCallback"></u-action-sheet>
    </view>
</template>

<script>
    export default {
        data() {
            return {
                customStyle: {
                    width: '100%',
                },
                model: {
                    birthday: '',
                    nickname: '',
                    sex: '',
                    sexIndex: 0,
                    avatar: '',
                    mobile: '',
                },
                showCalendar: false,
                show: false,
                calendarMode: 'date',
                errorType: ['message'],
                actionSheetShow: false,
                actionSheetList: [
                    {
                        text: '男'
                    },
                    {
                        text: '女'
                    },
                    {
                        text: '保密'
                    }
                ],
                rules: {
                    nickname: [
                        {
                            required: true,
                            message: '请输入昵称',
                            trigger: 'blur',
                        },
                        {
                            min: 2,
                            max: 16,
                            message: '昵称长度在2到16个长度',
                            trigger: ['change', 'blur'],
                        },
                        //{
                        //    validator: (rule, value, callback) => {
                        //        return this.$u.test.chinese(value);
                        //    },
                        //    message: '昵称必须为中文',
                        //    trigger: ['change', 'blur'],
                        //}
                    ],
                    sex: [
                        {
                            required: true,
                            message: '请选择性别',
                            trigger: 'change'
                        },
                    ],
                    birthday: [
                        {
                            required: true,
                            message: '请选择生日',
                            trigger: 'blur',
                        }
                    ],
                },
                index: 2,
                submitStatus: false
            }
        },
        computed: {
            startDate() {
                return this.getDate('start');
            },
            endDate() {
                return this.getDate('end');
            }
        },
        // 必须要在onReady生命周期，因为onLoad生命周期组件可能尚未创建完毕
        onReady() {
            this.$refs.uForm.setRules(this.rules);
        },
        methods: {
            getCalendar: function () {
                this.showCalendar = true;
            },
            calendarChange(e) {
                console.log(e);
                this.model.birthday = e.result;
            },
            //生日
            bindDateChange: function (e) {
                this.birthday = e.target.value;
            },
            // 点击actionSheet回调
            actionSheetCallback(index) {
                uni.hideKeyboard();
                this.model.sex = this.actionSheetList[index].text;
                this.model.sexIndex = index;
            },
            getDate(type) {
                const date = new Date();
                let year = date.getFullYear();
                let month = date.getMonth() + 1;
                let day = date.getDate();

                if (type === 'start') {
                    year = year - 60;
                } else if (type === 'end') {
                    year = year + 2;
                }
                month = month > 9 ? month : '0' + month;;
                day = day > 9 ? day : '0' + day;
                return `${year}-${month}-${day}`;
            },
            // 用户上传头像
            uploadAvatar() {
                this.$upload.uploadFiles(null, res => {
                    if (res.status) {
                        let avatar = res.data.src // 上传成功的图片地址
                        // 执行头像修改
                        this.$u.api.changeAvatar({
                            id: avatar
                        }).then(res => {
                            if (res.status) {
                                this.$refs.uToast.show({
                                    title: '上传成功', type: 'success', callback: function () {
                                        this.model.avatar = res.data
                                    }
                                })
                            } else {
                                this.$u.toast(res.msg)
                            }
                        })
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
            // 保存资料
            submitHandler() {
                this.submitStatus = true;
                let sex = parseInt(this.model.sexIndex) + 1;
                if (!!!this.model.birthday) {
                    this.$refs.uToast.show({ title: '请选择出生日期', type: 'error' })
                    this.submitStatus = false;
                    return false;
                } else {
                    this.$refs.uForm.validate(valid => {
                        if (valid) {
                            console.log('验证通过');
                            this.$u.api.editInfo({
                                sex: sex,
                                birthday: this.model.birthday,
                                nickname: this.model.nickname
                            }).then(res => {
                                this.submitStatus = false;
                                this.$refs.uToast.show({ title: res.msg, type: 'success', back: false })
                            });
                        } else {
                            console.log('验证失败');
                            this.submitStatus = false;
                            this.$refs.uToast.show({ title: '验证失败，请完善信息', type: 'error' })
                        }
                    });
                }
            }
        },
        onLoad: function () {
            var _this = this;
            _this.$u.api.userInfo().then(res => {
                if (res.status) {
                    var theSex = res.data.sex - 1; //数组从0开始
                    if (res.data.birthday) {
                        _this.model.birthday = res.data.birthday.substring(0, 10);
                    }
                    _this.model.nickname = res.data.nickName;
                    _this.model.mobile = res.data.mobile;
                    _this.model.sex = _this.actionSheetList[theSex].text;
                    console.log(_this.model.sex);
                    _this.model.avatar = res.data.avatarImage;

                } else {
                    //报错了
                    _this.$u.toast(res.msg);
                }
            });
        }
    }
</script>

<style scoped lang="scss">
</style>
