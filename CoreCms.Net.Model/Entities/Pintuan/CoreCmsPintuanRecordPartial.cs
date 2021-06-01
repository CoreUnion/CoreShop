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
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     拼团记录
    /// </summary>
    public partial class CoreCmsPinTuanRecord
    {
        /// <summary>
        ///     用户头像
        /// </summary>
        [Display(Name = "用户头像")]
        [SugarColumn(IsIgnore = true)]
        public string userAvatar { get; set; }


        /// <summary>
        ///     昵称
        /// </summary>
        [Display(Name = "昵称")]
        [SugarColumn(IsIgnore = true)]
        public string nickName { get; set; }


        /// <summary>
        ///     参与队员信息
        /// </summary>
        [Display(Name = "参与队员信息")]
        [SugarColumn(IsIgnore = true)]
        public List<PinTuanRecordTeam> teams { get; set; }


        /// <summary>
        ///     参与数量
        /// </summary>
        [Display(Name = "参与数量")]
        [SugarColumn(IsIgnore = true)]
        public int teamNums { get; set; }


        /// <summary>
        ///     参与人数计算
        /// </summary>
        [Display(Name = "参与人数计算")]
        [SugarColumn(IsIgnore = true)]
        public int peopleNumber { get; set; } = 0;


        /// <summary>
        ///     剩余时间
        /// </summary>
        [Display(Name = "剩余时间")]
        [SugarColumn(IsIgnore = true)]
        public int lastTime { get; set; }


        /// <summary>
        ///     剩余时间
        /// </summary>
        [Display(Name = "剩余时间")]
        [SugarColumn(IsIgnore = true)]
        public bool isOverdue { get; set; } = false;


        /// <summary>
        ///     活动名称
        /// </summary>
        [Display(Name = "活动名称")]
        [SugarColumn(IsIgnore = true)]
        public string ruleName { get; set; }


        /// <summary>
        ///     商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [SugarColumn(IsIgnore = true)]
        public string goodName { get; set; }
    }


    public class PinTuanRecordTeam
    {
        /// <summary>
        ///     用户头像
        /// </summary>
        [Display(Name = "用户头像")]
        [SugarColumn(IsIgnore = true)]
        public string userAvatar { get; set; }


        /// <summary>
        ///     昵称
        /// </summary>
        [Display(Name = "昵称")]
        [SugarColumn(IsIgnore = true)]
        public string nickName { get; set; }


        /// <summary>
        ///     记录编号
        /// </summary>
        [Display(Name = "记录编号")]
        [SugarColumn(IsIgnore = true)]
        public int recordId { get; set; } = 0;

        /// <summary>
        ///     拼团队伍编号
        /// </summary>
        [Display(Name = "拼团队伍编号")]
        [SugarColumn(IsIgnore = true)]
        public int teamId { get; set; } = 0;

        /// <summary>
        ///     用户编号
        /// </summary>
        [Display(Name = "用户编号")]
        [SugarColumn(IsIgnore = true)]
        public int userId { get; set; } = 0;
    }
}