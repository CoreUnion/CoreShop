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
    /// 服务项目表
    ///</summary>
    [Description("服务项目表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsServicesController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsServicesServices _coreCmsServicesServices;
        private readonly ICoreCmsUserGradeServices _coreCmsUserGradeServices;
        private readonly ICoreCmsStoreServices _storeServices;
        private readonly ICoreCmsUserServicesOrderServices _userServicesOrderServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsServicesController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsServicesServices coreCmsServicesServices, ICoreCmsUserGradeServices coreCmsUserGradeServices, ICoreCmsStoreServices storeServices, ICoreCmsUserServicesOrderServices userServicesOrderServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsServicesServices = coreCmsServicesServices;
            _coreCmsUserGradeServices = coreCmsUserGradeServices;
            _storeServices = storeServices;
            _userServicesOrderServices = userServicesOrderServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsServices/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsServices>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsServices, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "title":
                    orderEx = p => p.title;
                    break;
                case "thumbnail":
                    orderEx = p => p.thumbnail;
                    break;
                case "description":
                    orderEx = p => p.description;
                    break;
                case "contentBody":
                    orderEx = p => p.contentBody;
                    break;
                case "allowedMembership":
                    orderEx = p => p.allowedMembership;
                    break;
                case "status":
                    orderEx = p => p.status;
                    break;
                case "maxBuyNumber":
                    orderEx = p => p.maxBuyNumber;
                    break;
                case "amount":
                    orderEx = p => p.amount;
                    break;
                case "startTime":
                    orderEx = p => p.startTime;
                    break;
                case "endTime":
                    orderEx = p => p.endTime;
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
                case "ticketNumber":
                    orderEx = p => p.ticketNumber;
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

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //项目名称 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title))
            {
                where = where.And(p => p.title.Contains(title));
            }
            //项目缩略图 nvarchar
            var thumbnail = Request.Form["thumbnail"].FirstOrDefault();
            if (!string.IsNullOrEmpty(thumbnail))
            {
                where = where.And(p => p.thumbnail.Contains(thumbnail));
            }
            //项目概述 nvarchar
            var description = Request.Form["description"].FirstOrDefault();
            if (!string.IsNullOrEmpty(description))
            {
                where = where.And(p => p.description.Contains(description));
            }
            //项目详细说明 nvarchar
            var contentBody = Request.Form["contentBody"].FirstOrDefault();
            if (!string.IsNullOrEmpty(contentBody))
            {
                where = where.And(p => p.contentBody.Contains(contentBody));
            }
            //允许购买会员级别 nvarchar
            var allowedMembership = Request.Form["allowedMembership"].FirstOrDefault();
            if (!string.IsNullOrEmpty(allowedMembership))
            {
                where = where.And(p => p.allowedMembership.Contains(allowedMembership));
            }
            //项目状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0)
            {
                where = where.And(p => p.status == status);
            }
            //项目重复购买次数 int
            var maxBuyNumber = Request.Form["maxBuyNumber"].FirstOrDefault().ObjectToInt(0);
            if (maxBuyNumber > 0)
            {
                where = where.And(p => p.maxBuyNumber == maxBuyNumber);
            }
            //项目可销售数量 int
            var amount = Request.Form["amount"].FirstOrDefault().ObjectToInt(0);
            if (amount > 0)
            {
                where = where.And(p => p.amount == amount);
            }
            //项目开始时间 datetime
            var startTime = Request.Form["startTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(startTime))
            {
                if (startTime.Contains("到"))
                {
                    var dts = startTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.startTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.startTime < dtEnd);
                }
                else
                {
                    var dt = startTime.ObjectToDate();
                    where = where.And(p => p.startTime > dt);
                }
            }
            //项目截止时间 datetime
            var endTime = Request.Form["endTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(endTime))
            {
                if (endTime.Contains("到"))
                {
                    var dts = endTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.endTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.endTime < dtEnd);
                }
                else
                {
                    var dt = endTime.ObjectToDate();
                    where = where.And(p => p.endTime > dt);
                }
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
            //核销服务券数量 int
            var ticketNumber = Request.Form["ticketNumber"].FirstOrDefault().ObjectToInt(0);
            if (ticketNumber > 0)
            {
                where = where.And(p => p.ticketNumber == ticketNumber);
            }
            //项目创建时间 datetime
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
            //项目更新时间 datetime
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
            var list = await _coreCmsServicesServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsServices/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public async Task<AdminUiCallBack> GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            //服务核销有效期类型
            var types = EnumHelper.EnumToList<GlobalEnumVars.ServicesValidityType>();
            //服务状态列表
            var status = EnumHelper.EnumToList<GlobalEnumVars.ServicesStatus>();
            //用户级别列表
            var userGrade = await _coreCmsUserGradeServices.QueryAsync();
            jm.data = new
            {
                types,
                status,
                userGrade
            };

            return jm;
        }
        #endregion

        #region 创建数据============================================================
        // POST: Api/CoreCmsServices/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public async Task<AdminUiCallBack> GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            //服务核销有效期类型
            var types = EnumHelper.EnumToList<GlobalEnumVars.ServicesValidityType>();
            //服务状态列表
            var status = EnumHelper.EnumToList<GlobalEnumVars.ServicesStatus>();
            //用户级别列表
            var userGrade = await _coreCmsUserGradeServices.QueryAsync();
            //门店列表
            var stores = await _storeServices.QueryListByClauseAsync(p => p.id > 0, p => p.isDefault, OrderByType.Desc);
            jm.data = new
            {
                types,
                status,
                userGrade,
                stores

            };
            return jm;
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsServices/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsServices entity)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(entity.consumableStore))
            {
                jm.msg = "请选择审核门店";
                return jm;
            }
            if (string.IsNullOrEmpty(entity.allowedMembership))
            {
                jm.msg = "请选择会员级别";
                return jm;
            }

            entity.createTime = DateTime.Now;
            entity.consumableStore = "," + entity.consumableStore + ",";
            entity.allowedMembership = "," + entity.allowedMembership + ",";

            if (entity.validityType == (int)GlobalEnumVars.ServicesValidityType.TimeFrame && (entity.validityEndTime == null || entity.validityStartTime == null))
            {
                jm.msg = "限制时间段情况下,必须要设置核验时间段。";
            }

            var bl = await _coreCmsServicesServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = (bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure);

            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsServices/GetEdit
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

            var model = await _coreCmsServicesServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            model.consumableStore = CommonHelper.GetCaptureInterceptedText(model.consumableStore, ",");
            model.allowedMembership = CommonHelper.GetCaptureInterceptedText(model.allowedMembership, ",");

            //服务核销有效期类型
            var types = EnumHelper.EnumToList<GlobalEnumVars.ServicesValidityType>();
            //服务状态列表
            var status = EnumHelper.EnumToList<GlobalEnumVars.ServicesStatus>();
            //用户级别列表
            var userGrade = await _coreCmsUserGradeServices.QueryAsync();
            //门店列表
            var stores = await _storeServices.QueryListByClauseAsync(p => p.id > 0, p => p.isDefault, OrderByType.Desc);
            jm.code = 0;

            jm.data = new
            {
                types,
                status,
                userGrade,
                model,
                stores
            };

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Api/CoreCmsServices/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsServices entity)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(entity.consumableStore))
            {
                jm.msg = "请选择审核门店";
                return jm;
            }

            var oldModel = await _coreCmsServicesServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            //oldModel.id = entity.id;
            oldModel.title = entity.title;
            oldModel.thumbnail = entity.thumbnail;
            oldModel.description = entity.description;
            oldModel.contentBody = entity.contentBody;
            //oldModel.allowedMembership = entity.allowedMembership;
            oldModel.status = entity.status;
            oldModel.maxBuyNumber = entity.maxBuyNumber;
            oldModel.amount = entity.amount;
            oldModel.startTime = entity.startTime;
            oldModel.endTime = entity.endTime;
            oldModel.validityType = entity.validityType;
            oldModel.validityStartTime = entity.validityStartTime;
            oldModel.validityEndTime = entity.validityEndTime;
            oldModel.ticketNumber = entity.ticketNumber;
            //oldModel.createTime = entity.createTime;
            oldModel.updateTime = DateTime.Now;
            oldModel.money = entity.money;

            oldModel.consumableStore = "," + entity.consumableStore + ",";
            oldModel.allowedMembership = "," + entity.allowedMembership + ",";



            //事物处理过程结束
            var bl = await _coreCmsServicesServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsServices/DoDelete/10
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

            var model = await _coreCmsServicesServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var haveOrder = await _userServicesOrderServices.ExistsAsync(p => p.servicesId == model.id);
            if (haveOrder)
            {
                jm.msg = "存在关联订单,禁止删除";
                return jm;

            }


            var bl = await _coreCmsServicesServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }
        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsServices/GetDetails/10
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

            var model = await _coreCmsServicesServices.QueryByIdAsync(entity.id);
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
        // POST: Api/CoreCmsServices/SelectExportExcel/10
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
            var listModel = await _coreCmsServicesServices.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("序列");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("项目名称");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("项目缩略图");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("项目概述");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("项目详细说明");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("允许购买会员级别");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("项目状态");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("项目重复购买次数");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);

            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("项目可销售数量");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);

            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("项目开始时间");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);

            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("项目截止时间");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);

            var cell11 = headerRow.CreateCell(11);
            cell11.SetCellValue("核销有效期类型");
            cell11.CellStyle = headerStyle;
            mySheet.SetColumnWidth(11, 10 * 256);

            var cell12 = headerRow.CreateCell(12);
            cell12.SetCellValue("核销开始时间");
            cell12.CellStyle = headerStyle;
            mySheet.SetColumnWidth(12, 10 * 256);

            var cell13 = headerRow.CreateCell(13);
            cell13.SetCellValue("核销结束时间");
            cell13.CellStyle = headerStyle;
            mySheet.SetColumnWidth(13, 10 * 256);

            var cell14 = headerRow.CreateCell(14);
            cell14.SetCellValue("核销服务券数量");
            cell14.CellStyle = headerStyle;
            mySheet.SetColumnWidth(14, 10 * 256);

            var cell15 = headerRow.CreateCell(15);
            cell15.SetCellValue("项目创建时间");
            cell15.CellStyle = headerStyle;
            mySheet.SetColumnWidth(15, 10 * 256);

            var cell16 = headerRow.CreateCell(16);
            cell16.SetCellValue("项目更新时间");
            cell16.CellStyle = headerStyle;
            mySheet.SetColumnWidth(16, 10 * 256);

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
                rowTemp1.SetCellValue(listModel[i].title.ToString());
                rowTemp1.CellStyle = commonCellStyle;

                var rowTemp2 = rowTemp.CreateCell(2);
                rowTemp2.SetCellValue(listModel[i].thumbnail.ToString());
                rowTemp2.CellStyle = commonCellStyle;

                var rowTemp3 = rowTemp.CreateCell(3);
                rowTemp3.SetCellValue(listModel[i].description.ToString());
                rowTemp3.CellStyle = commonCellStyle;

                var rowTemp4 = rowTemp.CreateCell(4);
                rowTemp4.SetCellValue(listModel[i].contentBody.ToString());
                rowTemp4.CellStyle = commonCellStyle;

                var rowTemp5 = rowTemp.CreateCell(5);
                rowTemp5.SetCellValue(listModel[i].allowedMembership.ToString());
                rowTemp5.CellStyle = commonCellStyle;

                var rowTemp6 = rowTemp.CreateCell(6);
                rowTemp6.SetCellValue(listModel[i].status.ToString());
                rowTemp6.CellStyle = commonCellStyle;

                var rowTemp7 = rowTemp.CreateCell(7);
                rowTemp7.SetCellValue(listModel[i].maxBuyNumber.ToString());
                rowTemp7.CellStyle = commonCellStyle;

                var rowTemp8 = rowTemp.CreateCell(8);
                rowTemp8.SetCellValue(listModel[i].amount.ToString());
                rowTemp8.CellStyle = commonCellStyle;

                var rowTemp9 = rowTemp.CreateCell(9);
                rowTemp9.SetCellValue(listModel[i].startTime.ToString());
                rowTemp9.CellStyle = commonCellStyle;

                var rowTemp10 = rowTemp.CreateCell(10);
                rowTemp10.SetCellValue(listModel[i].endTime.ToString());
                rowTemp10.CellStyle = commonCellStyle;

                var rowTemp11 = rowTemp.CreateCell(11);
                rowTemp11.SetCellValue(listModel[i].validityType.ToString());
                rowTemp11.CellStyle = commonCellStyle;

                var rowTemp12 = rowTemp.CreateCell(12);
                rowTemp12.SetCellValue(listModel[i].validityStartTime.ToString());
                rowTemp12.CellStyle = commonCellStyle;

                var rowTemp13 = rowTemp.CreateCell(13);
                rowTemp13.SetCellValue(listModel[i].validityEndTime.ToString());
                rowTemp13.CellStyle = commonCellStyle;

                var rowTemp14 = rowTemp.CreateCell(14);
                rowTemp14.SetCellValue(listModel[i].ticketNumber.ToString());
                rowTemp14.CellStyle = commonCellStyle;

                var rowTemp15 = rowTemp.CreateCell(15);
                rowTemp15.SetCellValue(listModel[i].createTime.ToString());
                rowTemp15.CellStyle = commonCellStyle;

                var rowTemp16 = rowTemp.CreateCell(16);
                rowTemp16.SetCellValue(listModel[i].updateTime.ToString());
                rowTemp16.CellStyle = commonCellStyle;

            }
            // 导出excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsServices导出(选择结果).xls";
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
        // POST: Api/CoreCmsServices/QueryExportExcel/10
        /// <summary>
        /// 查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsServices>();
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //项目名称 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title))
            {
                where = where.And(p => p.title.Contains(title));
            }
            //项目缩略图 nvarchar
            var thumbnail = Request.Form["thumbnail"].FirstOrDefault();
            if (!string.IsNullOrEmpty(thumbnail))
            {
                where = where.And(p => p.thumbnail.Contains(thumbnail));
            }
            //项目概述 nvarchar
            var description = Request.Form["description"].FirstOrDefault();
            if (!string.IsNullOrEmpty(description))
            {
                where = where.And(p => p.description.Contains(description));
            }
            //项目详细说明 nvarchar
            var contentBody = Request.Form["contentBody"].FirstOrDefault();
            if (!string.IsNullOrEmpty(contentBody))
            {
                where = where.And(p => p.contentBody.Contains(contentBody));
            }
            //允许购买会员级别 nvarchar
            var allowedMembership = Request.Form["allowedMembership"].FirstOrDefault();
            if (!string.IsNullOrEmpty(allowedMembership))
            {
                where = where.And(p => p.allowedMembership.Contains(allowedMembership));
            }
            //项目状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0)
            {
                where = where.And(p => p.status == status);
            }
            //项目重复购买次数 int
            var maxBuyNumber = Request.Form["maxBuyNumber"].FirstOrDefault().ObjectToInt(0);
            if (maxBuyNumber > 0)
            {
                where = where.And(p => p.maxBuyNumber == maxBuyNumber);
            }
            //项目可销售数量 int
            var amount = Request.Form["amount"].FirstOrDefault().ObjectToInt(0);
            if (amount > 0)
            {
                where = where.And(p => p.amount == amount);
            }
            //项目开始时间 datetime
            var startTime = Request.Form["startTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(startTime))
            {
                var dt = startTime.ObjectToDate();
                where = where.And(p => p.startTime > dt);
            }
            //项目截止时间 datetime
            var endTime = Request.Form["endTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(endTime))
            {
                var dt = endTime.ObjectToDate();
                where = where.And(p => p.endTime > dt);
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
            //核销服务券数量 int
            var ticketNumber = Request.Form["ticketNumber"].FirstOrDefault().ObjectToInt(0);
            if (ticketNumber > 0)
            {
                where = where.And(p => p.ticketNumber == ticketNumber);
            }
            //项目创建时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                var dt = createTime.ObjectToDate();
                where = where.And(p => p.createTime > dt);
            }
            //项目更新时间 datetime
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
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _coreCmsServicesServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("序列");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("项目名称");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("项目缩略图");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("项目概述");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("项目详细说明");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("允许购买会员级别");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("项目状态");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("项目重复购买次数");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);

            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("项目可销售数量");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);

            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("项目开始时间");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);

            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("项目截止时间");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);

            var cell11 = headerRow.CreateCell(11);
            cell11.SetCellValue("核销有效期类型");
            cell11.CellStyle = headerStyle;
            mySheet.SetColumnWidth(11, 10 * 256);

            var cell12 = headerRow.CreateCell(12);
            cell12.SetCellValue("核销开始时间");
            cell12.CellStyle = headerStyle;
            mySheet.SetColumnWidth(12, 10 * 256);

            var cell13 = headerRow.CreateCell(13);
            cell13.SetCellValue("核销结束时间");
            cell13.CellStyle = headerStyle;
            mySheet.SetColumnWidth(13, 10 * 256);

            var cell14 = headerRow.CreateCell(14);
            cell14.SetCellValue("核销服务券数量");
            cell14.CellStyle = headerStyle;
            mySheet.SetColumnWidth(14, 10 * 256);

            var cell15 = headerRow.CreateCell(15);
            cell15.SetCellValue("项目创建时间");
            cell15.CellStyle = headerStyle;
            mySheet.SetColumnWidth(15, 10 * 256);

            var cell16 = headerRow.CreateCell(16);
            cell16.SetCellValue("项目更新时间");
            cell16.CellStyle = headerStyle;
            mySheet.SetColumnWidth(16, 10 * 256);


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
                rowTemp1.SetCellValue(listModel[i].title.ToString());
                rowTemp1.CellStyle = commonCellStyle;



                var rowTemp2 = rowTemp.CreateCell(2);
                rowTemp2.SetCellValue(listModel[i].thumbnail.ToString());
                rowTemp2.CellStyle = commonCellStyle;



                var rowTemp3 = rowTemp.CreateCell(3);
                rowTemp3.SetCellValue(listModel[i].description.ToString());
                rowTemp3.CellStyle = commonCellStyle;



                var rowTemp4 = rowTemp.CreateCell(4);
                rowTemp4.SetCellValue(listModel[i].contentBody.ToString());
                rowTemp4.CellStyle = commonCellStyle;



                var rowTemp5 = rowTemp.CreateCell(5);
                rowTemp5.SetCellValue(listModel[i].allowedMembership.ToString());
                rowTemp5.CellStyle = commonCellStyle;



                var rowTemp6 = rowTemp.CreateCell(6);
                rowTemp6.SetCellValue(listModel[i].status.ToString());
                rowTemp6.CellStyle = commonCellStyle;



                var rowTemp7 = rowTemp.CreateCell(7);
                rowTemp7.SetCellValue(listModel[i].maxBuyNumber.ToString());
                rowTemp7.CellStyle = commonCellStyle;



                var rowTemp8 = rowTemp.CreateCell(8);
                rowTemp8.SetCellValue(listModel[i].amount.ToString());
                rowTemp8.CellStyle = commonCellStyle;



                var rowTemp9 = rowTemp.CreateCell(9);
                rowTemp9.SetCellValue(listModel[i].startTime.ToString());
                rowTemp9.CellStyle = commonCellStyle;



                var rowTemp10 = rowTemp.CreateCell(10);
                rowTemp10.SetCellValue(listModel[i].endTime.ToString());
                rowTemp10.CellStyle = commonCellStyle;



                var rowTemp11 = rowTemp.CreateCell(11);
                rowTemp11.SetCellValue(listModel[i].validityType.ToString());
                rowTemp11.CellStyle = commonCellStyle;



                var rowTemp12 = rowTemp.CreateCell(12);
                rowTemp12.SetCellValue(listModel[i].validityStartTime.ToString());
                rowTemp12.CellStyle = commonCellStyle;



                var rowTemp13 = rowTemp.CreateCell(13);
                rowTemp13.SetCellValue(listModel[i].validityEndTime.ToString());
                rowTemp13.CellStyle = commonCellStyle;



                var rowTemp14 = rowTemp.CreateCell(14);
                rowTemp14.SetCellValue(listModel[i].ticketNumber.ToString());
                rowTemp14.CellStyle = commonCellStyle;



                var rowTemp15 = rowTemp.CreateCell(15);
                rowTemp15.SetCellValue(listModel[i].createTime.ToString());
                rowTemp15.CellStyle = commonCellStyle;



                var rowTemp16 = rowTemp.CreateCell(16);
                rowTemp16.SetCellValue(listModel[i].updateTime.ToString());
                rowTemp16.CellStyle = commonCellStyle;


            }
            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsServices导出(查询结果).xls";
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
