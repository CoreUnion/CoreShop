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
using System.IO;
using System.Threading.Tasks;
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using ToolGood.Words;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     标签表 接口实现
    /// </summary>
    public class ToolsServices : IToolsServices
    {
        private IWebHostEnvironment _hostEnvironment;

        public ToolsServices(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }


        /// <summary>
        /// 查询是否存在违规内容并进行替换
        /// </summary>
        /// <returns></returns>
        public async Task<String> IllegalWordsReplace(string oldString, char symbol = '*')
        {
            var cache = ManualDataCache.Instance.Get<string>(ToolsVars.IllegalWordsCahceName);
            if (string.IsNullOrEmpty(cache))
            {
                IFileProvider fileProvider = this._hostEnvironment.ContentRootFileProvider;
                IFileInfo fileInfo = fileProvider.GetFileInfo("illegalWord/IllegalKeywords.txt");

                string fileContent = null;

                using (StreamReader readSteam = new StreamReader(fileInfo.CreateReadStream()))
                {
                    fileContent = await readSteam.ReadToEndAsync();
                }
                cache = fileContent;
                ManualDataCache.Instance.Set(ToolsVars.IllegalWordsCahceName, cache);
            }

            WordsMatch wordsSearch = new WordsMatch();
            wordsSearch.SetKeywords(cache.Split("|"));

            var t = wordsSearch.Replace(oldString, symbol);
            return t;
        }


        /// <summary>
        /// 查询是否存在违规内容
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IllegalWordsContainsAny(string oldString)
        {
            var cache = ManualDataCache.Instance.Get<string>(ToolsVars.IllegalWordsCahceName);
            if (string.IsNullOrEmpty(cache))
            {
                IFileProvider fileProvider = this._hostEnvironment.ContentRootFileProvider;
                IFileInfo fileInfo = fileProvider.GetFileInfo("illegalWord/IllegalKeywords.txt");

                string fileContent = null;

                using (StreamReader readSteam = new StreamReader(fileInfo.CreateReadStream()))
                {
                    fileContent = await readSteam.ReadToEndAsync();
                }
                cache = fileContent;
                ManualDataCache.Instance.Set(ToolsVars.IllegalWordsCahceName, cache);
            }

            WordsMatch wordsSearch = new WordsMatch();
            wordsSearch.SetKeywords(cache.Split("|"));

            var bl = wordsSearch.ContainsAny(oldString);

            return bl;
        }

    }
}