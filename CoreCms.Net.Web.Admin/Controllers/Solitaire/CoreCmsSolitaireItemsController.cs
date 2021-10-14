/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/6/19 23:42:28
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
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.ViewModels.UI;
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
    /// 接龙活动商品表
    ///</summary>
    [Description("接龙活动商品表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsSolitaireItemsController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsSolitaireItemsServices _CoreCmsSolitaireItemsServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsSolitaireItemsController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsSolitaireItemsServices CoreCmsSolitaireItemsServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _CoreCmsSolitaireItemsServices = CoreCmsSolitaireItemsServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsSolitaireItems/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsSolitaireItems>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<CoreCmsSolitaireItems, object>> orderEx = orderField switch
            {
                "id" => p => p.id,
                "solitaireId" => p => p.solitaireId,
                "goodId" => p => p.goodId,
                "productId" => p => p.productId,
                "price" => p => p.price,
                "activityStock" => p => p.activityStock,
                "oneCanBuy" => p => p.oneCanBuy,
                "isDelete" => p => p.isDelete,
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

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //接龙序列 int
            var solitaireId = Request.Form["solitaireId"].FirstOrDefault().ObjectToInt(0);
            if (solitaireId > 0)
            {
                where = where.And(p => p.solitaireId == solitaireId);
            }
            //商品序列 int
            var goodId = Request.Form["goodId"].FirstOrDefault().ObjectToInt(0);
            if (goodId > 0)
            {
                where = where.And(p => p.goodId == goodId);
            }
            //货品价格 int
            var productId = Request.Form["productId"].FirstOrDefault().ObjectToInt(0);
            if (productId > 0)
            {
                where = where.And(p => p.productId == productId);
            }
            //活动库存 int
            var activityStock = Request.Form["activityStock"].FirstOrDefault().ObjectToInt(0);
            if (activityStock > 0)
            {
                where = where.And(p => p.activityStock == activityStock);
            }
            //每人可买 int
            var oneCanBuy = Request.Form["oneCanBuy"].FirstOrDefault().ObjectToInt(0);
            if (oneCanBuy > 0)
            {
                where = where.And(p => p.oneCanBuy == oneCanBuy);
            }
            //标注删除 bit
            var isDelete = Request.Form["isDelete"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isDelete == true);
            }
            else if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isDelete == false);
            }
            //获取数据
            var list = await _CoreCmsSolitaireItemsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsSolitaireItems/GetIndex
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
        // POST: Api/CoreCmsSolitaireItems/GetCreate
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
        // POST: Api/CoreCmsSolitaireItems/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsSolitaireItems entity)
        {
            var jm = await _CoreCmsSolitaireItemsServices.InsertAsync(entity);
            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsSolitaireItems/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsSolitaireItemsServices.QueryByIdAsync(entity.id, false);
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
        // POST: Api/CoreCmsSolitaireItems/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsSolitaireItems entity)
        {
            var jm = await _CoreCmsSolitaireItemsServices.UpdateAsync(entity);
            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsSolitaireItems/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsSolitaireItemsServices.ExistsAsync(p => p.id == entity.id, true);
            if (!model)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            jm = await _CoreCmsSolitaireItemsServices.DeleteByIdAsync(entity.id);

            return jm;
        }
        #endregion

        #region 批量删除============================================================
        // POST: Api/CoreCmsSolitaireItems/DoBatchDelete/10,11,20
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<AdminUiCallBack> DoBatchDelete([FromBody] FMArrayIntIds entity)
        {
            var jm = await _CoreCmsSolitaireItemsServices.DeleteByIdsAsync(entity.id);
            return jm;
        }

        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsSolitaireItems/GetDetails/10
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsSolitaireItemsServices.QueryByIdAsync(entity.id, false);
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
        // POST: Api/CoreCmsSolitaireItems/SelectExportExcel/10
        /// <summary>
        /// 选择导出
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
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _CoreCmsSolitaireItemsServices.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("序列");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("接龙序列");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("商品序列");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("货品价格");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("接龙价");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("活动库存");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("每人可买");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("标注删除");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);

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
                rowTemp1.SetCellValue(listModel[i].solitaireId.ToString());
                rowTemp1.CellStyle = commonCellStyle;

                var rowTemp2 = rowTemp.CreateCell(2);
                rowTemp2.SetCellValue(listModel[i].goodId.ToString());
                rowTemp2.CellStyle = commonCellStyle;

                var rowTemp3 = rowTemp.CreateCell(3);
                rowTemp3.SetCellValue(listModel[i].productId.ToString());
                rowTemp3.CellStyle = commonCellStyle;

                var rowTemp4 = rowTemp.CreateCell(4);
                rowTemp4.SetCellValue(listModel[i].price.ToString());
                rowTemp4.CellStyle = commonCellStyle;

                var rowTemp5 = rowTemp.CreateCell(5);
                rowTemp5.SetCellValue(listModel[i].activityStock.ToString());
                rowTemp5.CellStyle = commonCellStyle;

                var rowTemp6 = rowTemp.CreateCell(6);
                rowTemp6.SetCellValue(listModel[i].oneCanBuy.ToString());
                rowTemp6.CellStyle = commonCellStyle;

                var rowTemp7 = rowTemp.CreateCell(7);
                rowTemp7.SetCellValue(listModel[i].isDelete.ToString());
                rowTemp7.CellStyle = commonCellStyle;

            }
            // 导出excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsSolitaireItems导出(选择结果).xls";
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
        // POST: Api/CoreCmsSolitaireItems/QueryExportExcel/10
        /// <summary>
        /// 查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsSolitaireItems>();
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //接龙序列 int
            var solitaireId = Request.Form["solitaireId"].FirstOrDefault().ObjectToInt(0);
            if (solitaireId > 0)
            {
                where = where.And(p => p.solitaireId == solitaireId);
            }
            //商品序列 int
            var goodId = Request.Form["goodId"].FirstOrDefault().ObjectToInt(0);
            if (goodId > 0)
            {
                where = where.And(p => p.goodId == goodId);
            }
            //货品价格 int
            var productId = Request.Form["productId"].FirstOrDefault().ObjectToInt(0);
            if (productId > 0)
            {
                where = where.And(p => p.productId == productId);
            }
            //活动库存 int
            var activityStock = Request.Form["activityStock"].FirstOrDefault().ObjectToInt(0);
            if (activityStock > 0)
            {
                where = where.And(p => p.activityStock == activityStock);
            }
            //每人可买 int
            var oneCanBuy = Request.Form["oneCanBuy"].FirstOrDefault().ObjectToInt(0);
            if (oneCanBuy > 0)
            {
                where = where.And(p => p.oneCanBuy == oneCanBuy);
            }
            //标注删除 bit
            var isDelete = Request.Form["isDelete"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isDelete == true);
            }
            else if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isDelete == false);
            }
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _CoreCmsSolitaireItemsServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("序列");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("接龙序列");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("商品序列");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("货品价格");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("接龙价");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("活动库存");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("每人可买");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("标注删除");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);


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
                rowTemp1.SetCellValue(listModel[i].solitaireId.ToString());
                rowTemp1.CellStyle = commonCellStyle;



                var rowTemp2 = rowTemp.CreateCell(2);
                rowTemp2.SetCellValue(listModel[i].goodId.ToString());
                rowTemp2.CellStyle = commonCellStyle;



                var rowTemp3 = rowTemp.CreateCell(3);
                rowTemp3.SetCellValue(listModel[i].productId.ToString());
                rowTemp3.CellStyle = commonCellStyle;



                var rowTemp4 = rowTemp.CreateCell(4);
                rowTemp4.SetCellValue(listModel[i].price.ToString());
                rowTemp4.CellStyle = commonCellStyle;



                var rowTemp5 = rowTemp.CreateCell(5);
                rowTemp5.SetCellValue(listModel[i].activityStock.ToString());
                rowTemp5.CellStyle = commonCellStyle;



                var rowTemp6 = rowTemp.CreateCell(6);
                rowTemp6.SetCellValue(listModel[i].oneCanBuy.ToString());
                rowTemp6.CellStyle = commonCellStyle;



                var rowTemp7 = rowTemp.CreateCell(7);
                rowTemp7.SetCellValue(listModel[i].isDelete.ToString());
                rowTemp7.CellStyle = commonCellStyle;


            }
            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsSolitaireItems导出(查询结果).xls";
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


        #region 设置标注删除============================================================
        // POST: Api/CoreCmsSolitaireItems/DoSetisDelete/10
        /// <summary>
        /// 设置标注删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置标注删除")]
        public async Task<AdminUiCallBack> DoSetisDelete([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _CoreCmsSolitaireItemsServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isDelete = (bool)entity.data;

            var bl = await _CoreCmsSolitaireItemsServices.UpdateAsync(p => new CoreCmsSolitaireItems() { isDelete = oldModel.isDelete }, p => p.id == oldModel.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion


    }
}
