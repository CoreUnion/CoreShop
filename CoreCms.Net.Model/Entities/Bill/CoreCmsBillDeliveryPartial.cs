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
    /// 发货单表
    /// </summary>
    public partial class CoreCmsBillDelivery
    {

        /// <summary>
        /// 物流名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public System.String logiName { get; set; }


        /// <summary>
        /// 所属区域三级全名
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public System.String shipAreaIdName { get; set; }

    }
}
