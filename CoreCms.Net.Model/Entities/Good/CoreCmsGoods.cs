/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:58
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 商品表
    /// </summary>
    [SugarTable("CoreCmsGoods",TableDescription = "商品表")]
    public partial class CoreCmsGoods
    {
        /// <summary>
        /// 商品表
        /// </summary>
        public CoreCmsGoods()
        {
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Display(Name = "商品ID")]
        [SugarColumn(ColumnDescription = "商品ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        [Display(Name = "商品条码")]
        [SugarColumn(ColumnDescription = "商品条码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(30, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String bn { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [SugarColumn(ColumnDescription = "商品名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 商品简介
        /// </summary>
        [Display(Name = "商品简介")]
        [SugarColumn(ColumnDescription = "商品简介", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String brief { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        [Display(Name = "缩略图")]
        [SugarColumn(ColumnDescription = "缩略图", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String image { get; set; }
        /// <summary>
        /// 图集
        /// </summary>
        [Display(Name = "图集")]
        [SugarColumn(ColumnDescription = "图集", IsNullable = true)]
        public System.String images { get; set; }
        /// <summary>
        /// 视频
        /// </summary>
        [Display(Name = "视频")]
        [SugarColumn(ColumnDescription = "视频", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String video { get; set; }
        /// <summary>
        /// 佣金分配方式
        /// </summary>
        [Display(Name = "佣金分配方式")]
        [SugarColumn(ColumnDescription = "佣金分配方式")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 productsDistributionType { get; set; }
        /// <summary>
        /// 商品分类
        /// </summary>
        [Display(Name = "商品分类")]
        [SugarColumn(ColumnDescription = "商品分类")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsCategoryId { get; set; }
        /// <summary>
        /// 商品类别
        /// </summary>
        [Display(Name = "商品类别")]
        [SugarColumn(ColumnDescription = "商品类别")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsTypeId { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Display(Name = "品牌")]
        [SugarColumn(ColumnDescription = "品牌")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 brandId { get; set; }
        /// <summary>
        /// 是否虚拟商品
        /// </summary>
        [Display(Name = "是否虚拟商品")]
        [SugarColumn(ColumnDescription = "是否虚拟商品")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isNomalVirtual { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        [Display(Name = "是否上架")]
        [SugarColumn(ColumnDescription = "是否上架")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isMarketable { get; set; }
        /// <summary>
        /// 商品单位
        /// </summary>
        [Display(Name = "商品单位")]
        [SugarColumn(ColumnDescription = "商品单位", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String unit { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        [Display(Name = "商品详情")]
        [SugarColumn(ColumnDescription = "商品详情", IsNullable = true)]
        public System.String intro { get; set; }
        /// <summary>
        /// 商品规格序列号存储
        /// </summary>
        [Display(Name = "商品规格序列号存储")]
        [SugarColumn(ColumnDescription = "商品规格序列号存储", IsNullable = true)]
        public System.String spesDesc { get; set; }
        /// <summary>
        /// 参数序列化
        /// </summary>
        [Display(Name = "参数序列化")]
        [SugarColumn(ColumnDescription = "参数序列化", IsNullable = true)]
        public System.String parameters { get; set; }
        /// <summary>
        /// 评论次数
        /// </summary>
        [Display(Name = "评论次数")]
        [SugarColumn(ColumnDescription = "评论次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 commentsCount { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        [Display(Name = "浏览次数")]
        [SugarColumn(ColumnDescription = "浏览次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 viewCount { get; set; }
        /// <summary>
        /// 购买次数
        /// </summary>
        [Display(Name = "购买次数")]
        [SugarColumn(ColumnDescription = "购买次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 buyCount { get; set; }
        /// <summary>
        /// 上架时间
        /// </summary>
        [Display(Name = "上架时间")]
        [SugarColumn(ColumnDescription = "上架时间", IsNullable = true)]
        public System.DateTime? uptime { get; set; }
        /// <summary>
        /// 下架时间
        /// </summary>
        [Display(Name = "下架时间")]
        [SugarColumn(ColumnDescription = "下架时间", IsNullable = true)]
        public System.DateTime? downtime { get; set; }
        /// <summary>
        /// 商品排序
        /// </summary>
        [Display(Name = "商品排序")]
        [SugarColumn(ColumnDescription = "商品排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
        /// <summary>
        /// 标签id逗号分隔
        /// </summary>
        [Display(Name = "标签id逗号分隔")]
        [SugarColumn(ColumnDescription = "标签id逗号分隔", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String labelIds { get; set; }
        /// <summary>
        /// 自定义规格名称
        /// </summary>
        [Display(Name = "自定义规格名称")]
        [SugarColumn(ColumnDescription = "自定义规格名称", IsNullable = true)]
        public System.String newSpec { get; set; }
        /// <summary>
        /// 开启规则
        /// </summary>
        [Display(Name = "开启规则")]
        [SugarColumn(ColumnDescription = "开启规则")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 openSpec { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        [Display(Name = "是否推荐")]
        [SugarColumn(ColumnDescription = "是否推荐")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isRecommend { get; set; }
        /// <summary>
        /// 是否热门
        /// </summary>
        [Display(Name = "是否热门")]
        [SugarColumn(ColumnDescription = "是否热门")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isHot { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [SugarColumn(ColumnDescription = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDel { get; set; }
    }
}