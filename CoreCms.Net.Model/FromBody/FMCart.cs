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
using System.Text;

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    /// 单个货品接入购物车
    /// </summary>
    public class FMCartAdd
    {
        /// <summary>
        /// 单品数量
        /// </summary>
        public int Nums { get; set; } = 0;

        /// <summary>
        /// 货品序号
        /// </summary>
        public int ProductId { get; set; } = 0;

        /// <summary>
        /// 数量类型 1是直接增加，2是赋值
        /// </summary>
        public int type { get; set; } = 1;

        /// <summary>
        /// 普通购物还是团购秒杀/关联CartTypes
        /// </summary>
        public int cartType { get; set; } = 1;

        /// <summary>
        /// 非普通货品，关联对象序列
        /// </summary>
        public int objectId { get; set; } = 0;
    }

    /// <summary>
    /// 获取购物车列表提交实体
    /// </summary>
    public class FMCartGetList
    {
        /// <summary>
        /// 用户序列
        /// </summary>
        public int userId { get; set; } = 0;

        /// <summary>
        /// 购物车数据
        /// </summary>
        public string ids { get; set; } = null;

        /// <summary>
        /// 购物车类型
        /// </summary>
        public int type { get; set; } = 1;

        /// <summary>
        /// 区域编码
        /// </summary>
        public int areaId { get; set; } = 0;

        /// <summary>
        /// 积分
        /// </summary>
        public int point { get; set; } = 0;
        /// <summary>
        /// 优惠券码
        /// </summary>

        public string couponCode { get; set; }

        /// <summary>
        /// 配送方式是否包邮   1=快递配送（要去算运费）生成订单记录快递方式  2=门店自提（不需要计算运费）生成订单记录门店自提信息
        /// </summary>
        public int receiptType { get; set; } = 1;


        /// <summary>
        /// 关联非普通订单对象序列
        /// </summary>
        public int objectId { get; set; } = 0;


    }


}
