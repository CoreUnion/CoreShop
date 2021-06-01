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
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.View;
using CoreCms.Net.Utility.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 单页 接口实现
    /// </summary>
    public class CoreCmsPagesRepository : BaseRepository<CoreCmsPages>, ICoreCmsPagesRepository
    {
        public CoreCmsPagesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 重写异步更新方法方法==========================================================

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateAsync(FmPagesUpdate entity)
        {
            var jm = new AdminUiCallBack();

            var bl = false;

            var oldModel = await DbClient.Queryable<CoreCmsPages>().Where(p => p.code == entity.pageCode).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始

            bl = await DbClient.Deleteable<CoreCmsPagesItems>().Where(p => p.pageCode == entity.pageCode).ExecuteCommandHasChangeAsync();
            var list = new List<CoreCmsPagesItems>();
            var count = 0;
            entity.datalist.ForEach(p =>
            {
                var model = new CoreCmsPagesItems
                {
                    widgetCode = p.sType,
                    pageCode = entity.pageCode,
                    positionId = count,
                    sort = count + 1,
                    parameters = p.sValue
                };
                list.Add(model);
                count++;
            });

            if (list.Any())
            {
                bl = await DbClient.Insertable(list).ExecuteCommandAsync() > 0;
            }
            //事物处理过程结束
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;


            return jm;
        }


        #endregion


        ///// <summary>
        ///// 获取首页数据
        ///// </summary>
        ///// <param name="entity">实体数据</param>
        ///// <returns></returns>
        //public async Task<WebApiCallBack> GetPageConfig(string code)
        //{
        //    var jm = new WebApiCallBack();

        //    return jm;
        //}

    }
}
