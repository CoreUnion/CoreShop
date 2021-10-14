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
    ///     Nlog记录表
    /// </summary>
    [Description("Nlog记录表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class SysNLogRecordsController : ControllerBase
    {
        private readonly ISysNLogRecordsServices _sysNLogRecordsServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public SysNLogRecordsController(IWebHostEnvironment webHostEnvironment
            , ISysNLogRecordsServices sysNLogRecordsServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _sysNLogRecordsServices = sysNLogRecordsServices;
        }

        #region 获取列表============================================================

        // POST: Api/SysNLogRecords/GetPageList
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
            var where = PredicateBuilder.True<SysNLogRecords>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<SysNLogRecords, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "LogDate":
                    orderEx = p => p.LogDate;
                    break;
                case "LogLevel":
                    orderEx = p => p.LogLevel;
                    break;
                case "LogType":
                    orderEx = p => p.LogType;
                    break;
                case "Logger":
                    orderEx = p => p.Logger;
                    break;
                case "Message":
                    orderEx = p => p.Message;
                    break;
                case "MachineName":
                    orderEx = p => p.MachineName;
                    break;
                case "MachineIp":
                    orderEx = p => p.MachineIp;
                    break;
                case "NetRequestMethod":
                    orderEx = p => p.NetRequestMethod;
                    break;
                case "NetRequestUrl":
                    orderEx = p => p.NetRequestUrl;
                    break;
                case "NetUserIsauthenticated":
                    orderEx = p => p.NetUserIsauthenticated;
                    break;
                case "NetUserAuthtype":
                    orderEx = p => p.NetUserAuthtype;
                    break;
                case "NetUserIdentity":
                    orderEx = p => p.NetUserIdentity;
                    break;
                case "Exception":
                    orderEx = p => p.Exception;
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
            //时间 datetime
            var LogDate = Request.Form["LogDate"].FirstOrDefault();
            if (!string.IsNullOrEmpty(LogDate))
            {
                if (LogDate.Contains("到"))
                {
                    var dts = LogDate.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.LogDate > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.LogDate < dtEnd);
                }
                else
                {
                    var dt = LogDate.ObjectToDate();
                    where = where.And(p => p.LogDate > dt);
                }
            }

            //级别 nvarchar
            var LogLevel = Request.Form["LogLevel"].FirstOrDefault();
            if (!string.IsNullOrEmpty(LogLevel)) @where = @where.And(p => p.LogLevel.Contains(LogLevel));
            //事件日志上下文 nvarchar
            var LogType = Request.Form["LogType"].FirstOrDefault();
            if (!string.IsNullOrEmpty(LogType)) @where = @where.And(p => p.LogType.Contains(LogType));
            //记录器名字 nvarchar
            var Logger = Request.Form["Logger"].FirstOrDefault();
            if (!string.IsNullOrEmpty(Logger)) @where = @where.And(p => p.Logger.Contains(Logger));
            //消息 nvarchar
            var Message = Request.Form["Message"].FirstOrDefault();
            if (!string.IsNullOrEmpty(Message)) @where = @where.And(p => p.Message.Contains(Message));
            //名称 nvarchar
            var MachineName = Request.Form["MachineName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(MachineName)) @where = @where.And(p => p.MachineName.Contains(MachineName));
            //ip nvarchar
            var MachineIp = Request.Form["MachineIp"].FirstOrDefault();
            if (!string.IsNullOrEmpty(MachineIp)) @where = @where.And(p => p.MachineIp.Contains(MachineIp));
            //请求方式 nvarchar
            var NetRequestMethod = Request.Form["NetRequestMethod"].FirstOrDefault();
            if (!string.IsNullOrEmpty(NetRequestMethod))
                @where = @where.And(p => p.NetRequestMethod.Contains(NetRequestMethod));
            //请求地址 nvarchar
            var NetRequestUrl = Request.Form["NetRequestUrl"].FirstOrDefault();
            if (!string.IsNullOrEmpty(NetRequestUrl)) @where = @where.And(p => p.NetRequestUrl.Contains(NetRequestUrl));
            //是否授权 nvarchar
            var NetUserIsauthenticated = Request.Form["NetUserIsauthenticated"].FirstOrDefault();
            if (!string.IsNullOrEmpty(NetUserIsauthenticated))
                @where = @where.And(p => p.NetUserIsauthenticated.Contains(NetUserIsauthenticated));
            //授权类型 nvarchar
            var NetUserAuthtype = Request.Form["NetUserAuthtype"].FirstOrDefault();
            if (!string.IsNullOrEmpty(NetUserAuthtype))
                @where = @where.And(p => p.NetUserAuthtype.Contains(NetUserAuthtype));
            //身份认证 nvarchar
            var NetUserIdentity = Request.Form["NetUserIdentity"].FirstOrDefault();
            if (!string.IsNullOrEmpty(NetUserIdentity))
                @where = @where.And(p => p.NetUserIdentity.Contains(NetUserIdentity));
            //异常信息 nvarchar
            var Exception = Request.Form["Exception"].FirstOrDefault();
            if (!string.IsNullOrEmpty(Exception)) @where = @where.And(p => p.Exception.Contains(Exception));
            //获取数据
            var list = await _sysNLogRecordsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/SysNLogRecords/GetIndex
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

        #region 删除数据============================================================

        // POST: Api/SysNLogRecords/DoDelete/10
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

            var model = await _sysNLogRecordsServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var bl = await _sysNLogRecordsServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }

        #endregion

        #region 批量删除============================================================

        // POST: Api/SysNLogRecords/DoBatchDelete/10,11,20
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

            var bl = await _sysNLogRecordsServices.DeleteByIdsAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/SysNLogRecords/GetDetails/10
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

            var model = await _sysNLogRecordsServices.QueryByIdAsync(entity.id);
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

        // POST: Api/SysNLogRecords/SelectExportExcel/10
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
            var listmodel = await _sysNLogRecordsServices.QueryListByClauseAsync(p => entity.id.Contains(p.id),
                p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序列");
            row1.CreateCell(1).SetCellValue("时间");
            row1.CreateCell(2).SetCellValue("级别");
            row1.CreateCell(3).SetCellValue("事件日志上下文");
            row1.CreateCell(4).SetCellValue("记录器名字");
            row1.CreateCell(5).SetCellValue("消息");
            row1.CreateCell(6).SetCellValue("名称");
            row1.CreateCell(7).SetCellValue("ip");
            row1.CreateCell(8).SetCellValue("请求方式");
            row1.CreateCell(9).SetCellValue("请求地址");
            row1.CreateCell(10).SetCellValue("是否授权");
            row1.CreateCell(11).SetCellValue("授权类型");
            row1.CreateCell(12).SetCellValue("身份认证");
            row1.CreateCell(13).SetCellValue("异常信息");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].LogDate.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].LogLevel);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].LogType);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].Logger);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].Message);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].MachineName);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].MachineIp);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].NetRequestMethod);
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].NetRequestUrl);
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].NetUserIsauthenticated);
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].NetUserAuthtype);
                rowtemp.CreateCell(12).SetCellValue(listmodel[i].NetUserIdentity);
                rowtemp.CreateCell(13).SetCellValue(listmodel[i].Exception);
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-SysNLogRecords导出(选择结果).xls";
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

        // POST: Api/SysNLogRecords/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<SysNLogRecords>();
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //时间 datetime
            var LogDate = Request.Form["LogDate"].FirstOrDefault();
            if (!string.IsNullOrEmpty(LogDate))
            {
                var dt = LogDate.ObjectToDate();
                where = where.And(p => p.LogDate > dt);
            }

            //级别 nvarchar
            var LogLevel = Request.Form["LogLevel"].FirstOrDefault();
            if (!string.IsNullOrEmpty(LogLevel)) @where = @where.And(p => p.LogLevel.Contains(LogLevel));
            //事件日志上下文 nvarchar
            var LogType = Request.Form["LogType"].FirstOrDefault();
            if (!string.IsNullOrEmpty(LogType)) @where = @where.And(p => p.LogType.Contains(LogType));
            //记录器名字 nvarchar
            var Logger = Request.Form["Logger"].FirstOrDefault();
            if (!string.IsNullOrEmpty(Logger)) @where = @where.And(p => p.Logger.Contains(Logger));
            //消息 nvarchar
            var Message = Request.Form["Message"].FirstOrDefault();
            if (!string.IsNullOrEmpty(Message)) @where = @where.And(p => p.Message.Contains(Message));
            //名称 nvarchar
            var MachineName = Request.Form["MachineName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(MachineName)) @where = @where.And(p => p.MachineName.Contains(MachineName));
            //ip nvarchar
            var MachineIp = Request.Form["MachineIp"].FirstOrDefault();
            if (!string.IsNullOrEmpty(MachineIp)) @where = @where.And(p => p.MachineIp.Contains(MachineIp));
            //请求方式 nvarchar
            var NetRequestMethod = Request.Form["NetRequestMethod"].FirstOrDefault();
            if (!string.IsNullOrEmpty(NetRequestMethod))
                @where = @where.And(p => p.NetRequestMethod.Contains(NetRequestMethod));
            //请求地址 nvarchar
            var NetRequestUrl = Request.Form["NetRequestUrl"].FirstOrDefault();
            if (!string.IsNullOrEmpty(NetRequestUrl))
                @where = @where.And(p => p.NetRequestUrl.Contains(NetRequestUrl));
            //是否授权 nvarchar
            var NetUserIsauthenticated = Request.Form["NetUserIsauthenticated"].FirstOrDefault();
            if (!string.IsNullOrEmpty(NetUserIsauthenticated))
                @where = @where.And(p => p.NetUserIsauthenticated.Contains(NetUserIsauthenticated));
            //授权类型 nvarchar
            var NetUserAuthtype = Request.Form["NetUserAuthtype"].FirstOrDefault();
            if (!string.IsNullOrEmpty(NetUserAuthtype))
                @where = @where.And(p => p.NetUserAuthtype.Contains(NetUserAuthtype));
            //身份认证 nvarchar
            var NetUserIdentity = Request.Form["NetUserIdentity"].FirstOrDefault();
            if (!string.IsNullOrEmpty(NetUserIdentity))
                @where = @where.And(p => p.NetUserIdentity.Contains(NetUserIdentity));
            //异常信息 nvarchar
            var Exception = Request.Form["Exception"].FirstOrDefault();
            if (!string.IsNullOrEmpty(Exception)) @where = @where.And(p => p.Exception.Contains(Exception));
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel = await _sysNLogRecordsServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序列");
            row1.CreateCell(1).SetCellValue("时间");
            row1.CreateCell(2).SetCellValue("级别");
            row1.CreateCell(3).SetCellValue("事件日志上下文");
            row1.CreateCell(4).SetCellValue("记录器名字");
            row1.CreateCell(5).SetCellValue("消息");
            row1.CreateCell(6).SetCellValue("名称");
            row1.CreateCell(7).SetCellValue("ip");
            row1.CreateCell(8).SetCellValue("请求方式");
            row1.CreateCell(9).SetCellValue("请求地址");
            row1.CreateCell(10).SetCellValue("是否授权");
            row1.CreateCell(11).SetCellValue("授权类型");
            row1.CreateCell(12).SetCellValue("身份认证");
            row1.CreateCell(13).SetCellValue("异常信息");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].LogDate.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].LogLevel);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].LogType);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].Logger);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].Message);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].MachineName);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].MachineIp);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].NetRequestMethod);
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].NetRequestUrl);
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].NetUserIsauthenticated);
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].NetUserAuthtype);
                rowtemp.CreateCell(12).SetCellValue(listmodel[i].NetUserIdentity);
                rowtemp.CreateCell(13).SetCellValue(listmodel[i].Exception);
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-SysNLogRecords导出(查询结果).xls";
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

        #region 清空数据============================================================

        // POST: Api/SysNLogRecords/DoBatchDelete/10,11,20
        /// <summary>
        ///     清空数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("清空数据")]
        public async Task<AdminUiCallBack> DoWipeData()
        {
            var jm = new AdminUiCallBack();

            var bl = await _sysNLogRecordsServices.DeleteAsync(p => p.id > 0);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        #endregion

    }
}