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
using CoreCms.Net.Utility.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;

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


        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsPages entity)
        {
            var jm = new AdminUiCallBack();

            var have = await DbClient.Queryable<CoreCmsPages>().Where(p => p.code == entity.code).With(SqlWith.NoLock).AnyAsync();
            if (have)
            {
                jm.msg = "存在相同【区域编码】请更正";
                return jm;
            }

            entity.code = entity.code.Trim();

            var id = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync();
            var bl = id > 0;

            if (bl && entity.type == 1)
            {
                //如果设为新默认，则修改其他为非默认。
                await DbClient.Updateable<CoreCmsPages>().Where(p => p.type == 1 && p.id != id).SetColumns(p => new CoreCmsPages() { type = 2 }).ExecuteCommandAsync();
            }

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsPages entity)
        {
            var jm = new AdminUiCallBack();

            var have = await DbClient.Queryable<CoreCmsPages>().Where(p => p.code == entity.code && p.id != entity.id).With(SqlWith.NoLock).AnyAsync();
            if (have)
            {
                jm.msg = "存在相同【区域编码】请更正";
                return jm;
            }
            var oldModel = await DbClient.Queryable<CoreCmsPages>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            var oldType = oldModel.type;
            var newType = entity.type;

            oldModel.code = entity.code;
            oldModel.name = entity.name;
            oldModel.description = entity.description;
            oldModel.layout = entity.layout;
            oldModel.type = entity.type;

            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();

            if (bl)
            {
                //如果不是默认的情况下
                if (oldType == 1 && newType != 1)
                {
                    //判断修改当前，而是否其他有默认
                    var haveDefault = await DbClient.Queryable<CoreCmsPages>().Where(p => p.type == 1 && p.id != oldModel.id).With(SqlWith.NoLock).AnyAsync();
                    //如果不存在，则当前不能调整为非默认。
                    if (!haveDefault)
                    {
                        await DbClient.Updateable<CoreCmsPages>().Where(p => p.id == oldModel.id).SetColumns(p => new CoreCmsPages() { type = 1 }).ExecuteCommandAsync();
                    }
                }
                //如果设为新默认，则修改其他为非默认。
                if (newType == 1)
                {
                    await DbClient.Updateable<CoreCmsPages>().Where(p => p.id != oldModel.id).SetColumns(p => new CoreCmsPages() { type = 2 }).ExecuteCommandAsync();
                }
            }


            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> DeleteByIdAsync(int id)
        {
            var jm = new AdminUiCallBack();

            var model = await DbClient.Queryable<CoreCmsPages>().Where(p => p.id == id).FirstAsync();
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            if (model.type == 1)
            {
                jm.msg = "默认页面禁止删除";
                return jm;
            }

            var count = await DbClient.Queryable<CoreCmsPages>().CountAsync();
            if (count == 1)
            {
                jm.msg = "只有一个页面了，别删了。";
                return jm;
            }

            var bl = await DbClient.Deleteable<CoreCmsPages>(id).ExecuteCommandHasChangeAsync();
            if (bl)
            {
                await DbClient.Deleteable<CoreCmsPagesItems>().Where(p => p.pageCode == model.code).ExecuteCommandAsync();
            }
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }


        /// <summary>
        /// 复制一个同样的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> CopyByIdAsync(int id)
        {
            var jm = new AdminUiCallBack();

            var model = await DbClient.Queryable<CoreCmsPages>().Where(p => p.id == id).FirstAsync();
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            var oldCode = model.code;
            model.type = 2;
            model.code = model.code + DateTime.Now.ToString("yyyyMMddHHmmssfffff");
            model.name = model.name + "（复制）";

            var items = await DbClient.Queryable<CoreCmsPagesItems>().Where(p => p.pageCode == oldCode).ToListAsync();
            foreach (var item in items)
            {
                item.pageCode = model.code;
            }

            var bl = await DbClient.Insertable<CoreCmsPages>(model).ExecuteReturnIdentityAsync() > 0;
            if (bl)
            {
                await DbClient.Insertable<CoreCmsPagesItems>(items).ExecuteCommandAsync();
            }
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }



        #region 更新设计==========================================================

        /// <summary>
        /// 更新设计
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateDesignAsync(FmPagesUpdate entity)
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
    }
}
