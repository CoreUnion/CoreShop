<template>
    <view class="coreshop-bg-white">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="店铺设置"></u-navbar>
        <view class="u-padding-20 ">
            <u-form :model="form" :rules="rules" ref="uForm" :errorType="errorType">
                <u-form-item label="店铺名称" label-width="140" prop="storeName">
                    <u-input type="text" placeholder='请输入店铺名称' v-model="form.storeName" />
                </u-form-item>
                <u-form-item label="店铺Logo" label-width="140">
                    <image class='coreshop-avatar xl radius bg-gray' mode="aspectFill" :src="form.storeLogo" @click="uploadLogo"></image>
                </u-form-item>
                <u-form-item label="店铺简介" label-width="140" prop="storeDesc">
                    <u-input v-model="form.storeDesc" type="textarea" border="true" height="100" auto-height="true" placeholder="请您在此描述问题(最多200字)" maxlength="200" />
                </u-form-item>
                <u-form-item label="店铺招牌" label-width="140">
                    <u-upload :action="upLoadAction" ref="uUpload" :file-list="fileList" max-count="3" :max-size="2 * 1024 * 1024" width="120" height="120" :header="header"></u-upload>
                </u-form-item>
            </u-form>
        </view>
        <!--按钮-->
        <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
            <view class="flex u-padding-20 flex-direction">
                <u-button :custom-style="customStyle" type="error" size="medium" @click="submitHandler">保存</u-button>
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
                upLoadAction: this.$globalConstVars.apiBaseUrl + '/Api/Common/UploadImages',
                header: {
                    'Accept': 'application/json',
                    'Content-Type': 'multipart/form-data',
                    'Authorization': 'Bearer ' + this.$db.get('userToken')
                },
                fileList: [],
                errorType: ['message'],
                form: {
                    storeName: '',//店铺名称
                    storeLogo: '',
                    storeBanner: '',
                    storeDesc: '',//店铺介绍
                },
                rules: {
                    storeName: [
                        {
                            required: true,
                            message: '请输入店铺名称',
                            trigger: ['blur', 'change']
                        }
                    ],
                    storeDesc: [
                        {
                            required: true,
                            message: '请输入店铺简介',
                            trigger: ['blur', 'change']
                        }
                    ]
                }
            }
        },
        onReady() {
            this.$refs.uForm.setRules(this.rules);
            this.fileList = this.$refs.uUpload.lists;
        },
        methods: {
            // 用户上传头像
            uploadLogo() {
                this.$upload.uploadFiles(null, res => {
                    if (res.status) {
                        this.form.storeLogo = res.data.src;
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
            submitHandler() {
                this.$refs.uForm.validate(valid => {
                    if (valid) {
                        console.log('验证通过');
                        console.log(this.fileList);

                        if (!this.form.storeName || this.form.storeName == '') {
                            this.$u.toast('请填写店铺名称');
                            return;
                        }
                        if (this.fileList.length <= 0) {
                            this.$u.toast('请上传店铺招牌');
                            return;
                        } else {
                            let images = [];
                            for (var i = 0; i < this.fileList.length; i++) {
                                if (this.fileList[i].url) {
                                    images.push(this.fileList[i].url);
                                } else if (this.fileList[i].error == false) {
                                    images.push(this.fileList[i].response.data.fileUrl);
                                }
                            }
                            this.form.storeBanner = images.join(',');
                        }

                        if (!this.form.storeLogo) {
                            this.$u.toast('请上传店铺LOGO');
                            return;
                        }

                        this.$u.api.setAgentStore({
                            storeName: this.form.storeName,
                            storeLogo: this.form.storeLogo,
                            storeBanner: this.form.storeBanner,
                            storeDesc: this.form.storeDesc
                        }).then(res => {
                            if (res.status) {
                                this.$refs.uToast.show({ title: res.msg, type: 'success', back: false })
                            } else {
                                this.$u.toast(res.msg);
                            }
                        });

                    } else {
                        console.log('验证失败');
                    }
                });
            }
        },
        onLoad: function () {
            var _this = this;
            _this.$u.api.getAgentInfo({ check_condition: false }).then(res => {
                if (res.status) {
                    _this.form.storeName = res.data.storeName;
                    _this.form.storeDesc = res.data.storeDesc;
                    _this.form.storeLogo = res.data.storeLogo;
                    _this.form.storeBanner = res.data.storeBanner;
                    if (res.data.storeBanner) {
                        var arr = res.data.storeBanner.split(',');
                        for (var i = 0; i < arr.length; i++) {
                            _this.fileList.push({
                                url: arr[i]
                            });
                        }
                    }
                } else {
                    //报错了
                    _this.$u.toast(res.msg);
                }
            });
        }
    }
</script>

<style lang="scss" scoped>
</style>
