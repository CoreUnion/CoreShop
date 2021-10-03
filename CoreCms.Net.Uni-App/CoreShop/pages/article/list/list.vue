<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :custom-back="goBack" :title="typeName"></u-navbar>
        <view class="help-bg coreshop-bg-red"></view>
        <view class="help-body">
            <view class="help-h3">帮助中心</view>
            <u-tabs :list="articleType" :is-scroll="false" :current="current" @change="change"></u-tabs>

            <u-card :title="articleType[current].name">
                <view class="" slot="body">
                    <view class="u-body-item u-flex u-border-bottom u-row-between u-flex-nowrap" v-for="item in list" :key="item.id" @click="goArticleDetail(item.id)">
                        <view class="u-flex u-flex-nowrap">
                            <u-image width="50rpx" height="50rpx" :src="item.coverImage" mode="aspectFill" class="u-margin-right-10"></u-image>
                            <view class="u-body-item-title u-line-2">{{item.title}}</view>
                        </view>
                        <view class="coreshop-text-gray u-text-right coreshop-justify-end">
                            <u-icon name="arrow-right-double"></u-icon>
                        </view>
                    </view>
                </view>
                <view class="" slot="foot">
                    <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="0" margin-bottom="20" class="u-padding-top-20" />
                </view>
            </u-card>
        </view>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>

<script>
    import { articles, tools } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [articles, tools],
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
                this.cid = Number(options.cid);
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
                        for (var i = 0; i < this.articleType.length; i++) {
                            if (this.cid === this.articleType[i].id) {
                                this.current = i;
                            }
                        }
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
    @import "list.scss";
</style>
