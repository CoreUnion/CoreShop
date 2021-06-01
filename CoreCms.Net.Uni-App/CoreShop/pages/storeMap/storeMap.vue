<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="门店列表"></u-navbar>
        <view class="content">
            <view class="map-body">
                <map style="width: 100%;height: 100%;" id="map"
                     :scale="11"
                     :enable-3D="true"
                     :show-compass="true"
                     :enable-overlooking="true"
                     :enable-traffic="true"
                     :latitude="latitude"
                     :longitude="longitude"
                     :markers="covers">
                </map>
            </view>
            <scroll-view class="store-list" scroll-y>

                <view class="u-margin-top-30" v-if="storeList.length>0">
                    <view class="cu-list menu-avatar" v-for="(item, key) in storeList" :key="key">
                        <view class="cu-item  u-padding-top-20  u-padding-bottom-20">
                            <view class="cu-avatar lg radius" :style="[{backgroundImage:'url('+ item.logoImage +')'}]" />
                            <view class="content">
                                <view class="text-grey">{{item.storeName|| ''}}</view>
                                <view class="text-gray text-sm flex">
                                    <u-icon name="phone" size="28"></u-icon>
                                    <view class="text-cut" @tap="doPhoneCall(item.mobile)">
                                        电话：{{item.mobile|| ''}}
                                    </view>
                                </view>
                                <view class="text-gray text-sm flex">
                                    <u-icon name="map" size="28"></u-icon>
                                    <view class="text-cut">
                                        地址：{{item.address|| ''}}
                                    </view>
                                </view>
                            </view>
                            <view class="action" @click="goMarkers(item.latitude,item.longitude)">
                                <view class="text-grey text-xs u-margin-bottom-20">{{item.distanceStr|| ''}}</view>
                                <view class="cu-tag round bg-grey sm">点击查看</view>
                            </view>
                        </view>
                    </view>
                </view>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无门店列表" mode="list"></u-empty>
                </view>

            </scroll-view>
        </view>
        <!-- 登录提示 -->
		<corecms-login-modal></corecms-login-modal>
    </view>
</template>

<script>
    export default {
        data() {
            return {
                storeList: [],
                longitude: 0,
                latitude: 0,
                covers: [{
                    'longitude': 0,
                    'latitude': 0
                }],
                page: 1,
                limit: 30,
            };
        },
        onLoad() {
            this.getMyLocation();
        },
        methods: {
            // 获取自己的位置信息
            getMyLocation() {
                let _this = this;
                uni.getLocation({
                    type: 'wgs84',
                    success: function (res) {
                        _this.longitude = res.longitude;
                        _this.latitude = res.latitude;
                        _this.getStoreList();
                    },
                    fail: function () {
                        _this.$u.toast("获取位置信息失败")
                    }
                });

            },
            // 获取店铺列表信息
            getStoreList() {
                let _this = this;

                let data = {
                    'longitude': _this.longitude,
                    'latitude': _this.latitude,
                    'page': _this.page,
                    'limit': _this.limit,
                }

                _this.$u.api.storeList(data).then(res => {
                    if (res.status) {
                        _this.storeList = res.data;
                        let storeList = res.data;
                        for (let i = 0; i < storeList.length; i++) {
                            let mark = {
                                id: storeList[i].id,
                                latitude: storeList[i].latitude,
                                longitude: storeList[i].longitude,
                                iconPath: '/static/images/map/location.png',
                                width: 15,
                                height: 23,
                            }
                            _this.covers.push(mark);
                        }
                    } else {
                        _this.$u.toast("门店信息获取失败")
                    }
                });
            },
            doPhoneCall(phome) {
                if (phome != 0) {
                    uni.makePhoneCall({
                        phoneNumber: phome
                    });
                }
            },
            goMarkers(latitude, longitude) {
                this.longitude = longitude;
                this.latitude = latitude;
            }
        }
    };
</script>

<style scoped lang="scss">
    view { box-sizing: border-box; }
    .content { width: 100%; /* #ifdef H5 */ height: calc(100vh - 44px); /* #endif */ }
    .map-body { width: 100%; height: 700rpx; position: relative; }
    .store-list { background-color: #fff; height: calc(100vh - 44px - 700rpx); }

    /*    .cu-list { margin: 20rpx; }
        .cu-list.menu-avatar > .cu-item { height: auto; }
            .cu-list.menu-avatar > .cu-item > .cu-avatar { position: absolute; left: 15rpx; }
            .cu-list.menu-avatar > .cu-item .content { position: initial; width: calc(100% - 96rpx - 120rpx - 20rpx); }*/
</style>
