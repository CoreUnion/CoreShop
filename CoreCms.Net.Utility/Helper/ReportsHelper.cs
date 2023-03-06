/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-08-31 0:46:50
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;

namespace CoreCms.Net.Utility.Helper
{
    /// <summary>
    /// 报表帮助类
    /// </summary>
    public static class ReportsHelper
    {

        //根据时间，返回时间段
        public static WebApiCallBack GetDate(string data, int section)
        {
            var jm = new WebApiCallBack();
            var reportsBackForGetDate = new ReportsBackForGetDate();
            if (!string.IsNullOrEmpty(data))
            {
                var dts = data.Split("到");
                if (dts.Length == 2)
                {
                    reportsBackForGetDate.start = dts[0].Trim().ObjectToDate();
                    reportsBackForGetDate.start = new DateTime(reportsBackForGetDate.start.Year, reportsBackForGetDate.start.Month, reportsBackForGetDate.start.Day, 0, 0, 0);
                    reportsBackForGetDate.end = dts[1].Trim().ObjectToDate();
                    reportsBackForGetDate.end = new DateTime(reportsBackForGetDate.end.Year, reportsBackForGetDate.end.Month, reportsBackForGetDate.end.Day, 23, 59, 59);
                    reportsBackForGetDate.section = 1;
                    jm.data = reportsBackForGetDate;
                }
                else
                {
                    jm.msg = "时间段格式不正确";
                    return jm;
                }
            }
            else
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }
            //切片维度，1是小时，2是天，3是周，4是月，5是季度，6是半年，7是年
            if (section > 0)
            {
                reportsBackForGetDate.section = section;
            }
            //算统计需要的参数
            return getTmp(reportsBackForGetDate);

        }

        //根据时间节点和时间粒度算x轴个数
        public static WebApiCallBack getTmp(ReportsBackForGetDate dateArr)
        {
            var jm = new WebApiCallBack();

            if (dateArr.end <= dateArr.start)
            {
                jm.msg = "开始时间必须小于结束时间";
                return jm;
            }
            TimeSpan diffDt = dateArr.end - dateArr.start;  //两个时间相减 。默认得到的是两个的时间差
            switch (dateArr.section)
            {
                case 1:                 //小时
                    dateArr.section = 60 * 60;
                    dateArr.num = Convert.ToInt32(diffDt.TotalHours);
                    break;
                case 2:                 //天
                    dateArr.section = 60 * 60 * 24;
                    dateArr.num = Convert.ToInt32(diffDt.TotalDays);
                    break;
                default:
                    jm.msg = "没有此时间粒度";
                    return jm;
            }
            //算x轴数据个数
            jm.status = true;
            jm.data = dateArr;
            return jm;
        }

        public static WebApiCallBack GetXdata(ReportsBackForGetDate dateArr)
        {
            var jm = new WebApiCallBack();

            //校验,x轴最多1000个
            if (dateArr.num > 1000)
            {
                jm.msg = GlobalErrorCodeVars.Code13226;
                return jm;
            }

            var xType = "";
            switch (dateArr.section)
            {
                case 3600:                 //小时
                    if (dateArr.num <= 24)
                    {
                        xType = "d日H时";
                    }
                    else if (dateArr.num <= 720)
                    {
                        xType = "M月d日H时";
                    }
                    else
                    {
                        xType = "M月d日H时";
                    }
                    break;
                case 86400:                 //天
                    if (dateArr.num <= 31)
                    {
                        xType = "M月d号";
                    }
                    else if (dateArr.num <= 365)
                    {
                        xType = "yyyy年M月dd日";
                    }
                    else
                    {
                        xType = "yyyy年M月dd日";
                    }
                    break;
            }
            if (xType == "")
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }

            var arr = new List<string>();
            if (dateArr.section == 3600)
            {
                var dtStart = dateArr.start;
                for (int i = 0; i < dateArr.num; i++)
                {
                    arr.Add(dtStart.AddHours(i).ToString(xType));
                }
            }
            else
            {
                var dtStart = dateArr.start;
                for (int i = 0; i < dateArr.num; i++)
                {
                    arr.Add(dtStart.AddDays(i).ToString(xType));
                }
            }

            jm.status = true;
            jm.data = arr;
            jm.otherData = dateArr;
            return jm;
        }

    }
}
