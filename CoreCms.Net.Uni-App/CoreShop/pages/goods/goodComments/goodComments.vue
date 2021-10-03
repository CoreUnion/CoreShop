<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="评论列表"></u-navbar>
        <view class="u-padding-10">
            <!--评论-->
            <view class="coreshop-bg-white">
                <view v-for="(item, index) in goodsComments" :key="index">
                    <view class="coreshop-solid-bottom u-margin-top-20 u-margin-bottom-20" v-if="index > 0" />
                    <view class="coreshop-common-view-box">
                        <view class="coreshop-flex coreshop-flex-wrap u-font-sm">
                            <view class="coreshop-basis-1">
                                <view class="coreshop-avatar sm round" :style="[{backgroundImage:'url('+ item.avatarImage +')'}]" />
                            </view>
                            <view class="coreshop-basis-9 u-font-sm">
                                <view>{{ (item.nickName && item.nickName != '')?item.nickName:item.mobile }}</view>
                                <view class="u-margin-top-20">{{ item.contentBody || ''}}</view>
                                <view class="coreshop-text-gray u-margin-top-10">
                                    <u-rate :current="item.score" :disabled="true" size="26"></u-rate>
                                </view>
                                <view class="coreshop-text-gray u-margin-top-10">{{ item.createTime || ''}} {{ item.addon || ''}}</view>
                            </view>
                        </view>
                    </view>
                </view>
                <u-loadmore :status="loadStatus" class="u-margin-top-20" :icon-type="iconType" :load-text="loadText" />
            </view>
        </view>
        <!-- 登录提示 -->
		<coreshop-login-modal></coreshop-login-modal>
    </view>
</template>

<script>
    export default {
        data() {
            return {
                goodsId: 0,
                goodsComments: [],
                page: 1,
                limit: 10,
                page: 1,
                pageSize: 20,
                loadStatus: 'loadmore',
                loadIconType: 'flower',
                loadText: {
                    loadmore: '点击或上拉加载更多',
                    loading: '正在加载中',
                    nomore: '没有更多了'
                },
            }
        },
        onLoad(options) {
            this.goodsId = options.id;
            this.getGoodsComments();
        },
        onReachBottom() {
            if (this.loadStatus != 'nomore') {
                this.getGoodsComments();
            }
        },
        methods: {
            // 获取商品评论信息
            getGoodsComments() {
                var _that = this;
                let data = {
                    page: _that.page,
                    limit: _that.limit,
                    id: _that.goodsId,
                }
                this.loadStatus = 'loading';
                this.$u.api.goodsComment(data).then(res => {
                    if (res.status == true) {
                        let _list = res.data.list;
                        // 如果评论没有图片 在这块作处理否则控制台报错
                        _list.forEach(item => {
                            if (!item.hasOwnProperty('images')) {
                                _that.$set(item, 'images', [])
                            }
                            _that.goodsComments.push(item);
                        });
                        // 根据count数量判断是否还有数据
                        if (res.data.totalPages > _that.page) {
                            _that.loadStatus = 'loadmore';
                            _that.page++;
                        } else {
                            // 数据已加载完毕
                            this.loadStatus = 'nomore';
                        }
                    } else {
                        _that.$refs.uToast.show({ title: res.msg, type: 'error' });
                    }
                })
            },
        }
    }
</script>

<style lang="scss" scoped>

</style>
