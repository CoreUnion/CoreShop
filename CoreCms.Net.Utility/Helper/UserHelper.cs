/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-02-26 0:58:28
 *        Description: 暂无
 ***********************************************************************/


using System;
using CoreCms.Net.Configuration;

namespace CoreCms.Net.Utility.Helper
{
    /// <summary>
    /// 用户帮助类
    /// </summary>
    public static class UserHelper
    {
        /// <summary>
        /// 获取金额来源备注
        /// </summary>
        /// <param name="tpye">类型</param>
        /// <param name="money">金额</param>
        /// <param name="cateMoney">手续费</param>
        /// <returns></returns>
        public static string GetMemo(int tpye, decimal money, decimal cateMoney = 0)
        {
            var str = string.Empty;
            switch (tpye)
            {
                case (int)GlobalEnumVars.UserBalanceSourceTypes.Pay:
                    str += "消费了" + money + "元";
                    break;
                case (int)GlobalEnumVars.UserBalanceSourceTypes.Refund:
                    str += "收到了退款" + money + "元";
                    break;
                case (int)GlobalEnumVars.UserBalanceSourceTypes.Recharge:
                    str += "充值了" + money + "元";
                    break;
                case (int)GlobalEnumVars.UserBalanceSourceTypes.Tocash:
                    str += "提现了" + money + "元";
                    if (cateMoney > 0)
                    {
                        str += ",手续费" + cateMoney + "元";
                    }
                    break;
                case (int)GlobalEnumVars.UserBalanceSourceTypes.Distribution:
                    str += "佣金" + money + "元";
                    break; 
                case (int)GlobalEnumVars.UserBalanceSourceTypes.Agent:
                    str += "佣金" + money + "元";
                    break;
                case (int)GlobalEnumVars.UserBalanceSourceTypes.Admin:
                    str += "后台操作" + money + "元";
                    break;
                case (int)GlobalEnumVars.UserBalanceSourceTypes.Prize:
                    str += "抽奖活动奖励" + money + "元";
                    break;
                case (int)GlobalEnumVars.UserBalanceSourceTypes.Service:
                    str += "购买服务消费了" + money + "元";
                    break;
            }
            //::todo    这里还可以做一些其他的校验
            return str;
        }

        /// <summary>
        /// 获取用户分享码(刻意封装)
        /// </summary>
        public static int GetShareCodeByUserId(int userId)
        {
            return (userId + 1234) * 3;
        }

        /// <summary>
        /// 解码获取用户ID(刻意封装)
        /// </summary>
        public static int GetUserIdByShareCode(int userId)
        {
            return (userId / 3) - 1234;
        }

        /// <summary>
        /// 将传入的字符串中间部分字符替换成特殊字符
        /// </summary>
        /// <param name="value">需要替换的字符串</param>
        /// <param name="startLen">前保留长度</param>
        /// <param name="endLen">尾保留长度</param>
        /// <param name="replaceChar">特殊字符</param>
        /// <returns>被特殊字符替换的字符串</returns>
        public static string BankCardNoFormat(string value, int startLen = 4, int endLen = 4, char specialChar = '*')
        {

            int lenth = value.Length - startLen - endLen;

            string replaceStr = value.Substring(startLen, lenth);

            string specialStr = string.Empty;

            for (int i = 0; i < replaceStr.Length; i++)
            {
                specialStr += specialChar;
            }

            value = value.Replace(replaceStr, specialStr);

            return value;
        }

        /// <summary>
        /// 格式化用户手机号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string FormatMobile(string mobile)
        {
            try
            {
                return mobile.Substring(0, 5) + "****" + mobile.Substring(9, 2);
            }
            catch
            {
                return mobile.Substring(0, 5) + "****";
            }
        }
    }
}
