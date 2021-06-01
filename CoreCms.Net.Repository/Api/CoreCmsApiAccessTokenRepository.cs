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
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 第三方授权记录表 接口实现
    /// </summary>
    public class CoreCmsApiAccessTokenRepository : BaseRepository<CoreCmsApiAccessToken>, ICoreCmsApiAccessTokenRepository
    {
        public CoreCmsApiAccessTokenRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


    }
}
