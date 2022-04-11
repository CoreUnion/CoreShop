/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-03-28 23:22:14
 *        Description: 暂无
 ***********************************************************************/

namespace CoreCms.Net.Configuration
{
    /// <summary>
    /// 数据接口错误编码返回
    /// 11000 用户
    /// 12000 商品
    /// 13000 订单
    /// 14000 api
    /// 15000 促销&优惠券
    /// </summary>
    public class GlobalErrorCodeVars
    {

        public const string Code10000 = "未定义的错误信息";
        public const string Code10002 = "没有找到此记录";
        public const string Code10003 = "参数不正确";
        public const string Code10004 = "保存失败";
        public const string Code10005 = "用户信息没有修改";
        public const string Code10006 = "图片超过限定张数";
        public const string Code10007 = "删除失败";
        public const string Code10008 = "没有此配置参数";
        public const string Code10009 = "没有此消息编码，请确认";
        public const string Code10010 = "您没有该操作权限";
        public const string Code10011 = "请选择商户";
        public const string Code10012 = "验证码错误";
        public const string Code10013 = "请输入验证码";
        public const string Code10014 = "没有此推荐人";
        public const string Code10015 = "商户公众号未配置";
        public const string Code10016 = "编辑失败";
        public const string Code10018 = "操作失败";
        public const string Code10019 = "新增失败";
        public const string Code10020 = "出了点小状况，请刷新重试~";
        public const string Code10021 = "更新失败";
        public const string Code10022 = "非法操作";
        public const string Code10023 = "删除失败";
        public const string Code10024 = "修改失败";
        public const string Code10025 = "获取失败";
        public const string Code10026 = "创建失败";
        public const string Code10027 = "查询失败";
        public const string Code10028 = "有非法查询字段";
        public const string Code10029 = "查询字段错误";
        public const string Code10030 = "字段校检通过";
        public const string Code10031 = "排序错误";
        public const string Code10032 = "排序校检通过";
        public const string Code10033 = "无参数相关信息";
        public const string Code10034 = "删除消息失败";
        public const string Code10035 = "上传失败";
        public const string Code10036 = "没有符合的数据";
        public const string Code10037 = "失败";
        public const string Code10038 = "添加失败";
        public const string Code10039 = "导出执行失败";
        public const string Code10040 = "导出执行成功";
        public const string Code10041 = "导入执行失败";
        public const string Code10042 = "图片保存失败";
        public const string Code10043 = "请先上传图片";
        public const string Code10045 = "请输入任务名称,防止混淆";
        public const string Code10046 = "导出任务加入成功，请到任务列表中下载文件";
        public const string Code10047 = "导入任务加入成功，请到任务列表中查看进度";
        public const string Code10048 = "请求地址出错";
        public const string Code10049 = "callback参数不合法";


        public const string Code10050 = "此支付方式未启用";
        public const string Code10051 = "缺少参数，请确认";
        public const string Code10052 = "此支付方式未启用，或不是一个有效的支付方式";
        public const string Code10053 = "已开启过此支付方式，不需要重复开启";
        public const string Code10054 = "没有此支付类型,请确认";
        public const string Code10055 = "请选择支付方式";
        public const string Code10056 = "请输入支付单号";
        public const string Code10057 = "没有此支付方式";
        public const string Code10058 = "没有此支付方式，或此支付方式未启用";
        public const string Code10059 = "支付单金额为0，直接支付成功";
        public const string Code10060 = "没有找到此支付单";
        public const string Code10061 = "不需要获取openid";
        public const string Code10062 = "请用户先进行微信登陆或绑定";
        public const string Code10063 = "请用户先进行支付宝登陆或绑定";
        public const string Code10064 = "请先选择标签";
        public const string Code10065 = "发送失败";
        public const string Code10066 = "msg里的值就是跳转的url";  //微信公众号静默登陆
        public const string Code10067 = "公众号支付必须传url参数";
        public const string Code10068 = "code必传";
        public const string Code10069 = "后台小程序配置的APPID和APPSECRET错误，无法生成海报";
        public const string Code10070 = "iv参数缺失";
        public const string Code10071 = "加密参数缺失";
        public const string Code10072 = "地址库不存在，请重新生成";
        public const string Code10073 = "未查询到授权信息";
        public const string Code10074 = "清除缓存成功";
        public const string Code10075 = "后台操作日志默认不让删除";
        public const string Code10076 = "时间段格式不正确";
        public const string Code10077 = "没有此时间维度";
        public const string Code10078 = "开始时间必须小于结束时间";
        public const string Code10079 = "没有此时间粒度";
        public const string Code10080 = "无此业务类型";
        public const string Code10081 = "设置失败";
        public const string Code10082 = "已超时或重复提交，请重试或刷新页面";
        public const string Code10083 = "无可导出数据";
        public const string Code10084 = "平台名称不能为空";
        public const string Code10085 = "联系方式号码格式错误";
        public const string Code10099 = "暂无消息";
        public const string Code10100 = "没有此消息编码";


