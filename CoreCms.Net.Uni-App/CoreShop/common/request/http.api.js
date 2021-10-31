
// 此处第二个参数vm，就是我们在页面使用的this，你可以通过vm获取vuex等操作，更多内容详见uView对拦截器的介绍部分：
// https://uviewui.com/js/http.html#%E4%BD%95%E8%B0%93%E8%AF%B7%E6%B1%82%E6%8B%A6%E6%88%AA%EF%BC%9F
const install = (Vue, vm) => {

    // 获取店铺配置
    //let shopConfig = (params = {}) => vm.$u.post('/Api/Common/GetConfig', params, { method: 'common.shopConfig', needToken: false });
    let shopConfigV2 = (params = {}) => vm.$u.post('/Api/Common/GetConfigV2', params, { method: 'common.shopConfigV2', needToken: false });
    //获取商城关键词说明
    let getServiceDescription = (params = {}) => vm.$u.post('/Api/Common/GetServiceDescription', params, { method: 'common.getServiceDescription', needToken: false });
    // 用户信息
    let userInfo = (params = {}) => vm.$u.post('/Api/User/GetUserInfo', params, { method: 'user.info', needToken: true });
    // 上传头像
    let changeAvatar = (params = {}) => vm.$u.post('/Api/User/ChangeAvatar', params, { method: 'user.changeavatar', needToken: true });
    // 编辑用户信息
    let editInfo = (params = {}) => vm.$u.post('/Api/User/EditInfo', params, { method: 'user.editinfo', needToken: true });
    // 发送短信验证码
    let sms = (params = {}) => vm.$u.post('/Api/User/SendSms', params, { method: 'user.sms', needToken: false });
    // 短信验证码登录
    let smsLogin = (params = {}) => vm.$u.post('/Api/User/SmsLogin', params, { method: 'user.smslogin', needToken: false });
    // 退出登录
    let logout = (params = {}) => vm.$u.post('/Api/User/LogOut', params, { method: 'user.logout', needToken: true });
    // 获取首页幻灯片
    let slider = (params = {}) => vm.$u.post('/Api/Advert/GetAdvertList', params, { method: 'advert.getAdvertList', needToken: false });
    // 获取广告
    let advert = (params = {}) => vm.$u.post('/Api/Advert/GetPositionList', params, { method: 'advert.getcarousellists', needToken: false });
    // 获取公告列表
    let notice = (params = {}) => vm.$u.post('/Api/Notice/NoticeList', params, { method: 'notice.noticeList', needToken: false });
    // 获取公告详情
    let noticeInfo = (params = {}) => vm.$u.post('/Api/Notice/NoticeInfo', params, { method: 'notice.noticeInfo', needToken: false });
    // 获取文章详情
    let articleInfo = (params = {}) => vm.$u.post('/Api/Article/GetArticleDetail', params, { method: 'articles.getArticleDetail', needToken: false });
    // 获取文章列表
    let articleList = (params = {}) => vm.$u.post('/Api/Article/GetArticleList', params, { method: 'articles.getArticleList', needToken: false });


    // 获取商品分类
    let categories = (params = {}) => vm.$u.post('/Api/Good/GetAllCategories', params, { method: 'categories.getallcat', needToken: false });
    // 获取商品列表
    let goodsList = (params = {}) => vm.$u.post('/Api/Good/GetGoodsPageList', params, { method: 'goods.goodsList', needToken: false });
    // 获取商品详情
    let goodsDetail = (params = {}) => vm.$u.post('/Api/Good/GetDetial', params, { method: 'goods.getdetial', needToken: false });
    //获取随机推荐商品
    let getGoodsRecommendList = (params = {}) => vm.$u.post('/Api/Good/GetGoodsRecommendList', params, { method: 'goods.getGoodsRecommendList', needToken: false });
    // 获取商品详情
    let goodsDetailByToken = (params = {}) => vm.$u.post('/Api/Good/GetDetialByToken', params, { method: 'goods.getDetialByToken', needToken: true });
    // 获取商品参数信息
    let goodsParams = (params = {}) => vm.$u.post('/Api/Good/GetGoodsParams', params, { method: 'goods.getgoodsparams', needToken: false });
    // 获取设置默认货品
    let getProductInfo = (params = {}) => vm.$u.post('/Api/Good/GetProductInfo', params, { method: 'goods.getproductinfo', needToken: false });
    // 获取商品评论信息
    let goodsComment = (params = {}) => vm.$u.post('/Api/Good/GetGoodsComment', params, { method: 'goods.getgoodscomment', needToken: false });




    // 添加购物车
    let addCart = (params = {}) => vm.$u.post('/Api/Cart/AddCart', params, { method: 'cart.add', needToken: true });
    // 移除购物车
    let removeCart = (params = {}) => vm.$u.post('/Api/Cart/DoDelete', params, { method: 'cart.del', needToken: true });
    // 获取购物车列表
    let cartList = (params = {}) => vm.$u.post('/Api/Cart/GetList', params, { method: 'cart.getlist', needToken: true });
    // 设置购物车商品数量
    let setCartNum = (params = {}) => vm.$u.post('/Api/Cart/SetCartNum', params, { method: 'cart.setnums', needToken: true });
    // 获取购物车数量
    let getCartNum = (params = {}) => vm.$u.post('/Api/User/GetCartNumber', params, { method: 'cart.getnumber', needToken: true });
    // 根据购物车已有数据获取能够使用的优惠券
    let getCartCoupon = (params = {}) => vm.$u.post('/Api/Cart/GetCartAvailableCoupon', params, { method: 'cart.getCartCoupon', needToken: true });

    // 获取用户的收货地址列表
    let userShip = (params = {}) => vm.$u.post('/Api/User/GetUserShip', params, { method: 'user.getusership', needToken: true });
    // 获取用户默认收货地址
    let userDefaultShip = (params = {}) => vm.$u.post('/Api/User/GetUserDefaultShip', params, { method: 'user.getuserdefaultship', needToken: true });
    // 存储用户收货地址
    let saveUserShip = (params = {}) => vm.$u.post('/Api/User/SaveUserShip', params, { method: 'user.vuesaveusership', needToken: true });
    // 微信存储收货地址
    let saveUserShipWx = (params = {}) => vm.$u.post('/Api/User/SaveUserShip', params, { method: 'user.saveusership', needToken: true });
    //获取区域ID
    let getAreaId = (params = {}) => vm.$u.post('/Api/User/GetAreaId', params, { method: 'user.getareaid', needToken: false });
    // 获取收货地址详情
    let shipDetail = (params = {}) => vm.$u.post('/Api/User/GetShipDetail', params, { method: 'user.getshipdetail', needToken: true });
    // 收货地址编辑
    let editShip = (params = {}) => vm.$u.post('/Api/User/SaveUserShip', params, { method: 'user.editship', needToken: true });
    // 收货地址删除
    let removeShip = (params = {}) => vm.$u.post('/Api/User/RemoveShip', params, { method: 'user.removeship', needToken: true });
    // 设置默认收货地址
    let setDefShip = (params = {}) => vm.$u.post('/Api/User/SetDefShip', params, { method: 'user.setdefship', needToken: true });

    // 生成订单
    let createOrder = (params = {}) => vm.$u.post('/Api/Order/CreateOrder', params, { method: 'order.create', needToken: true });
    // 取消订单
    let cancelOrder = (params = {}) => vm.$u.post('/Api/Order/CancelOrder', params, { method: 'order.cancel', needToken: true });
    // 删除订单
    let delOrder = (params = {}) => vm.$u.post('/Api/Order/DeleteOrder', params, { method: 'order.del', needToken: true });
    // 获取订单详情
    let orderDetail = (params = {}) => vm.$u.post('/Api/Order/OrderDetails', params, { method: 'order.details', needToken: true });
    // 确认收货
    let confirmOrder = (params = {}) => vm.$u.post('/Api/Order/OrderConfirm', params, { method: 'order.confirm', needToken: true });
    // 获取配送方式
    let orderShip = (params = {}) => vm.$u.post('/Api/Order/GetShip', params, { method: 'order.getship', needToken: true });
    // 获取全部订单列表
    let orderList = (params = {}) => vm.$u.post('/Api/Order/GetOrderList', params, { method: 'order.getorderlist', needToken: true });
    // 获取订单不同状态的数量
    let getOrderStatusSum = (params = {}) => vm.$u.post('/Api/Order/GetOrderStatusNum', params, { method: 'order.getorderstatusnum', needToken: true });

    // 售后单列表
    let afterSalesList = (params = {}) => vm.$u.post('/Api/Order/AftersalesList', params, { method: 'order.aftersaleslist', needToken: true });
    // 售后单详情
    let afterSalesInfo = (params = {}) => vm.$u.post('/Api/Order/Aftersalesinfo', params, { method: 'order.aftersalesinfo', needToken: true });
    // 添加售后单
    let addAfterSales = (params = {}) => vm.$u.post('/Api/Order/AddAftersales', params, { method: 'order.addaftersales', needToken: true });
    // 用户发送退货包裹
    let sendShip = (params = {}) => vm.$u.post('/Api/Order/SendReship', params, { method: 'order.sendreship', needToken: true });

    // 添加商品浏览足迹
    let addGoodsBrowsing = (params = {}) => vm.$u.post('/Api/User/AddGoodsBrowsing', params, { method: 'user.addgoodsbrowsing', needToken: true });
    // 删除商品浏览足迹
    let delGoodsBrowsing = (params = {}) => vm.$u.post('/Api/User/DelGoodsBrowsing', params, { method: 'user.delgoodsbrowsing', needToken: true });
    // 获取商品浏览足迹
    let goodsBrowsing = (params = {}) => vm.$u.post('/Api/User/Goodsbrowsing', params, { method: 'user.goodsbrowsing', needToken: true });
    // 商品收藏 关注/取消
    let goodsCollection = (params = {}) => vm.$u.post('/Api/User/GoodsCollectionCreateOrDelete', params, { method: 'user.goodscollection', needToken: true });
    // 获取商品收藏关注列表
    let goodsCollectionList = (params = {}) => vm.$u.post('/Api/User/GoodscollectionList', params, { method: 'user.goodscollectionlist', needToken: true });

    // 获取店铺支付方式列表
    let paymentList = (params = {}) => vm.$u.post('/Api/Payments/GetList', params, { method: 'payments.getlist', needToken: false });
    // 获取支付单详情
    let paymentInfo = (params = {}) => vm.$u.post('/Api/Payments/GetInfo', params, { method: 'payments.getinfo', needToken: true });
    // 支付接口
    let pay = (params = {}) => vm.$u.post('/Api/User/Pay', params, { method: 'user.pay', needToken: true });
    // 订单评价接口
    let orderEvaluate = (params = {}) => vm.$u.post('/Api/User/OrderEvaluate', params, { method: 'user.orderevaluate', needToken: true });
    // 判断是否签到
    let isSign = (params = {}) => vm.$u.post('/Api/User/IsSign', params, { method: 'user.issign', needToken: true });
    // 签到接口
    let sign = (params = {}) => vm.$u.post('/Api/User/Sign', params, { method: 'user.sign', needToken: true });
    // 积分记录
    let pointLog = (params = {}) => vm.$u.post('/Api/User/UserPointLog', params, { method: 'user.userpointlog', needToken: true });
    // 物流信息接口
    let logistics = (params = {}) => vm.$u.post('/Api/Order/LogisticsByApi', params, { method: 'order.logisticbyapi', needToken: true });
    // 优惠券列表
    let couponList = (params = {}) => vm.$u.post('/Api/Coupon/CouponList', params, { method: 'coupon.couponlist', needToken: false });
    // 优惠券详情
    let couponDetail = (params = {}) => vm.$u.post('/Api/Coupon/CouponDetail', params, { method: 'coupon.coupondetail', needToken: false });
    // 用户领取优惠券
    let getCoupon = (params = {}) => vm.$u.post('/Api/Coupon/GetCoupon', params, { method: 'coupon.getcoupon', needToken: true });
    // 用户已领取的优惠券列表
    let userCoupon = (params = {}) => vm.$u.post('/Api/Coupon/UserCoupon', params, { method: 'coupon.usercoupon', needToken: true });
    // 获取我的银行卡列表
    let getBankCardList = (params = {}) => vm.$u.post('/Api/User/GetMyBankcardsList', params, { method: 'user.getbankcardlist', needToken: true });
    // 获取默认的银行卡
    let getDefaultBankCard = (params = {}) => vm.$u.post('/Api/User/GetDefaultBankCard', params, { method: 'user.getdefaultbankcard', needToken: true });
    // 添加银行卡
    let addBankCard = (params = {}) => vm.$u.post('/Api/User/AddBankCards', params, { method: 'user.addbankcard', needToken: true });
    // 删除银行卡
    let removeBankCard = (params = {}) => vm.$u.post('/Api/User/Removebankcard', params, { method: 'user.removebankcard', needToken: true });
    // 设置默认银行卡
    let setDefaultBankCard = (params = {}) => vm.$u.post('/Api/User/SetDefaultBankCard', params, { method: 'user.setdefaultbankcard', needToken: true });
    // 获取银行卡信息
    let getBankCardInfo = (params = {}) => vm.$u.post('/Api/User/GetBankCardInfo', params, { method: 'user.getbankcardinfo', needToken: true });
    // 获取银行卡组织信息
    let getBankCardOrganization = (params = {}) => vm.$u.post('/Api/User/GetBankCardsOrganization', params, { method: 'user.getbankcardorganization', needToken: true });
    // 用户修改密码
    let editPwd = (params = {}) => vm.$u.post('/Api/User/EditPwd', params, { method: 'user.editpwd', needToken: true });
    // 用户找回密码
    let forgotPwd = (params = {}) => vm.$u.post('/Api/Common/InterFaceTest', params, { method: 'user.forgotpwd', needToken: true });
    // 获取用户余额明细
    let getBalanceList = (params = {}) => vm.$u.post('/Api/User/UserBalance', params, { method: 'user.balancelist', needToken: true });
    // 用户推荐列表
    let recommendUserList = (params = {}) => vm.$u.post('/Api/User/Recommend', params, { method: 'user.recommend', needToken: true });
    // 邀请码
    let shareCode = (params = {}) => vm.$u.post('/Api/User/ShareCode', params, { method: 'user.sharecode', needToken: true });
    // 用户提现
    let userToCash = (params = {}) => vm.$u.post('/Api/User/Cash', params, { method: 'user.cash', needToken: true });
    // 用户提现列表
    let cashList = (params = {}) => vm.$u.post('/Api/User/CashList', params, { method: 'user.cashlist', needToken: true });
    // 判断用户下单可以使用多少积分
    let usablePoint = (params = {}) => vm.$u.post('/Api/User/GetUserPoint', params, { method: 'user.getuserpoint', needToken: true });

    // 门店列表
    let storeList = (params = {}) => vm.$u.post('/Api/Store/GetStoreList', params, { method: 'store.getstorelist', needToken: false });
    //根据用户序列获取门店数据
    let getStoreByUserId = (params = {}) => vm.$u.post('/Api/Store/GetStoreByUserId', params, { method: 'store.getStoreByUserId', needToken: true });
    //根据序列获取门店数据
    let getStoreById = (params = {}) => vm.$u.post('/Api/Store/GetStoreById', params, { method: 'store.getStoreByUserId', needToken: false });
    //获取门店订单列表
    let getOrderPageByMerchant = (params = {}) => vm.$u.post('/Api/Store/GetOrderPageByMerchant', params, { method: 'store.getOrderPageByMerchant', needToken: true });
    //获取门店订单列表
    let getOrderPageByMerchantSearch = (params = {}) => vm.$u.post('/Api/Store/GetOrderPageByMerchantSearch', params, { method: 'store.getOrderPageByMerchantSearch', needToken: true });

    // 判断是否开启门店自提
    let switchStore = (params = {}) => vm.$u.post('/Api/Store/GetStoreSwitch', params, { method: 'store.getstoreswitch', needToken: false });
    // 获取默认的门店
    let defaultStore = (params = {}) => vm.$u.post('/Api/Store/GetDefaultStore', params, { method: 'store.getdefaultstore', needToken: false });
    // 判断是否开启积分
    let isPoint = (params = {}) => vm.$u.post('/Api/User/isPoint', params, { method: 'user.ispoint', needToken: false });
    // 用户输入code领取优惠券
    let couponKey = (params = {}) => vm.$u.post('/Api/Coupon/GetCouponKey', params, { method: 'coupon.getcouponkey', needToken: true });
    // 判断是否是店员
    let isStoreUser = (params = {}) => vm.$u.post('/Api/Store/IsClerk', params, { method: 'store.isclerk', needToken: true });
    // 获取店铺提货单列表
    let storeLadingList = (params = {}) => vm.$u.post('/Api/Store/StoreLadingList', params, { method: 'store.storeladinglist', needToken: true });
    // 获取提货单详情
    let ladingInfo = (params = {}) => vm.$u.post('/Api/Store/LadingInfo', params, { method: 'store.ladinginfo', needToken: true });
    // 店铺提单核销操作
    let ladingExec = (params = {}) => vm.$u.post('/Api/Store/Lading', params, { method: 'store.lading', needToken: true });
    // 提货单删除
    let ladingDel = (params = {}) => vm.$u.post('/Api/Store/LadingDelete', params, { method: 'store.ladingdel', needToken: true });



    // 获取活动列表
    let activityList = (params = {}) => vm.$u.post('/Api/Group/GetList', params, { method: 'group.getlist', needToken: false });
    // 获取活动详情
    let activityDetail = (params = {}) => vm.$u.post('/Api/Group/GetGoodsDetial', params, { method: 'group.getgoodsdetial', needToken: false });
    //小程序解析code
    let onLogin = (params = {}) => vm.$u.post('/Api/User/OnLogin', params, { method: 'user.wxappOnlogin', needToken: false });
    //小程序登录第二步（核验数据并获取用户详细资料）
    let loginByDecodeEncryptedData = (params = {}) => vm.$u.post('/Api/User/DecodeEncryptedData', params, { method: 'user.wxapploginByDecodeEncryptedData', needToken: false });
    //小程序同步用户数据
    let syncWeChatInfo = (params = {}) => vm.$u.post('/Api/User/SyncWeChatInfo', params, { method: 'user.SyncWeChatInfo', needToken: true });
    //小程序手机授权（拉取手机号码）
    let loginByGetPhoneNumber = (params = {}) => vm.$u.post('/Api/User/DecryptPhoneNumber', params, { method: 'user.wxapploginByGetPhoneNumber', needToken: false });
    //取下级地址列表
    let getAreaList = (params = {}) => vm.$u.post('/Api/Common/GetAreas', params, { method: 'user.getarealist', needToken: false });
    //取搜索页推荐关键字
    let getRecommendKeys = (params = {}) => vm.$u.post('/Api/Common/GetRecommendKeys', params, { method: 'common.getrecommendkeys', needToken: false });
    // 获取我的邀请信息
    let myInvite = (params = {}) => vm.$u.post('/Api/User/MyInvite', params, { method: 'user.myinvite', needToken: true });
    // 设置我的上级邀请人
    let setMyInvite = (params = {}) => vm.$u.post('/Api/User/SetMyInvite', params, { method: 'user.SetMyInvite', needToken: true });
    // 获取我的上级邀请人
    let getMyInvite = (params = {}) => vm.$u.post('/Api/User/GetMyInvite', params, { method: 'user.GetMyInvite', needToken: true });
    //获取我的下级发展用户数量
    let getMyChildSum = (params = {}) => vm.$u.post('/Api/User/GetMyChildSum', params, { method: 'user.GetMyChildSum', needToken: true });
    // 获取秒杀团购
    let getGroup = (params = {}) => vm.$u.post('/Api/Group/GetList', params, { method: 'group.getlist', needToken: false });
    // 获取秒杀团购详情
    let groupInfo = (params = {}) => vm.$u.post('/Api/Group/GetGoodsDetial', params, { method: 'group.getgoodsdetial', needToken: false });
    // 自定义页面
    let getPageConfig = (params = {}) => vm.$u.post('/Api/Page/GetPageConfig', params, { method: 'pages.getpageconfig', needToken: false });


    // 获取分销商进度状态
    let getDistributionInfo = (params = {}) => vm.$u.post('/Api/Distribution/Info', params, { method: 'distribution_center-api-info', needToken: true });
    // 申请分销商
    let applyDistribution = (params = {}) => vm.$u.post('/Api/Distribution/ApplyDistribution', params, { method: 'distribution_center-api-applydistribution', needToken: true });
    // 店铺设置
    let setDistributionStore = (params = {}) => vm.$u.post('/Api/Distribution/SetStore', params, { method: 'distribution_center-api-setstore', needToken: true });
    //获取店铺信息
    let getDistributionStoreInfo = (params = {}) => vm.$u.post('/Api/Distribution/GetStoreInfo', params, { method: 'distribution_center-api-getstoreinfo', needToken: false });
    //我的分销订单
    let getDistributionOrder = (params = {}) => vm.$u.post('/Api/Distribution/MyOrder', params, { method: 'distribution_center-api-myorder', needToken: true });
    //分销团队统计
    let getDistributionTeamSum = (params = {}) => vm.$u.post('/Api/Distribution/GetTeamSum', params, { method: 'distribution.getTeamSum', needToken: true });
    //分销订单统计
    let getDistributionOrderSum = (params = {}) => vm.$u.post('/Api/Distribution/GetOrderSum', params, { method: 'distribution.getOrderSum', needToken: true });
    //获取分销商排行
    let getDistributionRanking = (params = {}) => vm.$u.post('/Api/Distribution/getDistributionRanking', params, { method: 'distribution.getDistributionRanking', needToken: true });

    // 获取代理商进度状态
    let getAgentInfo = (params = {}) => vm.$u.post('/Api/Agent/Info', params, { method: 'agent_center-api-info', needToken: true });
    // 申请代理商
    let applyAgent = (params = {}) => vm.$u.post('/Api/Agent/ApplyAgent', params, { method: 'agent_center-api-applyAgent', needToken: true });
    // 店铺设置
    let setAgentStore = (params = {}) => vm.$u.post('/Api/Agent/SetStore', params, { method: 'agent_center-api-setstore', needToken: true });
    //获取店铺信息
    let getAgentStoreInfo = (params = {}) => vm.$u.post('/Api/Agent/GetStoreInfo', params, { method: 'agent_center-api-getstoreinfo', needToken: false });
    //我的代理订单
    let getAgentOrder = (params = {}) => vm.$u.post('/Api/Agent/MyOrder', params, { method: 'agent_center-api-myorder', needToken: true });
    //代理团队统计
    let getAgentTeamSum = (params = {}) => vm.$u.post('/Api/Agent/GetTeamSum', params, { method: 'agent.getTeamSum', needToken: true });
    //代理订单统计
    let getAgentOrderSum = (params = {}) => vm.$u.post('/Api/Agent/GetOrderSum', params, { method: 'agent.getOrderSum', needToken: true });
    //获取代理池商品数据
    let getAgentGoodsPageList = (params = {}) => vm.$u.post('/Api/Agent/GetGoodsPageList', params, { method: 'agent.getGoodsPageList', needToken: false });
    //获取代理商排行
    let getAgentRanking = (params = {}) => vm.$u.post('/Api/Agent/GetAgentRanking', params, { method: 'agent.getAgentRanking', needToken: true });


    // 拼团列表
    let pinTuanList = (params = {}) => vm.$u.post('/Api/PinTuan/GetList', params, { method: 'pinTuan.list', needToken: false });
    // 拼团商品详情
    let pinTuanGoodsInfo = (params = {}) => vm.$u.post('/Api/PinTuan/GetGoodsInfo', params, { method: 'pinTuan.goodsinfo', needToken: false });
    // 拼团货品详情
    let pinTuanProductInfo = (params = {}) => vm.$u.post('/Api/PinTuan/GetProductInfo', params, { method: 'pinTuan.productinfo', needToken: false });
    //获取我的发票列表
    let myInvoiceList = (params = {}) => vm.$u.post('/Api/User/UserInvoiceList', params, { method: 'user.myinvoicelist', needToken: true });
    //获取支付信息
    let paymentsCheckpay = (params = {}) => vm.$u.post('/Api/Payments/CheckPay', params, { method: 'payments.checkpay', needToken: true });
    //忘记密码
    let userForgetpwd = (params = {}) => vm.$u.post('/Api/User/ForgetPwd', params, { method: 'user.forgetpwd', needToken: false });
    // 根据订单id取拼团信息，用在订单详情页
    let getOrderPinTuanTeamInfo = (params = {}) => vm.$u.post('/Api/PinTuan/GetPinTuanTeam', params, { method: 'pinTuan.pinTuanteam', needToken: true });
    //发票模糊查询
    let getTaxInfo = (params = {}) => vm.$u.post('/Api/Order/GetTaxCode', params, { method: 'order.gettaxcode', needToken: true });


    // 获取店铺设置
    let getSetting = (params = {}) => vm.$u.post('/Api/User/GetSetting', params, { method: 'user.getsetting', needToken: false });
    // 获取商户配置信息
    let getSellerSetting = (params = {}) => vm.$u.post('/Api/User/GetSellerSetting', params, { method: 'user.getsellersetting', needToken: false });
    // 获取小程序二维码
    let getInviteQRCode = (params = {}) => vm.$u.post('/Api/Store/GetInviteQrCode', params, { method: 'store.getinviteqrcode', needToken: false });
    // 生成海报
    let createPoster = (params = {}) => vm.$u.post('/Api/User/GetPoster', params, { method: 'user.getposter', needToken: false });
    //============================================================//万能表单
    let getFormDetial = (params = {}) => vm.$u.post('/Api/Form/GetFormDetial', params, { method: 'form.getformdetial', needToken: false });
    //============================================================//提交表单
    let addSubmitForm = (params = {}) => vm.$u.post('/Api/Form/AddSubmit', params, { method: 'form.addsubmit', needToken: false });

    //================================================================////抽奖规则
    let lotteryConfig = (params = {}) => vm.$u.post('/Api/Lottery/GetLotteryConfig', params, { method: 'lottery-api-getLotteryConfig', needToken: true });
    //================================================================////抽奖操作
    let lottery = (params = {}) => vm.$u.post('/Api/Lottery/Lottery', params, { method: 'lottery-api-lottery', needToken: true });
    //================================================================////获取我的抽奖记录
    let myLottery = (params = {}) => vm.$u.post('/Api/Lottery/LotteryLog', params, { method: 'lottery-api-lotteryLog', needToken: true });
    //================================================================////生成分享URL
    let createShareUrl = (params = {}) => vm.$u.post('/Api/User/ShareUrl', params, { method: 'user.shareurl', needToken: false });
    //================================================================////微信图文消息
    let messageDetail = (params = {}) => vm.$u.post('/Api/Articles/GetWeChatMessage', params, { method: 'articles.getweixinmessage', needToken: false });
    //================================================================////获取APP版本
    let getAppVersion = (params = {}) => vm.$u.post('/Api/App/CheckVersion', params, { method: 'app-api-checkVersion', needToken: false });

    //============================================================//公众号授权获取openid（第三方登录）
    let getOpenId = (params = {}) => vm.$u.post('/Api/User/OfficialLogin', params, { method: 'user.officiallogin', needToken: false });
    //============================================================// 获取授权登录方式（获取第三方登录列表）
    let getTrustLogin = (params = {}) => vm.$u.post('/Api/User/GetTrustLogin', params, { method: 'user.gettrustlogin', needToken: false });
    //============================================================// APP信任登录(app第三方登录方式)
    let appTrustLogin = (params = {}) => vm.$u.post('/Api/User/UniAppLogin', params, { method: 'user.uniapplogin', needToken: false });
    //================================================================//// 绑定授权登录
    let trustBind = (params = {}) => vm.$u.post('/Api/User/TrustBind', params, { method: 'user.trustbind', needToken: false });
    //================================================================//支付宝小程序解析code（第三方支付宝登录方式）
    let alilogin1 = (params = {}) => vm.$u.post('/Api/User/AlipayAppLogin1', params, { method: 'user.alipayapplogin1', needToken: false });
    //================================================================////头条小程序登录
    let ttlogin = (params = {}) => vm.$u.post('/Api/User/TtLogin', params, { method: 'user.ttlogin', needToken: false });
    //获取订阅模板
    let getSubscriptionTmplIds = (params = {}) => vm.$u.post('/Api/WeChatAppletsMessage/Tmpl', params, { method: 'wechat_applets_message-api-tmpl', needToken: true });
    //订阅状态修改
    let setSubscriptionStatus = (params = {}) => vm.$u.post('/Api/WeChatAppletsMessage/SetTip', params, { method: 'wechat_applets_message-api-settip', needToken: true });
    //用户关闭订阅提醒
    let subscriptionCloseTip = (params = {}) => vm.$u.post('/Api/WeChatAppletsMessage/CloseTip', params, { method: 'wechat_applets_message-api-closetip', needToken: true });
    //判断用户是否需要显示订阅提醒
    let subscriptionIsTip = (params = {}) => vm.$u.post('/Api/WeChatAppletsMessage/IsTip', params, { method: 'wechat_applets_message-api-istip', needToken: true });
    //统一分享
    let share = (params = {}) => vm.$u.post('/Api/User/Share', params, { method: 'user.share', needToken: false });
    //统一分享解码
    let deshare = (params = {}) => vm.$u.post('/Api/User/deshare', params, { method: 'user.deshare', needToken: false });

    //获取服务列表
    let getServicelist = (params = {}) => vm.$u.post('/Api/Service/GetPageList', params, { method: 'service.getpagelist', needToken: false });
    //获取服务详情
    let getServiceDetail = (params = {}) => vm.$u.post('/Api/Service/GetDetails', params, { method: 'service.getdetail', needToken: false });
    //生成服务购买订单
    let addServiceOrder = (params = {}) => vm.$u.post('/Api/Service/AddServiceOrder', params, { method: 'service.addServiceOrder', needToken: true });

    //获取个人服务订单列表
    let getUserServicesPageList = (params = {}) => vm.$u.post('/Api/User/GetServicesPageList', params, { method: 'user.getServicesPageList', needToken: true });
    //获取服务卡下用户券列表
    let getServicesTickets = (params = {}) => vm.$u.post('/Api/User/GetServicesTickets', params, { method: 'user.getServicesTickets', needToken: true });


    //门店核销的服务券列表
    let getverificationPageList = (params = {}) => vm.$u.post('/Api/Service/VerificationPageList', params, { method: 'service.verificationPageList', needToken: true });
    //删除核销券
    let serviceLogDelete = (params = {}) => vm.$u.post('/Api/Service/LogDelete', params, { method: 'service.logDelete', needToken: true });
    // 获取服务券详情准备核销
    let getServiceVerificationTicketInfo = (params = {}) => vm.$u.post('/Api/Service/GetTicketInfo', params, { method: 'service.getTicketInfo', needToken: true });
    //核销服务券
    let serviceVerificationTicket = (params = {}) => vm.$u.post('/Api/Service/VerificationTicket', params, { method: 'service.verificationTicket', needToken: true });


    // 用户注册（废弃，改为自动获取app数据及使用短信验证码登录）建议直接使用smsLogin接口
    //let reg  = (params = {}) => vm.$u.post('/Api/Common/InterFaceTest', params, { method: 'user.reg', needToken: true });
    // 用户登录(废弃，改为短信验证码登录)
    //let login  = (params = {}) => vm.$u.post('/Api/Common/InterFaceTest', params, { method: 'user.login', needToken: true });
    // 获取用户信息(废弃)
    // let trustLogin  = (params = {}) => vm.$u.post('/Api/Common/InterFaceTest', params, { method: 'user.trustcallback', needToken: true });
    // 订单售后状态(废弃方法，建议直接用order.details接口)
    // let afterSalesStatus  = (params = {}) => vm.$u.post('/Api/Common/InterFaceTest', params, { method: 'order.aftersalesstatus', needToken: true });
    // 我的积分（弃用）
    //let myPoint  = (params = {}) => vm.$u.post('/Api/Common/InterFaceTest', params, { method: 'user.mypoint', needToken: true });

    // 将各个定义的接口名称，统一放进对象挂载到vm.$u.api(因为vm就是this，也即this.$u.api)下
    vm.$u.api = {
        shopConfigV2,
        getServiceDescription,
        userInfo,
        changeAvatar,
        editInfo,
        sms,
        smsLogin,
        logout,
        slider,
        advert,
        notice,
        noticeInfo,
        articleInfo,
        articleList,

        categories,
        goodsList,
        goodsDetail,
        getGoodsRecommendList,
        goodsDetailByToken,
        goodsParams,
        getProductInfo,
        goodsComment,

        addCart,
        removeCart,
        cartList,
        setCartNum,
        getCartNum,
        getCartCoupon,
        userShip,
        userDefaultShip,
        saveUserShip,
        saveUserShipWx,
        getAreaId,
        shipDetail,
        editShip,
        removeShip,
        setDefShip,
        createOrder,
        cancelOrder,
        delOrder,
        orderDetail,
        confirmOrder,
        orderShip,
        orderList,
        getOrderStatusSum,
        afterSalesList,
        afterSalesInfo,
        addAfterSales,
        sendShip,
        addGoodsBrowsing,
        delGoodsBrowsing,
        delGoodsBrowsing,
        goodsBrowsing,
        goodsCollection,
        goodsCollectionList,
        paymentList,
        paymentInfo,
        pay,
        orderEvaluate,
        isSign,
        sign,
        pointLog,
        logistics,
        couponList,
        couponDetail,
        getCoupon,
        userCoupon,
        getBankCardList,
        getDefaultBankCard,
        addBankCard,
        removeBankCard,
        setDefaultBankCard,
        getBankCardInfo,
        getBankCardOrganization,
        editPwd,
        forgotPwd,
        getBalanceList,
        recommendUserList,
        shareCode,
        userToCash,
        cashList,
        usablePoint,

        storeList,
        getStoreByUserId,
        getStoreById,
        getOrderPageByMerchant,
        getOrderPageByMerchantSearch,
        switchStore,
        defaultStore,
        isPoint,
        couponKey,
        isStoreUser,
        storeLadingList,

        ladingInfo,
        ladingExec,
        ladingDel,
        activityList,
        activityDetail,
        onLogin,
        loginByDecodeEncryptedData,
        syncWeChatInfo,
        loginByGetPhoneNumber,
        getAreaList,
        getRecommendKeys,
        myInvite,
        setMyInvite,
        getMyInvite,
        getMyChildSum,
        getGroup,
        groupInfo,
        getPageConfig,

        getDistributionInfo,
        applyDistribution,
        setDistributionStore,
        getDistributionStoreInfo,
        getDistributionOrder,
        getDistributionTeamSum,
        getDistributionOrderSum,
        getDistributionRanking,

        getAgentInfo,
        applyAgent,
        setAgentStore,
        getAgentStoreInfo,
        getAgentOrder,
        getAgentTeamSum,
        getAgentOrderSum,
        getAgentGoodsPageList,
        getAgentRanking,

        pinTuanList,
        pinTuanGoodsInfo,
        pinTuanProductInfo,
        myInvoiceList,
        paymentsCheckpay,
        userForgetpwd,
        getOrderPinTuanTeamInfo,
        getTaxInfo,
        getSetting,
        getSellerSetting,
        getInviteQRCode,
        createPoster,
        getFormDetial,
        addSubmitForm,
        lotteryConfig,
        lottery,
        myLottery,
        createShareUrl,
        messageDetail,
        getAppVersion,
        getOpenId,
        getTrustLogin,
        appTrustLogin,
        trustBind,
        ttlogin,
        alilogin1,
        getSubscriptionTmplIds,
        setSubscriptionStatus,
        subscriptionCloseTip,
        subscriptionIsTip,
        share,
        deshare,
        getServicelist,
        getServiceDetail,
        addServiceOrder,
        getUserServicesPageList,
        getServicesTickets,
        getverificationPageList,
        serviceLogDelete,
        getServiceVerificationTicketInfo,
        serviceVerificationTicket
    };
}

export default {
    install
}