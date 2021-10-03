<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="搜索"></u-navbar>
        <view class="u-padding-15 u-margin-bottom-15 coreshop-bg-white u-border-bottom">
            <u-search placeholder="请输入关键字搜索" v-model="key" :show-action="true" action-text="搜索" :animation="false" @search="search" @custom="search"></u-search>
        </view>

        <!--搜索区域-->
        <view class="u-padding-15 u-margin-15" v-if="!deleteView">
            <!--搜索记录-->
            <view class="u-margin-bottom-15" v-show="keys.length > 0">
                <u-section title="历史搜索" sub-title="删除" arrow="false" @click="deleteKey"></u-section>
                <view class="btn-view">
                    <u-tag text="item" type="success" v-for="(item, key) in keys" :key="key" @click="toNav(item)" />
                </view>
            </view>

            <!--推荐搜索-->
            <view class="u-margin-bottom-15" v-show="recommend && recommend.length > 0">
                <u-section title="推荐搜索" :right="false"></u-section>
                <view class="u-margin-top-15 u-margin-bottom-15">
                    <u-tag class="u-padding-10" :text="item" v-for="(item, key) in recommend" :key="key" type="info" @click="toNav(item)" />
                </view>
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
                keys: [],
                key: '',
                navType: 'toNav',
                focus: true,
            }
        },
        computed: {
            recommend() {
                return this.$store.state.config.recommendKeys
            }
        },
        methods: {
            //搜索
            search: function () {
                let keys = this.key;
                if (keys != '') {
                    let search_key = this.$db.get('search_key');
                    if (!search_key) {
                        search_key = [];
                    }
                    let flag = true;
                    for (var key in search_key) {
                        if (search_key[key] == keys) {
                            flag = false;
                        }
                    }
                    if (flag) {
                        search_key.unshift(keys);
                    }
                    this.$db.set('search_key', search_key);
                    this.$db.set('search_term', keys);
                    this.$u.route('/pages/category/list/list?key=' + keys);
                } else {
                    this.$refs.uToast.show({
                        title: '请输入要查询的关键字',
                        type: 'warning',
                    })
                }
            },

            //清除
            deleteKey: function () {
                //删除显示
                this.keys = [];
                //删除存储
                this.$db.del('search_key');
            },

            //跳转操作
            toNav: function (keys) {
                this.$db.set('search_term', keys);
                let search_key = this.$db.get('search_key');
                if (!search_key) {
                    search_key = [];
                }
                var flag = true;
                for (var key in search_key) {
                    if (search_key[key] == keys) {
                        flag = false;
                    }
                }
                if (flag) {
                    search_key.unshift(keys);
                }
                this.$db.set('search_key', search_key);
                this.$u.route('/pages/category/list/list?key=' + keys);
            },
        },
        //加载触发
        onShow(e) {
            this.keys = this.$db.get('search_key');
            this.focus = true;
        },
        //页面卸载触发
        onUnload() {
            this.$db.set('search_term', '');
        }
    }
</script>

<style lang="scss">
    page { background: #fff; }
</style>
