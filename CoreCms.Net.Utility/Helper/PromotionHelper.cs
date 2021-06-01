/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-03-14 4:54:44
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.Utility.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.Utility.Helper
{
    public static class PromotionHelper
    {
        /// <summary>
        /// 根据结果类型返回相应的参数数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string GetResultMsg(string code, string json)
        {
            var msg = string.Empty;
            var obj = (JObject)JsonConvert.DeserializeObject(json);
            switch (code)
            {
                case "GOODS_REDUCE":
                    if (obj != null) msg = "减" + obj["money"].ObjectToString() + "元 ";
                    break;
                case "GOODS_DISCOUNT":
                    if (obj != null) msg = "打" + obj["discount"].ObjectToString() + "折 ";
                    break;
                case "GOODS_ONE_PRICE":
                    if (obj != null) msg = "一口价" + obj["money"].ObjectToString() + "元 ";
                    break;
                case "ORDER_REDUCE":
                    if (obj != null) msg = "订单减" + obj["money"].ObjectToString() + "元 ";
                    break;
                case "ORDER_DISCOUNT":
                    if (obj != null) msg = "订单打" + obj["discount"].ObjectToString() + "折 ";
                    break;
                case "GOODS_HALF_PRICE":
                    if (obj != null)
                        msg = "第" + obj["num"].ObjectToString() + "件" + obj["money"].ObjectToString() + "元";
                    break;
            }
            return msg;
        }

        /// <summary>
        /// 根据条件类型返回相应的参数数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string GetConditionMsg(string code, string json)
        {
            string msg = string.Empty;
            var obj = (JObject)JsonConvert.DeserializeObject(json);
            switch (code)
            {
                case "GOODS_ALL":
                    msg = "购买任意商品 ";
                    break;
                case "GOODS_IDS":
                    msg = "购买指定商品 ";
                    break;
                case "GOODS_CATS":
                    msg = "购买指定分类商品 ";
                    break;
                case "GOODS_BRANDS":
                    msg = "购买指定品牌商品 ";
                    break;
                case "ORDER_FULL":
                    if (obj != null) msg = "购买订单满" + obj["money"].ObjectToString() + "元 ";
                    break;
                case "USER_GRADE":
                    msg = "用户符合指定等级";
                    break;
            }
            return msg;
        }

    }
}
