<template>
    <view>
        <u-navbar :is-back="true" back-text="返回" title="地图展示"></u-navbar>
        <view class="content">
            <map :style="[{height:height+'px'},{width:'100%'}]" id="map" :latitude="lat"
                 :longitude="lng"
                 :scale="15"
                 :enable-3D="true"
                 :show-compass="true"
                 :enable-overlooking="true"
                 :enable-traffic="true"
                 :markers="covers">
            </map>
        </view>
        <!-- 登录提示 -->
		<coreshop-login-modal></coreshop-login-modal>
    </view>
</template>

<script>
    export default {
        data() {
            return {
                id: 0,
                lat: 0,
                lng: 0,
                covers: [],
                height: 0
            }
        },
        onLoad(options) {
            //console.log(options);
            const res = uni.getSystemInfoSync();
            this.height = res.windowHeight;

            this.lat = options.latitude;
            this.lng = options.longitude;
            let mark = {
                id: options.id,
                latitude: options.latitude,
                longitude: options.longitude,
                iconPath: '/static/images/map/location.png',
                width: 15,
                height: 23,
            }
            this.covers.push(mark);
        },
        methods: {

        }
    }
</script>

<style lang="scss" scoped>

</style>
