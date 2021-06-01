/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-08-13 23:57:23
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using MediatR;
using Newtonsoft.Json;
using NLog;
using Qc.YilianyunSdk;

namespace CoreCms.Net.Services.Mediator
{
    /// <summary>
    /// 消息
    /// </summary>
    public class YiLianYunPrintCommand : IRequest<WebApiCallBack>
    {
        public CoreCmsOrder order { get; set; }
    }

    /// <summary>
    /// 处理器-订单完成后
    /// </summary>
    public class YiLianYunPrintEventHandler : IRequestHandler<YiLianYunPrintCommand, WebApiCallBack>
    {

        private readonly YilianyunService _yilianyunService;
        private readonly ICoreCmsApiAccessTokenServices _accessTokenServices;
        private readonly ICoreCmsOrderItemServices _orderItemServices;
        private readonly ICoreCmsAreaServices _areaServices;

        private readonly string _machineCode = AppSettingsConstVars.YiLianYunConfigMachineCode; //设备号
        private readonly string _msign = AppSettingsConstVars.YiLianYunConfigMsign;// 终端密钥
        private readonly string _printerName = AppSettingsConstVars.YiLianYunConfigPrinterName; // 打印机名称
        private readonly string _phone = AppSettingsConstVars.YiLianYunConfigPhone; //手机卡号
        private readonly bool _enabled = AppSettingsConstVars.YiLianYunConfigEnabled; //是否开启


        public YiLianYunPrintEventHandler(YilianyunService yilianyunService, ICoreCmsApiAccessTokenServices accessTokenServices, ICoreCmsOrderItemServices orderItemServices, ICoreCmsAreaServices areaServices)
        {
            _yilianyunService = yilianyunService;
            _accessTokenServices = accessTokenServices;
            _orderItemServices = orderItemServices;
            _areaServices = areaServices;



        }

