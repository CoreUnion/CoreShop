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
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Repository
{
    /// <summary>
    ///     微信授权交互 接口实现
    /// </summary>
    public class WeChatAccessTokenRepository : BaseRepository<WeChatAccessToken>, IWeChatAccessTokenRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public WeChatAccessTokenRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}