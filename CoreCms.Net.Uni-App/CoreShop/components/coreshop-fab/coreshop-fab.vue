<template>
    <view>
        <view class="fab-box fab" :class="{ leftBottom: leftBottom, rightBottom: rightBottom, leftTop: leftTop, rightTop: rightTop }">
            <view class="fab-circle"
                  :class="{
					left: horizontal === 'left' && direction === 'horizontal',
					top: vertical === 'top' && direction === 'vertical',
					bottom: vertical === 'bottom' && direction === 'vertical',
					right: horizontal === 'right' && direction === 'horizontal'
				}"
                  :style="{ 'background-color': styles.buttonColor }"
                  @click="open">
                <image class="icon icon-jia" src="/static/images/common/menu.png" mode="" :class="{ active: showContent }"></image>
                <!-- <text class="icon icon-jia" :class="{ active: showContent }"></text> -->
            </view>
            <view class="fab-content"
                  :class="{
					left: horizontal === 'left',
					right: horizontal === 'right',
					flexDirection: direction === 'vertical',
					flexDirectionStart: flexDirectionStart,
					flexDirectionEnd: flexDirectionEnd
				}"
                  :style="{ width: boxWidth, height: boxHeight, background: styles.backgroundColor }">
                <view v-if="flexDirectionStart || horizontalLeft" class="fab-item first"></view>
                <view class="fab-item"
                      v-for="(item, index) in content"
                      :key="index"
                      :class="{ active: showContent }"
                      :style="{
						color: item.active ? styles.selectedColor : styles.color
					}"
                      @click="taps(index, item)">
                    <image class="content-image icon"
                           :src="item.active ? item.selectedIconPath : item.iconPath"
                           mode=""></image>
                    <text class="text">{{ item.text }}</text>
                </view>
                <view v-if="flexDirectionEnd || horizontalRight" class="fab-item first"></view>
            </view>
        </view>
    </view>
</template>

<script>
    export default {
        props: {
            pattern: {
                type: Object,
                default: () => {
                    return {
                        color: '#7A7E83',
                        backgroundColor: '#fff',
                        selectedColor: '#007AFF',
                        buttonColor: '#FF7159'
                    };
                }
            },
            horizontal: {
                type: String,
                default: 'left'
            },
            vertical: {
                type: String,
                default: 'bottom'
            },
            direction: {
                type: String,
                default: 'horizontal'
            },
            content: {
                type: Array,
                default: () => {
                    return [{
                        iconPath: '/static/images/common/tab-ic-hom-selected.png',
                        selectedIconPath: '/static/images/common/tab-ic-hom-unselected.png',
                        // text: '首页',
                        active: false,
                        url: '/pages/index/default/default'
                    },
                    {
                        iconPath: '/static/images/common/tab-ic-me-selected.png',
                        selectedIconPath: '/static/images/common/tab-ic-me-unselected.png',
                        // text: '个人中心',
                        active: false,
                        url: '/pages/index/member/member'
                    }];
                }
            }
        },
        data() {
            return {
                fabShow: false,
                flug: true,
                showContent: false,
                styles: {
                    color: '#3c3e49',
                    selectedColor: '#007AFF',
                    backgroundColor: '#fff',
                    buttonColor: '#3c3e49'
                }
            };
        },
        created() {
            if (this.top === 0) {
                this.fabShow = true;
            }
            // 初始化样式
            this.styles = Object.assign({}, this.styles, this.pattern);
        },
        methods: {
            open() {
                this.showContent = !this.showContent;
            },
            /**
             * 按钮点击事件
             */
            taps(index, item) {
                //this.$emit('trigger', {
                //    index,
                //    item
                //});
                this.$u.route({
                    type: 'switchTab',
                    url: item.url
                });
            },
            /**
             * 获取 位置信息
             */
            getPosition(types, paramA, paramB) {
                if (types === 0) {
                    return this.horizontal === paramA && this.vertical === paramB;
                } else if (types === 1) {
                    return this.direction === paramA && this.vertical === paramB;
                } else if (types === 2) {
                    return this.direction === paramA && this.horizontal === paramB;
                } else {
                    return this.showContent && this.direction === paramA
                        ? this.contentWidth
                        : this.contentWidthMin;
                }
            }
        },
        watch: {
            pattern(newValue, oldValue) {
                // console.log(JSON.stringify(newValue));
                this.styles = Object.assign({}, this.styles, newValue);
            }
        },
        computed: {
            contentWidth(e) {
                return uni.upx2px((this.content.length + 1) * 90 + 20) + 'px';
            },
            contentWidthMin() {
                return uni.upx2px(90) + 'px';
            },
            // 动态计算宽度
            boxWidth() {
                return this.getPosition(3, 'horizontal');
            },
            // 动态计算高度
            boxHeight() {
                return this.getPosition(3, 'vertical');
            },
            // 计算左下位置
            leftBottom() {
                return this.getPosition(0, 'left', 'bottom');
            },
            // 计算右下位置
            rightBottom() {
                return this.getPosition(0, 'right', 'bottom');
            },
            // 计算左上位置
            leftTop() {
                return this.getPosition(0, 'left', 'top');
            },
            rightTop() {
                return this.getPosition(0, 'right', 'top');
            },
            flexDirectionStart() {
                return this.getPosition(1, 'vertical', 'top');
            },
            flexDirectionEnd() {
                return this.getPosition(1, 'vertical', 'bottom');
            },
            horizontalLeft() {
                return this.getPosition(2, 'horizontal', 'left');
            },
            horizontalRight() {
                return this.getPosition(2, 'horizontal', 'right');
            }
        }
    };
