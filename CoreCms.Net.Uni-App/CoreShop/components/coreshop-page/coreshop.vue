<template>
    <view>
        <block v-for="(item,index) in coreshopdata" :key="index">
            <!--ËÑË÷£¨ÐÞ¸´£©-->
            <coreshopsearch :coreshopdata="item" v-if="item.widgetCode=='search' "></coreshopsearch>
            <!--ÇÐ»»-->
            <coreshoptabbar :coreshopdata="item" v-if="item.widgetCode=='tabBar' "></coreshoptabbar>
            <!--¹«¸æ£¨ÐÞ¸´£©-->
            <coreshopnotice :coreshopdata="item" v-if="item.widgetCode=='notice' "></coreshopnotice>
            <!--Í¼Æ¬ÂÖ²¥£¨ÐÞ¸´£©-->
            <coreshopimgSlide :coreshopdata="item" v-if="item.widgetCode=='imgSlide' "></coreshopimgSlide>
            <!--ÓÅ»ÝÈ¯£¨ÐÞ¸´£©-->
            <coreshopcoupon :coreshopdata="item" v-if="item.widgetCode=='coupon' "></coreshopcoupon>
            <!--¿Õ¸ñ£¨ÐÞ¸´£©-->
            <coreshopblank :coreshopdata="item" v-if="item.widgetCode=='blank' "></coreshopblank>
            <!--¶àÐÐÎÄ±¾ÊäÈëÇø£¨ÐÞ¸´£©-->
            <coreshoptextarea :coreshopdata="item" v-if="item.widgetCode=='textarea' "></coreshoptextarea>
            <!--ÊÓÆµ£¨ÐÞ¸´£©-->
            <coreshopvideo :coreshopdata="item" v-if="item.widgetCode=='video' "></coreshopvideo>
            <!--Í¼Æ¬¼¯£¨ÐÞ¸´£©-->
            <coreshopimgWindow :coreshopdata="item" v-if="item.widgetCode=='imgWindow' "></coreshopimgWindow>
            <!--Í¼Æ¬--£¨ÐÞ¸´£©-->
            <coreshopimgSingle :coreshopdata="item" v-if="item.widgetCode=='imgSingle' "></coreshopimgSingle>
            <!--ÉÌÆ·£¨ÐÞ¸´£©-->
            <coreshopgoods :coreshopdata="item" v-if="item.widgetCode=='goods' "></coreshopgoods>
            <!--ÉÌÆ·Ñ¡Ïî¿¨£¨ÐÞ¸´£©-->
            <coreshopgoodTabBar :coreshopdata="item" v-if="item.widgetCode=='goodTabBar' "></coreshopgoodTabBar>
            <!--ÎÄÕÂ£¨ÐÞ¸´£©-->
            <coreshoparticle :coreshopdata="item" v-if="item.widgetCode=='article' "></coreshoparticle>
            <!--ÎÄÕÂ·ÖÀà£¨ÐÞ¸´£©-->
            <coreshoparticleClassify :coreshopdata="item" v-if="item.widgetCode=='articleClassify' "></coreshoparticleClassify>
            <!--¹¬¸ñ×Ô¶¨Òåµ¼º½£¨ÐÞ¸´£©-->
            <coreshopnavBar :coreshopdata="item" v-if="item.widgetCode=='navBar' "></coreshopnavBar>
            <!--ÍÅ¹º£¨ÐÞ¸´£©-->
            <coreshopgroupPurchase :coreshopdata="item" v-if="item.widgetCode=='groupPurchase' "></coreshopgroupPurchase>
            <!--ä¯ÀÀ¼ÇÂ¼£¨ÐÞ¸´£©-->
            <coreshoprecord :coreshopdata="item" v-if="item.widgetCode=='record' "></coreshoprecord>
            <!--Æ´ÍÅ£¨ÐÞ¸´£©-->
            <coreshoppinTuan :coreshopdata="item" v-if="item.widgetCode=='pinTuan' "></coreshoppinTuan>
            <!--·þÎñ£¨ÐÞ¸´£©-->
            <coreshopservice :coreshopdata="item" v-if="item.widgetCode=='service' "></coreshopservice>
            <!--µ¯´°¹ã¸æ-->
            <coreshopadpop :coreshopdata="item" v-if="item.widgetCode=='adpop' "></coreshopadpop>
            <!--ÎÄ±¾ÄÚÈÝ£¨ÐÞ¸´£©-->
            <coreshopContent :coreshopdata="item" v-if="item.widgetCode=='content' "></coreshopContent>
        </block>
    </view>
</template>

<script>
    import coreshopimgSlide from '@/components/coreshop-page/coreshop-imgSlide.vue'
    import coreshopsearch from '@/components/coreshop-page/coreshop-search.vue'
    import coreshopnotice from '@/components/coreshop-page/coreshop-notice.vue'
    import coreshopcoupon from '@/components/coreshop-page/coreshop-coupon.vue'
    import coreshopblank from '@/components/coreshop-page/coreshop-blank.vue'
    import coreshoptextarea from '@/components/coreshop-page/coreshop-textarea.vue'
    import coreshopvideo from '@/components/coreshop-page/coreshop-video.vue'
    import coreshopimgWindow from '@/components/coreshop-page/coreshop-imgWindow.vue'
    import coreshopimgSingle from '@/components/coreshop-page/coreshop-imgSingle.vue'
    import coreshopgoods from '@/components/coreshop-page/coreshop-goods.vue'
    import coreshopgoodTabBar from '@/components/coreshop-page/coreshop-goodTabBar.vue'
    import coreshoparticle from '@/components/coreshop-page/coreshop-article.vue'
    import coreshoparticleClassify from '@/components/coreshop-page/coreshop-articleClassify.vue'
    import coreshopnavBar from '@/components/coreshop-page/coreshop-navBar.vue'
    import coreshopgroupPurchase from '@/components/coreshop-page/coreshop-groupPurchase.vue'
    import coreshoprecord from '@/components/coreshop-page/coreshop-record.vue'
    import coreshoppinTuan from '@/components/coreshop-page/coreshop-pinTuan.vue'
    import coreshopservice from '@/components/coreshop-page/coreshop-service.vue'
    import coreshoptabbar from '@/components/coreshop-page/coreshop-tabbar.vue'
    import coreshopadpop from '@/components/coreshop-page/coreshop-adpop.vue'
    import coreshopContent from '@/components/coreshop-page/coreshop-content.vue'

    export default {
        name: 'coreshopPage',
        components: {
            coreshopimgSlide,
            coreshopsearch,
            coreshopnotice,
            coreshopcoupon,
            coreshopblank,
            coreshoptextarea,
            coreshopvideo,
            coreshopimgWindow,
            coreshopimgSingle,
            coreshopgoods,
            coreshopgoodTabBar,
            coreshoparticle,
            coreshoparticleClassify,
            coreshopnavBar,
            coreshopgroupPurchase,
            coreshoprecord,
            coreshoppinTuan,
            coreshopservice,
            coreshoptabbar,
            coreshopadpop,
            coreshopContent
        },
        props: {
            coreshopdata: {
                default: function () {
                    return []
                }
            }
        }
    }
</script>
