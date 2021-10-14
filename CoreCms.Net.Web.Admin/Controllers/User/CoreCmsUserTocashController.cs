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
    ///     用户提现记录表
    /// </summary>
    [Description("用户提现记录表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsUserTocashController : ControllerBase
    {
        private readonly ICoreCmsUserTocashServices _coreCmsUserTocashServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsUserTocashServices"></param>
        public CoreCmsUserTocashController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsUserTocashServices coreCmsUserTocashServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsUserTocashServices = coreCmsUserTocashServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsUserTocash/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsUserTocash>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsUserTocash, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "money":
                    orderEx = p => p.money;
                    break;
                case "bankName":
                    orderEx = p => p.bankName;
                    break;
                case "bankCode":
                    orderEx = p => p.bankCode;
                    break;
                case "bankAreaId":
                    orderEx = p => p.bankAreaId;
                    break;
                case "accountBank":
                    orderEx = p => p.accountBank;
                    break;
                case "accountName":
                    orderEx = p => p.accountName;
                    break;
                case "cardNumber":
                    orderEx = p => p.cardNumber;
                    break;
                case "withdrawals":
                    orderEx = p => p.withdrawals;
                    break;
                case "status":
                    orderEx = p => p.status;
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

            //ID号 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //用户ID int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);

            //银行名称 nvarchar
            var bankName = Request.Form["bankName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(bankName)) @where = @where.And(p => p.bankName.Contains(bankName));
            //银行缩写 nvarchar
            var bankCode = Request.Form["bankCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(bankCode)) @where = @where.And(p => p.bankCode.Contains(bankCode));
            //账号地区ID int
            var bankAreaId = Request.Form["bankAreaId"].FirstOrDefault().ObjectToInt(0);
            if (bankAreaId > 0) @where = @where.And(p => p.bankAreaId == bankAreaId);
            //开户行 nvarchar
            var accountBank = Request.Form["accountBank"].FirstOrDefault();
            if (!string.IsNullOrEmpty(accountBank)) @where = @where.And(p => p.accountBank.Contains(accountBank));
            //账户名 nvarchar
            var accountName = Request.Form["accountName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(accountName)) @where = @where.And(p => p.accountName.Contains(accountName));
            //卡号 nvarchar
            var cardNumber = Request.Form["cardNumber"].FirstOrDefault();
            if (!string.IsNullOrEmpty(cardNumber)) @where = @where.And(p => p.cardNumber.Contains(cardNumber));
            //提现状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) @where = @where.And(p => p.status == status);
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
            var list = await _coreCmsUserTocashServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsUserTocash/GetIndex
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

            var userTocashTypes = EnumHelper.EnumToList<GlobalEnumVars.UserTocashTypes>();

            jm.data = new
            {
                userTocashTypes
            };
            return jm;
        }

        #endregion


        #region 设置状态============================================================

        // POST: Api/CoreCmsUser/DoSetisDelete/10
        /// <summary>
        ///     设置状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置状态")]
        public async Task<AdminUiCallBack> SetStatus([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsUserTocashServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var res = await _coreCmsUserTocashServices.Examine(entity.id, entity.data.ObjectToInt(0));
            jm.code = res.status ? 0 : 1;
            jm.data = res.data;
            jm.msg = res.status ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion


        #region 选择导出============================================================

        // POST: Api/CoreCmsUserTocash/SelectExportExcel/10
        /// <summary>
        ///     选择导出
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
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel = await _coreCmsUserTocashServices.QueryListByClauseAsync(p => entity.id.Contains(p.id),
                p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("ID号");
            row1.CreateCell(1).SetCellValue("用户ID");
            row1.CreateCell(2).SetCellValue("提现金额");
            row1.CreateCell(3).SetCellValue("银行名称");
            row1.CreateCell(4).SetCellValue("银行缩写");
            row1.CreateCell(5).SetCellValue("账号地区ID");
            row1.CreateCell(6).SetCellValue("开户行");
            row1.CreateCell(7).SetCellValue("账户名");
            row1.CreateCell(8).SetCellValue("卡号");
            row1.CreateCell(9).SetCellValue("提现服务费");
            row1.CreateCell(10).SetCellValue("提现状态");
            row1.CreateCell(11).SetCellValue("创建时间");
            row1.CreateCell(12).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].money.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].bankName);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].bankCode);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].bankAreaId.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].accountBank);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].accountName);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].cardNumber);
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].withdrawals.ToString());
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(12).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsUserTocash导出(选择结果).xls";
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

        // POST: Api/CoreCmsUserTocash/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsUserTocash>();
            //查询筛选

            //ID号 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //用户ID int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //银行名称 nvarchar
            var bankName = Request.Form["bankName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(bankName)) @where = @where.And(p => p.bankName.Contains(bankName));
            //银行缩写 nvarchar
            var bankCode = Request.Form["bankCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(bankCode)) @where = @where.And(p => p.bankCode.Contains(bankCode));
            //账号地区ID int
            var bankAreaId = Request.Form["bankAreaId"].FirstOrDefault().ObjectToInt(0);
            if (bankAreaId > 0) @where = @where.And(p => p.bankAreaId == bankAreaId);
            //开户行 nvarchar
            var accountBank = Request.Form["accountBank"].FirstOrDefault();
            if (!string.IsNullOrEmpty(accountBank)) @where = @where.And(p => p.accountBank.Contains(accountBank));
            //账户名 nvarchar
            var accountName = Request.Form["accountName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(accountName)) @where = @where.And(p => p.accountName.Contains(accountName));
            //卡号 nvarchar
            var cardNumber = Request.Form["cardNumber"].FirstOrDefault();
            if (!string.IsNullOrEmpty(cardNumber)) @where = @where.And(p => p.cardNumber.Contains(cardNumber));
            //提现状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) @where = @where.And(p => p.status == status);
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
                await _coreCmsUserTocashServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("ID号");
            row1.CreateCell(1).SetCellValue("用户ID");
            row1.CreateCell(2).SetCellValue("提现金额");
            row1.CreateCell(3).SetCellValue("银行名称");
            row1.CreateCell(4).SetCellValue("银行缩写");
            row1.CreateCell(5).SetCellValue("账号地区ID");
            row1.CreateCell(6).SetCellValue("开户行");
            row1.CreateCell(7).SetCellValue("账户名");
            row1.CreateCell(8).SetCellValue("卡号");
            row1.CreateCell(9).SetCellValue("提现服务费");
            row1.CreateCell(10).SetCellValue("提现状态");
            row1.CreateCell(11).SetCellValue("创建时间");
            row1.CreateCell(12).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].money.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].bankName);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].bankCode);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].bankAreaId.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].accountBank);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].accountName);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].cardNumber);
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].withdrawals.ToString());
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(12).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsUserTocash导出(查询结果).xls";
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