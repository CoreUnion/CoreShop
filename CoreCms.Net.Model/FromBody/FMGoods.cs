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
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    ///     新建商品保存提交类
    /// </summary>
    public class FMGoodsInsertModel
    {
        /// <summary>
        ///     商品信息
        /// </summary>
        public CoreCmsGoods goods { get; set; }

        /// <summary>
        ///     生成货品信息
        /// </summary>
        public List<CoreCmsProducts> products { get; set; } = null;

        /// <summary>
        ///     会员价格体系
        /// </summary>
        public List<gradePrice> gradePrice { get; set; }

        /// <summary>
        ///     栏目扩展
        /// </summary>
        public string goodsCategoryExtendIds { get; set; }
    }

    public class gradePrice
    {
        public int key { get; set; }
        public decimal value { get; set; }
    }

    /// <summary>
    ///     批量修改金额提交
    /// </summary>
    public class FmBatchModifyPrice
    {
        /// <summary>
        ///     序列数组
        /// </summary>
        public int[] ids { get; set; }

        /// <summary>
        ///     变更方式+-*/
        /// </summary>
        public string modifyType { get; set; }

        /// <summary>
        ///     变更类型
        /// </summary>
        public string priceType { get; set; }

        /// <summary>
        ///     金额或倍数
        /// </summary>
        public decimal priceValue { get; set; }
    }


    /// <summary>
    ///     批量修改库存提交
    /// </summary>
    public class FmBatchModifyStock
    {
        /// <summary>
        ///     序列数组
        /// </summary>
        public int[] ids { get; set; }

        /// <summary>
        ///     变更方式+-*/
        /// </summary>
        public string modifyType { get; set; }

        /// <summary>
        ///     变更至
        /// </summary>
        public int modifyValue { get; set; }
    }

    /// <summary>
    ///     提交设置标签实体
    /// </summary>
    public class FmSetLabel
    {
        /// <summary>
        ///     序列数组
        /// </summary>
        public int[] ids { get; set; }

        public List<labels> labels { get; set; }
    }

    public class labels
    {
        public string text { get; set; }
        public string style { get; set; }
    }


    public class FMGetProductInfo
    {
        public int id { get; set; }
        public string type { get; set; }
        public int groupId { get; set; } = 0;
    }
}