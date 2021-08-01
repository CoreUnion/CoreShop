/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/29 11:09:03
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCms.Net.WeChat.Service.Models
{
    [Serializable]
    public class DecodedRunData : DecodeEntityBase
    {
        public List<DecodedRunData_StepModel> stepInfoList { get; set; }
    }

    [Serializable]
    public class DecodedRunData_StepModel
    {
        public long timestamp { get; set; }
        public long step { get; set; }
    }
}
