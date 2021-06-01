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

    //后端接口====================================================================================================


    //Api接口====================================================================================================
    /// <summary>
    /// 前端接口提交售后发货快递信息
    /// </summary>
    public class FMBillReshipForSendReshipPost
    {

        public string logiCode { set; get; }

        public string logiNo { get; set; }

        public string reshipId { get; set; }

    }
}
