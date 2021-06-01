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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;

namespace CoreCms.Net.Repository
{
    /// <summary>
    ///     提货单表 接口实现
    /// </summary>
    public class CoreCmsBillLadingRepository : BaseRepository<CoreCmsBillLading>, ICoreCmsBillLadingRepository
    {
        public CoreCmsBillLadingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        /// <summary>
        ///     添加提货单
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> AddData(string orderId, int storeId, string name, string mobile)
        {
            var jm = new WebApiCallBack();

            var model = new CoreCmsBillLading();
            model.id = GenerateId();
            model.orderId = orderId;
            model.storeId = storeId;
            model.name = name;
            model.mobile = mobile;
            model.clerkId = 0;
            model.status = false;
            model.createTime = DateTime.Now;
            model.isDel = false;

            //事物处理过程结束
            await DbClient.Insertable(model).ExecuteCommandAsync();
            jm.code = 0;
            jm.msg = "添加成功";

            return jm;
        }


        /// <summary>
        ///     生成唯一提货单号
        /// </summary>
        /// <returns></returns>
        private string GenerateId()
        {
            bool bl;
            string id;
            do
            {
                id = CommonHelper.GetSerialNumberType((int) GlobalEnumVars.SerialNumberType.提货单号);
                var id1 = id;
                bl = DbClient.Queryable<CoreCmsBillLading>().Any(p => p.id == id1);
            } while (bl);

            return id;
        }
    }
}