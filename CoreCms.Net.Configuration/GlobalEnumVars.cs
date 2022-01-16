using System.ComponentModel;

namespace CoreCms.Net.Configuration
{
    /// <summary>
    /// 系统常用枚举类
    /// </summary>
    public class GlobalEnumVars
    {
        #region 系统相关===============================================================

        /// <summary>
        /// 缓存优先级:低、普通、高、永不移除
        /// </summary>
        public enum CacheItemPriority
        {
            Low = 0,
            Normal = 1,
            High = 2,
            NeverRemove = 3
        }

        /// <summary>
        /// 用户登录方式
        /// </summary>
        public enum LoginType
        {
            [Description("普通")]
            Common = 1,
            [Description("短信")]
            Sms = 2,
            [Description("微信小程序拉取手机号")]
            WeChatPhoneNumber = 3,
        }

        /// <summary>
        /// 订单编号类型大全
        /// </summary>
        public enum SerialNumberType
        {
            [Description("订单编号")]
            订单编号 = 1,
            [Description("支付单编号")]
            支付单编号 = 2,
            [Description("商品编号")]
            商品编号 = 3,
            [Description("货品编号")]
            货品编号 = 4,
            [Description("售后单编号")]
            售后单编号 = 5,
            [Description("退款单编号")]
            退款单编号 = 6,
            [Description("退货单编号")]
            退货单编号 = 7,
            [Description("发货单编号")]
            发货单编号 = 8,
            [Description("提货单号")]
            提货单号 = 9,
            [Description("服务订单编号")]
            服务订单编号 = 10,
            [Description("服务券兑换码")]
            服务券兑换码 = 11,
        }
        /// <summary>
        /// 来源
        /// 订单来源[对应CoreCmsOrder表source字段]
        /// </summary>
        public enum Source
        {
            [Description("PC页面")]
            PC页面 = 1,
            [Description("H5页面")]
            H5页面 = 2,
            [Description("微信小程序")]
            微信小程序 = 3,
            [Description("支付宝小程序")]
            支付宝小程序 = 4,
            [Description("APP")]
            APP = 5,
            [Description("头条系小程序")]
            头条 = 6
        }

        /// <summary>
        /// 用户登录日志类型
        /// </summary>

        public enum LoginRecordType
        {
            登录成功 = 0,
            登录失败 = 1,
            退出登录 = 2,
            刷新Token = 0
        }


        /// <summary>
        /// 附件存储支持类型
        /// </summary>
        public enum FilesStorageOptionsType
        {
            [Description("本地存储")]
            LocalStorage = 0,
            [Description("阿里云OSS")]
            AliYunOSS = 1,
            [Description("腾讯云COS")]
            QCloudOSS = 2,
            [Description("七牛云KoDo")]
            QiNiuKoDo = 3,
        }


        /// <summary>
        /// 本地存储位置
        /// </summary>
        public enum FilesStorageLocation
        {
            [Description("后端")]
            Admin = 0,
            [Description("API接口端")]
            API = 1
        }

        #endregion

        #region User用户相关===========================================================================
        /// <summary>
        /// 性别[1男2女3未知]
        /// 对应CoreCmsUserWX表的gender类型
        /// </summary>
        public enum UserSexTypes
        {
            [Description("男")]
            男 = 1,
            [Description("女")]
            女 = 2,
            [Description("未知")]
            未知 = 3
        }
        /// <summary>
        /// 用户状态
        /// </summary>
        public enum UserStatus
        {
            [Description("正常")]
            正常 = 1,
            [Description("停用")]
            停用 = 2
        }
        /// <summary>
        /// 第三方账号来源
        /// [对应CoreCmsUserWX表的type类型]
        /// </summary>
        public enum UserAccountTypes
        {
            [Description("微信公众号")]
            微信公众号 = 1,
            [Description("微信小程序")]
            微信小程序 = 2,
            [Description("支付宝小程序")]
            支付宝小程序 = 3,
            [Description("微信APP快捷登陆")]
            微信APP快捷登陆 = 4,
            [Description("QQ在APP中快捷登陆")]
            QQ在APP中快捷登陆 = 5,
            [Description("头条系小程序")]
            头条系小程序 = 6,
        }
        /// <summary>
        /// 用户余额变动来源类型【对应CoreCmsUserBalance.type字段】
        /// </summary>
        public enum UserBalanceSourceTypes
        {
            /// <summary>
            /// 用户消费
            /// </summary>
            [Description("用户消费")]
            Pay = 1,
            /// <summary>
            /// 用户退款
            /// </summary>
            [Description("用户退款")]
            Refund = 2,
            /// <summary>
            /// 充值
            /// </summary>
            [Description("充值")]
            Recharge = 3,
            /// <summary>
            /// 提现
            /// </summary>
            [Description("提现")]
            Tocash = 4,
            /// <summary>
            /// 三级分销佣金
            /// </summary>
            [Description("三级分销佣金")]
            Distribution = 5,
            /// <summary>
            /// 平台调整
            /// </summary>
            [Description("平台调整")]
            Admin = 6,
            /// <summary>
            /// 奖励
            /// </summary>
            [Description("奖励")]
            Prize = 7,
            /// <summary>
            /// 服务项目
            /// </summary>
            [Description("服务订单")]
            Service = 8,
            /// <summary>
            /// 代理商提成
            /// </summary>
            [Description("代理商提成")]
            Agent = 9,
        }
        /// <summary>
        /// 用户积分变动来源类型
        /// 对应CoreCmsUserPointLog表type字段
        /// </summary>
        public enum UserPointSourceTypes
        {
            /// <summary>
            /// 签到
            /// </summary>
            [Description("签到")]
            PointTypeSign = 1,
            /// <summary>
            /// 购物返积分
            /// </summary>
            [Description("购物返积分")]
            PointTypeRebate = 2,
            /// <summary>
            /// 购物使用积分
            /// </summary>
            [Description("购物使用积分")]
            PointTypeDiscount = 3,
            /// <summary>
            /// 后台编辑
            /// </summary>
            [Description("后台编辑")]
            PointTypeAdminEdit = 4,
            /// <summary>
            /// 奖励积分
            /// </summary>
            [Description("奖励积分")]
            PointTypePrize = 5,
            /// <summary>
            /// 积分兑换
            /// </summary>
            [Description("积分兑换")]
            PointTypeExchange = 6,
            /// <summary>
            /// 售后退款返还
            /// </summary>
            [Description("售后退款返还")]
            PointRefundReturn = 7,
            /// <summary>
            /// 取消订单返还
            /// </summary>
            [Description("取消订单返还")]
            PointCanCelOrder = 8,
        }

        /// <summary>
        /// 用户签到积分类型
        /// </summary>
        public enum UserPointSignTypes
        {
            /// <summary>
            /// 签到固定积分
            /// </summary>
            [Description("签到固定积分")]
            FixedPoint = 1,
            /// <summary>
            /// 签到随机积分
            /// </summary>
            [Description("签到随机积分")]
            RandomPoint = 2
        }


        /// <summary>
        /// 用户日志状态[对应CoreCmsUserLog表的state字段]
        /// </summary>
        public enum UserLogTypes
        {
            [Description("登录")]
            登录 = 1,
            [Description("退出")]
            退出 = 2,
            [Description("注册")]
            注册 = 3
        }
        /// <summary>
        /// 用户提现状态[对应CoreCmsUserTocash表的status字段]
        /// </summary>
        public enum UserTocashTypes
        {
            [Description("待审核")]
            待审核 = 1,
            [Description("提现成功")]
            提现成功 = 2,
            [Description("提现失败")]
            提现失败 = 3
        }
        #endregion

        #region Order订单相关=========================================================

