/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

namespace CoreCms.Net.Model.ViewModels.DTO
{
    /// <summary>
    ///     返回不同分销级别的详细配置数据，主要是json转回实体操作
    /// </summary>
    public class DistributionDto
    {
        /// <summary>
        ///     分销等级
        /// </summary>
        public int DistributionLevel { get; set; }

        /// <summary>
        ///     关联会员等级
        /// </summary>
        public int grade_id { get; set; }

        /// <summary>
        ///     分销级别一配置
        /// </summary>
        public commission commission_1 { get; set; }

        /// <summary>
        ///     分销级别二配置
        /// </summary>
        public commission commission_2 { get; set; }

        /// <summary>
        ///     分销级别三配置
        /// </summary>
        public commission commission_3 { get; set; }
    }

    public class commission
    {
        /// <summary>
        ///     类型(百分比/固定金额)
        /// </summary>
        public int type { get; set; }

        /// <summary>
        ///     小数
        /// </summary>
        public decimal discount { get; set; }
    }
}