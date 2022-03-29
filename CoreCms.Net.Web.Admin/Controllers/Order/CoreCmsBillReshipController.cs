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
    ///     退货单表
    /// </summary>
    [Description("退货单表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsBillReshipController : ControllerBase
    {
        private readonly ICoreCmsBillReshipServices _coreCmsBillReshipServices;
        private readonly ICoreCmsBillReshipItemServices _billReshipItemServices;
        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly ICoreCmsStockServices _stockServices;
        private readonly ICoreCmsStockLogServices _stockLogServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsBillReshipController(IWebHostEnvironment webHostEnvironment,
            ICoreCmsBillReshipServices coreCmsBillReshipServices, ICoreCmsBillReshipItemServices billReshipItemServices, ICoreCmsGoodsServices goodsServices, ICoreCmsStockServices stockServices, ICoreCmsStockLogServices stockLogServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsBillReshipServices = coreCmsBillReshipServices;
            _billReshipItemServices = billReshipItemServices;
            _goodsServices = goodsServices;
            _stockServices = stockServices;
            _stockLogServices = stockLogServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsBillReship/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsBillReship>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsBillReship, object>> orderEx;
            switch (orderField)
            {
                case "reshipId":
                    orderEx = p => p.reshipId;
                    break;
                case "orderId":
                    orderEx = p => p.orderId;
                    break;
                case "aftersalesId":
                    orderEx = p => p.aftersalesId;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "logiCode":
                    orderEx = p => p.logiCode;
                    break;
                case "logiNo":
                    orderEx = p => p.logiNo;
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
                    orderEx = p => p.reshipId;
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

            //退货单号 nvarchar
            var reshipId = Request.Form["reshipId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(reshipId)) @where = @where.And(p => p.reshipId.Contains(reshipId));
            //订单序列 nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId)) @where = @where.And(p => p.orderId.Contains(orderId));
            //售后单序列 nvarchar
            var aftersalesId = Request.Form["aftersalesId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(aftersalesId)) @where = @where.And(p => p.aftersalesId.Contains(aftersalesId));
            //用户ID 关联user.id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //物流公司编码 nvarchar
            var logiCode = Request.Form["logiCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiCode)) @where = @where.And(p => p.logiCode.Contains(logiCode));
            //物流单号 nvarchar
            var logiNo = Request.Form["logiNo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiNo)) @where = @where.And(p => p.logiNo.Contains(logiNo));
            //状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) @where = @where.And(p => p.status == status);
            //备注 nvarchar
            var memo = Request.Form["memo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(memo)) @where = @where.And(p => p.memo.Contains(memo));
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
            var list = await _coreCmsBillReshipServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsBillReship/GetIndex
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

            var status = EnumHelper.EnumToList<GlobalEnumVars.BillReshipStatus>();

            jm.data = new
            {
                status
            };

            return jm;
        }

        #endregion

        #region 确认收货============================================================

        // POST: Api/CoreCmsBillReship/DoDelete/10
        /// <summary>
        ///     确认收货
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("确认收货")]
        public async Task<AdminUiCallBack> DoAudit([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsBillReshipServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var bl = await _coreCmsBillReshipServices.UpdateAsync(
                p => new CoreCmsBillReship
                { status = (int)GlobalEnumVars.BillReshipStatus.已收退货, updateTime = DateTime.Now },
                p => p.reshipId == entity.id);

            if (bl)
            {
                //调整库存
                var items = await _billReshipItemServices.QueryListByClauseAsync(p => p.reshipId == entity.id);
                var stockLogs = new List<CoreCmsStockLog>();
                foreach (var item in items)
                {
                    _goodsServices.ChangeStock(item.productId, GlobalEnumVars.OrderChangeStockType.@return.ToString(), item.nums);

                    var sLog = new CoreCmsStockLog
                    {
                        stockId = entity.id,
                        productId = item.productId,
                        goodsId = item.goodsId,
                        nums = item.nums,
                        sn = item.sn,
                        bn = item.bn,
                        goodsName = item.name,
                        spesDesc = item.addon
                    };
                    stockLogs.Add(sLog);

                }

                //库存管理日志

                var stock = new CoreCmsStock
                {
                    manager = 0,
                    id = entity.id,
                    createTime = DateTime.Now,
                    type = (int)GlobalEnumVars.StockType.ReturnedGoods,
                    memo = "退货单退货"
                };
                await _stockServices.InsertAsync(stock);
                await _stockLogServices.InsertAsync(stockLogs);
            }

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "确认成功" : "确认失败";
            return jm;
        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/CoreCmsBillReship/GetDetails/10
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

            var model = await _coreCmsBillReshipServices.GetDetails(p => p.reshipId == entity.id, p => p.createTime, OrderByType.Desc);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            var status = EnumHelper.EnumToList<GlobalEnumVars.BillReshipStatus>();

            jm.code = 0;
            jm.data = new
            {
                status,
                model
            };

            return jm;
        }

        #endregion

        #region 选择导出============================================================

        // POST: Api/CoreCmsBillReship/SelectExportExcel/10
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
                await _coreCmsBillReshipServices.QueryListByClauseAsync(p => entity.id.Contains(p.reshipId),
                    p => p.reshipId, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("退货单号");
            row1.CreateCell(1).SetCellValue("订单序列");
            row1.CreateCell(2).SetCellValue("售后单序列");
            row1.CreateCell(3).SetCellValue("用户ID 关联user.id");
            row1.CreateCell(4).SetCellValue("物流公司编码");
            row1.CreateCell(5).SetCellValue("物流单号");
            row1.CreateCell(6).SetCellValue("状态");
            row1.CreateCell(7).SetCellValue("备注");
            row1.CreateCell(8).SetCellValue("创建时间");
            row1.CreateCell(9).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].reshipId);
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].orderId);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].aftersalesId);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].logiCode);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].logiNo);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].memo);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillReship导出(选择结果).xls";
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

        // POST: Api/CoreCmsBillReship/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsBillReship>();
            //查询筛选

            //退货单号 nvarchar
            var reshipId = Request.Form["reshipId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(reshipId)) @where = @where.And(p => p.reshipId.Contains(reshipId));
            //订单序列 nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId)) @where = @where.And(p => p.orderId.Contains(orderId));
            //售后单序列 nvarchar
            var aftersalesId = Request.Form["aftersalesId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(aftersalesId))
                @where = @where.And(p => p.aftersalesId.Contains(aftersalesId));
            //用户ID 关联user.id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //物流公司编码 nvarchar
            var logiCode = Request.Form["logiCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiCode)) @where = @where.And(p => p.logiCode.Contains(logiCode));
            //物流单号 nvarchar
            var logiNo = Request.Form["logiNo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiNo)) @where = @where.And(p => p.logiNo.Contains(logiNo));
            //状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) @where = @where.And(p => p.status == status);
            //备注 nvarchar
            var memo = Request.Form["memo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(memo)) @where = @where.And(p => p.memo.Contains(memo));
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
                await _coreCmsBillReshipServices.QueryListByClauseAsync(where, p => p.reshipId, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("退货单号");
            row1.CreateCell(1).SetCellValue("订单序列");
            row1.CreateCell(2).SetCellValue("售后单序列");
            row1.CreateCell(3).SetCellValue("用户ID 关联user.id");
            row1.CreateCell(4).SetCellValue("物流公司编码");
            row1.CreateCell(5).SetCellValue("物流单号");
            row1.CreateCell(6).SetCellValue("状态");
            row1.CreateCell(7).SetCellValue("备注");
            row1.CreateCell(8).SetCellValue("创建时间");
            row1.CreateCell(9).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].reshipId);
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].orderId);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].aftersalesId);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].logiCode);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].logiNo);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].memo);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillReship导出(查询结果).xls";
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