<template>
    <view>
        <u-gap :height="corecmsdata.parameters.height * 2" :bg-color="corecmsdata.parameters.backgroundColor"></u-gap>
    </view>
</template>

<script>
    export default {
        name: "corecmsblank",
        props: {
            corecmsdata: {
                // type: Array,
                required: true,
            }
        },
        methods: {

        }
    }
</script>

<style scoped lang="scss">
</style>
