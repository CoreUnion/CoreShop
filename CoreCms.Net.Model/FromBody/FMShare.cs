/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


using Newtonsoft.Json.Linq;

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    ///     分享提交认证实体
    /// </summary>
    public class FMShare
    {
        /// <summary>
        ///     场景值|1店铺首页，2商品详情页，3拼团详情页,4邀请好友（店铺页面,params里需要传store）,5文章页面,6参团页面，7自定义页面，8智能表单，9团购秒杀
        /// </summary>
        public int page { get; set; }

        /// <summary>
        ///     url，前端地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        ///     参数集合，根据场景值不一样而内容不一样
        /// 1
        /// 2 goodsId:商品ID
        /// 3 goodsId:商品ID，teamId:拼团ID
        /// 4 store:店铺code
        /// 5 articleId:文章ID，articleType:文章类型
        /// 6 goodsId:商品ID，groupId:参团ID，teamId:拼团ID
        /// 7 pageCode:自定义页面code
        /// 8 id：智能表单ID
        /// 9 goodsId:商品ID，groupId:团购秒杀ID
        ///     type	类型，1url，2二维码，3海报
        ///     token	可以保存推荐人的信息
        ///     client	终端，1普通h5，2微信小程序，3微信公众号（h5），4头条系小程序,5pc，6阿里小程序
        /// 10 store:店铺code
        /// </summary>
        public JObject @params { get; set; }

        /// <summary>
        ///     类型，1url，2二维码，3海报
        /// </summary>
        public int type { get; set; }

        /// <summary>
        ///     终端，1普通h5，2微信小程序，3微信公众号（h5），4头条系小程序,5pc，6阿里小程序
        /// </summary>
        public int client { get; set; }
    }

    /// <summary>
    ///     统一分享解码提交参数
    /// </summary>
    public class FMDeShare
    {
        /// <summary>
        ///     提交编码
        /// </summary>
        public string code { get; set; }
    }
}