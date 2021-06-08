/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:59
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 用户订阅提醒状态
    /// </summary>
    [SugarTable("CoreCmsUserWeChatMsgSubscriptionSwitch",TableDescription = "用户订阅提醒状态")]
    public partial class CoreCmsUserWeChatMsgSubscriptionSwitch
    {
        /// <summary>
        /// 用户订阅提醒状态
        /// </summary>
        public CoreCmsUserWeChatMsgSubscriptionSwitch()
        {
        }

        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(ColumnDescription = "序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        [SugarColumn(ColumnDescription = "用户Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 是否关闭
        /// </summary>
        [Display(Name = "是否关闭")]
        [SugarColumn(ColumnDescription = "是否关闭")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isSwitch { get; set; }
    }
}