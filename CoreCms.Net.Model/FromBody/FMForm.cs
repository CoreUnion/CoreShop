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
    public class FMForm
    {
        /// <summary>
        ///     form实体
        /// </summary>
        public CoreCmsForm model { get; set; }

        /// <summary>
        ///     子集表单
        /// </summary>
        public List<CoreCmsFormItem> items { get; set; }
    }

    /// <summary>
    ///     获取表单详情提交数据
    /// </summary>
    public class FmGetForm
    {
        public int id { get; set; } = 0;
        public string token { get; set; }
    }

    /// <summary>
    ///     提交表单
    /// </summary>
    public class FmAddSubmit
    {
        public int id { get; set; } = 0;
        public string token { get; set; }

        public List<FmAddSubmitItems> data { get; set; }
    }

    /// <summary>
    ///     获取提交表单明细资源
    /// </summary>
    public class FmAddSubmitItems
    {
        public int key { get; set; }

        public string value { get; set; }
    }

    /// <summary>
    ///     获取提交表单明细资源中的商品数据
    /// </summary>
    public class FmAddSubmitItemGoods
    {
        public int key { get; set; }

        public int productId { get; set; }
        public int goodsId { get; set; }
        public int nums { get; set; }
        public decimal price { get; set; }
    }
}