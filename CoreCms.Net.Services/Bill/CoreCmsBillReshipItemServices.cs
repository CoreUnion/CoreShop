/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     退货单明细表 接口实现
    /// </summary>
    public class CoreCmsBillReshipItemServices : BaseServices<CoreCmsBillReshipItem>, ICoreCmsBillReshipItemServices
    {
        private readonly ICoreCmsBillReshipItemRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public CoreCmsBillReshipItemServices(IUnitOfWork unitOfWork, ICoreCmsBillReshipItemRepository dal)
        {
            _dal = dal;
            BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}