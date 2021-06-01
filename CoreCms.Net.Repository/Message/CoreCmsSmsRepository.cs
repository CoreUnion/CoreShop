
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
using System.IO;
using System.Net;
using System.Text;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 短信发送日志 接口实现
    /// </summary>
    public class CoreCmsSmsRepository : BaseRepository<CoreCmsSms>, ICoreCmsSmsRepository
    {
        public CoreCmsSmsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

    }
}
