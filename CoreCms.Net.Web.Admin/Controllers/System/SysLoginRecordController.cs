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
    ///     登录日志表
    /// </summary>
    [Description("登录日志表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class SysLoginRecordController : ControllerBase
    {
        private readonly ISysLoginRecordServices _sysLoginRecordServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public SysLoginRecordController(IWebHostEnvironment webHostEnvironment
            , ISysLoginRecordServices sysLoginRecordServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _sysLoginRecordServices = sysLoginRecordServices;
        }

        #region 获取列表============================================================

        // POST: Api/SysLoginRecord/GetPageList
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
            var where = PredicateBuilder.True<SysLoginRecord>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<SysLoginRecord, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "username":
                    orderEx = p => p.username;
                    break;
                case "os":
                    orderEx = p => p.os;
                    break;
                case "device":
                    orderEx = p => p.device;
                    break;
                case "browser":
                    orderEx = p => p.browser;
                    break;
                case "ip":
                    orderEx = p => p.ip;
                    break;
                case "operType":
                    orderEx = p => p.operType;
                    break;
                case "comments":
                    orderEx = p => p.comments;
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

            //主键 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //用户账号 nvarchar
            var username = Request.Form["username"].FirstOrDefault();
            if (!string.IsNullOrEmpty(username)) @where = @where.And(p => p.username.Contains(username));
            //操作系统 nvarchar
            var os = Request.Form["os"].FirstOrDefault();
            if (!string.IsNullOrEmpty(os)) @where = @where.And(p => p.os.Contains(os));
            //设备名 nvarchar
            var device = Request.Form["device"].FirstOrDefault();
            if (!string.IsNullOrEmpty(device)) @where = @where.And(p => p.device.Contains(device));
            //浏览器类型 nvarchar
            var browser = Request.Form["browser"].FirstOrDefault();
            if (!string.IsNullOrEmpty(browser)) @where = @where.And(p => p.browser.Contains(browser));
            //ip地址 nvarchar
            var ip = Request.Form["ip"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ip)) @where = @where.And(p => p.ip.Contains(ip));
            //操作类型,0登录成功,1登录失败,2退出登录,3刷新token int
            var operType = Request.Form["operType"].FirstOrDefault().ObjectToInt(0);
            if (operType > 0) @where = @where.And(p => p.operType == operType);
            //备注 nvarchar
            var comments = Request.Form["comments"].FirstOrDefault();
            if (!string.IsNullOrEmpty(comments)) @where = @where.And(p => p.comments.Contains(comments));
            //登录时间 datetime
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

            //修改时间 datetime
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
            var list = await _sysLoginRecordServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/SysLoginRecord/GetIndex
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

            var logType = EnumHelper.EnumToList<GlobalEnumVars.LoginRecordType>();
            jm.data = new
            {
                logType
            };

            return jm;
        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/SysLoginRecord/GetDetails/10
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

            var model = await _sysLoginRecordServices.QueryByIdAsync(entity.id);
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

        // POST: Api/SysLoginRecord/SelectExportExcel/10
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
            var listmodel = await _sysLoginRecordServices.QueryListByClauseAsync(p => entity.id.Contains(p.id),
                p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("主键");
            row1.CreateCell(1).SetCellValue("用户账号");
            row1.CreateCell(2).SetCellValue("操作系统");
            row1.CreateCell(3).SetCellValue("设备名");
            row1.CreateCell(4).SetCellValue("浏览器类型");
            row1.CreateCell(5).SetCellValue("ip地址");
            row1.CreateCell(6).SetCellValue("操作类型,0登录成功,1登录失败,2退出登录,3刷新token");
            row1.CreateCell(7).SetCellValue("备注");
            row1.CreateCell(8).SetCellValue("登录时间");
            row1.CreateCell(9).SetCellValue("修改时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].username);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].os);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].device);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].browser);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].ip);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].operType.ToString());
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].comments);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-SysLoginRecord导出(选择结果).xls";
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

        // POST: Api/SysLoginRecord/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<SysLoginRecord>();
            //查询筛选

            //主键 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //用户账号 nvarchar
            var username = Request.Form["username"].FirstOrDefault();
            if (!string.IsNullOrEmpty(username)) @where = @where.And(p => p.username.Contains(username));
            //操作系统 nvarchar
            var os = Request.Form["os"].FirstOrDefault();
            if (!string.IsNullOrEmpty(os)) @where = @where.And(p => p.os.Contains(os));
            //设备名 nvarchar
            var device = Request.Form["device"].FirstOrDefault();
            if (!string.IsNullOrEmpty(device)) @where = @where.And(p => p.device.Contains(device));
            //浏览器类型 nvarchar
            var browser = Request.Form["browser"].FirstOrDefault();
            if (!string.IsNullOrEmpty(browser)) @where = @where.And(p => p.browser.Contains(browser));
            //ip地址 nvarchar
            var ip = Request.Form["ip"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ip)) @where = @where.And(p => p.ip.Contains(ip));
            //操作类型,0登录成功,1登录失败,2退出登录,3刷新token int
            var operType = Request.Form["operType"].FirstOrDefault().ObjectToInt(0);
            if (operType > 0) @where = @where.And(p => p.operType == operType);
            //备注 nvarchar
            var comments = Request.Form["comments"].FirstOrDefault();
            if (!string.IsNullOrEmpty(comments)) @where = @where.And(p => p.comments.Contains(comments));
            //登录时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                var dt = createTime.ObjectToDate();
                where = where.And(p => p.createTime > dt);
            }

            //修改时间 datetime
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
            var listmodel = await _sysLoginRecordServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("主键");
            row1.CreateCell(1).SetCellValue("用户账号");
            row1.CreateCell(2).SetCellValue("操作系统");
            row1.CreateCell(3).SetCellValue("设备名");
            row1.CreateCell(4).SetCellValue("浏览器类型");
            row1.CreateCell(5).SetCellValue("ip地址");
            row1.CreateCell(6).SetCellValue("操作类型,0登录成功,1登录失败,2退出登录,3刷新token");
            row1.CreateCell(7).SetCellValue("备注");
            row1.CreateCell(8).SetCellValue("登录时间");
            row1.CreateCell(9).SetCellValue("修改时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].username);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].os);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].device);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].browser);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].ip);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].operType.ToString());
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].comments);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-SysLoginRecord导出(查询结果).xls";
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

            var bl = await _sysLoginRecordServices.DeleteByIdsAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

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

            var bl = await _sysLoginRecordServices.DeleteAsync(p => p.id > 0);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        #endregion

    }
}