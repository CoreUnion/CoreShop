/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.Configuration;
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
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
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     用户余额表
    /// </summary>
    [Description("用户余额表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsUserBalanceController : ControllerBase
    {
        private readonly ICoreCmsUserBalanceServices _coreCmsUserBalanceServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsUserBalanceServices"></param>
        public CoreCmsUserBalanceController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsUserBalanceServices coreCmsUserBalanceServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsUserBalanceServices = coreCmsUserBalanceServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsUserBalance/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsUserBalance>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsUserBalance, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;

                case "userId":
                    orderEx = p => p.userId;
                    break;

                case "type":
                    orderEx = p => p.type;
                    break;

                case "money":
                    orderEx = p => p.money;
                    break;

                case "balance":
                    orderEx = p => p.balance;
                    break;

                case "sourceId":
                    orderEx = p => p.sourceId;
                    break;

                case "memo":
                    orderEx = p => p.memo;
                    break;

                case "createTime":
                    orderEx = p => p.createTime;
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
            //用户id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) @where = @where.And(p => p.type == type);
            //资源id nvarchar
            var sourceId = Request.Form["sourceId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sourceId)) @where = @where.And(p => p.sourceId.Contains(sourceId));
            //描述 nvarchar
            var memo = Request.Form["memo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(memo)) @where = @where.And(p => p.memo.Contains(memo));
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

            //获取数据
            var list = await _coreCmsUserBalanceServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion 获取列表============================================================

        #region 首页数据============================================================

        // POST: Api/CoreCmsUserBalance/GetIndex
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

            var userBalanceSourceTypes = EnumHelper.EnumToList<GlobalEnumVars.UserBalanceSourceTypes>();
            jm.data = new
            {
                userBalanceSourceTypes
            };
            return jm;
        }

        #endregion 首页数据============================================================

        #region 预览数据============================================================

        // POST: Api/CoreCmsUserBalance/GetDetails/10
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

            var model = await _coreCmsUserBalanceServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;
            jm.data = model;

            return jm;
        }

        #endregion 预览数据============================================================

        #region 选择导出============================================================

        // POST: Api/CoreCmsUserBalance/SelectExportExcel/10
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
                await _coreCmsUserBalanceServices.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id,
                    OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序列");
            row1.CreateCell(1).SetCellValue("用户id");
            row1.CreateCell(2).SetCellValue("类型");
            row1.CreateCell(3).SetCellValue("金额");
            row1.CreateCell(4).SetCellValue("余额");
            row1.CreateCell(5).SetCellValue("资源id");
            row1.CreateCell(6).SetCellValue("描述");
            row1.CreateCell(7).SetCellValue("创建时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].money.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].balance.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].sourceId);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].memo);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].createTime.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsUserBalance导出(选择结果).xls";
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

        #endregion 选择导出============================================================

        #region 查询导出============================================================

        // POST: Api/CoreCmsUserBalance/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsUserBalance>();
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //用户id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) @where = @where.And(p => p.type == type);
            //资源id nvarchar
            var sourceId = Request.Form["sourceId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sourceId)) @where = @where.And(p => p.sourceId.Contains(sourceId));
            //描述 nvarchar
            var memo = Request.Form["memo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(memo)) @where = @where.And(p => p.memo.Contains(memo));
            //创建时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                var dt = createTime.ObjectToDate();
                where = where.And(p => p.createTime > dt);
            }

            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel =
                await _coreCmsUserBalanceServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序列");
            row1.CreateCell(1).SetCellValue("用户id");
            row1.CreateCell(2).SetCellValue("类型");
            row1.CreateCell(3).SetCellValue("金额");
            row1.CreateCell(4).SetCellValue("余额");
            row1.CreateCell(5).SetCellValue("资源id");
            row1.CreateCell(6).SetCellValue("描述");
            row1.CreateCell(7).SetCellValue("创建时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].money.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].balance.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].sourceId);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].memo);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].createTime.ToString());
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsUserBalance导出(查询结果).xls";
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

        #endregion 查询导出============================================================
    }
}