        /// <summary>
        /// 订单支付状态[对应CoreCmsOrder表payStatus字段]
        /// </summary>
        public enum OrderPayStatus
        {
            /// <summary>
            /// 未付款
            /// </summary>
            [Description("未付款")]
            No = 1,
            /// <summary>
            /// 已付款
            /// </summary>
            [Description("已付款")]
            Yes = 2,
            /// <summary>
            /// 部分付款
            /// </summary>
            [Description("部分付款")]
            PartialYes = 3,
            /// <summary>
            /// 部分退款
            /// </summary>
            [Description("部分退款")]
            PartialNo = 4,
            /// <summary>
            /// 已退款
            /// </summary>
            [Description("已退款")]
            Refunded = 5
        }
        /// <summary>
        /// 订单发货状态[对应CoreCmsOrder表shipStatus字段]
        /// </summary>
        public enum OrderShipStatus
        {
            /// <summary>
            /// 未发货
            /// </summary>
            [Description("未发货")]
            No = 1,
            /// <summary>
            /// 部分发货
            /// </summary>
            [Description("部分发货")]
            PartialYes = 2,
            /// <summary>
            /// 已发货
            /// </summary>
            [Description("已发货")]
            Yes = 3,
            /// <summary>
            /// 部分退货
            /// </summary>
            [Description("部分退货")]
            PartialNo = 4,
            /// <summary>
            /// 已退货
            /// </summary>
            [Description("已退货")]
            Returned = 5
        }
        /// <summary>
        /// 订单状态[对应CoreCmsOrder表status字段]
        /// </summary>
        public enum OrderStatus
        {
            /// <summary>
            /// 订单正常
            /// </summary>
            [Description("<button type='button' class='layui-btn  layui-btn-normal  layui-btn-xs'>订单正常</button>")]
            Normal = 1,
            /// <summary>
            /// 订单完成
            /// </summary>
            [Description("<button type='button' class='layui-btn  layui-btn-primary layui-btn-xs'>订单完成</button>")]
            Complete = 2,
            /// <summary>
            /// 订单取消
            /// </summary>
            [Description("<button type='button' class='layui-btn  layui-btn-primary layui-btn-disabled layui-btn-xs'>订单取消</button>")]
            Cancel = 3
        }

        /// <summary>
        /// 订单状态[对应CoreCmsOrder表status字段]
        /// </summary>
        public enum OrderStatusDescription
        {
            /// <summary>
            /// 订单正常
            /// </summary>
            [Description("正常（-）")]
            Normal = 1,
            /// <summary>
            /// 订单完成
            /// </summary>
            [Description("完成（√）")]
            Complete = 2,
            /// <summary>
            /// 订单取消
            /// </summary>
            [Description("取消（×）")]
            Cancel = 3
        }

        /// <summary>
        /// 订单收货状态[对应CoreCmsOrder表confirmStatus字段]
        /// </summary>
        public enum OrderConfirmStatus
        {
            /// <summary>
            /// 未确认收货
            /// </summary>
            [Description("未确认收货")]
            ReceiptNotConfirmed = 1,
            /// <summary>
            /// 已确认收货
            /// </summary>
            [Description("已确认收货")]
            ConfirmReceipt = 2
        }
        /// <summary>
        /// 全局总订单类型
        /// </summary>
        public enum OrderAllStatusType
        {
            /// <summary>
            /// 待付款
            /// </summary>
            [Description("待付款")]
            ALL_PENDING_PAYMENT = 1,
            /// <summary>
            /// 待发货
            /// </summary>
            [Description("待发货")]
            ALL_PENDING_DELIVERY = 2,
            /// <summary>
            /// 待收货
            /// </summary>
            [Description("待收货")]
            ALL_PENDING_RECEIPT = 3,
            /// <summary>
            /// 待评价
            /// </summary>
            [Description("待评价")]
            ALL_PENDING_EVALUATE = 4,
            /// <summary>
            /// 已评价
            /// </summary>
            [Description("已评价")]
            ALL_COMPLETED_EVALUATE = 5,
            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")]
            ALL_COMPLETED = 6,
            /// <summary>
            /// 已取消
            /// </summary>
            [Description("已取消")]
            ALL_CANCEL = 7,
            /// <summary>
            /// 部分发货
            /// </summary>
            [Description("部分发货")]
            ALL_PARTIAL_DELIVERY = 8,
        }
        /// <summary>
        /// 开票类型
        /// </summary>
        public enum OrderTaxCategory
        {
            /// <summary>
            /// 订单
            /// </summary>
            [Description("订单")]
            Order = 1
        }
        /// <summary>
        /// 订单开票类型[对应CoreCmsOrder表taxType字段]
        /// </summary>
        public enum OrderTaxType
        {
            /// <summary>
            /// 不开发票
            /// </summary>
            [Description("不开发票")]
            No = 1,
            /// <summary>
            /// 个人发票
            /// </summary>
            [Description("个人发票")]
            Personal = 2,
            /// <summary>
            /// 公司发票
            /// </summary>
            [Description("公司发票")]
            Company = 3
        }
        /// <summary>
        /// 订单开票状态
        /// </summary>
        public enum OrderTaxStatus
        {
            /// <summary>
            /// 未开票
            /// </summary>
            [Description("未开票")]
            No = 1,
            /// <summary>
            /// 已开票
            /// </summary>
            [Description("已开票")]
            Yes = 2
        }
        /// <summary>
        /// 订单用户性别[1男2女3未知]
        /// </summary>
        public enum OrderUserSex
        {
            [Description("男")]
            男 = 1,
            [Description("女")]
            女 = 2,
            [Description("未知")]
            未知 = 3
        }
        /// <summary>
        /// 订单评价状态
        /// </summary>
        public enum OrderIsComment
        {
            /// <summary>
            /// 没有评价
            /// </summary>
            [Description("没有评价")]
            NoComment = 1,
            /// <summary>
            /// 已经评价
            /// </summary>
            [Description("已经评价")]
            AlreadyComment = 2
        }
        /// <summary>
        /// 订单类型[对应CoreCmsOrder表orderType字段]/也对应购物车cart订单类型
        /// </summary>
        public enum OrderType
        {
            /// <summary>
            /// 普通
            /// </summary>
            [Description("普通")]
            Common = 1,
            /// <summary>
            /// 拼团
            /// </summary>
            [Description("拼团")]
            PinTuan = 2,
            /// <summary>
            /// 团购
            /// </summary>
            [Description("团购")]
            Group = 3,
            /// <summary>
            /// 秒杀
            /// </summary>
            [Description("秒杀")]
            Skill = 4,
            /// <summary>
            /// 砍价
            /// </summary>
            [Description("砍价")]
            Bargain = 6,
            /// <summary>
            /// 赠品
            /// </summary>
            [Description("赠品")]
            Giveaway = 7,
            /// <summary>
            /// 接龙
            /// </summary>
            [Description("接龙")]
            Solitaire = 8,
            /// <summary>
            /// 微信交易组件
            /// </summary>
            [Description("微信交易组件")]
            TransactionComponent = 10,

        }
        /// <summary>
        /// 发货单状态
        /// </summary>
        public enum OrderLogisticsState
        {
            [Description("在途中")]
            在途中 = 0,
            [Description("已揽收")]
            已揽收 = 1,
            [Description("疑难")]
            疑难 = 2,
            [Description("已签收")]
            已签收 = 3
        }



        /// <summary>
        /// 售后类型
        /// </summary>
        public enum AftersaleTypes
        {
            [Description("退款")]
            退款 = 1,
            [Description("退款退货")]
            退款退货 = 2
        }

        /// <summary>
        /// 库存改变机制类型
        /// </summary>
        public enum OrderChangeStockType
        {
            /// <summary>
            /// 下单
            /// </summary>
            [Description("下单")]
            order = 1,
            /// <summary>
            /// 发货
            /// </summary>
            [Description("发货")]
            send = 2,
            /// <summary>
            /// 退款
            /// </summary>
            [Description("退款")]
            refund = 3,
            /// <summary>
            /// 退货
            /// </summary>
            [Description("退货")]
            @return = 4,
            /// <summary>
            /// 取消订单
            /// </summary>
            [Description("取消订单")]
            cancel = 5,
            /// <summary>
            /// 完成订单
            /// </summary>
            [Description("完成订单")]
            complete = 6,
        }


        /// <summary>
        /// 后台订单列表类型(用于html切换)
        /// </summary>
        public enum OrderCountType
        {
            /// <summary>
            /// 全部
            /// </summary>
            [Description("全部")]
            all = 0,
            /// <summary>
            /// 待支付
            /// </summary>
            [Description("待支付")]
            payment = 1,
            /// <summary>
            /// 待发货
            /// </summary>
            [Description("待发货")]
            delivered = 2,
            /// <summary>
            /// 待收货
            /// </summary>
            [Description("待收货")]
            receive = 3,
            /// <summary>
            /// 已评价
            /// </summary>
            [Description("已评价")]
            evaluated = 4,
            /// <summary>
            /// 待评价
            /// </summary>
            [Description("待评价")]
            noevaluat = 5,
            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")]
            complete = 6,
            /// <summary>
            /// 已取消
            /// </summary>
            [Description("已取消")]
            cancel = 7,
            /// <summary>
            /// 已删除
            /// </summary>
            [Description("已删除")]
            delete = 999,
        }

