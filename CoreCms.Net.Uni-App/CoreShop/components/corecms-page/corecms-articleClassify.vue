<template>
    <view class='index-article cell-group bottom-cell-group' v-if="corecmsdata.parameters.list && count">
        <view class='cell-item'
              v-for="item in corecmsdata.parameters.list"
              :key="item.id"
              @click="articleDetail(item.id)">
            <view class="cell-item-bd">
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
        name: "corecmsarticleclassify",
        props: {
            corecmsdata: {
                // type: Array,
                required: true,
            }
        },
        computed: {
            count() {
                if (!this.corecmsdata.parameters.list) {
                    return false;
                }
                return (this.corecmsdata.parameters.list.length > 0)
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

<style lang="scss" scoped>
    .index-article { border-radius: 16rpx; padding: 0rpx 25rpx; background: #FFFFFF !important; color: #333333 !important; margin: 20rpx 20rpx; overflow: hidden; }
    .cell-item { padding: 10rpx 0rpx 10rpx 0; float: left; width: 100%; border-bottom: 2rpx solid #f3f3f3; }
    .index-article .cell-item-bd { float: left; width: calc(100% - 200rpx); height: 90rpx; margin-top: 20rpx; }
    .index-article .cell-title-img { width: 160rpx; height: 160rpx; float: right; }
    .coverImage { width: 100%; height: 100%; }
    .index-article .article-title { font-size: 28rpx; color: #333; width: 100%; overflow: hidden; float: left; margin-bottom: 10rpx; }
    .index-article .article-des { font-size: 24rpx; color: #999; overflow: hidden; float: left; line-height: 40rpx; }
</style>
