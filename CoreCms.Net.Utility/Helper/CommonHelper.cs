using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.ViewModels.Basics;

namespace CoreCms.Net.Utility.Helper
{
    /// <summary>
    /// 通用帮助类
    /// </summary>
    public static class CommonHelper
    {

        #region 判断字符串是否为手机号码
        /// <summary>
        /// 判断字符串是否为手机号码
        /// </summary>
        /// <param name="mobilePhoneNumber"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobilePhoneNumber)
        {
            if (mobilePhoneNumber.Length < 11)
            {
                return false;
            }

            //电信手机号码正则
            string dianxin = @"^1[345789][01379]\d{8}$";
            Regex regexDx = new Regex(dianxin);
            //联通手机号码正则
            string liantong = @"^1[345678][01256]\d{8}$";
            Regex regexLt = new Regex(liantong);
            //移动手机号码正则
            string yidong = @"^1[345789][0123456789]\d{8}$";
            Regex regexYd = new Regex(yidong);
            if (regexDx.IsMatch(mobilePhoneNumber) || regexLt.IsMatch(mobilePhoneNumber) || regexYd.IsMatch(mobilePhoneNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 检测是否符合email格式

        /// <summary>
        ///     检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }

        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail,
                @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        #region 检测是否是正确的Url
        /// <summary>
        ///     检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsUrl(string strUrl)
        {
            return Regex.IsMatch(strUrl,
                @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }


        #endregion

        #region string 转int数组

        public static int[] StringToIntArray(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return new int[0];
                if (str.EndsWith(","))
                {
                    str = str.Remove(str.Length - 1, 1);
                }
                var idstrarr = str.Split(',');
                var idintarr = new int[idstrarr.Length];

                for (int i = 0; i < idstrarr.Length; i++)
                {
                    idintarr[i] = Convert.ToInt32(idstrarr[i]);
                }
                return idintarr;
            }
            catch
            {
                return new int[0];
            }
        }
        #endregion

        #region String转数组
        public static string[] StringToStringArray(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return new string[0];
                if (str.EndsWith(",")) str = str.Remove(str.Length - 1, 1);
                return str.Split(',');
            }
            catch
            {
                return new string[0];
            }
        }
        #endregion

        #region String数组转Int数组
        public static int[] StringArrAyToIntArray(string[] str)
        {
            try
            {
                int[] iNums = Array.ConvertAll<string, int>(str, s => int.Parse(s));
                return iNums;
            }
            catch
            {
                return new int[0];
            }
        }
        #endregion

        #region string转Guid数组
        public static System.Guid[] StringToGuidArray(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return new System.Guid[0];
                if (str.EndsWith(",")) str = str.Remove(str.Length - 1, 1);
                var strarr = str.Split(',');
                System.Guid[] guids = new System.Guid[strarr.Length];
                for (int index = 0; index < strarr.Length; index++)
                {
                    guids[index] = System.Guid.Parse(strarr[index]);
                }
                return guids;
            }
            catch
            {
                return new System.Guid[0];
            }
        }
        #endregion

        #region 获取32位md5加密
        /// <summary>
        /// 通过创建哈希字符串适用于任何 MD5 哈希函数 （在任何平台） 上创建 32 个字符的十六进制格式哈希字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns>32位md5加密字符串</returns>
        public static string Md5For32(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                string hash = sBuilder.ToString();
                return hash.ToUpper();
            }
        }
        #endregion

        #region 获取16位md5加密
        /// <summary>
        /// 获取16位md5加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns>16位md5加密字符串</returns>
        public static string Md5For16(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                //转换成字符串，并取9到25位
                string sBuilder = BitConverter.ToString(data, 4, 8);
                //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉
                sBuilder = sBuilder.Replace("-", "");
                return sBuilder.ToUpper();
            }
        }

        #endregion

        #region 返回当前的毫秒时间戳

        /// <summary>
        /// 返回当前的毫秒时间戳
        /// </summary>
        public static string Msectime()
        {
            long timeTicks = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            return timeTicks.ToString();
        }


        #endregion

        #region 获取多种数据编号
        /// <summary>
        /// 获取多种数据编号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSerialNumberType(int type)
        {
            var str = string.Empty;
            Random rand = new Random();
            switch (type)
            {
                case (int)GlobalEnumVars.SerialNumberType.订单编号:         //订单编号
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.支付单编号:         //支付单编号
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.商品编号:         //商品编号
                    str = 'G' + Msectime() + rand.Next(0, 5);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.货品编号:         //货品编号
                    str = 'P' + Msectime() + rand.Next(0, 5);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.售后单编号:         //售后单编号
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.退款单编号:         //退款单编号
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.退货单编号:         //退货单编号
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.发货单编号:         //发货单编号
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.服务订单编号:         //服务订单编号
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.提货单号:         //提货单号
                    //str = 'T' + type + msectime() + rand.Next(0, 5);
                    var charsStr = new[] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var charsLen = charsStr.Length - 1;
                    //    shuffle($chars);
                    str = "";
                    for (int i = 0; i < 6; i++)
                    {
                        str += charsStr[rand.Next(0, charsLen)];
                    }
                    break;
                case (int)GlobalEnumVars.SerialNumberType.服务券兑换码:         //服务券兑换码
                    var charsStr2 = new[] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var charsLen2 = charsStr2.Length - 1;
                    //    shuffle($chars);
                    str = "";
                    for (int i = 0; i < 6; i++)
                    {
                        str += charsStr2[rand.Next(0, charsLen2)];
                    }
                    break;
                default:
                    str = 'T' + Msectime() + rand.Next(0, 9);
                    break;
            }
            return str;
        }