        //文章等
        public const string Code10800 = "文章分类";
        public const string Code10801 = "文章不存在或已删除";
        public const string Code10802 = "无法选择自己和自己的子级为父级";
        //广告位
        public const string Code10820 = "该广告位模板已经添加";
        public const string Code10821 = "该广告位下有广告，删除失败";

        public const string Code10840 = "该地区下存在关联地区，无法删除";

        public const string Code11001 = "用户未登录";
        public const string Code11002 = "此微信用户未登录或当前账号未绑定微信账号";
        public const string Code11003 = "请选择头像";
        public const string Code11004 = "没有找到此用户";
        public const string Code11005 = "此用户没有绑定手机号码，所以发送短信失败";
        public const string Code11006 = "此用户以停用，请联系总管理员";
        public const string Code11007 = "余额不足";
        public const string Code11008 = "请输入用户名，长度6-20位";
        public const string Code11009 = "请输入密码，长度为6-16位";
        public const string Code11010 = "没有找到此管理员";
        public const string Code11011 = "用户名重复";
        public const string Code11012 = "请输入旧密码";
        public const string Code11013 = "请输入新密码";
        public const string Code11014 = "请输入确认密码";
        public const string Code11015 = "用户余额不足";
        public const string Code11016 = "没有找到此提现银行卡";
        public const string Code11017 = "请输入银行卡号";
        public const string Code11018 = "请输入提现金额";
        public const string Code11019 = "已注册过，请直接登陆";
        public const string Code11020 = "请输入正确的充值金额";
        public const string Code11021 = "请检查银行卡号是否有误";
        public const string Code11022 = "账号已停用";
        public const string Code11023 = "超级管理员，就不要编辑了吧？";
        public const string Code11024 = "超级管理员，就不要删除了把？";
        public const string Code11025 = "两次密码输入不一致";
        public const string Code11026 = "密码过期了";
        public const string Code11027 = "请选择出生日期";
        public const string Code11028 = "请输入昵称";
        public const string Code11029 = "请选择您的性别";

        public const string Code11030 = "没有此用户等级";
        public const string Code11031 = "请输入手机号码或者密码";
        public const string Code11032 = "没有找到此账号";
        public const string Code11033 = "密码错误，请重试";
        public const string Code11044 = "新密码和旧密码一致";
        public const string Code11045 = "旧密码不正确";
        public const string Code11046 = "短信验证码错误";
        public const string Code11047 = "此账号已经注册过，请直接登陆";
        public const string Code11048 = "填写邀请码失败";
        public const string Code11049 = "自己不能邀请自己";
        public const string Code11050 = "没有此收货地址信息";
        public const string Code11051 = "请输入手机号码";
        public const string Code11052 = "邀请码不存在";
        public const string Code11053 = "已有上级邀请，不能绑定其他的邀请";
        public const string Code11054 = "不能关联这个邀请人，因为他是你的下级或者下下级";
        public const string Code11055 = "请选择自提门店";
        public const string Code11056 = "用户暂无收货地址";
        public const string Code11057 = "请输入正确的手机号";
        public const string Code11058 = "手机号已经存在，请更换手机号重新添加";
        public const string Code11059 = "用户名已经存在，请确认";
        public const string Code11060 = "该卡片已经添加";
        public const string Code11061 = "该银行卡不存在";
        public const string Code11062 = "该地址不存在";
        public const string Code11063 = "提现最低不能少于{str1}元";
        public const string Code11064 = "每日提现不能超过{str1}元";
        public const string Code11065 = "提现失败";
        public const string Code11066 = "没有此记录或不是待审核状态";



        public const string Code11070 = "请输入角色名称";
        public const string Code11071 = "没有此角色信息";
        public const string Code11072 = "没有选择权限信息";

