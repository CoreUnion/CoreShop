/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-03-03 3:24:15
 *        Description: 暂无
 ***********************************************************************/


namespace CoreCms.Net.Configuration
{
    /// <summary>
    /// 平台设置字段缓存名称定义
    /// </summary>
    public static class SystemSettingConstVars
    {
        /// <summary>
        /// 平台名称
        /// </summary>
        public const string ShopName = "shopName";

        /// <summary>
        /// 平台描述
        /// </summary>
        public const string ShopDesc = "shopDesc";

        /// <summary>
        /// 平台地址
        /// </summary>
        public const string ShopAddress = "shopAddress";

        /// <summary>
        /// 备案信息
        /// </summary>
        public const string ShopBeiAn = "shopBeiAn";

        /// <summary>
        /// 平台logo
        /// </summary>
        public const string ShopLogo = "shopLogo";

        /// <summary>
        /// Favicon图标
        /// </summary>
        public const string ShopFavicon = "shopFavicon";

        /// <summary>
        /// 默认图
        /// </summary>
        public const string ShopDefaultImage = "shopDefaultImage";

        /// <summary>
        /// 商家手机号
        /// </summary>
        public const string ShopMobile = "shopMobile";

        /// <summary>
        /// 开启门店自提
        /// </summary>
        public const string StoreSwitch = "storeSwitch";

        /// <summary>
        /// 分类样式
        /// </summary>
        public const string CateStyle = "cateStyle";

        /// <summary>
        /// H5分类样式
        /// </summary>
        public const string CateType = "cateType";

        /// <summary>
        /// 订单取消时间
        /// </summary>
        public const string OrderCancelTime = "orderCancelTime";

        /// <summary>
        /// 订单完成时间
        /// </summary>
        public const string OrderCompleteTime = "orderCompleteTime";

        /// <summary>
        /// 订单确认收货时间
        /// </summary>
        public const string OrderAutoSignTime = "orderAutoSignTime";

        /// <summary>
        /// 订单自动评价时间
        /// </summary>
        public const string OrderAutoEvalTime = "orderAutoEvalTime";

        /// <summary>
        /// 订单提醒付款时间
        /// </summary>
        public const string RemindOrderTime = "remindOrderTime";

        /// <summary>
        /// 门店订单自动发货
        /// </summary>
        public const string StoreOrderAutomaticDelivery = "storeOrderAutomaticDelivery";

        //分销功能（老分销）=============================================================
        /// <summary>
        /// 是否开启分销
        /// </summary>
        public const string OpenDistribution = "openDistribution";
        /// <summary>
        /// 用户须知：成为分销商后，可以获取佣金，用户只可被推荐一次，越早推荐越返利越多哦。
        /// </summary>
        public const string DistributionNotes = "distributionNotes";
        /// <summary>
        /// 分销协议
        /// </summary>
        public const string DistributionAgreement = "distributionAgreement";
        /// <summary>
        /// 是否开启店铺
        /// </summary>
        public const string DistributionStore = "distributionStore";

        /// <summary>
        /// 显示邀请人信息
        /// </summary>
        public const string ShowInviterInfo = "showInviterInfo";


        /// <summary>
        /// 分销层级1,2层
        /// </summary>
        public const string DistributionLevel = "distributionLevel";
        /// <summary>
        /// 成为分销商条件：1无条件(需要审核),2申请(需要审核),3无条件
        /// </summary>
        public const string DistributionType = "distributionType";
        /// <summary>
        /// 消费自动成为分销商：元
        /// </summary>
        public const string DistributionMoney = "distributionMoney";
        /// <summary>
        /// 购买商品成为分销商：1关闭，2任意商品，3指定商品
        /// </summary>
        public const string DistributionGoods = "distributionGoods";
        /// <summary>
        /// 购买商品成为分销商指定商品序列号
        /// </summary>
        public const string DistributionGoodsId = "distributionGoodsId";
        /// <summary>
        /// 佣金类型：1百分比，2固定金额
        /// </summary>
        public const string CommissionType = "commissionType";
        /// <summary>
        /// 一级佣金
        /// </summary>
        public const string CommissionFirst = "commissionFirst";
        /// <summary>
        /// 二级佣金
        /// </summary>
        public const string CommissionSecond = "commissionSecond";
        /// <summary>
        /// 三级佣金
        /// </summary>
        public const string CommissionThird = "commissionThird";


