
// 提示框
function modelShow(
    title = '提示',
    content = '确认执行此操作吗?',
    callback = () => { },
    showCancel = true,
    cancelText = '取消',
    confirmText = '确定'
) {
    uni.showModal({
        title: title,
        content: content,
        showCancel: showCancel,
        cancelText: cancelText,
        confirmText: confirmText,
        cancelText: cancelText,
        success: function (res) {
            if (res.confirm) {
                // 用户点击确定操作
                setTimeout(() => {
                    callback()
                }, 500)
            } else if (res.cancel) {
                // 用户取消操作
            }
        }
    })
};


//时间差转倒计时数据
function timeToDateObj(micro_second) {
    var time = {}
    // 总秒数
    var second = Math.floor(micro_second)
    // 天数
    time.day = Math.floor(second / 3600 / 24)
    // 小时
    time.hour = Math.floor((second / 3600) % 24)
    // 分钟
    time.minute = Math.floor((second / 60) % 60)
    // 秒
    time.second = Math.floor(second % 60)
    return time
};

//货币格式化
function formatMoney(number, places, symbol, thousand, decimal) {
    // console.log(number)
    // console.log(places)
    number = number || 0
    places = !isNaN((places = Math.abs(places))) ? places : 2
    symbol = symbol !== undefined ? symbol : '￥'
    thousand = thousand || ','
    decimal = decimal || '.'
    var negative = number < 0 ? '-' : '',
        i = parseInt((number = Math.abs(+number || 0).toFixed(places)), 10) + '',
        j = (j = i.length) > 3 ? j % 3 : 0
    return (
        symbol +
        negative +
        (j ? i.substr(0, j) + thousand : '') +
        i.substr(j).replace(/(\d{3})(?=\d)/g, '$1' + thousand) +
        (places ?
            decimal +
            Math.abs(number - i)
                .toFixed(places)
                .slice(2) :
            '')
    )
}

/**
 * 获取url参数
 *
 * @param {*} name
 * @param {*} [url=window.location.serach]
 * @returns
 */
function getQueryString(name, url) {
    var url = url || window.location.href
    var reg = new RegExp('(^|&|/?)' + name + '=([^&|/?]*)(&|/?|$)', 'i')
    var r = url.substr(1).match(reg)
    if (r != null) {
        return r[2]
    }
    return null
}

/**
 *
 *  判断是否在微信浏览器 true是
 */
function isWeiXinBrowser() {
    // #ifdef H5
    // window.navigator.userAgent属性包含了浏览器类型、版本、操作系统类型、浏览器引擎类型等信息，这个属性可以用来判断浏览器类型
    let ua = window.navigator.userAgent.toLowerCase()
    // 通过正则表达式匹配ua中是否含有MicroMessenger字符串
    if (ua.match(/MicroMessenger/i) == 'micromessenger') {
        return true
    } else {
        return false
    }
    // #endif
    return false
}

/**
 * 金额相加
 * @param {Object} value1
 * @param {Object} value2
 */
function moneySum(value1, value2) {
    return (parseFloat(value1) + parseFloat(value2)).toFixed(2);
}
/**
 * 金额相减
 * @param {Object} value1
 * @param {Object} value2
 */
function moneySub(value1, value2) {
    let res = (parseFloat(value1) - parseFloat(value2)).toFixed(2);
    return res > 0 ? res : 0;
}


//设置手机通知栏字体颜色
function setBarColor(black = false) {
    if (black) {
        uni.setNavigationBarColor({
            frontColor: '#000000',
            backgroundColor: '#FAFAFA'
        });
    } else {
        uni.setNavigationBarColor({
            frontColor: '#ffffff',
            backgroundColor: '#FAFAFA'
        });
    }
}


export {
    formatMoney,
    modelShow,
    isWeiXinBrowser,
    getQueryString,
    timeToDateObj,
    moneySum,
    moneySub,
    setBarColor
}
