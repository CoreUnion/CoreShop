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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 代码生成器
    /// </summary>
    [Description("代码生成器")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Permissions.Name)]
    public class CodeGeneratorController : ControllerBase
    {
        private readonly ICodeGeneratorServices _codeGeneratorServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CodeGeneratorController(ICodeGeneratorServices codeGeneratorServices, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _codeGeneratorServices = codeGeneratorServices;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }


        /// <summary>
        /// 获取数据表格列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取数据表格列表")]
        public AdminUiCallBack GetTables()
        {
            var tables = _codeGeneratorServices.GetDbTables();

            var fristDb = new SqlSugar.DbTableInfo { Name = "AllTable", Description = "所有数据表含视图" };

            tables.Insert(0, fristDb);

            var jm = new AdminUiCallBack
            {
                code = 0,
                data = _mapper
                    .Map<List<SqlSugar.DbTableInfo>, List<CoreCms.Net.Model.ViewModels.Basics.DbTableInfoTree>>(tables)
            };
            return jm;
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("生成代码")]
        public ActionResult CodeGenDown([FromQuery] string tableName, [FromQuery] string fileType)
        {
            var jm = new AdminUiCallBack();
            if (string.IsNullOrEmpty(tableName))
            {
                jm.msg = "请选择数据库";
                return new JsonResult(jm);
            }
            if (string.IsNullOrEmpty(fileType))
            {
                jm.msg = "请选择要导出的文件类型";
                return new JsonResult(jm);
            }

            if (tableName == "AllTable")
            {
                var data = _codeGeneratorServices.CodeGenByAll(fileType);
                if (data != null)
                {
                    return File(data, System.Net.Mime.MediaTypeNames.Application.Zip, tableName + "-" + fileType + ".zip");
                }
                else
                {
                    jm.msg = tableName + "获取数据库字段失败";
                    return new JsonResult(jm);
                }
            }
            else
            {
                var data = _codeGeneratorServices.CodeGen(tableName, fileType);
                if (data != null)
                {
                    return File(data, System.Net.Mime.MediaTypeNames.Application.Zip, tableName + "-" + fileType + ".zip");
                }
                else
                {
                    jm.msg = tableName + "获取数据库字段失败";
                    return new JsonResult(jm);
                }
            }

        }


        #region 获取所有表和表名并生成excel

        /// <summary>
        /// 获取所有表和表名并生成excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取所有表和表名并生成excel")]
        public AdminUiCallBack GetDataBaseTablesToExcel()
        {
            var jm = new AdminUiCallBack();

                //创建Excel文件的对象
                var book = new HSSFWorkbook();
                //添加一个sheet
                var mySheet = book.CreateSheet("Sheet1");
                //获取list数据
                var listmodel = _codeGeneratorServices.GetDbTables();

                var headerStyle = ExcelHelper.GetHeaderStyle(book);
                var commonCellStyle = ExcelHelper.GetCommonStyle(book);


                //给sheet1添加第一行的头部标题
                var headerRow = mySheet.CreateRow(0);

                var cell0 = headerRow.CreateCell(0); cell0.SetCellValue("序列"); cell0.CellStyle = headerStyle;
                mySheet.SetColumnWidth(0, 10 * 256);

                var cell1 = headerRow.CreateCell(1); cell1.SetCellValue("表名"); cell1.CellStyle = headerStyle;
                mySheet.SetColumnWidth(1, 50 * 256);

                var cell2 = headerRow.CreateCell(2); cell2.SetCellValue("描述"); cell2.CellStyle = headerStyle;
                mySheet.SetColumnWidth(2, 50 * 256);

                //将数据逐步写入sheet1各个行
                for (var i = 0; i < listmodel.Count; i++)
                {
                    var rowtemp = mySheet.CreateRow(i + 1);

                    var rowtemp01 = rowtemp.CreateCell(0);
                    rowtemp01.SetCellValue(i);
                    rowtemp01.CellStyle = commonCellStyle;

                    var rowtemp02 = rowtemp.CreateCell(1);
                    rowtemp02.SetCellValue(listmodel[i].Name);
                    rowtemp02.CellStyle = commonCellStyle;

                    var rowtemp03 = rowtemp.CreateCell(2);
                    rowtemp03.SetCellValue(listmodel[i].Description);
                    rowtemp03.CellStyle = commonCellStyle;
                }
                // 导出excel
                string webRootPath = _webHostEnvironment.WebRootPath;
                string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-数据库表导出.xls";
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