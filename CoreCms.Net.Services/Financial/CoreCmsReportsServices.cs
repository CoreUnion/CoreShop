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
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.Echarts;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 报表 接口实现
    /// </summary>
    public class CoreCmsReportsServices : BaseServices<GetOrdersReportsDbSelectOut>, ICoreCmsReportsServices
    {
        private readonly ICoreCmsReportsRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsReportsServices(IUnitOfWork unitOfWork, ICoreCmsReportsRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 订单报表
        /// </summary>
        /// <param name="num">数量</param>
        /// <param name="where">查询条件</param>
        /// <param name="section">查询值</param>
        /// <param name="sTime">开始时间</param>
        /// <param name="joinVal">筛选字段createTime/paymentTime </param>
        /// <returns></returns>
        public List<GetOrdersReportsDbSelectOut> GetOrderMark(int num, string where, int section, DateTime sTime, string joinVal)
        {

            var sqlStr = string.Empty;
            string dbTypeString = AppSettingsConstVars.DbDbType;
            if (dbTypeString == DbType.SqlServer.ToString())
            {
                sqlStr = @"SELECT  tmp_x.number ,
                                ISNULL(SUM(o.orderAmount), 0) AS val ,
                                COUNT(o.orderId) AS num
                        FROM    ( ( SELECT  number
                                    FROM    MASTER..spt_values
                                    WHERE   TYPE = 'P'
                                            AND number >= 0
                                            AND number <= " + num + @"
                                  ) tmp_x
                                  LEFT OUTER JOIN ( SELECT  *
                                                    FROM    CoreCmsOrder
                                                    WHERE isdel=0  " + where + @"
                                                  ) o ON tmp_x.number = DATEDIFF(" + (section == 3600 ? "HOUR" : "DAY") + ", '" + sTime.ToString("yyyy-MM-dd HH:mm:ss") + @"', o." + joinVal + @")
                                )
                        GROUP BY tmp_x.number";
            }
            else if (dbTypeString == DbType.MySql.ToString())
            {
                sqlStr = @"select tmp_x.x,ifnull(sum(o.orderAmount),0) as val,count(o.orderId) as num
            from
              (SELECT @xi:=@xi+1 as x from
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x1,
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x2,
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x3,
                (SELECT @xi:=-1) x0 limit 0," + num + @") tmp_x
                left join (
                    select * from CoreCmsOrder
                where 1 = 1
                " + where + @"
                        ) o on tmp_x.x = ((unix_timestamp(o." + joinVal + @") - " + CommonHelper.ConvertDateTimeToInt(sTime) + @") div(" + section + @"))
                group by tmp_x.x
                ";
            }


            var sp = new List<SugarParameter>();

            var list = _dal.SqlQuery(sqlStr, sp);

            return list;
        }


        /// <summary>
        /// 支付单报表
        /// </summary>
        /// <param name="num">数量</param>
        /// <param name="where">查询条件</param>
        /// <param name="section">查询值</param>
        /// <param name="sTime">开始时间</param>
        /// <param name="joinVal">筛选字段createTime/paymentTime </param>
        /// <returns></returns>
        public List<GetOrdersReportsDbSelectOut> GetPaymentsMark(int num, string where, int section, DateTime sTime, string joinVal)
        {
            var sqlStr = string.Empty;
            string dbTypeString = AppSettingsConstVars.DbDbType;
            if (dbTypeString == DbType.SqlServer.ToString())
            {
                sqlStr = @"SELECT  tmp_x.number ,
                                ISNULL(SUM(o.money), 0) AS val ,
                                COUNT(o.paymentId) AS num
                        FROM    ( ( SELECT  number
                                    FROM    MASTER..spt_values
                                    WHERE   TYPE = 'P'
                                            AND number >= 0
                                            AND number <= " + num + @"
                                  ) tmp_x
                                  LEFT OUTER JOIN ( SELECT  *
                                                    FROM    CoreCmsBillPayments
                                                    WHERE 1=1  " + where + @"
                                                  ) o ON tmp_x.number = DATEDIFF(" + (section == 3600 ? "HOUR" : "DAY") + ", '" + sTime.ToString("yyyy-MM-dd HH:mm:ss") + @"', o." + joinVal + @")
                                )
                        GROUP BY tmp_x.number";
            }
            else if (dbTypeString == DbType.MySql.ToString())
            {
                sqlStr = @"select tmp_x.x,ifnull(sum(o.money),0) as val,count(o.paymentId) as num
            from
              (SELECT @xi:=@xi+1 as x from
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x1,
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x2,
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x3,
                (SELECT @xi:=-1) x0 limit 0," + num + @") tmp_x 
            left join (
                    select * from CoreCmsBillPayments 
            where 1 = 1
                " + where + @"
                        ) o on tmp_x.x = ((unix_timestamp(o." + joinVal + @") - " + CommonHelper.ConvertDateTimeToInt(sTime) + @") div(" + section + @"))
                group by tmp_x.x
                ";
            }

            var sp = new List<SugarParameter>();


            var list = _dal.SqlQuery(sqlStr, sp);

            return list;
        }



        /// <summary>
        /// 退款单报表
        /// </summary>
        /// <param name="num">数量</param>
        /// <param name="where">查询条件</param>
        /// <param name="section">查询值</param>
        /// <param name="sTime">开始时间</param>
        /// <param name="joinVal">筛选字段createTime/paymentTime </param>
        /// <returns></returns>
        public List<GetOrdersReportsDbSelectOut> GetRefundMark(int num, string where, int section, DateTime sTime, string joinVal)
        {

            var sqlStr = string.Empty;
            string dbTypeString = AppSettingsConstVars.DbDbType;
            if (dbTypeString == DbType.SqlServer.ToString())
            {
                sqlStr = @"SELECT  tmp_x.number ,
                                ISNULL(SUM(o.money), 0) AS val ,
                                COUNT(o.refundId) AS num
                        FROM    ( ( SELECT  number
                                    FROM    MASTER..spt_values
                                    WHERE   TYPE = 'P'
                                            AND number >= 0
                                            AND number <= " + num + @"
                                  ) tmp_x
                                  LEFT OUTER JOIN ( SELECT  *
                                                    FROM    CoreCmsBillRefund
                                                    WHERE 1=1  " + where + @"
                                                  ) o ON tmp_x.number = DATEDIFF(" + (section == 3600 ? "HOUR" : "DAY") + ", '" + sTime.ToString("yyyy-MM-dd HH:mm:ss") + @"', o." + joinVal + @")
                                )
                        GROUP BY tmp_x.number";
            }
            else if (dbTypeString == DbType.MySql.ToString())
            {
                sqlStr = @"select tmp_x.x,ifnull(sum(o.money),0) as val,count(o.refundId) as num
            from
              (SELECT @xi:=@xi+1 as x from
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x1,
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x2,
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x3,
                (SELECT @xi:=-1) x0 limit 0," + num + @") tmp_x
                left join(
                    select* from CoreCmsBillRefund
                where 1 = 1
                " + where + @"
                        ) o on tmp_x.x = ((unix_timestamp(o." + joinVal + @") - " + CommonHelper.ConvertDateTimeToInt(sTime) + @") div(" + section + @"))
                group by tmp_x.x
                ";
            }

            var sp = new List<SugarParameter>();

            var list = _dal.SqlQuery(sqlStr, sp);

            return list;
        }

        /// <summary>
        /// 用户提现报表
        /// </summary>
        /// <param name="num">数量</param>
        /// <param name="where">查询条件</param>
        /// <param name="section">查询值</param>
        /// <param name="sTime">开始时间</param>
        /// <param name="joinVal">筛选字段createTime/paymentTime </param>
        /// <returns></returns>
        public List<GetOrdersReportsDbSelectOut> GetTocashMark(int num, string where, int section, DateTime sTime, string joinVal)
        {

            var sqlStr = string.Empty;
            string dbTypeString = AppSettingsConstVars.DbDbType;
            if (dbTypeString == DbType.SqlServer.ToString())
            {
                sqlStr = @"SELECT  tmp_x.number ,
                                ISNULL(SUM(o.money), 0) AS val ,
                                COUNT(o.id) AS num
                        FROM    ( ( SELECT  number
                                    FROM    MASTER..spt_values
                                    WHERE   TYPE = 'P'
                                            AND number >= 0
                                            AND number <= " + num + @"
                                  ) tmp_x
                                  LEFT OUTER JOIN ( SELECT  *
                                                    FROM    CoreCmsUserTocash
                                                    WHERE 1=1  " + where + @"
                                                  ) o ON tmp_x.number = DATEDIFF(" + (section == 3600 ? "HOUR" : "DAY") + ", '" + sTime.ToString("yyyy-MM-dd HH:mm:ss") + @"', o." + joinVal + @")
                                )
                        GROUP BY tmp_x.number";
            }
            else if (dbTypeString == DbType.MySql.ToString())
            {
                sqlStr = @"select tmp_x.x,ifnull(sum(o.money),0) as val,count(o.id) as num
            from
              (SELECT @xi:=@xi+1 as x from
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x1,
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x2,
                (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) x3,
                (SELECT @xi:=-1) x0 limit 0," + num + @") tmp_x 
            left join (
                    select * from CoreCmsUserTocash 
            where 1 = 1
                " + where + @"
                        ) o on tmp_x.x = ((unix_timestamp(o." + joinVal + @") - " + CommonHelper.ConvertDateTimeToInt(sTime) + @") div(" + section + @"))
                group by tmp_x.x
                ";
            }

            var sp = new List<SugarParameter>();

            var list = _dal.SqlQuery(sqlStr, sp);

            return list;
        }

        /// <summary>
        /// 获取订单销量查询返回结果
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="filter"></param>
        /// <param name="filterSed"></param>
        /// <param name="thesort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IPageList<GoodsSalesVolume>> GetGoodsSalesVolumes(string start, string end, string filter, string filterSed, string thesort, int pageIndex = 1, int pageSize = 5000)
        {
            return await _dal.GetGoodsSalesVolumes(start, end, filter, filterSed, thesort, pageIndex, pageSize);
        }


        /// <summary>
        /// 获取商品收藏查询返回结果
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="thesort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IPageList<GoodsCollection>> GetGoodsCollections(string start, string end, string thesort, int pageIndex = 1, int pageSize = 5000)
        {
            return await _dal.GetGoodsCollections(start, end, thesort, pageIndex, pageSize);

        }


    }
}