        /// <summary>
        /// 库存警报数量
        /// </summary>
        public const string GoodsStocksWarn = "goodsStocksWarn";



        /// <summary>
        /// 退货联系人
        /// </summary>
        public const string ReshipName = "reshipName";

        /// <summary>
        /// 退货联系方式
        /// </summary>
        public const string ReshipMobile = "reshipMobile";

        /// <summary>
        /// 退货区域
        /// </summary>
        public const string ReshipAreaId = "reshipAreaId";

        /// <summary>
        /// 退货详细地址
        /// </summary>
        public const string ReshipAddress = "reshipAddress";

        /// <summary>
        /// 退货坐标
        /// </summary>
        public const string ReshipCoordinate = "reshipCoordinate";

        /// <summary>
        /// 签到奖励类型
        /// </summary>
        public const string SignPointType = "signPointType";

        /// <summary>
        /// 随机奖励积分最小值
        /// </summary>
        public const string SignRandomMin = "signRandomMin";

        /// <summary>
        /// 随机奖励积分最大值
        /// </summary>
        public const string SignRandomMax = "signRandomMax";

        /// <summary>
        /// 首次奖励积分
        /// </summary>
        public const string FirstSignPoint = "firstSignPoint";

        /// <summary>
        /// 连续签到追加
        /// </summary>
        public const string ContinuitySignAdditional = "continuitySignAdditional";

        /// <summary>
        /// 单日最大奖励
        /// </summary>
        public const string SignMostPoint = "signMostPoint";

        /// <summary>
        /// 开启积分功能
        /// </summary>
        public const string PointSwitch = "pointSwitch";

        /// <summary>
        /// 订单积分折现比例
        /// </summary>
        public const string PointDiscountedProportion = "pointDiscountedProportion";

        /// <summary>
        /// 订单积分使用比例
        /// </summary>
        public const string OrdersPointProportion = "ordersPointProportion";

        /// <summary>
        /// 订单积分奖励比例
        /// </summary>
        public const string OrdersRewardProportion = "ordersRewardProportion";

        /// <summary>
        /// 指定特殊日期状态
        /// </summary>
        public const string SignAppointDateStatus = "signAppointDateStatus";

        /// <summary>
        /// 指定特殊日期
        /// </summary>
        public const string SignAppointDate = "signAppointDate";

        /// <summary>
        /// 指定日期奖励类型
        /// </summary>
        public const string SignAppointDataType = "signAppointDataType";

        /// <summary>
        /// 指定日期倍率
        /// </summary>
        public const string SignAppointDateRate = "signAppointDateRate";

        /// <summary>
        /// 指定日期追加
        /// </summary>
        public const string SignAppointDateAdditional = "signAppointDateAdditional";




        //小程序设置============================================================================
        /// <summary>
        /// 小程序部署URL
        /// </summary>
        public const string WxUrl = "wxUrl";
        /// <summary>
        /// 小程序名称
        /// </summary>
        public const string WxNickName = "wxNickName";

        /// <summary>
        /// 小程序AppId
        /// </summary>
        public const string WxAppid = "wxAppid";

        /// <summary>
        /// 小程序AppSecret
        /// </summary>
        public const string WxAppSecret = "wxAppSecret";

        /// <summary>
        /// 小程序TOKEN
        /// </summary>
        public const string WxToken = "wxToken";

        /// <summary>
        /// 小程序EncodingAESKey
        /// </summary>
        public const string WxEncodeaeskey = "wxEncodeaeskey";


