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
    ///     售后单
    /// </summary>
    [Description("售后单")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsBillAftersalesController : ControllerBase
    {
        private readonly ICoreCmsBillAftersalesServices _coreCmsBillAftersalesServices;
        private readonly ICoreCmsBillAftersalesImagesServices _imagesServices;
        private readonly ICoreCmsBillAftersalesItemServices _itemServices;
        private readonly ICoreCmsOrderServices _orderServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsBillAftersalesController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsBillAftersalesServices coreCmsBillAftersalesServices
            , ICoreCmsUserServices userServices
            , ICoreCmsBillAftersalesItemServices itemServices
            , ICoreCmsBillAftersalesImagesServices imagesServices
            , ICoreCmsOrderServices orderServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsBillAftersalesServices = coreCmsBillAftersalesServices;
            _userServices = userServices;
            _itemServices = itemServices;
            _imagesServices = imagesServices;
            _orderServices = orderServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsBillAftersales/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsBillAftersales>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsBillAftersales, object>> orderEx;
            switch (orderField)
            {
                case "aftersalesId":
                    orderEx = p => p.aftersalesId;
                    break;
                case "orderId":
                    orderEx = p => p.orderId;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "type":
                    orderEx = p => p.type;
                    break;
                case "refundAmount":
                    orderEx = p => p.refundAmount;
                    break;
                case "status":
                    orderEx = p => p.status;
                    break;
                case "reason":
                    orderEx = p => p.reason;
                    break;
                case "mark":
                    orderEx = p => p.mark;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                default:
                    orderEx = p => p.createTime;
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

            //售后单id nvarchar
            var aftersalesId = Request.Form["aftersalesId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(aftersalesId)) @where = @where.And(p => p.aftersalesId.Contains(aftersalesId));
            //订单ID nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId)) @where = @where.And(p => p.orderId.Contains(orderId));
            //用户ID int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //售后类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) @where = @where.And(p => p.type == type);
            //状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) @where = @where.And(p => p.status == status);
            //退款原因 nvarchar
            var reason = Request.Form["reason"].FirstOrDefault();
            if (!string.IsNullOrEmpty(reason)) @where = @where.And(p => p.reason.Contains(reason));
            //卖家备注，如果审核失败了，会显示到前端 nvarchar
            var mark = Request.Form["mark"].FirstOrDefault();
            if (!string.IsNullOrEmpty(mark)) @where = @where.And(p => p.mark.Contains(mark));
            //提交时间 datetime
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

            //更新时间 datetime
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
            var list = await _coreCmsBillAftersalesServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);

            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsBillAftersales/GetIndex
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

        #region 审核数据============================================================

        // POST: Api/CoreCmsBillAftersales/GetEdit
        /// <summary>
        ///     审核数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("审核数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsBillAftersalesServices.PreAudit(entity.id);
            if (!model.status)
            {
                jm.msg = model.msg;
                return jm;
            }

            jm.code = 0;
            jm.data = model.data;

            return jm;
        }

        #endregion

        #region 审核提交============================================================

        // POST: Api/CoreCmsBillAftersales/Edit
        /// <summary>
        ///     审核提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("审核提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] FMBillAftersalesAddPost entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.status == 0)
            {
                jm.msg = "请选择审核状态";
                return jm;
            }

            var res = await _coreCmsBillAftersalesServices.Audit(entity.aftersalesId, entity.status, entity.type,
                entity.refund, entity.mark, entity.items);
            jm.code = res.status ? 0 : 1;
            jm.msg = res.msg;
            jm.data = res.data;
            jm.otherData = entity;

            return jm;
        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/CoreCmsBillAftersales/GetDetails/10
        /// <summary>
        ///     预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsBillAftersalesServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var userModel = await _userServices.QueryByClauseAsync(p => p.id == model.userId);
            model.userNickName = userModel != null ? userModel.nickName : "";
            model.statusName =
                EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillAftersalesStatus>(model.status);
            model.images = await _imagesServices.QueryListByClauseAsync(p => p.aftersalesId == model.aftersalesId);
            model.items = await _itemServices.QueryListByClauseAsync(p => p.aftersalesId == model.aftersalesId);

            jm.code = 0;
            jm.data = model;


            return jm;
        }

        #endregion

        #region 选择导出============================================================

        // POST: Api/CoreCmsBillAftersales/SelectExportExcel/10
        /// <summary>
        ///     选择导出
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("选择导出")]
        public async Task<AdminUiCallBack> SelectExportExcel([FromBody] FMArrayStringIds entity)
        {
            var jm = new AdminUiCallBack();

            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel =
                await _coreCmsBillAftersalesServices.QueryListByClauseAsync(p => entity.id.Contains(p.aftersalesId),
                    p => p.createTime, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("售后单id");
            row1.CreateCell(1).SetCellValue("订单ID");
            row1.CreateCell(2).SetCellValue("用户ID");
            row1.CreateCell(3).SetCellValue("售后类型");
            row1.CreateCell(4).SetCellValue("退款金额");
            row1.CreateCell(5).SetCellValue("状态");
            row1.CreateCell(6).SetCellValue("退款原因");
            row1.CreateCell(7).SetCellValue("卖家备注，如果审核失败了，会显示到前端");
            row1.CreateCell(8).SetCellValue("提交时间");
            row1.CreateCell(9).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].aftersalesId);
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].orderId);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].refundAmount.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].reason);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].mark);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillAftersales导出(选择结果).xls";
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

        // POST: Api/CoreCmsBillAftersales/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsBillAftersales>();
            //查询筛选

            //售后单id nvarchar
            var aftersalesId = Request.Form["aftersalesId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(aftersalesId))
                @where = @where.And(p => p.aftersalesId.Contains(aftersalesId));
            //订单ID nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId)) @where = @where.And(p => p.orderId.Contains(orderId));
            //用户ID int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //售后类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) @where = @where.And(p => p.type == type);
            //状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) @where = @where.And(p => p.status == status);
            //退款原因 nvarchar
            var reason = Request.Form["reason"].FirstOrDefault();
            if (!string.IsNullOrEmpty(reason)) @where = @where.And(p => p.reason.Contains(reason));
            //卖家备注，如果审核失败了，会显示到前端 nvarchar
            var mark = Request.Form["mark"].FirstOrDefault();
            if (!string.IsNullOrEmpty(mark)) @where = @where.And(p => p.mark.Contains(mark));
            //提交时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                var dt = createTime.ObjectToDate();
                where = where.And(p => p.createTime > dt);
            }

            //更新时间 datetime
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
            var listmodel =
                await _coreCmsBillAftersalesServices.QueryListByClauseAsync(where, p => p.createTime,
                    OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("售后单id");
            row1.CreateCell(1).SetCellValue("订单ID");
            row1.CreateCell(2).SetCellValue("用户ID");
            row1.CreateCell(3).SetCellValue("售后类型");
            row1.CreateCell(4).SetCellValue("退款金额");
            row1.CreateCell(5).SetCellValue("状态");
            row1.CreateCell(6).SetCellValue("退款原因");
            row1.CreateCell(7).SetCellValue("卖家备注");
            row1.CreateCell(8).SetCellValue("提交时间");
            row1.CreateCell(9).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].aftersalesId);
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].orderId);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].refundAmount.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].reason);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].mark);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillAftersales导出(查询结果).xls";
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