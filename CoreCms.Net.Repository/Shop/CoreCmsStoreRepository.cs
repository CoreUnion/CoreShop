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
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 门店表 接口实现
    /// </summary>
    public class CoreCmsStoreRepository : BaseRepository<CoreCmsStore>, ICoreCmsStoreRepository
    {


        public CoreCmsStoreRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsStore entity)
        {
            var jm = new AdminUiCallBack();

            var isDefaultObj = DbClient.Queryable<CoreCmsStore>().Where(p => p.isDefault == true).Any();
            if (isDefaultObj && entity.isDefault == true)
            {
                await DbClient.Updateable<CoreCmsStore>().SetColumns(it => it.isDefault == false).Where(p => p.id > 0).ExecuteCommandAsync(); ;
            }
            else if (!isDefaultObj)
            {
                entity.isDefault = true;
            }
            entity.createTime = DateTime.Now;
            entity.updateTime = DateTime.Now;
            entity.distance = 0;
            if (entity.coordinate.Contains(","))
            {
                var latlong = entity.coordinate.Split(",");
                entity.latitude = latlong[0];
                entity.longitude = latlong[1];
            }

            var id = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync();
            var bl = id > 0;

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsStore entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await DbClient.Queryable<CoreCmsStore>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.storeName = entity.storeName;
            oldModel.mobile = entity.mobile;
            oldModel.linkMan = entity.linkMan;
            oldModel.logoImage = entity.logoImage;
            oldModel.areaId = entity.areaId;
            oldModel.address = entity.address;
            oldModel.coordinate = entity.coordinate;
            oldModel.latitude = entity.latitude;
            oldModel.longitude = entity.longitude;
            oldModel.updateTime = entity.updateTime;
            oldModel.isDefault = entity.isDefault;

            if (entity.coordinate.Contains(","))
            {
                var latlong = entity.coordinate.Split(",");
                oldModel.latitude = latlong[0];
                oldModel.longitude = latlong[1];
            }

            var isDefaultObj = DbClient.Queryable<CoreCmsStore>().Where(p => p.isDefault == true).Any();
            if (isDefaultObj && entity.isDefault == true)
            {
                await DbClient.Updateable<CoreCmsStore>().SetColumns(it => it.isDefault == false).Where(p => p.id > 0).ExecuteCommandAsync();
            }
            else if (!isDefaultObj)
            {
                oldModel.isDefault = true;
            }

            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }


        #region Sql根据条件查询分页数据带距离

        /// <summary>
        ///     Sql根据条件查询分页数据带距离
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">精度</param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsStore>> QueryPageAsyncByCoordinate(Expression<Func<CoreCmsStore, bool>> predicate,
            Expression<Func<CoreCmsStore, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, decimal latitude = 0, decimal longitude = 0)
        {
            RefAsync<int> totalCount = 0;

            //MySql与SqlServer查询语句相同
            List<CoreCmsStore> page;
            if (latitude > 0 && longitude > 0)
            {
                var sqrt = "SQRT(power(SIN((" + latitude + "*PI()/180-(CoreCmsStore.latitude)*PI()/180)/2),2)+COS(" + latitude + "*PI()/180)*COS((CoreCmsStore.latitude)*PI()/180)*power(SIN((" + longitude + "*PI()/180-(CoreCmsStore.longitude)*PI()/180)/2),2))";
                var sql = "SELECT id, storeName, mobile, linkMan, logoImage, areaId, address, coordinate, latitude, longitude, isDefault, createTime, updateTime, ROUND(6378.138*2*ASIN(" + sqrt + ")*1000,2)  AS distance FROM CoreCmsStore";

                page = await DbClient.SqlQueryable<CoreCmsStore>(sql)
                    .WhereIF(predicate != null, predicate)
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .Select(p => new CoreCmsStore
                    {
                        id = p.id,
                        storeName = p.storeName,
                        mobile = p.mobile,
                        linkMan = p.linkMan,
                        logoImage = p.logoImage,
                        areaId = p.areaId,
                        address = p.address,
                        coordinate = p.coordinate,
                        latitude = p.latitude,
                        longitude = p.longitude,
                        isDefault = p.isDefault,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        distance = Convert.ToDecimal(p.distance)
                    }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsStore>()
                    .WhereIF(predicate != null, predicate)
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .Select(p => new CoreCmsStore
                    {
                        id = p.id,
                        storeName = p.storeName,
                        mobile = p.mobile,
                        linkMan = p.linkMan,
                        logoImage = p.logoImage,
                        areaId = p.areaId,
                        address = p.address,
                        coordinate = p.coordinate,
                        latitude = p.latitude,
                        longitude = p.longitude,
                        isDefault = p.isDefault,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        distance = Convert.ToDecimal(p.distance)
                    }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }

            var list = new PageList<CoreCmsStore>(page, pageIndex, pageSize, totalCount);
            return list;
        }
        #endregion


        #region 根据用户序列获取单个门店数据

        /// <summary>
        ///     根据用户序列获取单个门店数据
        /// </summary>
        /// <param name="userId">用户序列</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<CoreCmsStore> GetStoreByUserId(int userId, bool blUseNoLock = false)
        {
            CoreCmsStore obj;
            if (blUseNoLock)
            {
                obj = await DbClient.Queryable<CoreCmsStore, CoreCmsClerk>((p, clerks) => new JoinQueryInfos(
                         JoinType.Left, p.id == clerks.storeId
                         ))
                    .Where((p, clerks) => clerks.userId == userId)
                    .Select((p, clerks) => new CoreCmsStore
                    {
                        id = p.id,
                        storeName = p.storeName,
                        mobile = p.mobile,
                        linkMan = p.linkMan,
                        logoImage = p.logoImage,
                        areaId = p.areaId,
                        address = p.address,
                        coordinate = p.coordinate,
                        latitude = p.latitude,
                        longitude = p.longitude,
                        isDefault = p.isDefault,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                    }).With(SqlWith.NoLock)
                .FirstAsync();
            }
            else
            {
                obj = await DbClient.Queryable<CoreCmsStore, CoreCmsClerk>((p, clerks) => new JoinQueryInfos(
                        JoinType.Left, p.id == clerks.storeId
                    ))
                    .Where((p, clerks) => clerks.userId == userId)
                    .Select((p, clerks) => new CoreCmsStore
                    {
                        id = p.id,
                        storeName = p.storeName,
                        mobile = p.mobile,
                        linkMan = p.linkMan,
                        logoImage = p.logoImage,
                        areaId = p.areaId,
                        address = p.address,
                        coordinate = p.coordinate,
                        latitude = p.latitude,
                        longitude = p.longitude,
                        isDefault = p.isDefault,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                    })
                    .FirstAsync();

            }
            return obj;
        }
        #endregion





    }
}
