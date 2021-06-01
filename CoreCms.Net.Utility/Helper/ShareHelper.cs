using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCms.Net.Utility.Helper
{
    public static class ShareHelper
    {

        /// <summary>
        /// 分享URL压缩（type-invite-id-teamId  type=场景类型  invite=邀请码  id=场景ID值  teamId=拼团ID）
        /// </summary>
        /// <param name="type">场景类型</param>
        /// <param name="invite">邀请码</param>
        /// <param name="id">场景ID值</param>
        /// <param name="teamId">拼团ID</param>
        /// <returns></returns>
        public static string share_parameter_encode(string type, string invite, string id, string teamId)
        {
            var newUrl = type + "-" + invite + "-" + id + "-" + teamId;
            return newUrl;
        }


        /// <summary>
        /// 分享URL解压缩
        /// </summary>
        /// <param name="url">已经压缩的url，多个-组成的url</param>
        /// <returns></returns>
        public static string share_parameter_decode(string url)
        {
            var urlArr = url.Split("-");
            var newUrl = "type=" + urlArr[0] + "&invite=" + urlArr[1] + "&id=" + urlArr[2] + "&teamId=" + urlArr[3];
            return newUrl;
        }

    }
}
