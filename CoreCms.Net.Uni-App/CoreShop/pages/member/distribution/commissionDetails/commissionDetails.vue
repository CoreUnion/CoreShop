<!-- 佣金明细 -->
<template>
    <view>
        <u-navbar title="佣金明细"></u-navbar>
        <view class="topheader">
            <!-- 筛选 -->
            <view class="wallet-wrap">
                <view class="wallet-card coreshop-bg-red">
                    <view class="head-box coreshop-flex coreshop-align-center">
                        <view class="head-title">总收益（元）</view>
                         <view class="look-btn" @click="onEye">
                                <u-icon class="eye" v-if="showMoney" name="eye-fill"></u-icon>
                                <u-icon class="eye" v-else name="eye-off"></u-icon>
                         </view>
                    </view>
                    <view class="card-num">{{ showMoney ? info.totalSettlementAmount || '0.00' : '***' }}</view>
                    <view class="card-bottom u-flex u-row-between">
                        <view class="card-item coreshop-flex coreshop-flex-direction coreshop-align-start">
                            <view class="item-title">待入账佣金</view>
                            <view class="item-value">{{ showMoney ? info.freezeAmount || '0.00' : '***' }}</view>
                        </view>
                        <view class="card-item coreshop-flex coreshop-flex-direction coreshop-align-start">
                            <view class="item-title">可提现佣金</view>
                            <view class="item-value">{{ showMoney ? userInfo.balance || '0.00' : '***' }}</view>
                        </view>
                        <view class=""></view>
                    </view>
                    <button class="cu-btn draw-btn" @tap="goWithdraw">提现</button>
                </view>
            </view>

            <view class="head_box u-flex u-row-between">
                <u-button @click="onFilterDate" shape="square" size="mini" class="date-btn">{{ selDateText }}<u-icon name="arrow-down" class="u-margin-left-20"></u-icon></u-button>
                <view class="total-box">收入￥{{ totalMoney || '0.00' }}</view>
            </view>
            <!-- 状态分类 -->
            <!--<view class="coreshop-flex coreshop-align-center nav-box">
                <view class="state-item flex-sub " v-for="(state, index) in statusList" :key="state.value" @tap="onTab(state.value)">
                    <text class="state-title" :class="{ 'title-active': stateCurrent === state.value }">{{ state.name }}</text>
                    <text class="underline" :class="{ 'underline-active': stateCurrent === state.value }"></text>
                </view>
            </view>-->

        </view>

        <scroll-view :style="'height:'+viewHeight+'px'" scroll-y="true" class="scroll-Y" @scrolltolower="bottomOut()">
                <!-- 佣金明细列表 -->
                <view v-if="list.length>0">
                    <view class="coreshop-log-item u-flex u-row-between" v-for="item in list" :key="item.id">
                        <view class="item-left coreshop-flex coreshop-align-center">
                            <!--<image class="log-img" :src="item.buyer.avatar" mode=""></image>-->
                            <view class="coreshop-flex coreshop-flex-direction coreshop-align-start">
                                <view class="log-name  coreshop-text-black">{{ item.typeName }}</view>
                                <view class="log-notice  coreshop-text-grey">订单号：{{ item.sourceId }}</view>
                            </view>
                        </view>
                        <view class="item-right coreshop-flex coreshop-flex-direction coreshop-align-end">
                            <view class="log-num  coreshop-text-red">+{{ item.money }}</view>
                            <view class="log-date  coreshop-text-grey">{{ $u.timeFormat(item.createTime, 'yyyy.mm.dd') }}</view>
                        </view>
                    </view>
                    <!-- 更多 -->
                    <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
                </view>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/history.png'" icon-size="300" text="当前日期暂无佣金明细" mode="list"></u-empty>
                </view>
        </scroll-view>

        <!-- 日期选择 -->
        <u-calendar v-model="showCalendar"
                    :mode="mode"
                    :start-text="startText"
                    :end-text="endText"
                    :range-color="rangeColor"
                    :range-bg-color="rangeBgColor"
                    :active-bg-color="activeBgColor"
                    btnType="success"
                    @change="selDate"></u-calendar>
    </view>
</template>

