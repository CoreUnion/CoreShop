<template>
    <view>
        <view class="select-item" v-for="(item, index) in specList" :key="index">
            <view class="coreshop-text-black u-margin-10  coreshop-solid-bottom u-padding-bottom-20">{{ index }}</view>
            <view class="select-btn">
                <button class="sku-btn u-margin-10" :class="spes.cla" v-for="(spes, key) in item" :key="key" @click="specChangeSpes(index, key)">
                    <u-avatar :src="spes.image" size="50" class="u-margin-right-10 u-margin-top-10" style="margin-left: -20rpx;"></u-avatar>
                    {{ spes.name }}
                </button>
            </view>
        </view>
    </view>
</template>
<script>
    export default {
        name: "spec",
        data() {
            return {
                specList: {}
            }
        },
        props: {
            // 默认picker选中项索引
            spesData: {
                required: true
            }
        },
        watch: {
            spesData: function (val) {
                let d = JSON.parse(val);
                this.specList = d;
            }
        },
        methods: {
            specChangeSpes(v, k) {
                let newData = {
                    v: v,
                    k: k
                }
                this.$emit("changeSpes", newData);
            },
            changeSpecData() {
                this.specList = {};
            }
        }
    }
</script>
<style scoped lang="scss">
    .selected { border: 2upx solid #ff0000; background-color: #fff5f6; color: #ff0000; }
    .not-selected { border: 2upx solid #ccc; }
    .none { border: 2upx dashed #ccc; color: #888; display: none; }
</style>