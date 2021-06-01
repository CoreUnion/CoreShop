<template>
	<view class="lvv-popup" v-show="popshow" @touchmove.prevent>
		<view class="lvv-popupmark" :class="position=='top'&&!hideanimation?'pt':position=='left'&&!hideanimation?'pl':position=='right'&&!hideanimation?'pr':position=='bottom'&&!hideanimation?'pc':position=='top'&&hideanimation?'ht':position=='left'&&hideanimation?'hl':position=='right'&&hideanimation?'hr':position=='bottom'&&hideanimation?'hc':''" @click="close"></view>
		<view class="lvv-popupcontent" @click="close" :class="position=='top'&&!hideanimation?'pt':position=='left'&&!hideanimation?'pl':position=='right'&&!hideanimation?'pr':position=='bottom'&&!hideanimation?'pb':position=='top'&&hideanimation?'ht':position=='left'&&hideanimation?'hl':position=='right'&&hideanimation?'hr':position=='bottom'&&hideanimation?'hb':''">
			<view class="realcontent" @click.stop="">
				<slot></slot>
			</view>
		</view>
	</view>
</template>
<script>
	export default {
		props:{
			position:{
				type:String,
				default:null 
			}
		},
		data() { 
			return { 
				popshow:false,
				hideanimation:false,
			};
		},
		methods:{
			// Toshow popup page
			show:function(){
				this.popshow = true;
			},
			// Tohide popup page
			close:function(){
				let that = this;
				this.$emit("close");
				that.hideanimation = true;
				if(that.position==null){
					that.popshow = false;
				}else{
					setTimeout(function(){
						that.popshow = false;
						that.hideanimation = false;
					},500)
				}
			}
		},
	}
</script>

<style scoped lang="scss">
    .lvv-popup { top: 0; left: 0; width: 100%; height: 100%; position: fixed; z-index: 98; }
        .lvv-popup .lvv-popupmark { top: 0; left: 0; width: 100%; height: 100%; z-index: 99; position: absolute; background: rgba(0, 0, 0, 0.5); }
            .lvv-popup .lvv-popupmark.pt, .lvv-popup .lvv-popupmark.ht { background: none; }
        .lvv-popup .lvv-popupcontent { width: 100%; height: 100%; top: 0; left: 0; position: absolute; z-index: 100; }
        .lvv-popup .pt { animation: showtop 0.5s; }
        .lvv-popup .pl { animation: showleft 0.5s; }
        .lvv-popup .pr { animation: showright 0.5s; }
        .lvv-popup .pb { animation: showbottom .5s; }
        .lvv-popup .ht { animation: hidetop 0.5s; }
        .lvv-popup .hl { animation: hideleft 0.55s; }
        .lvv-popup .hr { animation: hideright 0.55s; }
        .lvv-popup .hb { animation: hidebottom 1s; }
        .lvv-popup .pc { animation: showcontent .55s; }
        .lvv-popup .hc { animation: hidecontent .55s; }

    @keyframes showtop {
        0% { transform: translateY(-100%); opacity: 1; }
        100% { top: 0px; transform: translateY(0%); opacity: 1; }
    }

    @keyframes showleft {
        0% { transform: translateX(-100%); opacity: 1; }
        50% { opacity: 0; }
        100% { transform: translateX(0); }
    }

    @keyframes showright {
        0% { transform: translateX(100%); opacity: 1; }
        50% { opacity: 0; }
        100% { transform: translateX(0); }
    }

    @keyframes showbottom {
        0% { transform: translateY(100%); opacity: 1; }
        50% { opacity: 0.5; }
        100% { transform: translateY(0); }
    }

    @keyframes hidetop {
        0% { transform: translateY(0%); opacity: 1; }
        100% { transform: translateY(-100%); opacity: 1; }
    }

    @keyframes hideleft {
        0% { transform: translateX(0); }
        50% { opacity: 0; }
        100% { transform: translateX(-100%); opacity: 1; }
    }

    @keyframes hideright {
        0% { transform: translateX(0); }
        50% { opacity: 0; }
        100% { transform: translateX(100%); opacity: 1; }
    }

    @keyframes hidebottom {
        0% { transform: translateY(0); }
        50% { opacity: 0; }
        100% { transform: translateY(100%); opacity: 1; }
    }

    @keyframes showcontent {
        0% { opacity: 0; }
        100% { opacity: 1; }
    }

    @keyframes hidecontent {
        0% { opacity: 1; }
        100% { opacity: 0; }
    }
</style>
