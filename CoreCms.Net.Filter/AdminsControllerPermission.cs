/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-10-21 21:46:04
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreCms.Net.Filter
{
    public class AdminsControllerPermission
    {
        /// <summary>
        ///     反射获取所有controller 和action
        /// </summary>
        /// <returns></returns>
        public static List<ControllerPermission> GetAllControllerAndActionByAssembly()
        {

            var result = new List<ControllerPermission>();

            var types = Assembly.Load("CoreCms.Net.Web.Admin").GetTypes();


            var noController = new[] { "ToolsController", "LoginController", "DemoController" };

            var controllers = types.Where(p => p.Name.Contains("Controller") && !noController.Contains(p.Name));
            foreach (var type in controllers)
            {
                if (type.Name.Length > 10 && type.BaseType.Name == "ControllerBase" && type.Name.EndsWith("Controller")) //如果是Controller
                {
                    var members = type.GetMethods();
                    var cp = new ControllerPermission
                    {
                        name = type.Name.Substring(0, type.Name.Length - 10),
                        action = new List<ActionPermission>()
                    };

                    var objs = type.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (objs.Length > 0) cp.description = (objs[0] as DescriptionAttribute).Description;

                    if (!string.IsNullOrEmpty(cp.description))
                    {
                        cp.name += "【" + cp.description + "】";
                    }


                    var newMembers = members.Where(p =>
                        p.ReturnType.FullName != null && (p.ReturnType.Name == "ActionResult" ||
                                                          p.ReturnType.Name == "FileResult" ||
                                                          p.ReturnType.Name == "JsonResult" ||
                                                          (p.ReturnType.GenericTypeArguments.Length > 0 && p.ReturnType.GenericTypeArguments[0].Name == "JsonResult") ||
                                                          p.ReturnType.Name == "AdminUiCallBack" ||
                                                          p.ReturnType.Name == "IActionResult" ||
                                                          p.ReturnType.FullName.Contains("CoreCms.Net.Model.ViewModels.UI.AdminUiCallBack"))
                        ).ToList();

                    foreach (var member in newMembers)
                    {
                        if (member.Name == "ValidationProblem" || member.Name == "Json") continue;

                        //if (member.ReturnType.Name == "ActionResult" || member.ReturnType.Name == "FileResult" || member.ReturnType.Name == "JsonResult" || (member.ReturnType.GenericTypeArguments.Length > 0 && member.ReturnType.GenericTypeArguments[0].Name == "JsonResult")) //如果是Action
                        //{
                        //}


                        var ap = new ActionPermission
                        {
                            name = member.Name,
                            actionName = member.Name,
                            controllerName = member.DeclaringType.Name.Substring(0, member.DeclaringType.Name.Length - 10)
                        };
                        // 去掉“Controller”后缀

                        var attrs = member.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (attrs.Length > 0) ap.description = (attrs[0] as DescriptionAttribute).Description;

                        if (!string.IsNullOrEmpty(ap.description))
                        {
                            ap.name += "【" + ap.description + "】";
                        }
                        cp.action.Add(ap);

                    }
                    cp.action = cp.action.Distinct(new ModelComparer()).ToList();
                    result.Add(cp);
                }
            }
            return result;
        }

        private class ModelComparer : IEqualityComparer<ActionPermission>
        {
            public bool Equals(ActionPermission x, ActionPermission y)
            {
                return x.name.ToUpper() == y.name.ToUpper();
            }

            public int GetHashCode(ActionPermission obj)
            {
                return obj.name.ToUpper().GetHashCode();
            }
        }
    }



    public class ActionPermission
    {
        /// <summary>
        ///     请求地址
        /// </summary>
        public virtual string name { get; set; }

        /// <summary>
        ///     请求地址
        /// </summary>
        public virtual string controllerName { get; set; }

        /// <summary>
        ///     请求地址
        /// </summary>
        public virtual string actionName { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public virtual string description { get; set; }

        public virtual string type { get; set; } = "action";

    }

    public class ControllerPermission
    {
        public virtual string name { get; set; }

        public virtual string description { get; set; }

        public virtual IList<ActionPermission> action { get; set; }


        public virtual string type { get; set; } = "controller";

    }
}
