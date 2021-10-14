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
    ///     退款单表
    /// </summary>
    [Description("退款单表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsBillRefundController : ControllerBase
    {
        private readonly ICoreCmsBillRefundServices _coreCmsBillRefundServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsBillRefundServices"></param>
        /// <param name="userServices"></param>
        public CoreCmsBillRefundController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsBillRefundServices coreCmsBillRefundServices, ICoreCmsUserServices userServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsBillRefundServices = coreCmsBillRefundServices;
            _userServices = userServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsBillRefund/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsBillRefund>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsBillRefund, object>> orderEx;
            switch (orderField)
            {
                case "refundId":
                    orderEx = p => p.refundId;
                    break;
                case "aftersalesId":
                    orderEx = p => p.aftersalesId;
                    break;
                case "money":
                    orderEx = p => p.money;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "sourceId":
                    orderEx = p => p.sourceId;
                    break;
                case "type":
                    orderEx = p => p.type;
                    break;
                case "paymentCode":
                    orderEx = p => p.paymentCode;
                    break;
                case "tradeNo":
                    orderEx = p => p.tradeNo;
                    break;
                case "status":
                    orderEx = p => p.status;
                    break;
                case "memo":
                    orderEx = p => p.memo;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                default:
                    orderEx = p => p.refundId;
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

            //退款单ID nvarchar
            var refundId = Request.Form["refundId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(refundId)) where = where.And(p => p.refundId.Contains(refundId));
            //售后单id nvarchar
            var aftersalesId = Request.Form["aftersalesId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(aftersalesId)) where = where.And(p => p.aftersalesId.Contains(aftersalesId));
            //用户ID 关联user.id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) where = where.And(p => p.userId == userId);
            //资源id，根据type不同而关联不同的表 nvarchar
            var sourceId = Request.Form["sourceId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sourceId)) where = where.And(p => p.sourceId.Contains(sourceId));
            //资源类型1=订单,2充值单 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) where = where.And(p => p.type == type);
            //退款支付类型编码 默认原路返回 关联支付单表支付编码 nvarchar
            var paymentCode = Request.Form["paymentCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentCode)) where = where.And(p => p.paymentCode.Contains(paymentCode));
            //第三方平台交易流水号 nvarchar
            var tradeNo = Request.Form["tradeNo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(tradeNo)) where = where.And(p => p.tradeNo.Contains(tradeNo));
            //状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) where = where.And(p => p.status == status);
            //退款失败原因 nvarchar
            var memo = Request.Form["memo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(memo)) where = where.And(p => p.memo.Contains(memo));
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
            var list = await _coreCmsBillRefundServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsBillRefund/GetIndex
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
            var paymentsResourceTypes = EnumHelper.EnumToList<GlobalEnumVars.BillPaymentsType>();
            var refundStatus = EnumHelper.EnumToList<GlobalEnumVars.BillRefundStatus>();
            //订单支付方式
            var paymentCode = EnumHelper.EnumToList<GlobalEnumVars.PaymentsTypes>();
            jm.data = new
            {
                paymentsResourceTypes,
                refundStatus,
                paymentCode
            };

            return jm;
        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/CoreCmsBillRefund/GetDetails/10
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

            var model = await _coreCmsBillRefundServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var paymentsResourceTypes = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillPaymentsType>(model.type);
            var refundStatus = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillRefundStatus>(model.status);
            var paymentCode = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.PaymentsTypes>(model.paymentCode);

            jm.code = 0;
            jm.data = model;

            jm.data = new
            {
                paymentsResourceTypes,
                refundStatus,
                paymentCode,
                model
            };

            return jm;
        }

        #endregion


        #region 审核退款单============================================================

        // POST: Api/CoreCmsBillRefund/GetAudit
        /// <summary>
        ///     审核退款单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("审核退款单")]
        public async Task<AdminUiCallBack> GetAudit([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsBillRefundServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var paymentsResourceTypes =
                EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillPaymentsType>(model.type);
            var refundStatus = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillRefundStatus>(model.status);
            var paymentCode = EnumHelper.EnumToList<GlobalEnumVars.PaymentsTypes>();
            var userInfo = await _userServices.QueryByClauseAsync(p => p.id == model.userId);
            jm.code = 0;
            jm.data = new
            {
                paymentsResourceTypes,
                refundStatus,
                paymentCode,
                model,
                userInfo
            };

            return jm;
        }

        #endregion

        #region 提交审核结果============================================================

        // POST: Api/CoreCmsBillRefund/Edit
        /// <summary>
        ///     提交审核结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("提交审核结果")]
        public async Task<AdminUiCallBack> DoAudit([FromBody] FMDoAuditPost entity)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(entity.refundId))
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }

            if (string.IsNullOrEmpty(entity.paymentCode))
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }

            if (entity.status != 2 && entity.status != 4)
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }

            var result =
                await _coreCmsBillRefundServices.ToRefund(entity.refundId, entity.status, entity.paymentCode);

            //事物处理过程结束
            jm.code = result.status ? 0 : 1;
            jm.msg = result.msg;
            jm.data = result.data;

            return jm;
        }

        #endregion

        #region 退款失败状态再次退款============================================================

        // POST: Api/CoreCmsBillRefund/Edit
        /// <summary>
        ///     退款失败状态再次退款
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("退款失败状态再次退款")]
        public async Task<AdminUiCallBack> DoReAudit([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(entity.id))
            {
                jm.msg = GlobalErrorCodeVars.Code13215;
                return jm;
            }

            var oldModel = await _coreCmsBillRefundServices.QueryByClauseAsync(p =>
                p.refundId == entity.id && p.status == (int)GlobalEnumVars.BillRefundStatus.STATUS_FAIL);

            if (oldModel == null)
            {
                jm.msg = GlobalErrorCodeVars.Code13224;
                return jm;
            }

            var result = await _coreCmsBillRefundServices.PaymentRefund(entity.id);

            //事物处理过程结束
            jm.code = result.status ? 0 : 1;
            jm.msg = result.msg;
            jm.data = result.data;

            return jm;
        }

        #endregion


        #region 选择导出============================================================

        // POST: Api/CoreCmsBillRefund/SelectExportExcel/10
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
                await _coreCmsBillRefundServices.QueryListByClauseAsync(p => entity.id.Contains(p.refundId),
                    p => p.refundId, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("退款单ID");
            row1.CreateCell(1).SetCellValue("售后单id");
            row1.CreateCell(2).SetCellValue("退款金额");
            row1.CreateCell(3).SetCellValue("用户ID 关联user.id");
            row1.CreateCell(4).SetCellValue("资源id，根据type不同而关联不同的表");
            row1.CreateCell(5).SetCellValue("资源类型1=订单,2充值单");
            row1.CreateCell(6).SetCellValue("退款支付类型编码 默认原路返回 关联支付单表支付编码");
            row1.CreateCell(7).SetCellValue("第三方平台交易流水号");
            row1.CreateCell(8).SetCellValue("状态");
            row1.CreateCell(9).SetCellValue("退款失败原因");
            row1.CreateCell(10).SetCellValue("创建时间");
            row1.CreateCell(11).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].refundId);
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].aftersalesId);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].money.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].sourceId);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].paymentCode);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].tradeNo);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].memo);
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillRefund导出(选择结果).xls";
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

        // POST: Api/CoreCmsBillRefund/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsBillRefund>();
            //查询筛选

            //退款单ID nvarchar
            var refundId = Request.Form["refundId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(refundId)) where = where.And(p => p.refundId.Contains(refundId));
            //售后单id nvarchar
            var aftersalesId = Request.Form["aftersalesId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(aftersalesId))
                where = where.And(p => p.aftersalesId.Contains(aftersalesId));
            //用户ID 关联user.id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) where = where.And(p => p.userId == userId);
            //资源id，根据type不同而关联不同的表 nvarchar
            var sourceId = Request.Form["sourceId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sourceId)) where = where.And(p => p.sourceId.Contains(sourceId));
            //资源类型1=订单,2充值单 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) where = where.And(p => p.type == type);
            //退款支付类型编码 默认原路返回 关联支付单表支付编码 nvarchar
            var paymentCode = Request.Form["paymentCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentCode)) where = where.And(p => p.paymentCode.Contains(paymentCode));
            //第三方平台交易流水号 nvarchar
            var tradeNo = Request.Form["tradeNo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(tradeNo)) where = where.And(p => p.tradeNo.Contains(tradeNo));
            //状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) where = where.And(p => p.status == status);
            //退款失败原因 nvarchar
            var memo = Request.Form["memo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(memo)) where = where.And(p => p.memo.Contains(memo));
            //创建时间 datetime
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
                await _coreCmsBillRefundServices.QueryListByClauseAsync(where, p => p.refundId, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("退款单ID");
            row1.CreateCell(1).SetCellValue("售后单id");
            row1.CreateCell(2).SetCellValue("退款金额");
            row1.CreateCell(3).SetCellValue("用户ID 关联user.id");
            row1.CreateCell(4).SetCellValue("资源id，根据type不同而关联不同的表");
            row1.CreateCell(5).SetCellValue("资源类型1=订单,2充值单");
            row1.CreateCell(6).SetCellValue("退款支付类型编码 默认原路返回 关联支付单表支付编码");
            row1.CreateCell(7).SetCellValue("第三方平台交易流水号");
            row1.CreateCell(8).SetCellValue("状态");
            row1.CreateCell(9).SetCellValue("退款失败原因");
            row1.CreateCell(10).SetCellValue("创建时间");
            row1.CreateCell(11).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].refundId);
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].aftersalesId);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].money.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].sourceId);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].paymentCode);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].tradeNo);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].memo);
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillRefund导出(查询结果).xls";
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