</script>

<style scoped lang="scss">
    .fab-box { position: fixed; display: flex; justify-content: center; align-items: center; z-index: 2;
        &.top { width: 60upx; height: 60upx; right: 30upx; bottom: 180upx; border: 1px #5989b9 solid; background: #6699cc; border-radius: 10upx; color: #fff; transition: all 0.3; opacity: 0; }
        &.active { opacity: 1; }
        &.fab { z-index: 10;
            &.leftBottom { left: 30upx; bottom: 180upx; }
            &.leftTop { left: 30upx; top: 80upx; top: calc(80upx + var(--window-top)); }
            &.rightBottom { right: 30upx; bottom: 180upx; }
            &.rightTop { right: 30upx; top: 80upx; top: calc(80upx + var(--window-top)); }
        }
    }
    .fab-circle { display: flex; justify-content: center; align-items: center; position: absolute; width: 90upx; height: 90upx; background: #3c3e49; border-radius: 50%; box-shadow: 0 0 5px 2px rgba(0, 0, 0, 0.2); z-index: 11;
        &.left { left: 0; }
        &.right { right: 0; }
        &.top { top: 0; }
        &.bottom { bottom: 0; }
        .icon-jia { color: #ffffff; font-size: 50upx; transition: all 0.3s; width: 45rpx; height: 45rpx;
            &.active { transform: rotate(90deg); }
        }
    }
    .fab-content { background: #6699cc; box-sizing: border-box; display: flex; border-radius: 100upx; overflow: hidden; box-shadow: 0 0 5px 2px rgba(0, 0, 0, 0.1); transition: all 0.2s; width: 110upx;
        &.left { justify-content: flex-start; }
        &.right { justify-content: flex-end; }
        &.flexDirection { flex-direction: column; justify-content: flex-end; }
        &.flexDirectionStart { flex-direction: column; justify-content: flex-start; }
        &.flexDirectionEnd { flex-direction: column; justify-content: flex-end; }
        .fab-item { display: flex; flex-direction: column; justify-content: center; align-items: center; width: 90upx; height: 90upx; font-size: 24upx; color: #fff; opacity: 0; transition: opacity 0.2s;
            &.active { opacity: 1; }
            .content-image { width: 60upx; height: 60upx; margin-bottom: 10upx; }
            &.first { width: 110upx; }
        }
    }

</style>