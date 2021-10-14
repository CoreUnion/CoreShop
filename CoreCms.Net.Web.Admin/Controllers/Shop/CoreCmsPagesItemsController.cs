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
    ///     单页内容
    /// </summary>
    [Description("单页内容")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsPagesItemsController : ControllerBase
    {
        private readonly ICoreCmsPagesItemsServices _coreCmsPagesItemsServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsPagesItemsServices"></param>
        public CoreCmsPagesItemsController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsPagesItemsServices coreCmsPagesItemsServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsPagesItemsServices = coreCmsPagesItemsServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsPagesItems/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsPagesItems>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsPagesItems, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "widgetCode":
                    orderEx = p => p.widgetCode;
                    break;
                case "pageCode":
                    orderEx = p => p.pageCode;
                    break;
                case "positionId":
                    orderEx = p => p.positionId;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "parameters":
                    orderEx = p => p.parameters;
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

            // int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //组件编码 nvarchar
            var widgetCode = Request.Form["widgetCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(widgetCode)) @where = @where.And(p => p.widgetCode.Contains(widgetCode));
            //页面编码 nvarchar
            var pageCode = Request.Form["pageCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(pageCode)) @where = @where.And(p => p.pageCode.Contains(pageCode));
            //布局位置 int
            var positionId = Request.Form["positionId"].FirstOrDefault().ObjectToInt(0);
            if (positionId > 0) @where = @where.And(p => p.positionId == positionId);
            //排序，越小越靠前 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0) @where = @where.And(p => p.sort == sort);
            //组件配置内容 nvarchar
            var parameters = Request.Form["parameters"].FirstOrDefault();
            if (!string.IsNullOrEmpty(parameters)) @where = @where.And(p => p.parameters.Contains(parameters));
            //获取数据
            var list = await _coreCmsPagesItemsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsPagesItems/GetIndex
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
            return jm;
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/CoreCmsPagesItems/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public AdminUiCallBack GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsPagesItems/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsPagesItems entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _coreCmsPagesItemsServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsPagesItems/GetEdit
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

            var model = await _coreCmsPagesItemsServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;
            jm.data = model;

            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Api/CoreCmsPagesItems/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsPagesItems entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsPagesItemsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            //事物处理过程开始
            oldModel.widgetCode = entity.widgetCode;
            oldModel.pageCode = entity.pageCode;
            oldModel.positionId = entity.positionId;
            oldModel.sort = entity.sort;
            oldModel.parameters = entity.parameters;

            //事物处理过程结束
            var bl = await _coreCmsPagesItemsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsPagesItems/DoDelete/10
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

            var model = await _coreCmsPagesItemsServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var bl = await _coreCmsPagesItemsServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }

        #endregion

        #region 批量删除============================================================

        // POST: Api/CoreCmsPagesItems/DoBatchDelete/10,11,20
        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<AdminUiCallBack> DoBatchDelete([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _coreCmsPagesItemsServices.DeleteByIdsAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/CoreCmsPagesItems/GetDetails/10
        /// <summary>
        ///     预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPagesItemsServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;
            jm.data = model;

            return jm;
        }

        #endregion

        #region 选择导出============================================================

        // POST: Api/CoreCmsPagesItems/SelectExportExcel/10
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
            var listmodel = await _coreCmsPagesItemsServices.QueryListByClauseAsync(p => entity.id.Contains(p.id),
                p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("");
            row1.CreateCell(1).SetCellValue("组件编码");
            row1.CreateCell(2).SetCellValue("页面编码");
            row1.CreateCell(3).SetCellValue("布局位置");
            row1.CreateCell(4).SetCellValue("排序，越小越靠前");
            row1.CreateCell(5).SetCellValue("组件配置内容");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].widgetCode);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].pageCode);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].positionId.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].sort.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].parameters);
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsPagesItems导出(选择结果).xls";
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

        // POST: Api/CoreCmsPagesItems/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsPagesItems>();
            //查询筛选

            // int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //组件编码 nvarchar
            var widgetCode = Request.Form["widgetCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(widgetCode)) @where = @where.And(p => p.widgetCode.Contains(widgetCode));
            //页面编码 nvarchar
            var pageCode = Request.Form["pageCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(pageCode)) @where = @where.And(p => p.pageCode.Contains(pageCode));
            //布局位置 int
            var positionId = Request.Form["positionId"].FirstOrDefault().ObjectToInt(0);
            if (positionId > 0) @where = @where.And(p => p.positionId == positionId);
            //排序，越小越靠前 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0) @where = @where.And(p => p.sort == sort);
            //组件配置内容 nvarchar
            var parameters = Request.Form["parameters"].FirstOrDefault();
            if (!string.IsNullOrEmpty(parameters)) @where = @where.And(p => p.parameters.Contains(parameters));
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel =
                await _coreCmsPagesItemsServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("");
            row1.CreateCell(1).SetCellValue("组件编码");
            row1.CreateCell(2).SetCellValue("页面编码");
            row1.CreateCell(3).SetCellValue("布局位置");
            row1.CreateCell(4).SetCellValue("排序，越小越靠前");
            row1.CreateCell(5).SetCellValue("组件配置内容");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].widgetCode);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].pageCode);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].positionId.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].sort.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].parameters);
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsPagesItems导出(查询结果).xls";
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