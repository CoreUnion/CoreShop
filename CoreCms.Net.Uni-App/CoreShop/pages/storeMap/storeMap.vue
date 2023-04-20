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
                    <view class="coreshop-list menu-avatar" v-for="(item, key) in storeList" :key="key">
                        <view class="coreshop-list-item">
                            <image class="coreshop-avatar lg radius" :src="item.logoImage" mode="aspectFill"></image>
                            <view class="content">
                                <view class="coreshop-text-grey u-margin-bottom-10">{{item.storeName|| ''}}</view>
                                <view class="coreshop-text-gray u-font-sm flex u-margin-bottom-10">
                                    <view class="u-line-2" @tap="doPhoneCall(item.mobile)">
                                        <u-icon name="phone" size="24" :label="item.mobile"></u-icon>
                                    </view>
                                </view>
                                <view class="coreshop-text-gray u-font-sm flex u-margin-bottom-10">
                                    <view class="u-line-2 u-font-xs">
                                       地址：{{item.address}}
                                    </view>
                                </view>
                            </view>
                            <view class="action" @click="goMarkers(item.latitude,item.longitude)">
                                <view class="coreshop-text-grey u-font-xs u-margin-bottom-20">{{item.distanceStr|| ''}}</view>
                                <u-tag text="点击查看" type="success" size="mini"/>
                            </view>
                        </view>
                    </view>
                </view>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无门店列表" mode="list"></u-empty>
                </view>

            </scroll-view>
        </view>
        <!-- 登录提示 -->
		<coreshop-login-modal></coreshop-login-modal>
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
                wx.getFuzzyLocation({
                    type: 'wgs84',
                    success(res) {
                        _this.longitude = res.longitude;
                        _this.latitude = res.latitude;
                        _this.getStoreList();
                    },
                    fail: function () {
                        _this.$u.toast("获取您的经纬度坐标失败")
                    },
                    complete: function (res) {
                        if (!_this.longitude || !_this.latitude) {
                            _this.longitude = '0';
                            _this.latitude = '0';
                        }
                        _this.getStoreList();
                    }
                })
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
    @import "storeMap.scss";
</style>