        public async Task<WebApiCallBack> Handle(YiLianYunPrintCommand request, CancellationToken cancellationToken)
        {

            var jm = new WebApiCallBack();

            if (_enabled == false)
            {
                jm.msg = "打印机未开启";
                return await Task.FromResult(jm);
            }

            if (request.order == null)
            {
                jm.msg = "订单获取失败";
                return await Task.FromResult(jm);
            }

            var order = request.order;

            var accessModel = await _accessTokenServices.QueryByClauseAsync(p =>
                p.code == GlobalEnumVars.ThirdPartyEquipment.YiLianYun.ToString() && p.machineCode == _machineCode);
            string accessToken;
            if (accessModel == null)
            {
                var onPostAuthTerminal = _yilianyunService.AuthTerminal(_machineCode, _msign, _phone, _printerName);
                if (onPostAuthTerminal.IsSuccess())
                {
                    accessToken = onPostAuthTerminal.Body.Access_Token;

                    accessModel = new CoreCmsApiAccessToken();
                    accessModel.code = GlobalEnumVars.ThirdPartyEquipment.YiLianYun.ToString();
                    accessModel.name = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.ThirdPartyEquipment>(GlobalEnumVars.ThirdPartyEquipment.YiLianYun.ToString());
                    accessModel.machineCode = _machineCode;
                    accessModel.accessToken = onPostAuthTerminal.Body.Access_Token;
                    accessModel.refreshToken = onPostAuthTerminal.Body.Refresh_Token;
                    accessModel.expiresIn = onPostAuthTerminal.Body.Expires_In != null ? Convert.ToInt32(onPostAuthTerminal.Body.Expires_In) : 0;
                    accessModel.expiressEndTime = onPostAuthTerminal.Body.ExpiressEndTime;
                    accessModel.parameters = JsonConvert.SerializeObject(onPostAuthTerminal.Body);
                    accessModel.createTime = DateTime.Now;

                    await _accessTokenServices.InsertAsync(accessModel);
                }
                else
                {
                    jm.data = onPostAuthTerminal;
                    NLogUtil.WriteAll(LogLevel.Error, LogType.ApiRequest, "易联云重新获取Token", JsonConvert.SerializeObject(onPostAuthTerminal));
                    return await Task.FromResult(jm);
                }
            }
            else
            {
                //判断是否过期
                var dt = DateTime.Now;
                if (dt >= accessModel.expiressEndTime)
                {
                    var result = _yilianyunService.RefreshToken(accessModel.machineCode, accessModel.refreshToken);
                    if (result.IsSuccess())
                    {
                        accessToken = result.Body.Access_Token;

                        accessModel.accessToken = result.Body.Access_Token;
                        accessModel.refreshToken = result.Body.Refresh_Token;
                        accessModel.expiresIn = result.Body.Expires_In != null ? Convert.ToInt32(result.Body.Expires_In) : 0;
                        accessModel.expiressEndTime = result.Body.ExpiressEndTime;
                        await _accessTokenServices.UpdateAsync(accessModel);
                    }
                    else
                    {
                        jm.msg = "新accessToken刷新失败";
                        NLogUtil.WriteAll(LogLevel.Error, LogType.ApiRequest, "易联云更新Token", JsonConvert.SerializeObject(result));
                        return await Task.FromResult(jm);
                    }
                }
                else
                {
                    accessToken = accessModel.accessToken;
                }
            }
            //获取打印机是否在线
            var printerStatus = string.Empty;
            var resultOnline = _yilianyunService.PrinterGetStatus(accessModel.accessToken, accessModel.machineCode);
            if (resultOnline.IsSuccess())
            {
                printerStatus = resultOnline.Body.State.ToString();
            }
            if (printerStatus == "在线")
            {
                var payStr = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.PaymentsTypes>(order.paymentCode);
                var items = await _orderItemServices.QueryListByClauseAsync(p => p.orderId == order.orderId);
                var areas = await _areaServices.GetAreaFullName(order.shipAreaId);
                order.shipAreaName = areas.status ? areas.data + "" : "";

                var receiptType = string.Empty;
                if (order.receiptType == (int)GlobalEnumVars.OrderReceiptType.IntraCityService)
                {
                    receiptType = "同城配送";
                }
                else if (order.receiptType == (int)GlobalEnumVars.OrderReceiptType.Logistics)
                {
                    receiptType = "物流快递";
                }
                else if (order.receiptType == (int)GlobalEnumVars.OrderReceiptType.SelfDelivery)
                {
                    receiptType = "门店自提";
                }

                var printStr = new StringBuilder();
                printStr.Append("<center>--" + receiptType + "订单--</center>\r");
                printStr.Append("................................\r");
                printStr.Append("<center>--" + payStr + "--</center>\r");
                printStr.Append("下单时间：" + order.createTime.ToString("yyyy-MM-dd HH:mm:ss") + "\r");
                printStr.Append("订单编号：" + order.orderId + "\r");
                printStr.Append("**************商品**************\r");
                printStr.Append("<center>--购买明细--</center>\r");

                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        printStr.Append(item.name + "*" + item.nums + "\r");
                    }
                }
                //printStr.Append("<center>--其他消费--</center>\r");
                //printStr.Append("餐盒 1 2\r");
                printStr.Append("................................\r");
                printStr.Append("积分抵扣：￥" + order.pointMoney + "\r");
                printStr.Append("订单优惠：￥" + order.orderDiscountAmount + "\r");
                printStr.Append("商品优惠：￥" + order.goodsDiscountAmount + "\r");
                printStr.Append("优惠券：￥" + order.couponDiscountAmount + "\r");
                printStr.Append("总价：￥" + order.orderAmount + "\r");
                printStr.Append("*******************************\r");
                printStr.Append("区域：" + order.shipAreaName + "\r");
                printStr.Append("地址：" + order.shipAddress + "\r");
                printStr.Append("联系：" + order.shipName + " " + order.shipMobile + "\r");
                printStr.Append("***************完结*************\r");
                var onPostPrintText = _yilianyunService.PrintText(accessToken, _machineCode, printStr.ToString());
                jm.data = onPostPrintText;

