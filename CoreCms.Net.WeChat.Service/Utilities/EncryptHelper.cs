/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/29 1:06:57
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoreCms.Net.WeChat.Service.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.WeChat.Service.Utilities
{

    /// <summary>
    /// 签名及加密帮助类
    /// </summary>
    public static class EncryptHelper
    {
        ///// <summary>
        ///// SHA1加密
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static string EncryptToSHA1(string str)
        //{
        //    SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
        //    byte[] str1 = Encoding.UTF8.GetBytes(str);
        //    byte[] str2 = sha1.ComputeHash(str1);
        //    sha1.Clear();
        //    (sha1 as IDisposable).Dispose();
        //    return Convert.ToBase64String(str2);
        //}

        #region 签名


        /// <summary>
        /// 获得签名
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static string GetSignature(string rawData, string sessionKey)
        {
            var signature = GetSha1(rawData + sessionKey);
            //Senparc.Weixin.Helpers.EncryptHelper.SHA1_Encrypt(rawData + sessionKey);
            return signature;
        }

        /// <summary>采用SHA-1算法加密字符串（小写）</summary>
        /// <param name="encypStr">需要加密的字符串</param>
        /// <returns></returns>
        public static string GetSha1(string encypStr)
        {
            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(encypStr));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in hash)
                stringBuilder.AppendFormat("{0:x2}", (object)num);
            return stringBuilder.ToString();
        }


        /// <summary>
        /// 比较签名是否正确
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="rawData"></param>
        /// <param name="compareSignature"></param>
        /// <exception cref="WxOpenException">当SessionId或SessionKey无效时抛出异常</exception>
        /// <returns></returns>
        public static bool CheckSignature(string sessionKey, string rawData, string compareSignature)
        {
            var signature = GetSignature(rawData, sessionKey);
            return signature == compareSignature;
        }

        #endregion

        #region 解密

        #region 私有方法

        private static byte[] AES_Decrypt(String Input, byte[] Iv, byte[] Key)
        {
#if NET45
            RijndaelManaged aes = new RijndaelManaged();
#else
            SymmetricAlgorithm aes = Aes.Create();
#endif
            aes.KeySize = 128;//原始：256
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Key;
            aes.IV = Iv;
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            //using (ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV) /*aes.CreateDecryptor()*/)
            //{
            //    var src = Convert.FromBase64String(Input); 
            //    byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
            //    return dest;
            //    //return Encoding.UTF8.GetString(dest);
            //}


            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        //cs.Read(decryptBytes, 0, decryptBytes.Length);
                        //cs.Close();
                        //ms.Close();

                        //cs.FlushFinalBlock();//用于解决第二次获取小程序Session解密出错的情况


                        byte[] xXml = Convert.FromBase64String(Input);
                        byte[] msg = new byte[xXml.Length + 32 - xXml.Length % 32];
                        Array.Copy(xXml, msg, xXml.Length);
                        cs.Write(xXml, 0, xXml.Length);
                    }
                    //cs.Dispose();
                    xBuff = decode2(ms.ToArray());
                }
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                //Padding is invalid and cannot be removed.
                Console.WriteLine("===== CryptographicException =====");

                using (var ms = new MemoryStream())
                {
                    //cs 不自动释放，用于避免“Padding is invalid and cannot be removed”的错误    —— 2019.07.27 Jeffrey
                    var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write);
                    {
                        //cs.Read(decryptBytes, 0, decryptBytes.Length);
                        //cs.Close();
                        //ms.Close();

                        //cs.FlushFinalBlock();//用于解决第二次获取小程序Session解密出错的情况

                        byte[] xXml = Convert.FromBase64String(Input);
                        byte[] msg = new byte[xXml.Length + 32 - xXml.Length % 32];
                        Array.Copy(xXml, msg, xXml.Length);
                        cs.Write(xXml, 0, xXml.Length);
                    }
                    //cs.Dispose();
                    xBuff = decode2(ms.ToArray());
                }
            }
            return xBuff;
        }

        private static byte[] decode2(byte[] decrypted)
        {
            int pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
            {
                pad = 0;
            }
            byte[] res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }


        #endregion

        /// <summary>
        /// 解密所有消息的基础方法
        /// </summary>
        /// <param name="sessionKey">储存在 SessionBag 中的当前用户 会话 SessionKey</param>
        /// <param name="encryptedData">接口返回数据中的 encryptedData 参数</param>
        /// <param name="iv">接口返回数据中的 iv 参数，对称解密算法初始向量</param>
        /// <returns></returns>
        public static string DecodeEncryptedData(string sessionKey, string encryptedData, string iv)
        {
            //var aesCipher = Convert.FromBase64String(encryptedData);
            var aesKey = Convert.FromBase64String(sessionKey);
            var aesIV = Convert.FromBase64String(iv);

            var result = AES_Decrypt(encryptedData, aesIV, aesKey);
            var resultStr = Encoding.UTF8.GetString(result);
            return resultStr;
        }

        /// <summary>
        /// 解密消息（通过SessionId获取）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <exception cref="WxOpenException">当SessionId或SessionKey无效时抛出异常</exception>
        /// <returns></returns>
        public static string DecodeEncryptedDataBySessionId(string sessionKey, string encryptedData, string iv)
        {
            var resultStr = DecodeEncryptedData(sessionKey, encryptedData, iv);
            return resultStr;
        }


        /// <summary>
        /// 检查解密消息水印
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="appId"></param>
        /// <returns>entity为null时也会返回false</returns>
        public static bool CheckWatermark(this DecodeEntityBase entity, string appId)
        {
            if (entity == null)
            {
                return false;
            }
            return entity.watermark.appid == appId;
        }

        #region 解密实例信息

        /// <summary>
        /// 解密到实例信息
        /// </summary>
        /// <typeparam name="T">DecodeEntityBase</typeparam>
        /// <param name="sessionKey"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static T DecodeEncryptedDataToEntity<T>(string sessionKey, string encryptedData, string iv)
        {
            var jsonStr = DecodeEncryptedDataBySessionId(sessionKey, encryptedData, iv);

            //Console.WriteLine("===== jsonStr =====");
            //Console.WriteLine(jsonStr);
            //Console.WriteLine();

            var entity = JsonConvert.DeserializeObject<T>(jsonStr);
            return entity;
        }
        /// <summary>
        /// 解密到实例信息
        /// </summary>
        /// <typeparam name="T">DecodeEntityBase</typeparam>
        /// <param name="sessionKey"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static T DecodeEncryptedDataToEntityEasy<T>(string sessionKey, string encryptedData, string iv)
        {
            var jsonStr = DecodeEncryptedData(sessionKey, encryptedData, iv);
            var entity = JsonConvert.DeserializeObject<T>(jsonStr);
            return entity;
        }

        /// <summary>
        /// 解密UserInfo消息（通过SessionId获取）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <exception cref="WxOpenException">当SessionId或SessionKey无效时抛出异常</exception>
        /// <returns></returns>
        public static DecodedUserInfo DecodeUserInfoBySessionId(string sessionKey, string encryptedData, string iv)
        {
            return DecodeEncryptedDataToEntity<DecodedUserInfo>(sessionKey, encryptedData, iv);
        }

        /// <summary>
        /// 解密手机号
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static DecodedPhoneNumber DecryptPhoneNumber(string sessionKey, string encryptedData, string iv)
        {
            return DecodeEncryptedDataToEntity<DecodedPhoneNumber>(sessionKey, encryptedData, iv);
        }
        /// <summary>
        /// 解密手机号(根据sessionKey解密)
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static DecodedPhoneNumber DecryptPhoneNumberBySessionKey(string sessionKey, string encryptedData, string iv)
        {
            //var resultStr = DecodeEncryptedData(sessionKey, encryptedData, iv);

            //var entity = SerializerHelper.GetObject<DecodedPhoneNumber>(resultStr);
            //return entity;

            return DecodeEncryptedDataToEntityEasy<DecodedPhoneNumber>(sessionKey, encryptedData, iv);
        }

        /// <summary>
        /// 解密微信小程序运动步数
        /// 2019-04-02
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static DecodedRunData DecryptRunData(string sessionId, string encryptedData, string iv)
        {
            return DecodeEncryptedDataToEntity<DecodedRunData>(sessionId, encryptedData, iv);
        }


        #endregion

        #endregion
    }

}