        public const string Code11080 = "请输入管理员的手机号码";
        public const string Code11081 = "没有找到此用户";
        public const string Code11082 = "目前一个账号只能绑定一个店铺，此手机号码已注册过店铺，如果是未审核通过的店铺可以联系平台删除对应的店铺，然后再次添加此管理员";
        public const string Code11083 = "手机号码和用户id两者最少写一个";
        public const string Code11084 = "此账号已经是店铺管理员了，请勿重新设置";
        public const string Code11085 = "此账号是超级管理员，不需要添加";
        public const string Code11086 = "您不是管理员，请先成为商户管理员或者创建自己的店铺";
        public const string Code11087 = "用户绑定了多个商户平台，系统不知道你想登陆哪一个，需要用户去选择";      //严格意义上来说这个不是错误信息
        public const string Code11088 = "没有找到控制器，请联系平台管理员";
        public const string Code11089 = "没有找到此方法，请联系平台管理员";
        public const string Code11090 = "没有找到此方法所对应的关联方法，请联系平台管理员";
        public const string Code11091 = "请先清空下级节点";
        public const string Code11092 = "核心参数不能为空";
        public const string Code11093 = "父节点是模块，当前类型就必须是控制器";
        public const string Code11094 = "父节点是控制器，当前类型就必须是方法";
        public const string Code11095 = "父节点是根节点，当前类型就必须是模块";
        public const string Code11096 = "当前节点已经存在，请勿重复提交";
        public const string Code11097 = "设置的父节点可能会陷入死循环";
        public const string Code11098 = "设置的父菜单可能陷入死循环";
        public const string Code11099 = "如果是控制器节点，菜单节点必须和父节点保持一致";

        public const string Code11100 = "购物车商品不能为空，或不是有效的商品";
        public const string Code11101 = "父节点可能会陷入死循环";



        public const string Code11500 = "店铺不存在，请确认";
        public const string Code11501 = "店铺现在处于非正常状态";       //未审核通过或者是到期了
        public const string Code11502 = "这个手机号没有对应的店铺用户";
        public const string Code11503 = "已经存在这个店员，无需重复添加";
        public const string Code11504 = "不是店员";


        //积分
        public const string Code11600 = "积分不足，无法使用积分";
        public const string Code11601 = "积分超过订单可使用的积分数量";
        public const string Code11602 = "今天已经签到，无需重复签到";
        public const string Code11603 = "今天还没有签到";
        //商品
        //分类
        public const string Code12001 = "获取顶级分类失败";
        public const string Code12002 = "商品数据保存失败";
        public const string Code12003 = "货品数据保存失败";
        public const string Code12004 = "请选择默认货品";
        public const string Code12005 = "会员价保存失败";
        public const string Code12006 = "商品图片保存失败";
        public const string Code12007 = "扩展分类保存失败";
        public const string Code12008 = "总库存更新失败";
        public const string Code12009 = "商品ID不能为空";
        public const string Code12010 = "存在下级分类，不允许删除";
        public const string Code12011 = "属性值不能为空";
        public const string Code12012 = "属性值删除失败";
        public const string Code12013 = "属性值保存失败";
        public const string Code12014 = "商品： {str1} 已在未结束的活动{str2}中，请勿重复添加！";
        // public const string Code12015="上架";
        // public const string Code12016="下架";
        public const string Code12017 = "没有找到此商品分类";


        //品牌
        public const string Code12101 = "";
        //类型
        public const string Code12301 = "";
        //属性
        public const string Code12401 = "";
        //货品
        public const string Code12501 = "货品不存在";

        //商品
        public const string Code12700 = "商品不存在";
        public const string Code12701 = "无此规格信息";
        public const string Code12702 = "库存不足";
        public const string Code12703 = "库存更新失败";
        public const string Code12704 = "商品删除失败";
        public const string Code12705 = "获取促销商品失败";
        public const string Code12706 = "商品已下架";

        //订单
        public const string Code13001 = "请选择收货地址";
        public const string Code13002 = "取消订单成功";
        public const string Code13003 = "取消订单失败";
        public const string Code13004 = "暂未设置配送方式";
        public const string Code13005 = "下单成功";
        public const string Code13006 = "下单失败";
        public const string Code13007 = "订单支付失败";
        public const string Code13008 = "订单支付失败，该订单已支付";
        public const string Code13009 = "订单不存在";
        public const string Code13010 = "备注失败";

