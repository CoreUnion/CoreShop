﻿/***********************************************************************
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
    public class FormStatisticsDto
    {
        public string day { get; set; }
        public int nums { get; set; }
        public int formId { get; set; }
    }

    public class FormStatisticsViewDto
    {
        public string[] day { get; set; }
        public int[] data { get; set; }
        public int formId { get; set; }
    }
}