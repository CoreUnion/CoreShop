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
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.Options;
using CoreCms.Net.Model.ViewModels.Sms;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 店铺设置表 接口实现
    /// </summary>
    public class CoreCmsSettingServices : BaseServices<CoreCmsSetting>, ICoreCmsSettingServices
    {
        private readonly ICoreCmsSettingRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsSettingServices(IUnitOfWork unitOfWork, ICoreCmsSettingRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateAsync(FMCoreCmsSettingDoSaveModel model)
        {
            var jm = new AdminUiCallBack();

            var entity = model.entity;
            if (!entity.Any())
            {
                jm.msg = "数据不能为空";
                return jm;
            }
            var oldList = await _dal.QueryAsync();
            var bl = false;
            if (oldList.Any())
            {
                var arr = entity.Select(p => p.sKey).ToList();
                var old = oldList.Where(p => arr.Contains(p.sKey)).ToList();
                if (old.Any())
                {
                    old.ForEach(p =>
                    {
                        var o = entity.Find(c => c.sKey == p.sKey);
                        p.sValue = o != null ? o.sValue : "";
                    });
                    bl = await base.UpdateAsync(old);
                }
                var arrOld = oldList.Select(p => p.sKey).ToList();
                var newData = entity.Where(p => !arrOld.Contains(p.sKey)).ToList();
                if (newData.Any())
                {
                    var settings = new List<CoreCmsSetting>();
                    newData.ForEach(p =>
                    {
                        settings.Add(new CoreCmsSetting() { sKey = p.sKey, sValue = p.sValue.ToString() });
                    });
                    bl = await base.InsertAsync(settings) > 0;
                }
            }
            else
            {
                var settings = new List<CoreCmsSetting>();
                entity.ForEach(p =>
                {
                    settings.Add(new CoreCmsSetting() { sKey = p.sKey, sValue = p.sValue.ToString() });
                });
                bl = await base.InsertAsync(settings) > 0;
            }

            await UpdateCache();


            jm.code = bl ? 0 : 1;
            jm.msg = (bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure);

            return jm;
        }


        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <returns></returns>
        private async Task UpdateCache()
        {
            var list = await _dal.QueryAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheCoreCmsSettingList, list, 1440);

            var configs = SystemSettingDictionary.GetConfig();
            foreach (KeyValuePair<string, DictionaryKeyValues> kvp in configs)
            {
                var model = list.Find(p => p.sKey == kvp.Key);
                if (model != null)
                {
                    kvp.Value.sValue = model.sValue;
                }
            }
            ManualDataCache.Instance.Set(GlobalConstVars.CacheCoreCmsSettingByComparison, configs, 1440);
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <returns></returns>
        private async Task<List<CoreCmsSetting>> GetDatas()
        {
            var cache = ManualDataCache.Instance.Get<List<CoreCmsSetting>>(GlobalConstVars.CacheCoreCmsSettingList);
            if (cache == null)
            {
                var list = await _dal.QueryAsync();
                ManualDataCache.Instance.Set(GlobalConstVars.CacheCoreCmsSettingList, list, 1440);
                return list;
            }
            return ManualDataCache.Instance.Get<List<CoreCmsSetting>>(GlobalConstVars.CacheCoreCmsSettingList);
        }

        /// <summary>
        /// 获取数据库整合后配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, DictionaryKeyValues>> GetConfigDictionaries()
        {
            var configs = SystemSettingDictionary.GetConfig();
            var settings = await GetDatas();
            foreach (KeyValuePair<string, DictionaryKeyValues> kvp in configs)
            {
                var model = settings.Find(p => p.sKey == kvp.Key);
                if (model != null)
                {
                    kvp.Value.sValue = model.sValue;
                }
            }
            return configs;
        }


        /// <summary>
        /// 获取附件存储的配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<FilesStorageOptions> GetFilesStorageOptions()
        {
            var filesStorageOptions = new FilesStorageOptions();

            var configs = SystemSettingDictionary.GetConfig();
            var settings = await GetDatas();

            filesStorageOptions.StorageType = GetValue(SystemSettingConstVars.FilesStorageType, configs, settings);
            filesStorageOptions.Path = GetValue(SystemSettingConstVars.FilesStoragePath, configs, settings);
            filesStorageOptions.FileTypes = GetValue(SystemSettingConstVars.FilesStorageFileSuffix, configs, settings);
            filesStorageOptions.MaxSize = GetValue(SystemSettingConstVars.FilesStorageFileMaxSize, configs, settings).ObjectToInt(10);

            //云基础
            filesStorageOptions.BucketBindUrl = GetValue(SystemSettingConstVars.FilesStorageBucketBindUrl, configs, settings);
            filesStorageOptions.AccessKeyId = GetValue(SystemSettingConstVars.FilesStorageAccessKeyId, configs, settings);
            filesStorageOptions.AccessKeySecret = GetValue(SystemSettingConstVars.FilesStorageAccessKeySecret, configs, settings);
            //腾讯云
            filesStorageOptions.AccountId = GetValue(SystemSettingConstVars.FilesStorageTencentAccountId, configs, settings);
            filesStorageOptions.CosRegion = GetValue(SystemSettingConstVars.FilesStorageTencentCosRegion, configs, settings);
            filesStorageOptions.TencentBucketName = GetValue(SystemSettingConstVars.FilesStorageTencentBucketName, configs, settings);
            //阿里云
            filesStorageOptions.BucketName = GetValue(SystemSettingConstVars.FilesStorageAliYunBucketName, configs, settings);
            filesStorageOptions.Endpoint = GetValue(SystemSettingConstVars.FilesStorageAliYunEndpoint, configs, settings);

            //七牛云
            filesStorageOptions.QiNiuBucketName = GetValue(SystemSettingConstVars.FilesStorageQiNiuBucketName, configs, settings);

            //格式化存储文件夹路径
            filesStorageOptions.Path = UpLoadHelper.PathFormat(filesStorageOptions.StorageType, filesStorageOptions.Path);

            return filesStorageOptions;
        }


        /// <summary>
        /// 获取短信配置实体
        /// </summary>
        /// <returns></returns>
        public async Task<SMSOptions> GetSmsOptions()
        {
            var sms = new SMSOptions();

            var configs = SystemSettingDictionary.GetConfig();
            var settings = await GetDatas();

            sms.Enabled = GetValue(SystemSettingConstVars.SmsEnabled, configs, settings).ObjectToInt(1) == 1;
            sms.UserId = GetValue(SystemSettingConstVars.SmsUserId, configs, settings);
            sms.Account = GetValue(SystemSettingConstVars.SmsAccount, configs, settings);
            sms.Password = GetValue(SystemSettingConstVars.SmsPassword, configs, settings);
            sms.Signature = GetValue(SystemSettingConstVars.SmsSignature, configs, settings);
            sms.ApiUrl = GetValue(SystemSettingConstVars.SmsApiUrl, configs, settings);

            return sms;
        }


        public string GetValue(string key, Dictionary<string, DictionaryKeyValues> configs, List<CoreCmsSetting> settings)
        {
            var objSetting = settings.Find(p => p.sKey == key);
            if (objSetting != null)
            {
                return objSetting.sValue;
            }
            configs.TryGetValue(key, out var di);
            return di?.sValue;
        }

    }
}
