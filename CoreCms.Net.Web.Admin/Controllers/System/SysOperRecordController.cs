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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.IServices;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 操作日志表
    ///</summary>
    [Description("操作日志表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class SysOperRecordController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISysOperRecordServices _sysOperRecordServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public SysOperRecordController(IWebHostEnvironment webHostEnvironment
            ,ISysOperRecordServices sysOperRecordServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _sysOperRecordServices = sysOperRecordServices;
        }

        #region 获取列表============================================================
        // POST: Api/SysOperRecord/GetPageList
         /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<SysOperRecord>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<SysOperRecord, object>> orderEx = orderField switch
            {
                "id" => p => p.id,"userId" => p => p.userId,"userName" => p => p.userName,"model" => p => p.model,"description" => p => p.description,"url" => p => p.url,"requestMethod" => p => p.requestMethod,"operMethod" => p => p.operMethod,"param" => p => p.param,"result" => p => p.result,"ip" => p => p.ip,"spendTime" => p => p.spendTime,"state" => p => p.state,"createTime" => p => p.createTime,
                _ => p => p.id
            };

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
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
			//用户id int
			var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
			//用户名 nvarchar
			var userName = Request.Form["userName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(userName))
            {
                where = where.And(p => p.userName.Contains(userName));
            }
			//操作模块 nvarchar
			var model = Request.Form["model"].FirstOrDefault();
            if (!string.IsNullOrEmpty(model))
            {
                where = where.And(p => p.model.Contains(model));
            }
			//操作方法 nvarchar
			var description = Request.Form["description"].FirstOrDefault();
            if (!string.IsNullOrEmpty(description))
            {
                where = where.And(p => p.description.Contains(description));
            }
			//请求地址 nvarchar
			var url = Request.Form["url"].FirstOrDefault();
            if (!string.IsNullOrEmpty(url))
            {
                where = where.And(p => p.url.Contains(url));
            }
			//请求方式 nvarchar
			var requestMethod = Request.Form["requestMethod"].FirstOrDefault();
            if (!string.IsNullOrEmpty(requestMethod))
            {
                where = where.And(p => p.requestMethod.Contains(requestMethod));
            }
			//调用方法 nvarchar
			var operMethod = Request.Form["operMethod"].FirstOrDefault();
            if (!string.IsNullOrEmpty(operMethod))
            {
                where = where.And(p => p.operMethod.Contains(operMethod));
            }
			//请求参数 nvarchar
			var param = Request.Form["param"].FirstOrDefault();
            if (!string.IsNullOrEmpty(param))
            {
                where = where.And(p => p.param.Contains(param));
            }
			//返回结果 nvarchar
			var result = Request.Form["result"].FirstOrDefault();
            if (!string.IsNullOrEmpty(result))
            {
                where = where.And(p => p.result.Contains(result));
            }
			//ip地址 nvarchar
			var ip = Request.Form["ip"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ip))
            {
                where = where.And(p => p.ip.Contains(ip));
            }
			//请求耗时,单位毫秒 nvarchar
			var spendTime = Request.Form["spendTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(spendTime))
            {
                where = where.And(p => p.spendTime.Contains(spendTime));
            }
			//状态,0成功,1异常 int
			var state = Request.Form["state"].FirstOrDefault().ObjectToInt(0);
            if (state > 0)
            {
                where = where.And(p => p.state == state);
            }
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
            //获取数据
            var list = await _sysOperRecordServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/SysOperRecord/GetIndex
        /// <summary>
        /// 首页数据
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
        // POST: Api/SysOperRecord/GetCreate
        /// <summary>
        /// 创建数据
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

        #region 预览数据============================================================
        // POST: Api/SysOperRecord/GetDetails/10
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody]FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _sysOperRecordServices.QueryByIdAsync(entity.id, false);
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
        // POST: Api/SysOperRecord/SelectExportExcel/10
        /// <summary>
        /// 选择导出
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("选择导出")]
        public async Task<AdminUiCallBack> SelectExportExcel([FromBody]FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _sysOperRecordServices.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("主键");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("用户id");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("用户名");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("操作模块");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("操作方法");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("请求地址");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("请求方式");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("调用方法");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);

            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("请求参数");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);

            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("返回结果");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);

            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("ip地址");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);

            var cell11 = headerRow.CreateCell(11);
            cell11.SetCellValue("请求耗时,单位毫秒");
            cell11.CellStyle = headerStyle;
            mySheet.SetColumnWidth(11, 10 * 256);

            var cell12 = headerRow.CreateCell(12);
            cell12.SetCellValue("状态,0成功,1异常");
            cell12.CellStyle = headerStyle;
            mySheet.SetColumnWidth(12, 10 * 256);

            var cell13 = headerRow.CreateCell(13);
            cell13.SetCellValue("登录时间");
            cell13.CellStyle = headerStyle;
            mySheet.SetColumnWidth(13, 10 * 256);

            headerRow.Height = 30 * 20;
            var commonCellStyle = ExcelHelper.GetCommonStyle(book);

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listModel.Count; i++)
            {
                var rowTemp = mySheet.CreateRow(i + 1);

                    var rowTemp0 = rowTemp.CreateCell(0);
                        rowTemp0.SetCellValue(listModel[i].id.ToString());
                        rowTemp0.CellStyle = commonCellStyle;

                    var rowTemp1 = rowTemp.CreateCell(1);
                        rowTemp1.SetCellValue(listModel[i].userId.ToString());
                        rowTemp1.CellStyle = commonCellStyle;

                    var rowTemp2 = rowTemp.CreateCell(2);
                        rowTemp2.SetCellValue(listModel[i].userName.ToString());
                        rowTemp2.CellStyle = commonCellStyle;

                    var rowTemp3 = rowTemp.CreateCell(3);
                        rowTemp3.SetCellValue(listModel[i].model.ToString());
                        rowTemp3.CellStyle = commonCellStyle;

                    var rowTemp4 = rowTemp.CreateCell(4);
                        rowTemp4.SetCellValue(listModel[i].description.ToString());
                        rowTemp4.CellStyle = commonCellStyle;

                    var rowTemp5 = rowTemp.CreateCell(5);
                        rowTemp5.SetCellValue(listModel[i].url.ToString());
                        rowTemp5.CellStyle = commonCellStyle;

                    var rowTemp6 = rowTemp.CreateCell(6);
                        rowTemp6.SetCellValue(listModel[i].requestMethod.ToString());
                        rowTemp6.CellStyle = commonCellStyle;

                    var rowTemp7 = rowTemp.CreateCell(7);
                        rowTemp7.SetCellValue(listModel[i].operMethod.ToString());
                        rowTemp7.CellStyle = commonCellStyle;

                    var rowTemp8 = rowTemp.CreateCell(8);
                        rowTemp8.SetCellValue(listModel[i].param.ToString());
                        rowTemp8.CellStyle = commonCellStyle;

                    var rowTemp9 = rowTemp.CreateCell(9);
                        rowTemp9.SetCellValue(listModel[i].result.ToString());
                        rowTemp9.CellStyle = commonCellStyle;

                    var rowTemp10 = rowTemp.CreateCell(10);
                        rowTemp10.SetCellValue(listModel[i].ip.ToString());
                        rowTemp10.CellStyle = commonCellStyle;

                    var rowTemp11 = rowTemp.CreateCell(11);
                        rowTemp11.SetCellValue(listModel[i].spendTime.ToString());
                        rowTemp11.CellStyle = commonCellStyle;

                    var rowTemp12 = rowTemp.CreateCell(12);
                        rowTemp12.SetCellValue(listModel[i].state.ToString());
                        rowTemp12.CellStyle = commonCellStyle;

                    var rowTemp13 = rowTemp.CreateCell(13);
                        rowTemp13.SetCellValue(listModel[i].createTime.ToString());
                        rowTemp13.CellStyle = commonCellStyle;

            }
            // 导出excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-SysOperRecord导出(选择结果).xls";
            string filePath = webRootPath + tpath;
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return jm;
        }
        #endregion

        #region 查询导出============================================================
        // POST: Api/SysOperRecord/QueryExportExcel/10
        /// <summary>
        /// 查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<SysOperRecord>();
                //查询筛选
			
			//主键 int
			var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
			//用户id int
			var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
			//用户名 nvarchar
			var userName = Request.Form["userName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(userName))
            {
                where = where.And(p => p.userName.Contains(userName));
            }
			//操作模块 nvarchar
			var model = Request.Form["model"].FirstOrDefault();
            if (!string.IsNullOrEmpty(model))
            {
                where = where.And(p => p.model.Contains(model));
            }
			//操作方法 nvarchar
			var description = Request.Form["description"].FirstOrDefault();
            if (!string.IsNullOrEmpty(description))
            {
                where = where.And(p => p.description.Contains(description));
            }
			//请求地址 nvarchar
			var url = Request.Form["url"].FirstOrDefault();
            if (!string.IsNullOrEmpty(url))
            {
                where = where.And(p => p.url.Contains(url));
            }
			//请求方式 nvarchar
			var requestMethod = Request.Form["requestMethod"].FirstOrDefault();
            if (!string.IsNullOrEmpty(requestMethod))
            {
                where = where.And(p => p.requestMethod.Contains(requestMethod));
            }
			//调用方法 nvarchar
			var operMethod = Request.Form["operMethod"].FirstOrDefault();
            if (!string.IsNullOrEmpty(operMethod))
            {
                where = where.And(p => p.operMethod.Contains(operMethod));
            }
			//请求参数 nvarchar
			var param = Request.Form["param"].FirstOrDefault();
            if (!string.IsNullOrEmpty(param))
            {
                where = where.And(p => p.param.Contains(param));
            }
			//返回结果 nvarchar
			var result = Request.Form["result"].FirstOrDefault();
            if (!string.IsNullOrEmpty(result))
            {
                where = where.And(p => p.result.Contains(result));
            }
			//ip地址 nvarchar
			var ip = Request.Form["ip"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ip))
            {
                where = where.And(p => p.ip.Contains(ip));
            }
			//请求耗时,单位毫秒 nvarchar
			var spendTime = Request.Form["spendTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(spendTime))
            {
                where = where.And(p => p.spendTime.Contains(spendTime));
            }
			//状态,0成功,1异常 int
			var state = Request.Form["state"].FirstOrDefault().ObjectToInt(0);
            if (state > 0)
            {
                where = where.And(p => p.state == state);
            }
			//登录时间 datetime
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
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _sysOperRecordServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
                var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);
            
            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("主键");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);
			
            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("用户id");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);
			
            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("用户名");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);
			
            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("操作模块");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);
			
            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("操作方法");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);
			
            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("请求地址");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);
			
            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("请求方式");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);
			
            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("调用方法");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);
			
            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("请求参数");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);
			
            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("返回结果");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);
			
            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("ip地址");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);
			
            var cell11 = headerRow.CreateCell(11);
            cell11.SetCellValue("请求耗时,单位毫秒");
            cell11.CellStyle = headerStyle;
            mySheet.SetColumnWidth(11, 10 * 256);
			
            var cell12 = headerRow.CreateCell(12);
            cell12.SetCellValue("状态,0成功,1异常");
            cell12.CellStyle = headerStyle;
            mySheet.SetColumnWidth(12, 10 * 256);
			
            var cell13 = headerRow.CreateCell(13);
            cell13.SetCellValue("登录时间");
            cell13.CellStyle = headerStyle;
            mySheet.SetColumnWidth(13, 10 * 256);
			

            headerRow.Height = 30 * 20;
            var commonCellStyle = ExcelHelper.GetCommonStyle(book);

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listModel.Count; i++)
            {
                var rowTemp = mySheet.CreateRow(i + 1);


            var rowTemp0 = rowTemp.CreateCell(0);
            rowTemp0.SetCellValue(listModel[i].id.ToString());
            rowTemp0.CellStyle = commonCellStyle;



            var rowTemp1 = rowTemp.CreateCell(1);
            rowTemp1.SetCellValue(listModel[i].userId.ToString());
            rowTemp1.CellStyle = commonCellStyle;



            var rowTemp2 = rowTemp.CreateCell(2);
            rowTemp2.SetCellValue(listModel[i].userName.ToString());
            rowTemp2.CellStyle = commonCellStyle;



            var rowTemp3 = rowTemp.CreateCell(3);
            rowTemp3.SetCellValue(listModel[i].model.ToString());
            rowTemp3.CellStyle = commonCellStyle;



            var rowTemp4 = rowTemp.CreateCell(4);
            rowTemp4.SetCellValue(listModel[i].description.ToString());
            rowTemp4.CellStyle = commonCellStyle;



            var rowTemp5 = rowTemp.CreateCell(5);
            rowTemp5.SetCellValue(listModel[i].url.ToString());
            rowTemp5.CellStyle = commonCellStyle;



            var rowTemp6 = rowTemp.CreateCell(6);
            rowTemp6.SetCellValue(listModel[i].requestMethod.ToString());
            rowTemp6.CellStyle = commonCellStyle;



            var rowTemp7 = rowTemp.CreateCell(7);
            rowTemp7.SetCellValue(listModel[i].operMethod.ToString());
            rowTemp7.CellStyle = commonCellStyle;



            var rowTemp8 = rowTemp.CreateCell(8);
            rowTemp8.SetCellValue(listModel[i].param.ToString());
            rowTemp8.CellStyle = commonCellStyle;



            var rowTemp9 = rowTemp.CreateCell(9);
            rowTemp9.SetCellValue(listModel[i].result.ToString());
            rowTemp9.CellStyle = commonCellStyle;



            var rowTemp10 = rowTemp.CreateCell(10);
            rowTemp10.SetCellValue(listModel[i].ip.ToString());
            rowTemp10.CellStyle = commonCellStyle;



            var rowTemp11 = rowTemp.CreateCell(11);
            rowTemp11.SetCellValue(listModel[i].spendTime.ToString());
            rowTemp11.CellStyle = commonCellStyle;



            var rowTemp12 = rowTemp.CreateCell(12);
            rowTemp12.SetCellValue(listModel[i].state.ToString());
            rowTemp12.CellStyle = commonCellStyle;



            var rowTemp13 = rowTemp.CreateCell(13);
            rowTemp13.SetCellValue(listModel[i].createTime.ToString());
            rowTemp13.CellStyle = commonCellStyle;


            }
            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-SysOperRecord导出(查询结果).xls";
            string filePath = webRootPath + tpath;
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
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
