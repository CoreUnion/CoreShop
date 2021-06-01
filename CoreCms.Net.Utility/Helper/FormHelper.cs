using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using NPOI.OpenXmlFormats.Dml;

namespace CoreCms.Net.Utility.Helper
{
    public static class FormHelper
    {

        /// <summary>
        /// 验证字段类型及提交的值是否对应
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static bool ValidateField(string typeName, object thisValue)
        {
            var bl = false;
            var valueType = thisValue.GetType();
            if (typeName == GlobalEnumVars.FormValidationTypes.字符串.ToString())
            {
                return valueType == typeof(string);
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.数字.ToString())
            {
                return thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out _);
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.整数.ToString())
            {
                return thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out _);
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.价格.ToString())
            {
                return thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out _);
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.邮箱.ToString())
            {
                if (valueType == typeof(string) && !string.IsNullOrEmpty(thisValue.ToString()))
                {
                    return Regex.IsMatch(thisValue.ToString() ?? string.Empty, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
                }
                else
                {
                    return false;
                }
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.手机号.ToString())
            {
                if (valueType == typeof(string) && !string.IsNullOrEmpty(thisValue.ToString()))
                {
                    return CommonHelper.IsMobile(thisValue.ToString());
                }
                else
                {
                    return false;
                }
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.多数据.ToString())
            {
                return valueType == typeof(Array);
            }
            else
            {
                bl = false;
            }
            return bl;
        }


    }
}
