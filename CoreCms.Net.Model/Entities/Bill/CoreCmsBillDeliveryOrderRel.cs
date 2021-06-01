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
    /// 发货单订单关联表
    /// </summary>
    public partial class CoreCmsBillDeliveryOrderRel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsBillDeliveryOrderRel()
        {
        }
		
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
		[SugarColumn(IsPrimaryKey = true, IsIdentity = true)][Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id  { get; set; }
		
        /// <summary>
        /// 订单号
        /// </summary>
        [Display(Name = "订单号")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        public System.String orderId  { get; set; }
		
        /// <summary>
        /// 发货单号
        /// </summary>
        [Display(Name = "发货单号")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        public System.String deliveryId  { get; set; }
		
    }
}
