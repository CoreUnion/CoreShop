/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


namespace CoreCms.Net.Model.FromBody
{
    internal class FMProducts
    {
    }


    /// <summary>
    ///     获取子规格
    /// </summary>
    public class ItemSpesDesc
    {
        public string name { get; set; } = string.Empty;
        public bool isDefault { get; set; } = false;
        public int productId { get; set; } = 0;
    }


    /// <summary>
    ///     获取相应规格
    /// </summary>
    public class DefaultSpesDesc
    {
        public string name { get; set; } = string.Empty;
        public bool isDefault { get; set; } = false;
        public int productId { get; set; } = 0;
    }
}