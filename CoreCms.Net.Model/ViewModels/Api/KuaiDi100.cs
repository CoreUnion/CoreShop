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

namespace CoreCms.Net.Model.ViewModels.Api
{
    /// <summary>
    ///     快递100请求进行的参数封装实体
    /// </summary>
    public class KuaiDi100ApiPostParam
    {
        /// <summary>
        ///     查询的快递公司的编码， 一律用小写字母（如：yuantong）
        /// </summary>
        public string com { get; set; }

        /// <summary>
        ///     查询的快递单号， 单号的最大长度是32个字符
        /// </summary>
        public string num { get; set; }

        /// <summary>
        ///     收、寄件人的电话号码（手机和固定电话均可，只能填写一个，顺丰单号必填，其他快递公司选填。如座机号码有分机号，分机号无需上传。）
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        ///     出发地城市
        /// </summary>
        public string from { get; set; }

        /// <summary>
        ///     目的地城市，到达目的地后会加大监控频率
        /// </summary>
        public string to { get; set; }

        /// <summary>
        ///     添加此字段表示开通行政区域解析功能
        /// </summary>
        public int resultv2 { get; set; }
    }

    /// <summary>
    ///     快递100api查询后返回实体数据
    /// </summary>
    public class KuaiDi100ApiPostResult
    {
        /// <summary>
        ///     消息体，请忽略
        /// </summary>
        public string message { get; set; }

        /// <summary>
        ///     单号
        /// </summary>
        public string nu { get; set; }

        /// <summary>
        ///     是否签收标记，请忽略，明细状态请参考state字段
        /// </summary>
        public string ischeck { get; set; }

        /// <summary>
        ///     快递单明细状态标记，暂未实现，请忽略
        /// </summary>
        public string condition { get; set; }

        /// <summary>
        ///     快递公司编码,一律用小写字母
        /// </summary>
        public string com { get; set; }

        /// <summary>
        ///     通讯状态，请忽略
        /// </summary>
        public string status { get; set; }

        /// <summary>
        ///     通讯状态说明
        /// </summary>
        public string statusStr { get; set; }

        /// <summary>
        ///     快递单当前状态，包括0在途，1揽收，2疑难，3签收，4退签，5派件，6退回，7转单，10待清关，11清关中，12已清关，13清关异常，14收件人拒签等13个状态
        /// </summary>
        public string state { get; set; }

        /// <summary>
        ///     快递单当前状态说明
        /// </summary>
        public string stateStr { get; set; }

        /// <summary>
        ///     最新查询结果，数组，包含多项，全量，倒序（即时间最新的在最前），每项都是对象，对象包含字段请展开
        /// </summary>
        public List<DataItem> data { get; set; }
    }

    //数据详情
    public class DataItem
    {
        /// <summary>
        ///     时间，原始格式
        /// </summary>
        public string time { get; set; }

        /// <summary>
        ///     格式化后时间
        /// </summary>
        public string ftime { get; set; }

        /// <summary>
        ///     内容：【XXXXXXX公司】 派件中 派件人: 滕XX 电话 13787XXXXX 如有疑问，请联系：XXXXX-XXXXX
        /// </summary>
        public string context { get; set; }
    }
}