                NLogUtil.WriteAll(LogLevel.Trace, LogType.ApiRequest, "易联云打印结果", JsonConvert.SerializeObject(jm));
            }
            else
            {
                jm.msg = "机器离线";
                jm.data = printerStatus;
                jm.otherData = resultOnline;
                NLogUtil.WriteAll(LogLevel.Trace, LogType.ApiRequest, "易联云机器离线", JsonConvert.SerializeObject(jm));
            }

            return await Task.FromResult(jm);
        }
    }
}

//{ 'error':'0', 'error_description':'success'},
//{ 'error':'1', 'error_description':'response_type非法'},
//{ 'error':'2', 'error_description':'client_id不存在'},
//{ 'error':'3', 'error_description':'redirect_uri不匹配'},
//{ 'error':'4', 'error_description':'client_id、response_type、state均不允许为空'},
//{ 'error':'5', 'error_description':'client_id或client_secret错误'},
//{ 'error':'6', 'error_description':'code错误或已过期'},
//{ 'error':'7', 'error_description':'账号密码错误'},
//{ 'error':'8', 'error_description':'打印机信息错误,参数有误'},
//{ 'error':'9', 'error_description':'连接打印机失败,参数有误'},
//{ 'error':'10', 'error_description':'权限不足'},
//{ 'error':'11', 'error_description':'sign验证失败'},
//{ 'error':'12', 'error_description':'缺少必要参数'},
//{ 'error':'13', 'error_description':'打印失败,参数有误'},
//{ 'error':'14', 'error_description':'access_token错误'},
//{ 'error':'15', 'error_description':'权限不能大于初次授权的权限'},
//{ 'error':'16', 'error_description':'不支持k1,k2,k3机型'},
//{ 'error':'17', 'error_description':'该打印机已被他人绑定'},
//{ 'error':'18', 'error_description':'access_token过期或错误,请刷新access_token或者重新授权'},
//{ 'error':'19', 'error_description':'应用未上架或已下架'},
//{ 'error':'20', 'error_description':'refresh_token已过期,请重新授权'}
//{ 'error':'21', 'error_description':'关闭或重启失败'},
//{ 'error':'22', 'error_description':'声音设置失败'},
//{ 'error':'23', 'error_description':'获取机型和打印宽度失败'},
//{ 'error':'24', 'error_description':'操作失败，没有订单可以被取消'},
//{ 'error':'25', 'error_description':'未找到机型的硬件和软件版本'},
//{ 'error':'26', 'error_description':'取消logo失败'},
//{ 'error':'27', 'error_description':'请设置scope,权限默认为all'},
//{ 'error':'28', 'error_description':'设置logo失败'},
//{ 'error':'29', 'error_description':'client_id,machine_code,qr_key不能为空'},
//{ 'error':'30', 'error_description':'machine_code,qr_key错误'},
//{ 'error':'31', 'error_description':'接口不支持自有应用服务模式'},
//{ 'error':'32', 'error_description':'订单确认设置失败'},
//{ 'error':'33', 'error_description':'Uuid不合法'},
//{ 'error':'34', 'error_description':'非法操作'},
//{ 'error':'35', 'error_description':'machine_code或msign错误'},
//{ 'error':'36', 'error_description':'按键打印开启或关闭失败'},
//{ 'error':'37', 'error_description':'添加应用菜单失败'},
//{ 'error':'38', 'error_description':'应用菜单内容错误,content必须是Json数组'},
//{ 'error':'39', 'error_description':'应用菜单个数超过最大个数'},
//{ 'error':'40', 'error_description':'应用菜单内容错误,content中的name值重名'},
//{ 'error':'41',  'error_description':'获取或更新access_token的次数,已超过最大限制次数!'},
//{ 'error':'42',  'error_description':'该机型不支持面单打印'},
//{ 'error':'43',  'error_description':'shipper_type错误'},
//{ 'error':'45',  'error_description':'系统错误!请立即反馈'},
//{ 'error':'46', 'error_description': 'picture_url错误或格式错误'},
//{ 'error':'47',  'error_description':'参数错误',"body":"xxxxx"},
//{ 'error':'48', 'error_description': '无法设置,该型号版本不支持!'},
//{ 'error':'49', 'error_description': '错误',"body":"xxxxx"},