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
using System.Collections.Generic;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    /// <summary>
    ///     优惠券相关
    /// </summary>
    public class GetMyCouponResultDto
    {
        /// <summary>
        ///     优惠券编码
        /// </summary>
        public string couponCode { get; set; }

        /// <summary>
        ///     优惠券名称
        /// </summary>
        public string couponName { get; set; }

        /// <summary>
        ///     优惠券id
        /// </summary>
        public int promotionId { get; set; }

        /// <summary>
        ///     是否使用
        /// </summary>
        public bool isUsed { get; set; }

        /// <summary>
        ///     谁领取了
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        ///     被谁用了
        /// </summary>
        public string usedId { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime createTime { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        public DateTime? updateTime { get; set; }

        /// <summary>
        ///     条件
        /// </summary>
        public string expression1 { get; set; }

        /// <summary>
        ///     结果
        /// </summary>
        public string expression2 { get; set; }

        /// <summary>
        ///     是否结束
        /// </summary>
        public bool isExpire { get; set; }

        /// <summary>
        ///     开始时间
        /// </summary>
        public DateTime startTime { get; set; }

        /// <summary>
        ///     结束时间
        /// </summary>
        public DateTime endTime { get; set; }

        /// <summary>
        ///     开始时间缩写
        /// </summary>
        public string stime { get; set; }

        /// <summary>
        ///     结束时间缩写
        /// </summary>
        public string etime { get; set; }


        /// <summary>
        ///     条件集合
        /// </summary>
        public List<string> conditions { get; set; } = new();

        /// <summary>
        ///     结果集合
        /// </summary>
        public List<string> results { get; set; } = new();
    }
}