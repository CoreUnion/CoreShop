<template>
    <view class="u-padding-top-10 u-padding-bottom-10 u-margin-15 bg-white">
        <u-grid :col="limit" :border="false">
            <u-grid-item v-for="(item, index) in ListData" :key="index" @click="showSliderInfo(item.linkType, item.linkValue)">
                <u-icon :name="item.image" width="80" height="80" :label="item.text" :label-size="26" label-pos="bottom" margin-top="25"></u-icon>
            </u-grid-item>
        </u-grid>
    </view>
</template>

<script>
    import { linkType } from '@/common/setting/constVarsHelper.js';
    export default {
        name: "corecmsnavbar",
        components: {},
        props: {
            corecmsdata: {
                // type: Object,
                required: true,
            }
        },
        data() {
            return {
                ListData: [],
                limit: 0,
                multiple: 0,
                count: 0,
                scrollPage: 1,
                scrollDot: 0,
            }
        },
        mounted() {

        },
        created() {
            this.ListData = this.corecmsdata.parameters.list;
            this.limit = this.corecmsdata.parameters.limit;
        },
        methods: {
            showSliderInfo(type, val) {
                console.log(type)
                console.log(val)
                if (!val) {
                    return;
                }
                if (val.indexOf('http') != -1) {
                    // #ifdef H5
                    window.location.href = val
                    // #endif
                } else {
                    // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE || MP
                    if (val == '/pages/index/default/default' || val == '/pages/category/index/index' || val == '/pages/index/cart/cart' || val == '/pages/index/member/member') {
                        this.$u.route({ type: 'switchTab', url: val });
                        return;
                    } else {

                        if (type == this.$config.navLinkType.urlLink) {
                            this.$u.route(val);
                        } else if (type == this.$config.navLinkType.shop) {
                            this.$u.route('/pages/goods/goodDetails/goodDetails', { id: val });
                        }
                        else if (type == this.$config.navLinkType.article) {
                            this.$u.route('/pages/article/details/details', { idType: 1, id: val });
                        }
                        else if (type == this.$config.navLinkType.articleCategory) {
                            this.$u.route('/pages/article/list/list')
                        }
                        else if (type == this.$config.navLinkType.intelligentForms) {
                            this.$u.route('/pages/form/details/details', { id: val });
                        } else {
                            this.$u.route(val);
                        }

                        return;
                    }
                    // #endif
                }
            },
        }
    }
</script>
