<template>
    <view class="orderWrap">
        <u-navbar title="售后列表"></u-navbar>

        <view v-if="order.length>0">
            <view class="orderList" v-for="(item, key) in order" :key="key">
                <view class="top">
                    <view class="left" @click="doCopyData(item.aftersalesId)">
                        <u-icon name="home" :size="30" color="rgb(94,94,94)"></u-icon>
                        <view class="store">售后单号：{{item.aftersalesId}}</view>
                        <u-icon name="arrow-right" color="rgb(203,203,203)" :size="26"></u-icon>
                    </view>
                    <view class="right" v-if="item.status == 1">待审核</view>
                    <view class="right" v-else-if="item.status == 2">审核通过</view>
                    <view class="right" v-else-if="item.status == 3">审核拒绝</view>
                </view>
                <view class="item" v-for="(v, k) in item.items" :key="k" @click="showOrder(item.aftersalesId)">
                    <view class="left"><image :src="v.imageUrl && v.imageUrl!='null' ?  v.imageUrl+'?x-oss-process=image/resize,m_lfit,h_320,w_240' : '/static/images/common/empty-banner.png'" mode="aspectFill"></image></view>
                    <view class="content">
                        <view class="title u-line-2">{{v.name}}</view>
                        <view class="type">{{v.addon}}</view>
                    </view>
                    <view class="right">
                        <view class="number">x{{ v.nums }}</view>
                    </view>
                </view>
                <!-- 订单底部 -->
                <view class="bottom">
                    <view class="more">退款金额：￥{{item.refundAmount}}</view>
                    <view class='logistics coreshop-btn'  @click="showOrder(item.aftersalesId)">查看详情</view>
                </view>
            </view>
            <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
        </view>

        <!-- 无数据时默认显示 -->
        <view class="coreshop-emptybox" v-else>
            <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/order.png'" icon-size="300" text="暂无提现明细" mode="list"></u-empty>
        </view>

    </view>
</template>

<script>
    import { tools } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [tools],
        data() {
            return {
                order: [], //订单列表
                page: 1, //当前页
                limit: 10, //每页显示几条
                orderId: "",
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
            this.getOrderList();
        },
        onLoad(e) {
            this.order = [];
            this.orderId = e.orderId;
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getOrderList()
            }
        },
        methods: {
            //获取订单数据
            getOrderList() {
                let data = {};
                this.status = 'loading'
                data['page'] = this.page;
                data['limit'] = this.limit;
                if (this.orderId) {
                    data['id'] = this.orderId;
                }
                this.$u.api.afterSalesList(data).then(res => {
                    let orderList = this.dataFormat(res.data.list);
                    this.order = this.order.concat(orderList);
                    this.page = res.data.page * 1 + 1;
                    let allpage = res.data.totalPage;
                    if (allpage < this.page) {
                        this.status = 'nomore'
                    } else {
                        this.status = 'loadmore'
                    }
                });
            },

            //数据格式处理
            dataFormat(data) {
                for (var i = 0; i < data.length; i++) {
                    let countnum = 0
                    if (data[i].order && data[i].order.items) {
                        for (var j = 0; j < data[i].order.items.length; j++) {
                            countnum += data[i].order.items[j].nums;
                        }
                        data[i].countnum = countnum;
                    }
                }
                return data;
            },

            //查看详情
            showOrder(aftersalesId) {
                this.$u.route('/pages/member/afterSales/detail/detail?aftersalesId=' + aftersalesId);
            }
        },
    }
</script>

<style lang="scss" scoped>

</style>
