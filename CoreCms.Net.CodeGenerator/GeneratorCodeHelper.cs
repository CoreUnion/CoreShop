using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using CoreCms.Net.Model.ViewModels.Basics;
using DotLiquid;
using SqlSugar;

namespace CoreCms.Net.CodeGenerator
{
    /// <summary>
    /// 自动生成代码
    /// </summary>
    public static class GeneratorCodeHelper
    {

        /// <summary>
        /// 单表生成对应数据
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="tableDescription">表说明</param>
        /// <param name="columns">表字段</param>
        public static byte[] CodeGenerator(string tableName, string tableDescription, List<DbColumnInfo> columns, string fileType)
        {
            //ModelClassName
            //ModelName
            //ModelFields  Name Comment
            var dt = DateTime.Now;
            byte[] data;
            var obj = new
            {
                ModelCreateTime= dt,
                ModelName = tableName,
                ModelDescription = tableDescription,
                ModelClassName = tableName,
                ModelFields = columns.Select(r => new
                {
                    r.DbColumnName,
                    r.ColumnDescription,
                    r.DataType,
                    r.DecimalDigits,
                    r.DefaultValue,
                    r.IsIdentity,
                    r.IsNullable,
                    r.IsPrimarykey,
                    //Length = (r.DataType == "nvarchar" && r.Length > 0) ? r.Length / 2 : r.Length,
                    r.Length,
                    r.PropertyName,
                    r.PropertyType,
                    r.Scale,
                    r.TableId,
                    r.TableName,
                    r.Value
                }).ToArray()
            };
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(GeneratorCodeHelper)).Assembly;
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipArchive zip = new ZipArchive(ms, ZipArchiveMode.Create, false))
                {
                    string file;
                    string result;
                    Template template;

                    switch (fileType)
                    {
                        case "AllFiles":
                            //Controller
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Controllers.Controller.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry4 = zip.CreateEntry("Controller/" + tableName + "Controller.cs");
                                using (StreamWriter entryStream = new StreamWriter(entry4.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            //IRespository
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Repositories.IRepository.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry3 = zip.CreateEntry("IRepository/I" + tableName + "Repository.cs");
                                using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            //Respository
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Repositories.Repository.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Repository/" + tableName + "Repository.cs");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }

                            //IServices
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Services.IServices.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry3 = zip.CreateEntry("IServices/I" + tableName + "Services.cs");
                                using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            //Services
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Services.Services.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Services/" + tableName + "Services.cs");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }

                            //Model
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.DbModel.Model.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Entity/" + tableName + ".cs");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }

                            //CreateHtml
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Create.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + tableName.ToLower().ToLower() + "/create.html");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            //EditHtml
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Edit.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + tableName.ToLower().ToLower() + "/edit.html");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }

                            //DetailsHtml
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Details.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + tableName.ToLower().ToLower() + "/details.html");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }

                            //IndexHtml
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Index.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + tableName.ToLower().ToLower() + "/index.html");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            break;
                        case "EntityFiles":
                            //Model
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.DbModel.Model.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Entity/" + tableName + ".cs");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            break;
                        case "ServicesFiles":
                            //IServices
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Services.IServices.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry3 = zip.CreateEntry("IServices/I" + tableName + "Services.cs");
                                using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            //Services
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Services.Services.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Services/" + tableName + "Services.cs");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            break;
                        case "ViewFiles":
                            //CreateHtml
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Create.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + tableName.ToLower().ToLower() + "/create.html");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            //EditHtml
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Edit.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + tableName.ToLower().ToLower() + "/edit.html");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }

                            //DetailsHtml
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Details.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + tableName.ToLower().ToLower() + "/details.html");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }

                            //IndexHtml
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Index.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + tableName.ToLower().ToLower() + "/index.html");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            break;
                        case "InterFaceFiles":
                            //IRespository
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Repositories.IRepository.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry3 = zip.CreateEntry("IRepository/I" + tableName + "Repository.cs");
                                using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            //Respository
                            using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Repositories.Repository.tpl"), Encoding.UTF8))
                            {
                                file = reader.ReadToEnd();
                                template = Template.Parse(file);
                                result = template.Render(Hash.FromAnonymousObject(obj));
                                ZipArchiveEntry entry1 = zip.CreateEntry("Repository/" + tableName + "Repository.cs");
                                using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                {
                                    entryStream.Write(result);
                                }
                            }
                            break;
                    }
                }
                data = ms.ToArray();
            }
            return data;
        }

        /// <summary>
        /// 生成整个数据库文件
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="tableDescription">表说明</param>
        /// <param name="columns">表字段</param>
        public static byte[] CodeGeneratorAll(List<DbTableInfoAndColumns> dbModels, string fileType)
        {
            //ModelClassName
            //ModelName
            //ModelFields  Name Comment
            byte[] data;

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(GeneratorCodeHelper)).Assembly;
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipArchive zip = new ZipArchive(ms, ZipArchiveMode.Create, false))
                {
                    string file;
                    string result;
                    Template template;

                    foreach (var item in dbModels)
                    {
                        var obj = new
                        {
                            ModelName = item.Name,
                            ModelDescription = item.Description,
                            ModelClassName = item.Name,
                            ModelFields = item.columns.Select(r => new
                            {
                                r.DbColumnName,
                                r.ColumnDescription,
                                r.DataType,
                                r.DecimalDigits,
                                r.DefaultValue,
                                r.IsIdentity,
                                r.IsNullable,
                                r.IsPrimarykey,
                                Length = (r.DataType == "nvarchar" && r.Length > 0) ? r.Length / 2 : r.Length,
                                r.PropertyName,
                                r.PropertyType,
                                r.Scale,
                                r.TableId,
                                r.TableName,
                                r.Value
                            }).ToArray()
                        };

                        switch (fileType)
                        {
                            case "AllFiles":
                                //Controller
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Controllers.Controller.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry4 = zip.CreateEntry("Controller/" + item.Name + "Controller.cs");
                                    using (StreamWriter entryStream = new StreamWriter(entry4.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                //IRespository
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Repositories.IRepository.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry3 = zip.CreateEntry("IRepository/I" + item.Name + "Repository.cs");
                                    using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                //Respository
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Repositories.Repository.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Repository/" + item.Name + "Repository.cs");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }

                                //IServices
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Services.IServices.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry3 = zip.CreateEntry("IServices/I" + item.Name + "Services.cs");
                                    using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                //Services
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Services.Services.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Services/" + item.Name + "Services.cs");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }

                                //Model
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.DbModel.Model.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Entity/" + item.Name + ".cs");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }

                                //CreateHtml
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Create.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + item.Name.ToLower().ToLower() + "/create.html");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                //EditHtml
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Edit.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + item.Name.ToLower().ToLower() + "/edit.html");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }

                                //DetailsHtml
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Details.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + item.Name.ToLower().ToLower() + "/details.html");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }

                                //IndexHtml
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Index.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + item.Name.ToLower().ToLower() + "/index.html");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                break;
                            case "EntityFiles":
                                //Model
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.DbModel.Model.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Entity/" + item.Name + ".cs");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                break;
                            case "ServicesFiles":
                                //IServices
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Services.IServices.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry3 = zip.CreateEntry("IServices/I" + item.Name + "Services.cs");
                                    using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                //Services
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Services.Services.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Services/" + item.Name + "Services.cs");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                break;
                            case "ViewFiles":
                                //CreateHtml
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Create.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + item.Name.ToLower().ToLower() + "/create.html");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                //EditHtml
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Edit.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + item.Name.ToLower().ToLower() + "/edit.html");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }

                                //DetailsHtml
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Details.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + item.Name.ToLower().ToLower() + "/details.html");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }

                                //IndexHtml
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.View.Index.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Html/" + item.Name.ToLower().ToLower() + "/index.html");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                break;
                            case "InterFaceFiles":
                                //IRespository
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Repositories.IRepository.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry3 = zip.CreateEntry("IRepository/I" + item.Name + "Repository.cs");
                                    using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                //Respository
                                using (var reader = new StreamReader(assembly.GetManifestResourceStream("CoreCms.Net.CodeGenerator.CrudTemplete.Repositories.Repository.tpl"), Encoding.UTF8))
                                {
                                    file = reader.ReadToEnd();
                                    template = Template.Parse(file);
                                    result = template.Render(Hash.FromAnonymousObject(obj));
                                    ZipArchiveEntry entry1 = zip.CreateEntry("Repository/" + item.Name + "Repository.cs");
                                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                                    {
                                        entryStream.Write(result);
                                    }
                                }
                                break;
                        }
                    }


                }
                data = ms.ToArray();
            }
            return data;
        }

    }
}
