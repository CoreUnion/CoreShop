<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="已核销服务码"></u-navbar>
        <view v-if="logs.length>0">
            <view class="orderList" v-for="(item, key) in logs" :key="key">
                <view class="top">
                    <view class="left">
                        <u-icon name="order" :size="30" color="rgb(94,94,94)"></u-icon>
                        <view class="u-margin-left-20 u-margin-right-20 u-font-md">订单编号：{{item.ticket.serviceOrderId}}</view>
                    </view>
                    <view class="right">
                        <u-tag text="复制核销码" type="success" size="mini" @click="doCopyData(item.ticket.redeemCode)" />
                    </view>
                </view>
                <view class="item" v-if="item.service">
                    <view class="left"><image :src="item.service.thumbnail" mode="aspectFill"></image></view>
                    <view class="content">
                        <view class="title u-line-2">{{item.service.title}}</view>
                        <view class="type u-line-2">{{item.service.description}}</view>
                        <view class="success u-font-24 u-margin-top-10  u-margin-bottom-10 coreshop-text-yellow">核销时间：{{item.ticket.verificationTime}}</view>
                        <view class="success u-font-24 u-margin-top-10  u-margin-bottom-10 coreshop-text-yellow">核销码：{{item.ticket.redeemCode}}</view>
                    </view>
                </view>
                <view class="bottom u-margin-0">
                    <view class="more">
                        下单时间：{{ $u.timeFormat(item.ticket.createTime, 'mm-dd hh:MM:ss') }}
                    </view>
                    <view class='logistics coreshop-btn' @click="logsDel(item.id)">删除</view>
                </view>
            </view>
            <!-- 更多 -->
            <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
        </view>
        <!-- 无数据时默认显示 -->
        <view class="coreshop-emptybox" v-else>
            <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="暂无已核销服务码记录" mode="list"></u-empty>
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
                logs: [],
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
            this.getlogs();
        },
        methods: {
            //获取提货单列表
            getlogs() {
                let _this = this
                let data = {
                    page: _this.page,
                    limit: _this.limit
                }
                _this.status = 'loading';
                this.$u.api.getverificationPageList(data).then(res => {
                    if (res.status) {
                        _this.logs = [..._this.logs, ...res.data]
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
            //删除
            logsDel(id) {
                let _this = this
                _this.$common.modelShow('提示', '删除核验单后将无法找回！', res => {
                    let data = {
                        'id': id
                    }
                    _this.$u.api.serviceLogDelete(data).then(res => {
                        _this.$refs.uToast.show({
                            title: res.msg, type: 'success', callback: function () {
                                _this.getlogs();
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
                _this.getlogs()
            }
        }
    }
</script>

<style lang="scss" scoped>

</style>