        /// <summary>
        /// 原始Id
        /// </summary>
        public const string WxUserName = "wxUserName";

        /// <summary>
        /// 主体信息
        /// </summary>
        public const string WxPrincipalName = "wxPrincipalName";

        /// <summary>
        /// 简介
        /// </summary>
        public const string WxSignature = "wxSignature";


        //公众号设置============================================================================

        /// <summary>
        /// 公众号部署URL
        /// </summary>
        public const string WxOfficialUrl = "wxOfficialUrl";
        /// <summary>
        /// 公众号名称
        /// </summary>
        public const string WxOfficialName = "wxOfficialName";

        /// <summary>
        /// 微信号
        /// </summary>
        public const string WxOfficialId = "wxOfficialId";

        /// <summary>
        /// AppId
        /// </summary>
        public const string WxOfficialAppid = "wxOfficialAppid";

        /// <summary>
        /// AppSecret
        /// </summary>
        public const string WxOfficialAppSecret = "wxOfficialAppSecret";

        /// <summary>
        /// 公众号原始ID
        /// </summary>
        public const string WxOfficialSourceId = "wxOfficialSourceId";

        /// <summary>
        /// 微信验证TOKEN
        /// </summary>
        public const string WxOfficialToken = "wxOfficialToken";

        /// <summary>
        /// EncodingAESKey
        /// </summary>
        public const string WxOfficialEncodeaeskey = "wxOfficialEncodeaeskey";

        /// <summary>
        /// 公众号类型
        /// </summary>
        public const string WxOfficialType = "wxOfficialType";
        /// <summary>
        /// 公众号二维码
        /// </summary>
        public const string WxOfficialQrCode = "wxOfficialQrCode";


        // 提现设置============================================================================
        /// <summary>
        /// 最低提现金额
        /// </summary>
        public const string TocashMoneyLow = "tocashMoneyLow";

        /// <summary>
        /// 提现服务费率
        /// </summary>
        public const string TocashMoneyRate = "tocashMoneyRate";

        /// <summary>
        /// 每日提现上限
        /// </summary>
        public const string TocashMoneyLimit = "tocashMoneyLimit";


        //其他设置============================================================================

        /// <summary>
        /// 腾讯地图key
        /// </summary>
        public const string QqMapKey = "qqMapKey";

        /// <summary>
        /// 公司编号
        /// </summary>
        public const string Kuaidi100Customer = "kuaidi100Customer";

        /// <summary>
        /// 授权key
        /// </summary>
        public const string Kuaidi100Key = "kuaidi100Key";


        //搜索发现关键字============================================================================
        /// <summary>
        /// 搜索发现关键词
        /// </summary>
        public const string RecommendKeys = "recommendKeys";


        //统计代码============================================================================
        /// <summary>
        /// 百度统计代码
        /// </summary>
        public const string StatisticsCode = "statisticsCode";


        //发票开关============================================================================
        /// <summary>
        /// 发票功能
        /// </summary>
        public const string InvoiceSwitch = "invoiceSwitch";


        //第三方的登陆的时候，是否需要绑定手机号码，强烈建议用户开启，除非只在微信小程序内使用============================================================================
        //1绑定，2不绑定
        /// <summary>
        /// 绑定手机号码
        /// </summary>
        public const string IsBindMobile = "isBindMobile";

        //支付宝小程序appid============================================================================

        /// <summary>
        /// 支付宝小程序appid
        /// </summary>
        public const string MpAlipayAppid = "mpAlipayAppid";

        /// <summary>
        /// 分享图片
        /// </summary>
        public const string ShareImage = "shareImage";

        /// <summary>
        /// 分享标题
        /// </summary>
        public const string ShareTitle = "shareTitle";

        /// <summary>
        /// 分享描述
        /// </summary>
        public const string ShareDesc = "shareDesc";

        /// <summary>
        /// 关于我们文章
        /// </summary>
        public const string AboutArticleId = "aboutArticleId";

