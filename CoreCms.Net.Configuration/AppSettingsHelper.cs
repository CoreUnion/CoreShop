
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json.Linq;
using SqlSugar.Extensions;

namespace CoreCms.Net.Configuration
{
    /// <summary>
    /// 获取Appsettings配置信息
    /// </summary>
    public class AppSettingsHelper
    {
        static IConfiguration Configuration { get; set; }

        public AppSettingsHelper(string contentPath)
        {
            string Path = "appsettings.json";
            Configuration = new ConfigurationBuilder().SetBasePath(contentPath).Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true }).Build();
        }

        /// <summary>
        /// 封装要操作的字符
        /// AppSettingsHelper.GetContent(new string[] { "JwtConfig", "SecretKey" });
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string GetContent(params string[] sections)
        {
            try
            {

                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

            return "";
        }




        /// <summary>
        /// 获取电脑 MAC（物理） 地址
        /// </summary>
        /// <param name="needToken">是否只是为了套取key生成一个不同部署环境不同的序列串</param>
        /// <returns></returns>
        public static string GetMACIp(bool needToken)
        {
            //本地计算机网络连接信息
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            //获取本机所有网络连接
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            //获取本机电脑名
            var HostName = computerProperties.HostName;
            //获取域名
            var DomainName = computerProperties.DomainName;

            if (nics == null || nics.Length < 1)
            {
                return "";
            }

            var MACIp = needToken ? HostName + DomainName : "";
            foreach (NetworkInterface adapter in nics)
            {
                var adapterName = adapter.Name;

                var adapterDescription = adapter.Description;
                var NetworkInterfaceType = adapter.NetworkInterfaceType;
                if (adapterName == "本地连接" || needToken)
                {
                    PhysicalAddress address = adapter.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        MACIp += bytes[i].ToString("X2");

                        if (i != bytes.Length - 1)
                        {
                            MACIp += "-";
                        }
                    }
                }
            }

            return MACIp;
        }

        /// <summary>
        /// 获取电脑计算机名
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            //本地计算机网络连接信息
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();

            //获取本机电脑名
            var hostName = computerProperties.HostName;

            return !string.IsNullOrEmpty(hostName) ? hostName : "CoreShop.Professional";

        }




        /// <summary>
        /// 转MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMachineRandomKey(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }


    }
}