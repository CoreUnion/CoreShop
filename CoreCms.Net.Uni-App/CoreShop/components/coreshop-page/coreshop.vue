<template>
    <view>
        <block v-for="(item,index) in coreshopdata" :key="index">
            <!--搜索（修复）-->
            <coreshopsearch :coreshopdata="item" v-if="item.widgetCode=='search' "></coreshopsearch>
            <!--切换-->
            <coreshoptabbar :coreshopdata="item" v-if="item.widgetCode=='tabBar' "></coreshoptabbar>
            <!--公告（修复）-->
            <coreshopnotice :coreshopdata="item" v-if="item.widgetCode=='notice' "></coreshopnotice>
            <!--图片轮播（修复）-->
            <coreshopimgSlide :coreshopdata="item" v-if="item.widgetCode=='imgSlide' "></coreshopimgSlide>
            <!--优惠券（修复）-->
            <coreshopcoupon :coreshopdata="item" v-if="item.widgetCode=='coupon' "></coreshopcoupon>
            <!--空格（修复）-->
            <coreshopblank :coreshopdata="item" v-if="item.widgetCode=='blank' "></coreshopblank>
            <!--多行文本输入区（修复）-->
            <coreshoptextarea :coreshopdata="item" v-if="item.widgetCode=='textarea' "></coreshoptextarea>
            <!--视频（修复）-->
            <coreshopvideo :coreshopdata="item" v-if="item.widgetCode=='video' "></coreshopvideo>
            <!--图片集（修复）-->
            <coreshopimgWindow :coreshopdata="item" v-if="item.widgetCode=='imgWindow' "></coreshopimgWindow>
            <!--图片--（修复）-->
            <coreshopimgSingle :coreshopdata="item" v-if="item.widgetCode=='imgSingle' "></coreshopimgSingle>
            <!--商品（修复）-->
            <coreshopgoods :coreshopdata="item" v-if="item.widgetCode=='goods' "></coreshopgoods>
            <!--商品选项卡（修复）-->
            <coreshopgoodTabBar :coreshopdata="item" v-if="item.widgetCode=='goodTabBar' "></coreshopgoodTabBar>
            <!--文章（修复）-->
            <coreshoparticle :coreshopdata="item" v-if="item.widgetCode=='article' "></coreshoparticle>
            <!--文章分类（修复）-->
            <coreshoparticleClassify :coreshopdata="item" v-if="item.widgetCode=='articleClassify' "></coreshoparticleClassify>
            <!--宫格自定义导航（修复）-->
            <coreshopnavBar :coreshopdata="item" v-if="item.widgetCode=='navBar' "></coreshopnavBar>
            <!--团购（修复）-->
            <coreshopgroupPurchase :coreshopdata="item" v-if="item.widgetCode=='groupPurchase' "></coreshopgroupPurchase>
            <!--浏览记录（修复）-->
            <coreshoprecord :coreshopdata="item" v-if="item.widgetCode=='record' "></coreshoprecord>
            <!--拼团（修复）-->
            <coreshoppinTuan :coreshopdata="item" v-if="item.widgetCode=='pinTuan' "></coreshoppinTuan>
            <!--服务（修复）-->
            <coreshopservice :coreshopdata="item" v-if="item.widgetCode=='service' "></coreshopservice>
            <!--弹窗广告-->
            <coreshopadpop :coreshopdata="item" v-if="item.widgetCode=='adpop' "></coreshopadpop>
            <!--文本内容（修复）-->
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
