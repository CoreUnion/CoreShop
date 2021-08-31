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
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.DTO;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     货品
    /// </summary>
    public partial class CoreCmsProducts
    {
        /// <summary>
        ///     商品编码
        /// </summary>
        [Display(Name = "商品编码")]
        [SugarColumn(IsIgnore = true)]
        public string bn { get; set; }

        /// <summary>
        ///     商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [SugarColumn(IsIgnore = true)]
        public string name { get; set; }

        /// <summary>
        ///     是否上架
        /// </summary>
        [Display(Name = "是否上架")]
        [SugarColumn(IsIgnore = true)]
        public bool isMarketable { get; set; }


        /// <summary>
        ///     商品单位
        /// </summary>
        [Display(Name = "商品单位")]
        [SugarColumn(IsIgnore = true)]
        public string unit { get; set; }

        /// <summary>
        ///     原始总库存
        /// </summary>
        [Display(Name = "原始总库存")]
        [SugarColumn(IsIgnore = true)]
        public int totalStock { get; set; } = 0;

        /// <summary>
        ///     会员价格体系
        /// </summary>
        [Display(Name = "会员价格体系")]
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsGoodsGrade> gradePrice { get; set; } = new();

        /// <summary>
        ///     关联会员级别
        /// </summary>
        [Display(Name = "会员价格")]
        [SugarColumn(IsIgnore = true)]
        public object gradeInfo { get; set; }

        /// <summary>
        ///     初始化匹配sku
        /// </summary>
        [Display(Name = "初始化匹配sku")]
        [SugarColumn(IsIgnore = true)]
        public Dictionary<string, Dictionary<string, DefaultSpesDesc>> defaultSpecificationDescription { get; set; } =
            new();

        /// <summary>
        ///     商品总价格,商品单价乘以数量
        /// </summary>
        [Display(Name = "商品总价格,商品单价乘以数量")]
        [SugarColumn(IsIgnore = true)]
        public decimal amount { get; set; } = 0;

        /// <summary>
        ///     促销列表
        /// </summary>
        [Display(Name = "促销列表")]
        [SugarColumn(IsIgnore = true)]
        public Dictionary<int, WxNameTypeDto> promotionList { get; set; } = new();

        /// <summary>
        ///     促销金额
        /// </summary>
        [Display(Name = "促销金额")]
        [SugarColumn(IsIgnore = true)]
        public decimal promotionAmount { get; set; } = 0;

        /// <summary>
        ///     拼团购买数量
        /// </summary>
        [Display(Name = "拼团购买数量")]
        [SugarColumn(IsIgnore = true)]
        public int buyPinTuanCount { get; set; } = 0;

        /// <summary>
        ///     促销购买件数
        /// </summary>
        [Display(Name = "促销购买件数")]
        [SugarColumn(IsIgnore = true)]
        public int buyPromotionCount { get; set; } = 0;

        /// <summary>
        ///     是否参与拼团规则
        /// </summary>
        [Display(Name = "是否参与拼团规则")]
        [SugarColumn(IsIgnore = true)]
        public CoreCmsPinTuanRule pinTuanRule { get; set; }

        /// <summary>
        ///     一级佣金
        /// </summary>
        [Display(Name = "一级佣金")]
        [SugarColumn(IsIgnore = true)]
        public decimal levelOne { get; set; } = 0;

        /// <summary>
        ///     二级佣金
        /// </summary>
        [Display(Name = "二级佣金")]
        [SugarColumn(IsIgnore = true)]
        public decimal levelTwo { get; set; } = 0;

        /// <summary>
        ///     三级佣金
        /// </summary>
        [Display(Name = "三级佣金")]
        [SugarColumn(IsIgnore = true)]
        public decimal levelThree { get; set; } = 0;
    }
}