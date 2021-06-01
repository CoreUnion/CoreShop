<template>
    <view class="corecms-nowrap corecms-flex-vcenter" :style="{width:width}" @tap.stop="changeStatus">
        <view :class="['corecms-check-btn', status ? 'corecms-check-checked' : '']"
              :style="{fontSize:size+'rpx', lineHeight:size+'rpx', color : status ? checkedColor : color}"></view>
        <view class="corecms-check-lable"><slot></slot></view>
    </view>
</template>
<script>
    export default {
        props: {
            width: {
                type: String,
                default: '100%'
            },
            size: {
                type: Number,
                default: 38
            },
            color: {
                type: String,
                default: '#EEEEEE'
            },
            checked: {
                type: Boolean,
                default: false
            },
            checkedColor: {
                type: String,
                default: '#FF0036'
            },
            parameter: {
                type: Array,
                default: function () {
                    return []
                }
            }
        },
        data() {
            return {
                status: false
            }
        },
        watch: {
            checked: function (val, old) {
                this.status = val;
            }
        },
        created: function () {
            this.status = this.checked;
        },
        methods: {
            changeStatus: function () {
                this.status = !this.status;
                this.$emit('change', [this.status, this.parameter]);
            }
        }
    }
</script>
<style scoped lang="scss">
    .corecms-check-btn { color: #999999; flex-shrink: 0; }
        .corecms-check-btn:after { content: '\2713'; display: inline-block; vertical-align: 20rpx; width: 38rpx; height: 38rpx; margin-right: 20rpx; border-radius: 20rpx; background: #fff; text-indent: 8rpx; line-height: .65; border: 1rpx #c7c6c6 solid; }
    .corecms-check-checked:after { content: '\2713'; background: #ff0000; color: #fff; border: 1rpx #ff0000 solid; }
    .corecms-check-lable { color: #555555; margin-left: 20rpx; font-size: 26rpx; width: 700rpx; }
    .corecms-nowrap { display: flex; flex-direction: row; flex-wrap: nowrap; }
    .corecms-flex-vcenter { align-items: center; }
</style>