/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 支付单明细表
    /// </summary>
    public partial class CoreCmsBillPaymentsRel
    {
        /// <summary>
        /// 支付单表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public CoreCmsBillPayments bill { get; set; } = new CoreCmsBillPayments();
    }
}