        #endregion

        #region 剩余多久时间文字描述
        /// <summary>
        /// 剩余多久时间
        /// </summary>
        /// <param name="remainingTime"></param>
        /// <returns>文字描述</returns>
        public static string GetRemainingTime(DateTime remainingTime)
        {
            TimeSpan timeSpan = remainingTime - DateTime.Now;
            var day = timeSpan.Days;
            var hours = timeSpan.Hours;
            var minute = timeSpan.Minutes;
            var seconds = timeSpan.Seconds;
            if (day > 0)
            {
                return day + "天" + hours + "小时" + minute + "分" + seconds + "秒";
            }
            else
            {
                if (hours > 0)
                {
                    return hours + "小时" + minute + "分" + seconds + "秒";
                }
                else
                {
                    return minute + "分" + seconds + "秒";
                }
            }
        }

        #endregion

        #region 剩余多久时间返回时间类型
        /// <summary>
        /// 剩余多久时间
        /// </summary>
        /// <param name="remainingTime"></param>
        /// <returns>返回时间类型</returns>
        public static void GetBackTime(DateTime remainingTime, out int day, out int hours, out int minute, out int seconds)
        {
            TimeSpan timeSpan = remainingTime - DateTime.Now;
            day = timeSpan.Days;
            hours = timeSpan.Hours;
            minute = timeSpan.Minutes;
            seconds = timeSpan.Seconds;
        }

        #endregion

        #region 计算时间戳剩余多久时间

