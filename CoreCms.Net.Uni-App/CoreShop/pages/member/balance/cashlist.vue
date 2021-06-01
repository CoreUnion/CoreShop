<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="提现明细"></u-navbar>
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
                        <view class="text-grey">{{ item.statusName }}</view>
                        <view class="text-gray text-sm flex">
                            提现卡号：{{ item.cardNumber }}
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
                <u-empty :src="$apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="暂无提现明细" mode="list"></u-empty>
            </view>
        </view>
    </view>

</template>

<script>
    export default {
        data() {
            return {
                objectType: ['全部', '待审核', '提现成功', '提现失败'],
                index: 0,	// 默认选中的类型	索引
                page: 1,
                limit: 10,
                list: [],
                states: [0, 1, 2, 3], // 不同类型的状态
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                }
            }
        },
        onLoad() {
            this.getCash()
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getCash()
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
            getCash() {
                let data = {
                    page: this.page,
                    limit: this.limit
                }

                if (this.states[this.index]) {
                    data.id = this.states[this.index];
                } else {
                    data.id = 0;
                }

                this.status = 'loading'

                this.$u.api.cashList(data).then(res => {
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
                this.getCash()
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
