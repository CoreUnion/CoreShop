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
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     发票表
    /// </summary>
    [Description("发票表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsInvoiceController : ControllerBase
    {
        private readonly ICoreCmsInvoiceServices _coreCmsInvoiceServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsInvoiceServices"></param>
        public CoreCmsInvoiceController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsInvoiceServices coreCmsInvoiceServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsInvoiceServices = coreCmsInvoiceServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsInvoice/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsInvoice>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsInvoice, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "category":
                    orderEx = p => p.category;
                    break;
                case "sourceId":
                    orderEx = p => p.sourceId;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "type":
                    orderEx = p => p.type;
                    break;
                case "title":
                    orderEx = p => p.title;
                    break;
                case "taxNumber":
                    orderEx = p => p.taxNumber;
                    break;
                case "amount":
                    orderEx = p => p.amount;
                    break;
                case "status":
                    orderEx = p => p.status;
                    break;
                case "remarks":
                    orderEx = p => p.remarks;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
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

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) where = where.And(p => p.id == id);
            //开票类型 int
            var category = Request.Form["category"].FirstOrDefault().ObjectToInt(0);
            if (category > 0) where = where.And(p => p.category == category);
            //资源ID nvarchar
            var sourceId = Request.Form["sourceId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sourceId)) where = where.And(p => p.sourceId.Contains(sourceId));
            //所属用户ID int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) where = where.And(p => p.userId == userId);
            //发票类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) where = where.And(p => p.type == type);
            //发票抬头 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title)) where = where.And(p => p.title.Contains(title));
            //发票税号 nvarchar
            var taxNumber = Request.Form["taxNumber"].FirstOrDefault();
            if (!string.IsNullOrEmpty(taxNumber)) where = where.And(p => p.taxNumber.Contains(taxNumber));
            //开票备注 nvarchar
            var remarks = Request.Form["remarks"].FirstOrDefault();
            if (!string.IsNullOrEmpty(remarks)) where = where.And(p => p.remarks.Contains(remarks));
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

            //获取数据
            var list = await _coreCmsInvoiceServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsInvoice/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public AdminUiCallBack GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var orderTaxCategory = EnumHelper.EnumToList<GlobalEnumVars.OrderTaxCategory>();
            var orderTaxType = EnumHelper.EnumToList<GlobalEnumVars.OrderTaxType>();
            var orderTaxStatus = EnumHelper.EnumToList<GlobalEnumVars.OrderTaxStatus>();
            jm.data = new
            {
                orderTaxCategory,
                orderTaxType,
                orderTaxStatus
            };

            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsInvoice/GetEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsInvoiceServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var orderTaxCategory = EnumHelper.EnumToList<GlobalEnumVars.OrderTaxCategory>();
            var orderTaxType = EnumHelper.EnumToList<GlobalEnumVars.OrderTaxType>();
            var orderTaxStatus = EnumHelper.EnumToList<GlobalEnumVars.OrderTaxStatus>();
            jm.code = 0;
            jm.data = new
            {
                model,
                orderTaxCategory,
                orderTaxType,
                orderTaxStatus
            };

            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Api/CoreCmsInvoice/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsInvoice entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsInvoiceServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            //事物处理过程开始
            oldModel.type = entity.type;
            oldModel.title = entity.title;
            oldModel.taxNumber = entity.taxNumber;
            oldModel.amount = entity.amount;
            oldModel.status = entity.status;
            oldModel.remarks = entity.remarks;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await _coreCmsInvoiceServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsInvoice/DoDelete/10
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

            var model = await _coreCmsInvoiceServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var bl = await _coreCmsInvoiceServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        #endregion

        #region 选择导出============================================================

        // POST: Api/CoreCmsInvoice/SelectExportExcel/10
        /// <summary>
        ///     选择导出
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("选择导出")]
        public async Task<AdminUiCallBack> SelectExportExcel([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel = await _coreCmsInvoiceServices.QueryListByClauseAsync(p => entity.id.Contains(p.id),
                p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序列");
            row1.CreateCell(1).SetCellValue("开票类型");
            row1.CreateCell(2).SetCellValue("资源ID");
            row1.CreateCell(3).SetCellValue("所属用户ID");
            row1.CreateCell(4).SetCellValue("发票类型");
            row1.CreateCell(5).SetCellValue("发票抬头");
            row1.CreateCell(6).SetCellValue("发票税号");
            row1.CreateCell(7).SetCellValue("发票金额");
            row1.CreateCell(8).SetCellValue("是否开票");
            row1.CreateCell(9).SetCellValue("开票备注");
            row1.CreateCell(10).SetCellValue("创建时间");
            row1.CreateCell(11).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].category.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].sourceId);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].title);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].taxNumber);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].amount.ToString());
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].remarks);
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsInvoice导出(选择结果).xls";
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

        // POST: Api/CoreCmsInvoice/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsInvoice>();
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) where = where.And(p => p.id == id);
            //开票类型 int
            var category = Request.Form["category"].FirstOrDefault().ObjectToInt(0);
            if (category > 0) where = where.And(p => p.category == category);
            //资源ID nvarchar
            var sourceId = Request.Form["sourceId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sourceId)) where = where.And(p => p.sourceId.Contains(sourceId));
            //所属用户ID int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) where = where.And(p => p.userId == userId);
            //发票类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) where = where.And(p => p.type == type);
            //发票抬头 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title)) where = where.And(p => p.title.Contains(title));
            //发票税号 nvarchar
            var taxNumber = Request.Form["taxNumber"].FirstOrDefault();
            if (!string.IsNullOrEmpty(taxNumber)) where = where.And(p => p.taxNumber.Contains(taxNumber));
            //开票备注 nvarchar
            var remarks = Request.Form["remarks"].FirstOrDefault();
            if (!string.IsNullOrEmpty(remarks)) where = where.And(p => p.remarks.Contains(remarks));
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

            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel = await _coreCmsInvoiceServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序列");
            row1.CreateCell(1).SetCellValue("开票类型");
            row1.CreateCell(2).SetCellValue("资源ID");
            row1.CreateCell(3).SetCellValue("所属用户ID");
            row1.CreateCell(4).SetCellValue("发票类型");
            row1.CreateCell(5).SetCellValue("发票抬头");
            row1.CreateCell(6).SetCellValue("发票税号");
            row1.CreateCell(7).SetCellValue("发票金额");
            row1.CreateCell(8).SetCellValue("是否开票");
            row1.CreateCell(9).SetCellValue("开票备注");
            row1.CreateCell(10).SetCellValue("创建时间");
            row1.CreateCell(11).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].category.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].sourceId);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].title);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].taxNumber);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].amount.ToString());
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].remarks);
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsInvoice导出(查询结果).xls";
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
    }
}