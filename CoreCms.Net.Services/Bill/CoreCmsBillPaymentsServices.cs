/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.WeChat.Service.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 支付单表 接口实现
    /// </summary>
    public class CoreCmsBillPaymentsServices : BaseServices<CoreCmsBillPayments>, ICoreCmsBillPaymentsServices
    {
        private readonly ICoreCmsBillPaymentsRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;


        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsUserBalanceServices _userBalanceServices;
        private readonly ICoreCmsFormSubmitServices _formSubmitServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private IWeChatPayServices _weChatPayServices;
        private readonly ICoreCmsPaymentsServices _paymentsServices;
        private readonly ICoreCmsOrderItemServices _orderItemServices;
        private readonly ICoreCmsServicesServices _servicesServices;
        private readonly ICoreCmsUserServicesOrderServices _userServicesOrderServices;
        private readonly ICoreCmsUserWeChatInfoServices _userWeChatInfoServices;
        private readonly WeChatOptions _weChatOptions;




        public CoreCmsBillPaymentsServices(IUnitOfWork unitOfWork
            , ICoreCmsBillPaymentsRepository dal
            , ICoreCmsSettingServices settingServices
            , IHttpContextAccessor httpContextAccessor
            , ICoreCmsUserBalanceServices userBalanceServices
            , ICoreCmsFormSubmitServices formSubmitServices
            //, IWeChatPayServices weChatPayServices
            , ICoreCmsPaymentsServices paymentsServices
            , ICoreCmsOrderItemServices orderItemServices
            , IServiceProvider serviceProvider, ICoreCmsServicesServices servicesServices
            , ICoreCmsUserServicesOrderServices userServicesOrderServices
            , ICoreCmsUserWeChatInfoServices userWeChatInfoServices
            , IOptions<WeChatOptions> weChatOptions
            )
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;

            _httpContextAccessor = httpContextAccessor;
            _settingServices = settingServices;
            _userBalanceServices = userBalanceServices;
            _formSubmitServices = formSubmitServices;
            //_weChatPayServices = weChatPayServices;
            _formSubmitServices = formSubmitServices;
            _paymentsServices = paymentsServices;
            _orderItemServices = orderItemServices;
            _serviceProvider = serviceProvider;
            _servicesServices = servicesServices;
            _userServicesOrderServices = userServicesOrderServices;
            _userWeChatInfoServices = userWeChatInfoServices;
            _weChatOptions = weChatOptions.Value;
        }

        #region 生成支付单的时候，格式化支付单明细

        /// <summary>
        /// 生成支付单的时候，格式化支付单明细
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="type"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> FormatPaymentRel(string orderId, int type, JObject @params)
        {
            using var container = _serviceProvider.CreateScope();
            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();

            var jm = new WebApiCallBack();

            var dto = new CheckPayDTO();

            //订单
            if (type == (int)GlobalEnumVars.BillPaymentsType.Order)
            {
                //如果是订单生成支付单的话，取第一条订单的店铺id，后面的所有订单都要保证是此店铺的id
                var orderModel = await orderServices.QueryByClauseAsync(p =>
                    p.orderId == orderId && p.payStatus == (int)GlobalEnumVars.OrderPayStatus.No &&
                    p.status == (int)GlobalEnumVars.OrderStatus.Normal);
                if (orderModel != null)
                {
                    dto.rel.Add(new rel()
                    {
                        sourceId = orderId,
                        money = orderModel.orderAmount
                    });
                    dto.money += orderModel.orderAmount;
                }
                else
                {
                    jm.status = false;
                    jm.msg = "订单号：" + orderId + "没有找到,或不是未支付状态";
                    return jm;
                }
                jm.status = true;
                jm.data = dto;

            }
            //充值
            else if (type == (int)GlobalEnumVars.BillPaymentsType.Recharge)
            {
                if (@params != null && @params.ContainsKey("money"))
                {
                    dto.money = @params["money"].ObjectToDecimal(0); //充值金额
                }
                else
                {
                    jm.status = false;
                    jm.msg = "请输入正确的充值金额";
                    return jm;
                }
                dto.rel.Add(new rel()
                {
                    sourceId = orderId,
                    money = dto.money
                });
                jm.status = true;
                jm.data = dto;
            }
            //表单
            else if (type == (int)GlobalEnumVars.BillPaymentsType.FormPay || type == (int)GlobalEnumVars.BillPaymentsType.FormOrder)
            {
                dto.money = 0;
                var intId = orderId.ObjectToInt(0);
                if (intId <= 0)
                {
                    jm.status = false;
                    jm.msg = "表单：" + intId + "没有找到,或不是未支付状态";
                    return jm;
                }


                var formInfo = await _formSubmitServices.QueryByClauseAsync(p => p.id == intId && p.payStatus == false);
                if (formInfo != null)
                {
                    dto.rel.Add(new rel()
                    {
                        sourceId = intId.ToString(),
                        money = formInfo.money
                    });
                    dto.money += formInfo.money;
                }
                else
                {
                    jm.status = false;
                    jm.msg = "表单：" + intId + "没有找到,或不是未支付状态";
                    return jm;
                }
                jm.status = true;
                jm.data = dto;
            }
            else if (type == (int)GlobalEnumVars.BillPaymentsType.ServiceOrder)
            {
                dto.money = 0;

                var order = await _userServicesOrderServices.QueryByClauseAsync(p => p.serviceOrderId == orderId);

                var dt = DateTime.Now;
                var where = PredicateBuilder.True<CoreCmsServices>();
                @where = @where.And(p => p.status == (int)GlobalEnumVars.ServicesStatus.Shelve);
                @where = @where.And(p => p.amount > 0);
                @where = @where.And(p => p.startTime < dt && p.endTime > dt);
                @where = @where.And(p => p.id == order.servicesId);

                var serviceInfo = await _servicesServices.QueryByClauseAsync(@where);
                if (serviceInfo != null)
                {
                    dto.rel.Add(new rel()
                    {
                        sourceId = orderId,
                        money = serviceInfo.money
                    });
                    dto.money += serviceInfo.money;
                }
                else
                {
                    jm.status = false;
                    jm.msg = "服务订单：" + orderId + "没有找到,或不是有效状态";
                    return jm;
                }

                jm.status = true;
                jm.data = dto;
            }

            else if (false)
            {
                //todo 其他业务逻辑
            }
            else
            {
                jm.status = false;
                jm.msg = GlobalErrorCodeVars.Code10054;
                jm.data = 10054;
                return jm;
            }

            return jm;
        }


        #endregion

        #region 生成支付单的时候，格式化支付单明细

        /// <summary>
        /// 生成支付单的时候，格式化支付单明细
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <param name="type"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BatchFormatPaymentRel(string[] sourceStr, int type, JObject @params)
        {
            using var container = _serviceProvider.CreateScope();
            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();

            var jm = new WebApiCallBack();
            var dto = new CheckPayDTO();

            //订单
            if (type == (int)GlobalEnumVars.BillPaymentsType.Order)
            {
                //如果是订单生成支付单的话，取第一条订单的店铺id，后面的所有订单都要保证是此店铺的id
                foreach (var item in sourceStr)
                {
                    var orderModel = await orderServices.QueryByClauseAsync(p =>
                         p.orderId == item && p.payStatus == (int)GlobalEnumVars.OrderPayStatus.No &&
                         p.status == (int)GlobalEnumVars.OrderStatus.Normal);
                    if (orderModel != null)
                    {
                        dto.rel.Add(new rel()
                        {
                            sourceId = item,
                            money = orderModel.orderAmount
                        });
                        dto.money += orderModel.orderAmount;
                    }
                    else
                    {
                        jm.status = false;
                        jm.msg = "订单号：" + item + "没有找到,或不是未支付状态";
                        return jm;
                    }
                }
                jm.status = true;
                jm.data = dto;
            }
            //充值
            else if (type == (int)GlobalEnumVars.BillPaymentsType.Recharge)
            {
                if (@params != null && @params.ContainsKey("money"))
                {
                    dto.money = @params["money"].ObjectToDecimal(0); //充值金额
                }
                else
                {
                    jm.status = false;
                    jm.msg = "请输入正确的充值金额";
                    return jm;
                }
                foreach (var item in sourceStr)
                {
                    dto.rel.Add(new rel()
                    {
                        sourceId = item,
                        money = dto.money
                    });
                }
                jm.status = true;
                jm.data = dto;
            }
            //表单
            else if (type == (int)GlobalEnumVars.BillPaymentsType.FormPay || type == (int)GlobalEnumVars.BillPaymentsType.FormOrder)
            {
                dto.money = 0;
                var intIds = CommonHelper.StringArrAyToIntArray(sourceStr);

                foreach (var item in intIds)
                {
                    var formInfo = await _formSubmitServices.QueryByClauseAsync(p => p.id == item && p.payStatus == false);
                    if (formInfo != null)
                    {
                        dto.rel.Add(new rel()
                        {
                            sourceId = item.ToString(),
                            money = formInfo.money
                        });
                        dto.money += formInfo.money;
                    }
                    else
                    {
                        jm.status = false;
                        jm.msg = "表单：" + item + "没有找到,或不是未支付状态";
                        return jm;
                    }

                }
                jm.status = true;
                jm.data = dto;
            }
            else if (type == (int)GlobalEnumVars.BillPaymentsType.ServiceOrder)
            {
                dto.money = 0;

                foreach (var item in sourceStr)
                {

                    var order = await _userServicesOrderServices.QueryByClauseAsync(p => p.serviceOrderId == item);

                    var dt = DateTime.Now;
                    var where = PredicateBuilder.True<CoreCmsServices>();
                    @where = @where.And(p => p.status == (int)GlobalEnumVars.ServicesStatus.Shelve);
                    @where = @where.And(p => p.amount > 0);
                    @where = @where.And(p => p.startTime < dt && p.endTime > dt);
                    @where = @where.And(p => p.id == order.servicesId);

                    var serviceInfo = await _servicesServices.QueryByClauseAsync(@where);
                    if (serviceInfo != null)
                    {
                        dto.rel.Add(new rel()
                        {
                            sourceId = item,
                            money = serviceInfo.money
                        });
                        dto.money += serviceInfo.money;
                    }
                    else
                    {
                        jm.status = false;
                        jm.msg = "服务订单：" + item + "没有找到,或不是有效状态";
                        return jm;
                    }

                }
                jm.status = true;
                jm.data = dto;
            }

            else if (false)
            {
                //todo 其他业务逻辑
            }
            else
            {
                jm.status = false;
                jm.msg = GlobalErrorCodeVars.Code10054;
                jm.data = 10054;
                return jm;
            }





            return jm;
        }


        #endregion


        #region 支付，先生成支付单，然后去支付
        /// <summary>
        /// 支付，先生成支付单，然后去支付
        /// </summary>
        /// <param name="sourceStr">来源，一般是订单号或者用户id，比如充值</param>
        /// <param name="paymentCode">支付方式</param>
        /// <param name="userId">用户序列</param>
        /// <param name="type">订单/充值/服务订单</param>
        /// <param name="params">支付的时候用到的参数，如果是微信支付的话，这里可以传trade_type=>'JSAPI'(小程序支付),或者'MWEB'(h5支付),当是JSPI的时候，可以不传其他参数了，默认就可以，默认的这个值就是JSAPI，如果是MWEB的话，需要传wap_url(网站url地址)参数和wap_name（网站名称）参数，其他支付方式需要传什么参数这个以后再说</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Pay(string sourceStr, string paymentCode, int userId, int type, JObject @params)
        {
            using var container = _serviceProvider.CreateScope();

            var weChatPayServices = container.ServiceProvider.GetService<IWeChatPayServices>();
            var aliPayServices = container.ServiceProvider.GetService<IAliPayServices>();
            var balancePayServices = container.ServiceProvider.GetService<IBalancePayServices>();
            var offlinePayServices = container.ServiceProvider.GetService<IOfflinePayServices>();

            var jm = new WebApiCallBack();

            //如果支付类型为余额充值，那么资源ID就是用户ID
            if (type == (int)GlobalEnumVars.BillPaymentsType.Recharge)
            {
                sourceStr = userId.ToString();
            }
            //判断支付方式是否开启
            var paymentInfo = await _paymentsServices.QueryByClauseAsync(p => p.code == paymentCode && p.isEnable == true);
            if (paymentInfo == null)
            {
                jm.data = jm.code = 10050;
                jm.msg = GlobalErrorCodeVars.Code10050;
                return jm;
            }
            //如果是公众号支付，并且没有登陆或者没有open_id的话，报错
            var res = await CheckOpenId(paymentCode, @params);
            if (res.status == false)
            {
                return res;
            }

            //生成支付单,只是单纯的生成了支付单
            var result = await ToAdd(sourceStr, paymentCode, userId, type, @params);
            if (result.status == false)
            {
                return result;
            }

            var billPayments = result.data as CoreCmsBillPayments;

            if (billPayments.money < 0)
            {
                jm.msg = "支付金额异常！";
                return jm;
            }

            //根据支付方式返回支付配置
            //微信支付
            if (paymentCode == GlobalEnumVars.PaymentsTypes.wechatpay.ToString())
            {
                jm = await weChatPayServices.PubPay(billPayments);
            }
            //支付宝支付
            else if (paymentCode == GlobalEnumVars.PaymentsTypes.alipay.ToString())
            {
                jm = aliPayServices.PubPay(billPayments);

            }
            //余额支付
            else if (paymentCode == GlobalEnumVars.PaymentsTypes.balancepay.ToString())
            {
                jm = await balancePayServices.PubPay(billPayments);

            }
            //线下支付
            else if (paymentCode == GlobalEnumVars.PaymentsTypes.offline.ToString())
            {
                jm = offlinePayServices.PubPay(billPayments);

            }

            return jm;
        }


        #endregion

        #region 如果是公众号支付，并且没有登陆或者没有open_id的话，报错
        /// <summary>
        /// 如果是公众号支付，并且没有登陆或者没有open_id的话，报错
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        private async Task<WebApiCallBack> CheckOpenId(string paymentCode, JObject jobj)
        {
            var jm = new WebApiCallBack { status = true };

            //当只有微信支付的时候，才判断
            if (paymentCode != "wechatpay") return jm;

            if (jobj != null)
            {
                //当只有公众号支付的时候，才判断
                if (jobj.ContainsKey("trade_type") && jobj["trade_type"].ObjectToString() == "JSAPI_OFFICIAL") return jm;
                if (jobj.ContainsKey("openid") && jobj["openid"].ObjectToString() != "") return jm;

                //到这里基本上就说明
                if (!jobj.ContainsKey("url"))
                {
                    jm.data = 10067;
                    jm.code = 10067;
                    jm.msg = GlobalErrorCodeVars.Code10067;
                    return jm;
                }
                var allConfigs = await _settingServices.GetConfigDictionaries();
                var wxOfficialAppid = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.WxOfficialAppid);
                var redirectUrl = CommonHelper.UrlEncode(jobj["url"].ObjectToString());

                jm.status = false;
                jm.data = 10006;

                jm.msg = $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={_weChatOptions.WeiXinAppId}&redirect_uri={redirectUrl}&response_type={"code"}&scope={3}&state={"corecms"}{"&connect_redirect=1"}#wechat_redirect";
            }
            return jm;
        }

        #endregion

        #region 生成支付单,只是单纯的生成了支付单

        /// <summary>
        /// 生成支付单,只是单纯的生成了支付单
        /// </summary>
        /// <param name="sourceStr">资源id字段</param>
        /// <param name="paymentCode">支付方式</param>
        /// <param name="userId">支付用户id</param>
        /// <param name="type">支付类型</param>
        /// <param name="params">参数</param>
        /// <returns></returns>
        private async Task<WebApiCallBack> ToAdd(string sourceStr, string paymentCode, int userId = 0, int type = (int)GlobalEnumVars.BillPaymentsType.Order, JObject @params = null)
        {
            var jm = new WebApiCallBack();

            //判断支付方式
            var paymentInfo = await _paymentsServices.QueryByClauseAsync(p => p.code == paymentCode && p.isEnable == true);
            if (paymentInfo == null)
            {
                jm.data = jm.code = 10050;
                jm.msg = GlobalErrorCodeVars.Code10050;
                return jm;
            }

            var paymentRelData = new CheckPayDTO();

            var sourceStrArr = sourceStr.Split(",");
            if (sourceStrArr.Length > 0)
            {
                var paymentRel = await BatchFormatPaymentRel(sourceStrArr, type, @params);
                if (paymentRel.status == false)
                {
                    return paymentRel;
                }
                paymentRelData = paymentRel.data as CheckPayDTO;
            }
            else
            {
                var paymentRel = await FormatPaymentRel(sourceStr, type, @params);
                if (paymentRel.status == false)
                {
                    return paymentRel;
                }
                paymentRelData = paymentRel.data as CheckPayDTO;
            }

            var billPayments = new CoreCmsBillPayments();
            billPayments.paymentId = CommonHelper.GetSerialNumberType((int)GlobalEnumVars.SerialNumberType.支付单编号);
            billPayments.sourceId = sourceStr;
            billPayments.money = paymentRelData.money;
            billPayments.userId = userId;
            billPayments.type = type;
            billPayments.status = (int)GlobalEnumVars.BillPaymentsStatus.NoPay;
            billPayments.paymentCode = paymentCode;
            billPayments.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";
            billPayments.parameters = @params != null ? JsonConvert.SerializeObject(@params) : "";
            billPayments.createTime = DateTime.Now;

            await _dal.InsertAsync(billPayments);

            //判断支付单金额是否为0，如果为0，直接支付成功,
            if (billPayments.money == 0)
            {
                //更新订单信息
                await ToUpdate(billPayments.paymentId, (int)GlobalEnumVars.BillPaymentsStatus.Payed, billPayments.paymentCode, billPayments.money, "金额为0，自动支付成功", "");

                jm.data = jm.code = 10059;
                jm.msg = GlobalErrorCodeVars.Code10059;
                return jm;
            }
            //取支付标题，就不往数据库里存了吧
            billPayments.payTitle = await PayTitle(billPayments);

            jm.status = true;
            jm.data = billPayments;

            return jm;
        }


        #endregion

        #region 支付成功后，更新支付单状态

        /// <summary>
        /// 支付成功后，更新支付单状态
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="paymentCode"></param>
        /// <param name="money"></param>
        /// <param name="status"></param>
        /// <param name="payedMsg"></param>
        /// <param name="tradeNo"></param>
        public async Task<WebApiCallBack> ToUpdate(string paymentId, int status, string paymentCode, decimal money, string payedMsg = "", string tradeNo = "")
        {
            using var container = _serviceProvider.CreateScope();

            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();

            var jm = new WebApiCallBack();

            var billPaymentInfo = await _dal.QueryByClauseAsync(p =>
                p.paymentId == paymentId && p.money == money &&
                p.status != (int)GlobalEnumVars.BillPaymentsStatus.Payed);
            if (billPaymentInfo == null)
            {
                NLogUtil.WriteAll(LogLevel.Trace, LogType.Order, "支付成功后，更新支付单状态", "没有找到此未支付的支付单号");
                jm.msg = "没有找到此未支付的支付单号";
                return jm;
            }

            billPaymentInfo.status = status;
            billPaymentInfo.paymentCode = paymentCode;
            billPaymentInfo.payedMsg = payedMsg;
            billPaymentInfo.tradeNo = tradeNo;
            billPaymentInfo.updateTime = DateTime.Now;

            await _dal.UpdateAsync(billPaymentInfo);
            if (status == (int)GlobalEnumVars.BillPaymentsStatus.Payed)
            {
                if (billPaymentInfo.type == (int)GlobalEnumVars.BillPaymentsType.Order)
                {
                    //如果是订单类型，做支付后处理
                    await orderServices.Pay(billPaymentInfo.sourceId, paymentCode, billPaymentInfo);
                }
                else if (billPaymentInfo.type == (int)GlobalEnumVars.BillPaymentsType.Recharge)
                {
                    //给用户做充值
                    var userId = billPaymentInfo.sourceId.ObjectToInt(0);
                    await _userBalanceServices.Change(userId, (int)GlobalEnumVars.UserBalanceSourceTypes.Recharge, billPaymentInfo.money, billPaymentInfo.paymentId);
                }
                else if (billPaymentInfo.type == (int)GlobalEnumVars.BillPaymentsType.ServiceOrder)
                {
                    //给用户做增加购买关系和生成券操作
                    await _userServicesOrderServices.CreateUserServicesTickets(billPaymentInfo.sourceId, billPaymentInfo.paymentId);
                }
                else if (billPaymentInfo.type == (int)GlobalEnumVars.BillPaymentsType.FormOrder || billPaymentInfo.type == (int)GlobalEnumVars.BillPaymentsType.FormPay)
                {
                    //form表单支付
                    var id = billPaymentInfo.sourceId.ObjectToInt(0);
                    await _formSubmitServices.Pay(id);
                }
                else
                {
                    //::todo 其他业务逻辑
                }
            }
            jm.status = true;
            jm.data = paymentId;
            jm.msg = "支付成功";

            return jm;
        }

        #endregion


        #region 获取支付单详情
        /// <summary>
        /// 获取支付单详情
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetInfo(string paymentId, int userId = 0)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(paymentId))
            {
                jm.msg = GlobalErrorCodeVars.Code13100;
                return jm;
            }
            var where = PredicateBuilder.True<CoreCmsBillPayments>();
            where = where.And(p => p.paymentId == paymentId);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            var billPayments = await _dal.QueryByClauseAsync(where);
            if (billPayments == null)
            {
                jm.msg = "没有找到此支付记录";
                jm.data = jm.code = 10002;
                return jm;
            }

            jm.status = true;
            jm.data = billPayments;
            return jm;
        }
        #endregion

        //扩展方法==========================================================================================

        #region 扩展方法
        private async Task<string> PayTitle(CoreCmsBillPayments entity)
        {

            var res = string.Empty;
            switch (entity.type)
            {
                case (int)GlobalEnumVars.BillPaymentsType.Order:
                    var orderItem = await _orderItemServices.QueryByClauseAsync(p => p.orderId == entity.sourceId);
                    if (orderItem != null)
                    {
                        res = orderItem.name;
                    }
                    break;
                case (int)GlobalEnumVars.BillPaymentsType.Recharge:
                    res = "账户充值";
                    break;
                case (int)GlobalEnumVars.BillPaymentsType.FormPay:
                    break;
                case (int)GlobalEnumVars.BillPaymentsType.FormOrder:
                    break;
                case (int)GlobalEnumVars.BillPaymentsType.ServiceOrder:
                    break;
                default:
                    break;
            }
            if (string.IsNullOrEmpty(res))
            {
                var allConfigs = await _settingServices.GetConfigDictionaries();
                res = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopName);  //店铺名称
            }
            return res;
        }

        #endregion

        #region 卖家直接支付操作
        /// <summary>
        /// 卖家直接支付操作
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="type">支付类型</param>
        /// <param name="paymentCode">支付类型编码</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ToPay(string orderId, int type, string paymentCode)
        {
            using (var container = _serviceProvider.CreateScope())
            {
                var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();

                var jm = new WebApiCallBack();

                //查支付人id
                var userId = 0;
                switch (type)
                {
                    case (int)GlobalEnumVars.BillPaymentsType.Order:
                        var orderInfo = await orderServices.QueryByIdAsync(orderId);
                        if (orderInfo == null)
                        {
                            jm.code = 10000;
                            jm.msg = GlobalErrorCodeVars.Code10000;
                            return jm;
                        }
                        userId = orderInfo.userId;
                        break;
                }
                //::todo 校验支付方式是否存在
                //生成支付单
                var result = await ToAdd(orderId, paymentCode, userId, type);
                if (!result.status)
                {
                    return result;
                }
                var data = result.data as CoreCmsBillPayments;
                //支付单支付
                jm = await ToUpdate(data.paymentId, (int)GlobalEnumVars.BillPaymentsStatus.Payed,
                    data.paymentCode, data.money, "后台手动支付");
                return jm;
            }
        }
        #endregion


        #region 支付单7天统计
        /// <summary>
        /// 支付单7天统计
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatisticsOut>> Statistics()
        {
            return await _dal.Statistics();
        }
        #endregion


        #region 重写根据条件查询分页数据
        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<IPageList<CoreCmsBillPayments>> QueryPageAsync(Expression<Func<CoreCmsBillPayments, bool>> predicate,
            Expression<Func<CoreCmsBillPayments, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion

    }
}
