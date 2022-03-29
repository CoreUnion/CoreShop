/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: {{ModelCreateTime}}
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
    /// {{ModelDescription}}
    ///</summary>
    [Description("{{ModelDescription}}")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize]
    public class {{ModelClassName}}Controller : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly I{{ModelClassName}}Services _{{ModelClassName}}Services;

        /// <summary>
        /// 构造函数
        ///</summary>
        public {{ModelClassName}}Controller(IWebHostEnvironment webHostEnvironment
            ,I{{ModelClassName}}Services {{ModelClassName}}Services
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _{{ModelClassName}}Services = {{ModelClassName}}Services;
        }

        #region 获取列表============================================================
        // POST: Api/{{ModelClassName}}/GetPageList
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
            var where = PredicateBuilder.True<{{ModelClassName}}>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<{{ModelClassName}}, object>> orderEx = orderField switch
            {
                {% for field in ModelFields %}"{{field.DbColumnName}}" => p => p.{{field.DbColumnName}},{% endfor %}
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
			{% for field in ModelFields %}{% if field.DataType == 'nvarchar' %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault();
            if (!string.IsNullOrEmpty({{field.DbColumnName}}))
            {
                where = where.And(p => p.{{field.DbColumnName}}.Contains({{field.DbColumnName}}));
            }{% elsif  field.DataType == 'int'  or field.DataType == 'bigint'  %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault().ObjectToInt(0);
            if ({{field.DbColumnName}} > 0)
            {
                where = where.And(p => p.{{field.DbColumnName}} == {{field.DbColumnName}});
            }{% elsif  field.DataType == 'decimal'  %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault().ObjectToDecimal(0);
            if ({{field.DbColumnName}} > 0)
            {
                where = where.And(p => p.{{field.DbColumnName}} == {{field.DbColumnName}});
            }{% elsif  field.DataType == 'datetime' %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault();
            if (!string.IsNullOrEmpty({{field.DbColumnName}}))
            {
                if ({{field.DbColumnName}}.Contains("到"))
                {
                    var dts = {{field.DbColumnName}}.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.{{field.DbColumnName}} > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.{{field.DbColumnName}} < dtEnd);
                }
                else
                {
                    var dt = {{field.DbColumnName}}.ObjectToDate();
                    where = where.And(p => p.{{field.DbColumnName}} > dt);
                }
            }{% elsif  field.DataType == 'bit' %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault();
            if (!string.IsNullOrEmpty({{field.DbColumnName}}) && {{field.DbColumnName}}.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.{{field.DbColumnName}} == true);
            }
            else if (!string.IsNullOrEmpty({{field.DbColumnName}}) && {{field.DbColumnName}}.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.{{field.DbColumnName}} == false);
            }{% else %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault();
            if (!string.IsNullOrEmpty({{field.DbColumnName}}))
            {
                where = where.And(p => p.{{field.DbColumnName}}.Contains({{field.DbColumnName}}));
            }{% endif %}{% endfor %}
            //获取数据
            var list = await _{{ModelClassName}}Services.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/{{ModelClassName}}/GetIndex
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
        // POST: Api/{{ModelClassName}}/GetCreate
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

        #region 创建提交============================================================
        // POST: Api/{{ModelClassName}}/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody]{{ModelClassName}} entity)
        {
            var jm = await _{{ModelClassName}}Services.InsertAsync(entity);
            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/{{ModelClassName}}/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody]FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _{{ModelClassName}}Services.QueryByIdAsync(entity.id, false);
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
        // POST: Api/{{ModelClassName}}/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody]{{ModelClassName}} entity)
        {
            var jm = await _{{ModelClassName}}Services.UpdateAsync(entity);
            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/{{ModelClassName}}/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody]FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _{{ModelClassName}}Services.ExistsAsync(p => p.id == entity.id, true);
            if (!model)
            {
                jm.msg = GlobalConstVars.DataisNo;
				return jm;
            }
            jm = await _{{ModelClassName}}Services.DeleteByIdAsync(entity.id);

            return jm;
        }
        #endregion

        #region 批量删除============================================================
        // POST: Api/{{ModelClassName}}/DoBatchDelete/10,11,20
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<AdminUiCallBack> DoBatchDelete([FromBody]FMArrayIntIds entity)
        {
            var jm = await _{{ModelClassName}}Services.DeleteByIdsAsync(entity.id);
            return jm;
        }

        #endregion

        #region 预览数据============================================================
        // POST: Api/{{ModelClassName}}/GetDetails/10
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

            var model = await _{{ModelClassName}}Services.QueryByIdAsync(entity.id, false);
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
        // POST: Api/{{ModelClassName}}/SelectExportExcel/10
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
            var listModel = await _{{ModelClassName}}Services.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);
{% for field in ModelFields %}
            var cell{{ forloop.index0  }} = headerRow.CreateCell({{ forloop.index0  }});
            cell{{ forloop.index0  }}.SetCellValue("{{field.ColumnDescription}}");
            cell{{ forloop.index0  }}.CellStyle = headerStyle;
            mySheet.SetColumnWidth({{ forloop.index0  }}, 10 * 256);
{% endfor %}
            headerRow.Height = 30 * 20;
            var commonCellStyle = ExcelHelper.GetCommonStyle(book);

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listModel.Count; i++)
            {
                var rowTemp = mySheet.CreateRow(i + 1);
{% for field in ModelFields %}
                    var rowTemp{{ forloop.index0  }} = rowTemp.CreateCell({{ forloop.index0  }});
                        rowTemp{{ forloop.index0  }}.SetCellValue(listModel[i].{{field.DbColumnName}}.ToString());
                        rowTemp{{ forloop.index0  }}.CellStyle = commonCellStyle;
{% endfor %}
            }
            // 导出excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-{{ModelClassName}}导出(选择结果).xls";
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
        // POST: Api/{{ModelClassName}}/QueryExportExcel/10
        /// <summary>
        /// 查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<{{ModelClassName}}>();
                //查询筛选
			{% for field in ModelFields %}{% if field.DataType == 'nvarchar' %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault();
            if (!string.IsNullOrEmpty({{field.DbColumnName}}))
            {
                where = where.And(p => p.{{field.DbColumnName}}.Contains({{field.DbColumnName}}));
            }{% elsif  field.DataType == 'int' or field.DataType == 'bigint'  %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault().ObjectToInt(0);
            if ({{field.DbColumnName}} > 0)
            {
                where = where.And(p => p.{{field.DbColumnName}} == {{field.DbColumnName}});
            }{% elsif  field.DataType == 'decimal'  %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault().ObjectToDecimal(0);
            if ({{field.DbColumnName}} > 0)
            {
                where = where.And(p => p.{{field.DbColumnName}} == {{field.DbColumnName}});
            }{% elsif  field.DataType == 'datetime' %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault();
            if (!string.IsNullOrEmpty({{field.DbColumnName}}))
            {
                var dt = {{field.DbColumnName}}.ObjectToDate();
                where = where.And(p => p.{{field.DbColumnName}} > dt);
            }{% elsif  field.DataType == 'bit' %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault();
            if (!string.IsNullOrEmpty({{field.DbColumnName}}) && {{field.DbColumnName}}.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.{{field.DbColumnName}} == true);
            }
            else if (!string.IsNullOrEmpty({{field.DbColumnName}}) && {{field.DbColumnName}}.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.{{field.DbColumnName}} == false);
            }{% else %}
			//{{field.ColumnDescription}} {{field.DataType}}
			var {{field.DbColumnName}} = Request.Form["{{field.DbColumnName}}"].FirstOrDefault();
            if (!string.IsNullOrEmpty({{field.DbColumnName}}))
            {
                where = where.And(p => p.{{field.DbColumnName}}.Contains({{field.DbColumnName}}));
            }{% endif %}{% endfor %}
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _{{ModelClassName}}Services.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
                var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);
            {% for field in ModelFields %}
            var cell{{ forloop.index0  }} = headerRow.CreateCell({{ forloop.index0  }});
            cell{{ forloop.index0  }}.SetCellValue("{{field.ColumnDescription}}");
            cell{{ forloop.index0  }}.CellStyle = headerStyle;
            mySheet.SetColumnWidth({{ forloop.index0  }}, 10 * 256);
			{% endfor %}

            headerRow.Height = 30 * 20;
            var commonCellStyle = ExcelHelper.GetCommonStyle(book);

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listModel.Count; i++)
            {
                var rowTemp = mySheet.CreateRow(i + 1);
{% for field in ModelFields %}

            var rowTemp{{ forloop.index0  }} = rowTemp.CreateCell({{ forloop.index0  }});
            rowTemp{{ forloop.index0  }}.SetCellValue(listModel[i].{{field.DbColumnName}}.ToString());
            rowTemp{{ forloop.index0  }}.CellStyle = commonCellStyle;

{% endfor %}
            }
            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-{{ModelClassName}}导出(查询结果).xls";
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

        {% for field in ModelFields %}{% if  field.DataType == 'bit' %}
        #region 设置{{field.ColumnDescription}}============================================================
        // POST: Api/{{ModelClassName}}/DoSet{{field.DbColumnName}}/10
        /// <summary>
        /// 设置{{field.ColumnDescription}}
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置{{field.ColumnDescription}}")]
        public async Task<AdminUiCallBack> DoSet{{field.DbColumnName}}([FromBody]FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _{{ModelClassName}}Services.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.{{field.DbColumnName}} = (bool)entity.data;

            var bl = await _{{ModelClassName}}Services.UpdateAsync(p => new {{ModelClassName}}() { {{field.DbColumnName}} = oldModel.{{field.DbColumnName}} }, p => p.id == oldModel.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
		}
        #endregion
        {% endif %}{% endfor %}

    }
}
