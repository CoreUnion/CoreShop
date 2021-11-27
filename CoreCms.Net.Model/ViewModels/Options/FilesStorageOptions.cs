/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


namespace CoreCms.Net.Model.ViewModels.Options
{
    /// <summary>
    ///     存储配置转换对象
    /// </summary>
    public class FilesStorageOptions
    {
        /// <summary>
        ///     存储方式（'LocalStorage','AliYunOSS','QCloudOSS'）
        /// </summary>
        public string StorageType { get; set; } = "LocalStorage";

        /// <summary>
        ///     存储目录
        /// </summary>
        public string Path { get; set; } = "Upload";

        /// <summary>
        ///     账户标识（腾讯云）
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        ///     存储桶地域（腾讯云）
        /// </summary>
        public string CosRegion { get; set; }

        /// <summary>
        ///     存储桶名称（腾讯云）
        /// </summary>
        public string TencentBucketName { get; set; }

        /// <summary>
        ///     存储桶名称（七牛云）
        /// </summary>
        public string QiNiuBucketName { get; set; }

        /// <summary>
        ///     授权账户
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        ///     授权密钥
        /// </summary>
        public string AccessKeySecret { get; set; }

        /// <summary>
        ///     节点
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        ///     桶名称
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        ///     桶绑定域名
        /// </summary>
        public string BucketBindUrl { get; set; }

        /// <summary>
        ///     文件类型
        /// </summary>
        public string FileTypes { get; set; } = "gif,jpg,jpeg,png,bmp,xls,xlsx,doc,pdf,mp4,WebM,Ogv";

        /// <summary>
        ///     最大允许上传单个文件大小（M）
        /// </summary>
        public int MaxSize { get; set; } = 20;
    }
}