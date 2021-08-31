/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统
 *                Web: https://www.corecms.net
 *             Author: 大灰灰
 *              Email: jianweie@163.com
 *         CreateTime: 2021/6/21 1:16:08
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     接龙活动商品表
    /// </summary>
    public partial class CoreCmsSolitaireItems
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     接龙序列
        /// </summary>
        [Display(Name = "接龙序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int solitaireId { get; set; }

        /// <summary>
        ///     商品序列
        /// </summary>
        [Display(Name = "商品序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int goodId { get; set; }

        /// <summary>
        ///     货品价格
        /// </summary>
        [Display(Name = "货品价格")]
        [Required(ErrorMessage = "请输入{0}")]
        public int productId { get; set; }

        /// <summary>
        ///     接龙价
        /// </summary>
        [Display(Name = "接龙价")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal price { get; set; }

        /// <summary>
        ///     活动库存
        /// </summary>
        [Display(Name = "活动库存")]
        [Required(ErrorMessage = "请输入{0}")]
        public int activityStock { get; set; }

        /// <summary>
        ///     每人可买
        /// </summary>
        [Display(Name = "每人可买")]
        [Required(ErrorMessage = "请输入{0}")]
        public int oneCanBuy { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sortId { get; set; }

        /// <summary>
        ///     标注删除
        /// </summary>
        [Display(Name = "标注删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDelete { get; set; }
    }
}