        /// <summary>
        /// 关于我们文章
        /// </summary>
        public const string AboutArticle = "aboutArticle";

        /// <summary>
        /// 客服ID
        /// </summary>
        public const string EntId = "entId";

        /// <summary>
        /// 用户协议
        /// </summary>
        public const string UserAgreementId = "userAgreementId";

        /// <summary>
        /// 用户协议
        /// </summary>
        public const string UserAgreement = "userAgreement";

        /// <summary>
        /// 隐私政策
        /// </summary>
        public const string PrivacyPolicyId = "privacyPolicyId";

        /// <summary>
        /// 隐私政策
        /// </summary>
        public const string PrivacyPolicy = "privacyPolicy";

        /// <summary>
        /// 显示门店列表
        /// </summary>
        public const string ShowStoresSwitch = "showStoresSwitch";

        /// <summary>
        /// 显示充值功能
        /// </summary>
        public const string ShowStoreBalanceRechargeSwitch = "showStoreBalanceRechargeSwitch";

        //第三方接口============================================================================
        /// <summary>
        /// 易源接口授权key
        /// </summary>
        public const string ShowApiAppid = "showApiAppid";
        /// <summary>
        /// 易源接口授权密钥
        /// </summary>
        public const string ShowApiSecret = "showApiSecret";


        //短信平台============================================================================
        /// <summary>
        /// 是否开启短信
        /// </summary>
        public const string SmsEnabled = "smsEnabled";
        /// <summary>
        /// 用户ID
        /// </summary>
        public const string SmsUserId = "smsUserId";
        /// <summary>
        /// 用户账号
        /// </summary>
        public const string SmsAccount = "smsAccount";
        /// <summary>
        /// 用户密码
        /// </summary>
        public const string SmsPassword = "smsPassword";
        /// <summary>
        /// 短信api地址
        /// </summary>
        public const string SmsApiUrl = "smsApiUrl";
        /// <summary>
        /// 短信签名
        /// </summary>
        public const string SmsSignature = "smsSignature";


        /// <summary>
        /// 账户注册-短信内容模板
        /// </summary>
        public const string SmsTplForReg = "smsTplForReg";

        /// <summary>
        /// 账户登录-短信内容模板
        /// </summary>
        public const string SmsTplForLogin = "smsTplForLogin";

        /// <summary>
        /// 验证验证码-短信内容模板
        /// </summary>
        public const string SmsTplForVeri = "smsTplForVeri";

        /// <summary>
        /// 下单成功时-短信内容模板
        /// </summary>
        public const string SmsTplForCreateOrder = "smsTplForCreateOrder";

        /// <summary>
        /// 订单支付成功时-短信内容模板
        /// </summary>
        public const string SmsTplForOrderPayed = "smsTplForOrderPayed";

        /// <summary>
        /// 订单催付提醒-短信内容模板
        /// </summary>
        public const string SmsTplForRemindOrderPay = "smsTplForRemindOrderPay";

        /// <summary>
        /// 订单发货通知-短信内容模板
        /// </summary>
        public const string SmsTplForDeliveryNotice = "smsTplForDeliveryNotice";

        /// <summary>
        /// 售后确认通过-短信内容模板
        /// </summary>
        public const string SmsTplForAfterSalesPass = "smsTplForAfterSalesPass";

        /// <summary>
        /// 用户退款成功通知-短信内容模板
        /// </summary>
        public const string SmsTplForRefundSuccess = "smsTplForRefundSuccess";

        /// <summary>
        /// 订单付款成功平台通知-短信内容模板
        /// </summary>
        public const string SmsTplForSellerOrderNotice = "smsTplForSellerOrderNotice";

        /// <summary>
        /// 通用类型-短信内容模板
        /// </summary>
        public const string SmsTplForCommon = "smsTplForCommon";

