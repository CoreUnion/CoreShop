<template>
    <view>
        <block v-for="(item,index) in corecmsdata" :key="index">
            <!--ËÑË÷£¨ÐÞ¸´£©-->
            <corecmssearch :corecmsdata="item" v-if="item.widgetCode=='search' "></corecmssearch>
            <!--ÇÐ»»-->
            <corecmstabbar :corecmsdata="item" v-if="item.widgetCode=='tabBar' "></corecmstabbar>
            <!--¹«¸æ£¨ÐÞ¸´£©-->
            <corecmsnotice :corecmsdata="item" v-if="item.widgetCode=='notice' "></corecmsnotice>
            <!--Í¼Æ¬ÂÖ²¥£¨ÐÞ¸´£©-->
            <corecmsimgSlide :corecmsdata="item" v-if="item.widgetCode=='imgSlide' "></corecmsimgSlide>
            <!--ÓÅ»ÝÈ¯£¨ÐÞ¸´£©-->
            <corecmscoupon :corecmsdata="item" v-if="item.widgetCode=='coupon' "></corecmscoupon>
            <!--¿Õ¸ñ£¨ÐÞ¸´£©-->
            <corecmsblank :corecmsdata="item" v-if="item.widgetCode=='blank' "></corecmsblank>
            <!--¶àÐÐÎÄ±¾ÊäÈëÇø£¨ÐÞ¸´£©-->
            <corecmstextarea :corecmsdata="item" v-if="item.widgetCode=='textarea' "></corecmstextarea>
            <!--ÊÓÆµ£¨ÐÞ¸´£©-->
            <corecmsvideo :corecmsdata="item" v-if="item.widgetCode=='video' "></corecmsvideo>
            <!--Í¼Æ¬¼¯£¨ÐÞ¸´£©-->
            <corecmsimgWindow :corecmsdata="item" v-if="item.widgetCode=='imgWindow' "></corecmsimgWindow>
            <!--Í¼Æ¬--£¨ÐÞ¸´£©-->
            <corecmsimgSingle :corecmsdata="item" v-if="item.widgetCode=='imgSingle' "></corecmsimgSingle>
            <!--ÉÌÆ·£¨ÐÞ¸´£©-->
            <corecmsgoods :corecmsdata="item" v-if="item.widgetCode=='goods' "></corecmsgoods>
            <!--ÎÄÕÂ£¨ÐÞ¸´£©-->
            <corecmsarticle :corecmsdata="item" v-if="item.widgetCode=='article' "></corecmsarticle>
            <!--ÎÄÕÂ·ÖÀà£¨ÐÞ¸´£©-->
            <corecmsarticleClassify :corecmsdata="item" v-if="item.widgetCode=='articleClassify' "></corecmsarticleClassify>
            <!--¹¬¸ñ×Ô¶¨Òåµ¼º½£¨ÐÞ¸´£©-->
            <corecmsnavBar :corecmsdata="item" v-if="item.widgetCode=='navBar' "></corecmsnavBar>
            <!--ÍÅ¹º£¨ÐÞ¸´£©-->
            <corecmsgroupPurchase :corecmsdata="item" v-if="item.widgetCode=='groupPurchase' "></corecmsgroupPurchase>
            <!--ä¯ÀÀ¼ÇÂ¼£¨ÐÞ¸´£©-->
            <corecmsrecord :corecmsdata="item" v-if="item.widgetCode=='record' "></corecmsrecord>
            <!--Æ´ÍÅ£¨ÐÞ¸´£©-->
            <corecmspinTuan :corecmsdata="item" v-if="item.widgetCode=='pinTuan' "></corecmspinTuan>
            <!--·þÎñ£¨ÐÞ¸´£©-->
            <corecmsservice :corecmsdata="item" v-if="item.widgetCode=='service' "></corecmsservice>
            <!--µ¯´°¹ã¸æ-->
            <corecmsadpop :corecmsdata="item" v-if="item.widgetCode=='adpop' "></corecmsadpop>
            <!--ÎÄ±¾ÄÚÈÝ£¨ÐÞ¸´£©-->
            <corecmsContent :corecmsdata="item" v-if="item.widgetCode=='content' "></corecmsContent>
        </block>
    </view>
</template>

<script>
    import corecmsimgSlide from '@/components/corecms-page/corecms-imgSlide.vue'
    import corecmssearch from '@/components/corecms-page/corecms-search.vue'
    import corecmsnotice from '@/components/corecms-page/corecms-notice.vue'
    import corecmscoupon from '@/components/corecms-page/corecms-coupon.vue'
    import corecmsblank from '@/components/corecms-page/corecms-blank.vue'
    import corecmstextarea from '@/components/corecms-page/corecms-textarea.vue'
    import corecmsvideo from '@/components/corecms-page/corecms-video.vue'
    import corecmsimgWindow from '@/components/corecms-page/corecms-imgWindow.vue'
    import corecmsimgSingle from '@/components/corecms-page/corecms-imgSingle.vue'
    import corecmsgoods from '@/components/corecms-page/corecms-goods.vue'
    import corecmsarticle from '@/components/corecms-page/corecms-article.vue'
    import corecmsarticleClassify from '@/components/corecms-page/corecms-articleClassify.vue'
    import corecmsnavBar from '@/components/corecms-page/corecms-navBar.vue'
    import corecmsgroupPurchase from '@/components/corecms-page/corecms-groupPurchase.vue'
    import corecmsrecord from '@/components/corecms-page/corecms-record.vue'
    import corecmspinTuan from '@/components/corecms-page/corecms-pinTuan.vue'
    import corecmsservice from '@/components/corecms-page/corecms-service.vue'
    import corecmstabbar from '@/components/corecms-page/corecms-tabbar.vue'
    import corecmsadpop from '@/components/corecms-page/corecms-adpop.vue'
    import corecmsContent from '@/components/corecms-page/corecms-content.vue'

    export default {
        name: 'corecmsPage',
        components: {
            corecmsimgSlide,
            corecmssearch,
            corecmsnotice,
            corecmscoupon,
            corecmsblank,
            corecmstextarea,
            corecmsvideo,
            corecmsimgWindow,
            corecmsimgSingle,
            corecmsgoods,
            corecmsarticle,
            corecmsarticleClassify,
            corecmsnavBar,
            corecmsgroupPurchase,
            corecmsrecord,
            corecmspinTuan,
            corecmsservice,
            corecmstabbar,
            corecmsadpop,
            corecmsContent
        },
        props: {
            corecmsdata: {
                default: function () {
                    return []
                }
            }
        }
    }
</script>
