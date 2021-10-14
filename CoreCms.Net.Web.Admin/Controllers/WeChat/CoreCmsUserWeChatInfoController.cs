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
    ///     用户表
    /// </summary>
    [Description("用户表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsUserWeChatInfoController : ControllerBase
    {
        private readonly ICoreCmsUserWeChatInfoServices _coreCmsUserWeChatInfoServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsUserWeChatInfoServices"></param>
        public CoreCmsUserWeChatInfoController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsUserWeChatInfoServices coreCmsUserWeChatInfoServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsUserWeChatInfoServices = coreCmsUserWeChatInfoServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsUserWeChatInfo/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsUserWeChatInfo>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsUserWeChatInfo, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "type":
                    orderEx = p => p.type;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "openid":
                    orderEx = p => p.openid;
                    break;
                case "sessionKey":
                    orderEx = p => p.sessionKey;
                    break;
                case "unionId":
                    orderEx = p => p.unionId;
                    break;
                case "avatar":
                    orderEx = p => p.avatar;
                    break;
                case "nickName":
                    orderEx = p => p.nickName;
                    break;
                case "gender":
                    orderEx = p => p.gender;
                    break;
                case "language":
                    orderEx = p => p.language;
                    break;
                case "city":
                    orderEx = p => p.city;
                    break;
                case "province":
                    orderEx = p => p.province;
                    break;
                case "country":
                    orderEx = p => p.country;
                    break;
                case "countryCode":
                    orderEx = p => p.countryCode;
                    break;
                case "mobile":
                    orderEx = p => p.mobile;
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

            //用户ID int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //第三方登录类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) @where = @where.And(p => p.type == type);
            //关联用户表 int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //openId nvarchar
            var openid = Request.Form["openid"].FirstOrDefault();
            if (!string.IsNullOrEmpty(openid)) @where = @where.And(p => p.openid.Contains(openid));
            //缓存key nvarchar
            var sessionKey = Request.Form["sessionKey"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sessionKey)) @where = @where.And(p => p.sessionKey.Contains(sessionKey));
            //unionid nvarchar
            var unionId = Request.Form["unionId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(unionId)) @where = @where.And(p => p.unionId.Contains(unionId));
            //头像 nvarchar
            var avatar = Request.Form["avatar"].FirstOrDefault();
            if (!string.IsNullOrEmpty(avatar)) @where = @where.And(p => p.avatar.Contains(avatar));
            //昵称 nvarchar
            var nickName = Request.Form["nickName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(nickName)) @where = @where.And(p => p.nickName.Contains(nickName));
            //性别 int
            var gender = Request.Form["gender"].FirstOrDefault().ObjectToInt(0);
            if (gender > 0) @where = @where.And(p => p.gender == gender);
            //语言 nvarchar
            var language = Request.Form["language"].FirstOrDefault();
            if (!string.IsNullOrEmpty(language)) @where = @where.And(p => p.language.Contains(language));
            //城市 nvarchar
            var city = Request.Form["city"].FirstOrDefault();
            if (!string.IsNullOrEmpty(city)) @where = @where.And(p => p.city.Contains(city));
            //省 nvarchar
            var province = Request.Form["province"].FirstOrDefault();
            if (!string.IsNullOrEmpty(province)) @where = @where.And(p => p.province.Contains(province));
            //国家 nvarchar
            var country = Request.Form["country"].FirstOrDefault();
            if (!string.IsNullOrEmpty(country)) @where = @where.And(p => p.country.Contains(country));
            //手机号码国家编码 nvarchar
            var countryCode = Request.Form["countryCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(countryCode)) @where = @where.And(p => p.countryCode.Contains(countryCode));
            //手机号码 nvarchar
            var mobile = Request.Form["mobile"].FirstOrDefault();
            if (!string.IsNullOrEmpty(mobile)) @where = @where.And(p => p.mobile.Contains(mobile));
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
            var list = await _coreCmsUserWeChatInfoServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent,
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

        // POST: Api/CoreCmsUserWeChatInfo/GetIndex
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

        #region 创建数据============================================================

        // POST: Api/CoreCmsUserWeChatInfo/GetCreate
        /// <summary>
        ///     创建数据
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

        // POST: Api/CoreCmsUserWeChatInfo/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsUserWeChatInfo entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _coreCmsUserWeChatInfoServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsUserWeChatInfo/GetEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsUserWeChatInfoServices.QueryByIdAsync(entity.id);
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

        // POST: Api/CoreCmsUserWeChatInfo/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsUserWeChatInfo entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsUserWeChatInfoServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.type = entity.type;
            oldModel.userId = entity.userId;
            oldModel.openid = entity.openid;
            oldModel.sessionKey = entity.sessionKey;
            oldModel.unionId = entity.unionId;
            oldModel.avatar = entity.avatar;
            oldModel.nickName = entity.nickName;
            oldModel.gender = entity.gender;
            oldModel.language = entity.language;
            oldModel.city = entity.city;
            oldModel.province = entity.province;
            oldModel.country = entity.country;
            oldModel.countryCode = entity.countryCode;
            oldModel.mobile = entity.mobile;
            oldModel.createTime = entity.createTime;
            oldModel.updateTime = entity.updateTime;

            //事物处理过程结束
            var bl = await _coreCmsUserWeChatInfoServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsUserWeChatInfo/DoDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsUserWeChatInfoServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var bl = await _coreCmsUserWeChatInfoServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }

        #endregion

        #region 批量删除============================================================

        // POST: Api/CoreCmsUserWeChatInfo/DoBatchDelete/10,11,20
        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<AdminUiCallBack> DoBatchDelete([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _coreCmsUserWeChatInfoServices.DeleteByIdsAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/CoreCmsUserWeChatInfo/GetDetails/10
        /// <summary>
        ///     预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsUserWeChatInfoServices.QueryByIdAsync(entity.id);
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

        // POST: Api/CoreCmsUserWeChatInfo/SelectExportExcel/10
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
            var listmodel =
                await _coreCmsUserWeChatInfoServices.QueryListByClauseAsync(p => entity.id.Contains(p.id),
                    p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("用户ID");
            row1.CreateCell(1).SetCellValue("第三方登录类型");
            row1.CreateCell(2).SetCellValue("关联用户表");
            row1.CreateCell(3).SetCellValue("openId");
            row1.CreateCell(4).SetCellValue("缓存key");
            row1.CreateCell(5).SetCellValue("unionid");
            row1.CreateCell(6).SetCellValue("头像");
            row1.CreateCell(7).SetCellValue("昵称");
            row1.CreateCell(8).SetCellValue("性别");
            row1.CreateCell(9).SetCellValue("语言");
            row1.CreateCell(10).SetCellValue("城市");
            row1.CreateCell(11).SetCellValue("省");
            row1.CreateCell(12).SetCellValue("国家");
            row1.CreateCell(13).SetCellValue("手机号码国家编码");
            row1.CreateCell(14).SetCellValue("手机号码");
            row1.CreateCell(15).SetCellValue("创建时间");
            row1.CreateCell(16).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].openid);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].sessionKey);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].unionId);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].avatar);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].nickName);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].gender.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].language);
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].city);
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].province);
                rowtemp.CreateCell(12).SetCellValue(listmodel[i].country);
                rowtemp.CreateCell(13).SetCellValue(listmodel[i].countryCode);
                rowtemp.CreateCell(14).SetCellValue(listmodel[i].mobile);
                rowtemp.CreateCell(15).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(16).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 导出excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsUserWeChatInfo导出(选择结果).xls";
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

        // POST: Api/CoreCmsUserWeChatInfo/QueryExportExcel/10
        /// <summary>
        ///     查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsUserWeChatInfo>();
            //查询筛选

            //用户ID int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //第三方登录类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) @where = @where.And(p => p.type == type);
            //关联用户表 int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //openId nvarchar
            var openid = Request.Form["openid"].FirstOrDefault();
            if (!string.IsNullOrEmpty(openid)) @where = @where.And(p => p.openid.Contains(openid));
            //缓存key nvarchar
            var sessionKey = Request.Form["sessionKey"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sessionKey)) @where = @where.And(p => p.sessionKey.Contains(sessionKey));
            //unionid nvarchar
            var unionId = Request.Form["unionId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(unionId)) @where = @where.And(p => p.unionId.Contains(unionId));
            //头像 nvarchar
            var avatar = Request.Form["avatar"].FirstOrDefault();
            if (!string.IsNullOrEmpty(avatar)) @where = @where.And(p => p.avatar.Contains(avatar));
            //昵称 nvarchar
            var nickName = Request.Form["nickName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(nickName)) @where = @where.And(p => p.nickName.Contains(nickName));
            //性别 int
            var gender = Request.Form["gender"].FirstOrDefault().ObjectToInt(0);
            if (gender > 0) @where = @where.And(p => p.gender == gender);
            //语言 nvarchar
            var language = Request.Form["language"].FirstOrDefault();
            if (!string.IsNullOrEmpty(language)) @where = @where.And(p => p.language.Contains(language));
            //城市 nvarchar
            var city = Request.Form["city"].FirstOrDefault();
            if (!string.IsNullOrEmpty(city)) @where = @where.And(p => p.city.Contains(city));
            //省 nvarchar
            var province = Request.Form["province"].FirstOrDefault();
            if (!string.IsNullOrEmpty(province)) @where = @where.And(p => p.province.Contains(province));
            //国家 nvarchar
            var country = Request.Form["country"].FirstOrDefault();
            if (!string.IsNullOrEmpty(country)) @where = @where.And(p => p.country.Contains(country));
            //手机号码国家编码 nvarchar
            var countryCode = Request.Form["countryCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(countryCode)) @where = @where.And(p => p.countryCode.Contains(countryCode));
            //手机号码 nvarchar
            var mobile = Request.Form["mobile"].FirstOrDefault();
            if (!string.IsNullOrEmpty(mobile)) @where = @where.And(p => p.mobile.Contains(mobile));
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
                await _coreCmsUserWeChatInfoServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("用户ID");
            row1.CreateCell(1).SetCellValue("第三方登录类型");
            row1.CreateCell(2).SetCellValue("关联用户表");
            row1.CreateCell(3).SetCellValue("openId");
            row1.CreateCell(4).SetCellValue("缓存key");
            row1.CreateCell(5).SetCellValue("unionid");
            row1.CreateCell(6).SetCellValue("头像");
            row1.CreateCell(7).SetCellValue("昵称");
            row1.CreateCell(8).SetCellValue("性别");
            row1.CreateCell(9).SetCellValue("语言");
            row1.CreateCell(10).SetCellValue("城市");
            row1.CreateCell(11).SetCellValue("省");
            row1.CreateCell(12).SetCellValue("国家");
            row1.CreateCell(13).SetCellValue("手机号码国家编码");
            row1.CreateCell(14).SetCellValue("手机号码");
            row1.CreateCell(15).SetCellValue("创建时间");
            row1.CreateCell(16).SetCellValue("更新时间");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].id.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].type.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].openid);
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].sessionKey);
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].unionId);
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].avatar);
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].nickName);
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].gender.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].language);
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].city);
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].province);
                rowtemp.CreateCell(12).SetCellValue(listmodel[i].country);
                rowtemp.CreateCell(13).SetCellValue(listmodel[i].countryCode);
                rowtemp.CreateCell(14).SetCellValue(listmodel[i].mobile);
                rowtemp.CreateCell(15).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(16).SetCellValue(listmodel[i].updateTime.ToString());
            }

            // 写入到excel
            var webRootPath = _webHostEnvironment.WebRootPath;
            var tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsUserWeChatInfo导出(查询结果).xls";
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