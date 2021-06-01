/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


namespace CoreCms.Net.Model.ViewModels.Sms
{
    /// <summary>
    ///     凯信通接口短信
    /// </summary>
    public class SMSOptions
    {
        /// <summary>
        ///     是否开启
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        ///     用户序列
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     用户账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        ///     用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     提交地址
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        ///     短信签名
        /// </summary>
        public string Signature { get; set; }
    }
}