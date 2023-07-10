<template>
    <view>
        <u-navbar title="门店列表"></u-navbar>
        <view class="content">
            <view class="u-padding-15 coreshop-bg-white u-border-bottom">
                <u-search placeholder="请输入门店名" v-model="key" :show-action="true" action-text="搜索" :animation="false" @clear="clear" @search="storeSearch" @custom="storeSearch"></u-search>
            </view>

            <view v-if="storeList.length>0">
                <view class="coreshop-list menu-avatar" v-for="(item, key) in storeList" :key="key" @click="selectStore(item.id, item.storeName, item.mobile, item.address)">
                    <view class="coreshop-list-item  u-padding-top-20  u-padding-bottom-20">
                        <view class="coreshop-avatar lg radius" :style="[{backgroundImage:'url('+ item.logoImage +')'}]" />
                        <view class="content">
                            <view class="coreshop-text-grey">{{item.storeName|| ''}}</view>
                            <view class="coreshop-text-gray u-font-sm">
                                <view class="u-line-1">
                                    电话：{{item.mobile|| ''}}
                                </view>
                            </view>
                            <view class="coreshop-text-gray u-font-sm">
                                <view class="u-line-2">
                                    地址：{{item.address|| ''}}
                                </view>
                            </view>
                        </view>
                        <view class="action">
                            <view class="coreshop-text-grey u-font-xs u-margin-bottom-20">{{item.distanceStr|| ''}}</view>
                            <u-button type="success" size="mini">选择</u-button>
                        </view>
                    </view>
                </view>
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无门店列表" mode="list"></u-empty>
            </view>
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
                key: '',
                longitude: '',
                latitude: '',
                page: 1,
                limit: 30,
            }
        },
        onShow() {
            this.getStoreList();
        },
        methods: {
            //门店搜索
            storeSearch() {
                console.log('storeSearch');
                this.page = 1;
                this.limit = 30;
                this.storeList = [];
                this.getStoreList();
            },
            clear() {
                console.log('clear');
                this.page = 1;
                this.limit = 30;
                this.storeList = [];
                this.getStoreList();
            },
            //获取门店列表
            getStoreList() {
                let _this = this;
                wx.getFuzzyLocation({
                    type: 'wgs84',
                    success(res) {
                        _this.latitude = res.latitude
                        _this.longitude = res.longitude
                        console.log('当前位置的经度1：' + res);
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
            //门店选择
            selectStore(id, name, mobile, address) {
                let pages = getCurrentPages()
                let pre = pages[pages.length - 2]
                let store = {};
                store['id'] = id;
                store['name'] = name;
                store['mobile'] = mobile;
                store['address'] = address;

                pre.$vm.store = store


                uni.navigateBack({
                    delta: 1
                });
            }
        }
    }
</script>

<style lang="scss">
    @import "storeList.scss";
</style>
