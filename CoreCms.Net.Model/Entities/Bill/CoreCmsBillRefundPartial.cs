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

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     退款单表
    /// </summary>
    public partial class CoreCmsBillRefund
    {
        /// <summary>
        ///     支付代码描述
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string paymentCodeName { get; set; }

        /// <summary>
        ///     状态中文描述
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string statusName { get; set; }


        /// <summary>
        ///     用户昵称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string userNickName { get; set; }
    }
}