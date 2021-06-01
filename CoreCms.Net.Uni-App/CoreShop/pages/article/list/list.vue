<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :title="typeName"></u-navbar>
        <view class="help-bg bg-red"></view>
        <view class="help-body">
            <view class="help-h3">帮助中心</view>
            <u-tabs :list="articleType" :is-scroll="false" :current="current" @change="change"></u-tabs>
            <view class="groupBox">
                <u-cell-group class="u-padding-top-20 u-padding-bottom-20">
                    <u-cell-item icon="list-dot" :title="item.title" v-for="item in list" :key="item.id" @click="goArticleDetail(item.id)"></u-cell-item>
                </u-cell-group>
                <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="0" margin-bottom="20" class="u-padding-top-20" />
            </view>
        </view>
        <!-- 登录提示 -->
        <corecms-login-modal></corecms-login-modal>
    </view>
</template>

<script>
    import { articles } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [articles],
        data() {
            return {
                cid: 0, // 文章分类id
                page: 1,
                limit: 10,
                list: [],
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                },
                articleType: [],
                typeName: '',
                current: 0
            };
        },
        onLoad(options) {
            if (options.cid) {
                this.cid = options.cid;
            } else {
                this.cid = 1;
            }

            this.articleList();
            if (options.current) {
                this.current = options.current;
            }
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.articleList();
            }
        },
        methods: {
            articleList() {
                let data = {
                    page: this.page,
                    limit: this.limit,
                    id: this.cid
                };
                this.status = 'loading';
                this.$u.api.articleList(data).then(res => {
                    if (res.status) {
                        this.articleType = res.data.articleType;
                        this.typeName = res.data.typeName;
                        const _list = res.data.list;
                        this.list = [...this.list, ..._list];
                        if (res.data.count > this.list.length) {
                            this.status = 'loadmore';
                            this.page++;
                        } else {
                            // 数据已加载完毕
                            this.status = 'nomore';
                        }
                    } else {
                        // 接口请求出错了
                        this.$u.toast(res.msg);
                    }
                });
            },
            change(index) {
                this.current = index;
                this.cid = this.articleType[index].id;
                this.list = [];
                this.page = 1;
                this.limit = 10;
                this.loadStatus = 'more';
                this.articleList();
            }
        }
    };
</script>

<style lang="scss" scoped>
    .help-bg { height: 350rpx; background-image: url('/static/images/common/bg.png'); background-size: cover; background-position: center; border-radius: 0 0 50rpx 50rpx; }
    .help-body { margin-top: -300rpx; padding: 25rpx; }
    .help-h3 { font-size: 45rpx; color: #FFFFFF !important; margin: 20rpx 20rpx; }
    .groupBox { background-color: #fff; min-height: 150rpx; }
</style>
