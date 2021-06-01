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
    /// 支付单明细表
    /// </summary>
    public partial class CoreCmsBillPaymentsRel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsBillPaymentsRel()
        {
        }
		
        /// <summary>
        /// 支付单编号
        /// </summary>
        [Display(Name = "支付单编号")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        public System.String paymentId  { get; set; }
		
        /// <summary>
        /// 资源编号
        /// </summary>
        [Display(Name = "资源编号")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        public System.String sourceId  { get; set; }
		
        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Decimal money  { get; set; }
		
    }
}
