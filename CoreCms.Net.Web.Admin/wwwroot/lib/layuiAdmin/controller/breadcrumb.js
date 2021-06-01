/** 根据访问路径对应左侧导航效果 */
layui.define(['view'], function (exports) {
    var view = layui.view
        , $ = layui.$;
    layui.data.updateMainBreadcrumb = function () {
        var breadcrumbElem = $('#LAY_app_body .layadmin-tabsbody-item.layui-show .layui-breadcrumb');
        if (breadcrumbElem.length) {
            var _html = [];
            var navActive = $('#LAY-system-side-menu .layui-this');
            do {
                var texts = navActive.find('>a').first().text();
                texts = texts.replace(/\s+/g, "");/*去掉字符串中的空格，因为我的左侧菜单，有加入空格*/
                _html.unshift('<a><cite>' + texts + '</cite></a>');
                navActive = navActive.parents('.layui-nav-itemed').first();
            }
            while (navActive.length);
            breadcrumbElem.html('<a lay-href="">主页</a>' + _html.join(''));
            breadcrumbElem.attr('lay-separator', '>>');  /*修改 修饰符*/
            layui.element.render('breadcrumb', breadcrumbElem.attr('lay-filter'))
        }
    };

    //对外暴露的接口
    exports('breadcrumb', {});
});