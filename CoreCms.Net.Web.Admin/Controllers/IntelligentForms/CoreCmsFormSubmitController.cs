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
    /// 用户对智能表单的提交记录
    ///</summary>
    [Description("用户对智能表单的提交记录")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsFormSubmitController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsFormSubmitServices _coreCmsFormSubmitServices;
        private readonly ICoreCmsFormSubmitDetailServices _formSubmitDetailServices;
        private readonly ICoreCmsFormServices _formServices;
        private readonly ICoreCmsUserServices _userServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsFormSubmitController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsFormSubmitServices coreCmsFormSubmitServices, ICoreCmsFormServices formServices, ICoreCmsFormSubmitDetailServices formSubmitDetailServices, ICoreCmsUserServices userServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsFormSubmitServices = coreCmsFormSubmitServices;
            _formServices = formServices;
            _formSubmitDetailServices = formSubmitDetailServices;
            _userServices = userServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsFormSubmit/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsFormSubmit>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<CoreCmsFormSubmit, object>> orderEx = orderField switch
            {
                "id" => p => p.id,
                "formId" => p => p.formId,
                "formName" => p => p.formName,
                "userId" => p => p.userId,
                "money" => p => p.money,
                "payStatus" => p => p.payStatus,
                "status" => p => p.status,
                "feedback" => p => p.feedback,
                "ip" => p => p.ip,
                "createTime" => p => p.createTime,
                "updateTime" => p => p.updateTime,
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

            //表单id int
            var formId = Request.Form["formId"].FirstOrDefault().ObjectToInt(0);
            if (formId > 0)
            {
                where = where.And(p => p.formId == formId);
            }

            //用户昵称 nvarchar
            var userName = Request.Form["userName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(userName))
            {
                where = where.And(p => p.userName.Contains(userName));
            }

            //是否支付 bit
            var payStatus = Request.Form["payStatus"].FirstOrDefault();
            if (!string.IsNullOrEmpty(payStatus) && payStatus.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.payStatus == true);
            }
            else if (!string.IsNullOrEmpty(payStatus) && payStatus.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.payStatus == false);
            }
            //是否处理 bit
            var status = Request.Form["status"].FirstOrDefault();
            if (!string.IsNullOrEmpty(status) && status.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.status == true);
            }
            else if (!string.IsNullOrEmpty(status) && status.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.status == false);
            }
            //表单反馈 nvarchar
            var feedback = Request.Form["feedback"].FirstOrDefault();
            if (!string.IsNullOrEmpty(feedback))
            {
                where = where.And(p => p.feedback.Contains(feedback));
            }
            //提交人ip nvarchar
            var ip = Request.Form["ip"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ip))
            {
                where = where.And(p => p.ip.Contains(ip));
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
            var list = await _coreCmsFormSubmitServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsFormSubmit/GetIndex
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

            var forms = await _formServices.QueryAsync();
            jm.data = new
            {
                forms
            };

            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsFormSubmit/GetEdit
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

            var model = await _coreCmsFormSubmitServices.QueryByIdAsync(entity.id, false);
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

        #region 删除数据============================================================
        // POST: Api/CoreCmsFormSubmit/DoDelete/10
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

            var model = await _coreCmsFormSubmitServices.ExistsAsync(p => p.id == entity.id, true);
            if (!model)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            jm = await _coreCmsFormSubmitServices.DeleteByIdAsync(entity.id);

            return jm;
        }
        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsFormSubmit/GetDetails/10
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

            var model = await _coreCmsFormSubmitServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;

            var getResult = await _formServices.GetFormInfo(model.formId, "", true);
            var formModel = getResult.data as CoreCmsForm;
            if (formModel == null)
            {
                jm.msg = "表单信息获取失败";
                jm.data = getResult;
                return jm;
            }

            if (formModel.Items.Any())
            {
                var ids = formModel.Items.Select(p => p.id).ToArray();
                var submitDetail = await _formSubmitDetailServices.QueryListByClauseAsync(p => ids.Contains(p.formItemId) && p.submitId == model.id);

                foreach (var item in formModel.Items)
                {
                    var sub = submitDetail.Find(p => p.formItemId == item.id);
                    if (sub != null)
                    {
                        if (item.type == GlobalEnumVars.FormFieldTypes.goods.ToString())
                        {
                            item.value = sub.formItemName + " x 数量" + sub.formItemValue;
                        }
                        else
                        {
                            item.value = sub.formItemValue;
                        }
                    }
                    else
                    {
                        item.value = "";
                    }

                }
            }

            if (model.userId > 0)
            {
                var user = await _userServices.QueryByClauseAsync(p => p.id == model.userId);
                model.userName = user != null ? user.nickName : "";
            }

            jm.data = new
            {
                model,
                formModel.Items
            };

            return jm;
        }
        #endregion

        #region 选择导出============================================================
        // POST: Api/CoreCmsFormSubmit/SelectExportExcel/10
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
            var listModel = await _coreCmsFormSubmitServices.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("序列");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("表单id");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("表单名称");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("会员id");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("总金额");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("是否支付");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("是否处理");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("表单反馈");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);

            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("提交人ip");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);

            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("创建时间");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);

            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("更新时间");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);

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
                rowTemp1.SetCellValue(listModel[i].formId.ToString());
                rowTemp1.CellStyle = commonCellStyle;

                var rowTemp2 = rowTemp.CreateCell(2);
                rowTemp2.SetCellValue(listModel[i].formName.ToString());
                rowTemp2.CellStyle = commonCellStyle;

                var rowTemp3 = rowTemp.CreateCell(3);
                rowTemp3.SetCellValue(listModel[i].userId.ToString());
                rowTemp3.CellStyle = commonCellStyle;

                var rowTemp4 = rowTemp.CreateCell(4);
                rowTemp4.SetCellValue(listModel[i].money.ToString());
                rowTemp4.CellStyle = commonCellStyle;

                var rowTemp5 = rowTemp.CreateCell(5);
                rowTemp5.SetCellValue(listModel[i].payStatus.ToString());
                rowTemp5.CellStyle = commonCellStyle;

                var rowTemp6 = rowTemp.CreateCell(6);
                rowTemp6.SetCellValue(listModel[i].status.ToString());
                rowTemp6.CellStyle = commonCellStyle;

                var rowTemp7 = rowTemp.CreateCell(7);
                rowTemp7.SetCellValue(listModel[i].feedback.ToString());
                rowTemp7.CellStyle = commonCellStyle;

                var rowTemp8 = rowTemp.CreateCell(8);
                rowTemp8.SetCellValue(listModel[i].ip.ToString());
                rowTemp8.CellStyle = commonCellStyle;

                var rowTemp9 = rowTemp.CreateCell(9);
                rowTemp9.SetCellValue(listModel[i].createTime.ToString());
                rowTemp9.CellStyle = commonCellStyle;

                var rowTemp10 = rowTemp.CreateCell(10);
                rowTemp10.SetCellValue(listModel[i].updateTime.ToString());
                rowTemp10.CellStyle = commonCellStyle;

            }
            // 导出excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsFormSubmit导出(选择结果).xls";
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
        // POST: Api/CoreCmsFormSubmit/QueryExportExcel/10
        /// <summary>
        /// 查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsFormSubmit>();
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //表单id int
            var formId = Request.Form["formId"].FirstOrDefault().ObjectToInt(0);
            if (formId > 0)
            {
                where = where.And(p => p.formId == formId);
            }
            //表单名称 nvarchar
            var formName = Request.Form["formName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(formName))
            {
                where = where.And(p => p.formName.Contains(formName));
            }
            //会员id int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            //是否支付 bit
            var payStatus = Request.Form["payStatus"].FirstOrDefault();
            if (!string.IsNullOrEmpty(payStatus) && payStatus.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.payStatus == true);
            }
            else if (!string.IsNullOrEmpty(payStatus) && payStatus.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.payStatus == false);
            }
            //是否处理 bit
            var status = Request.Form["status"].FirstOrDefault();
            if (!string.IsNullOrEmpty(status) && status.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.status == true);
            }
            else if (!string.IsNullOrEmpty(status) && status.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.status == false);
            }
            //表单反馈 nvarchar
            var feedback = Request.Form["feedback"].FirstOrDefault();
            if (!string.IsNullOrEmpty(feedback))
            {
                where = where.And(p => p.feedback.Contains(feedback));
            }
            //提交人ip nvarchar
            var ip = Request.Form["ip"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ip))
            {
                where = where.And(p => p.ip.Contains(ip));
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
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _coreCmsFormSubmitServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("序列");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("表单id");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("表单名称");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("会员id");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("总金额");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("是否支付");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("是否处理");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("表单反馈");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);

            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("提交人ip");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);

            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("创建时间");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);

            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("更新时间");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);


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
                rowTemp1.SetCellValue(listModel[i].formId.ToString());
                rowTemp1.CellStyle = commonCellStyle;



                var rowTemp2 = rowTemp.CreateCell(2);
                rowTemp2.SetCellValue(listModel[i].formName.ToString());
                rowTemp2.CellStyle = commonCellStyle;



                var rowTemp3 = rowTemp.CreateCell(3);
                rowTemp3.SetCellValue(listModel[i].userId.ToString());
                rowTemp3.CellStyle = commonCellStyle;



                var rowTemp4 = rowTemp.CreateCell(4);
                rowTemp4.SetCellValue(listModel[i].money.ToString());
                rowTemp4.CellStyle = commonCellStyle;



                var rowTemp5 = rowTemp.CreateCell(5);
                rowTemp5.SetCellValue(listModel[i].payStatus.ToString());
                rowTemp5.CellStyle = commonCellStyle;



                var rowTemp6 = rowTemp.CreateCell(6);
                rowTemp6.SetCellValue(listModel[i].status.ToString());
                rowTemp6.CellStyle = commonCellStyle;



                var rowTemp7 = rowTemp.CreateCell(7);
                rowTemp7.SetCellValue(listModel[i].feedback.ToString());
                rowTemp7.CellStyle = commonCellStyle;



                var rowTemp8 = rowTemp.CreateCell(8);
                rowTemp8.SetCellValue(listModel[i].ip.ToString());
                rowTemp8.CellStyle = commonCellStyle;



                var rowTemp9 = rowTemp.CreateCell(9);
                rowTemp9.SetCellValue(listModel[i].createTime.ToString());
                rowTemp9.CellStyle = commonCellStyle;



                var rowTemp10 = rowTemp.CreateCell(10);
                rowTemp10.SetCellValue(listModel[i].updateTime.ToString());
                rowTemp10.CellStyle = commonCellStyle;


            }
            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsFormSubmit导出(查询结果).xls";
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

        #region 设置是否支付============================================================
        // POST: Api/CoreCmsFormSubmit/DoSetpayStatus/10
        /// <summary>
        /// 设置是否支付
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否支付")]
        public async Task<AdminUiCallBack> DoSetpayStatus([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsFormSubmitServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.payStatus = (bool)entity.data;

            //事物处理过程结束
            var bl = await _coreCmsFormSubmitServices.UpdateAsync(p => new CoreCmsFormSubmit() { payStatus = oldModel.payStatus }, p => p.id == oldModel.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;


            return jm;
        }
        #endregion

        #region 设置是否处理============================================================
        // POST: Api/CoreCmsFormSubmit/DoSetstatus/10
        /// <summary>
        /// 设置是否处理
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否处理")]
        public async Task<AdminUiCallBack> DoSetstatus([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsFormSubmitServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.status = (bool)entity.data;

            //事物处理过程结束
            var bl = await _coreCmsFormSubmitServices.UpdateAsync(p => new CoreCmsFormSubmit() { status = oldModel.status }, p => p.id == oldModel.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 更新反馈============================================================
        // POST: Api/CoreCmsFormSubmit/DoSetstatus/10
        /// <summary>
        /// 更新反馈
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("更新反馈")]
        public async Task<AdminUiCallBack> DoSetFeedback([FromBody] FMUpdateStringDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsFormSubmitServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.feedback = entity.data.ToString();

            //事物处理过程结束
            var bl = await _coreCmsFormSubmitServices.UpdateAsync(p => new CoreCmsFormSubmit() { feedback = oldModel.feedback, updateTime = DateTime.Now }, p => p.id == oldModel.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

    }
}
