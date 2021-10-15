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
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 菜单表 接口实现
    /// </summary>
    public class SysMenuRepository : BaseRepository<SysMenu>, ISysMenuRepository
    {
        public SysMenuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(SysMenu entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync() > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(SysMenu entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await DbClient.Queryable<SysMenu>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            //oldModel.id = entity.id;
            oldModel.parentId = entity.parentId;
            oldModel.menuName = entity.menuName;
            oldModel.menuIcon = entity.menuIcon;
            oldModel.path = entity.path;
            oldModel.component = entity.component;
            oldModel.menuType = entity.menuType;
            oldModel.sortNumber = entity.sortNumber;
            oldModel.authority = entity.authority;
            oldModel.target = entity.target;
            oldModel.iconColor = entity.iconColor;
            oldModel.hide = entity.hide;
            //oldModel.deleted = entity.deleted;
            //oldModel.createTime = oldModel.createTime;
            oldModel.updateTime = DateTime.Now;
            oldModel.identificationCode = entity.identificationCode;

            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<SysMenu> entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

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

            var all = await GetCaChe();
            var model = all.Find(p => p.id == id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var ids = new List<int>() { id };
            GetIds(all, id, ids);

            var bl = await DbClient.Deleteable<SysMenu>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        #endregion


        /// <summary>
        ///获取下级所有数据序列
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parentId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        private List<int> GetIds(List<SysMenu> data, int parentId, List<int> ids)
        {
            var childs = data.Where(p => p.parentId == parentId).ToList();
            foreach (var item in childs)
            {
                ids.Add(item.id);
                if (data.Exists(p => p.parentId == item.id))
                {
                    ids = GetIds(data, item.id, ids);
                }
            }
            return ids;
        }

        #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysMenu>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<SysMenu>>(GlobalConstVars.CacheSysMenu);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<SysMenu>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<SysMenu>().ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheSysMenu, list);
            return list;
        }

        #endregion


    }
}
