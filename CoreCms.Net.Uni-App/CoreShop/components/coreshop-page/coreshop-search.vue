<template>
    <!-- 搜索组件宽度自适应于外层 -->
    <view class="u-padding-top-10 u-padding-bottom-10 u-padding-left-25 u-padding-right-25 u-margin-15 coreshop-bg-white" v-bind:class="coreshopdata.parameters.style">
        <u-toast ref="uToast" />
        <u-search :placeholder="coreshopdata.parameters.keywords" v-model="keyword" shape="square" :show-action="true" action-text="搜索" @custom="goSearch" @search="goSearch"></u-search>
    </view>
</template>
<script>
    export default {
        name: "coreshopsearch",
        props: {
            coreshopdata: {
                required: true,
            }
        },
        data() {
            return {
                keyword: '',
                searchTop: 0,
                scrollTop: 0,
                searchFixed: this.$store.state.searchFixed || false,
            };
        },
        created() {
            //#ifdef H5
            this.$nextTick(() => {
                this.searchTop = this.$refs.searchBar.$el.offsetTop;
            })
            // #endif
            this.searchStyle()
        },
        mounted() {
            // #ifdef H5
            window.addEventListener('scroll', this.handleScroll)
            // #endif
        },
        methods: {
            searchStyle() {
                this.$store.commit('searchStyle', this.coreshopdata.parameters.style)
                // console.log(this.data.parameters.style)
            },
            change(value) {
                // 搜索框内容变化时，会触发此事件，value值为输入框的内容
                //console.log(value);
            },
            goSearch() {
                if (this.keyword != '') {
                    this.$u.route('/pages/category/list/list?key=' + this.keyword);
                } else {
                    this.$refs.uToast.show({
                        title: '请输入查询关键字',
                        type: 'warning',
                    })
                }
            },
            handleScroll() {
                this.scrollTop = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop;
                this.scrollTop >= this.searchTop ? this.searchFixed = true : this.searchFixed = false;
            },
        },
    }
</script>
