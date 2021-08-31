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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 用户对表的提交记录 接口实现
    /// </summary>
    public class CoreCmsFormSubmitRepository : BaseRepository<CoreCmsFormSubmit>, ICoreCmsFormSubmitRepository
    {
        public CoreCmsFormSubmitRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        #region 实现重写增删改查操作==========================================================


        /// <summary>
        /// 重写异步插入方法并返回自增值
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public async Task<int> InsertReturnIdentityAsync(CoreCmsFormSubmit entity)
        {
            return await DbClient.Insertable(entity).ExecuteReturnIdentityAsync();
        }



        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsFormSubmit entity)
        {
            var jm = new AdminUiCallBack();

            var id = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync();
            var bl = id > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            jm.count = id;


            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsFormSubmit entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await DbClient.Queryable<CoreCmsFormSubmit>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.formId = entity.formId;
            oldModel.formName = entity.formName;
            oldModel.userId = entity.userId;
            oldModel.money = entity.money;
            oldModel.payStatus = entity.payStatus;
            oldModel.status = entity.status;
            oldModel.feedback = entity.feedback;
            oldModel.ip = entity.ip;
            oldModel.createTime = entity.createTime;
            oldModel.updateTime = entity.updateTime;

            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsFormSubmit> entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsFormSubmit>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

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

            var bl = await DbClient.Deleteable<CoreCmsFormSubmit>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
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
        public new async Task<IPageList<CoreCmsFormSubmit>> QueryPageAsync(Expression<Func<CoreCmsFormSubmit, bool>> predicate,
            Expression<Func<CoreCmsFormSubmit, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsFormSubmit> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsFormSubmit, CoreCmsUser>((p, sc) => new JoinQueryInfos(
                        JoinType.Left, p.userId == sc.id))
                        .Select((p, sc) => new CoreCmsFormSubmit
                        {
                            id = p.id,
                            formId = p.formId,
                            formName = p.formName,
                            userId = p.userId,
                            money = p.money,
                            payStatus = p.payStatus,
                            status = p.status,
                            feedback = p.feedback,
                            ip = p.ip,
                            createTime = p.createTime,
                            updateTime = p.updateTime,
                            userName = sc.nickName,
                            avatarImage = sc.avatarImage
                        })
                        .With(SqlWith.NoLock)
                    .MergeTable()
                        .WhereIF(predicate != null, predicate)
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)

                        .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsFormSubmit, CoreCmsUser>((p, sc) => new JoinQueryInfos(
                        JoinType.Left, p.userId == sc.id))
                    .Select((p, sc) => new CoreCmsFormSubmit
                    {
                        id = p.id,
                        formId = p.formId,
                        formName = p.formName,
                        userId = p.userId,
                        money = p.money,
                        payStatus = p.payStatus,
                        status = p.status,
                        feedback = p.feedback,
                        ip = p.ip,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        userName = sc.nickName,
                        avatarImage = sc.avatarImage
                    })
                .MergeTable()
                .WhereIF(predicate != null, predicate)
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsFormSubmit>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion



        #region 表单支付

        /// <summary>
        /// 表单支付
        /// </summary>
        /// <param name="id">序列</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Pay(int id)
        {
            var jm = new WebApiCallBack();


            if (id == 0)
            {
                jm.msg = "支付失败";
                return jm;
            }

            await DbClient.Updateable<CoreCmsFormSubmit>().SetColumns(p => p.payStatus == true).Where(p => p.id == id && p.payStatus == false)
                .ExecuteCommandAsync();
            jm.status = true;
            jm.data = jm.msg = "支付成功";

            return jm;
        }

        #endregion


        /// <summary>
        /// 获取表单的统计数据
        /// </summary>
        /// <param name="formId">表单序列</param>
        /// <param name="day">多少天内的数据</param>
        /// <returns></returns>
        public async Task<FormStatisticsViewDto> GetStatisticsByFormid(int formId, int day)
        {
            var dt = DateTime.Now;
            var whereDt = dt.AddDays(-day);

            var list = await DbClient.Queryable<CoreCmsFormSubmit>()
                .Where(p => p.formId == formId)
                .WhereIF(day > 0, p => p.createTime >= whereDt)
                .GroupBy(p => new { p.createTime, p.formId })
                .Select(p => new FormStatisticsDto()
                {
                    day = SqlFunc.MappingColumn(p.createTime, "CONVERT(varchar(100), createTime, 111)").ToString(),
                    nums = SqlFunc.AggregateCount(p.id),
                    formId = p.formId
                })
                .OrderBy(p => p.day)
                .With(SqlWith.NoLock)
                .ToListAsync();

            var num = day - 1;
            var days = new int[day];
            var d = new String[day];

            for (int i = 0; i <= num; i++)
            {
                var j = num - i;
                var iDt = dt.AddDays(-j).ToString("yyyy/MM/dd");

                d[i] = iDt;

                var result = list.Find(p => p.day == iDt);
                if (result != null)
                {
                    days[i] = result.nums;
                }
                else
                {
                    days[i] = 0;
                }
            }

            var obj = new FormStatisticsViewDto { data = days, day = d, formId = formId };


            return obj;
        }

    }
}
