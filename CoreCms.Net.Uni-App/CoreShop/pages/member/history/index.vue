<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="我的足迹"></u-navbar>
        <view v-if="list.length">
            <u-swipe-action :show="item.show" :index="index" v-for="(item, index) in list" :key="item.id" @click="click" @open="open" :options="options">
                <view class="u-padding-10 u-border-bottom coreshop-display-flex" @click="goGoodsDetail(item.goodsId)">
                    <u-avatar :src="item.goodImage" mode="square" size="120" class="u-margin-10"></u-avatar>
                    <!-- 此层wrap在此为必写的，否则可能会出现标题定位错误 -->
                    <view class="title-wrap">
                        <text class="u-text-left title  u-line-2">{{ item.goodsName }}</text>
                        <view class="u-margin-top-10">
                            <u-icon name="clock" class="u-margin-right-6"></u-icon><text class="u-text-left desc u-line-1">{{ item.createTime }}</text>
                        </view>
                    </view>
                </view>
            </u-swipe-action>
            <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" />
        </view>
        <!-- 无数据时默认显示 -->
        <view class="coreshop-emptybox" v-else>
            <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/collect.png'" icon-size="300" text="暂无足迹" mode="list"></u-empty>
        </view>
    </view>

</template>


<script>
    import { goods } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [goods],
        data() {
            return {
                page: 1,
                limit: 10,
                list: [], // 商品浏览足迹
                loadStatus: 'more',
                options: [
                    {
                        text: '收藏',
                        style: {
                            backgroundColor: '#007aff'
                        }
                    },
                    {
                        text: '删除',
                        style: {
                            backgroundColor: '#dd524d'
                        }
                    }
                ],
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                }
            };
        },
        onLoad() {
            this.goodsBrowsing()
        },
        onReachBottom() {
            if (this.status === 'loading') {
                this.goodsBrowsing();
            }
        },
        methods: {
            goodsBrowsing() {
                let data = {
                    page: this.page,
                    limit: this.limit
                }
                this.status = 'loading'
                this.$u.api.goodsBrowsing(data).then(res => {
                    if (res.status) {
                        let _list = res.data.list
                        _list.forEach(item => {
                            item.show = false;
                        })
                        this.list = [...this.list, ..._list]
                        if (res.data.count > this.list.length) {
                            this.page++
                            this.status = 'loading'
                        } else {
                            this.status = 'nomore'
                        }
                        console.log(res.data);
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
            click(index, index1) {
                let _this = this;
                if (index1 == 0) {
                    let data = {
                        id: _this.list[index].goodsId
                    }
                    _this.$u.api.goodsCollection(data).then(res => {
                        if (res.status) {
                            _this.$refs.uToast.show({
                                title: res.msg, type: 'success', callback: function () {
                                    _this.$nextTick(() => {
                                        _this.list[index].isCollection = !this.list[index].isCollection
                                    })
                                }
                            })
                        } else {
                            _this.$u.toast(res.msg)
                        }
                    })
                } else if (index1 == 1) {
                    let data = {
                        id: _this.list[index].goodsId
                    }
                    _this.$u.api.delGoodsBrowsing(data).then(res => {
                        if (res.status) {
                            _this.list.splice(index, 1)
                            _this.$refs.uToast.show({ title: res.msg, type: 'success' })
                        } else {
                            _this.$u.toast(res.msg)
                        }
                    })

                }
            },
            // 如果打开一个的时候，不需要关闭其他，则无需实现本方法
            open(index) {
                this.list[index].show = true;
                this.list.map((val, idx) => {
                    if (index != idx) this.list[idx].show = false;
                })
            }
        }
    };
</script>

<style lang="scss" scoped>
    @import "index.scss";
</style>