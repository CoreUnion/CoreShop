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
using SqlSugar;

namespace CoreCms.Net.IRepository
{
    public interface ICodeGeneratorRepository
    {
        /// <summary>
        ///     获取所有的表
        /// </summary>
        /// <returns></returns>
        List<DbTableInfo> GetDbTables();

        /// <summary>
        ///     获取表下面所有的字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        List<DbColumnInfo> GetDbTablesColumns(string tableName);

        ///// <summary>
        ///// 自动生成全部代码
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        byte[] CodeGen(string tableName, string fileType);


        /// <summary>
        ///     自动生成类型的所有数据库代码
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        byte[] CodeGenByAll(string fileType);
    }
}