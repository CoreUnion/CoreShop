<template>
    <view class='coreshop-index-article coreshop-cell-group u-margin-bottom-20' v-if="coreshopdata.parameters.list && count">
        <u-toast ref="uToast" />
        <view class='coreshop-cell-item' v-for="item in coreshopdata.parameters.list" :key="item.id" @click="articleDetail(item.id)">
            <view class="coreshop-cell-item-bd">
                <view class="article-title ">
                    {{ item.title }}
                </view>
                <view class="article-des u-line-2">
                    {{ item.brief }}
                </view>
            </view>
            <view class="cell-title-img">
                <image :src="item.coverImage" mode="aspectFill" class="coverImage"></image>
            </view>
        </view>
    </view>
</template>

<script>
    export default {
        name: "coreshoparticle",
        props: {
            coreshopdata: {
                // type: Array,
                required: true,
            }
        },
        computed: {
            count() {
                return (this.coreshopdata.parameters.list.length > 0)
            }
        },
        methods: {
            // 查看文章详情
            articleDetail(articleId) {
                this.$u.route('/pages/article/details/details?id=' + articleId + '&idType=1')
            }
        }
    }
</script>

<style scoped lang="scss">
    .coreshop-index-article { border-radius: 16rpx; padding: 0rpx 25rpx; background: #FFFFFF !important; color: #333333 !important; margin: 20rpx 20rpx; overflow: hidden;
        .coreshop-cell-item { padding: 10rpx 0rpx 10rpx 0; float: left; width: 100%; border-bottom: 2rpx solid #f3f3f3;
            .coreshop-cell-item-bd { float: left; width: calc(100% - 200rpx); margin-top: 20rpx; display: flex; flex-direction: column; align-items: flex-start;
                .article-title { font-size: 28rpx; color: #333; width: 100%; overflow: hidden; float: left; margin-bottom: 10rpx; }
                .article-des { font-size: 24rpx; color: #999; overflow: hidden; float: left; line-height: 40rpx; }
            }
            .cell-title-img { width: 160rpx; height: 160rpx; float: right;
                .coverImage { width: 100%; height: 100%; }
            }
        }
    }
</style>
