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
    ///     用户角色关联表
    /// </summary>
    [Description("用户角色关联表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class SysUserRoleController : ControllerBase
    {
        private readonly ISysUserRoleServices _sysUserRoleServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public SysUserRoleController(IWebHostEnvironment webHostEnvironment
            , ISysUserRoleServices sysUserRoleServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _sysUserRoleServices = sysUserRoleServices;
        }

        #region 获取列表============================================================

        // POST: Api/SysUserRole/GetPageList
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
            var where = PredicateBuilder.True<SysUserRole>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<SysUserRole, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "roleId":
                    orderEx = p => p.roleId;
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
            //用户id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //角色id int
            var roleId = Request.Form["roleId"].FirstOrDefault().ObjectToInt(0);
            if (roleId > 0) @where = @where.And(p => p.roleId == roleId);
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
            var list = await _sysUserRoleServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/SysUserRole/GetIndex
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

        // POST: Api/SysUserRole/GetCreate
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

        // POST: Api/SysUserRole/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] SysUserRole entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _sysUserRoleServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/SysUserRole/GetEdit
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

            var model = await _sysUserRoleServices.QueryByIdAsync(entity.id);
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

        // POST: Api/SysUserRole/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] SysUserRole entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sysUserRoleServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.userId = entity.userId;
            oldModel.roleId = entity.roleId;
            oldModel.createTime = entity.createTime;
            oldModel.updateTime = entity.updateTime;

            //事物处理过程结束
            var bl = await _sysUserRoleServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/SysUserRole/DoDelete/10
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

            var model = await _sysUserRoleServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var bl = await _sysUserRoleServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }

        #endregion

        #region 批量删除============================================================

        // POST: Api/SysUserRole/DoBatchDelete/10,11,20
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

            var bl = await _sysUserRoleServices.DeleteByIdsAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/SysUserRole/GetDetails/10
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

            var model = await _sysUserRoleServices.QueryByIdAsync(entity.id);
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

        // POST: Api/SysUserRole/SelectExportExcel/10
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
                await _sysUserRoleServices.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id,
                    OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("主键");
            row1.CreateCell(1).SetCellValue("用户id");
            row1.CreateCell(2).SetCellValue("角色id");
            row1.CreateCell(3).SetCellValue("创建时间");
            row1.CreateCell(4).SetCellValue("修改时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].roleId.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-SysUserRole导出(选择结果).xls";
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

        // POST: Api/SysUserRole/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<SysUserRole>();
            //查询筛选

            //主键 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //用户id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //角色id int
            var roleId = Request.Form["roleId"].FirstOrDefault().ObjectToInt(0);
            if (roleId > 0) @where = @where.And(p => p.roleId == roleId);
            //创建时间 datetime
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
            var listmodel = await _sysUserRoleServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("主键");
            row1.CreateCell(1).SetCellValue("用户id");
            row1.CreateCell(2).SetCellValue("角色id");
            row1.CreateCell(3).SetCellValue("创建时间");
            row1.CreateCell(4).SetCellValue("修改时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].roleId.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-SysUserRole导出(查询结果).xls";
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