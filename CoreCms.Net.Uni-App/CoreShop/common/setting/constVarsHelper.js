/**
 *  CoreShop全局配置文件
 *  @version 1.0.0
 */
//接口请求地址
export const apiBaseUrl = 'https://api.demo.coreshop.com.cn';
//项目静态资源请求地址，如果使用官方的静态文件地址可以直接替换为：https://files.coreshop.corecms.net
export const apiFilesUrl = 'https://files.coreshop.corecms.net';

export const h5Url = apiBaseUrl + "wap/"; //H5端网站地址,

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

// #ifdef MP-TOUTIAO
export const ttPlatform = 'toutiao'; //toutiao=今日头条小程序, douyin=抖音小程序, pipixia=皮皮虾小程序, huoshan=火山小视频小程序
// #endif