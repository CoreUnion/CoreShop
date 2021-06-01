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
    /// 广告位置表
    /// </summary>
    public partial class CoreCmsAdvertPosition
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsAdvertPosition()
        {
        }
		
        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
		[SugarColumn(IsPrimaryKey = true, IsIdentity = true)][Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id  { get; set; }
		
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:120,ErrorMessage = "{0}不能超过{1}字")]
        public System.String name  { get; set; }
		
        /// <summary>
        /// 位置编码
        /// </summary>
        [Display(Name = "位置编码")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:32,ErrorMessage = "{0}不能超过{1}字")]
        public System.String code  { get; set; }
		
        /// <summary>
        /// 添加时间
        /// </summary>
        [Display(Name = "添加时间")]
		
        public System.DateTime? createTime  { get; set; }
		
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
		
        public System.DateTime? updateTime  { get; set; }
		
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isEnable  { get; set; }
		
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort  { get; set; }
		
    }
}
