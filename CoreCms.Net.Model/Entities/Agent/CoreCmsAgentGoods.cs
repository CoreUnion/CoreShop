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
    /// 代理商品池
    /// </summary>
    public partial class CoreCmsAgentGoods
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsAgentGoods()
        {
        }
		
        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
		
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 id  { get; set; }
        
		
        /// <summary>
        /// 商品序列
        /// </summary>
        [Display(Name = "商品序列")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 goodId  { get; set; }
        
		
        /// <summary>
        /// 商品编辑时间
        /// </summary>
        [Display(Name = "商品编辑时间")]
		
        
        
        
        
        public System.DateTime? goodRefreshTime  { get; set; }
        
		
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 sortId  { get; set; }
        
		
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Boolean isEnable  { get; set; }
        
		
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.DateTime createTime  { get; set; }
        
		
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Display(Name = "最后更新时间")]
		
        
        
        
        
        public System.DateTime? updateTime  { get; set; }
        
		
    }
}
