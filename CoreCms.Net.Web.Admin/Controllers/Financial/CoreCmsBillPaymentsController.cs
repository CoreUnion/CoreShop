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
    ///     支付单表
    /// </summary>
    [Description("支付单表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsBillPaymentsController : ControllerBase
    {
        private readonly ICoreCmsBillPaymentsServices _coreCmsBillPaymentsServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsBillPaymentsServices"></param>
        public CoreCmsBillPaymentsController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsBillPaymentsServices coreCmsBillPaymentsServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsBillPaymentsServices = coreCmsBillPaymentsServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsBillPayments/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsBillPayments>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsBillPayments, object>> orderEx;
            switch (orderField)
            {
                case "paymentId":
                    orderEx = p => p.paymentId;
                    break;
                case "money":
                    orderEx = p => p.money;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "type":
                    orderEx = p => p.type;
                    break;
                case "status":
                    orderEx = p => p.status;
                    break;
                case "paymentCode":
                    orderEx = p => p.paymentCode;
                    break;
                case "ip":
                    orderEx = p => p.ip;
                    break;
                case "parameters":
                    orderEx = p => p.parameters;
                    break;
                case "payedMsg":
                    orderEx = p => p.payedMsg;
                    break;
                case "tradeNo":
                    orderEx = p => p.tradeNo;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                default:
                    orderEx = p => p.paymentId;
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

            //支付单号 nvarchar
            var paymentId = Request.Form["paymentId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentId)) where = where.And(p => p.paymentId.Contains(paymentId));

            //用户ID 关联user.id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) where = where.And(p => p.userId == userId);
            //资源类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) where = where.And(p => p.type == type);
            //支付状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) where = where.And(p => p.status == status);
            //支付类型编码 关联payments.code nvarchar
            var paymentCode = Request.Form["paymentCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentCode)) where = where.And(p => p.paymentCode.Contains(paymentCode));
            //支付单生成IP nvarchar
            var ip = Request.Form["ip"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ip)) where = where.And(p => p.ip.Contains(ip));
            //支付的时候需要的参数，存的是json格式的一维数组 nvarchar
            var parameters = Request.Form["parameters"].FirstOrDefault();
            if (!string.IsNullOrEmpty(parameters)) where = where.And(p => p.parameters.Contains(parameters));
            //支付回调后的状态描述 nvarchar
            var payedMsg = Request.Form["payedMsg"].FirstOrDefault();
            if (!string.IsNullOrEmpty(payedMsg)) where = where.And(p => p.payedMsg.Contains(payedMsg));
            //第三方平台交易流水号 nvarchar
            var tradeNo = Request.Form["tradeNo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(tradeNo)) where = where.And(p => p.tradeNo.Contains(tradeNo));
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
            var list = await _coreCmsBillPaymentsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent,
                pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsBillPayments/GetIndex
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

            var paymentsStatus = EnumHelper.EnumToList<GlobalEnumVars.BillPaymentsStatus>();
            var paymentsResourceTypes = EnumHelper.EnumToList<GlobalEnumVars.BillPaymentsType>();

            //订单支付方式
            var paymentCode = EnumHelper.EnumToList<GlobalEnumVars.PaymentsTypes>();

            jm.data = new
            {
                paymentsStatus,
                paymentsResourceTypes,
                paymentCode
            };

            return jm;
        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/CoreCmsBillPayments/GetDetails/10
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

            var model = await _coreCmsBillPaymentsServices.QueryByClauseAsync(p => p.paymentId == entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var paymentsStatus = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillPaymentsStatus>(model.status);
            var paymentsResourceTypes = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillPaymentsType>(model.type);
            var paymentCode = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.PaymentsTypes>(model.paymentCode);

            jm.code = 0;
            jm.data = new
            {
                model,
                paymentsStatus,
                paymentsResourceTypes,
                paymentCode,
            };


            return jm;
        }

        #endregion

        #region 选择导出============================================================

        // POST: Api/CoreCmsBillPayments/SelectExportExcel/10
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
                await _coreCmsBillPaymentsServices.QueryListByClauseAsync(p => entity.id.Contains(p.paymentId),
                    p => p.paymentId, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("支付单号");
            row1.CreateCell(1).SetCellValue("支付金额");
            row1.CreateCell(2).SetCellValue("用户ID 关联user.id");
            row1.CreateCell(3).SetCellValue("资源类型");
            row1.CreateCell(4).SetCellValue("支付状态");
            row1.CreateCell(5).SetCellValue("支付类型编码 关联payments.code");
            row1.CreateCell(6).SetCellValue("支付单生成IP");
            row1.CreateCell(7).SetCellValue("支付的时候需要的参数，存的是json格式的一维数组");
            row1.CreateCell(8).SetCellValue("支付回调后的状态描述");
            row1.CreateCell(9).SetCellValue("第三方平台交易流水号");
            row1.CreateCell(10).SetCellValue("创建时间");
            row1.CreateCell(11).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].paymentId);
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].money.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].paymentCode);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].ip);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].parameters);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].payedMsg);
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].tradeNo);
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillPayments导出(选择结果).xls";
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

        // POST: Api/CoreCmsBillPayments/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsBillPayments>();
            //查询筛选

            //支付单号 nvarchar
            var paymentId = Request.Form["paymentId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentId)) where = where.And(p => p.paymentId.Contains(paymentId));
            //用户ID 关联user.id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) where = where.And(p => p.userId == userId);
            //资源类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) where = where.And(p => p.type == type);
            //支付状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) where = where.And(p => p.status == status);
            //支付类型编码 关联payments.code nvarchar
            var paymentCode = Request.Form["paymentCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentCode)) where = where.And(p => p.paymentCode.Contains(paymentCode));
            //支付单生成IP nvarchar
            var ip = Request.Form["ip"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ip)) where = where.And(p => p.ip.Contains(ip));
            //支付的时候需要的参数，存的是json格式的一维数组 nvarchar
            var parameters = Request.Form["parameters"].FirstOrDefault();
            if (!string.IsNullOrEmpty(parameters)) where = where.And(p => p.parameters.Contains(parameters));
            //支付回调后的状态描述 nvarchar
            var payedMsg = Request.Form["payedMsg"].FirstOrDefault();
            if (!string.IsNullOrEmpty(payedMsg)) where = where.And(p => p.payedMsg.Contains(payedMsg));
            //第三方平台交易流水号 nvarchar
            var tradeNo = Request.Form["tradeNo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(tradeNo)) where = where.And(p => p.tradeNo.Contains(tradeNo));
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
                await _coreCmsBillPaymentsServices.QueryListByClauseAsync(where, p => p.paymentId, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("支付单号");
            row1.CreateCell(1).SetCellValue("支付金额");
            row1.CreateCell(2).SetCellValue("用户ID 关联user.id");
            row1.CreateCell(3).SetCellValue("资源类型");
            row1.CreateCell(4).SetCellValue("支付状态");
            row1.CreateCell(5).SetCellValue("支付类型编码 关联payments.code");
            row1.CreateCell(6).SetCellValue("支付单生成IP");
            row1.CreateCell(7).SetCellValue("支付的时候需要的参数，存的是json格式的一维数组");
            row1.CreateCell(8).SetCellValue("支付回调后的状态描述");
            row1.CreateCell(9).SetCellValue("第三方平台交易流水号");
            row1.CreateCell(10).SetCellValue("创建时间");
            row1.CreateCell(11).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].paymentId);
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].money.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].paymentCode);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].ip);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].parameters);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].payedMsg);
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].tradeNo);
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillPayments导出(查询结果).xls";
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