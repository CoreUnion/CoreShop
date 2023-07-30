using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar.Extensions;

namespace CoreCms.Net.Configuration
{
    /// <summary>
    /// 配置文件格式化
    /// </summary>
    public class AppSettingsConstVars
    {

        #region 全局地址================================================================================
        /// <summary>
        /// 系统后端地址
        /// </summary>
        public static readonly string AppConfigAppUrl = AppSettingsHelper.GetContent("AppConfig", "AppUrl");
        /// <summary>
        /// 系统接口地址
        /// </summary>
        public static readonly string AppConfigAppInterFaceUrl = AppSettingsHelper.GetContent("AppConfig", "AppInterFaceUrl");
        #endregion

        #region 数据库================================================================================
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public static readonly string DbSqlConnection = AppSettingsHelper.GetContent("ConnectionStrings", "SqlConnection");
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        public static readonly string DbDbType = AppSettingsHelper.GetContent("ConnectionStrings", "DbType");
        #endregion

        #region redis================================================================================

        /// <summary>
        /// 获取redis连接字符串
        /// </summary>
        public static readonly string RedisConfigConnectionString = AppSettingsHelper.GetContent("RedisConfig", "ConnectionString");

        /// <summary>
        /// 启用redis作为缓存选择
        /// </summary>
        public static readonly bool RedisUseCache = AppSettingsHelper.GetContent("RedisConfig", "UseCache").ObjToBool();
        /// <summary>
        /// 启用redis作为定时任务
        /// </summary>
        public static readonly bool RedisUseTimedTask = AppSettingsHelper.GetContent("RedisConfig", "UseTimedTask").ObjToBool();

        #endregion

        #region AOP================================================================================
        /// <summary>
        /// 事务切面开关
        /// </summary>
        public static readonly bool TranAopEnabled = AppSettingsHelper.GetContent("TranAOP", "Enabled").ObjToBool();

        #endregion

        #region Jwt授权配置================================================================================

        public static readonly string JwtConfigSecretKey = AppSettingsHelper.GetContent("JwtConfig", "SecretKey") + AppSettingsHelper.GetMachineRandomKey(DbSqlConnection + AppSettingsHelper.GetMACIp(true));
        public static readonly string JwtConfigIssuer = !string.IsNullOrEmpty(AppSettingsHelper.GetContent("JwtConfig", "Issuer")) ? AppSettingsHelper.GetContent("JwtConfig", "Issuer") : AppSettingsHelper.GetHostName();
        public static readonly string JwtConfigAudience = AppSettingsHelper.GetContent("JwtConfig", "Audience");
        #endregion

        #region Cors跨域设置================================================================================
        public static readonly string CorsPolicyName = AppSettingsHelper.GetContent("Cors", "PolicyName");
        public static readonly bool CorsEnableAllIPs = AppSettingsHelper.GetContent("Cors", "EnableAllIPs").ObjToBool();
        public static readonly string CorsIPs = AppSettingsHelper.GetContent("Cors", "IPs");
        #endregion

        #region Middleware中间件================================================================================
        /// <summary>
        /// Ip限流
        /// </summary>
        public static readonly bool MiddlewareIpLogEnabled = AppSettingsHelper.GetContent("Middleware", "IPLog", "Enabled").ObjToBool();
        /// <summary>
        /// 记录请求与返回数据
        /// </summary>
        public static readonly bool MiddlewareRequestResponseLogEnabled = AppSettingsHelper.GetContent("Middleware", "RequestResponseLog", "Enabled").ObjToBool();
        /// <summary>
        /// 用户访问记录-是否开启
        /// </summary>
        public static readonly bool MiddlewareRecordAccessLogsEnabled = AppSettingsHelper.GetContent("Middleware", "RecordAccessLogs", "Enabled").ObjToBool();
        /// <summary>
        /// 用户访问记录-过滤ip
        /// </summary>
        public static readonly string MiddlewareRecordAccessLogsIgnoreApis = AppSettingsHelper.GetContent("Middleware", "RecordAccessLogs", "IgnoreApis");

        #endregion

        #region 支付================================================================================

        /// <summary>
        /// 微信支付回调
        /// </summary>
        public static readonly string PayCallBackWeChatPayUrl = AppSettingsHelper.GetContent("PayCallBack", "WeChatPayUrl");
        /// <summary>
        /// 微信退款回调
        /// </summary>
        public static readonly string PayCallBackWeChatRefundUrl = AppSettingsHelper.GetContent("PayCallBack", "WeChatRefundUrl");
        /// <summary>
        /// 支付宝支付回调
        /// </summary>
        public static readonly string PayCallBackAlipayUrl = AppSettingsHelper.GetContent("PayCallBack", "AlipayUrl");
        /// <summary>
        /// 支付宝退款回调
        /// </summary>
        public static readonly string PayCallBackAlipayRefundUrl = AppSettingsHelper.GetContent("PayCallBack", "AlipayRefundUrl");
        #endregion

        #region 易联云打印机================================================================================

        /// <summary>
        /// 是否开启
        /// </summary>
        public static readonly bool YiLianYunConfigEnabled = AppSettingsHelper.GetContent("YiLianYunConfig", "Enabled").ObjToBool();
        /// <summary>
        /// 应用ID
        /// </summary>
        public static readonly string YiLianYunConfigClientId = AppSettingsHelper.GetContent("YiLianYunConfig", "ClientId");
        /// <summary>
        /// 应用密钥
        /// </summary>
        public static readonly string YiLianYunConfigClientSecret = AppSettingsHelper.GetContent("YiLianYunConfig", "ClientSecret");
        /// <summary>
        /// 打印机设备号
        /// </summary>
        public static readonly string YiLianYunConfigMachineCode = AppSettingsHelper.GetContent("YiLianYunConfig", "MachineCode");
        /// <summary>
        /// 打印机终端密钥
        /// </summary>
        public static readonly string YiLianYunConfigMsign = AppSettingsHelper.GetContent("YiLianYunConfig", "Msign");
        /// <summary>
        /// 打印机名称
        /// </summary>
        public static readonly string YiLianYunConfigPrinterName = AppSettingsHelper.GetContent("YiLianYunConfig", "PrinterName");
        /// <summary>
        /// 打印机设置联系方式
        /// </summary>
        public static readonly string YiLianYunConfigPhone = AppSettingsHelper.GetContent("YiLianYunConfig", "Phone");

        #endregion

        #region HangFire定时任务================================================================================
        /// <summary>
        /// 登录账号
        /// </summary>
        public static readonly string HangFireLogin = AppSettingsHelper.GetContent("HangFire", "Login");
        /// <summary>
        /// 登录密码
        /// </summary>
        public static readonly string HangFirePassWord = AppSettingsHelper.GetContent("HangFire", "PassWord");


        #endregion

    }
}
