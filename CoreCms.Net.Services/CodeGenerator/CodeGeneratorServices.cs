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
using CoreCms.Net.IRepository;
using CoreCms.Net.IServices;
using SqlSugar;

namespace CoreCms.Net.Services.CodeGenerator
{
    public class CodeGeneratorServices : ICodeGeneratorServices
    {
        ICodeGeneratorRepository _dal;

        public CodeGeneratorServices(ICodeGeneratorRepository dal)
        {
            this._dal = dal;
        }


        /// <summary>
        /// 获取所有的表
        /// </summary>
        /// <returns></returns>
        public List<DbTableInfo> GetDbTables()
        {
            return _dal.GetDbTables();
        }

        /// <summary>
        /// 获取表下面所有的字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<DbColumnInfo> GetDbTablesColumns(string tableName)
        {
            return _dal.GetDbTablesColumns(tableName);
        }


        /// <summary>
        /// 自动生成代码
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public byte[] CodeGen(string tableName, string fileType)
        {
            return _dal.CodeGen(tableName, fileType);
        }



        /// <summary>
        /// 自动生成类型的所有数据库代码
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public byte[] CodeGenByAll(string fileType)
        {

            return _dal.CodeGenByAll(fileType);
        }

    }
}
