<template>
    <view>
        <view class="select-item" v-for="(item, index) in specList" :key="index">
            <view class="text-black u-margin-10">{{ index }}</view>
            <view class="select-btn">
                <button class="cu-btn u-margin-10" :class="spes.cla" v-for="(spes, key) in item" :key="key" @click="specChangeSpes(index, key)"> {{ spes.name }}</button>
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
    .goods-specs, .goods-number { padding: 26upx; border-top: 1px solid #f3f3f3; }
        .goods-specs:first-child { border: none; }
    .pop-m-title { margin-right: 10upx; color: #666; }
    .pop-m-item { display: inline-block; float: left; padding: 6upx 16upx; background-color: #fff; color: #333; margin-right: 16upx; margin-bottom: 10upx; }
    .pop-m-bd { overflow: hidden; margin-top: 10upx; }
    .selected { border: 2upx solid #333; background-color: #333; color: #fff; }
        .selected:not([class*="bg-"]) { background-color: #333; }
    .not-selected { border: 2upx solid #ccc; }
    .none { border: 2upx dashed #ccc; color: #888; }
</style>