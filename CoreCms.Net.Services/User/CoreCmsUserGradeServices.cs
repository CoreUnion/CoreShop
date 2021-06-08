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
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 用户等级表 接口实现
    /// </summary>
    public class CoreCmsUserGradeServices : BaseServices<CoreCmsUserGrade>, ICoreCmsUserGradeServices
    {
        private readonly ICoreCmsUserGradeRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsUserGradeServices(IUnitOfWork unitOfWork, ICoreCmsUserGradeRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }



    }
}