        /// <summary>
        /// 订单打印类别
        /// </summary>
        public enum OrderPrintType
        {
            /// <summary>
            /// 购物清单
            /// </summary>
            [Description("购物清单")]
            Shopping = 1,
            /// <summary>
            /// 配货单
            /// </summary>
            [Description("配货单")]
            Distribution = 2,
            /// <summary>
            /// 联合打印
            /// </summary>
            [Description("联合打印")]
            Union = 3,
            /// <summary>
            /// 联合打印快递单
            /// </summary>
            [Description("联合打印快递单")]
            Express = 4,
        }


        /// <summary>
        /// 订单收货方式
        /// </summary>
        public enum OrderReceiptType
        {
            /// <summary>
            /// 物流快递
            /// </summary>
            [Description("<button type='button' class='layui-btn  layui-btn-xs'>物流快递</button>")]
            Logistics = 1,
            /// <summary>
            /// 同城配送
            /// </summary>
            [Description("<button type='button' class='layui-btn  layui-btn-danger layui-btn-xs'>同城配送</button>")]
            IntraCityService = 2,
            /// <summary>
            /// 门店自提
            /// </summary>
            [Description("<button type='button' class='layui-btn  layui-btn-warm layui-btn-xs'>门店自提</button>")]
            SelfDelivery = 3
        }

        #endregion

        #region OrderLog订单日志=====================================================

        /// <summary>
        /// 订单日志状态[对应CoreCmsOrderLog表Type字段]
        /// </summary>
        public enum OrderLogTypes
        {
            /// <summary>
            /// 订单创建
            /// </summary>
            [Description("订单创建")]
            LOG_TYPE_CREATE = 1,
            /// <summary>
            /// 订单支付
            /// </summary>
            [Description("订单支付")]
            LOG_TYPE_PAY = 2,
            /// <summary>
            /// 订单发货
            /// </summary>
            [Description("订单发货")]
            LOG_TYPE_SHIP = 3,
            /// <summary>
            /// 订单签收
            /// </summary>
            [Description("订单签收")]
            LOG_TYPE_SIGN = 4,
            /// <summary>
            /// 订单评价
            /// </summary>
            [Description("订单评价")]
            LOG_TYPE_EVALUATION = 5,
            /// <summary>
            /// 订单完成
            /// </summary>
            [Description("订单完成")]
            LOG_TYPE_COMPLETE = 6,
            /// <summary>
            /// 订单取消
            /// </summary>
            [Description("订单取消")]
            LOG_TYPE_CANCEL = 7,
            /// <summary>
            /// 订单编辑
            /// </summary>
            [Description("订单编辑")]
            LOG_TYPE_EDIT = 8,
            /// <summary>
            /// 订单自动签收
            /// </summary>
            [Description("订单自动签收")]
            LOG_TYPE_AUTO_SIGN = 9,
            /// <summary>
            /// 订单自动评价
            /// </summary>
            [Description("订单自动评价")]
            LOG_TYPE_AUTO_EVALUATION = 10,
            /// <summary>
            /// 订单自动完成
            /// </summary>
            [Description("订单自动完成")]
            LOG_TYPE_AUTO_COMPLETE = 11,
            /// <summary>
            /// 订单自动取消
            /// </summary>
            [Description("订单自动取消")]
            LOG_TYPE_AUTO_CANCEL = 12,
        }




        #endregion

        #region 优惠券===================================================
        /// <summary>
        /// 优惠券状态
        /// </summary>
        public enum CouponStatus
        {
            [Description("启用")]
            启用 = 1,
            [Description("禁用")]
            禁用 = 2
        }

        /// <summary>
        /// 领取状态
        /// </summary>
        public enum CouponIsUsedStatus
        {
            [Description("未使用")]
            未使用 = 1,
            [Description("已使用")]
            已使用 = 2
        }


        /// <summary>
        /// 前端领取优惠券状态
        /// </summary>
        public enum CouponIsUsedStatusText
        {
            [Description("未使用")]
            noUsed = 1,
            [Description("已使用")]
            yesUsed = 2,
            [Description("已失效")]
            invalid = 3
        }

        #endregion

        #region payments支付=================================
        /// <summary>
        /// 支付方式
        /// </summary>
        public enum PaymentsIsOnline
        {
            /// <summary>
            /// 线上支付
            /// </summary>
            [Description("线上支付")]
            PaymentOnline = 1,
            /// <summary>
            /// 线下支付
            /// </summary>
            [Description("线下支付")]
            PaymentOffline = 2
        }
        /// <summary>
        /// 支付启用状态
        /// </summary>
        public enum PaymentsStatus
        {
            [Description("启用")]
            启用 = 1,
            [Description("禁用")]
            禁用 = 2
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public enum PaymentsTypes
        {
            /// <summary>
            /// 微信支付
            /// </summary>
            [Description("微信支付")]
            wechatpay = 1,
            /// <summary>
            /// 支付宝支付
            /// </summary>
            [Description("支付宝支付")]
            alipay = 2,
            /// <summary>
            /// 线下支付
            /// </summary>
            [Description("线下支付")]
            offline = 3,
            /// <summary>
            /// 余额支付
            /// </summary>
            [Description("余额支付")]
            balancepay = 4
        }
        #endregion

        #region BillPayments付款单========================================================
        /// <summary>
        /// 付款单类型 【对应CoreCmsBillPayments.type】
        /// </summary>
        public enum BillPaymentsType
        {
            /// <summary>
            /// 订单
            /// </summary>
            [Description("订单")]
            Order = 1,
            /// <summary>
            /// 充值
            /// </summary>
            [Description("充值")]
            Recharge = 2,
            /// <summary>
            /// 表单订单
            /// </summary>
            [Description("表单订单")]
            FormOrder = 3,
            /// <summary>
            /// 表单付款码
            /// </summary>
            [Description("表单付款码")]
            FormPay = 4,
            /// <summary>
            /// 服务订单
            /// </summary>
            [Description("服务订单")]
            ServiceOrder = 5,
        }
        /// <summary>
        /// 付款单状态【对应CoreCmsBillPayments.status字段】
        /// </summary>
        public enum BillPaymentsStatus
        {
            /// <summary>
            /// 待支付
            /// </summary>
            [Description("未支付")]
            NoPay = 1,
            /// <summary>
            /// 已支付
            /// </summary>
            [Description("已支付")]
            Payed = 2,
            /// <summary>
            /// 其他
            /// </summary>
            [Description("其他")]
            Other = 3
        }
        #endregion

        #region BillAftersales售后单========================================================
        /// <summary>
        /// 售后单类型
        /// </summary>
        public enum BillAftersalesType
        {
            [Description("售后中")]
            售后中 = 1,
            [Description("售后通过")]
            售后通过 = 2,
            [Description("售后拒绝")]
            售后拒绝 = 3
        }

        /// <summary>
        /// 售后单状态
        /// </summary>
        public enum BillAftersalesStatus
        {
            /// <summary>
            /// 等待审核
            /// </summary>
            [Description("等待审核")]
            WaitAudit = 1,
            /// <summary>
            /// 审核通过
            /// </summary>
            [Description("审核通过")]
            Success = 2,
            /// <summary>
            /// 审核拒绝
            /// </summary>
            [Description("审核拒绝")]
            Refuse = 3
        }

        /// <summary>
        /// 是否收货
        /// </summary>
        public enum BillAftersalesIsReceive
        {
            /// <summary>
            /// 未收到货
            /// </summary>
            [Description("未收到货")]
            Refund = 1,
            /// <summary>
            /// 已收到货
            /// </summary>
            [Description("已收到货")]
            Reship = 2
        }
        #endregion

