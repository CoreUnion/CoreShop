<template>
    <view class="u-padding-top-10 u-padding-bottom-10 u-margin-15 coreshop-bg-white" v-if="count > 0">
        <u-notice-bar mode="vertical" type="none" :more-icon="true" :list="list" @click="click"></u-notice-bar>
    </view>
</template>

<script>
    export default {
        name: "coreshopnotice",
        components: {
        },
        props: {
            coreshopdata: {
                // type: Object,
                required: true,
            }
        },
        data() {
            return {
                speakerMsgs: [],
                list: [],
            }
        },
        created() {
            var data = this.coreshopdata.parameters.list;
            for (var i = 0; i < data.length; i++) {
                let moder = {
                    title: data[i].title,
                    url: '/pages/article/details/details?id=' + data[i].id + '&idType=2',
                    opentype: "navigate"
                }
                this.list.push(data[i].title);
                this.speakerMsgs.push(moder);
            }
        },
        methods: {
            click(index) {
                console.log(this.speakerMsgs[index].url);
                var url = this.speakerMsgs[index].url;
                this.$u.route(url)
            }
        },
        computed: {
            count() {
                return (this.coreshopdata.parameters.list.length > 0)
            }
        }
    }
</script>
