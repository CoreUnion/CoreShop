<!-- 门店列表 -->
<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="选择门店"></u-navbar>
        <label v-for="store in storeList" :key="store.id" @tap="selStore(store.id)">
            <view class="store-item u-flex u-row-between">
                <view class="coreshop-flex coreshop-align-center">
                    <view class="img-box"><image class="store-img" :src="store.logoImage" mode="aspectFill" lazy-load></image></view>
                    <view class="item-left coreshop-flex coreshop-flex-direction coreshop-align-start">
                        <text class="store-title">{{ store.storeName }}</text>
                        <text class="store-content">{{ store.address }}</text>
                    </view>
                </view>

                <radio style="transform: scale(0.7);" class="orange" :checked="storeId == store.id" :class="{ checked: storeId == store.id }"></radio>
            </view>
        </label>

        <view class="coreshop-bg-white coreshop-card-hight-box" />

        <!--底部-->
        <view class="coreshop-foot-hight-view" />
        <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom u-text-center u-padding-20">
            <u-button class='coreshop-bg-red' type="success" size="default"  @click="saveStore">确认</u-button>
        </view>

    </view>
</template>

<script>
    export default {
        components: {},
        data() {
            return {
                storeList: [],
                storeId: uni.getStorageSync('storeId')
            };
        },
        computed: {},
        onLoad() {
            this.getStoreAddress();
        },
        methods: {
            // 选择门店
            selStore(id) {
                this.storeId = id;
            },
            // 确认门店
            saveStore() {
                //uni.setStorageSync('storeId', this.storeId);
                this.$u.route('/pages/member/merchant/index/index', {
                    storeId: this.storeId
                });
            },
            //获取门店列表
            getStoreAddress() {
                let _this = this;
                wx.getFuzzyLocation({
                    type: 'wgs84',
                    success(res) {
                        _this.latitude = res.latitude
                        _this.longitude = res.longitude
                        //console.log('当前位置的经度1：' + res);
                    },
                    fail: function () {
                        _this.$u.toast("获取您的经纬度坐标失败")
                    },
                    complete: function (res) {
                        if (!_this.longitude || !_this.latitude) {
                            _this.longitude = '0';
                            _this.latitude = '0';
                        }
                        let data = {
                            'key': _this.key,
                            'longitude': _this.longitude,
                            'latitude': _this.latitude,
                            'page': _this.page,
                            'limit': _this.limit,
                        }
                        _this.$u.api.storeList(data).then(e => {
                            if (e.status) {
                                console.log(e);
                                _this.storeList = [..._this.storeList, ...e.data]
                            } else {
                                _this.$u.toast("门店数据获取失败。");
                            }
                        });
                    }
                })
            },
        }
    };
</script>


<style lang="scss" scoped>
    @import "storeList.scss";
</style>
