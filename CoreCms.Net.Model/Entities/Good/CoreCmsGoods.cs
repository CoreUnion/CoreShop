/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统
 *                Web: https://www.corecms.net
 *             Author: 大灰灰
 *              Email: jianweie@163.com
 *         CreateTime: 2021/7/16 1:14:14
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     商品表
    /// </summary>
    public partial class CoreCmsGoods
    {
        /// <summary>
        ///     商品ID
        /// </summary>
        [Display(Name = "商品ID")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     商品条码
        /// </summary>
        [Display(Name = "商品条码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]
        public string bn { get; set; }

        /// <summary>
        ///     商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     商品简介
        /// </summary>
        [Display(Name = "商品简介")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string brief { get; set; }

        /// <summary>
        ///     缩略图
        /// </summary>
        [Display(Name = "缩略图")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string image { get; set; }

        /// <summary>
        ///     图集
        /// </summary>
        [Display(Name = "图集")]
        public string images { get; set; }

        /// <summary>
        ///     视频
        /// </summary>
        [Display(Name = "视频")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string video { get; set; }

        /// <summary>
        ///     佣金分配方式
        /// </summary>
        [Display(Name = "佣金分配方式")]
        [Required(ErrorMessage = "请输入{0}")]
        public int productsDistributionType { get; set; }

        /// <summary>
        ///     商品分类
        /// </summary>
        [Display(Name = "商品分类")]
        [Required(ErrorMessage = "请输入{0}")]
        public int goodsCategoryId { get; set; }

        /// <summary>
        ///     商品类别
        /// </summary>
        [Display(Name = "商品类别")]
        [Required(ErrorMessage = "请输入{0}")]
        public int goodsTypeId { get; set; }

        /// <summary>
        ///     sku序列
        /// </summary>
        [Display(Name = "sku序列")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string goodsSkuIds { get; set; }

        /// <summary>
        ///     参数序列
        /// </summary>
        [Display(Name = "参数序列")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string goodsParamsIds { get; set; }

        /// <summary>
        ///     品牌
        /// </summary>
        [Display(Name = "品牌")]
        [Required(ErrorMessage = "请输入{0}")]
        public int brandId { get; set; }

        /// <summary>
        ///     是否虚拟商品
        /// </summary>
        [Display(Name = "是否虚拟商品")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isNomalVirtual { get; set; }

        /// <summary>
        ///     是否上架
        /// </summary>
        [Display(Name = "是否上架")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isMarketable { get; set; }

        /// <summary>
        ///     商品单位
        /// </summary>
        [Display(Name = "商品单位")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string unit { get; set; }

        /// <summary>
        ///     商品详情
        /// </summary>
        [Display(Name = "商品详情")]
        public string intro { get; set; }

        /// <summary>
        ///     商品规格序列号存储
        /// </summary>
        [Display(Name = "商品规格序列号存储")]
        public string spesDesc { get; set; }

        /// <summary>
        ///     参数序列化
        /// </summary>
        [Display(Name = "参数序列化")]
        public string parameters { get; set; }

        /// <summary>
        ///     评论次数
        /// </summary>
        [Display(Name = "评论次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public int commentsCount { get; set; }

        /// <summary>
        ///     浏览次数
        /// </summary>
        [Display(Name = "浏览次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public int viewCount { get; set; }

        /// <summary>
        ///     购买次数
        /// </summary>
        [Display(Name = "购买次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public int buyCount { get; set; }

        /// <summary>
        ///     上架时间
        /// </summary>
        [Display(Name = "上架时间")]
        public DateTime? uptime { get; set; }

        /// <summary>
        ///     下架时间
        /// </summary>
        [Display(Name = "下架时间")]
        public DateTime? downtime { get; set; }

        /// <summary>
        ///     商品排序
        /// </summary>
        [Display(Name = "商品排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }

        /// <summary>
        ///     标签id逗号分隔
        /// </summary>
        [Display(Name = "标签id逗号分隔")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string labelIds { get; set; }

        /// <summary>
        ///     自定义规格名称
        /// </summary>
        [Display(Name = "自定义规格名称")]
        public string newSpec { get; set; }

        /// <summary>
        ///     开启规则
        /// </summary>
        [Display(Name = "开启规则")]
        [Required(ErrorMessage = "请输入{0}")]
        public int openSpec { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime? createTime { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? updateTime { get; set; }

        /// <summary>
        ///     是否推荐
        /// </summary>
        [Display(Name = "是否推荐")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isRecommend { get; set; }

        /// <summary>
        ///     是否热门
        /// </summary>
        [Display(Name = "是否热门")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isHot { get; set; }

        /// <summary>
        ///     是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDel { get; set; }
    }
}