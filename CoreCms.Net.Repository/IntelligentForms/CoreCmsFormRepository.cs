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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 表单 接口实现
    /// </summary>
    public class CoreCmsFormRepository : BaseRepository<CoreCmsForm>, ICoreCmsFormRepository
    {
        public CoreCmsFormRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 重写异步插入方法==========================================================
        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> InsertAsync(FMForm entity)
        {
            var jm = new AdminUiCallBack();


            var check = CheckItems(entity.items);
            if (check.code == 1)
            {
                return check;
            }

            entity.model.createTime = DateTime.Now;

            var id = await DbClient.Insertable(entity.model).ExecuteReturnIdentityAsync();
            entity.items.ForEach(p =>
            {
                p.formId = id;
            });

            var bl = await DbClient.Insertable(entity.items).ExecuteCommandAsync() > 0;

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;


            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }
        #endregion

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateAsync(FMForm entity)
        {
            var jm = new AdminUiCallBack();

            var check = CheckItems(entity.items);
            if (check.code == 1)
            {
                return check;
            }

            var oldModel = await DbClient.Queryable<CoreCmsForm>().In(entity.model.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            //oldModel.id = entity.model.id;
            oldModel.name = entity.model.name;
            oldModel.type = entity.model.type;
            oldModel.sort = entity.model.sort;
            oldModel.images = entity.model.images;
            oldModel.videoPath = entity.model.videoPath;
            oldModel.description = entity.model.description;
            oldModel.headType = entity.model.headType;
            oldModel.headTypeValue = entity.model.headTypeValue;
            oldModel.headTypeVideo = entity.model.headTypeVideo;
            oldModel.buttonName = entity.model.buttonName;
            oldModel.buttonColor = entity.model.buttonColor;
            oldModel.isLogin = entity.model.isLogin;
            oldModel.times = entity.model.times;
            //oldModel.qrcode = entity.model.qrcode;
            oldModel.returnMsg = entity.model.returnMsg;
            oldModel.endDateTime = entity.model.endDateTime;
            //oldModel.createTime = entity.model.createTime;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                //查询已经存在的数据
                var items = await DbClient.Queryable<CoreCmsFormItem>().Where(p => p.formId == oldModel.id).ToListAsync();
                //找到提交的数据是否有老数据
                var oldPostItems = entity.items.Where(p => p.id > 0).ToList();
                if (oldPostItems.Any())
                {
                    var oldPostItemsIds = oldPostItems.Select(p => p.id).ToList();
                    //找到数据库中和提交的数据库中都不存在差集数据进行删除
                    var deletes = items.Where(p => !oldPostItemsIds.Contains(p.id)).ToList();
                    if (deletes.Any())
                    {
                        await DbClient.Deleteable<CoreCmsFormItem>(deletes).ExecuteCommandHasChangeAsync();
                    }
                    //对提交的老数据进行更新处理
                    var oldDataItems = items.Where(p => oldPostItemsIds.Contains(p.id)).ToList();
                    if (oldDataItems.Any())
                    {
                        oldDataItems.ForEach(p =>
                        {
                            var child = oldPostItems.Find(o => o.id == p.id);
                            if (child != null)
                            {
                                p.name = child.name;
                                p.type = child.type;
                                p.validationType = child.validationType;
                                p.value = child.value;
                                p.defaultValue = child.defaultValue;
                                //p.formId = child.formId;
                                p.required = child.required;
                                p.sort = child.sort;

                            }
                        });
                        await DbClient.Updateable(oldDataItems).ExecuteCommandHasChangeAsync();
                    }
                }
                else
                {
                    await DbClient.Deleteable<CoreCmsFormItem>(items).ExecuteCommandHasChangeAsync();
                }

                //新数据
                var newPostItems = entity.items.Where(p => p.id == 0).ToList();
                if (newPostItems.Any())
                {
                    entity.items.ForEach(p =>
                    {
                        p.formId = oldModel.id;
                    });
                    await DbClient.Insertable(newPostItems).ExecuteCommandAsync();
                }
                await UpdateCaChe();
            }
            return jm;
        }

        public AdminUiCallBack CheckItems(List<CoreCmsFormItem> items)
        {
            var jm = new AdminUiCallBack();

            if (!items.Any())
            {
                jm.msg = "请添加字段数据";
                return jm;
            }

            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.name))
                {
                    jm.msg = "字段名称不能为空";
                    return jm;
                }

                if (!string.IsNullOrEmpty(item.defaultValue))
                {
                    item.defaultValue = item.defaultValue.Trim();
                    item.defaultValue = item.defaultValue.Replace("，", ",");
                }
                if (!string.IsNullOrEmpty(item.value))
                {
                    item.value = item.value.Trim();
                    item.value = item.value.Replace("，", ",");
                }

                if (item.type == GlobalEnumVars.FormFieldTypes.goods.ToString() && string.IsNullOrEmpty(item.value))
                {
                    jm.msg = "【商品】字段必须要选择商品";
                    return jm;
                }
                else if (item.type == GlobalEnumVars.FormFieldTypes.radio.ToString() && string.IsNullOrEmpty(item.value))
                {
                    jm.msg = "【单选】字段必须要至少有二个以上值,并且以逗号分隔";
                    return jm;
                }
                else if (item.type == GlobalEnumVars.FormFieldTypes.radio.ToString() && !string.IsNullOrEmpty(item.value))
                {
                    var arr = item.value.Split(",");
                    if (arr.Length < 2)
                    {
                        jm.msg = "【单选】字段必须要至少有二个以上值,并且以逗号分隔";
                        return jm;
                    }
                }
                else if (item.type == GlobalEnumVars.FormFieldTypes.checbox.ToString() && string.IsNullOrEmpty(item.value))
                {
                    jm.msg = "【多选项】字段必须要至少有二个或以上值,并且以逗号分隔";
                    return jm;
                }
                else if (item.type == GlobalEnumVars.FormFieldTypes.checbox.ToString() && !string.IsNullOrEmpty(item.value))
                {
                    var arr = item.value.Split(",");
                    if (arr.Length < 2)
                    {
                        jm.msg = "【多选项】字段必须要至少有二个词组或以上值,并且以逗号分隔";
                        return jm;
                    }
                }
                else if (item.type == GlobalEnumVars.FormFieldTypes.date.ToString())
                {
                    //没有值的时候设置下默认值
                }
                else if (item.type == GlobalEnumVars.FormFieldTypes.time.ToString())
                {
                    //没有值的时候设置下默认值
                }
            }

            jm.code = 0;

            return jm;
        }


        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsForm entity)
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
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsForm> entity)
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

            var isHaveSubmitData = await DbClient.Queryable<CoreCmsFormSubmit>().AnyAsync(p => p.formId == id);
            if (isHaveSubmitData)
            {
                jm.msg = "此表单已经存在用户提交数据，禁止删除";
                return jm;
            }


            var bl = await DbClient.Deleteable<CoreCmsForm>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await DbClient.Deleteable<CoreCmsFormItem>().Where(p => p.formId == id).ExecuteCommandHasChangeAsync();
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsForm>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        #endregion

        #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoreCmsForm>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<CoreCmsForm>>(GlobalConstVars.CacheCoreCmsForm);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<CoreCmsForm>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<CoreCmsForm>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheCoreCmsForm, list);
            return list;
        }

        #endregion


        #region 重写根据条件查询分页数据
        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<IPageList<CoreCmsForm>> QueryPageAsync(Expression<Func<CoreCmsForm, bool>> predicate,
            Expression<Func<CoreCmsForm, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsForm> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsForm>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsForm
                {
                    id = p.id,
                    name = p.name,
                    type = p.type,
                    sort = p.sort,
                    images = p.images,
                    videoPath = p.videoPath,
                    description = p.description,
                    headType = p.headType,
                    headTypeValue = p.headTypeValue,
                    headTypeVideo = p.headTypeVideo,
                    buttonName = p.buttonName,
                    buttonColor = p.buttonColor,
                    isLogin = p.isLogin,
                    times = p.times,
                    qrcode = p.qrcode,
                    returnMsg = p.returnMsg,
                    endDateTime = p.endDateTime,
                    createTime = p.createTime,
                    updateTime = p.updateTime,
                })
                .Mapper(it => it.Items, it => it.Items.First().formId)
                .With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsForm>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsForm
                {
                    id = p.id,
                    name = p.name,
                    type = p.type,
                    sort = p.sort,
                    images = p.images,
                    videoPath = p.videoPath,
                    description = p.description,
                    headType = p.headType,
                    headTypeValue = p.headTypeValue,
                    headTypeVideo = p.headTypeVideo,
                    buttonName = p.buttonName,
                    buttonColor = p.buttonColor,
                    isLogin = p.isLogin,
                    times = p.times,
                    qrcode = p.qrcode,
                    returnMsg = p.returnMsg,
                    endDateTime = p.endDateTime,
                    createTime = p.createTime,
                    updateTime = p.updateTime,

                })
                .Mapper(it => it.Items, it => it.Items.First().formId)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsForm>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion




    }
}
