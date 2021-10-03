<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="余额明细"></u-navbar>
        <view class="u-flex u-flex-nowrap u-row-between u-padding-30 coreshop-bg-white">
            <view class="title">类型筛选</view>
            <picker @change="changeState" :value="index" :range="objectType">
                <view class="picker">
                    {{index>-1?objectType[index]:''}}
                    <u-icon name="arrow-right-double" margin-left="20rpx" class="u-margin-left-20"></u-icon>
                </view>
            </picker>
        </view>
        <view class="u-margin-top-30  u-margin-left-30 u-margin-right-30 radius">
            <view v-if="list.length>0">
                <view class="coreshop-log-item u-flex u-row-between" v-for="item in list" :key="item.id">
                    <view class="item-left coreshop-flex coreshop-align-center">
                        <view class="coreshop-flex coreshop-flex-direction coreshop-align-start">
                            <view class="log-name coreshop-text-black">{{ item.typeName }}</view>
                            <view class="log-notice coreshop-text-grey">余额：{{ item.balance }}</view>
                        </view>
                    </view>
                    <view class="item-right coreshop-flex coreshop-flex-direction coreshop-align-end">
                        <view class="log-num coreshop-text-red">{{ item.money }}</view>
                        <view class="log-date coreshop-text-grey">{{ item.createTime }}</view>
                    </view>
                </view>
                <!-- 更多 -->
                <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
            </view>
            <!-- 无数据时默认显示 -->
            <view class="coreshop-emptybox" v-else>
                <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无余额明细" mode="list"></u-empty>
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

<style lang="scss">
</style>