        #region BillRefund退款单========================================================
        /// <summary>
        /// 退款单状态 【对应CoreCmsBillRefund.status字段】
        /// </summary>
        public enum BillRefundStatus
        {
            /// <summary>
            /// 未退款
            /// </summary>
            [Description("<button type=\"button\" class=\"layui-btn layui-btn-normal layui-btn-xs\">未退款</button>")]
            STATUS_NOREFUND = 1,
            /// <summary>
            /// 已退款
            /// </summary>
            [Description("<button type=\"button\" class=\"layui-btn layui-btn-xs\">已退款</button>")]
            STATUS_REFUND = 2,
            /// <summary>
            /// 同意退款但原路退还失败
            /// </summary>
            [Description("<button type=\"button\" class=\"layui-btn layui-btn-danger layui-btn-xs\">退还失败</button>")]
            STATUS_FAIL = 3,
            /// <summary>
            /// 拒绝
            /// </summary>
            [Description("<button type=\"button\" class=\"layui-btn layui-btn-disabled layui-btn-xs\">拒绝</button>")]
            STATUS_REFUSE = 4,
        }
        /// <summary>
        /// 退款单类型
        /// </summary>
        public enum BillRefundType
        {
            /// <summary>
            /// 订单
            /// </summary>
            [Description("订单")]
            Order = 1,
            /// <summary>
            /// 充值
            /// </summary>
            [Description("充值")]
            Recharge = 2,
            /// <summary>
            /// 表单订单
            /// </summary>
            [Description("表单订单")]
            FormOrder = 3,
            /// <summary>
            /// 表单付款码
            /// </summary>
            [Description("表单付款码")]
            FormPay = 4,
            /// <summary>
            /// 服务订单
            /// </summary>
            [Description("服务订单")]
            ServiceOrder = 5,
        }
        #endregion

        #region BillReship退货单========================================================
        /// <summary>
        /// 退货单状态
        /// </summary>
        public enum BillReshipStatus
        {
            [Description("待退货/审核通过待发货")]
            待退货 = 1,
            [Description("运输中/已发退货")]
            运输中 = 2,
            [Description("已收退货")]
            已收退货 = 5
        }
        #endregion

        #region BillLading提货单========================================================
        /// <summary>
        /// 提货单状态
        /// </summary>
        public enum BillLadingStatus
        {
            /// <summary>
            /// 未提货
            /// </summary>
            [Description("未提货")]
            Order = 1,
            /// <summary>
            /// 已提货
            /// </summary>
            [Description("已提货")]
            Recharge = 2
        }
        #endregion

        #region BillDelivery发货单========================================================
        /// <summary>
        /// 发货单状态
        /// </summary>
        public enum BillDeliveryStatus
        {
            /// <summary>
            /// 准备发货
            /// </summary>
            [Description("准备发货")]
            Ready = 1,
            /// <summary>
            /// 已发货
            /// </summary>
            [Description("已发货")]
            Already = 2,
            /// <summary>
            /// 已确认
            /// </summary>
            [Description("已确认")]
            Confirm = 3,
            /// <summary>
            /// 其它
            /// </summary>
            [Description("其它")]
            Other = 4
        }
        #endregion

        #region  PinTuan拼团=============================================================

        /// <summary>
        /// 拼团记录状态表[对应CoreCmsPinTuanRecord表status字段]
        /// </summary>
        public enum PinTuanRecordStatus
        {
            /// <summary>
            /// 拼团中
            /// </summary>
            [Description("拼团中")]
            InProgress = 1,
            /// <summary>
            /// 开团成功
            /// </summary>
            [Description("开团成功")]
            Succeed = 2,
            /// <summary>
            /// 开团失败
            /// </summary>
            [Description("开团失败")]
            Defeated = 3
        }


        /// <summary>
        /// 拼团规则是否在时间范围内的状态
        /// </summary>
        public enum PinTuanRuleStatus
        {
            /// <summary>
            /// 已开始
            /// </summary>
            [Description("已开始")]
            begin = 1,
            /// <summary>
            /// 未开始
            /// </summary>
            [Description("未开始")]
            notBegun = 2,
            /// <summary>
            /// 已过期
            /// </summary>
            [Description("已过期")]
            haveExpired = 3
        }

        #endregion

        #region Form表单相关==============================================================
        /// <summary>
        /// 自定义表单类型
        /// </summary>
        public enum FormTypes
        {
            [Description("订单")]
            订单 = 1,
            [Description("付款码")]
            付款码 = 2,
            [Description("留言")]
            留言 = 3,
            [Description("反馈")]
            反馈 = 4,
            [Description("登记")]
            登记 = 5,
            [Description("调研")]
            调研 = 6
        }
        /// <summary>
        /// 表单头部类型
        /// </summary>
        public enum FormHeadTypes
        {
            [Description("图片")]
            图片 = 1,
            [Description("轮播")]
            轮播 = 2,
            [Description("视频")]
            视频 = 3,
        }

        /// <summary>
        /// 表单字段类型
        /// </summary>
        public enum FormFieldTypes
        {
            /// <summary>
            /// 单选
            /// </summary>
            [Description("单选")]
            radio = 1,
            /// <summary>
            /// 复选
            /// </summary>
            [Description("复选")]
            checbox = 2,
            /// <summary>
            /// 文本框
            /// </summary>
            [Description("文本框")]
            text = 3,
            /// <summary>
            /// 文本域
            /// </summary>
            [Description("文本域")]
            textarea = 4,
            /// <summary>
            /// 日期
            /// </summary>
            [Description("日期")]
            date = 5,
            /// <summary>
            /// 时间
            /// </summary>
            [Description("时间")]
            time = 6,
            /// <summary>
            /// 商品
            /// </summary>
            [Description("商品")]
            goods = 7,
            /// <summary>
            /// 金额
            /// </summary>
            [Description("金额")]
            money = 8,
            /// <summary>
            /// 密码
            /// </summary>
            [Description("密码")]
            password = 9,
            /// <summary>
            /// 省市区
            /// </summary>
            [Description("省市区")]
            area = 10,
            /// <summary>
            /// 图片
            /// </summary>
            [Description("图片")]
            image = 11,
            /// <summary>
            /// 坐标
            /// </summary>
            [Description("坐标")]
            coordinate = 12
        }

        /// <summary>
        /// 表单验证类型
        /// </summary>
        public enum FormValidationTypes
        {
            [Description("string")]
            字符串 = 1,
            [Description("number")]
            数字 = 2,
            [Description("integer")]
            整数 = 3,
            [Description("price")]
            价格 = 4,
            [Description("email")]
            邮箱 = 5,
            [Description("mobile")]
            手机号 = 6,
            [Description("array")]
            多数据 = 7,
        }


        #endregion

        #region 商品相关==============================================================
        /// <summary>
        /// 商品参数表类型
        /// </summary>
        public enum GoodsParamTypes
        {
            /// <summary>
            /// 文本框
            /// </summary>
            [Description("文本框")]
            text = 1,
            /// <summary>
            /// 单选
            /// </summary>
            [Description("单选")]
            radio = 2,
            /// <summary>
            /// 复选框
            /// </summary>
            [Description("复选框")]
            checkbox = 3,
        }
        /// <summary>
        /// 商品分销方式
        /// </summary>
        public enum ProductsDistributionType
        {
            /// <summary>
            /// 全局设置
            /// </summary>
            [Description("全局设置")]
            Global = 1,
            /// <summary>
            /// 单独设置
            /// </summary>
            [Description("单独设置")]
            Detail = 2,
        }


        #endregion

        #region 配送======================================================
        /// <summary>
        /// 配送区域类型
        /// </summary>
        public enum ShipAreaType
        {
            /// <summary>
            /// 全部地区
            /// </summary>
            [Description("全部地区")]
            All = 1,
            /// <summary>
            /// 部分地区
            /// </summary>
            [Description("部分地区")]
            Part = 2
        }


        /// <summary>
        /// 配送状态正常还是停用
        /// </summary>
        public enum ShipStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            [Description("正常")]
            Yes = 1,
            /// <summary>
            /// 停用
            /// </summary>
            [Description("停用")]
            No = 2
        }

        /// <summary>
        /// 配送方式重量
        /// </summary>
        public enum ShipUnit
        {
            /// <summary>
            /// 500克
            /// </summary>
            [Description("500克")]
            K500 = 500,
            /// <summary>
            /// 1公斤
            /// </summary>
            [Description("1公斤")]
            K1000 = 1000,
            /// <summary>
            /// 1.2公斤
            /// </summary>
            [Description("1.2公斤")]
            K1200 = 1200,
            /// <summary>
            /// 2公斤
            /// </summary>
            [Description("2公斤")]
            K2000 = 2000,
            /// <summary>
            /// 5公斤
            /// </summary>
            [Description("5公斤")]
            K5000 = 5000,
            /// <summary>
            /// 10公斤
            /// </summary>
            [Description("10公斤")]
            K10000 = 10000,
            /// <summary>
            /// 20公斤
            /// </summary>
            [Description("20公斤")]
            K20000 = 20000,
            /// <summary>
            /// 50公斤
            /// </summary>
            [Description("50公斤")]
            K50000 = 50000
        }

