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
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     店员视图表
    /// </summary>
    public class StoreClerkDto
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "")]
        public int id { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public int storeId { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public int userId { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public bool isDel { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public DateTime createTime { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public DateTime? updateTime { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public string storeName { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public string nickName { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public string mobile { get; set; }

        /// <summary>
        /// </summary>
        [Display(Name = "")]
        public string avatarImage { get; set; }
    }
}