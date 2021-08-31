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
    ///     ck编辑器上传图片回调数据
    /// </summary>
    public class CKEditorUploadedResult
    {
        /// <summary>
        ///     1成功0失败
        /// </summary>
        public int uploaded { get; set; } = 0;

        /// <summary>
        ///     文件名称
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        ///     查看地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        ///     错误说明
        /// </summary>

        public CKEditorUploadedError error { get; set; } = new();

        public object otherData { get; set; }
    }

    public class CKEditorUploadedError
    {
        public string message { get; set; }
    }
}