/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-03-14 16:30:32
 *        Description: 暂无
 ***********************************************************************/


namespace CoreCms.Net.Configuration
{
    /// <summary>
    /// HTTP 返回格式状态码
    /// </summary>
    public static class GlobalStatusCodes
    {
        public const int Status100Continue = 100;
        public const int Status101SwitchingProtocols = 101;
        public const int Status102Processing = 102;
        public const int Status200Ok = 200;

        // 等等等等

        public const int Status400BadRequest = 400;
        public const int Status401Unauthorized = 401;
        public const int Status402PaymentRequired = 402;
        public const int Status403Forbidden = 403;
        public const int Status404NotFound = 404;
        public const int Status405MethodNotAllowed = 405;
        public const int Status406NotAcceptable = 406;

        public const int Status414RequestUriTooLong = 414;
        public const int Status414UriTooLong = 414;
        public const int Status415UnsupportedMediaType = 415;
        public const int Status416RangeNotSatisfiable = 416;
        public const int Status416RequestedRangeNotSatisfiable = 416;
        public const int Status417ExpectationFailed = 417;
        public const int Status418ImATeapot = 418;
        public const int Status419AuthenticationTimeout = 419;
        public const int Status421MisdirectedRequest = 421;
        public const int Status422UnprocessableEntity = 422;
        public const int Status423Locked = 423;
        public const int Status424FailedDependency = 424;

        // 等等等等

        public const int Status500InternalServerError = 500;
        public const int Status501NotImplemented = 501;
        public const int Status502BadGateway = 502;
        public const int Status503ServiceUnavailable = 503;
        public const int Status504GatewayTimeout = 504;
        public const int Status505HttpVersionNotsupported = 505;
        public const int Status506VariantAlsoNegotiates = 506;
        public const int Status507InsufficientStorage = 507;
        public const int Status508LoopDetected = 508;
        public const int Status510NotExtended = 510;
        public const int Status511NetworkAuthenticationRequired = 511;

    }
}
