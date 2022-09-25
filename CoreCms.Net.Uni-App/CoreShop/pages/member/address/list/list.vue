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
                <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/address.png'" icon-size="300" text="暂无地址信息" mode="list"></u-empty>
            </view>
            <view class="coreshop-bottomBox">
                <button class="coreshop-btn coreshop-btn-b coreshop-btn-square" @click="toAdd()">新增收货地址</button>
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
                this.$u.route('/pages/member/address/index/index?shipId=' + id);
            },
            //添加
            toAdd() {
                this.$u.route('/pages/member/address/index/index');
            },
            //选择
            isSelect(data) {
                if (this.type == 'order') {
                    let pages = getCurrentPages();//当前页
                    let beforePage = pages[pages.length - 2];//上个页面

                    beforePage.$vm.userShip = data;
                    beforePage.$vm.params.areaId = data.areaId;

                    uni.navigateBack({
                        delta: 1
                    });
                    // this.$u.route("/pages/placeOrder/index/index")
                }
            },
        }
    }
</script>

<style lang="scss" scoped>
    @import "list.scss";
</style>