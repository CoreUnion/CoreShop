/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        Projectname= 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-03-02 23:52:48
 *        Description: 暂无
 ***********************************************************************/


using System.Collections.Generic;
using CoreCms.Net.Model.ViewModels.Basics;

namespace CoreCms.Net.Configuration
{
    /// <summary>
    /// 全局基础配置字典类型
    /// </summary>
    public static class SystemSettingDictionary
    {

        /// <summary>
        /// 获取系统配置字典，不匹配数据库(1是2否)
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, DictionaryKeyValues> GetConfig()
        {
            Dictionary<string, DictionaryKeyValues> di = new Dictionary<string, DictionaryKeyValues>();
            //平台设置
            di.Add(SystemSettingConstVars.ShopName, new DictionaryKeyValues() { sKey = "平台名称", sValue = "核心内容管理系统" });
            di.Add(SystemSettingConstVars.ShopDesc, new DictionaryKeyValues() { sKey = "平台描述", sValue = "平台描述会展示在前台及微信分享描述" });
            di.Add(SystemSettingConstVars.ShopAddress, new DictionaryKeyValues() { sKey = "平台地址", sValue = "我的平台地址" });
            di.Add(SystemSettingConstVars.ShopBeiAn, new DictionaryKeyValues() { sKey = "备案信息", sValue = "网站备案信息" });
            di.Add(SystemSettingConstVars.ShopLogo, new DictionaryKeyValues() { sKey = "平台logo", sValue = "" });
            di.Add(SystemSettingConstVars.ShopFavicon, new DictionaryKeyValues() { sKey = "Favicon图标", sValue = "" });
            di.Add(SystemSettingConstVars.ShopDefaultImage, new DictionaryKeyValues() { sKey = "默认图", sValue = "" });
            di.Add(SystemSettingConstVars.StoreSwitch, new DictionaryKeyValues() { sKey = "开启门店自提", sValue = "2" });
            di.Add(SystemSettingConstVars.CateStyle, new DictionaryKeyValues() { sKey = "分类样式", sValue = "3" });
            di.Add(SystemSettingConstVars.CateType, new DictionaryKeyValues() { sKey = "H5分类样式", sValue = "1" });
            di.Add(SystemSettingConstVars.AboutArticleId, new DictionaryKeyValues() { sKey = "关于我们文章", sValue = "2" });
            di.Add(SystemSettingConstVars.AboutArticle, new DictionaryKeyValues() { sKey = "关于我们文章", sValue = "" });
            di.Add(SystemSettingConstVars.UserAgreementId, new DictionaryKeyValues() { sKey = "用户协议", sValue = "3" });
            di.Add(SystemSettingConstVars.UserAgreement, new DictionaryKeyValues() { sKey = "用户协议", sValue = "" });
            di.Add(SystemSettingConstVars.PrivacyPolicyId, new DictionaryKeyValues() { sKey = "隐私政策", sValue = "4" });
            di.Add(SystemSettingConstVars.PrivacyPolicy, new DictionaryKeyValues() { sKey = "隐私政策", sValue = "" });

            di.Add(SystemSettingConstVars.ShowStoresSwitch, new DictionaryKeyValues() { sKey = "显示门店列表", sValue = "2" });
            di.Add(SystemSettingConstVars.ShowStoreBalanceRechargeSwitch, new DictionaryKeyValues() { sKey = "显示充值功能", sValue = "2" });

            //搜索发现关键字
            di.Add(SystemSettingConstVars.RecommendKeys, new DictionaryKeyValues() { sKey = "搜索发现关键词", sValue = "核心,内容,管理,系统" });
            //分享设置
            di.Add(SystemSettingConstVars.ShareImage, new DictionaryKeyValues() { sKey = "分享图片", sValue = "" });
            di.Add(SystemSettingConstVars.ShareTitle, new DictionaryKeyValues() { sKey = "分享标题", sValue = "优质好店邀您共享" });
            di.Add(SystemSettingConstVars.ShareDesc, new DictionaryKeyValues() { sKey = "分享描述", sValue = "" });
            //会员设置
            di.Add(SystemSettingConstVars.ShopMobile, new DictionaryKeyValues() { sKey = "商家手机号", sValue = "" });
            //1绑定，2不绑定-第三方的登陆的时候，是否需要绑定手机号码，强烈建议用户开启，除非只在微信小程序内使用
            di.Add(SystemSettingConstVars.IsBindMobile, new DictionaryKeyValues() { sKey = "绑定手机号码", sValue = "1" });
            //商品设置
            di.Add(SystemSettingConstVars.GoodsStocksWarn, new DictionaryKeyValues() { sKey = "库存警报数量", sValue = "10" });

            //订单管理
            di.Add(SystemSettingConstVars.OrderCancelTime, new DictionaryKeyValues() { sKey = "订单取消时间", sValue = "1" });
            di.Add(SystemSettingConstVars.OrderCompleteTime, new DictionaryKeyValues() { sKey = "订单完成时间", sValue = "30" });
            di.Add(SystemSettingConstVars.OrderAutoSignTime, new DictionaryKeyValues() { sKey = "订单确认收货时间", sValue = "20" });
            di.Add(SystemSettingConstVars.OrderAutoEvalTime, new DictionaryKeyValues() { sKey = "订单自动评价时间", sValue = "30" });
            di.Add(SystemSettingConstVars.RemindOrderTime, new DictionaryKeyValues() { sKey = "订单提醒付款时间", sValue = "1" });
            di.Add(SystemSettingConstVars.ReshipName, new DictionaryKeyValues() { sKey = "退货联系人", sValue = "" });
            di.Add(SystemSettingConstVars.ReshipMobile, new DictionaryKeyValues() { sKey = "退货联系方式", sValue = "" });
            di.Add(SystemSettingConstVars.ReshipAreaId, new DictionaryKeyValues() { sKey = "退货区域", sValue = "" });
            di.Add(SystemSettingConstVars.ReshipAddress, new DictionaryKeyValues() { sKey = "退货详细地址", sValue = "" });
            di.Add(SystemSettingConstVars.ReshipCoordinate, new DictionaryKeyValues() { sKey = "退货坐标", sValue = "" });


            di.Add(SystemSettingConstVars.StoreOrderAutomaticDelivery, new DictionaryKeyValues() { sKey = "门店自提自动发货", sValue = "2" });



            //分销功能

            di.Add(SystemSettingConstVars.OpenDistribution, new DictionaryKeyValues() { sKey = "是否开启三级分销", sValue = "1" });
            di.Add(SystemSettingConstVars.DistributionNotes, new DictionaryKeyValues() { sKey = "用户须知", sValue = "" });
            di.Add(SystemSettingConstVars.DistributionAgreement, new DictionaryKeyValues() { sKey = "分销协议", sValue = "" });
            di.Add(SystemSettingConstVars.DistributionStore, new DictionaryKeyValues() { sKey = "是否开启店铺", sValue = "2" });
            //di.Add(GlobalSettingConstVars.FirstPushAward, new DictionaryKeyValues() { sKey = "直推奖励", sValue = "0" });
            //di.Add(GlobalSettingConstVars.SecondPushAward, new DictionaryKeyValues() { sKey = "次推奖励", sValue = "0" });
            di.Add(SystemSettingConstVars.ShowInviterInfo, new DictionaryKeyValues() { sKey = "是否显示邀请人信息", sValue = "2" });


            di.Add(SystemSettingConstVars.DistributionLevel, new DictionaryKeyValues() { sKey = "分销层级", sValue = "2" });
            di.Add(SystemSettingConstVars.DistributionType, new DictionaryKeyValues() { sKey = "成为分销商条件", sValue = "1" });
            di.Add(SystemSettingConstVars.DistributionMoney, new DictionaryKeyValues() { sKey = "消费自动成为分销商", sValue = "100" });
            di.Add(SystemSettingConstVars.DistributionGoods, new DictionaryKeyValues() { sKey = "购买商品成为分销商", sValue = "1" });
            di.Add(SystemSettingConstVars.DistributionGoodsId, new DictionaryKeyValues() { sKey = "购买商品成为分销商指定商品序列号", sValue = "0" });

            di.Add(SystemSettingConstVars.CommissionType, new DictionaryKeyValues() { sKey = "佣金类型", sValue = "1" });
            di.Add(SystemSettingConstVars.CommissionFirst, new DictionaryKeyValues() { sKey = "一级佣金", sValue = "0" });
            di.Add(SystemSettingConstVars.CommissionSecond, new DictionaryKeyValues() { sKey = "二级佣金", sValue = "0" });
            di.Add(SystemSettingConstVars.CommissionThird, new DictionaryKeyValues() { sKey = "三级佣金", sValue = "0" });

            //代理功能
            di.Add(SystemSettingConstVars.IsOpenAgent, new DictionaryKeyValues() { sKey = "是否开启代理模块", sValue = "1" });
            di.Add(SystemSettingConstVars.IsShowAgentPortal, new DictionaryKeyValues() { sKey = "前端显示入口", sValue = "1" });
            di.Add(SystemSettingConstVars.AgentNotes, new DictionaryKeyValues() { sKey = "用户须知", sValue = "" });
            di.Add(SystemSettingConstVars.AgentAgreement, new DictionaryKeyValues() { sKey = "代理协议", sValue = "" });
            di.Add(SystemSettingConstVars.IsAllowProcurementService, new DictionaryKeyValues() { sKey = "是否允许代理代购服务", sValue = "1" });

            //积分设置
            di.Add(SystemSettingConstVars.SignPointType, new DictionaryKeyValues() { sKey = "签到奖励类型", sValue = "2" });
            di.Add(SystemSettingConstVars.SignRandomMin, new DictionaryKeyValues() { sKey = "随机奖励积分最小值", sValue = "1", });
            di.Add(SystemSettingConstVars.SignRandomMax, new DictionaryKeyValues() { sKey = "随机奖励积分最大值", sValue = "10" });
            di.Add(SystemSettingConstVars.FirstSignPoint, new DictionaryKeyValues() { sKey = "首次奖励积分", sValue = "1" });
            di.Add(SystemSettingConstVars.ContinuitySignAdditional, new DictionaryKeyValues() { sKey = "连续签到追加", sValue = "1" });
            di.Add(SystemSettingConstVars.SignMostPoint, new DictionaryKeyValues() { sKey = "单日最大奖励", sValue = "10" });
            di.Add(SystemSettingConstVars.PointSwitch, new DictionaryKeyValues() { sKey = "开启积分功能", sValue = "1" });
            di.Add(SystemSettingConstVars.PointDiscountedProportion, new DictionaryKeyValues() { sKey = "订单积分折现比例", sValue = "100" });
            di.Add(SystemSettingConstVars.OrdersPointProportion, new DictionaryKeyValues() { sKey = "订单积分使用比例", sValue = "10" });
            di.Add(SystemSettingConstVars.OrdersRewardProportion, new DictionaryKeyValues() { sKey = "订单积分奖励比例", sValue = "1" });

            di.Add(SystemSettingConstVars.SignAppointDateStatus, new DictionaryKeyValues() { sKey = "指定特殊日期状态", sValue = "false" });
            di.Add(SystemSettingConstVars.SignAppointDate, new DictionaryKeyValues() { sKey = "指定特殊日期", sValue = "" });
            di.Add(SystemSettingConstVars.SignAppointDataType, new DictionaryKeyValues() { sKey = "指定日期奖励类型", sValue = "1" });
            di.Add(SystemSettingConstVars.SignAppointDateRate, new DictionaryKeyValues() { sKey = "指定日期倍率", sValue = "2" });
            di.Add(SystemSettingConstVars.SignAppointDateAdditional, new DictionaryKeyValues() { sKey = "指定日期追加", sValue = "10" });

            // 提现设置
            di.Add(SystemSettingConstVars.TocashMoneyLow, new DictionaryKeyValues() { sKey = "最低提现金额", sValue = "0.01" });
            di.Add(SystemSettingConstVars.TocashMoneyRate, new DictionaryKeyValues() { sKey = "提现服务费率", sValue = "0" });
            di.Add(SystemSettingConstVars.TocashMoneyLimit, new DictionaryKeyValues() { sKey = "每日提现上限", sValue = "0" });

            //小程序设置
            di.Add(SystemSettingConstVars.WxUrl, new DictionaryKeyValues() { sKey = "小程序部署URL", sValue = "https://", });
            di.Add(SystemSettingConstVars.WxNickName, new DictionaryKeyValues() { sKey = "小程序名称", sValue = "CoreShop", });
            di.Add(SystemSettingConstVars.WxAppid, new DictionaryKeyValues() { sKey = "AppId", sValue = "", });
            di.Add(SystemSettingConstVars.WxAppSecret, new DictionaryKeyValues() { sKey = "AppSecret", sValue = "" });
            di.Add(SystemSettingConstVars.WxToken, new DictionaryKeyValues() { sKey = "小程序验证TOKEN", sValue = "", });
            di.Add(SystemSettingConstVars.WxEncodeaeskey, new DictionaryKeyValues() { sKey = "小程序EncodingAESKey", sValue = "" });
            di.Add(SystemSettingConstVars.WxUserName, new DictionaryKeyValues() { sKey = "原始Id", sValue = "", });
            di.Add(SystemSettingConstVars.WxPrincipalName, new DictionaryKeyValues() { sKey = "主体信息", sValue = "核心内容管理系统", });
            di.Add(SystemSettingConstVars.WxSignature, new DictionaryKeyValues() { sKey = "简介", sValue = "核心内容管理系统", });

            //公众号设置
            di.Add(SystemSettingConstVars.WxOfficialUrl, new DictionaryKeyValues() { sKey = "公众号部署URL", sValue = "https://", });
            di.Add(SystemSettingConstVars.WxOfficialName, new DictionaryKeyValues() { sKey = "公众号名称", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialId, new DictionaryKeyValues() { sKey = "微信号", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialAppid, new DictionaryKeyValues() { sKey = "AppId", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialAppSecret, new DictionaryKeyValues() { sKey = "AppSecret", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialSourceId, new DictionaryKeyValues() { sKey = "公众号原始ID", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialToken, new DictionaryKeyValues() { sKey = "微信验证TOKEN", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialEncodeaeskey, new DictionaryKeyValues() { sKey = "EncodingAESKey", sValue = "" });
            di.Add(SystemSettingConstVars.WxOfficialType, new DictionaryKeyValues() { sKey = "公众号类型", sValue = "service" });
            di.Add(SystemSettingConstVars.WxOfficialQrCode, new DictionaryKeyValues() { sKey = "公众号二维码", sValue = "" });

            //其他设置
            di.Add(SystemSettingConstVars.QqMapKey, new DictionaryKeyValues() { sKey = "腾讯地图key", sValue = "" });
            di.Add(SystemSettingConstVars.Kuaidi100Customer, new DictionaryKeyValues() { sKey = "公司编号", sValue = "" });
            di.Add(SystemSettingConstVars.Kuaidi100Key, new DictionaryKeyValues() { sKey = "授权key", sValue = "" });

            //统计代码
            di.Add(SystemSettingConstVars.StatisticsCode, new DictionaryKeyValues() { sKey = "百度统计代码", sValue = "" });
            //发票开关
            di.Add(SystemSettingConstVars.InvoiceSwitch, new DictionaryKeyValues() { sKey = "发票功能", sValue = "1" });
            //支付宝小程序appid
            di.Add(SystemSettingConstVars.MpAlipayAppid, new DictionaryKeyValues() { sKey = "支付宝小程序appid", sValue = "" });
            //客服ID
            di.Add(SystemSettingConstVars.EntId, new DictionaryKeyValues() { sKey = "客服ID", sValue = "" });
            //易源接口授权
            di.Add(SystemSettingConstVars.ShowApiAppid, new DictionaryKeyValues() { sKey = "AppId", sValue = "" });
            di.Add(SystemSettingConstVars.ShowApiSecret, new DictionaryKeyValues() { sKey = "授权Secret", sValue = "" });

            //凯信通短信设置
            di.Add(SystemSettingConstVars.SmsEnabled, new DictionaryKeyValues() { sKey = "是否开启短信", sValue = "1" });
            di.Add(SystemSettingConstVars.SmsUserId, new DictionaryKeyValues() { sKey = "用户ID", sValue = "" });
            di.Add(SystemSettingConstVars.SmsAccount, new DictionaryKeyValues() { sKey = "账号", sValue = "" });
            di.Add(SystemSettingConstVars.SmsPassword, new DictionaryKeyValues() { sKey = "密码", sValue = "" });
            di.Add(SystemSettingConstVars.SmsApiUrl, new DictionaryKeyValues() { sKey = "Api地址", sValue = "http://sms.corecms.net:9999/sms.aspx" });
            di.Add(SystemSettingConstVars.SmsSignature, new DictionaryKeyValues() { sKey = "短信签名", sValue = "" });

            //附件存储
            di.Add(SystemSettingConstVars.FilesStorageType, new DictionaryKeyValues() { sKey = "存储方式", sValue = "LocalStorage" });
            di.Add(SystemSettingConstVars.FilesStoragePath, new DictionaryKeyValues() { sKey = "存储路径", sValue = "/upload/" });
            di.Add(SystemSettingConstVars.FilesStorageFileSuffix, new DictionaryKeyValues() { sKey = "文件后缀类型", sValue = "gif,jpg,jpeg,png,bmp,xls,xlsx,doc,pdf,mp4,WebM,Ogv" });
            di.Add(SystemSettingConstVars.FilesStorageFileMaxSize, new DictionaryKeyValues() { sKey = "文件最大大小", sValue = "10" });
            di.Add(SystemSettingConstVars.FilesStorageBucketBindUrl, new DictionaryKeyValues() { sKey = "云存储绑定域名", sValue = "http://www.coreshop.cn/" });
            di.Add(SystemSettingConstVars.FilesStorageAccessKeyId, new DictionaryKeyValues() { sKey = "云存储授权账户", sValue = "" });
            di.Add(SystemSettingConstVars.FilesStorageAccessKeySecret, new DictionaryKeyValues() { sKey = "云存储授权密钥", sValue = "" });
            di.Add(SystemSettingConstVars.FilesStorageTencentAccountId, new DictionaryKeyValues() { sKey = "腾讯云账户标识", sValue = "" });
            di.Add(SystemSettingConstVars.FilesStorageTencentCosRegion, new DictionaryKeyValues() { sKey = "腾讯云桶地域", sValue = "" });
            di.Add(SystemSettingConstVars.FilesStorageTencentBucketName, new DictionaryKeyValues() { sKey = "腾讯云桶名称", sValue = "" });
            di.Add(SystemSettingConstVars.FilesStorageAliYunEndpoint, new DictionaryKeyValues() { sKey = "阿里云节点", sValue = "https://oss-cn-shenzhen.aliyuncs.com" });
            di.Add(SystemSettingConstVars.FilesStorageAliYunBucketName, new DictionaryKeyValues() { sKey = "阿里云桶名称", sValue = "CoreShop" });

            di.Add(SystemSettingConstVars.FilesStorageQiNiuBucketName, new DictionaryKeyValues() { sKey = "七牛云桶名称", sValue = "CoreShop" });

            //短信发送内容模板
            di.Add(SystemSettingConstVars.SmsTplForReg, new DictionaryKeyValues() { sKey = "账户注册", sValue = "您正在注册账号，验证码是{code}，请勿告诉他人。" });
            di.Add(SystemSettingConstVars.SmsTplForLogin, new DictionaryKeyValues() { sKey = "账户登录", sValue = "您正在登陆账号，验证码是{code}，请勿告诉他人。" });
            di.Add(SystemSettingConstVars.SmsTplForVeri, new DictionaryKeyValues() { sKey = "验证验证码", sValue = "您的验证码是{code}，请勿告诉他人。" });
            di.Add(SystemSettingConstVars.SmsTplForCreateOrder, new DictionaryKeyValues() { sKey = "下单成功时", sValue = "恭喜您，订单创建成功，祝您购物愉快。" });
            di.Add(SystemSettingConstVars.SmsTplForOrderPayed, new DictionaryKeyValues() { sKey = "订单支付成功时", sValue = "恭喜您，订单支付成功，祝您购物愉快。" });
            di.Add(SystemSettingConstVars.SmsTplForRemindOrderPay, new DictionaryKeyValues() { sKey = "订单催付提醒", sValue = "您的订单还有1个小时就要取消了，请及时进行支付。" });
            di.Add(SystemSettingConstVars.SmsTplForDeliveryNotice, new DictionaryKeyValues() { sKey = "订单发货通知", sValue = "您好，您的订单已经发货。" });
            di.Add(SystemSettingConstVars.SmsTplForAfterSalesPass, new DictionaryKeyValues() { sKey = "售后确认通过", sValue = "您好，您的售后已经通过。" });
            di.Add(SystemSettingConstVars.SmsTplForRefundSuccess, new DictionaryKeyValues() { sKey = "用户退款成功通知", sValue = "用户您好，您的退款已经处理，请确认。" });
            di.Add(SystemSettingConstVars.SmsTplForSellerOrderNotice, new DictionaryKeyValues() { sKey = "订单付款成功平台通知", sValue = "您有新的订单了，请及时处理。" });
            di.Add(SystemSettingConstVars.SmsTplForCommon, new DictionaryKeyValues() { sKey = "通用类型", sValue = "欢迎您访问我们的微信小程序，有问题请联系客服。" });


            return di;
        }

        /// <summary>
        /// 获取促销添加参数类型字典
        /// </summary>
        /// <returns></returns>
        public static List<CommonKeyValues> GetPromotionConditionType()
        {
            var list = new List<CommonKeyValues>
            {
                new CommonKeyValues() {sDescription = "所有商品满足条件", sValue = "goods", sKey = "GOODS_ALL"},
                new CommonKeyValues() {sDescription = "指定某些商品满足条件", sValue = "goods", sKey = "GOODS_IDS"},
                new CommonKeyValues() {sDescription = "指定商品分类满足条件", sValue = "goods", sKey = "GOODS_CATS"},
                new CommonKeyValues() {sDescription = "指定商品品牌满足条件", sValue = "goods", sKey = "GOODS_BRANDS"},
                new CommonKeyValues() {sDescription = "订单满XX金额满足条件", sValue = "order", sKey = "ORDER_FULL"},
                new CommonKeyValues() {sDescription = "用户符合指定等级", sValue = "user", sKey = "USER_GRADE"}
            };
            return list;
        }


        /// <summary>
        /// 获取促销添加结果类型字典
        /// </summary>
        /// <returns></returns>
        public static List<CommonKeyValues> GetPromotionResultType()
        {
            var list = new List<CommonKeyValues>
            {
                new CommonKeyValues() {sDescription = "指定商品减固定金额", sValue = "goods", sKey = "GOODS_REDUCE"},
                new CommonKeyValues() {sDescription = "指定商品打X折", sValue = "goods", sKey = "GOODS_DISCOUNT"},
                new CommonKeyValues() {sDescription = "指定商品一口价", sValue = "goods", sKey = "GOODS_ONE_PRICE"},
                new CommonKeyValues() {sDescription = "订单减指定金额", sValue = "order", sKey = "ORDER_REDUCE"},
                new CommonKeyValues() {sDescription = "订单打X折", sValue = "order", sKey = "ORDER_DISCOUNT"},
                new CommonKeyValues() {sDescription = "指定商品每第几件减指定金额", sValue = "goods", sKey = "GOODS_HALF_PRICE"}
            };
            return list;
        }



        /// <summary>
        /// 获取系统默认发货物流方式
        /// </summary>
        /// <returns></returns>
        public static List<CommonKeyValues> GetSystemLogistics()
        {
            var list = new List<CommonKeyValues>
            {
                new CommonKeyValues() {sDescription = "本地同城配送", sValue = "无", sKey = "benditongcheng"},
                new CommonKeyValues() {sDescription = "本地上门自提", sValue = "无", sKey = "shangmenziti"},
            };
            return list;
        }



    }
}
