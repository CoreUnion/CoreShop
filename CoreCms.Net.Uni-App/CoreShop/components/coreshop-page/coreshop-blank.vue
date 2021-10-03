<template>
    <view>
        <u-gap :height="coreshopdata.parameters.height * 2" :bg-color="coreshopdata.parameters.backgroundColor"></u-gap>
    </view>
</template>

<script>
    export default {
        name: "coreshopblank",
        props: {
            coreshopdata: {
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
