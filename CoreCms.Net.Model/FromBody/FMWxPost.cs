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
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    ///     小程序提交数据标准接收实体
    /// </summary>
    public class FMWxPost
    {
        /// <summary>
        ///     页面编码
        /// </summary>
        public string code { get; set; }
    }

    /// <summary>
    ///     微信小程序登录解码数据
    /// </summary>
    public class FMWxLoginDecodeEncryptedData
    {
        public string encryptedData { get; set; }

        public string iv { get; set; }

        public string signature { get; set; }

        public string sessionAuthId { get; set; }
    }

    /// <summary>
    ///     微信小程序登录解码手机号码
    /// </summary>
    public class FMWxLoginDecryptPhoneNumber
    {
        public string encryptedData { get; set; }

        public string iv { get; set; }

        public string sessionAuthId { get; set; }

        public int invitecode { get; set; } = 0;
    }

    /// <summary>
    ///     微信账户创建
    /// </summary>
    public class FMWxAccountCreate
    {
        /// <summary>
        ///     密码
        /// </summary>
        public string password { get; set; } = "";

        /// <summary>
        ///     昵称
        /// </summary>
        public string nickname { get; set; } = "";

        /// <summary>
        ///     头像
        /// </summary>
        public string avatar { get; set; } = "";

        /// <summary>
        ///     短信验证码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        ///     手机号码
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        ///     微信小程序授权sessionAuthId
        /// </summary>
        public string sessionAuthId { get; set; }

        /// <summary>
        ///     来源
        /// </summary>
        public int platform { get; set; }

        /// <summary>
        ///     推荐码
        /// </summary>
        public int invitecode { get; set; } = 0;
    }


    /// <summary>
    ///     微信账户创建
    /// </summary>
    public class FMWxSync
    {
        public string avatarUrl { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public int gender { get; set; }
        public string language { get; set; }
        public string nickName { get; set; }
        public string province { get; set; }
    }


    public class FMWxSendSMS
    {
        /// <summary>
        ///     操作类型
        /// </summary>
        public string code { get; set; }

        /// <summary>
        ///     手机号码
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        ///     方法
        /// </summary>
        public string method { get; set; }
    }


    public class FMWeChatMsgTemplateEdit
    {
        public string title { get; set; }
        public List<CoreCmsUserWeChatMsgTemplate> list { get; set; }
    }


    /// <summary>
    ///     用户发起订阅提交
    /// </summary>
    public class SetWeChatAppletsMessageTip
    {
        public string templateId { get; set; }
        public string status { get; set; }
    }
}