        public const string Code13100 = "请输入订单编号";
        public const string Code13101 = "没有找到此订单信息,或者您没有权限查看此信息";
        public const string Code13102 = "已有售后,请联系客服";
        public const string Code13103 = "订单类型不能为空";

        //订单售后
        public const string Code13200 = "订单不是可售后状态";
        public const string Code13201 = "退货的数量超过可退的数量";
        public const string Code13202 = "退货商品不正确，请确认";
        public const string Code13203 = "订单状态不可退款";
        public const string Code13204 = "订单状态不可退货";
        public const string Code13205 = "请选择退货商品";
        public const string Code13206 = "总退款金额超过已支付金额";
        public const string Code13207 = "售后单不是待审核状态，或者没有找到此售后单";
        public const string Code13208 = "退款单金额为0，不需要退款";
        public const string Code13209 = "退货数量为空，不需要生成退货单";
        public const string Code13210 = "退款单已退或没权限进行操作";
        public const string Code13211 = "退货单已退或没权限进行操作";
        public const string Code13212 = "请输入退货单编号";
        public const string Code13213 = "请选择物流公司";
        public const string Code13214 = "请输入物流编码";
        public const string Code13215 = "请输入退款单号";
        public const string Code13216 = "请输入退款金额";
        public const string Code13217 = "请输入售后单号";
        public const string Code13218 = "没有找到此售后单";
        public const string Code13219 = "没有找到此退款单或此退款单状态不是未待退款状态";
        public const string Code13220 = "请输入退货单号";
        public const string Code13221 = "没有找到此退货单";
        public const string Code13222 = "请输入售后单号";
        public const string Code13223 = "没有找到此售后单号";
        public const string Code13224 = "没有找到此退款单或此退款单状态不是退款失败状态";
        public const string Code13225 = "缺少物流查询参数";
        public const string Code13226 = "x轴最多1000个节点，请减少时间范围，或者修改粒度";
        public const string Code13227 = "还没发货呢，怎么能收到货呢？";
        public const string Code13228 = "请选择审核状态";
        public const string Code13229 = "快递公司编码不能为空";
        public const string Code13230 = "确认收货失败";
        public const string Code13231 = "砍价活动订单更新失败";
        public const string Code13232 = "物流公司不存在";


        //订单发货
        public const string Code13300 = "订单已完成或取消不能发货";
        public const string Code13301 = "订单未付款不能发货";
        public const string Code13302 = "订单已发货不能再发货";
        public const string Code13303 = "订单中不存在要发货的商品";
        public const string Code13304 = "发货数量大于订单中商品的数量";
        public const string Code13305 = "发货单生成出现未知错误";
        public const string Code13306 = "发货失败，该货品已不存在";
        public const string Code13307 = "发货失败，商品数量不足";
        public const string Code13308 = "发货明细里包含订单之外的商品";

        public const string Code13309 = "收货地址信息不全";
        public const string Code13310 = "{str1}发超了";
        public const string Code13311 = "请至少发生一件商品！";
        public const string Code13312 = "提货单不存在";
        public const string Code13313 = "未提货的提货单不能删除";
        public const string Code13314 = "你无权删除该提货单";
        public const string Code13315 = "没有可提货的订单";
        public const string Code13316 = "请选择配送地区";
        public const string Code13317 = "请选择订单";
        public const string Code13318 = "门店自提订单和普通订单不能混合发货。";
        public const string Code13319 = "订单号：{str1}非正常状态不能发货。<br />";
        public const string Code13320 = "订单号：{str1} 未支付不能发货。<br />";
        public const string Code13321 = "订单号：{str1} 不是待发货和部分发货状态不能发货。<br />";
        public const string Code13322 = "订单号：{str1}有未审核的售后单，请先处理掉才能发货。";
        public const string Code13323 = "多个用户订单，";
        public const string Code13324 = "多个收货地址，";
        public const string Code13325 = "请注意！合并发货订单中存在：{str1}。确定发货吗？";
        public const string Code13326 = "{str1}的{str2}发超了";


        //评价
        public const string Code13400 = "评价缺少商品信息";
        public const string Code13401 = "评价缺少订单号";
        public const string Code13402 = "评价缺少商家店铺评价信息";
        public const string Code13403 = "缺少商品ID参数";
        public const string Code13404 = "评价失败：{str1}";
        public const string Code13405 = "订单状态存在问题，不能评价";

