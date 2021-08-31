using System.IO;
using AutoMapper;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using Newtonsoft.Json;

namespace CoreCms.Net.Mapping
{
    /// <summary>
    /// AutoMapper的全局实体映射配置静态类
    /// </summary>
    public class AutoMapperConfiguration : Profile, AutoMapperIProfile
    {
        public AutoMapperConfiguration()
        {
            //CreateMap<Manager, ManagerDTO>().ReverseMap();

            CreateMap<SqlSugar.DbTableInfo, CoreCms.Net.Model.ViewModels.Basics.DbTableInfoTree>()
                .AfterMap((from, to, context) =>
                {
                    to.Label = from.Name + "[" + from.Description + "]";
                });

            //商品分类转前端json
            CreateMap<CoreCmsGoodsCategory, DTreeList>()
                .AfterMap((from, to, context) =>
                {
                    to.id = from.id.ToString();
                    to.title = from.name;
                    to.checkArr = "0";
                    to.parentId = from.parentId.ToString();
                });

            #region 小程序交互相关=======================================================================================
            //小程序首页获取页面布局信息数据转换
            CreateMap<CoreCmsPagesItems, PagesItemsDto>()
                .AfterMap((from, to, context) =>
                {
                    to.parameters = new JsonSerializer().Deserialize(new JsonTextReader(new StringReader(from.parameters)));
                });
            #endregion

        }
    }
}
