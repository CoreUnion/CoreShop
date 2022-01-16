/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2022/1/15 1:30:57
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 服务购买表
    /// </summary>
    public partial class CoreCmsUserServicesOrder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsUserServicesOrder()
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
        /// 服务订单编号
        /// </summary>
        [Display(Name = "服务订单编号")]
		
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String serviceOrderId  { get; set; }
        
		
        /// <summary>
        /// 关联用户
        /// </summary>
        [Display(Name = "关联用户")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 userId  { get; set; }
        
		
        /// <summary>
        /// 关联服务
        /// </summary>
        [Display(Name = "关联服务")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 servicesId  { get; set; }
        
		
        /// <summary>
        /// 是否支付
        /// </summary>
        [Display(Name = "是否支付")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Boolean isPay  { get; set; }
        
		
        /// <summary>
        /// 支付时间
        /// </summary>
        [Display(Name = "支付时间")]
		
        
        
        
        
        public System.DateTime? payTime  { get; set; }
        
		
        /// <summary>
        /// 支付单号
        /// </summary>
        [Display(Name = "支付单号")]
		
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String paymentId  { get; set; }
        
		
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 status  { get; set; }
        
		
        /// <summary>
        /// 订单创建时间
        /// </summary>
        [Display(Name = "订单创建时间")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.DateTime createTime  { get; set; }
        
		
        /// <summary>
        /// 截止服务时间
        /// </summary>
        [Display(Name = "截止服务时间")]
		
        
        
        
        
        public System.DateTime? servicesEndTime  { get; set; }
        
		
    }
}
