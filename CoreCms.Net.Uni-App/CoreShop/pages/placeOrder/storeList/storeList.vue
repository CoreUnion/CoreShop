<template>
    <view>
        <u-navbar title="门店列表"></u-navbar>
        <view class="content">
            <view class="u-padding-15 bg-white u-border-bottom">
                <u-search placeholder="请输入门店名" v-model="key" :show-action="true" action-text="搜索" :animation="false" @search="storeSearch" @custom="storeSearch"></u-search>
            </view>

            <view v-if="storeList.length>0">
                <view class="cu-list menu-avatar" v-for="(item, key) in storeList" :key="key" @click="selectStore(item.id, item.storeName, item.mobile, item.address)">
                    <view class="cu-item  u-padding-top-20  u-padding-bottom-20">
                        <view class="cu-avatar lg radius" :style="[{backgroundImage:'url('+ item.logoImage +')'}]" />
                        <view class="content">
                            <view class="text-grey">{{item.storeName|| ''}}</view>
                            <view class="text-gray text-sm flex">
                                <view class="text-cut">
                                    电话：{{item.mobile|| ''}}
                                </view>
                            </view>
                            <view class="text-gray text-sm flex">
                                <view class="u-line-2">
                                    地址：{{item.address|| ''}}
                                </view>
                            </view>
                        </view>
                        <view class="action">
                            <view class="text-grey text-xs u-margin-bottom-20">{{item.distanceStr|| ''}}</view>
                            <view class="cu-tag round bg-grey sm">点击选择</view>
                        </view>
                    </view>
                </view>
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无门店列表" mode="list"></u-empty>
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
                this.getStoreList();
            },
            //获取门店列表
            getStoreList() {
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
            //门店选择
            selectStore(id, name, mobile, address) {
                let pages = getCurrentPages()
                let pre = pages[pages.length - 2]
                let store = {};
                store['id'] = id;
                store['name'] = name;
                store['mobile'] = mobile;
                store['address'] = address;

                // #ifdef MP-ALIPAY || MP-TOUTIAO
                this.$db.set('userStore', store, true);
                // #endif

                // #ifdef MP-WEIXIN
                pre.$vm.store = store
                // #endif

                // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE
                pre.store = store
                // #endif

                uni.navigateBack({
                    delta: 1
                });
            }
        }
    }
</script>

<style lang="scss">
    .cu-list { margin: 20rpx; }
        .cu-list.menu-avatar > .cu-item { height: auto; }
            .cu-list.menu-avatar > .cu-item > .cu-avatar { position: absolute; left: 15rpx; }
            .cu-list.menu-avatar > .cu-item .content { position: initial; width: calc(100% - 96rpx - 120rpx - 20rpx); }
</style>
