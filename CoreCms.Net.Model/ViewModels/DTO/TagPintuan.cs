/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


using System;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    public class TagPinTuanResult
    {
        /// <summary>
        ///     序列
        /// </summary>
        public int id { get; set; }

        /// <summary>
        ///     活动名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///     开始时间
        /// </summary>
        public DateTime startTime { get; set; }

        /// <summary>
        ///     结束时间
        /// </summary>
        public DateTime endTime { get; set; }

        /// <summary>
        ///     人数2-10人
        /// </summary>
        public int peopleNumber { get; set; }

        /// <summary>
        ///     单位分钟
        /// </summary>
        public int significantInterval { get; set; }

        /// <summary>
        ///     优惠金额
        /// </summary>
        public decimal discountAmount { get; set; }


        /// <summary>
        ///     销售价格
        /// </summary>
        public decimal goodsPrice { get; set; }

        /// <summary>
        ///     每人限购数量
        /// </summary>
        public int maxNums { get; set; }

        /// <summary>
        ///     每个商品活动数量
        /// </summary>
        public int maxGoodsNums { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        ///     是否开启
        /// </summary>
        public bool isStatusOpen { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>

        public DateTime? createTime { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>

        public DateTime? updateTime { get; set; }


        public int goodsId { get; set; }
        public string goodsName { get; set; }
        public string goodsImages { get; set; }
        public string goodsImage { get; set; }
    }
}