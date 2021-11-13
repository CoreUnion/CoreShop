//常用链接操作
export const commonUse = {
    mounted() { },
    methods: {
        //路由跳转
        goRoute(url) {
            // 无参数
            this.$u.route(url);
        },
        goRoute(url, params) {
            // 带参数，传递的对象形式的参数，如{name: 'lisa', age: 18}
            this.$u.route(url, params);
        },
        //查看所在坐标地图位置
        goShopMap() {
            var reshipCoordinate = this.$store.state.config.reshipCoordinate;
            if (reshipCoordinate && reshipCoordinate.indexOf(",") != -1) {
                var arr = reshipCoordinate.split(',')
                this.$u.route('/pages/map/map', { id: 1, latitude: arr[0], longitude: arr[1], });
            }
        },
        //查看所在坐标地图位置
        goMapDetails(id, latitude, longitude) {
            this.$u.route('/pages/map/map', { id: id, latitude: latitude, longitude: longitude, });
        },
        goUserCenter() {
            this.$u.route({
                url: '/pages/index/member/member',
                type: 'switchTab'
            });
        },
        // 返回上一页
        toBackBtn() {
            var pages = getCurrentPages();
            if (pages.length > 1) {
                uni.navigateBack({
                    delta: 1
                });
            } else {
                this.$u.route({ type: 'switchTab', url: '/pages/index/default/default' });
            }
        },
        //返回操作处理
        toOnBackPress(options) {
            if (options.from === 'navigateBack') {
                return false
            }
            let loginPages = ['/pages/index/cart/cart', '/pages/index/member/member']
            let backPage = this.$store.state.redirectPage
            if (loginPages.indexOf(backPage) > -1) {
                this.$store.commit({
                    type: 'redirect',
                    page: ''
                })
                this.$u.route({ type: 'switchTab', url: '/pages/index/default/default' })
                return true
            }
        },
        //登录成功统一跳转处理
        toLoginSuccessHandleBack() {
            let redirect = this.$store.state.redirectPage
            this.$store.commit({
                type: 'redirect',
                page: ''
            })
            let switchTabs = ['/pages/index/default/default', '/pages/index/member/member']
            if (switchTabs.indexOf(redirect) > -1) {
                this.$u.route({ type: 'switchTab', url: redirect })
            } else if (redirect) {
                this.$u.route({ type: 'switchTab', url: redirect })
            } else {
                this.$u.route({ type: 'switchTab', url: '/pages/index/default/default' })
            }
        }

    }
}



//订单接口信息
export const orders = {
    mounted() { },
    methods: {
        // 查看订单详情
        goOrderDetail(orderId) {
            this.$u.route('/pages/member/order/detail/detail', { orderId: orderId });
        },
        // 取消订单

        // 去支付
        goToPay(orderId) {
            this.$u.route('/pages/payment/pay/pay', { orderId: orderId, type: 1 });
        },
        // 确认收货

        // 去评价
        toEvaluate(orderId) {
            this.$u.route('/pages/member/order/evaluate/evaluate', { orderId: orderId });
        },
        // 申请售后

        // 查看物流信息
        goShowExpress(code, no, address = '', mobile = '') {
            let params = encodeURIComponent(
                'code=' + code + '&no=' + no + '&add=' + address + '&mobile=' + mobile
            )
            this.$u.route('/pages/member/order/expressDelivery/expressDelivery', { params: params });
        }
    }
}

//商品接口信息
export const goods = {
    mounted() { },
    methods: {
        // 查看商品详情
        goGoodsDetail(goodsId) {
            this.$u.route('/pages/goods/goodDetails/goodDetails', { id: goodsId });
        },
        // 查看商品评论详情
        goGoodComments(goodsId) {
            this.$u.route('/pages/goods/goodComments/goodComments', { id: goodsId });
        },
        // 跳转商品列表页
        goGoodsList(obj = {}) {
            let url = '/pages/category/list/list'
            if (Object.keys(obj).length) {
                url = url + this.$u.queryParams(obj)
            }
            this.$u.route(url)
        },
        // 秒杀详情
        goSeckillDetail(id, groupId) {
            this.$u.route('/pages/activity/seckill/details/details', { id: id, groupId: groupId, });
        },
        // 团购详情
        goGroupBuyingDetail(id, groupId) {
            this.$u.route('/pages/activity/groupBuying/details/details', { id: id, groupId: groupId, });
        },
        //拼团详情页
        goPinTuanDetail(id, pinTuanId, teamId) {
            console.log(id);
            console.log(teamId);
            if (teamId) {
                this.$u.route('/pages/activity/pinTuan/details/details', { id: id, pinTuanId: pinTuanId, teamId: teamId, });
            } else {
                this.$u.route('/pages/activity/pinTuan/details/details', { id: id, pinTuanId: pinTuanId });
            }
        },
        // 查看秒杀列表
        goSeckillList() {
            this.$u.route('/pages/activity/seckill/list/list')
        },
        // 查看拼团列表
        goPinTuanList() {
            this.$u.route('/pages/activity/pinTuan/list/list')
        }
    }
}


//文章接口
export const articles = {
    mounted() { },
    methods: {
        // 查看文章总列表
        goArticleList() {
            this.$u.route('/pages/article/list/list')
        },
        // 查看文章详情
        goArticleDetail(id) {
            this.$u.route('/pages/article/details/details', { idType: 1, id: id });
        },
        // 前往用户协议
        goUserAgreementPage() {
            var id = this.$store.state.config.userAgreementId;
            this.$u.route('/pages/article/details/details', { idType: 1, id: id });
        },
        // 前往隐私协议
        goUserPrivacyPolicy() {
            var id = this.$store.state.config.privacyPolicyId;
            this.$u.route('/pages/article/details/details', { idType: 1, id: id });
        },
        // 关于我们
        goAboutUs() {
            let id = this.$store.state.config.aboutArticleId;
            this.$u.route('/pages/article/details/details', { idType: 1, id: id });
        },
    }
}





//服务接口信息
export const services = {
    mounted() { },
    methods: {
        // 查看服务详情
        goServicesDetail(serviceId) {
            this.$u.route('/pages/serviceGoods/details/details', { id: serviceId });
        },
        // 查看服务列表
        goServicesList() {
            this.$u.route('/pages/serviceGoods/index/index')
        },
        // 查看个人详情
        goServicesUserDetail(serviceId) {
            this.$u.route('/pages/member/serviceOrder/details/details', { id: serviceId });
        },
    }
}

//用户接口信息
export const users = {
    mounted() { },
    methods: {

    }
}


/**
 * 工具函数
 */

export const tools = {
    methods: {
        doCopyData(data) {
            var _this = this;
            uni.setClipboardData({
                data: data,
                success: function () {
                    _this.$u.toast('复制成功')
                }
            });
        },
        doPhoneCall() {
            var phome = this.$store.state.config.shopMobile || 0;
            if (phome != 0) {
                uni.makePhoneCall({
                    phoneNumber: phome
                });
            }
        },
        goBack() {
            //处理兼容，如果没有上一级界面则返回首页
            const pages = getCurrentPages();
            if (pages.length === 2) {
                uni.navigateBack({
                    delta: 1
                });
            } else if (pages.length === 1) {
                uni.switchTab({
                    url: '/pages/index/default/default',
                })
            } else {
                uni.navigateBack({
                    delta: 1
                });
            }
        },
    }
}
