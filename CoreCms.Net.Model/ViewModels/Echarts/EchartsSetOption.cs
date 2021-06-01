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

namespace CoreCms.Net.Model.ViewModels.Echarts
{
    public class EchartsOption
    {
        /// <summary>
        ///     标题组件，包含主标题和副标题。
        /// </summary>
        public Title title { get; set; } = new() {text = "报表"};

        /// <summary>
        ///     提示框组件
        /// </summary>
        public Tooltip tooltip { get; set; } = new() {trigger = "axis"};

        /// <summary>
        ///     图例组件
        /// </summary>
        public Legend legend { get; set; } = new();

        /// <summary>
        ///     直角坐标系内绘图网格
        /// </summary>
        public Grid grid { get; set; } = new() {left = "3%", right = "4%", bottom = "3%", containLabel = true};

        /// <summary>
        ///     工具栏
        /// </summary>
        public Toolbox toolbox { get; set; } = new() {feature = new Feature {saveAsImage = new List<string>()}};

        /// <summary>
        ///     直角坐标系 grid 中的 x 轴
        /// </summary>
        public XAxis xAxis { get; set; } = new() {type = "category", boundaryGap = false, data = new List<string>()};

        /// <summary>
        ///     直角坐标系 grid 中的 y 轴
        /// </summary>
        public YAxis yAxis { get; set; } = new() {type = "value", name = "元"};

        /// <summary>
        ///     系列列表
        /// </summary>
        public List<SeriesItem> series { get; set; } = new();
    }


    //如果好用，请收藏地址，帮忙分享。
    public class Title
    {
        /// <summary>
        ///     订单统计
        /// </summary>
        public string text { get; set; }
    }

    public class Tooltip
    {
        /// <summary>
        /// </summary>
        public string trigger { get; set; }
    }

    public class Legend
    {
        /// <summary>
        /// </summary>
        public List<string> data { get; set; }
    }

    public class Grid
    {
        /// <summary>
        /// </summary>
        public string left { get; set; }

        /// <summary>
        /// </summary>
        public string right { get; set; }

        /// <summary>
        /// </summary>
        public string bottom { get; set; }

        /// <summary>
        /// </summary>
        public bool containLabel { get; set; }
    }

    public class Feature
    {
        /// <summary>
        /// </summary>
        public List<string> saveAsImage { get; set; }
    }

    public class Toolbox
    {
        /// <summary>
        /// </summary>
        public Feature feature { get; set; }
    }

    public class XAxis
    {
        /// <summary>
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// </summary>
        public bool boundaryGap { get; set; }

        /// <summary>
        /// </summary>
        public List<string> data { get; set; }
    }

    public class YAxis
    {
        /// <summary>
        /// </summary>
        public string type { get; set; }

        /// <summary>
        ///     元
        /// </summary>
        public string name { get; set; }
    }

    public class SeriesItem
    {
        /// <summary>
        ///     全部
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// </summary>
        public List<string> data { get; set; }
    }


    public class SeriesDataIntItem
    {
        /// <summary>
        ///     全部
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// </summary>
        public List<int> data { get; set; }
    }


    /// <summary>
    ///     返回订单统计表单数据
    /// </summary>
    public class OrderTableItem
    {
        /// <summary>
        ///     01时
        /// </summary>
        public string x { get; set; }

        /// <summary>
        /// </summary>
        public string order_all_val { get; set; }

        /// <summary>
        /// </summary>
        public int order_all_num { get; set; }

        /// <summary>
        /// </summary>
        public string order_nopay_val { get; set; }

        /// <summary>
        /// </summary>
        public int order_nopay_num { get; set; }

        /// <summary>
        /// </summary>
        public string order_payed_val { get; set; }

        /// <summary>
        /// </summary>
        public int order_payed_num { get; set; }
    }

    /// <summary>
    ///     财务收款单统计表单数据
    /// </summary>
    public class PaymentsTableItem
    {
        /// <summary>
        ///     01时
        /// </summary>
        public string x { get; set; }

        /// <summary>
        /// </summary>
        public string payments_all_val { get; set; }

        /// <summary>
        /// </summary>
        public int payments_all_num { get; set; }

        /// <summary>
        /// </summary>
        public string payments_order_val { get; set; }

        /// <summary>
        /// </summary>
        public int payments_order_num { get; set; }

        /// <summary>
        /// </summary>
        public string payments_order_refund_val { get; set; }

        /// <summary>
        /// </summary>
        public int payments_order_refund_num { get; set; }

        /// <summary>
        /// </summary>
        public string payments_recharge_val { get; set; }

        /// <summary>
        /// </summary>
        public int payments_recharge_num { get; set; }

        /// <summary>
        /// </summary>
        public string payments_tocash_val { get; set; }

        /// <summary>
        /// </summary>
        public int payments_tocash_num { get; set; }
    }
}