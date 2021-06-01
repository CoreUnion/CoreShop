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
    /// 退货单表
    /// </summary>
    public partial class CoreCmsBillAftersales
    {
        /// <summary>
        /// 商品子集
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsBillAftersalesItem> items { get; set; } = new List<CoreCmsBillAftersalesItem>();

        /// <summary>
        /// 图片子集
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsBillAftersalesImages> images { get; set; } = new List<CoreCmsBillAftersalesImages>();
        /// <summary>
        /// 退款单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public CoreCmsBillRefund billRefund { get; set; }
        /// <summary>
        /// 退货单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public CoreCmsBillReship billReship { get; set; }

        /// <summary>
        /// 状态说明
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string statusName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [SugarColumn(IsIgnore = true)]

        public string userNickName { get; set; }

        /// <summary>
        /// 关联订单数据
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public CoreCmsOrder order { get; set; }

    }
}