<script>
    import { commonUse } from '@/common/mixins/mixinsHelper.js'
    export default {
        mixins: [commonUse],
        data() {
            return {
                showMoney: false, //是否显示金额
                info: {}, //分销商信息
                userInfo: {}, // 用户信息
                //日期选择
                showCalendar: false,
                mode: 'range',
                result: '请选择日期',
                startText: '开始',
                endText: '结束',
                rangeColor: '#4CB89D',
                rangeBgColor: 'rgba(76,184,157,0.13)',
                activeBgColor: '#4CB89D',
                selDateText: '',
                list: [], //佣金记录
                propsDate: '', //日期参数
                totalMoney: 0, //收入
                page: 1,
                lastPage: 1,
                limit: 10,
                status: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                },
                stateCurrent: 'all', //默认
                statusList: [
                    {
                        name: '全部',
                        value: 'all'
                    },
                    {
                        name: '待入账',
                        value: 'waiting'
                    },
                    {
                        name: '已入账',
                        value: 'entry'
                    },
                    {
                        name: '已退回',
                        value: 'back'
                    }
                ],
                viewHeight: 0,
            }
        },
        onShow() {
            var _this = this;
            uni.getSystemInfo({
                success: function (res) { // res - 各种参数
                    console.log(res); // 屏幕的宽度
                    var windowHeight = res.windowHeight;

                    let info = uni.createSelectorQuery().select(".topheader");
                    info.boundingClientRect(function (data) { //data - 各种参数
                        var headHeight = data.height;
                        _this.viewHeight = windowHeight - headHeight;

                    }).exec()
                }
            });

            _this.$u.api.getDistributionInfo().then(res => {
                if (res.status) {
                    _this.info = res.data;
                } else {
                    //报错了
                    _this.$u.toast(res.msg);
                }
            });
        },
        onLoad() {
            this.initData()
            this.getToday();
            this.getCommissionLog();
        },
        onReachBottom() {
            if (this.status === 'loadmore') {
                this.getCommissionLog()
            }
        },
        methods: {
            //触底加载数据
            bottomOut() {
                if (this.status == 'nomore') {
                    return;
                }
                if (this.status == 'loadmore') {
                    this.getCommissionLog();//调用数据请求
                }
            },
            navigateToHandle(pageUrl) {
                this.$u.route(pageUrl)
            },
            // 切换分类
            onTab(state) {
                this.list = [];
                this.page = 1;
                this.stateCurrent = state;
                this.$u.debounce(this.getCommissionLog);
            },
            // 点击日期选择
            onFilterDate() {
                this.showCalendar = true;
            },
            // 是否显示金额
            onEye() {
                this.showMoney = !this.showMoney;
            },
            //  今日
            getToday() {
                let now = new Date();
                this.selDateText = `${now.getFullYear()}.${now.getMonth() + 1}.${now.getDate()}`;
                let dateText = `${now.getFullYear()}/${now.getMonth() + 1}/${now.getDate()}`;
                this.propsDate = `${dateText}-${dateText}`;
            },

            // 选择日期
            selDate(e) {
                this.list = [];
                this.page = 1;
                this.selDateText = `${e.startYear}.${e.startMonth}.${e.startDay}-${e.endYear}.${e.endMonth}.${e.endDay}`;
                let dateText = `${e.startYear}/${e.startMonth}/${e.startDay}-${e.endYear}/${e.endMonth}/${e.endDay}`;
                this.propsDate = dateText;
                console.log(this.propsDate);
                this.getCommissionLog();
            },
            //去提现
            goWithdraw() {
                this.$u.route('/pages/member/balance/withdrawCash/withdrawCash')
            },
            initData() {
                // 获取用户信息
                var _this = this
                this.$u.api.userInfo().then(res => {
                    if (res.status) {
                        _this.userInfo = res.data
                    }
                })
            },
            // 获取余额明细
            getCommissionLog() {
                let data = {
                    id: 5,
                    page: this.page,
                    limit: this.limit,
                    propsDate: this.propsDate
                }
                this.status = 'loading'
                this.$u.api.getBalanceList(data).then(res => {
                    if (res.status) {
                        this.totalMoney = res.otherData.sunMoney;
                        if (this.page >= res.otherData.totalPages) {
                            // 没有数据了
                            this.status = 'nomore'
                        } else {
                            // 未加载完毕
                            this.status = 'loadmore'
                            this.page++
                        }
                        this.list = [...this.list, ...res.data]
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            }
        },
    }
</script>

<style lang="scss">
    @import "commissionDetails.scss";
</style>