        /// <summary>
        /// 计算时间戳剩余多久时间
        /// </summary>
        /// <param name="postTime">提交时间(要是以前的时间)</param>
        /// <returns></returns>
        public static string TimeAgo(DateTime postTime)
        {
            //当前时间的时间戳
            var nowtimes = ConvertTicks(DateTime.Now);
            //提交的时间戳
            var posttimes = ConvertTicks(postTime);
            //相差时间戳
            var counttime = nowtimes - posttimes;

            //进行时间转换
            if (counttime <= 60)
            {
                return "刚刚";
            }
            else if (counttime > 60 && counttime <= 120)
            {
                return "1分钟前";
            }
            else if (counttime > 120 && counttime <= 180)
            {
                return "2分钟前";
            }
            else if (counttime > 180 && counttime < 3600)
            {
                return Convert.ToInt32((counttime / 60)) + "分钟前";
            }
            else if (counttime >= 3600 && counttime < 3600 * 24)
            {
                return Convert.ToInt32((counttime / 3600)) + "小时前";
            }
            else if (counttime >= 3600 * 24 && counttime < 3600 * 24 * 2)
            {
                return "昨天";
            }
            else if (counttime >= 3600 * 24 * 2 && counttime < 3600 * 24 * 3)
            {
                return "前天";
            }
            else if (counttime >= 3600 * 24 * 3 && counttime <= 3600 * 24 * 7)
            {
                return Convert.ToInt32((counttime / (3600 * 24))) + "天前";
            }
            else if (counttime >= 3600 * 24 * 7 && counttime <= 3600 * 24 * 30)
            {
                return Convert.ToInt32((counttime / (3600 * 24 * 7))) + "周前";
            }
            else if (counttime >= 3600 * 24 * 30 && counttime <= 3600 * 24 * 365)
            {
                return Convert.ToInt32((counttime / (3600 * 24 * 30))) + "个月前";
            }
            else if (counttime >= 3600 * 24 * 365)
            {
                return Convert.ToInt32((counttime / (3600 * 24 * 365))) + "年前";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 时间转换为秒的时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static long ConvertTicks(DateTime time)
        {
            long currentTicks = time.Ticks;
            DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long currentMillis = (currentTicks - dtFrom.Ticks) / 10000000;  //转换为秒为Ticks/10000000，转换为毫秒Ticks/10000
            return currentMillis;
        }

        #endregion

        #region 清除HTML中指定样式
        /// <summary>
        /// 清除HTML中指定样式
        /// </summary>
        /// <param name="content"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static string ClearHtml(string content, string[] rule)
        {
            if (!rule.Any())
            {
                return content;
            }

            foreach (var item in rule)
            {
                content = Regex.Replace(content, "/" + item + @"\s*=\s*\d+\s*/i", "");
                content = Regex.Replace(content, "/" + item + @"\s*=\s*.+?[""]/i", "");
                content = Regex.Replace(content, "/" + item + @"\s*:\s*\d+\s*px\s*;?/i", "");
            }
            return content;
        }
        #endregion

        #region list随机排序方法
        /// <summary>
        /// list随机排序方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListT"></param>
        /// <returns></returns>
        public static List<T> RandomSortList<T>(List<T> ListT)
        {
            Random random = new Random();
            List<T> newList = new List<T>();
            foreach (T item in ListT)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            return newList;
        }
        #endregion

        #region 从字典中取单个数据
        /// <summary>
        /// 从字典中取单个数据
        /// </summary>
        /// <param name="configs"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        public static string GetConfigDictionary(Dictionary<string, DictionaryKeyValues> configs, string skey)
        {
            configs.TryGetValue(skey, out var di);
            return di?.sValue;
        }

        #endregion

        #region 截前后字符(串)
        ///<summary>
        /// 截前后字符(串)
        ///</summary>
        ///<param name="val">原字符串</param>
        ///<param name="str">要截掉的字符串</param>
        ///<param name="all">是否贪婪</param>
        ///<returns></returns>
        public static string GetCaptureInterceptedText(string val, string str, bool all = false)
        {
            return Regex.Replace(val, @"(^(" + str + ")" + (all ? "*" : "") + "|(" + str + ")" + (all ? "*" : "") + "$)", "");
        }
        #endregion

        #region 密码加密方法
        /// <summary>
        /// 密码加密方法
        /// </summary>
        /// <param name="password">要加密的字符串</param>
        /// <param name="createTime">时间组合</param>
        /// <returns></returns>
        public static string EnPassword(string password, DateTime createTime)
        {
            var dtStr = createTime.ToString("yyyyMMddHHmmss");
            var md5 = Md5For32(password);
            var enPwd = Md5For32(md5 + dtStr);
            return enPwd;
        }
        #endregion

        #region 获取现在是星期几
        /// <summary>
        /// 获取现在是星期几
        /// </summary>
        /// <returns></returns>
        public static string GetWeek()
        {
            string week = string.Empty;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    week = "周一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "周三";
                    break;
                case DayOfWeek.Thursday:
                    week = "周四";
                    break;
                case DayOfWeek.Friday:
                    week = "周五";
                    break;
                case DayOfWeek.Saturday:
                    week = "周六";
                    break;
                case DayOfWeek.Sunday:
                    week = "周日";
                    break;
                default:
                    week = "N/A";
                    break;
            }
            return week;
        }

        #endregion

        #region UrlEncode (URL编码)
        /// <summary>
        /// UrlEncode (URL编码)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        #endregion

        #region 获取10位时间戳
        /// <summary>
        /// 获取10位时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampByTotalSeconds()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        #endregion

        #region 获取13位时间戳
        /// <summary>
        /// 获取13位时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampByTotalMilliseconds()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
        #endregion


        /// <summary>
        ///  精确计算base64字符串文件大小（单位：B）
        ///  param base64String
        ///  return double 字节大小
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static double Base64FileSize(String base64String)
        {
            //检测是否含有base64,文件头)
            if (base64String.LastIndexOf(",", StringComparison.Ordinal) > -1)
            {
                base64String = base64String[(base64String.LastIndexOf(",", StringComparison.Ordinal) + 1)..];
            }
            //获取base64字符串长度(不含data:audio/wav;base64,文件头)
            var size0 = base64String.Length;
            if (size0 <= 10) return size0 - ((double)size0 / 8) * 2;
            //获取字符串的尾巴的最后10个字符，用于判断尾巴是否有等号，正常生成的base64文件'等号'不会超过4个
            var tail = base64String[(size0 - 10)..];
            //找到等号，把等号也去掉,(等号其实是空的意思,不能算在文件大小里面)
            int equalIndex = tail.IndexOf("=", StringComparison.Ordinal);
            if (equalIndex > 0)
            {
                size0 = size0 - (10 - equalIndex);
            }
            //计算后得到的文件流大小，单位为字节
            return size0 - ((double)size0 / 8) * 2;
        }

        /// <summary>
        /// 判断文件大小
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="size"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool CheckBase64Size(string base64, int size, string unit = "M")
        {
            // 上传文件的大小, 单位为字节.
            var len = Base64FileSize(base64);
            // 准备接收换算后文件大小的容器
            double fileSize = unit.ToUpperInvariant() switch
            {
                "B" => len,
                "K" => (double)len / 1024,
                "M" => (double)len / 1048576,
                "G" => (double)len / 1073741824,
                _ => 0
            };
            // 如果上传文件大于限定的容量
            return !(fileSize > size);
        }


        /// <summary>
        /// 10位时间戳 转化
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000000;   //除10000000调整为10位    
            return t;
        }

    }
}
