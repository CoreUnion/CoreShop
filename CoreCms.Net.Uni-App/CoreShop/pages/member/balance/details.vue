<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="余额明细"></u-navbar>
        <view class="content">
            <view class="cu-form-group">
                <view class="title">类型筛选</view>
                <picker @change="changeState" :value="index" :range="objectType">
                    <view class="picker">
                        {{index>-1?objectType[index]:'禁止换行，超出容器部分会以 ... 方式截断'}}
                    </view>
                </picker>
            </view>

            <view class="cu-list menu-avatar sm-border card-menu margin-top" v-if="list.length>0">
                <view class="cu-item" v-for="(item, index) in list" :key="index">
                    <view class="content">
                        <view class="text-grey">{{ item.typeName }}</view>
                        <view class="text-gray text-sm flex">
                            余额：{{ item.balance }}
                        </view>
                    </view>
                    <view class="action">
                        <view class="text-grey text-sm"> {{ item.createTime }}</view>
                        <view class="text-red text-price text-xl"> {{ item.money }}</view>
                    </view>
                </view>
                <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无余额明细" mode="list"></u-empty>
            </view>
        </view>
    </view>

</template>

<script>
    export default {
        data() {
            return {
                objectType: ['全部', '消费', '退款', '充值', '提现', '佣金', '平台调整', '奖励'],
                index: 0,	// 默认选中的类型	索引
                page: 1,
                limit: 10,
                list: [],
                states: [0, 1, 2, 3, 4, 5, 6, 7], // 不同类型的状态
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                }
            }
        },
        onLoad(e) {
            if (e.status) {
                this.index = this.states.indexOf(parseInt(e.status));
            } else {
                this.balances()//修复多次加载问题
            }
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.balances()
            }
        },
        methods: {
            // 切换类型
            changeState(e) {
                if (this.index !== e.target.value) {
                    this.index = e.target.value;
                    this.page = 1
                    this.list = []
                }
            },
            // 获取余额明细
            balances() {
                let data = {
                    id: this.states[this.index],
                    page: this.page,
                    limit: this.limit
                }

                this.status = 'loading'

                this.$u.api.getBalanceList(data).then(res => {
                    if (res.status) {
                        if (this.page >= res.otherData.totalPages) {
                            // 没有数据了
                            this.status = 'nomore'
                        } else {
                            // 未加载完毕
                            this.status = 'loadmore'
                            this.page++
                        }
                        this.list = [...this.list, ...res.data]
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            }
        },
        watch: {
            index() {
                this.balances();
            }
        }
    }
</script>

<style lang="scss" scoped>
    .cu-form-group picker .picker { text-align: right; }
    .cu-list.menu-avatar > .cu-item .action { width: auto; }
    .cu-list.menu-avatar > .cu-item .content { position: absolute; left: 20rpx; width: calc(100% - 96rpx - 60rpx - 120rpx - 20rpx); line-height: 1.6em; }

    .cu-list.menu-avatar > .cu-item { padding-right: 20rpx; }
</style>
