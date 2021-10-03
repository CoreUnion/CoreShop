<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="收藏商品"></u-navbar>

        <view v-if="list.length">
            <u-swipe-action :show="item.show" :index="index" v-for="(item, index) in list" :key="item.id" @click="collect" @open="open" :options="options">
                <view class="u-padding-20 u-padding-bottom-0 u-border-bottom coreshop-display-flex" @click="goGoodsDetail(item.goodsId)">
                    <u-avatar :src="item.goods.image" mode="square" size="120" class="u-margin-10"></u-avatar>
                    <!-- 此层wrap在此为必写的，否则可能会出现标题定位错误 -->
                    <view class="title-wrap">
                        <text class="u-text-left title u-line-2">{{ item.goods.name }}</text>
                        <view class="u-margin-top-10">
                            <u-icon name="clock" class="u-margin-right-6"></u-icon><text class="u-text-left desc u-line-1">{{ item.createTime }}</text>
                        </view>
                    </view>
                </view>
            </u-swipe-action>
            <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
        </view>
        <!-- 无数据时默认显示 -->
        <view class="coreshop-emptybox" v-else>
            <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/collect.png'" icon-size="300" text="暂无收藏" mode="list"></u-empty>
        </view>
    </view>

</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [goods],
        data() {
            return {
                disabled: false,
                btnWidth: 180,
                show: false,
                options: [
                    {
                        text: '取消',
                        style: {
                            backgroundColor: '#dd524d'
                        }
                    }
                ],
                page: 1,
                limit: 10,
                list: [], // 商品浏览足迹
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
            this.goodsCollectionList()
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.goodsCollectionList()
            }
        },
        methods: {
            goodsCollectionList() {
                let data = {
                    page: this.page,
                    limit: this.limit
                }
                this.status = 'loading '
                this.$u.api.goodsCollectionList(data).then(res => {
                    if (res.status) {
                        let _list = res.data.list;
                        _list.forEach(item => {
                            item.show = false;
                        })
                        this.list = [...this.list, ..._list]

                        if (res.data.count > this.list.length) {
                            this.page++
                            this.status = 'loadmore'
                        } else {
                            this.status = 'nomore'
                        }
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
            // 取消收藏
            collect(index, index1) {
                let _that = this;
                let data = {
                    id: _that.list[index].goodsId
                }
                this.$u.api.goodsCollection(data).then(res => {
                    if (res.status) {
                        _that.$refs.uToast.show({
                            title: res.msg, type: 'success', callback: function () {
                                _that.$nextTick(() => {
                                    _that.list.splice(index, 1)
                                })
                            }
                        })
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            },
            // 如果打开一个的时候，不需要关闭其他，则无需实现本方法
            open(index) {
                // 先将正在被操作的swipeAction标记为打开状态，否则由于props的特性限制，
                // 原本为'false'，再次设置为'false'会无效
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