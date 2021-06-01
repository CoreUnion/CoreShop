/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.FromBody
{
    #region 通用更新实体============================================================

    /// <summary>
    ///     按照序列进行更新Bool类型数据
    /// </summary>
    public class FMUpdateBoolDataByIntId
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Required(ErrorMessage = "请输入序列")]
        public int id { get; set; }

        /// <summary>
        ///     数据
        /// </summary>
        [Required(ErrorMessage = "请输入相应数据")]
        public bool data { get; set; }
    }

    /// <summary>
    ///     按照序列进行更新String类型数据
    /// </summary>
    public class FMUpdateStringDataByIntId
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Required(ErrorMessage = "请输入序列")]
        public int id { get; set; }

        /// <summary>
        ///     数据
        /// </summary>
        [Required(ErrorMessage = "请输入相应数据")]
        public string data { get; set; }
    }

    /// <summary>
    ///     按照序列进行更新Int类型数据
    /// </summary>
    public class FMUpdateIntegerDataByIntId
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Required(ErrorMessage = "请输入序列")]
        public int id { get; set; }

        /// <summary>
        ///     数据
        /// </summary>
        [Required(ErrorMessage = "请输入相应数据")]
        public int data { get; set; }
    }


    /// <summary>
    ///     按照序列进行更新Decimal类型数据
    /// </summary>
    public class FMUpdateDecimalDataByIntId
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Required(ErrorMessage = "请输入序列")]
        public int id { get; set; }

        /// <summary>
        ///     数据
        /// </summary>
        [Required(ErrorMessage = "请输入相应数据")]
        public decimal data { get; set; }
    }


    /// <summary>
    ///     按照序列进行更新Decimal类型数据
    /// </summary>
    public class FMUpdateArrayIntDataByIntId
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Required(ErrorMessage = "请输入序列")]
        public int id { get; set; }

        /// <summary>
        ///     数据
        /// </summary>
        [Required(ErrorMessage = "请输入相应数据")]
        public List<int> data { get; set; }
    }

    /// <summary>
    ///     按照序列进行更新string数组类型数据
    /// </summary>
    public class FMUpdateArrayStringDataByIntId
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Required(ErrorMessage = "请输入序列")]
        public int id { get; set; }

        /// <summary>
        ///     数据
        /// </summary>
        [Required(ErrorMessage = "请输入相应数据")]
        public List<string> data { get; set; }
    }

    #endregion


    #region 用户相关=============================================================================

    /// <summary>
    ///     更新积分提交model
    /// </summary>
    public class FMUpdateUserPoint
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Required(ErrorMessage = "请输入序列")]
        public int id { get; set; }

        /// <summary>
        ///     积分
        /// </summary>
        [Required(ErrorMessage = "请输入积分")]
        public int point { get; set; }

        /// <summary>
        ///     说明
        /// </summary>
        [Required(ErrorMessage = "请输入说明")]
        public string memo { get; set; }
    }

    #endregion
}