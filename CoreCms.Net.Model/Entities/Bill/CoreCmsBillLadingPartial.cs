/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/
using System.Collections.Generic;
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 提货单表
    /// </summary>
    public partial class CoreCmsBillLading
    {

        /// <summary>
        /// 关联门店名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public System.String storeName { get; set; }


        /// <summary>
        /// 状态中文描述
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public System.String statusName { get; set; }



        /// <summary>
        /// 店员昵称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public System.String clerkIdName { get; set; }


        /// <summary>
        /// 关联订单项目
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsOrderItem> orderItems { get; set; }

    }
}