        //支付
        public const string Code13500 = "没有找到此未支付的支付单号";
        public const string Code13501 = "订单号：{str1}没有找到,或不是未支付状态";
        public const string Code13502 = "请输入正确的充值金额";
        public const string Code13503 = "表单：{str1}没有找到,或不是未支付状态";
        public const string Code13504 = "没有找到此支付记录";

        public const string Code13550 = "没有找到支付成功的支付单号";
        public const string Code13551 = "退款单退款方式和支付方式不一样，原路退还失败";
        public const string Code13552 = "";

        //售后
        public const string Code13600 = "aftersale_level值类型不对";
        public const string Code13601 = "未发货商品-{str1}{str2}最多能退{str3}个";
        public const string Code13602 = "已发货商品-{str1}{str2}最多能退{str3}个";


        public const string Code14001 = "";
        public const string Code14002 = "method参数结构错误";
        public const string Code14003 = "method参数1不存在";
        public const string Code14004 = "method参数2不存在";
        public const string Code14006 = "请先登录";
        public const string Code14007 = "用户身份过期请重新登录";
        public const string Code14008 = "操作失败，请重试1";
        public const string Code14009 = "操作失败，请重试2";
        public const string Code14011 = "请输入货品id";
        public const string Code14012 = "请输入货品数量";
        public const string Code14013 = "移除购物车成功";
        public const string Code14014 = "移除购物车失败";
        public const string Code14015 = "生成token失败";
        public const string Code14016 = "不是有效的token";


        //促销，优惠券
        public const string Code15001 = "请输入促销名称";
        public const string Code15002 = "请输入起止时间";
        public const string Code15003 = "请选择促销条件";
        public const string Code15004 = "没有找到此促销条件";
        public const string Code15005 = "没有找到此促销结果";
        public const string Code15006 = "请输入促销ID参数";
        public const string Code15007 = "该优惠券不存在或状态不可领取";
        public const string Code15008 = "你已领取过了,勿重复领取";
        public const string Code15009 = "优惠券号码不存在";
        public const string Code15010 = "优惠券还没有到开始时间";
        public const string Code15011 = "优惠券已经过期";
        public const string Code15012 = "优惠券禁用了，请联系客服";
        public const string Code15013 = "优惠券已经使用过了";
        public const string Code15014 = "优惠券不符合使用规则";
        public const string Code15015 = "同一类优惠券，只能使用一张";
        public const string Code15016 = "团购或秒杀只能应用一种促销结果";
        public const string Code15017 = "同一个商品只能同时存在一个团购秒杀";
        public const string Code15018 = "已超出领取限额";
        public const string Code15019 = "优惠券信息获取失败";
        public const string Code15020 = "优惠券号码不存在";
        public const string Code15021 = "领取失败";
        public const string Code15022 = "核销使用优惠券失败";
        public const string Code15023 = "一次最多可以生成5000张";
        public const string Code15024 = "一张都没生成";
        public const string Code15025 = "该优惠券已被使用";
        public const string Code15026 = "该优惠券已被其他人领取";
        public const string Code15027 = "绑定失败";
        public const string Code15028 = "优惠券超过最大领取数量";

        //拼团
        public const string Code15600 = "活动已结束";
        public const string Code15601 = "还没有到时间";
        public const string Code15602 = "已经结束了";
        public const string Code15603 = "没有找到此拼团商品";
        public const string Code15604 = "请传拼团id";
        public const string Code15605 = "请传商品id";
        public const string Code15606 = "请传入订单id或者teamId";
        public const string Code15607 = "没有此拼团记录,或不是已经结束";
        public const string Code15608 = "参加拼团的商品和下单商品不一致";
        public const string Code15609 = "没有找到拼团发起人";
        public const string Code15610 = "该商品已超过当前活动最大购买量";
        public const string Code15611 = "您已超过该活动最大购买量";
        public const string Code15612 = "货品折扣后价格已经小于0元";
        public const string Code15613 = "您不能参加自己的开团";

        //微信消息
        public const string Code16001 = "请输入标题";
        public const string Code16002 = "请先填写内容";



        public const string Code17001 = "商品数据不存在";
        public const string Code17002 = "收藏成功";
        public const string Code17003 = "取消收藏成功!";

