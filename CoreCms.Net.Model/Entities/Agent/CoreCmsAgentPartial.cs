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
    /// 代理商表
    /// </summary>
    public partial class CoreCmsAgent
    {

        /// <summary>
        /// 总金额
        /// </summary>
        [Display(Name = "总金额")]
        [SugarColumn(IsIgnore = true)]
        public decimal TotalSettlementAmount { get; set; }


        /// <summary>
        /// 已结算金额
        /// </summary>
        [Display(Name = "已结算金额")]
        [SugarColumn(IsIgnore = true)]
        public decimal SettlementAmount { get; set; }


        /// <summary>
        /// 冻结金额
        /// </summary>
        [Display(Name = "冻结金额")]
        [SugarColumn(IsIgnore = true)]
        public decimal FreezeAmount { get; set; }


        /// <summary>
        /// 本月订单数
        /// </summary>
        [Display(Name = "本月订单数")]
        [SugarColumn(IsIgnore = true)]
        public int CurrentMonthOrder { get; set; }



        /// <summary>
        /// 今日收益
        /// </summary>
        [Display(Name = "今日收益")]
        [SugarColumn(IsIgnore = true)]
        public decimal TodayFreezeAmount { get; set; }


        /// <summary>
        /// 今日订单
        /// </summary>
        [Display(Name = "今日订单")]
        [SugarColumn(IsIgnore = true)]
        public int TodayOrder { get; set; }


        /// <summary>
        /// 今日会员
        /// </summary>
        [Display(Name = "今日会员")]
        [SugarColumn(IsIgnore = true)]
        public int TodayUser { get; set; }


        /// <summary>
        ///上架商品数量
        /// </summary>
        [Display(Name = "上架商品数量")]
        [SugarColumn(IsIgnore = true)]
        public int TotalGoods { get; set; }


        /// <summary>
        ///所属等级名称
        /// </summary>
        [Display(Name = "所属等级名称")]
        [SugarColumn(IsIgnore = true)]
        public string GradeName { get; set; } = "";

        /// <summary>
        ///是否需要申请
        /// </summary>
        [Display(Name = "是否需要申请")]
        [SugarColumn(IsIgnore = true)]
        public bool NeedApply { get; set; } = true;

        /// <summary>
        ///条件说明
        /// </summary>
        [Display(Name = "条件说明")]
        [SugarColumn(IsIgnore = true)]
        public string ConditionMsg { get; set; } = "";

        /// <summary>
        ///升级条件状态
        /// </summary>
        [Display(Name = "升级条件状态")]
        [SugarColumn(IsIgnore = true)]
        public bool ConditionStatus { get; set; } = true;

        /// <summary>
        ///升级条件进度
        /// </summary>
        [Display(Name = "升级条件进度")]
        [SugarColumn(IsIgnore = true)]
        public int ConditionProgress { get; set; } = 0;


        /// <summary>
        ///店铺查询交互数据
        /// </summary>
        [Display(Name = "店铺查询交互数据")]
        [SugarColumn(IsIgnore = true)]
        public string Store { get; set; } = "";

    }
}
