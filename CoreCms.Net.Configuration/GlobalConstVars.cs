namespace CoreCms.Net.Configuration
{
    public class GlobalConstVars
    {
        /// <summary>
        /// 数据删除成功
        /// </summary>
        public const string DeleteSuccess = "数据删除成功";
        /// <summary>
        /// 数据删除失败
        /// </summary>
        public const string DeleteFailure = "数据删除失败";
        /// <summary>
        /// 系统禁止删除此数据
        /// </summary>
        public const string DeleteProhibitDelete = "系统禁止删除此数据";
        /// <summary>
        /// 此数据含有子类信息，禁止删除
        /// </summary>
        public const string DeleteIsHaveChildren = "此数据含有子类信息，禁止删除";
        /// <summary>
        /// 数据处理异常
        /// </summary>
        public const string DataHandleEx = "数据接口出现异常";
        /// <summary>
        /// 数据添加成功
        /// </summary>
        public const string CreateSuccess = "数据添加成功";
        /// <summary>
        /// 数据添加失败
        /// </summary>
        public const string CreateFailure = "数据添加失败";
        /// <summary>
        /// 数据移动成功
        /// </summary>
        public const string MoveSuccess = "数据移动成功";
        /// <summary>
        /// 数据移动失败
        /// </summary>
        public const string MoveFailure = "数据移动失败";
        /// <summary>
        /// 系统禁止添加数据
        /// </summary>
        public const string CreateProhibitCreate = "系统禁止添加数据";
        /// <summary>
        /// 数据编辑成功
        /// </summary>
        public const string EditSuccess = "数据编辑成功";
        /// <summary>
        /// 数据编辑失败
        /// </summary>
        public const string EditFailure = "数据编辑失败";
        /// <summary>
        /// 系统禁止编辑此数据
        /// </summary>
        public const string EditProhibitEdit = "系统禁止编辑此数据";
        /// <summary>
        /// 数据已存在
        /// </summary>
        public const string DataIsHave = "数据已存在";
        /// <summary>
        /// 数据不存在
        /// </summary>
        public const string DataisNo = "数据不存在";
        /// <summary>
        /// 请提交必要的参数
        /// </summary>
        public const string DataParameterError = "请提交必要的参数";
        /// <summary>
        /// 数据插入成功
        /// </summary>
        public const string InsertSuccess = "数据插入成功！";
        /// <summary>
        /// 数据插入失败
        /// </summary>
        public const string InsertFailure = "数据插入失败！";
        /// <summary>
        /// Excel导出失败
        /// </summary>
        public const string ExcelExportFailure = "Excel导出失败";
        /// <summary>
        /// Excel导出成功
        /// </summary>
        public const string ExcelExportSuccess = "Excel导出成功";
        /// <summary>
        /// 获取数据成功
        /// </summary>
        public const string GetDataSuccess = "获取数据成功！";
        /// <summary>
        /// 获取数据异常
        /// </summary>
        public const string GetDataException = "获取数据异常！";
        /// <summary>
        /// 获取数据失败
        /// </summary>
        public const string GetDataFailure = "获取数据失败！";
        /// <summary>
        /// 设置数据成功
        /// </summary>
        public const string SetDataSuccess = "设置数据成功！";
        /// <summary>
        /// 设置数据异常
        /// </summary>
        public const string SetDataException = "设置数据异常！";
        /// <summary>
        /// 设置数据失败
        /// </summary>
        public const string SetDataFailure = "设置数据失败！";

        //缓存数据
        /// <summary>
        /// 缓存已经排序后台导航
        /// </summary>
        public const string CacheFindNavSortList = "CacheFindNavSortList";
        /// <summary>
        /// 缓存未排序后台导航
        /// </summary>
        public const string CacheFindNavNoSortList = "CacheFindNavNoSortList";



        /// <summary>
        /// 缓存角色列表
        /// </summary>
        public const string CacheManagerRoleList = "CacheManagerRoleList";
        /// <summary>
        /// 缓存单页分类
        /// </summary>
        public const string CachePageCategoryList = "CachePageCategoryList";
        /// <summary>
        /// 缓存角色详细信息
        /// </summary>
        public const string CacheRoleValues = "CacheRoleValues";
        /// <summary>
        /// 缓存用户组
        /// </summary>
        public const string CacheUserCategoryList = "CacheUserCategoryList";
        /// <summary>
        /// 缓存业务
        /// </summary>
        public const string CacheJobDirectoryList = "CacheJobDirectoryList";
        /// <summary>
        /// 缓存无序区域业务
        /// </summary>
        public const string CacheAreaList = "CacheArea";


        /// <summary>
        /// 缓存配置信息
        /// </summary>
        public const string CacheCoreCmsSettingList = "CacheCoreCmsSettingList";

        public const string CacheCoreCmsSettingByComparison = "CacheCoreCmsSettingByComparison";


        /// <summary>
        /// CookieOpenid
        /// </summary>
        public const string CookieOpenId = "CookieOpenId";
        /// <summary>
        /// SessionOpenId
        /// </summary>
        public const string SessionOpenId = "SessionOpenId";
        /// <summary>
        /// 用户AccessToken有效期
        /// </summary>
        public const string CookieOAuthAccessTokenEndTime = "CookieOAuthAccessTokenEndTime";


        /// <summary>
        /// 广告表
        /// </summary>
        public const string CacheCoreCmsAdvertisement = "CacheCoreCmsAdvertisement";
        public const string CacheCoreCmsAdvertPosition = "CacheCoreCmsAdvertPosition"; //广告位置表
        public const string CacheCoreCmsArea = "CacheCoreCmsArea"; // 地区表
        public const string CacheCoreCmsArticle = "CacheCoreCmsArticle"; //文章表
        public const string CacheCoreCmsArticleType = "CacheCoreCmsArticleType"; // 文章分类表
        public const string CacheCoreCmsBillAftersales = "CacheCoreCmsBillAftersales"; // 退货单表
        public const string CacheCoreCmsBillAftersalesImages = "CacheCoreCmsBillAftersalesImages"; // 商品图片关联表
        public const string CacheCoreCmsBillAftersalesItem = "CacheCoreCmsBillAftersalesItem"; // 售后单明细表
        public const string CacheCoreCmsBillDelivery = "CacheCoreCmsBillDelivery"; //发货单表
        public const string CacheCoreCmsBillDeliveryItem = "CacheCoreCmsBillDeliveryItem"; // 发货单详情表
        public const string CacheCoreCmsBillDeliveryOrderRel = "CacheCoreCmsBillDeliveryOrderRel"; // 发货单订单关联表
        public const string CacheCoreCmsBillLading = "CacheCoreCmsBillLading"; // 提货单表
        public const string CacheCoreCmsBillPayments = "CacheCoreCmsBillPayments"; //支付单表
        public const string CacheCoreCmsBillPaymentsRel = "CacheCoreCmsBillPaymentsRel"; //支付单明细表
        public const string CacheCoreCmsBillRefund = "CacheCoreCmsBillRefund"; //退款单表
        public const string CacheCoreCmsBillReship = "CacheCoreCmsBillReship"; //退货单表
        public const string CacheCoreCmsBillReshipItem = "CacheCoreCmsBillReshipItem"; // 退货单明细表
        public const string CacheCoreCmsBrand = "CacheCoreCmsBrand"; //品牌表
        public const string CacheCoreCmsCart = "CacheCoreCmsCart"; // 购物车表
        public const string CacheCoreCmsClerk = "CacheCoreCmsClerk"; //店铺店员关联表
        public const string CacheCoreCmsCoupon = "CacheCoreCmsCoupon"; // 优惠券表
        public const string CacheCoreCmsDistribution = "CacheCoreCmsDistribution"; // 分销商表
        public const string CacheCoreCmsDistributionCondition = "CacheCoreCmsDistributionCondition"; //分销商等级升级条件
        public const string CacheCoreCmsDistributionGrade = "CacheCoreCmsDistributionGrade"; // 分销商等级设置表
        public const string CacheCoreCmsDistributionOrder = "CacheCoreCmsDistributionOrder"; //分销商订单记录表
        public const string CacheCoreCmsDistributionResult = "CacheCoreCmsDistributionResult"; // 等级佣金表
        public const string CacheCoreCmsErrorMessageLog = "CacheCoreCmsErrorMessageLog"; //后台异常错误表
        public const string CacheCoreCmsForm = "CacheCoreCmsForm"; //表单
        public const string CacheCoreCmsFormItem = "CacheCoreCmsFormItem"; // 表单项表
        public const string CacheCoreCmsFormSubmit = "CacheCoreCmsFormSubmit"; // 用户对表的提交记录
        public const string CacheCoreCmsFormSubmitDetail = "CacheCoreCmsFormSubmitDetail"; //提交表单保存大文本值表
        public const string CacheCoreCmsGoods = "CacheCoreCmsGoods"; // 商品表
        public const string CacheCoreCmsGoodsBrowsing = "CacheCoreCmsGoodsBrowsing"; // 商品浏览记录表
        public const string CacheCoreCmsGoodsCategory = "CacheCoreCmsGoodsCategory"; // 商品分类
        public const string CacheCoreCmsGoodsCategoryExtend = "CacheCoreCmsGoodsCategoryExtend"; //商品分类扩展表
        public const string CacheCoreCmsGoodsCollection = "CacheCoreCmsGoodsCollection"; //商品收藏表
        public const string CacheCoreCmsGoodsComment = "CacheCoreCmsGoodsComment"; //商品评价表
        public const string CacheCoreCmsGoodsGrade = "CacheCoreCmsGoodsGrade"; //商品会员价表
        public const string CacheCoreCmsGoodsImages = "CacheCoreCmsGoodsImages"; // 商品图片关联表
        public const string CacheCoreCmsGoodsParams = "CacheCoreCmsGoodsParams"; // 商品参数表
        public const string CacheCoreCmsGoodsType = "CacheCoreCmsGoodsType"; // 商品类型
        public const string CacheCoreCmsGoodsTypeParams = "CacheCoreCmsGoodsTypeParams"; // 商品参数类型关系表
        public const string CacheCoreCmsGoodsTypeSpec = "CacheCoreCmsGoodsTypeSpec"; //商品类型属性表
        public const string CacheCoreCmsGoodsTypeSpecRel = "CacheCoreCmsGoodsTypeSpecRel"; //商品类型和属性关联表
        public const string CacheCoreCmsGoodsTypeSpecValue = "CacheCoreCmsGoodsTypeSpecValue"; // 商品类型属性值表
        public const string CacheCoreCmsImages = "CacheCoreCmsImages"; // 图片表
        public const string CacheCoreCmsInvoice = "CacheCoreCmsInvoice"; // 发票表
        public const string CacheCoreCmsInvoiceRecord = "CacheCoreCmsInvoiceRecord"; //发票信息记录
        public const string CacheCoreCmsJobs = "CacheCoreCmsJobs"; // 队列表
        public const string CacheCoreCmsLabel = "CacheCoreCmsLabel"; //标签表
        public const string CacheCoreCmsLoginLog = "CacheCoreCmsLoginLog"; // 登录日志
        public const string CacheCoreCmsLogistics = "CacheCoreCmsLogistics"; // 物流公司表
        public const string CacheCoreCmsMessage = "CacheCoreCmsMessage"; //消息发送表
        public const string CacheCoreCmsMessageCenter = "CacheCoreCmsMessageCenter"; // 消息配置表
        public const string CacheCoreCmsNotice = "CacheCoreCmsNotice"; //公告表
        public const string CacheCoreCmsOrder = "CacheCoreCmsOrder"; //订单表
        public const string CacheCoreCmsOrderItem = "CacheCoreCmsOrderItem"; //订单明细表
        public const string CacheCoreCmsOrderLog = "CacheCoreCmsOrderLog"; //订单记录表
        public const string CacheCoreCmsPages = "CacheCoreCmsPages"; // 单页
        public const string CacheCoreCmsPagesItems = "CacheCoreCmsPagesItems"; //单页内容
        public const string CacheCoreCmsPayments = "CacheCoreCmsPayments"; // 支付方式表
        public const string CacheCoreCmsPinTuanGoods = "CacheCoreCmsPinTuanGoods"; //拼团商品表
        public const string CacheCoreCmsPinTuanRecord = "CacheCoreCmsPinTuanRecord"; //拼团记录表
        public const string CacheCoreCmsPinTuanRule = "CacheCoreCmsPinTuanRule"; //拼团规则表
        public const string CacheCoreCmsProducts = "CacheCoreCmsProducts"; //货品表
        public const string CacheCoreCmsPromotion = "CacheCoreCmsPromotion"; // 促销表
        public const string CacheCoreCmsPromotionCondition = "CacheCoreCmsPromotionCondition"; // 促销条件表
        public const string CacheCoreCmsPromotionResult = "CacheCoreCmsPromotionResult"; //促销结果表
        public const string CacheCoreCmsSetting = "CacheCoreCmsSetting"; //店铺设置表
        public const string CacheCoreCmsShip = "CacheCoreCmsShip"; //配送方式表
        public const string CacheCoreCmsSms = "CacheCoreCmsSms"; // 短信发送日志
        public const string CacheCoreCmsStore = "CacheCoreCmsStore"; // 门店表
        public const string CacheCoreCmsTemplate = "CacheCoreCmsTemplate"; //模板列表
        public const string CacheCoreCmsTemplateMessage = "CacheCoreCmsTemplateMessage"; //模板消息
        public const string CacheCoreCmsTemplateOrder = "CacheCoreCmsTemplateOrder"; //模板订购记录表
        public const string CacheCoreCmsUser = "CacheCoreCmsUser"; //用户表
        public const string CacheCoreCmsUserBalance = "CacheCoreCmsUserBalance"; //用户余额表
        public const string CacheCoreCmsUserBankCard = "CacheCoreCmsUserBankCard"; //银行卡信息
        public const string CacheCoreCmsUserGrade = "CacheCoreCmsUserGrade"; // 用户等级表
        public const string CacheCoreCmsUserLog = "CacheCoreCmsUserLog"; // 用户日志
        public const string CacheCoreCmsUserPointLog = "CacheCoreCmsUserPointLog"; //用户积分记录表
        public const string CacheCoreCmsUserShip = "CacheCoreCmsUserShip"; //用户地址表
        public const string CacheCoreCmsUserTocash = "CacheCoreCmsUserTocash"; //用户提现记录表
        public const string CacheCoreCmsUserToken = "CacheCoreCmsUserToken"; // 用户token
        public const string CacheCoreCmsUserWeChatInfo = "CacheCoreCmsUserWeChatInfo"; //用户表
        public const string CacheCoreCmsUserWeChatMsgSubscription = "CacheCoreCmsUserWeChatMsgSubscription"; // 微信订阅消息存储表
        public const string CacheCoreCmsUserWeChatMsgSubscriptionSwitch = "CacheCoreCmsUserWeChatMsgSubscriptionSwitch"; // 用户订阅提醒状态
        public const string CacheCoreCmsUserWeChatMsgTemplate = "CacheCoreCmsUserWeChatMsgTemplate"; // 微信小程序消息模板
        public const string CacheCoreCmsWeixinAuthor = "CacheCoreCmsWeixinAuthor"; // 获取授权方的帐号基本信息表
        public const string CacheCoreCmsWeixinMediaMessage = "CacheCoreCmsWeixinMediaMessage"; //微信图文消息表
        public const string CacheCoreCmsWeixinMenu = "CacheCoreCmsWeixinMenu"; //微信公众号菜单表
        public const string CacheCoreCmsWeixinMessage = "CacheCoreCmsWeixinMessage"; //微信消息表
        public const string CacheCoreCmsWeixinPublish = "CacheCoreCmsWeixinPublish"; //小程序发布审核表
        public const string CacheCoreCmsWorkSheet = "CacheCoreCmsWorkSheet"; //工作工单表
        public const string CacheSysDictionary = "CacheSysDictionary"; //数据字典表
        public const string CacheSysDictionaryData = "CacheSysDictionaryData"; //数据字典项表
        public const string CacheSysLoginRecord = "CacheSysLoginRecord"; //登录日志表
        public const string CacheSysMenu = "CacheSysMenu"; // 菜单表
        public const string CacheSysOperRecord = "CacheSysOperRecord"; // 操作日志表
        public const string CacheSysOrganization = "CacheSysOrganization"; // 组织机构表
        public const string CacheSysRole = "CacheSysRole"; //角色表
        public const string CacheSysRoleMenu = "CacheSysRoleMenu"; //角色菜单关联表
        public const string CacheSysUser = "CacheSysUser"; //用户表
        public const string CacheSysUserRole = "CacheSysUserRole"; //用户角色关联表
        public const string CacheViewStoreClerk = "CacheViewStoreClerk"; //店员视图表
        public const string CacheCoreCmsProductsDistribution = "CacheCoreCmsProductsDistribution"; //货品三级佣金表
        public const string CacheCoreCmsServiceDescription = "CacheCoreCmsServiceDescription";
        public const string CacheCoreCmsAgentGrade = "CacheCoreCmsAgentGrade";


    }


    /// <summary>
    /// Tools工具常量
    /// </summary>
    public static class ToolsVars
    {
        /// <summary>
        /// 
        /// </summary>
        public const string IllegalWordsCahceName = "IllegalWordsCahce";

    }


    /// <summary>
    /// 权限变量配置
    /// </summary>
    public static class Permissions
    {
        public const string Name = "Permission";

        /// <summary>
        /// 当前项目是否启用IDS4权限方案
        /// true：表示启动IDS4
        /// false：表示使用JWT
        public static bool IsUseIds4 = false;
    }

    /// <summary>
    /// 路由变量前缀配置
    /// </summary>
    public static class RoutePrefix
    {
        /// <summary>
        /// 前缀名
        /// 如果不需要，尽量留空，不要修改
        /// 除非一定要在所有的 api 前统一加上特定前缀
        /// </summary>
        public const string Name = "";
    }


    /// <summary>
    /// 银行卡相关常量定义
    /// </summary>
    public static class BankConst
    {
        public const string BankLogoUrl = "https://apimg.alipay.com/combo.png?d=cashier&t=";
    }

    /// <summary>
    /// RedisMqKey队列
    /// </summary>
    public static class RedisMessageQueueKey
    {
        /// <summary>
        /// 微信支付成功后推送到接口进行数据处理
        /// </summary>
        public const string WeChatPayNotice = "WeChatPayNoticeQueue";
        /// <summary>
        /// 微信模板消息
        /// </summary>
        public const string SendWxTemplateMessage = "SendWxTemplateMessage";



        /// <summary>
        /// 订单完结后走代理或分销商提成处理
        /// </summary>
        public const string OrderAgentOrDistribution = "OrderAgentOrDistributionQueue";
        /// <summary>
        /// 订单完成时，结算该订单
        /// </summary>
        public const string OrderFinishCommand = "OrderFinishCommandQueue";
        /// <summary>
        /// 订单完成时，门店订单自动发货
        /// </summary>
        public const string OrderAutomaticDelivery = "OrderAutomaticDeliveryQueue";
        /// <summary>
        /// 订单完结后走打印模块
        /// </summary>
        public const string OrderPrint = "OrderPrintQueue";
        /// <summary>
        /// 售后审核通过后处理
        /// </summary>
        public const string AfterSalesReview = "AfterSalesReview";




        /// <summary>
        /// 日志队列
        /// </summary>
        public const string LogingQueue = "LogingQueue";
        /// <summary>
        /// 短信发送队列
        /// </summary>
        public const string SmsQueue = "SmsQueue";





        //用户相关

        //订单支付成功后，用户升级处理
        public const string UserUpGrade = "UserUpGradeQueue";



    }


}