        //砍价
        public const string Code17601 = "砍价活动暂未开始";
        public const string Code17602 = "砍价活动已结束";
        public const string Code17603 = "没有找到此砍价商品";
        public const string Code17604 = "请传砍价id";
        // public const string Code17605="请传商品id";
        // public const string Code17610="该商品已超过当前活动最大购买量";
        public const string Code17611 = "您已超过该活动最大购买量";
        public const string Code17612 = "砍价活动不存在";
        public const string Code17613 = "您参与的活动已下单，请勿重复下单";
        public const string Code17614 = "您参与的活动已结束";
        public const string Code17615 = "您参与的活动已取消";
        public const string Code17616 = "该砍价已成功，请先支付后再继续参与活动";
        public const string Code17618 = "发起砍价活动失败";

        public const string Code17620 = "请输入活动名称";
        public const string Code17621 = "请输入活动简介";
        public const string Code17622 = "请选择单规格商品";
        public const string Code17623 = "砍价活动状态错误";
        public const string Code17624 = "请选择活动时间";
        public const string Code17625 = "请输入起始金额";
        public const string Code17626 = "请输入成交金额";
        public const string Code17627 = "请输入最大价";
        public const string Code17628 = "请输入最小价";
        public const string Code17629 = "请输入有效时长";
        public const string Code17630 = "请输入砍价次数";
        public const string Code17631 = "砍价总次数必须大于0";
        public const string Code17632 = "商品：{str1} 参加过砍价了";
        public const string Code17633 = "砍价记录不存在，请先参加活动";
        public const string Code17634 = "此商品只能砍价{str1}次";
        public const string Code17635 = "此商品已经砍到最底价了";
        public const string Code17636 = "您已超过该活动最大参加次数，看看别的活动吧~";
        public const string Code17637 = "您有正在进行中的砍价，请勿重复参加";
        public const string Code17638 = "活动数量已满，请看看其它活动吧";
        public const string Code17639 = "活动不存在";


        //表单
        public const string Code18001 = "表单不存在";
        public const string Code18002 = "表单已过期";
        public const string Code18003 = "您已达到最大提交次数，请忽继续提交。";
        public const string Code18004 = "格式错误，请重新输入";
        public const string Code18005 = "提交失败，请重试";
        public const string Code18006 = "请输入";
        public const string Code18007 = "表单明细提交失败，请重试";
        public const string Code18008 = "暂无表单";
        public const string Code18009 = "请先删除该表单下用户的提交记录";
        public const string Code18010 = "请先添加表单项";
        public const string Code18011 = "无此提交";
        public const string Code18012 = "此表单需要登录后操作";
        public const string Code18020 = "未提交任何数据";

        //通用 -21000
        public const string Code20000 = "请选择";
        public const string Code20001 = "请选择日期";
        public const string Code20002 = "请选择广告位";
        public const string Code20003 = "请选择广告商品";
        public const string Code20004 = "请选择广告文章";
        public const string Code20005 = "请选择文章分类";
        public const string Code20006 = "请选择更新时间段";
        public const string Code20007 = "请选择市";
        public const string Code20008 = "请选择县/区";
        public const string Code20009 = "请选择地区";
        public const string Code20010 = "请选择单规格商品";
        public const string Code20011 = "请选择智能表单";
        public const string Code20012 = "请选择待核销订单";
        public const string Code20013 = "请选择品牌";
        public const string Code20014 = "请选择分类";
        public const string Code20015 = "请选择类型";
        public const string Code20016 = "请选择属性";
        public const string Code20017 = "";
        public const string Code20018 = "";
        public const string Code20019 = "";
        public const string Code20020 = "";
        public const string Code20021 = "";
        public const string Code20022 = "";
        public const string Code20023 = "";
        public const string Code20024 = "";
        public const string Code20025 = "";
        public const string Code20026 = "";
        public const string Code20027 = "";
        public const string Code20028 = "";
        public const string Code20029 = "";
        public const string Code20030 = "";
        public const string Code20031 = "";
        public const string Code20032 = "";
        public const string Code20033 = "";
        public const string Code20034 = "";
        public const string Code20035 = "";
        public const string Code20036 = "";
        public const string Code20037 = "";
        public const string Code20038 = "";
        public const string Code20039 = "";
        public const string Code20040 = "";

        //会员管理 -21000

        //商品管理 -22000

        //订单管理 -23000

        //运营管理 -24000

        //促销管理 -25000

        //财务管理 -26000

        //控制面板 -27000


        //30000 前台

        //会员管理 -31000

        //商品管理 -32000

        //订单管理 -33000

        //运营管理 -34000

        //促销管理 -35000

        //财务管理 -36000

        //控制面板 -37000


    }
}
