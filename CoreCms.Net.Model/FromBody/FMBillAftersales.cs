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
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    /// 后台审核售后单提交参数
    /// </summary>
    public class FMBillAftersalesAddPost
    {
        public string aftersalesId { get; set; }
        public int status { get; set; }
        public int type { get; set; }
        public decimal refund { get; set; }
        public string mark { get; set; }
        public JArray items { get; set; }

    }
}
