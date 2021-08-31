/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 促销结果表 接口实现
    /// </summary>
    public class CoreCmsPromotionResultRepository : BaseRepository<CoreCmsPromotionResult>, ICoreCmsPromotionResultRepository
    {
        public CoreCmsPromotionResultRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
