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
    #region 查询货运公司列表返回实体数据

    /// <summary>
    ///     查询货运公司列表返回实体数据
    /// </summary>
    public class ShowApiGetExpressCompanyListResult
    {
        /// <summary>
        ///     错误说明
        /// </summary>
        public string showapi_res_error { get; set; }

        /// <summary>
        ///     状态码
        /// </summary>
        public int showapi_res_code { get; set; }

        /// <summary>
        ///     返回资源序列
        /// </summary>
        public string showapi_res_id { get; set; }

        /// <summary>
        ///     返回资源主体
        /// </summary>
        public ResultBody showapi_res_body { get; set; }
    }

    public class ResultBody
    {
        public int ret_code { get; set; }
        public bool flag { get; set; }
        public int page { get; set; }
        public int showapi_fee_codepage { get; set; }
        public int allNum { get; set; }
        public string msg { get; set; }
        public int maxSize { get; set; }

        public List<expressCompanyList> expressList { get; set; }
    }

    public class expressCompanyList
    {
        public string imgUrl { get; set; }
        public string simpleName { get; set; }
        public string phone { get; set; }
        public string expName { get; set; }
        public string note { get; set; }
        public string url { get; set; }
    }

    //错误码
    //-1，系统调用错误
    //-2，可调用次数或金额为0
    //-3，读取超时
    //-4，服务端返回数据解析错误
    //-5，后端服务器DNS解析错误
    //-6，服务不存在或未上线
    //-7, API创建者的网关资源不足
    //-1000，系统维护
    //-1002，showapi_appid字段必传
    //-1003，showapi_sign字段必传
    //-1004，签名sign验证有误
    //-1005，showapi_timestamp无效
    //-1006，app无权限调用接口 
    //-1007，没有订购套餐
    //-1008，服务商关闭对您的调用权限
    //-1009，调用频率受限
    //-1010，找不到您的应用
    //-1011，子授权app_child_id无效
    //-1012，子授权已过期或失效
    //-1013，子授权ip受限
    //-1014，token权限无效

    #endregion 查询物流信息

    #region MyRegion

    /// <summary>
    ///     查询货运公司列表返回实体数据
    /// </summary>
    public class ShowApiGetExpressPollResult
    {
        /// <summary>
        ///     错误说明
        /// </summary>
        public string showapi_res_error { get; set; }

        /// <summary>
        ///     状态码
        /// </summary>
        public int showapi_res_code { get; set; }

        /// <summary>
        ///     返回资源序列
        /// </summary>
        public string showapi_res_id { get; set; }

        /// <summary>
        ///     返回资源主体
        /// </summary>
        public ExpressPollResBody showapi_res_body { get; set; }
    }


    public class ExpressPollResBody
    {
        public long update { get; set; }
        public string upgrade_info { get; set; }
        public string updateStr { get; set; }
        public string logo { get; set; }
        public int dataSize { get; set; }
        public int status { get; set; }
        public int fee_num { get; set; }
        public string tel { get; set; }
        public List<PollData> data { get; set; }

        public string expSpellName { get; set; }
        public string msg { get; set; }
        public string mailNo { get; set; }
        public int queryTimes { get; set; }
        public int ret_code { get; set; }
        public bool flag { get; set; }
        public string expTextName { get; set; }
        public List<object> possibleExpList { get; set; }
    }

    public class PollData
    {
        public string time { get; set; }
        public string context { get; set; }
    }

    #endregion
}