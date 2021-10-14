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
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 页面接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private IMapper _mapper;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsPagesServices _pagesServices;
        private readonly ICoreCmsOrderServices _orderServices;
        private readonly ICoreCmsUserServices _userServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PageController(IMapper mapper
            , ICoreCmsSettingServices settingServices
            , ICoreCmsPagesServices pagesServices
            , ICoreCmsOrderServices orderServices
            , ICoreCmsUserServices userServices)
        {
            _mapper = mapper;
            _settingServices = settingServices;
            _pagesServices = pagesServices;
            _orderServices = orderServices;
            _userServices = userServices;
        }

        //公共接口====================================================================================================

        #region 获取页面布局数据=============================================================

        /// <summary>
        /// 获取页面布局数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("获取页面布局数据")]
        public async Task<WebApiCallBack> GetPageConfig([FromBody] FMWxPost entity)
        {
            var jm = await _pagesServices.GetPageConfig(entity.code);
            return jm;
        }
        #endregion

        #region 获取用户购买记录=============================================================

        /// <summary>
        /// 获取用户购买记录
        /// </summary>
        [HttpPost]
        [Description("获取用户购买记录")]
        public async Task<WebApiCallBack> GetRecod([FromBody] FMGetRecodPost entity)
        {
            var jm = new WebApiCallBack() { status = true, msg = "获取成功", otherData = entity };

            /***
             * 随机数
             * 其它随机数据，需要自己补充
             */
            //logo作为头像
            Random rand = new Random();

            var allConfigs = await _settingServices.GetConfigDictionaries();

            var avatar = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopLogo);
            var names = new string[] { "无人像你", "啭裑①羣豞", "朕射妳无罪", "骑着蜗牛狂奔", "残孤星", "上网可以，别开QVOD", "请把QQ留下！", "蹭网可以,一小时两块钱", "I～在。哭泣", "不倾国倾城只倾他一人", "你再发光我就拔你插头", "家，世间最温暖的地方", "挥着鸡翅膀的女孩", "难不难过都是一个人过", "原谅我盛装出席只为错过你", "残孤星", "只适合被遗忘", "爱情，算个屁丶", "执子辶掱", "朕今晚翻你牌子", "①苆兜媞命", "中华一样的高傲", "始于心动止于枯骨", "我们幸福呢", "表白失败，勿扰", "髮型吥能亂", "陽咣丅啲憂喐", "你棺材是翻盖的还是滑盖的", "孤枕", "泪颜葬相思", "喵星人", "超拽霸气的微博名字", "晚安晚安晚晚难安", "却输给了秒", "为什么我吃德芙没有黑丝飘", "请输入我大" };
            var listUsers = new List<RandUser>();

            foreach (var itemName in names)
            {
                var min = rand.Next(100, 1000);
                var createTime = DateTime.Now.AddMinutes(-min);
                listUsers.Add(new RandUser()
                {
                    avatar = avatar,
                    createTime = CommonHelper.TimeAgo(createTime),
                    nickname = itemName,
                    desc = "下单成功",
                    dt = createTime
                });
            }

            if (entity.type == "home")
            {
                //数据库里面随机取出来几条数据
                var orders = await _orderServices.QueryListByClauseAsync(p => p.isdel == false, 20, p => p.createTime,
                    OrderByType.Desc);
                if (orders != null && orders.Any())
                {
                    Random rd = new Random();
                    var index = rd.Next(orders.Count);
                    var orderItem = orders[index];
                    if (orderItem != null)
                    {
                        var user = await _userServices.QueryByIdAsync(orderItem.userId);
                        if (user != null && !string.IsNullOrEmpty(user.nickName))
                        {
                            jm.data = new RandUser()
                            {
                                avatar = !string.IsNullOrEmpty(user.avatarImage) ? user.avatarImage : avatar,
                                createTime = CommonHelper.TimeAgo(orderItem.createTime),
                                nickname = user.nickName,
                                desc = "下单成功",
                                dt = orderItem.createTime
                            };
                        }
                    }
                }
                else
                {
                    Random rd = new Random();
                    var listI = rd.Next(listUsers.Count);
                    jm.data = listUsers[listI];
                }
            }
            return jm;
        }
        #endregion


        //验证接口====================================================================================================

    }



}