        #endregion

        #region 消息推送=====================================================
        /// <summary>
        /// 模板列表类型[对应CoreCmsTemplate表type字段]
        /// </summary>
        public enum TemplateTypes
        {
            小程序 = 1
        }
        #endregion

        #region 短信相关=====================================================

        /// <summary>
        /// 短信消息分类
        /// </summary>
        public enum SmsMessageTypes
        {
            /// <summary>
            /// 账户注册
            /// </summary>
            [Description("账户注册")]
            Reg = 1,
            /// <summary>
            /// 账户登录
            /// </summary>
            [Description("账户登录")]
            Login = 2,
            /// <summary>
            /// 验证验证码
            /// </summary>
            [Description("验证验证码")]
            Veri = 3,
        }

        /// <summary>
        /// 平台消息分类
        /// </summary>
        public enum PlatformMessageTypes
        {
            /// <summary>
            /// 下单成功时
            /// </summary>
            [Description("下单成功时")]
            CreateOrder = 4,
            /// <summary>
            /// 订单支付成功时
            /// </summary>
            [Description("订单支付成功时")]
            OrderPayed = 5,
            /// <summary>
            /// 订单催付提醒
            /// </summary>
            [Description("订单催付提醒")]
            RemindOrderPay = 6,
            /// <summary>
            /// 订单发货通知
            /// </summary>
            [Description("订单发货通知")]
            DeliveryNotice = 7,
            /// <summary>
            /// 售后确认通过
            /// </summary>
            [Description("售后确认通过")]
            AfterSalesPass = 8,
            /// <summary>
            /// 用户退款成功通知
            /// </summary>
            [Description("用户退款成功通知")]
            RefundSuccess = 9,
            /// <summary>
            /// 订单付款成功平台通知
            /// </summary>
            [Description("订单付款成功平台通知")]
            SellerOrderNotice = 10,
            /// <summary>
            /// 通用类型
            /// </summary>
            [Description("通用类型")]
            Common = 11,
        }


        /// <summary>
        /// 商家消息类型
        /// </summary>
        public enum ShopMessageTypes
        {
            /// <summary>
            /// 有新的售后订单了
            /// </summary>
            [Description("有新的售后订单了")]
            AftersalesAdd = 1,

        }

        #endregion

        #region 微信配置相关=========================================================

        /// <summary>
        /// 授权方认证类型[关联CoreCmsWeixinAuthor表verifyTypeInfo字段]
        /// </summary>
        public enum WeiChatAuthorVerifyTypes
        {
            未认证 = -1, 微信认证 = 0
        }

        /// <summary>
        /// 微信消息类型[关联CoreCmsWeixinMessage表type字段]
        /// </summary>
        public enum WeiChatMessageTypes
        {
            [Description("文本消息")]
            文本消息 = 1,
            [Description("图片消息")]
            图片消息 = 2,
            [Description("图文消息")]
            图文消息 = 3
        }

        /// <summary>
        /// 微信支付交易类型
        /// </summary>
        public enum WeiChatPayTradeType
        {
            [Description("JSAPI")]
            JSAPI = 1,
            [Description("JSAPI_OFFICIAL")]
            JSAPI_OFFICIAL = 2,
            [Description("NATIVE")]
            NATIVE = 3,
            [Description("APP")]
            APP = 4,
            [Description("MWEB")]
            MWEB = 5
        }


        #endregion

        #region 价格相关==============================================

        /// <summary>
        /// 价格类型
        /// </summary>
        public enum PriceType
        {
            /// <summary>
            /// 销售价
            /// </summary>
            [Description("销售价")]
            price = 1,
            /// <summary>
            /// 市场价
            /// </summary>
            [Description("市场价")]
            mktprice = 2,
            /// <summary>
            /// 成本价
            /// </summary>
            [Description("成本价")]
            costprice = 3
        }
        #endregion

        #region 广告相关==================================================
        /// <summary>
        /// 广告表类型【关联CoreCmsAdvertisement.type字段】
        /// </summary>
        public enum AdvertPositionType
        {
            /// <summary>
            /// 网址URL
            /// </summary>
            [Description("网址URL")]
            Url = 1,
            /// <summary>
            /// 商品
            /// </summary>
            [Description("商品")]
            Good = 2,
            /// <summary>
            /// 文章
            /// </summary>
            [Description("文章")]
            Article = 3,
            /// <summary>
            /// 文章分类
            /// </summary>
            [Description("文章分类")]
            ArticleType = 4,
            /// <summary>
            /// 智能表单
            /// </summary>
            [Description("智能表单")]
            IntelligenceForm = 5
        }

        /// <summary>
        /// 广告模板编码
        /// </summary>
        public enum AdvertTemplateCode
        {
            /// <summary>
            /// 首页幻灯片广告位
            /// </summary>
            [Description("首页幻灯片广告位")]
            TplSlider = 1,
            /// <summary>
            /// 首页广告位1
            /// </summary>
            [Description("首页广告位1")]
            TplIndexBanner1 = 2,
            /// <summary>
            /// 首页广告位2
            /// </summary>
            [Description("首页广告位2")]
            TplIndexBanner2 = 3,
            /// <summary>
            /// 首页广告位3
            /// </summary>
            [Description("首页广告位3")]
            TplIndexBanner3 = 4,
            /// <summary>
            /// 分类页广告位
            /// </summary>
            [Description("分类页广告位")]
            TplClassBanner1 = 5
        }
        #endregion

        #region 促销相关===================================================
        /// <summary>
        /// 促销形式类型【对应CoreCmsPromotion.type字段】
        /// </summary>
        public enum PromotionType
        {
            /// <summary>
            /// 促销
            /// </summary>
            [Description("促销")]
            Promotion = 1,
            /// <summary>
            /// 优惠券
            /// </summary>
            [Description("优惠券")]
            Coupon = 2,
            /// <summary>
            /// 团购
            /// </summary>
            [Description("团购")]
            Group = 3,
            /// <summary>
            /// 秒杀
            /// </summary>
            [Description("秒杀")]
            Seckill = 4,
        }


        /// <summary>
        /// 团购秒杀活动状态
        /// </summary>
        public enum GroupSeckillStatus
        {
            /// <summary>
            /// 即将开始
            /// </summary>
            [Description("即将开始")]
            Upcoming = 0,
            /// <summary>
            /// 进行中
            /// </summary>
            [Description("进行中")]
            InProgress = 1,
            /// <summary>
            /// 已结束
            /// </summary>
            [Description("已结束")]
            Finished = 2
        }


        #endregion

        #region 分享相关============================================
        /// <summary>
        /// 分享类型
        /// </summary>
        public enum ShareType
        {
            /// <summary>
            /// url地址类型
            /// </summary>
            [Description("url地址类型")]
            Url = 1,
            /// <summary>
            /// 二维码
            /// </summary>
            [Description("二维码")]
            QrCode = 2,
            /// <summary>
            /// 海报
            /// </summary>
            [Description("海报")]
            Poster = 3,
        }

        /// <summary>
        /// Url分享场景值
        /// </summary>
        public enum UrlSharePageType
        {
            /// <summary>
            /// 首页
            /// </summary>
            [Description("首页")]
            Index = 1,
            /// <summary>
            /// 2商品详情页
            /// </summary>
            [Description("2商品详情页")]
            Goods = 2,
            /// <summary>
            /// 3拼团详情页
            /// </summary>
            [Description("3拼团详情页")]
            PinTuan = 3,
            /// <summary>
            /// 4邀请页面
            /// </summary>
            [Description("4邀请页面")]
            Inv = 4,
            /// <summary>
            /// 5文章页面
            /// </summary>
            [Description("5文章页面")]
            Article = 5,
            /// <summary>
            /// 6参团页面
            /// </summary>
            [Description("6参团页面")]
            AddPinTuan = 6,
            /// <summary>
            /// 7自定义页面
            /// </summary>
            [Description("7自定义页面")]
            Page = 7,
            /// <summary>
            /// 8智能表单
            /// </summary>
            [Description("8智能表单")]
            Form = 8,
            /// <summary>
            /// 9团购
            /// </summary>
            [Description("9团购")]
            Group = 9,
            /// <summary>
            /// 10秒杀
            /// </summary>
            [Description("10秒杀")]
            Seckill = 10,
            /// <summary>
            /// 11代理
            /// </summary>
            [Description("11代理")]
            Agent = 11,
        }