        //网络打印机============================================================================
        /// <summary>
        /// 是否开启
        /// </summary>
        public static readonly string NetWorkPrinterEnabled = "netWorkPrinterEnabled";
        /// <summary>
        /// 应用ID
        /// </summary>
        public static readonly string NetWorkPrinterClientId = "netWorkPrinterClientId";
        /// <summary>
        /// 应用密钥
        /// </summary>
        public static readonly string NetWorkPrinterClientSecret = "netWorkPrinterClientSecret";
        /// <summary>
        /// 打印机设备号
        /// </summary>
        public static readonly string NetWorkPrinterMachineCode = "netWorkPrinterMachineCode";
        /// <summary>
        /// 打印机终端密钥
        /// </summary>
        public static readonly string NetWorkPrinterMsign = "netWorkPrinterMsign";
        /// <summary>
        /// 打印机名称
        /// </summary>
        public static readonly string NetWorkPrinterPrinterName = "netWorkPrinterPrinterName";
        /// <summary>
        /// 打印机设置联系方式
        /// </summary>
        public static readonly string NetWorkPrinterPhone = "netWorkPrinterPhone";

        //代理模块============================================================================

        /// <summary>
        /// 是否开启代理模块
        /// </summary>
        public static readonly string IsOpenAgent = "isOpenAgent";

        /// <summary>
        /// 是否显示代理模块申请及管理入口
        /// </summary>
        public static readonly string IsShowAgentPortal = "isShowAgentPortal";

        /// <summary>
        /// 用户须知：
        /// </summary>
        public const string AgentNotes = "agentNotes";
        /// <summary>
        /// 分销协议:
        /// </summary>
        public const string AgentAgreement = "agentAgreement";
        /// <summary>
        /// 是否允许代理代购服务
        /// </summary>
        public const string IsAllowProcurementService = "isAllowProcurementService";


        //附件存储============================================================================

        /// <summary>
        /// 存储方式
        /// </summary>
        public static readonly string FilesStorageType = "filesStorageType";
        /// <summary>
        /// 存储路径
        /// </summary>
        public static readonly string FilesStoragePath = "filesStoragePath";
        /// <summary>
        /// 文件后缀类型
        /// </summary>
        public static readonly string FilesStorageFileSuffix = "filesStorageFileSuffix";
        /// <summary>
        /// 文件最大大小M
        /// </summary>
        public static readonly string FilesStorageFileMaxSize = "filesStorageFileMaxSize";

        /// <summary>
        /// 云存储绑定域名
        /// </summary>
        public static readonly string FilesStorageBucketBindUrl = "filesStorageBucketBindUrl";
        /// <summary>
        /// 云存储授权账户
        /// </summary>
        public static readonly string FilesStorageAccessKeyId = "filesStorageAccessKeyId";
        /// <summary>
        /// 云存储授权密钥
        /// </summary>
        public static readonly string FilesStorageAccessKeySecret = "filesStorageAccessKeySecret";

        /// <summary>
        /// 腾讯云账户标识
        /// </summary>
        public static readonly string FilesStorageTencentAccountId = "filesStorageTencentAccountId";
        /// <summary>
        /// 腾讯云存储桶地域
        /// </summary>
        public static readonly string FilesStorageTencentCosRegion = "filesStorageTencentCosRegion";
        /// <summary>
        /// 腾讯云存储桶名称
        /// </summary>
        public static readonly string FilesStorageTencentBucketName = "filesStorageTencentBucketName";

        /// <summary>
        /// 阿里云节点
        /// </summary>
        public static readonly string FilesStorageAliYunEndpoint = "filesStorageAliYunEndpoint";
        /// <summary>
        /// 阿里云桶名称
        /// </summary>
        public static readonly string FilesStorageAliYunBucketName = "filesStorageAliYunBucketName";

        /// <summary>
        /// 七牛云桶名称
        /// </summary>
        public static readonly string FilesStorageQiNiuBucketName = "filesStorageQiNiuBucketName";



    }
}