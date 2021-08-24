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
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Api;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using Flurl.Http;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 物流公司表 接口实现
    /// </summary>
    public class CoreCmsLogisticsServices : BaseServices<CoreCmsLogistics>, ICoreCmsLogisticsServices
    {
        private readonly ICoreCmsLogisticsRepository _dal;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly IUnitOfWork _unitOfWork;



        public CoreCmsLogisticsServices(IUnitOfWork unitOfWork, ICoreCmsLogisticsRepository dal, ICoreCmsSettingServices settingServices)
        {
            this._dal = dal;
            _settingServices = settingServices;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }



        /// <summary>
        /// 根据物流编码取物流名称等信息
        /// </summary>
        /// <param name="logiCode">物流编码</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetLogiInfo(string logiCode)
        {
            var jm = new WebApiCallBack();

            var model = await _dal.QueryByClauseAsync(p => p.logiCode == logiCode);
            jm.status = model != null;
            jm.data = model;
            jm.msg = jm.status ? GlobalConstVars.GetDataSuccess : GlobalConstVars.GetDataFailure;

            return jm;
        }


        /// <summary>
        /// 通过接口更新所有快递公司信息
        /// </summary>
        public async Task<AdminUiCallBack> DoUpdateCompany()
        {
            var jm = new AdminUiCallBack();

            var allConfigs = await _settingServices.GetConfigDictionaries();
            var showApiAppid = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShowApiAppid);
            var showApiSecret = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShowApiSecret);
            var shopMobile = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopMobile);

            var showApiTimesTamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            var maxSize = 1500;
            var signStr = "maxSize" + maxSize + "showapi_appid" + showApiAppid + "showapi_timestamp" + showApiTimesTamp + showApiSecret;
            var md5Sign = CommonHelper.Md5For32(signStr).ToLower();

            var url = "https://route.showapi.com/64-20?expName=&maxSize=1500&page=&showapi_appid=" + showApiAppid +
                      "&showapi_timestamp=" + showApiTimesTamp + "&showapi_sign=" + md5Sign;
            var person = await url.GetJsonAsync<ShowApiGetExpressCompanyListResult>();

            if (person.showapi_res_code == 0)
            {
                if (person.showapi_res_body != null && person.showapi_res_body.ret_code == 0 && person.showapi_res_body.expressList != null && person.showapi_res_body.expressList.Count > 0)
                {
                    var list = new List<CoreCmsLogistics>();


                    var systemLogistics = SystemSettingDictionary.GetSystemLogistics();
                    systemLogistics.ForEach(p =>
                    {
                        var logistics = new CoreCmsLogistics();
                        logistics.logiCode = p.sKey;
                        logistics.logiName = p.sDescription;
                        logistics.imgUrl = "";
                        logistics.phone = shopMobile;
                        logistics.url = "";
                        logistics.sort = -1;
                        logistics.isDelete = false;

                        list.Add(logistics);
                    });


                    var count = 0;
                    person.showapi_res_body.expressList.ForEach(p =>
                    {
                        var logistics = new CoreCmsLogistics();
                        logistics.logiCode = p.simpleName;
                        logistics.logiName = p.expName;
                        logistics.imgUrl = p.imgUrl;
                        logistics.phone = p.phone;
                        logistics.url = p.url;
                        logistics.sort = count * 5;
                        logistics.isDelete = false;

                        list.Add(logistics);
                        count++;
                    });
                    await _dal.DeleteAsync(p => p.id > 0);
                    var bl = await _dal.InsertAsync(list) > 0;
                    jm.code = bl ? 0 : 1;
                    jm.msg = bl ? "数据刷新成功" : "数据刷新失败";
                }
                else
                {
                    jm.msg = "接口获取数据失败";
                }
            }
            else
            {
                jm.msg = person.showapi_res_error;
            }

            return jm;
        }


        /// <summary>
        /// 通过接口获取快递信息
        /// </summary>
        /// <param name="com">来源</param>
        /// <param name="number">编号</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ExpressPoll(string com, string number, string phone)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(com))
            {
                jm.msg = "请提交来源";
                return jm;
            }
            else if (string.IsNullOrEmpty(number))
            {
                jm.msg = "请提交编号";
                return jm;
            }
            else if (string.IsNullOrEmpty(phone))
            {
                jm.msg = "请提交手机号码";
                return jm;
            }


            var allConfigs = await _settingServices.GetConfigDictionaries();
            var showApiAppid = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShowApiAppid);
            var showApiSecret = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShowApiSecret);

            var showApiTimesTamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            var signStr = "com" + com + "nu" + number + "phone" + phone + "showapi_appid" + showApiAppid + "showapi_timestamp" + showApiTimesTamp + showApiSecret;
            var md5Sign = CommonHelper.Md5For32(signStr).ToLower();

            var url = "https://route.showapi.com/64-19?com=" + com + "&nu=" + number + "&phone=" + phone + "&showapi_appid=" + showApiAppid +
                      "&showapi_timestamp=" + showApiTimesTamp + "&showapi_sign=" + md5Sign;
            var result = await url.GetJsonAsync<ShowApiGetExpressPollResult>();

            if (result.showapi_res_code != 0)
            {
                jm.status = false;
                jm.msg = result.showapi_res_error;
            }
            else
            {
                switch (result.showapi_res_body.ret_code)
                {
                    case 0:
                        jm.status = true;
                        jm.msg = "查询成功";
                        jm.data = result.showapi_res_body;
                        break;
                    case 1:
                        jm.status = false;
                        jm.msg = "输入参数错误";
                        jm.data = result.showapi_res_body;
                        break;
                    case 2:
                        jm.status = false;
                        jm.msg = "查不到物流信息";
                        jm.data = result.showapi_res_body;
                        break;
                    case 3:
                        jm.status = false;
                        jm.msg = "单号不符合规则";
                        jm.data = result.showapi_res_body;
                        break;
                    case 4:
                        jm.status = false;
                        jm.msg = "快递公司编码不符合规则";
                        jm.data = result.showapi_res_body;
                        break;
                    case 5:
                        jm.status = false;
                        jm.msg = "快递查询渠道异常";
                        jm.data = result.showapi_res_body;
                        break;
                    case 6:
                        jm.status = false;
                        jm.msg = " auto时未查到单号对应的快递公司,请指定快递公司编码";
                        jm.data = result.showapi_res_body;
                        break;
                    case 7:
                        jm.status = false;
                        jm.msg = "单号与手机号不匹配";
                        jm.data = result.showapi_res_body;
                        break;
                    default:
                        jm.status = false;
                        jm.msg = "接口调用失败";
                        jm.data = result.showapi_res_body;
                        break;
                }
            }
            return jm;
        }
    }
}
