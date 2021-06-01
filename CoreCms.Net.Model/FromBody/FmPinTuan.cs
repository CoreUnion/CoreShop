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
    ///     根据订单id取拼团信息提交参数
    /// </summary>
    public class FMGetPinTuanTeamPost
    {
        public string orderId { get; set; } = "";
        public int teamId { get; set; } = 0;
    }
}