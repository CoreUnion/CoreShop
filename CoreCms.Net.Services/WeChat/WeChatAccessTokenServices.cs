/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/28 20:42:38
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     微信授权交互 接口实现
    /// </summary>
    public class WeChatAccessTokenServices : BaseServices<WeChatAccessToken>, IWeChatAccessTokenServices
    {
        private readonly IWeChatAccessTokenRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public WeChatAccessTokenServices(IUnitOfWork unitOfWork, IWeChatAccessTokenRepository dal)
        {
            _dal = dal;
            BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}