        /// <summary>
        /// 分享来源
        /// </summary>
        public enum UrlShareClentType
        {
            /// <summary>
            /// 1普通h5
            /// </summary>
            [Description("1普通h5")]
            H5 = 1,
            /// <summary>
            /// 2微信小程序
            /// </summary>
            [Description("2微信小程序")]
            Wxmnapp = 2,
            /// <summary>
            /// 3微信公众号（h5）
            /// </summary>
            [Description("3微信公众号（h5）")]
            Wxofficial = 3,
            /// <summary>
            /// 4头条系小程序
            /// </summary>
            [Description("4头条系小程序")]
            Ttmnapp = 4,
            /// <summary>
            /// 5pc
            /// </summary>
            [Description("5pc")]
            Pc = 5,
            /// <summary>
            /// 6阿里小程序
            /// </summary>
            [Description("6阿里小程序")]
            Alimnapp = 6,
        }

        #endregion

        #region Area区域相关=================================================================
        /// <summary>
        /// 区域深度
        /// </summary>
        public enum AreaDepth
        {
            /// <summary>
            /// 省
            /// </summary>
            [Description("省")]
            Province = 1,
            /// <summary>
            /// 市
            /// </summary>
            [Description("市")]
            City = 2,
            /// <summary>
            /// 县
            /// </summary>
            [Description("县")]
            County = 3,
            /// <summary>
            /// 根节点
            /// </summary>
            [Description("根节点")]
            ProvinceParentId = 0,
        }
        #endregion

        #region 评价=======================================
        /// <summary>
        /// 评价类型
        /// </summary>
        public enum CommentTypes
        {
            [Description("好评")]
            好评 = 1,
            [Description("中评")]
            中评 = 2,
            [Description("差评")]
            差评 = -1
        }
        #endregion

        #region 银行卡相关
        /// <summary>
        /// 用户银行卡类型[对应CoreCmsUserBankCard的cardType字段]
        /// </summary>
        public enum BankType
        {
            /// <summary>
            /// 储蓄卡
            /// </summary>
            [Description("储蓄卡")]
            BankTypeDc = 1,
            /// <summary>
            /// 信用卡
            /// </summary>
            [Description("信用卡")]
            BankTypeCc = 2
        }

        public enum BankDefault
        {
            /// <summary>
            /// 默认
            /// </summary>
            [Description("默认")]
            DefaultYes = 1,
            /// <summary>
            /// 不默认
            /// </summary>
            [Description("不默认")]
            DefaultNo = 2
        }

        /// <summary>
        /// 银行名称及编码列表
        /// </summary>
        public enum BankList
        {
            [Description("深圳农村商业银行")] SRCB,
            [Description("广西北部湾银行")] BGB,
            [Description("上海农村商业银行")] SHRCB,
            [Description("北京银行")] BJBANK,
            [Description("威海市商业银行")] WHCCB,
            [Description("周口银行")] BOZK,
            [Description("库尔勒市商业银行")] KORLABANK,
            [Description("平安银行")] SPABANK,
            [Description("顺德农商银行")] SDEB,
            [Description("湖北省农村信用社")] HURCB,
            [Description("无锡农村商业银行")] WRCB,
            [Description("朝阳银行")] BOCY,
            [Description("浙商银行")] CZBANK,
            [Description("邯郸银行")] HDBANK,
            [Description("中国银行")] BOC,
            [Description("东莞银行")] BOD,
            [Description("中国建设银行")] CCB,
            [Description("遵义市商业银行")] ZYCBANK,
            [Description("绍兴银行")] SXCB,
            [Description("贵州省农村信用社")] GZRCU,
            [Description("张家口市商业银行")] ZJKCCB,
            [Description("锦州银行")] BOJZ,
            [Description("平顶山银行")] BOP,
            [Description("汉口银行")] HKB,
            [Description("上海浦东发展银行")] SPDB,
            [Description("宁夏黄河农村商业银行")] NXRCU,
            [Description("广东南粤银行")] NYNB,
            [Description("广州农商银行")] GRCB,
            [Description("苏州银行")] BOSZ,
            [Description("杭州银行")] HZCB,
            [Description("衡水银行")] HSBK,
            [Description("湖北银行")] HBC,
            [Description("嘉兴银行")] JXBANK,
            [Description("华融湘江银行")] HRXJB,
            [Description("丹东银行")] BODD,
            [Description("安阳银行")] AYCB,
            [Description("恒丰银行")] EGBANK,
            [Description("国家开发银行")] CDB,
            [Description("江苏太仓农村商业银行")] TCRCB,
            [Description("南京银行")] NJCB,
            [Description("郑州银行")] ZZBANK,
            [Description("德阳商业银行")] DYCB,
            [Description("宜宾市商业银行")] YBCCB,
            [Description("四川省农村信用")] SCRCU,
            [Description("昆仑银行")] KLB,
            [Description("莱商银行")] LSBANK,
            [Description("尧都农商行")] YDRCB,
            [Description("重庆三峡银行")] CCQTGB,
            [Description("富滇银行")] FDB,
            [Description("江苏省农村信用联合社")] JSRCU,
            [Description("济宁银行")] JNBANK,
            [Description("招商银行")] CMB,
            [Description("晋城银行JCBANK")] JINCHB,
            [Description("阜新银行")] FXCB,
            [Description("武汉农村商业银行")] WHRCB,
            [Description("湖北银行宜昌分行")] HBYCBANK,
            [Description("台州银行")] TZCB,
            [Description("泰安市商业银行")] TACCB,
            [Description("许昌银行")] XCYH,
            [Description("中国光大银行")] CEB,
            [Description("宁夏银行")] NXBANK,
            [Description("徽商银行")] HSBANK,
            [Description("九江银行")] JJBANK,
            [Description("农信银清算中心")] NHQS,
            [Description("浙江民泰商业银行")] MTBANK,
            [Description("廊坊银行")] LANGFB,
            [Description("鞍山银行")] ASCB,
            [Description("昆山农村商业银行")] KSRB,
            [Description("玉溪市商业银行")] YXCCB,
            [Description("大连银行")] DLB,
            [Description("东莞农村商业银行")] DRCBCL,
            [Description("广州银行")] GCB,
            [Description("宁波银行")] NBBANK,
            [Description("营口银行")] BOYK,
            [Description("陕西信合")] SXRCCU,
            [Description("桂林银行")] GLBANK,
            [Description("青海银行")] BOQH,
            [Description("成都农商银行")] CDRCB,
            [Description("青岛银行")] QDCCB,
            [Description("东亚银行")] HKBEA,
            [Description("湖北银行黄石分行")] HBHSBANK,
            [Description("温州银行")] WZCB,
            [Description("天津农商银行")] TRCB,
            [Description("齐鲁银行")] QLBANK,
            [Description("广东省农村信用社联合社")] GDRCC,
            [Description("浙江泰隆商业银行")] ZJTLCB,
            [Description("赣州银行")] GZB,
            [Description("贵阳市商业银行")] GYCB,
            [Description("重庆银行")] CQBANK,
            [Description("龙江银行")] DAQINGB,
            [Description("南充市商业银行")] CGNB,
            [Description("三门峡银行")] SCCB,
            [Description("常熟农村商业银行")] CSRCB,
            [Description("上海银行")] SHBANK,
            [Description("吉林银行")] JLBANK,
            [Description("常州农村信用联社")] CZRCB,
            [Description("潍坊银行")] BANKWF,
            [Description("张家港农村商业银行")] ZRCBANK,
            [Description("福建海峡银行")] FJHXBC,
            [Description("浙江省农村信用社联合社")] ZJNX,
            [Description("兰州银行")] LZYH,
            [Description("晋商银行")] JSB,
            [Description("渤海银行")] BOHAIB,
            [Description("浙江稠州商业银行")] CZCB,
            [Description("阳泉银行")] YQCCB,
            [Description("盛京银行")] SJBANK,
            [Description("西安银行")] XABANK,
            [Description("包商银行")] BSB,
            [Description("江苏银行")] JSBANK,
            [Description("抚顺银行")] FSCB,
            [Description("河南省农村信用")] HNRCU,
            [Description("交通银行")] COMM,
            [Description("邢台银行")] XTB,
            [Description("中信银行")] CITIC,
            [Description("华夏银行")] HXBANK,
            [Description("湖南省农村信用社")] HNRCC,
            [Description("东营市商业银行")] DYCCB,
            [Description("鄂尔多斯银行")] ORBANK,
            [Description("北京农村商业银行")] BJRCB,
            [Description("信阳银行")] XYBANK,
            [Description("自贡市商业银行")] ZGCCB,
            [Description("成都银行")] CDCB,
            [Description("韩亚银行")] HANABANK,
            [Description("中国民生银行")] CMBC,
            [Description("洛阳银行")] LYBANK,
            [Description("广东发展银行")] GDB,
            [Description("齐商银行")] ZBCB,
            [Description("开封市商业银行")] CBKF,
            [Description("内蒙古银行")] H3CB,
            [Description("兴业银行")] CIB,
            [Description("重庆农村商业银行")] CRCBANK,
            [Description("石嘴山银行")] SZSBK,
            [Description("德州银行")] DZBANK,
            [Description("上饶银行")] SRBANK,
            [Description("乐山市商业银行")] LSCCB,
            [Description("江西省农村信用")] JXRCU,
            [Description("中国工商银行")] ICBC,
            [Description("晋中市商业银行")] JZBANK,
            [Description("湖州市商业银行")] HZCCB,
            [Description("南海农村信用联社")] NHB,
            [Description("新乡银行")] XXBANK,
            [Description("江苏江阴农村商业银行")] JRCB,
            [Description("云南省农村信用社")] YNRCC,
            [Description("中国农业银行")] ABC,
            [Description("广西省农村信用")] GXRCU,
            [Description("中国邮政储蓄银行")] PSBC,
            [Description("驻马店银行")] BZMD,
            [Description("安徽省农村信用社")] ARCU,
            [Description("甘肃省农村信用")] GSRCU,
            [Description("辽阳市商业银行")] LYCB,
            [Description("吉林农信")] JLRCU,
            [Description("乌鲁木齐市商业银行")] URMQCCB,
            [Description("中山小榄村镇银行")] XLBANK,
            [Description("长沙银行")] CSCB,
            [Description("金华银行")] JHBANK,
            [Description("河北银行")] BHB,
            [Description("鄞州银行")] NBYZ,
            [Description("临商银行")] LSBC,
            [Description("承德银行")] BOCD,
            [Description("山东农信")] SDRCU,
            [Description("南昌银行")] NCB,
            [Description("天津银行")] TCCB,
            [Description("吴江农商银行")] WJRCB,
            [Description("城市商业银行资金清算中心")] CBBQS,
            [Description("河北省农村信用社")] HBRCU,
        }
        #endregion

