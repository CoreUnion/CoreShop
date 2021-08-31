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
using System.Linq;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 促销条件表 接口实现
    /// </summary>
    public class CoreCmsPromotionConditionRepository : BaseRepository<CoreCmsPromotionCondition>, ICoreCmsPromotionConditionRepository
    {

        public CoreCmsPromotionConditionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
