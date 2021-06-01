/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.IServices
{
    /// <summary>
    /// 用户积分记录表 服务工厂接口
    /// </summary>
    public interface ICoreCmsUserPointLogServices : IBaseServices<CoreCmsUserPointLog>
    {
        /// <summary>
        /// 积分消费日志设置
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="num"></param>
        /// <param name="type"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        Task<WebApiCallBack> SetPoint(int userId, int num, int type, string remarks);


        /// <summary>
        /// 订单完成送积分操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="money"></param>
        /// <param name="orderId"></param>
        Task OrderComplete(int userId, decimal money, string orderId);


        /// <summary>
        /// 判断今天是否签到
        /// </summary>
        /// <param name="userId"></param>
        Task<WebApiCallBack> IsSign(int userId);


        /// <summary>
        /// 用户签到
        /// </summary>
        /// <param name="userId"></param>
        Task<WebApiCallBack> Sign(int userId);
    }
}