        #region 分销设置
        /// <summary>
        /// 分销商申请审核状态
        /// </summary>
        public enum DistributionVerifyStatus
        {
            /// <summary>
            /// 审核通过
            /// </summary>
            [Description("审核通过")]
            VerifyYes = 1,
            /// <summary>
            /// 等待审核
            /// </summary>
            [Description("等待审核")]
            VerifyWait = 2,
            /// <summary>
            /// 审核拒绝
            /// </summary>
            [Description("审核拒绝")]
            VerifyRefuse = 3,
        }
        /// <summary>
        /// 分销商订单记录表是否结算状态
        /// </summary>

        public enum DistributionOrderSettlementStatus
        {
            /// <summary>
            /// 已结算
            /// </summary>
            [Description("已结算")]
            SettlementYes = 1,
            /// <summary>
            /// 未结算
            /// </summary>
            [Description("未结算")]
            SettlementNo = 2,
            /// <summary>
            /// 已失效
            /// </summary>
            [Description("已失效")]
            SettlementCancel = 3,
        }

        /// <summary>
        /// 分销升级相关编码类型
        /// </summary>
        public enum DistributionConditions
        {
            /// <summary>
            /// 购买所有商品
            /// </summary>
            [Description("购买所有商品")]
            GOODS_ALL = 1,
            /// <summary>
            /// 购买指定商品
            /// </summary>
            [Description("购买指定商品")]
            GOODS_IDS = 2,
            /// <summary>
            /// 个人消费总额
            /// </summary>
            [Description("个人消费总额")]
            USER_ORDERS = 3,
            /// <summary>
            /// 个人订单数量
            /// </summary>
            [Description("个人订单数量")]
            USER_ORDERSNUM = 4,
            /// <summary>
            /// 团队消费总额
            /// </summary>
            [Description("团队消费总额")]
            GROUP_ORDERS = 5,
            /// <summary>
            /// 直推几个指定用户等级
            /// </summary>
            [Description("直推几个指定用户等级")]
            USER_GRADE = 6,
        }


        /// <summary>
        /// 分销升级条件类型
        /// </summary>
        public enum DistributionConditionsCode
        {

            /// <summary>
            /// 个人消费总额
            /// </summary>
            [Description("个人消费总额(已完成的订单)")]
            USER_ORDERS = 3,
            /// <summary>
            /// 个人订单数量
            /// </summary>
            [Description("个人订单数量(已完成的订单)")]
            USER_ORDERSNUM = 4,
            /// <summary>
            /// 所有商品满足条件
            /// </summary>
            [Description("所有商品满足条件")]
            GOODS_ALL = 1,
            /// <summary>
            /// 购买指定商品
            /// </summary>
            [Description("购买指定商品")]
            GOODS_IDS = 2,

            //[Description("团队消费总额")]
            //GROUP_ORDERS = 5,
            //[Description("直推几个指定用户等级")]
            //USER_GRADE = 6,
        }


        /// <summary>
        /// 分销升级结果类型
        /// </summary>
        public enum DistributionCommissiontype
        {

            /// <summary>
            /// 百分比
            /// </summary>
            [Description("百分比")]
            COMMISSION_TYPE_PRE = 1,
            /// <summary>
            /// 固定
            /// </summary>
            [Description("固定")]
            COMMISSION_TYPE_FIXED = 2,
        }

        /// <summary>
        /// 成为分销商条件
        /// </summary>
        public enum DistributionConditionType
        {

            /// <summary>
            /// 无条件（需审核）
            /// </summary>
            [Description("无条件（需审核）")]
            Unconditional = 1,
            /// <summary>
            /// 申请（需审核）
            /// </summary>
            [Description("申请（需审核）")]
            Apply = 2,
            /// <summary>
            /// 无需审核
            /// </summary>
            [Description("无需审核")]
            NoReview = 3,
        }

        #endregion

        #region 快递100api接口相关===================
        /// <summary>
        /// 快递100api接口返回状态码说明
        /// </summary>
        public enum KuaiDi100ApiResultStatus
        {
            /// <summary>
            /// 查询成功
            /// </summary>
            [Description("查询成功")]
            Status200,
            /// <summary>
            /// 提交的数据不完整，或者贵公司没授权
            /// </summary>
            [Description("提交的数据不完整，或者贵公司没授权")]
            Status400,
            /// <summary>
            /// 表示查询失败，或没有POST提交
            /// </summary>
            [Description("表示查询失败，或没有POST提交")]
            Status500,
            /// <summary>
            /// 服务器错误，快递100服务器压力过大或需要升级，暂停服务
            /// </summary>
            [Description("服务器错误，快递100服务器压力过大或需要升级，暂停服务")]
            Status501,
            /// <summary>
            /// 服务器繁忙
            /// </summary>
            [Description("服务器繁忙")]
            Status502,
            /// <summary>
            /// 验证签名失败
            /// </summary>
            [Description("验证签名失败")]
            Status503,

        }

        /// <summary>
        /// 快递100api接口运单签收状态服务说明
        /// </summary>
        public enum KuaiDi100ApiResultState
        {
            [Description("快件处于运输过程中")]
            在途 = 0,
            [Description("快件已由快递公司揽收")]
            揽收 = 1,
            [Description("快递100无法解析的状态，或者是需要人工介入的状态， 比方说收件人电话错误。")]
            疑难 = 2,
            [Description("正常签收")]
            签收 = 3,
            [Description("货物退回发货人并签收")]
            退签 = 4,
            [Description("货物正在进行派件")]
            派件 = 5,
            [Description("货物正处于返回发货人的途中")]
            退回 = 6,
            [Description("货物转给其他快递公司邮寄")]
            转投 = 7,
            [Description("货物等待清关")]
            待清关 = 10,
            [Description("货物正在清关流程中")]
            清关中 = 11,
            [Description("货物已完成清关流程")]
            已清关 = 12,
            [Description("货物在清关过程中出现异常")]
            清关异常 = 13,
            [Description("收件人明确拒收")]
            收件人拒签 = 14
        }

        #endregion

        #region 微信小程序消息模板类型

