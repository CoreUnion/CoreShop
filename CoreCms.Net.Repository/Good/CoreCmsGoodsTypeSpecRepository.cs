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
using CoreCms.Net.Utility.Helper;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 商品类型属性表 接口实现
    /// </summary>
    public class CoreCmsGoodsTypeSpecRepository : BaseRepository<CoreCmsGoodsTypeSpec>, ICoreCmsGoodsTypeSpecRepository
    {

        public CoreCmsGoodsTypeSpecRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }


        /// <summary>
        ///     使用事务重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> InsertAsync(FmGoodsTypeSpecInsert entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.value == null)
            {
                jm.msg = "请添加属性值"; return jm;
            }

            for (int i = 0; i < entity.value.Count; i++)
            {
                entity.value[i] = entity.value[i].Trim();
                if (GoodsHelper.FilterChar(entity.value[i]) == false) continue;
                jm.msg = "属性值不符合支持规则"; return jm;
            }

            if (entity.value.GroupBy(n => n).Any(c => c.Count() > 1))
            {
                jm.msg = "属性值不允许有相同"; return jm;
            }

            var goodsTypeSpec = new CoreCmsGoodsTypeSpec();
            goodsTypeSpec.name = entity.name;
            goodsTypeSpec.sort = entity.sort;
            var specId = await DbClient.Insertable(goodsTypeSpec).ExecuteReturnIdentityAsync();
            if (specId > 0 && entity.value != null && entity.value.Count > 0)
            {
                var list = new List<CoreCmsGoodsTypeSpecValue>();
                for (var index = 0; index < entity.value.Count; index++)
                {
                    var item = entity.value[index];
                    list.Add(new CoreCmsGoodsTypeSpecValue()
                    {
                        specId = specId,
                        value = item,
                        sort = index + 1
                    });
                }
                var bl = await DbClient.Insertable(list).ExecuteCommandAsync() > 0;
                jm.code = bl ? 0 : 1;
                jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            }


            return jm;
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateAsync(FmGoodsTypeSpecUpdate entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.value == null)
            {
                jm.msg = "请添加属性值"; return jm;
            }
            for (int i = 0; i < entity.value.Count; i++)
            {
                entity.value[i] = entity.value[i].Trim();
                if (GoodsHelper.FilterChar(entity.value[i]) == false) continue;
                jm.msg = "属性值不符合支持规则"; return jm;
            }
            if (entity.value.GroupBy(n => n).Any(c => c.Count() > 1))
            {
                jm.msg = "属性值不允许有相同"; return jm;
            }

            var oldModel = await DbClient.Queryable<CoreCmsGoodsTypeSpec>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.name = entity.name;
            oldModel.sort = entity.sort;
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            if (bl)
            {
                var oldValues = await DbClient.Queryable<CoreCmsGoodsTypeSpecValue>().OrderBy(p => p.sort)
                    .Where(p => p.specId == oldModel.id).ToListAsync();

                //获取需要删除的数据库数据
                var deleteValues = oldValues.Where(p => !entity.value.Contains(p.value)).ToList();
                //删除旧数据
                if (deleteValues.Any()) bl = await DbClient.Deleteable<CoreCmsGoodsTypeSpecValue>(deleteValues).ExecuteCommandHasChangeAsync();

                //新数据
                var values = oldValues.Select(p => p.value).ToList();
                var newValues = entity.value.Except(values).ToList();

                //插入新数据
                if (newValues.Any())
                {
                    var newList = newValues.Select((t, index) => new CoreCmsGoodsTypeSpecValue() { specId = oldModel.id, value = t, sort = oldValues.Count + index }).ToList();
                    bl = await DbClient.Insertable<CoreCmsGoodsTypeSpecValue>(newList).ExecuteCommandAsync() > 0;
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
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsGoodsTypeSpec>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await DbClient.Deleteable<CoreCmsGoodsTypeSpecValue>(p => p.specId == (int)id).ExecuteCommandHasChangeAsync();
            }

            return jm;
        }

    }
}
