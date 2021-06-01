<!-- 门店列表 -->
<template>
    <view class="page_box store-list-wrap">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="选择门店"></u-navbar>
        <view class="content_box">
            <label v-for="store in storeList" :key="store.id" @tap="selStore(store.id)">
                <view class="store-item x-bc">
                    <view class="x-f">
                        <view class="img-box"><image class="store-img" :src="store.logoImage" mode="aspectFill" lazy-load></image></view>
                        <view class="item-left y-start">
                            <text class="store-title">{{ store.storeName }}</text>
                            <text class="store-content">{{ store.address }}</text>
                        </view>
                    </view>

                    <radio style="transform: scale(0.7);" class="orange" :checked="storeId == store.id" :class="{ checked: storeId == store.id }"></radio>
                </view>
            </label>
        </view>
        <view class="foot_box x-c"><button class="cu-btn save-btn" @tap="saveStore">确认</button></view>
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
                uni.getLocation({
                    type: 'gcj02',
                    success: function (res) {
                        _this.longitude = res.longitude;
                        _this.latitude = res.latitude;
                    },
                    complete: function (res) {
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
                });
            },
        }
    };
</script>


<style lang="scss" scoped>
    @import '../../../../static/style/merchant.scss';
</style>
