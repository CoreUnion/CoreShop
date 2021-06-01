<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="地址列表"></u-navbar>
        <view class="content">
            <view v-if="list.length">
                <view class="item" v-for="(item, key) in list" :key="key">
                    <view class="top" @click="isSelect(item)">
                        <view class="name">{{ item.name }}</view>
                        <view class="phone">{{ item.mobile }}</view>
                        <view class="tag">
                            <text class="red" v-if="item.isDefault">默认</text>
                        </view>
                    </view>
                    <view class="bottom" @click="isSelect(item)">
                        {{item.areaName + item.address}}
                        <u-icon name="edit-pen" :size="40" color="#999999" v-show="type != 'order'" @click="toEdit(item.id)"></u-icon>
                    </view>
                </view>
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$apiFilesUrl+'/static/images/empty/address.png'" icon-size="300" text="暂无地址信息" mode="list"></u-empty>
            </view>
            <view class="coreshop-bottomBox">
                <!-- #ifdef MP-WEIXIN -->
                <button class="coreshop-btn coreshop-btn-square coreshop-btn-b" @click="wechatAddress" hover-class="btn-hover2">从微信获取</button>
                <!-- #endif -->
                <button class="coreshop-btn coreshop-btn-square coreshop-btn-w" @click="toAdd()" hover-class="btn-hover2">新增收货地址</button>
            </view>

        </view>
    </view>
</template>

<script>
    export default {
        data() {
            return {
                list: [],// 用户收货地址列表
                type: ''
            }
        },
        onLoad(e) {
            if (e.type) {
                this.type = e.type;
            }
        },
        onShow() {
            this.userShipList();
        },
        methods: {
            // 获取收货地址列表
            userShipList() {
                this.$u.api.userShip().then(res => {
                    if (res.status) {
                        this.list = res.data
                    }
                })
            },
            //编辑
            toEdit(id) {
                this.$u.route('/pages/member/address/index?shipId=' + id);
            },
            //添加
            toAdd() {
                this.$u.route('/pages/member/address/index');
            },
            //选择
            isSelect(data) {
                if (this.type == 'order') {
                    let pages = getCurrentPages();//当前页
                    let beforePage = pages[pages.length - 2];//上个页面

                    // #ifdef MP-ALIPAY || MP-TOUTIAO
                    this.$db.set('addressUserShip', data, true);
                    // #endif

                    // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE
                    this.$store.commit("userShip", data)
                    // #endif

                    // #ifdef MP-WEIXIN
                    beforePage.$vm.userShip = data;
                    beforePage.$vm.params.areaId = data.areaId;
                    // #endif

                    uni.navigateBack({
                        delta: 1
                    });
                    // this.$u.route("/pages/placeOrder/index/index")
                }
            },
            // #ifdef MP-WEIXIN
            wechatAddress: function () {
                let _that = this;
                wx.chooseAddress({
                    success: res => {
                        if (res.errMsg == "chooseAddress:ok") {
                            //获取成功
                            //存储这个收获地区信息到数据库
                            let data = {
                                provinceName: res.provinceName,
                                cityName: res.cityName,
                                countyName: res.countyName,
                                postalCode: res.postalCode
                            };
                            let areaId = 0;
                            this.$u.api.getAreaId(data).then(res1 => {
                                if (res1.status) {
                                    //存储用户收货信息
                                    let userShipId = 0;
                                    let userShipData = {
                                        areaId: res1.data,
                                        name: res.userName,
                                        address: res.detailInfo,
                                        mobile: res.telNumber,
                                        isDefault: 2
                                    }
                                    this.$u.api.saveUserShipWx(userShipData).then(res2 => {
                                        if (res2.status) {
                                            this.$refs.uToast.show({
                                                title: '存储微信地址成功', type: 'success', callback: function () {
                                                    _that.userShipList();
                                                }
                                            });
                                        } else {
                                            this.$refs.uToast.show({ title: '存储微信地址失败', type: 'error' });
                                        }
                                    });
                                } else {
                                    this.$refs.uToast.show({ title: '获取微信地址失败', type: 'error' });
                                }
                            });
                        } else {
                            this.$refs.uToast.show({ title: '获取微信地址失败', type: 'error' });
                        }
                    }
                });
            },
            // #endif
        }
    }
</script>

<style lang="scss" scoped>
    .item { padding: 40rpx 20rpx; background: #fff; margin-bottom: 10rpx; }
        .item .top { display: flex; font-weight: bold; font-size: 34rpx; }
            .item .top .phone { margin-left: 60rpx; }
            .item .top .tag { display: flex; font-weight: normal; align-items: center; }
                .item .top .tag text { display: block; width: 60rpx; height: 34rpx; line-height: 34rpx; color: #ffffff; font-size: 20rpx; border-radius: 6rpx; text-align: center; margin-left: 30rpx; background-color: #3191fd; }
                .item .top .tag .red { background-color: red; }
        .item .bottom { display: flex; margin-top: 20rpx; font-size: 28rpx; justify-content: space-between; color: #999999; }
</style>