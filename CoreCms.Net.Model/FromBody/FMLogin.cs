/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    ///     用户登录验证实体
    /// </summary>
    public class FMLogin
    {
        public string userName { get; set; }
        public string password { get; set; }
    }


    /// <summary>
    ///     用户登录验证实体
    /// </summary>
    public class FMEditLoginUserPassWord
    {
        public string oldPassword { get; set; }
        public string password { get; set; }
        public string repassword { get; set; }
    }
}