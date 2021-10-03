<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="提货单列表"></u-navbar>
        <view v-if="ladingList.length>0">
            <view class="orderList" :class="item.status?' grayscale':''" v-for="(item, orderIndex) in ladingList" :key="orderIndex">
                <view class="top">
                    <view class="left" @click="doCopyData(item.id)">
                        <u-icon name="tags" :size="30" color="rgb(94,94,94)"></u-icon>
                        <view class="u-margin-left-20 u-margin-right-20 u-font-md ">提货码：{{item.id}}</view>
                        <u-tag text="复制" type="success" size="mini" />
                    </view>
                    <view class="right">{{item.statusName}}</view>
                </view>
                <view class="item" v-for="(v, k) in item.orderItems" :key="k">
                    <view class="left"><image :src="v.imageUrl && v.imageUrl!='null' ?  v.imageUrl : '/static/images/common/empty-banner.png'" mode="aspectFill"></image></view>
                    <view class="content">
                        <view class="title u-line-2">{{v.name}}</view>
                        <view class="type u-line-2">{{v.addon}}</view>
                        <view class="success u-font-24 u-margin-top-10  u-margin-bottom-10 coreshop-text-yellow">订单号：{{v.orderId}}</view>
                    </view>
                    <view class="right">
                        <view class="price">￥{{ v.price }}</view>
                        <view class="number">x{{ v.nums }}</view>
                    </view>
                </view>
                <view class="bottom u-margin-0">
                    <view class="more">
                        下单时间：{{ $u.timeFormat(item.createTime, 'mm-dd hh:MM:ss') }}
                    </view>
                    <view class='logistics coreshop-btn' v-if="item.status == true" @click="ladingDel(item.id)">删除</view>
                    <view class='evaluate coreshop-btn' v-if="item.status == false" @click="ladingWrite(item.id)">提货单核销</view>
                </view>
            </view>
            <!-- 更多 -->
            <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
        </view>
        <!-- 无数据时默认显示 -->
        <view class="coreshop-emptybox" v-else>
            <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="暂无已核销提货单记录" mode="list"></u-empty>
        </view>

    </view>
</template>

<script>
    import { tools } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [tools],
        data() {
            return {
                page: 1,
                limit: 10,
                ladingList: [],
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                }
            }
        },
        onShow() {
            this.page = 1;
            this.ladingList = [];
            this.getLadingList();
        },
        methods: {
            //获取提货单列表
            getLadingList() {
                let _this = this
                let data = {
                    page: _this.page,
                    limit: _this.limit
                }
                _this.status = 'loading';
                this.$u.api.storeLadingList(data).then(res => {
                    if (res.status) {
                        _this.ladingList = [..._this.ladingList, ...res.data]
                        // 判断数据是否加载完毕
                        if (_this.page < res.otherData.totalPages) {
                            _this.page++
                            _this.status = 'loadmore'
                        } else {
                            _this.status = 'nomore'
                        }
                    } else {
                        // 接口請求出錯
                        _this.$u.toast(res.msg)
                        _this.status = 'loadmore'
                    }
                });
            },
            //提货单核销
            ladingWrite(id) {
                this.$u.route('/pages/member/merchant/takeDelivery/index?id=' + id);
            },
            //删除
            ladingDel(id) {
                let _this = this
                this.$common.modelShow('提示', '删除提货单后将无法找回！', res => {
                    let data = {
                        'id': id
                    }
                    _this.$u.api.ladingDel(data).then(res => {
                        _this.$refs.uToast.show({
                            title: res.msg, type: 'success', callback: function () {
                                _this.page = 1;
                                _this.ladingList = [];
                                _this.getLadingList();
                            }
                        })
                    });
                });
            }
        },
        // 页面滚动到底部触发事件
        onReachBottom() {
            let _this = this
            if (_this.status === 'loadmore') {
                _this.getLadingList()
            }
        }
    }
</script>

<style lang="scss" scoped>
</style>