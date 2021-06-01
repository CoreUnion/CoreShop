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
    /// 服务消费券
    ///</summary>
    [Description("服务消费券")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize]
    public class CoreCmsUserServicesTicketController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsUserServicesTicketServices _CoreCmsUserServicesTicketServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsUserServicesTicketController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsUserServicesTicketServices CoreCmsUserServicesTicketServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _CoreCmsUserServicesTicketServices = CoreCmsUserServicesTicketServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsUserServicesTicket/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsUserServicesTicket>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsUserServicesTicket, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;

                case "serviceOrderId":
                    orderEx = p => p.serviceOrderId;
                    break;

                case "securityCode":
                    orderEx = p => p.securityCode;
                    break;

                case "redeemCode":
                    orderEx = p => p.redeemCode;
                    break;

                case "serviceId":
                    orderEx = p => p.serviceId;
                    break;

                case "userId":
                    orderEx = p => p.userId;
                    break;

                case "status":
                    orderEx = p => p.status;
                    break;

                case "validityType":
                    orderEx = p => p.validityType;
                    break;

                case "validityStartTime":
                    orderEx = p => p.validityStartTime;
                    break;

                case "validityEndTime":
                    orderEx = p => p.validityEndTime;
                    break;

                case "createTime":
                    orderEx = p => p.createTime;
                    break;

                case "isVerification":
                    orderEx = p => p.isVerification;
                    break;

                case "verificationTime":
                    orderEx = p => p.verificationTime;
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
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //关联购买订单 nvarchar
            var serviceOrderId = Request.Form["serviceOrderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(serviceOrderId))
            {
                where = where.And(p => p.serviceOrderId.Contains(serviceOrderId));
            }
            //兑换码 nvarchar
            var redeemCode = Request.Form["redeemCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(redeemCode))
            {
                where = where.And(p => p.redeemCode.Contains(redeemCode));
            }
            //关联服务项目id int
            var serviceId = Request.Form["serviceId"].FirstOrDefault().ObjectToInt(0);
            if (serviceId > 0)
            {
                where = where.And(p => p.serviceId == serviceId);
            }
            //关联用户id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            //状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0)
            {
                where = where.And(p => p.status == status);
            }
            //核销有效期类型 int
            var validityType = Request.Form["validityType"].FirstOrDefault().ObjectToInt(0);
            if (validityType > 0)
            {
                where = where.And(p => p.validityType == validityType);
            }
            //核销开始时间 datetime
            var validityStartTime = Request.Form["validityStartTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(validityStartTime))
            {
                if (validityStartTime.Contains("到"))
                {
                    var dts = validityStartTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.validityStartTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.validityStartTime < dtEnd);
                }
                else
                {
                    var dt = validityStartTime.ObjectToDate();
                    where = where.And(p => p.validityStartTime > dt);
                }
            }
            //核销结束时间 datetime
            var validityEndTime = Request.Form["validityEndTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(validityEndTime))
            {
                if (validityEndTime.Contains("到"))
                {
                    var dts = validityEndTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.validityEndTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.validityEndTime < dtEnd);
                }
                else
                {
                    var dt = validityEndTime.ObjectToDate();
                    where = where.And(p => p.validityEndTime > dt);
                }
            }
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
            //是否核销 bit
            var isVerification = Request.Form["isVerification"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isVerification) && isVerification.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isVerification == true);
            }
            else if (!string.IsNullOrEmpty(isVerification) && isVerification.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isVerification == false);
            }
            //核销时间 datetime
            var verificationTime = Request.Form["verificationTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(verificationTime))
            {
                if (verificationTime.Contains("到"))
                {
                    var dts = verificationTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.verificationTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.verificationTime < dtEnd);
                }
                else
                {
                    var dt = verificationTime.ObjectToDate();
                    where = where.And(p => p.verificationTime > dt);
                }
            }
            //获取数据
            var list = await _CoreCmsUserServicesTicketServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion 获取列表============================================================

        #region 首页数据============================================================

        // POST: Api/CoreCmsUserServicesTicket/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public JsonResult GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return new JsonResult(jm);
        }

        #endregion 首页数据============================================================

        #region 创建数据============================================================

        // POST: Api/CoreCmsUserServicesTicket/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public JsonResult GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return new JsonResult(jm);
        }

        #endregion 创建数据============================================================

        #region 创建提交============================================================

        // POST: Api/CoreCmsUserServicesTicket/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] CoreCmsUserServicesTicket entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _CoreCmsUserServicesTicketServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = (bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure);

            return new JsonResult(jm);
        }

        #endregion 创建提交============================================================

        #region 编辑数据============================================================

        // POST: Api/CoreCmsUserServicesTicket/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<JsonResult> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsUserServicesTicketServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            jm.code = 0;
            jm.data = model;

            return new JsonResult(jm);
        }

        #endregion 编辑数据============================================================

        #region 编辑提交============================================================

        // POST: Api/CoreCmsUserServicesTicket/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] CoreCmsUserServicesTicket entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _CoreCmsUserServicesTicketServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            //事物处理过程开始
            oldModel.serviceOrderId = entity.serviceOrderId;
            oldModel.securityCode = entity.securityCode;
            oldModel.redeemCode = entity.redeemCode;
            oldModel.serviceId = entity.serviceId;
            oldModel.userId = entity.userId;
            oldModel.status = entity.status;
            oldModel.validityType = entity.validityType;
            oldModel.validityStartTime = entity.validityStartTime;
            oldModel.validityEndTime = entity.validityEndTime;
            oldModel.createTime = entity.createTime;
            oldModel.isVerification = entity.isVerification;
            oldModel.verificationTime = entity.verificationTime;

            //事物处理过程结束
            var bl = await _CoreCmsUserServicesTicketServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion 编辑提交============================================================

        #region 删除数据============================================================

        // POST: Api/CoreCmsUserServicesTicket/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<JsonResult> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsUserServicesTicketServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }
            var bl = await _CoreCmsUserServicesTicketServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return new JsonResult(jm);
        }

        #endregion 删除数据============================================================

        #region 批量删除============================================================

        // POST: Api/CoreCmsUserServicesTicket/DoBatchDelete/10,11,20
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<JsonResult> DoBatchDelete([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _CoreCmsUserServicesTicketServices.DeleteByIdsAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return new JsonResult(jm);
        }

        #endregion 批量删除============================================================

        #region 预览数据============================================================

        // POST: Api/CoreCmsUserServicesTicket/GetDetails/10
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<JsonResult> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsUserServicesTicketServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            jm.code = 0;
            jm.data = model;

            return new JsonResult(jm);
        }

        #endregion 预览数据============================================================

        #region 选择导出============================================================

        // POST: Api/CoreCmsUserServicesTicket/SelectExportExcel/10
        /// <summary>
        /// 选择导出
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("选择导出")]
        public async Task<JsonResult> SelectExportExcel([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _CoreCmsUserServicesTicketServices.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("序列");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("关联购买订单");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("安全码");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("兑换码");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("关联服务项目id");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("关联用户id");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("状态");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("核销有效期类型");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);

            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("核销开始时间");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);

            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("核销结束时间");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);

            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("创建时间");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);

            var cell11 = headerRow.CreateCell(11);
            cell11.SetCellValue("是否核销");
            cell11.CellStyle = headerStyle;
            mySheet.SetColumnWidth(11, 10 * 256);

            var cell12 = headerRow.CreateCell(12);
            cell12.SetCellValue("核销时间");
            cell12.CellStyle = headerStyle;
            mySheet.SetColumnWidth(12, 10 * 256);

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
                rowTemp1.SetCellValue(listModel[i].serviceOrderId.ToString());
                rowTemp1.CellStyle = commonCellStyle;

                var rowTemp2 = rowTemp.CreateCell(2);
                rowTemp2.SetCellValue(listModel[i].securityCode.ToString());
                rowTemp2.CellStyle = commonCellStyle;

                var rowTemp3 = rowTemp.CreateCell(3);
                rowTemp3.SetCellValue(listModel[i].redeemCode.ToString());
                rowTemp3.CellStyle = commonCellStyle;

                var rowTemp4 = rowTemp.CreateCell(4);
                rowTemp4.SetCellValue(listModel[i].serviceId.ToString());
                rowTemp4.CellStyle = commonCellStyle;

                var rowTemp5 = rowTemp.CreateCell(5);
                rowTemp5.SetCellValue(listModel[i].userId.ToString());
                rowTemp5.CellStyle = commonCellStyle;

                var rowTemp6 = rowTemp.CreateCell(6);
                rowTemp6.SetCellValue(listModel[i].status.ToString());
                rowTemp6.CellStyle = commonCellStyle;

                var rowTemp7 = rowTemp.CreateCell(7);
                rowTemp7.SetCellValue(listModel[i].validityType.ToString());
                rowTemp7.CellStyle = commonCellStyle;

                var rowTemp8 = rowTemp.CreateCell(8);
                rowTemp8.SetCellValue(listModel[i].validityStartTime.ToString());
                rowTemp8.CellStyle = commonCellStyle;

                var rowTemp9 = rowTemp.CreateCell(9);
                rowTemp9.SetCellValue(listModel[i].validityEndTime.ToString());
                rowTemp9.CellStyle = commonCellStyle;

                var rowTemp10 = rowTemp.CreateCell(10);
                rowTemp10.SetCellValue(listModel[i].createTime.ToString());
                rowTemp10.CellStyle = commonCellStyle;

                var rowTemp11 = rowTemp.CreateCell(11);
                rowTemp11.SetCellValue(listModel[i].isVerification.ToString());
                rowTemp11.CellStyle = commonCellStyle;

                var rowTemp12 = rowTemp.CreateCell(12);
                rowTemp12.SetCellValue(listModel[i].verificationTime.ToString());
                rowTemp12.CellStyle = commonCellStyle;
            }
            // 导出excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsUserServicesTicket导出(选择结果).xls";
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

            return new JsonResult(jm);
        }

        #endregion 选择导出============================================================

        #region 查询导出============================================================

        // POST: Api/CoreCmsUserServicesTicket/QueryExportExcel/10
        /// <summary>
        /// 查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<JsonResult> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsUserServicesTicket>();
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //关联购买订单 nvarchar
            var serviceOrderId = Request.Form["serviceOrderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(serviceOrderId))
            {
                where = where.And(p => p.serviceOrderId.Contains(serviceOrderId));
            }
            //兑换码 nvarchar
            var redeemCode = Request.Form["redeemCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(redeemCode))
            {
                where = where.And(p => p.redeemCode.Contains(redeemCode));
            }
            //关联服务项目id int
            var serviceId = Request.Form["serviceId"].FirstOrDefault().ObjectToInt(0);
            if (serviceId > 0)
            {
                where = where.And(p => p.serviceId == serviceId);
            }
            //关联用户id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            //状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0)
            {
                where = where.And(p => p.status == status);
            }
            //核销有效期类型 int
            var validityType = Request.Form["validityType"].FirstOrDefault().ObjectToInt(0);
            if (validityType > 0)
            {
                where = where.And(p => p.validityType == validityType);
            }
            //核销开始时间 datetime
            var validityStartTime = Request.Form["validityStartTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(validityStartTime))
            {
                var dt = validityStartTime.ObjectToDate();
                where = where.And(p => p.validityStartTime > dt);
            }
            //核销结束时间 datetime
            var validityEndTime = Request.Form["validityEndTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(validityEndTime))
            {
                var dt = validityEndTime.ObjectToDate();
                where = where.And(p => p.validityEndTime > dt);
            }
            //创建时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                var dt = createTime.ObjectToDate();
                where = where.And(p => p.createTime > dt);
            }
            //是否核销 bit
            var isVerification = Request.Form["isVerification"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isVerification) && isVerification.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isVerification == true);
            }
            else if (!string.IsNullOrEmpty(isVerification) && isVerification.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isVerification == false);
            }
            //核销时间 datetime
            var verificationTime = Request.Form["verificationTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(verificationTime))
            {
                var dt = verificationTime.ObjectToDate();
                where = where.And(p => p.verificationTime > dt);
            }
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _CoreCmsUserServicesTicketServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("序列");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("关联购买订单");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("安全码");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("兑换码");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("关联服务项目id");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("关联用户id");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("状态");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("核销有效期类型");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);

            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("核销开始时间");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);

            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("核销结束时间");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);

            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("创建时间");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);

            var cell11 = headerRow.CreateCell(11);
            cell11.SetCellValue("是否核销");
            cell11.CellStyle = headerStyle;
            mySheet.SetColumnWidth(11, 10 * 256);

            var cell12 = headerRow.CreateCell(12);
            cell12.SetCellValue("核销时间");
            cell12.CellStyle = headerStyle;
            mySheet.SetColumnWidth(12, 10 * 256);

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
                rowTemp1.SetCellValue(listModel[i].serviceOrderId.ToString());
                rowTemp1.CellStyle = commonCellStyle;

                var rowTemp2 = rowTemp.CreateCell(2);
                rowTemp2.SetCellValue(listModel[i].securityCode.ToString());
                rowTemp2.CellStyle = commonCellStyle;

                var rowTemp3 = rowTemp.CreateCell(3);
                rowTemp3.SetCellValue(listModel[i].redeemCode.ToString());
                rowTemp3.CellStyle = commonCellStyle;

                var rowTemp4 = rowTemp.CreateCell(4);
                rowTemp4.SetCellValue(listModel[i].serviceId.ToString());
                rowTemp4.CellStyle = commonCellStyle;

                var rowTemp5 = rowTemp.CreateCell(5);
                rowTemp5.SetCellValue(listModel[i].userId.ToString());
                rowTemp5.CellStyle = commonCellStyle;

                var rowTemp6 = rowTemp.CreateCell(6);
                rowTemp6.SetCellValue(listModel[i].status.ToString());
                rowTemp6.CellStyle = commonCellStyle;

                var rowTemp7 = rowTemp.CreateCell(7);
                rowTemp7.SetCellValue(listModel[i].validityType.ToString());
                rowTemp7.CellStyle = commonCellStyle;

                var rowTemp8 = rowTemp.CreateCell(8);
                rowTemp8.SetCellValue(listModel[i].validityStartTime.ToString());
                rowTemp8.CellStyle = commonCellStyle;

                var rowTemp9 = rowTemp.CreateCell(9);
                rowTemp9.SetCellValue(listModel[i].validityEndTime.ToString());
                rowTemp9.CellStyle = commonCellStyle;

                var rowTemp10 = rowTemp.CreateCell(10);
                rowTemp10.SetCellValue(listModel[i].createTime.ToString());
                rowTemp10.CellStyle = commonCellStyle;

                var rowTemp11 = rowTemp.CreateCell(11);
                rowTemp11.SetCellValue(listModel[i].isVerification.ToString());
                rowTemp11.CellStyle = commonCellStyle;

                var rowTemp12 = rowTemp.CreateCell(12);
                rowTemp12.SetCellValue(listModel[i].verificationTime.ToString());
                rowTemp12.CellStyle = commonCellStyle;
            }
            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsUserServicesTicket导出(查询结果).xls";
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

            return new JsonResult(jm);
        }

        #endregion 查询导出============================================================

        #region 设置是否核销============================================================

        // POST: Api/CoreCmsUserServicesTicket/DoSetisVerification/10
        /// <summary>
        /// 设置是否核销
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否核销")]
        public async Task<JsonResult> DoSetisVerification([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _CoreCmsUserServicesTicketServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            oldModel.isVerification = (bool)entity.data;

            var bl = await _CoreCmsUserServicesTicketServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion 设置是否核销============================================================
    }
}