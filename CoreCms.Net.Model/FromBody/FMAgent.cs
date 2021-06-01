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
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Model.FromBody
{

    public class FMCreateAgentGood
    {
        /// <summary>
        /// 商品主体
        /// </summary>
        public CoreCmsAgentGoods good { get; set; }

        /// <summary>
        /// 货品数据
        /// </summary>
        public List<CoreCmsAgentProducts> products { get; set; }
    }



    //API接口提交================================================
    /// <summary>
    /// 申请成为代理商接口提交参数
    /// </summary>
    public class FMAgentApply
    {
        public string agreement { get; set; }
        public string mobile { get; set; }
        public string name { get; set; }
        public string qq { get; set; }
        public string weixin { get; set; }
    }


    /// <summary>
    /// 代理商店铺设置提交参数
    /// </summary>
    public class FMSetAgentStorePost
    {
        public string storeBanner { get; set; }
        public string storeDesc { get; set; }
        public string storeLogo { get; set; }
        public string storeName { get; set; }
    }




}
