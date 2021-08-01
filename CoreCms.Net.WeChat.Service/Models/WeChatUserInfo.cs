/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/29 1:19:20
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCms.Net.WeChat.Service.Models
{
    /// <summary>
    /// 微信小程序用户信息结构
    /// </summary>

    public class WeChatUserInfo

    {
        public string openId { get; set; }

        public string nickName { get; set; }

        public int gender { get; set; }

        public string city { get; set; }

        public string province { get; set; }

        public string country { get; set; }

        public string avatarUrl { get; set; }

        public string unionId { get; set; }

        public Watermark watermark { get; set; }


    }


    [Serializable]
    public class DecodeEntityBase
    {
        public Watermark watermark { get; set; }
    }

    /// <summary>
    /// 解码后的用户信息
    /// </summary>
    [Serializable]
    public class DecodedUserInfo : DecodeEntityBase
    {
        public string openId { get; set; }
        public string nickName { get; set; }
        public int gender { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string avatarUrl { get; set; }
        public string unionId { get; set; }
    }

}
