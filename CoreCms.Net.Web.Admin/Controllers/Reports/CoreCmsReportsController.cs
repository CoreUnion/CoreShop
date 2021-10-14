/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.Configuration;
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Echarts;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Utility.Extensions;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers.Content
{
    /// <summary>
    /// 报表统计
    ///</summary>
    [Description("报表统计")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsReportsController : ControllerBase
    {
        private readonly ICoreCmsReportsServices _reportsServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reportsServices"></param>
        public CoreCmsReportsController(ICoreCmsReportsServices reportsServices)
        {
            _reportsServices = reportsServices;
        }

        #region 订单销量统计
        // POST: Api/CoreCmsReports/GetOrder
        /// <summary>
        /// 订单销量统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("订单销量统计")]
        public AdminUiCallBack GetOrder([FromBody] FMReports entity)
        {
            var jm = new AdminUiCallBack();

            var dataRes = ReportsHelper.GetDate(entity.date, entity.section);
            if (!dataRes.status)
            {
                jm.msg = dataRes.msg;
                return jm;
            }

            var echartsOption = new EchartsOption();

            echartsOption.title.text = "订单统计";
            var legend = new List<string>() { "全部", "待付款", "已付款" };
            echartsOption.legend.data = legend;

            var getDate = dataRes.data as ReportsBackForGetDate;

            var xData = ReportsHelper.GetXdata(getDate);
            if (!xData.status)
            {
                jm.msg = dataRes.msg;
                return jm;
            }
            echartsOption.xAxis.data = xData.data as List<string>;

            var whereSql = string.Empty;
            var data = new List<GetOrdersReportsDbSelectOut>();
            var data1 = new List<GetOrdersReportsDbSelectOut>();
            var data2 = new List<GetOrdersReportsDbSelectOut>();
            var data3 = new List<GetOrdersReportsDbSelectOut>();
            foreach (var item in legend)
            {
                switch (item)
                {
                    case "全部":
                        whereSql = string.Empty;
                        whereSql += " and createTime > '" + getDate.start.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and createTime < '" + getDate.end.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        data = _reportsServices.GetOrderMark(getDate.num, whereSql, getDate.section, getDate.start, "createTime");
                        data1 = data;
                        break;
                    case "待付款":
                        whereSql = string.Empty;
                        whereSql += " and createTime > '" + getDate.start.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and createTime < '" + getDate.end.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and payStatus=1 ";
                        data = _reportsServices.GetOrderMark(getDate.num, whereSql, getDate.section, getDate.start, "createTime");
                        data2 = data;
                        break;
                    case "已付款":
                        whereSql = string.Empty;
                        whereSql += " and paymentTime > '" + getDate.start.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and paymentTime < '" + getDate.end.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and payStatus>1 ";
                        data = _reportsServices.GetOrderMark(getDate.num, whereSql, getDate.section, getDate.start, "paymentTime");
                        data3 = data;
                        break;
                }

                if (data != null && data.Any())
                {
                    var vals = data.Select(p => p.val).ToList();
                    echartsOption.series.Add(new SeriesItem()
                    {
                        name = item,
                        type = "line",
                        data = vals.ConvertAll<string>(x => x.ToString(CultureInfo.InvariantCulture))
                    });
                }
                else
                {
                    echartsOption.series.Add(new SeriesItem()
                    {
                        name = item,
                        type = "line",
                        data = new List<string>()
                    });
                }
            }
            //组装数据列表用于table里使用
            var tableData = new List<OrderTableItem>();
            for (int i = 0; i < getDate.num; i++)
            {
                var item = new OrderTableItem();
                if (echartsOption.xAxis.data != null) item.x = echartsOption.xAxis.data[i];
                item.order_all_val = data1[i].val.ToString(CultureInfo.InvariantCulture);
                item.order_all_num = data1[i].num;
                item.order_nopay_val = data2[i].val.ToString(CultureInfo.InvariantCulture);
                item.order_nopay_num = data2[i].num;
                item.order_payed_val = data3[i].val.ToString(CultureInfo.InvariantCulture);
                item.order_payed_num = data3[i].num;
                tableData.Add(item);
            }

            jm.code = 0;
            jm.data = new
            {
                option = echartsOption,
                table = tableData
            };

            return jm;

        }
        #endregion

        #region 财务收款单
        // POST: Api/CoreCmsReports/GetPayments
        /// <summary>
        /// 财务收款单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("财务收款单")]
        public AdminUiCallBack GetPayments([FromBody] FMReports entity)
        {
            var jm = new AdminUiCallBack();

            var dataRes = ReportsHelper.GetDate(entity.date, entity.section);
            if (!dataRes.status)
            {
                jm.msg = dataRes.msg;
                return jm;
            }

            var echartsOption = new EchartsOption();

            echartsOption.title.text = "财务统计";
            var legend = new List<string>() { "收款单", "订单收款", "订单退款", "充值", "提现" };
            echartsOption.legend.data = legend;

            var getDate = dataRes.data as ReportsBackForGetDate;

            var xData = ReportsHelper.GetXdata(getDate);
            if (!xData.status)
            {
                jm.msg = dataRes.msg;
                return jm;
            }
            echartsOption.xAxis.data = xData.data as List<string>;

            var whereSql = string.Empty;
            var data = new List<GetOrdersReportsDbSelectOut>();
            var data1 = new List<GetOrdersReportsDbSelectOut>();
            var data2 = new List<GetOrdersReportsDbSelectOut>();
            var data3 = new List<GetOrdersReportsDbSelectOut>();
            var data4 = new List<GetOrdersReportsDbSelectOut>();
            var data5 = new List<GetOrdersReportsDbSelectOut>();
            foreach (var item in legend)
            {
                switch (item)
                {
                    case "收款单":
                        whereSql = string.Empty;
                        whereSql += " and updateTime > '" + getDate.start.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and updateTime < '" + getDate.end.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and status = 2 ";
                        data = _reportsServices.GetPaymentsMark(getDate.num, whereSql, getDate.section, getDate.start, "updateTime");
                        data1 = data;
                        break;
                    case "订单收款":
                        whereSql = string.Empty;
                        whereSql += " and updateTime > '" + getDate.start.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and updateTime < '" + getDate.end.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and status = 2 ";
                        whereSql += " and type = 1 ";
                        data = _reportsServices.GetPaymentsMark(getDate.num, whereSql, getDate.section, getDate.start, "updateTime");
                        data2 = data;
                        break;
                    case "订单退款":
                        whereSql = string.Empty;
                        whereSql += " and updateTime > '" + getDate.start.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and updateTime < '" + getDate.end.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and status = 2 ";
                        whereSql += " and type = 1 ";
                        data = _reportsServices.GetRefundMark(getDate.num, whereSql, getDate.section, getDate.start, "updateTime");
                        data3 = data;
                        break;
                    case "充值":
                        whereSql = string.Empty;
                        whereSql += " and updateTime > '" + getDate.start.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and updateTime < '" + getDate.end.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and status = 2 ";
                        whereSql += " and type = 2 ";
                        data = _reportsServices.GetPaymentsMark(getDate.num, whereSql, getDate.section, getDate.start, "updateTime");
                        data4 = data;
                        break;
                    case "提现":
                        whereSql = string.Empty;
                        whereSql += " and updateTime > '" + getDate.start.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and updateTime < '" + getDate.end.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and status = 2 ";
                        data = _reportsServices.GetTocashMark(getDate.num, whereSql, getDate.section, getDate.start, "updateTime");
                        data5 = data;
                        break;
                }

                if (data != null && data.Any())
                {
                    var vals = data.Select(p => p.val).ToList();
                    echartsOption.series.Add(new SeriesItem()
                    {
                        name = item,
                        type = "line",
                        data = vals.ConvertAll<string>(x => x.ToString(CultureInfo.InvariantCulture))
                    });
                }
                else
                {
                    echartsOption.series.Add(new SeriesItem()
                    {
                        name = item,
                        type = "line",
                        data = new List<string>()
                    });
                }
            }
            //组装数据列表用于table里使用
            //var legend = new List<string>() { "收款单", "订单收款", "订单退款", "充值", "提现" };

            var tableData = new List<PaymentsTableItem>();
            for (int i = 0; i < getDate.num; i++)
            {
                var item = new PaymentsTableItem();
                if (echartsOption.xAxis.data != null) item.x = echartsOption.xAxis.data[i];
                item.payments_all_val = data1[i].val.ToString(CultureInfo.InvariantCulture);
                item.payments_all_num = data1[i].num;
                item.payments_order_val = data2[i].val.ToString(CultureInfo.InvariantCulture);
                item.payments_order_num = data2[i].num;
                item.payments_order_refund_val = data3[i].val.ToString(CultureInfo.InvariantCulture);
                item.payments_order_refund_num = data3[i].num;

                item.payments_recharge_val = data4[i].val.ToString(CultureInfo.InvariantCulture);
                item.payments_recharge_num = data4[i].num;

                item.payments_tocash_val = data5[i].val.ToString(CultureInfo.InvariantCulture);
                item.payments_tocash_num = data5[i].num;

                tableData.Add(item);
            }

            jm.code = 0;
            jm.data = new
            {
                option = echartsOption,
                table = tableData
            };

            return jm;

        }
        #endregion

        #region 商品销量统计
        // POST: Api/CoreCmsReports/GetGoods
        /// <summary>
        /// 商品销量统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("商品销量统计")]
        public async Task<AdminUiCallBack> GetGoods()
        {
            var jm = new AdminUiCallBack();

            var page = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var limit = Request.Form["limit"].FirstOrDefault().ObjectToInt(5000);

            var thesort = Request.Form["thesort"].FirstOrDefault();
            if (string.IsNullOrEmpty(thesort))
            {
                thesort = "desc";
            }

            var filter = Request.Form["filter"].FirstOrDefault();
            if (string.IsNullOrEmpty(filter))
            {
                filter = "nums";
            }
            var filter_sed = filter == "nums" ? "amount" : "nums";

            var dt = DateTime.Now;
            var start = "";
            var end = "";
            var date = Request.Form["date"].FirstOrDefault();
            if (!string.IsNullOrEmpty(date))
            {
                var dts = date.Split("到");
                if (dts.Length == 2)
                {
                    var startDt = dts[0].Trim().ObjectToDate();
                    start = startDt.ToString("yyyy-MM-dd 00:00:00");
                    var endDt = dts[1].Trim().ObjectToDate();
                    end = endDt.ToString("yyyy-MM-dd 23:59:59");
                }
                else
                {
                    jm.msg = "时间段格式不正确";
                    return jm;
                }
            }
            else
            {
                //默认今天
                start = dt.ToString("yyyy-MM-dd 00:00:00");
                end = dt.ToString("yyyy-MM-dd 23:59:59");
            }
            var list = await _reportsServices.GetGoodsSalesVolumes(start, end, filter, filter_sed, thesort, page, limit);

            jm.code = 0;
            jm.data = list;
            jm.count = list.TotalCount;

            return jm;

        }
        #endregion

        #region 商品收藏统计
        // POST: Api/CoreCmsReports/GetGoodsCollection
        /// <summary>
        /// 商品收藏统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("商品收藏统计")]
        public async Task<AdminUiCallBack> GetGoodsCollection()
        {
            var jm = new AdminUiCallBack();

            var page = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var limit = Request.Form["limit"].FirstOrDefault().ObjectToInt(5000);

            var thesort = Request.Form["thesort"].FirstOrDefault();
            if (string.IsNullOrEmpty(thesort))
            {
                thesort = "desc";
            }
            var dt = DateTime.Now;
            var start = "";
            var end = "";
            var date = Request.Form["date"].FirstOrDefault();
            if (!string.IsNullOrEmpty(date))
            {
                var dts = date.Split("到");
                if (dts.Length == 2)
                {
                    var startDt = dts[0].Trim().ObjectToDate();
                    start = startDt.ToString("yyyy-MM-dd 00:00:00");
                    var endDt = dts[1].Trim().ObjectToDate();
                    end = endDt.ToString("yyyy-MM-dd 23:59:59");
                }
                else
                {
                    jm.msg = "时间段格式不正确";
                    return jm;
                }
            }
            else
            {
                //默认今天
                start = dt.ToString("yyyy-MM-dd 00:00:00");
                end = dt.ToString("yyyy-MM-dd 23:59:59");
            }
            var list = await _reportsServices.GetGoodsCollections(start, end, thesort, page, limit);

            jm.code = 0;
            jm.data = list;
            jm.count = list.TotalCount;

            return jm;

        }
        #endregion

        #region 换算前台时间按钮

        //换算前台时间按钮
        // POST: Api/CoreCmsReports/GetDateType
        /// <summary>
        /// 换算前台时间按钮
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("换算前台时间按钮")]
        public AdminUiCallBack GetDateType([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.id <= 0)
            {
                jm.msg = "请选择合法的时间段";
                return jm;
            }

            DateTime dt = DateTime.Now;  //当前时间  
            var start = "";
            var end = "";
            switch (entity.id)
            {
                case 1: //当天
                    start = dt.ToString("yyyy-MM-dd");
                    end = start;
                    break;
                case 2: //昨天
                    start = dt.AddDays(-1).ToString("yyyy-MM-dd");
                    end = start;
                    break;
                case 3: //本周
                    var startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  
                    var endWeek = startWeek.AddDays(6);  //本周周日  
                    start = startWeek.ToString("yyyy-MM-dd");
                    end = endWeek.ToString("yyyy-MM-dd");
                    break;
                case 4: //上周
                    var lastWeekStart = dt.AddDays(Convert.ToInt32(1 - Convert.ToInt32(dt.DayOfWeek)) - 7);
                    var lastWeekEnd = dt.AddDays(Convert.ToInt32(1 - Convert.ToInt32(dt.DayOfWeek)) - 7).AddDays(6);     //上周末（星期日） 
                    start = lastWeekStart.ToString("yyyy-MM-dd");
                    end = lastWeekEnd.ToString("yyyy-MM-dd");
                    break;
                case 5:  //本月
                    var startMonth = dt.AddDays(1 - dt.Day);  //本月月初  
                    var endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末  
                    start = startMonth.ToString("yyyy-MM-dd");
                    end = endMonth.ToString("yyyy-MM-dd");
                    break;
                case 6:  //上月
                    start = DateTime.Parse(dt.ToString("yyyy-MM-01")).AddMonths(-1).ToString("yyyy-MM-dd");
                    end = DateTime.Parse(dt.ToString("yyyy-MM-01")).AddDays(-1).ToString("yyyy-MM-dd");
                    break;
                case 7: //最近7天
                    start = dt.AddDays(-7).ToString("yyyy-MM-dd");
                    end = dt.Date.ToString("yyyy-MM-dd");
                    break;
                case 8: //最近一月
                    start = dt.AddMonths(-1).AddDays(1).ToString("yyyy-MM-dd");
                    end = dt.Date.ToString("yyyy-MM-dd");
                    break;
                case 9: //最近3月
                    start = dt.AddMonths(-3).AddDays(1).ToString("yyyy-MM-dd");
                    end = dt.Date.ToString("yyyy-MM-dd");
                    break;
                case 10: //最近6个月
                    start = dt.AddMonths(-6).AddDays(1).ToString("yyyy-MM-dd");
                    end = dt.Date.ToString("yyyy-MM-dd");
                    break;
                case 11: //最近一年
                    start = dt.AddMonths(-12).AddDays(1).ToString("yyyy-MM-dd");
                    end = dt.Date.ToString("yyyy-MM-dd");
                    break;
                case 12: //本年度
                    start = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).ToString("yyyy-MM-dd");
                    end = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
                    break;
                case 13: //上年度
                    start = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1).ToString("yyyy-MM-dd");
                    end = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddDays(-1).ToString("yyyy-MM-dd");
                    break;
                default:
                    jm.msg = "没有此时间维度";
                    break;
            }

            jm.code = 0;
            jm.data = new
            {
                start,
                end
            };
            return jm;

        }
        #endregion

    }
}
