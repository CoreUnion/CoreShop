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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     提货单表
    /// </summary>
    [Description("提货单表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsBillLadingController : ControllerBase
    {
        private readonly ICoreCmsBillLadingServices _coreCmsBillLadingServices;
        private readonly ICoreCmsStoreServices _storeServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsBillLadingController(
            IWebHostEnvironment webHostEnvironment
            , ICoreCmsBillLadingServices coreCmsBillLadingServices
            , ICoreCmsStoreServices storeServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsBillLadingServices = coreCmsBillLadingServices;
            _storeServices = storeServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsBillLading/GetPageList
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsBillLading>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsBillLading, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "orderId":
                    orderEx = p => p.orderId;
                    break;
                case "storeId":
                    orderEx = p => p.storeId;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "mobile":
                    orderEx = p => p.mobile;
                    break;
                case "clerkId":
                    orderEx = p => p.clerkId;
                    break;
                case "pickUpTime":
                    orderEx = p => p.pickUpTime;
                    break;
                case "status":
                    orderEx = p => p.status;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                case "isDel":
                    orderEx = p => p.isDel;
                    break;
                default:
                    orderEx = p => p.id;
                    break;
            }

            //设置排序方式
            var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
            var orderBy = orderDirection switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };
            //查询筛选

            //提货单号 nvarchar
            var id = Request.Form["id"].FirstOrDefault();
            if (!string.IsNullOrEmpty(id)) @where = @where.And(p => p.id.Contains(id));
            //订单号 nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId)) @where = @where.And(p => p.orderId.Contains(orderId));
            //提货门店ID int
            var storeId = Request.Form["storeId"].FirstOrDefault().ObjectToInt(0);
            if (storeId > 0) @where = @where.And(p => p.storeId == storeId);
            //提货人姓名 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //提货手机号 nvarchar
            var mobile = Request.Form["mobile"].FirstOrDefault();
            if (!string.IsNullOrEmpty(mobile)) @where = @where.And(p => p.mobile.Contains(mobile));
            //处理店员ID int
            var clerkId = Request.Form["clerkId"].FirstOrDefault().ObjectToInt(0);
            if (clerkId > 0) @where = @where.And(p => p.clerkId == clerkId);
            //提货时间 datetime
            var pickUpTime = Request.Form["pickUpTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(pickUpTime))
            {
                if (pickUpTime.Contains("到"))
                {
                    var dts = pickUpTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.pickUpTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.pickUpTime < dtEnd);
                }
                else
                {
                    var dt = pickUpTime.ObjectToDate();
                    where = where.And(p => p.pickUpTime > dt);
                }
            }

            //是否提货 bit
            var status = Request.Form["status"].FirstOrDefault();
            if (!string.IsNullOrEmpty(status) && status.ToLowerInvariant() == "true")
                @where = @where.And(p => p.status);
            else if (!string.IsNullOrEmpty(status) && status.ToLowerInvariant() == "false")
                @where = @where.And(p => p.status == false);
            //创建时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                if (createTime.Contains("到"))
                {
                    var dts = createTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.createTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.createTime < dtEnd);
                }
                else
                {
                    var dt = createTime.ObjectToDate();
                    where = where.And(p => p.createTime > dt);
                }
            }

            //更新时间 datetime
            var updateTime = Request.Form["updateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updateTime))
            {
                if (updateTime.Contains("到"))
                {
                    var dts = updateTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime < dtEnd);
                }
                else
                {
                    var dt = updateTime.ObjectToDate();
                    where = where.And(p => p.updateTime > dt);
                }
            }

            //删除时间 bit
            var isDel = Request.Form["isDel"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDel) && isDel.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isDel);
            else if (!string.IsNullOrEmpty(isDel) && isDel.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isDel == false);
            //获取数据
            var list = await _coreCmsBillLadingServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsBillLading/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public async Task<AdminUiCallBack> GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            var stores = await _storeServices.QueryAsync();
            jm.data = new
            {
                stores
            };
            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsBillLading/GetEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsBillLadingServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var stores = await _storeServices.QueryAsync();
            jm.code = 0;
            jm.data = new
            {
                model,
                stores
            };

            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Admins/CoreCmsBillLading/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsBillLading entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsBillLadingServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            //事物处理过程开始
            oldModel.storeId = entity.storeId;
            oldModel.name = entity.name;
            oldModel.mobile = entity.mobile;
            //事物处理过程结束

            var bl = await _coreCmsBillLadingServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsBillLading/DoDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsBillLadingServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var bl = await _coreCmsBillLadingServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }

        #endregion

        #region 选择导出============================================================

        // POST: Api/CoreCmsBillLading/SelectExportExcel/10
        /// <summary>
        ///     选择导出
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("选择导出")]
        public async Task<AdminUiCallBack> SelectExportExcel([FromBody] FMArrayStringIds entity)
        {
            var jm = new AdminUiCallBack();

            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel = await _coreCmsBillLadingServices.QueryListByClauseAsync(p => entity.id.Contains(p.id),
                p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("提货单号");
            row1.CreateCell(1).SetCellValue("订单号");
            row1.CreateCell(2).SetCellValue("提货门店ID");
            row1.CreateCell(3).SetCellValue("提货人姓名");
            row1.CreateCell(4).SetCellValue("提货手机号");
            row1.CreateCell(5).SetCellValue("处理店员ID");
            row1.CreateCell(6).SetCellValue("提货时间");
            row1.CreateCell(7).SetCellValue("是否提货");
            row1.CreateCell(8).SetCellValue("创建时间");
            row1.CreateCell(9).SetCellValue("更新时间");
            row1.CreateCell(10).SetCellValue("删除时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id);
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].orderId);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].storeId.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].name);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].mobile);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].clerkId.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].pickUpTime.ToString());
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].updateTime.ToString());
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].isDel.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillLading导出(选择结果).xls";
            var filePath = webRootPath + tpath;
            var di = new DirectoryInfo(filePath);
            if (!di.Exists) di.Create();
            var fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return jm;
        }

        #endregion

        #region 查询导出============================================================

        // POST: Api/CoreCmsBillLading/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsBillLading>();
            //查询筛选

            //提货单号 nvarchar
            var id = Request.Form["id"].FirstOrDefault();
            if (!string.IsNullOrEmpty(id)) @where = @where.And(p => p.id.Contains(id));
            //订单号 nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId)) @where = @where.And(p => p.orderId.Contains(orderId));
            //提货门店ID int
            var storeId = Request.Form["storeId"].FirstOrDefault().ObjectToInt(0);
            if (storeId > 0) @where = @where.And(p => p.storeId == storeId);
            //提货人姓名 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //提货手机号 nvarchar
            var mobile = Request.Form["mobile"].FirstOrDefault();
            if (!string.IsNullOrEmpty(mobile)) @where = @where.And(p => p.mobile.Contains(mobile));
            //处理店员ID int
            var clerkId = Request.Form["clerkId"].FirstOrDefault().ObjectToInt(0);
            if (clerkId > 0) @where = @where.And(p => p.clerkId == clerkId);
            //提货时间 datetime
            var pickUpTime = Request.Form["pickUpTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(pickUpTime))
            {
                var dt = pickUpTime.ObjectToDate();
                where = where.And(p => p.pickUpTime > dt);
            }

            //是否提货 bit
            var status = Request.Form["status"].FirstOrDefault();
            if (!string.IsNullOrEmpty(status) && status.ToLowerInvariant() == "true")
                @where = @where.And(p => p.status);
            else if (!string.IsNullOrEmpty(status) && status.ToLowerInvariant() == "false")
                @where = @where.And(p => p.status == false);
            //创建时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                var dt = createTime.ObjectToDate();
                where = where.And(p => p.createTime > dt);
            }

            //更新时间 datetime
            var updateTime = Request.Form["updateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updateTime))
            {
                var dt = updateTime.ObjectToDate();
                where = where.And(p => p.updateTime > dt);
            }

            //删除时间 bit
            var isDel = Request.Form["isDel"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDel) && isDel.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isDel);
            else if (!string.IsNullOrEmpty(isDel) && isDel.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isDel == false);
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel =
                await _coreCmsBillLadingServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("提货单号");
            row1.CreateCell(1).SetCellValue("订单号");
            row1.CreateCell(2).SetCellValue("提货门店ID");
            row1.CreateCell(3).SetCellValue("提货人姓名");
            row1.CreateCell(4).SetCellValue("提货手机号");
            row1.CreateCell(5).SetCellValue("处理店员ID");
            row1.CreateCell(6).SetCellValue("提货时间");
            row1.CreateCell(7).SetCellValue("是否提货");
            row1.CreateCell(8).SetCellValue("创建时间");
            row1.CreateCell(9).SetCellValue("更新时间");
            row1.CreateCell(10).SetCellValue("删除时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id);
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].orderId);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].storeId.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].name);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].mobile);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].clerkId.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].pickUpTime.ToString());
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].updateTime.ToString());
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].isDel.ToString());
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillLading导出(查询结果).xls";
            var filePath = webRootPath + tpath;
            var di = new DirectoryInfo(filePath);
            if (!di.Exists) di.Create();
            var fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return jm;
        }

        #endregion

        #region 核销数据============================================================

        // POST: Api/CoreCmsBillLading/LadingOperating/10
        /// <summary>
        ///     核销数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("核销数据")]
        public async Task<AdminUiCallBack> LadingOperating([FromBody] FMStringId entity)
        {
            var ids = entity.id.Split(",");
            var jm = await _coreCmsBillLadingServices.LadingOperating(ids);

            return jm;
        }

        #endregion


        #region 批量核销数据============================================================

        // POST: Api/CoreCmsBillLading/LadingOperating/10
        /// <summary>
        ///     批量核销数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("核销数据")]
        public async Task<AdminUiCallBack> BatchLadingOperating([FromBody] FMArrayStringIds entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.id.Length == 0)
            {
                jm.msg = "请选择需要核销的数据";
                return jm;
            }

            jm = await _coreCmsBillLadingServices.LadingOperating(entity.id);

            return jm;
        }

        #endregion
    }
}