        public enum WeChatMsgTemplateType
        {
            /// <summary>
            /// 下单通知
            /// </summary>
            [Description("下单通知")]
            order,
            /// <summary>
            /// 催付通知
            /// </summary>
            [Description("催付通知")]
            cancel,
            /// <summary>
            /// 支付通知
            /// </summary>
            [Description("支付通知")]
            pay,
            /// <summary>
            /// 发货通知
            /// </summary>
            [Description("发货通知")]
            ship,
            /// <summary>
            /// 售后通知
            /// </summary>
            [Description("售后通知")]
            aftersale,
            /// <summary>
            /// 退款通知
            /// </summary>
            [Description("退款通知")]
            refund,
        }
        #endregion

        #region 第三方接口配置

        public enum ThirdPartyEquipment
        {
            /// <summary>
            /// 易联云打印机
            /// </summary>
            [Description("易联云打印机")]
            YiLianYun,
        }

        #endregion

        #region HangFire定时任务相关

        public enum HangFireQueuesConfig
        {
            /// <summary>
            /// 默认
            /// </summary>
            [Description("默认")]
            @default = 1,
            /// <summary>
            /// 接口
            /// </summary>
            [Description("接口")]
            apis = 2,
            /// <summary>
            /// 网站
            /// </summary>
            [Description("网站")]
            web = 3,
            /// <summary>
            /// 循环时间
            /// </summary>
            [Description("循环时间")]
            recurring = 4,
        }

        #endregion

        #region 服务项目相关

        /// <summary>
        /// 服务项目核销有效期类型
        /// </summary>
        public enum ServicesValidityType
        {
            /// <summary>
            /// 不限，就是不限制，永久可以使用。
            /// </summary>
            [Description("不限")]
            Unlimited = 1,
            /// <summary>
            /// 限时间段  ，就是买了之后，某个时间段才能用。
            /// </summary>
            [Description("限时间段")]
            TimeFrame = 2
        }

        /// <summary>
        /// 服务项目状态
        /// </summary>
        public enum ServicesStatus
        {
            /// <summary>
            /// 上架
            /// </summary>
            [Description("上架")]
            Shelve = 0,
            /// <summary>
            /// 下架
            /// </summary>
            [Description("下架")]
            UnShelve = 1,
            /// <summary>
            /// 售罄
            /// </summary>
            [Description("售罄")]
            SoldOut = 2
        }


        /// <summary>
        /// 服务券状态
        /// </summary>
        public enum ServicesTicketStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            [Description("<button type='button' class='layui-btn  layui-btn-normal  layui-btn-xs'>正常</button>")]
            Normal = 0,
            /// <summary>
            /// 过期
            /// </summary>
            [Description("<button type='button' class='layui-btn layui-btn-warm layui-btn-disabled layui-btn-xs'>过期</button>")]
            Overdue = 1,
            /// <summary>
            /// 作废
            /// </summary>
            [Description("<button type='button' class='layui-btn  layui-btn-primary layui-btn-disabled layui-btn-xs'>作废</button>")]
            Cancellation = 2,
            /// <summary>
            /// 已核销
            /// </summary>
            [Description("<button type='button' class='layui-btn layui-btn-disabled layui-btn-xs'>已核销</button>")]
            Verification = 3
        }

        /// <summary>
        /// 服务订单状态
        /// </summary>
        public enum ServicesOrderStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            [Description("<button type='button' class='layui-btn  layui-btn-normal  layui-btn-xs'>订单正常</button>")]
            正常 = 1,
            /// <summary>
            /// 作废
            /// </summary>
            [Description("<button type='button' class='layui-btn  layui-btn-primary layui-btn-disabled layui-btn-xs'>订单作废</button>")]
            作废 = 2,

            /// <summary>
            /// 过期
            /// </summary>
            [Description("<button type='button' class='layui-btn layui-btn-warm layui-btn-disabled layui-btn-xs'>订单过期</button>")]
            过期 = 3,

            /// <summary>
            /// 用罄
            /// </summary>
            [Description("<button type='button' class='layui-btn layui-btn-disabled layui-btn-xs'>订单用罄</button>")]
            用罄 = 4
        }


        /// <summary>
        /// 服务是否在时间范围内的状态
        /// </summary>
        public enum ServicesOpenStatus
        {
            /// <summary>
            /// 已开始
            /// </summary>
            [Description("已开始")]
            begin = 1,
            /// <summary>
            /// 未开始
            /// </summary>
            [Description("未开始")]
            notBegun = 2,
            /// <summary>
            /// 已过期
            /// </summary>
            [Description("已过期")]
            haveExpired = 3
        }

        #endregion

        #region 页面设计相关

        /// <summary>
        /// 是否默认
        /// </summary>
        public enum PagesType
        {
            /// <summary>
            /// 是
            /// </summary>
            [Description("是")]
            Mobile = 1,
            /// <summary>
            /// 否
            /// </summary>
            [Description("否")]
            否 = 2,
        }

        /// <summary>
        /// 布局样式编码
        /// </summary>
        public enum PagesLayout
        {
            /// <summary>
            /// 移动端
            /// </summary>
            [Description("移动端")]
            Mobile = 1,
            /// <summary>
            /// PC端
            /// </summary>
            [Description("PC")]
            PC = 2,
        }
        #endregion

        #region 商城关键词说明

        public enum ShopServiceNoteType
        {
            /// <summary>
            /// 常见问题
            /// </summary>
            [Description("常见问题")]
            CommonQuestion = 1,
            /// <summary>
            /// 服务
            /// </summary>
            [Description("服务")]
            Service = 2,
            /// <summary>
            /// 发货
            /// </summary>
            [Description("发货")]
            Delivery = 3,

        }

        #endregion

        #region 库存

        /// <summary>
        /// 库存操作单类型
        /// </summary>
        public enum StockType
        {
            /// <summary>
            /// 入库
            /// </summary>
            [Description("入库")]
            In = 1,
            /// <summary>
            /// 出库
            /// </summary>
            [Description("出库")]
            Out = 2,
            /// <summary>
            /// 库存盘点
            /// </summary>
            [Description("库存盘点")]
            CheckGoods = 3,
            /// <summary>
            /// 发货
            /// </summary>
            [Description("发货")]
            DeliverGoods = 4,
            /// <summary>
            /// 退货
            /// </summary>
            [Description("退货")]
            ReturnedGoods = 5,
        }

        #endregion

        #region 代理设置
        /// <summary>
        /// 代理商申请审核状态
        /// </summary>
        public enum AgentVerifyStatus
        {
            /// <summary>
            /// 审核通过
            /// </summary>
            [Description("审核通过")]
            VerifyYes = 1,
            /// <summary>
            /// 等待审核
            /// </summary>
            [Description("等待审核")]
            VerifyWait = 2,
            /// <summary>
            /// 审核拒绝
            /// </summary>
            [Description("审核拒绝")]
            VerifyRefuse = 3,
        }
        /// <summary>
        /// 代理商订单记录表是否结算状态
        /// </summary>

        public enum AgentOrderSettlementStatus
        {
            /// <summary>
            /// 已结算
            /// </summary>
            [Description("已结算")]
            SettlementYes = 1,
            /// <summary>
            /// 未结算
            /// </summary>
            [Description("未结算")]
            SettlementNo = 2,
            /// <summary>
            /// 已失效
            /// </summary>
            [Description("已失效")]
            SettlementCancel = 3,
        }

        /// <summary>
        /// 代理默认价格加成方式
        /// </summary>
        public enum AgentDefaultSalesPriceType
        {

            /// <summary>
            /// 百分比
            /// </summary>
            [Description("百分比")]
            CommissionTypePre = 1,
            /// <summary>
            /// 固定
            /// </summary>
            [Description("固定")]
            CommissionTypeFixed = 2,
        }

        #endregion


        #region 接龙相关

        public enum SolitaireStatus
        {

            /// <summary>
            /// 开启
            /// </summary>
            [Description("开启")]
            Open = 1,
            /// <summary>
            /// 关闭
            /// </summary>
            [Description("关闭")]
            Close = 2,
        }
        #endregion

        #region redis缓存类型
        public enum AccessTokenEnum
        {

            /// <summary>
            /// 微信小程序
            /// </summary>
            WxOpenAccessToken = 1,

            /// <summary>
            /// 微信公众号
            /// </summary>
            WeiXinAccessToken = 2,

            /// <summary>
            /// 易联云打印机
            /// </summary>
            YiLianYunAccessToken = 3,
        }
        #endregion

    }
}
