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
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Caching;
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 店铺设置表 接口实现
    /// </summary>
    public class CoreCmsSettingRepository : BaseRepository<CoreCmsSetting>, ICoreCmsSettingRepository
    {
        public CoreCmsSettingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        
    }
}
