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
    ///     等级佣金表
    /// </summary>
    [Description("等级佣金表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsDistributionResultController : ControllerBase
    {
        private readonly ICoreCmsDistributionResultServices _coreCmsDistributionResultServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsDistributionResultController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsDistributionResultServices coreCmsDistributionResultServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsDistributionResultServices = coreCmsDistributionResultServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsDistributionResult/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsDistributionResult>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsDistributionResult, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "gradeId":
                    orderEx = p => p.gradeId;
                    break;
                case "code":
                    orderEx = p => p.code;
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

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //会员等级Id int
            var gradeId = Request.Form["gradeId"].FirstOrDefault().ObjectToInt(0);
            if (gradeId > 0) @where = @where.And(p => p.gradeId == gradeId);
            //佣金编码 nvarchar
            var code = Request.Form["code"].FirstOrDefault();
            if (!string.IsNullOrEmpty(code)) @where = @where.And(p => p.code.Contains(code));
            //佣金设置序列化参数 nvarchar
            var parameters = Request.Form["parameters"].FirstOrDefault();
            if (!string.IsNullOrEmpty(parameters)) @where = @where.And(p => p.parameters.Contains(parameters));
            //获取数据
            var list = await _coreCmsDistributionResultServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent,
                pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsDistributionResult/GetIndex
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

        // POST: Api/CoreCmsDistributionResult/GetCreate
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

        // POST: Api/CoreCmsDistributionResult/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsDistributionResult entity)
        {
            var jm = await _coreCmsDistributionResultServices.InsertAsync(entity);
            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsDistributionResult/GetEdit
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

            var model = await _coreCmsDistributionResultServices.QueryByIdAsync(entity.id);
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

        // POST: Api/CoreCmsDistributionResult/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsDistributionResult entity)
        {
            var jm = await _coreCmsDistributionResultServices.UpdateAsync(entity);
            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsDistributionResult/DoDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = await _coreCmsDistributionResultServices.DeleteByIdAsync(entity.id);
            return jm;
        }

        #endregion

        #region 批量删除============================================================

        // POST: Api/CoreCmsDistributionResult/DoBatchDelete/10,11,20
        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<AdminUiCallBack> DoBatchDelete([FromBody] FMArrayIntIds entity)
        {
            var jm = await _coreCmsDistributionResultServices.DeleteByIdsAsync(entity.id);
            return jm;
        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/CoreCmsDistributionResult/GetDetails/10
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

            var model = await _coreCmsDistributionResultServices.QueryByIdAsync(entity.id);
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

        // POST: Api/CoreCmsDistributionResult/SelectExportExcel/10
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
            var listmodel =
                await _coreCmsDistributionResultServices.QueryListByClauseAsync(p => entity.id.Contains(p.id),
                    p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序列");
            row1.CreateCell(1).SetCellValue("会员等级Id");
            row1.CreateCell(2).SetCellValue("佣金编码");
            row1.CreateCell(3).SetCellValue("佣金设置序列化参数");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].gradeId.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].code);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].parameters);
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsDistributionResult导出(选择结果).xls";
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

        // POST: Api/CoreCmsDistributionResult/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsDistributionResult>();
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //会员等级Id int
            var gradeId = Request.Form["gradeId"].FirstOrDefault().ObjectToInt(0);
            if (gradeId > 0) @where = @where.And(p => p.gradeId == gradeId);
            //佣金编码 nvarchar
            var code = Request.Form["code"].FirstOrDefault();
            if (!string.IsNullOrEmpty(code)) @where = @where.And(p => p.code.Contains(code));
            //佣金设置序列化参数 nvarchar
            var parameters = Request.Form["parameters"].FirstOrDefault();
            if (!string.IsNullOrEmpty(parameters)) @where = @where.And(p => p.parameters.Contains(parameters));
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel =
                await _coreCmsDistributionResultServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序列");
            row1.CreateCell(1).SetCellValue("会员等级Id");
            row1.CreateCell(2).SetCellValue("佣金编码");
            row1.CreateCell(3).SetCellValue("佣金设置序列化参数");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].gradeId.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].code);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].parameters);
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsDistributionResult导出(查询结果).xls";
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