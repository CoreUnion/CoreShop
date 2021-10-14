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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     发货单表
    /// </summary>
    [Description("发货单表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsBillDeliveryController : ControllerBase
    {
        private readonly ICoreCmsAreaServices _areaServices;
        private readonly ICoreCmsBillDeliveryServices _coreCmsBillDeliveryServices;
        private readonly ICoreCmsBillDeliveryItemServices _itemServices;
        private readonly ICoreCmsLogisticsServices _logisticsServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsBillDeliveryController(IWebHostEnvironment webHostEnvironment,
            ICoreCmsBillDeliveryServices coreCmsBillDeliveryServices, ICoreCmsAreaServices areaServices,
            ICoreCmsBillDeliveryItemServices itemServices, ICoreCmsLogisticsServices logisticsServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsBillDeliveryServices = coreCmsBillDeliveryServices;
            _areaServices = areaServices;
            _itemServices = itemServices;
            _logisticsServices = logisticsServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsBillDelivery/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsBillDelivery>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsBillDelivery, object>> orderEx;
            switch (orderField)
            {
                case "deliveryId":
                    orderEx = p => p.deliveryId;
                    break;
                case "logiCode":
                    orderEx = p => p.logiCode;
                    break;
                case "logiNo":
                    orderEx = p => p.logiNo;
                    break;
                case "logiInformation":
                    orderEx = p => p.logiInformation;
                    break;
                case "logiStatus":
                    orderEx = p => p.logiStatus;
                    break;
                case "shipAreaId":
                    orderEx = p => p.shipAreaId;
                    break;
                case "shipAddress":
                    orderEx = p => p.shipAddress;
                    break;
                case "shipName":
                    orderEx = p => p.shipName;
                    break;
                case "shipMobile":
                    orderEx = p => p.shipMobile;
                    break;
                case "status":
                    orderEx = p => p.status;
                    break;
                case "memo":
                    orderEx = p => p.memo;
                    break;
                case "confirmTime":
                    orderEx = p => p.confirmTime;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                default:
                    orderEx = p => p.deliveryId;
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

            //发货单序列 nvarchar
            var deliveryId = Request.Form["deliveryId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(deliveryId)) @where = @where.And(p => p.deliveryId.Contains(deliveryId));
            //物流公司编码 nvarchar
            var logiCode = Request.Form["logiCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiCode)) @where = @where.And(p => p.logiCode.Contains(logiCode));
            //物流单号 nvarchar
            var logiNo = Request.Form["logiNo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiNo)) @where = @where.And(p => p.logiNo.Contains(logiNo));
            //快递物流信息 nvarchar
            var logiInformation = Request.Form["logiInformation"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiInformation))
                @where = @where.And(p => p.logiInformation.Contains(logiInformation));
            //快递是否不更新 bit
            var logiStatus = Request.Form["logiStatus"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiStatus) && logiStatus.ToLowerInvariant() == "true")
                @where = @where.And(p => p.logiStatus);
            else if (!string.IsNullOrEmpty(logiStatus) && logiStatus.ToLowerInvariant() == "false")
                @where = @where.And(p => p.logiStatus == false);
            //收货地区ID int
            var shipAreaId = Request.Form["shipAreaId"].FirstOrDefault().ObjectToInt(0);
            if (shipAreaId > 0) @where = @where.And(p => p.shipAreaId == shipAreaId);
            //收货详细地址 nvarchar
            var shipAddress = Request.Form["shipAddress"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipAddress)) @where = @where.And(p => p.shipAddress.Contains(shipAddress));
            //收货人姓名 nvarchar
            var shipName = Request.Form["shipName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipName)) @where = @where.And(p => p.shipName.Contains(shipName));
            //收货电话 nvarchar
            var shipMobile = Request.Form["shipMobile"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipMobile)) @where = @where.And(p => p.shipMobile.Contains(shipMobile));
            //状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) @where = @where.And(p => p.status == status);
            //备注 nvarchar
            var memo = Request.Form["memo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(memo)) @where = @where.And(p => p.memo.Contains(memo));
            //确认收货时间 datetime
            var confirmTime = Request.Form["confirmTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(confirmTime))
            {
                if (confirmTime.Contains("到"))
                {
                    var dts = confirmTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.confirmTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.confirmTime < dtEnd);
                }
                else
                {
                    var dt = confirmTime.ObjectToDate();
                    where = where.And(p => p.confirmTime > dt);
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
            var list = await _coreCmsBillDeliveryServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent,
                pageSize);

            if (list.Any())
            {
                var logist = await _logisticsServices.QueryAsync();
                var areaCache =await _areaServices.GetCaChe();
                foreach (var item in list)
                {
                    if (item.shipAreaId > 0)
                    {
                        var result =await _areaServices.GetAreaFullName(item.shipAreaId, areaCache);
                        if (result.status) item.shipAddress = result.data + item.shipAddress;
                    }


                    if (!string.IsNullOrEmpty(item.logiCode))
                    {
                        var logiModel = logist.Find(p => p.logiCode == item.logiCode);
                        if (logiModel != null) item.logiName = logiModel.logiName;
                    }
                }
            }

            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsBillDelivery/GetIndex
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

        #region 编辑数据============================================================

        // POST: Api/CoreCmsBillDelivery/GetEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsBillDeliveryServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;

            var logistics = await _logisticsServices.QueryListByClauseAsync(p => p.isDelete == false);

            jm.code = 0;
            jm.data = new
            {
                model,
                logistics
            };

            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Api/CoreCmsBillDelivery/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsBillDelivery entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsBillDeliveryServices.QueryByIdAsync(entity.deliveryId);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            //事物处理过程开始
            oldModel.logiCode = entity.logiCode;
            oldModel.logiNo = entity.logiNo;
            oldModel.shipAreaId = entity.shipAreaId;
            oldModel.shipAddress = entity.shipAddress;
            oldModel.shipName = entity.shipName;
            oldModel.shipMobile = entity.shipMobile;
            oldModel.memo = entity.memo;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await _coreCmsBillDeliveryServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;


            return jm;
        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/CoreCmsBillDelivery/GetDetails/10
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

            var model = await _coreCmsBillDeliveryServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            if (model.shipAreaId > 0)
            {
                var result =await _areaServices.GetAreaFullName(model.shipAreaId);
                if (result.status) model.shipAddress = result.data + model.shipAddress;
            }

            if (!string.IsNullOrEmpty(model.logiCode))
            {
                var logiModel = await _logisticsServices.QueryByClauseAsync(p => p.logiCode == model.logiCode);
                ;
                if (logiModel != null) model.logiName = logiModel.logiName;
            }

            var items = await _itemServices.QueryListByClauseAsync(p => p.deliveryId == model.deliveryId,
                p => p.id, OrderByType.Asc);
            jm.code = 0;
            jm.data = new
            {
                model,
                items
            };

            return jm;
        }

        #endregion


        #region 选择导出============================================================

        // POST: Api/CoreCmsBillDelivery/SelectExportExcel/10
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
                await _coreCmsBillDeliveryServices.QueryListByClauseAsync(p => entity.id.Contains(p.deliveryId),
                    p => p.deliveryId, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("发货单序列");
            row1.CreateCell(1).SetCellValue("物流公司编码");
            row1.CreateCell(2).SetCellValue("物流单号");
            row1.CreateCell(3).SetCellValue("快递物流信息");
            row1.CreateCell(4).SetCellValue("快递是否不更新");
            row1.CreateCell(5).SetCellValue("收货地区ID");
            row1.CreateCell(6).SetCellValue("收货详细地址");
            row1.CreateCell(7).SetCellValue("收货人姓名");
            row1.CreateCell(8).SetCellValue("收货电话");
            row1.CreateCell(9).SetCellValue("状态");
            row1.CreateCell(10).SetCellValue("备注");
            row1.CreateCell(11).SetCellValue("确认收货时间");
            row1.CreateCell(12).SetCellValue("创建时间");
            row1.CreateCell(13).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].deliveryId);
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].logiCode);
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].logiNo);
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].logiInformation);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].logiStatus.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].shipAreaId.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].shipAddress);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].shipName);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].shipMobile);
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].memo);
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].confirmTime.ToString());
                rowtemp.CreateCell(12).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(13).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillDelivery导出(选择结果).xls";
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

        // POST: Api/CoreCmsBillDelivery/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

                var where = PredicateBuilder.True<CoreCmsBillDelivery>();
                //查询筛选

                //发货单序列 nvarchar
                var deliveryId = Request.Form["deliveryId"].FirstOrDefault();
                if (!string.IsNullOrEmpty(deliveryId)) @where = @where.And(p => p.deliveryId.Contains(deliveryId));
                //物流公司编码 nvarchar
                var logiCode = Request.Form["logiCode"].FirstOrDefault();
                if (!string.IsNullOrEmpty(logiCode)) @where = @where.And(p => p.logiCode.Contains(logiCode));
                //物流单号 nvarchar
                var logiNo = Request.Form["logiNo"].FirstOrDefault();
                if (!string.IsNullOrEmpty(logiNo)) @where = @where.And(p => p.logiNo.Contains(logiNo));
                //快递物流信息 nvarchar
                var logiInformation = Request.Form["logiInformation"].FirstOrDefault();
                if (!string.IsNullOrEmpty(logiInformation))
                    @where = @where.And(p => p.logiInformation.Contains(logiInformation));
                //快递是否不更新 bit
                var logiStatus = Request.Form["logiStatus"].FirstOrDefault();
                if (!string.IsNullOrEmpty(logiStatus) && logiStatus.ToLowerInvariant() == "true")
                    @where = @where.And(p => p.logiStatus);
                else if (!string.IsNullOrEmpty(logiStatus) && logiStatus.ToLowerInvariant() == "false")
                    @where = @where.And(p => p.logiStatus == false);
                //收货地区ID int
                var shipAreaId = Request.Form["shipAreaId"].FirstOrDefault().ObjectToInt(0);
                if (shipAreaId > 0) @where = @where.And(p => p.shipAreaId == shipAreaId);
                //收货详细地址 nvarchar
                var shipAddress = Request.Form["shipAddress"].FirstOrDefault();
                if (!string.IsNullOrEmpty(shipAddress)) @where = @where.And(p => p.shipAddress.Contains(shipAddress));
                //收货人姓名 nvarchar
                var shipName = Request.Form["shipName"].FirstOrDefault();
                if (!string.IsNullOrEmpty(shipName)) @where = @where.And(p => p.shipName.Contains(shipName));
                //收货电话 nvarchar
                var shipMobile = Request.Form["shipMobile"].FirstOrDefault();
                if (!string.IsNullOrEmpty(shipMobile)) @where = @where.And(p => p.shipMobile.Contains(shipMobile));
                //状态 int
                var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
                if (status > 0) @where = @where.And(p => p.status == status);
                //备注 nvarchar
                var memo = Request.Form["memo"].FirstOrDefault();
                if (!string.IsNullOrEmpty(memo)) @where = @where.And(p => p.memo.Contains(memo));
                //确认收货时间 datetime
                var confirmTime = Request.Form["confirmTime"].FirstOrDefault();
                if (!string.IsNullOrEmpty(confirmTime))
                {
                    var dt = confirmTime.ObjectToDate();
                    where = where.And(p => p.confirmTime > dt);
                }

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
                    await _coreCmsBillDeliveryServices.QueryListByClauseAsync(where, p => p.deliveryId,
                        OrderByType.Asc);
                //给sheet1添加第一行的头部标题
                var row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("发货单序列");
                row1.CreateCell(1).SetCellValue("物流公司编码");
                row1.CreateCell(2).SetCellValue("物流单号");
                row1.CreateCell(3).SetCellValue("快递物流信息");
                row1.CreateCell(4).SetCellValue("快递是否不更新");
                row1.CreateCell(5).SetCellValue("收货地区ID");
                row1.CreateCell(6).SetCellValue("收货详细地址");
                row1.CreateCell(7).SetCellValue("收货人姓名");
                row1.CreateCell(8).SetCellValue("收货电话");
                row1.CreateCell(9).SetCellValue("状态");
                row1.CreateCell(10).SetCellValue("备注");
                row1.CreateCell(11).SetCellValue("确认收货时间");
                row1.CreateCell(12).SetCellValue("创建时间");
                row1.CreateCell(13).SetCellValue("更新时间");

                //将数据逐步写入sheet1各个行
                for (var i = 0; i < listmodel.Count; i++)
                {
                    var rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(listmodel[i].deliveryId);
                    rowtemp.CreateCell(1).SetCellValue(listmodel[i].logiCode);
                    rowtemp.CreateCell(2).SetCellValue(listmodel[i].logiNo);
                    rowtemp.CreateCell(3).SetCellValue(listmodel[i].logiInformation);
                    rowtemp.CreateCell(4).SetCellValue(listmodel[i].logiStatus.ToString());
                    rowtemp.CreateCell(5).SetCellValue(listmodel[i].shipAreaId.ToString());
                    rowtemp.CreateCell(6).SetCellValue(listmodel[i].shipAddress);
                    rowtemp.CreateCell(7).SetCellValue(listmodel[i].shipName);
                    rowtemp.CreateCell(8).SetCellValue(listmodel[i].shipMobile);
                    rowtemp.CreateCell(9).SetCellValue(listmodel[i].status.ToString());
                    rowtemp.CreateCell(10).SetCellValue(listmodel[i].memo);
                    rowtemp.CreateCell(11).SetCellValue(listmodel[i].confirmTime.ToString());
                    rowtemp.CreateCell(12).SetCellValue(listmodel[i].createTime.ToString());
                    rowtemp.CreateCell(13).SetCellValue(listmodel[i].updateTime.ToString());
                }

                // 写入到excel
                var webRootPath = _webHostEnvironment.WebRootPath;
                var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsBillDelivery导出(查询结果).xls";
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