/**
 *  全局配置文件
 *  @version 1.0.0
 */

//接口请求地址，如果需要不部署接口端的情况下测试uni-app，可以直接替换为官方测试接口：https://api.demo.coreshop.cn
export const apiBaseUrl = 'https://api.demo.coreshop.cn';
//项目静态资源请求地址，如果使用官方的静态文件地址可以直接替换为：https://files.cdn.coreshop.cn
export const apiFilesUrl = 'https://files.cdn.coreshop.cn';

// #ifdef H5
export const baseUrl = process.env.NODE_ENV === 'development' ? window.location.origin + '/' : apiBaseUrl
// #endif

export const paymentType = {
    //支付单类型
    order: 1, //订单
    recharge: 2, //充值
    formPay: 3, //表单订单
    formOrder: 4, //表单付款码
    serviceOrder: 5, //服务订单
};

//nav页面导航类型
export const navLinkType = {
    urlLink: 1, //"URL链接"
    shop: 2,// "商品"
    article: 3,// "文章"
    articleCategory: 4,// "文章分类",
    intelligentForms: 5// "智能表单"
};
