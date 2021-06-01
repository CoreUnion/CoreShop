
layui.define(['laypage', 'form'], function (exports) {
    "use strict";

    var IconPicker =function () {
        this.v = '0.1.beta';
    }, _MOD = 'iconPicker',
        _this = this,
        $ = layui.jquery,
        page = layui.laypage,
        form = layui.form,
        BODY = 'body',
        TIPS = '请选择图标';

    /**
     * 渲染组件
     */
    IconPicker.prototype.render = function(options){
        var opts = options,
            // DOM选择器
            elem = opts.elem,
            // 数据类型：fontClass/unicode
            type = opts.type == null ? 'fontClass' : opts.type,
            // 是否分页：true/false
            page = opts.page,
            // 每页显示数量
            limit = 20,
            // 是否开启搜索：true/false
            search = opts.search == null ? true : opts.search,
            // 点击回调
            click = opts.click,
            // json数据
            data = {},
            // 唯一标识
            tmp = new Date().getTime(),
            // 是否使用的class数据
            isFontClass = opts.type === 'fontClass',
            TITLE = 'layui-select-title',
            TITLE_ID = 'layui-select-title-' + tmp,
            ICON_BODY = 'layui-iconpicker-' + tmp,
            PICKER_BODY = 'layui-iconpicker-body-' + tmp,
            LIST_BOX = 'layui-iconpicker-list-box',
            selected = 'layui-form-selected',
            unselect = 'layui-unselect';

        var a = {
            init: function () {
                data = common.getData[type]();

                a.hideElem().createSelect().createBody().toggleSelect();
                common.loadCss();
                return a;
            },
            /**
             * 隐藏elem
             */
            hideElem: function () {
                $(elem).hide();
                return a;
            },
            /**
             * 绘制select下拉选择框
             */
            createSelect: function () {
                var selectHtml = '<div class="layui-iconpicker layui-unselect layui-form-select" id="'+ ICON_BODY +'">' +
                    '<div class="'+ TITLE +'" id="'+ TITLE_ID +'">' +
                        '<div class="layui-iconpicker-item">'+
                            '<span class="layui-iconpicker-icon layui-unselect">' +
                                '<i class="layui-icon">&#xe617;</i>' +
                            '</span>'+
                            '<i class="layui-edge"></i>' +
                        '</div>'+
                    '</div>' +
                    '<div class="layui-anim layui-anim-upbit" style="">' +
                        '123' +
                    '</div>';
                $(elem).after(selectHtml);
                return a;
            },
            /**
             * 展开/折叠下拉框
             */
            toggleSelect: function () {
                var item = '#' + TITLE_ID + ' .layui-iconpicker-item';
                a.event('click', item, function () {
                    var $icon = $('#' + ICON_BODY);
                    if ($icon.hasClass(selected)) {
                        $icon.removeClass(selected).addClass(unselect);
                    } else {
                        $icon.addClass(selected).removeClass(unselect);
                    }
                });
                return a;
            },
            /**
             * 绘制主体部分
             */
            createBody: function () {
                // 获取数据
                var searchHtml = '';

                if (search) {
                    searchHtml = '<div class="layui-iconpicker-search">' +
                        '<input class="layui-input">' +
                        '<i class="layui-icon">&#xe615;</i>' +
                        '</div>';
                }

                // 组合dom
                var bodyHtml = '<div class="layui-iconpicker-body" id="'+ PICKER_BODY +'">' +
                    searchHtml +
                        '<div class="'+ LIST_BOX +'"></div> '+
                     '</div>';
                $('#' + ICON_BODY).find('.layui-anim').eq(0).html(bodyHtml);
                a.search().createList();
                return a;
            },
            /**
             * 绘制图标列表
             * @param text 模糊查询关键字
             * @returns {string}
             */
            createList: function (text) {
                var d = data,
                    l = d.length,
                    pageHtml = '',
                    listHtml = $('<div class="layui-iconpicker-list">')//'<div class="layui-iconpicker-list">';

                // 判断是否分页 TODO
                if (page){
                    pageHtml = '<div class="layui-iconpicker-page">' +
                        '' +
                        '</div> ';


                    listHtml += '</div> ';
                } else {
                    var c = 0;
                    for (var i = 0; i < l; i++) {
                        var obj = d[i];
                        // 判断是否模糊查询
                        if (text && obj.indexOf(text) === -1) {
                            c++;
                            continue;
                        }
                        var m = $('<div class="layui-iconpicker-icon-item" title="'+ obj +'">');
                        if (isFontClass){
                            m.append('<i class="layui-icon '+ obj +'"></i>');
                        } else {
                            m.append($('<i class="layui-icon">').html(obj.replace('amp;', '')));//'<i class="layui-icon">'+ obj +'</i>';
                        }
                        listHtml.append(m);
                    }

                    // 无数据
                    if (c === l) {
                        listHtml.append('<p class="layui-iconpicker-tips">无数据</p>');
                    }

                }

                $('#' + ICON_BODY).find('.layui-anim').find('.' + LIST_BOX).html('').append(listHtml).append(pageHtml);
                a.check();
                return a;
            },
            /**
             * 搜索
             */
            search: function () {
                var item = '#' + PICKER_BODY + ' .layui-iconpicker-search .layui-input';
                a.event('input propertychange', item, function (e) {
                    var elem = e.target,
                        t = $(elem).val();
                    a.createList(t);
                });
                return a;
            },
            /**
             * 点击选中图标
             */
            check: function () {
                var item = '#' + PICKER_BODY + ' .layui-iconpicker-icon-item';
                a.event('click', item, function (e) {
                    var el = $(e.currentTarget).find('.layui-icon'),
                        icon = '';
                    if (isFontClass) {
                        var clsArr = el.attr('class').split(/[\s\n]/),
                            cls = clsArr[1],
                            icon = cls;
                        $('#' + TITLE_ID).find('.layui-iconpicker-item .layui-icon').html('').attr('class', clsArr.join(' '));
                    } else {
                        var cls = el.html(),
                            icon = cls;
                        $('#' + TITLE_ID).find('.layui-iconpicker-item .layui-icon').html(icon);
                    }

                    $('#' + ICON_BODY).removeClass(selected).addClass(unselect);
                    $(elem).attr('value', icon);
                    // 回调
                    if (click) {
                        click({
                            icon: icon
                        });
                    }

                });
            },
            event: function (evt, el, fn) {
                $(BODY).on(evt, el, fn);
            }

        };

        var common = {
            /**
             * 加载样式表
             */
            loadCss: function () {
                var css = '.layui-iconpicker .layui-anim{display:none;position:absolute;left:0;top:42px;padding:5px 0;z-index:899;min-width:100%;border:1px solid #d2d2d2;max-height:300px;overflow-y:auto;background-color:#fff;border-radius:2px;box-shadow:0 2px 4px rgba(0,0,0,.12);box-sizing:border-box;}.layui-iconpicker-item{border:1px solid #e6e6e6;width:90px;height:38px;border-radius:4px;cursor:pointer;position:relative;}.layui-iconpicker-icon{border-right:1px solid #e6e6e6;-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;display:block;width:60px;height:100%;float:left;text-align:center;background:#fff;transition:all .3s;}.layui-iconpicker-icon i{line-height:38px;font-size:18px;}.layui-iconpicker-item > .layui-edge{left:70px;}.layui-iconpicker-item:hover{border-color:#D2D2D2!important;}.layui-iconpicker-item:hover .layui-iconpicker-icon{border-color:#D2D2D2!important;}.layui-iconpicker.layui-form-selected .layui-anim{display:block;}.layui-iconpicker-body{padding:6px;}.layui-iconpicker .layui-iconpicker-list{background-color:#fff;border:1px solid #ccc;border-radius:4px;}.layui-iconpicker .layui-iconpicker-icon-item{display:inline-block;width:21.1%;line-height:36px;text-align:center;cursor:pointer;vertical-align:top;height:36px;margin:4px;border:1px solid #ddd;border-radius:2px;transition:300ms;}.layui-iconpicker .layui-iconpicker-icon-item i.layui-icon{font-size:17px;}.layui-iconpicker .layui-iconpicker-icon-item:hover{background-color:#eee;border-color:#ccc;-webkit-box-shadow:0 0 2px #aaa,0 0 2px #fff inset;-moz-box-shadow:0 0 2px #aaa,0 0 2px #fff inset;box-shadow:0 0 2px #aaa,0 0 2px #fff inset;text-shadow:0 0 1px #fff;}.layui-iconpicker-search{position:relative;margin:0 0 6px 0;border:1px solid #e6e6e6;border-radius:2px;transition:300ms;}.layui-iconpicker-search:hover{border-color:#D2D2D2!important;}.layui-iconpicker-search .layui-input{cursor:text;display:inline-block;width:86%;border:none;padding-right:0;margin-top:1px;}.layui-iconpicker-search .layui-icon{position:absolute;top:11px;right:4%;}.layui-iconpicker-tips{text-align:center;padding:8px 0;cursor:not-allowed;}';
                $('head').append('<style rel="stylesheet">'+css+'</style>');
            },
            /**
             * 获取数据
             */
            getData: {
                fontClass: function () {
                    var arr = ["layui-icon-rate-half","layui-icon-rate","layui-icon-rate-solid","layui-icon-cellphone","layui-icon-vercode","layui-icon-login-wechat","layui-icon-login-qq","layui-icon-login-weibo","layui-icon-password","layui-icon-username","layui-icon-refresh-3","layui-icon-auz","layui-icon-spread-left","layui-icon-shrink-right","layui-icon-snowflake","layui-icon-tips","layui-icon-note","layui-icon-home","layui-icon-senior","layui-icon-refresh","layui-icon-refresh-1","layui-icon-flag","layui-icon-theme","layui-icon-notice","layui-icon-website","layui-icon-console","layui-icon-face-surprised","layui-icon-set","layui-icon-template-1","layui-icon-app","layui-icon-template","layui-icon-praise","layui-icon-tread","layui-icon-male","layui-icon-female","layui-icon-camera","layui-icon-camera-fill","layui-icon-more","layui-icon-more-vertical","layui-icon-rmb","layui-icon-dollar","layui-icon-diamond","layui-icon-fire","layui-icon-return","layui-icon-location","layui-icon-read","layui-icon-survey","layui-icon-face-smile","layui-icon-face-cry","layui-icon-cart-simple","layui-icon-cart","layui-icon-next","layui-icon-prev","layui-icon-upload-drag","layui-icon-upload","layui-icon-download-circle","layui-icon-component","layui-icon-file-b","layui-icon-user","layui-icon-find-fill","layui-icon-loading","layui-icon-loading-1","layui-icon-add-1","layui-icon-play","layui-icon-pause","layui-icon-headset","layui-icon-video","layui-icon-voice","layui-icon-speaker","layui-icon-fonts-del","layui-icon-fonts-code","layui-icon-fonts-html","layui-icon-fonts-strong","layui-icon-unlink","layui-icon-picture","layui-icon-link","layui-icon-face-smile-b","layui-icon-align-left","layui-icon-align-right","layui-icon-align-center","layui-icon-fonts-u","layui-icon-fonts-i","layui-icon-tabs","layui-icon-radio","layui-icon-circle","layui-icon-edit","layui-icon-share","layui-icon-delete","layui-icon-form","layui-icon-cellphone-fine","layui-icon-dialogue","layui-icon-fonts-clear","layui-icon-layer","layui-icon-date","layui-icon-water","layui-icon-code-circle","layui-icon-carousel","layui-icon-prev-circle","layui-icon-layouts","layui-icon-util","layui-icon-templeate-1","layui-icon-upload-circle","layui-icon-tree","layui-icon-table","layui-icon-chart","layui-icon-chart-screen","layui-icon-engine","layui-icon-triangle-d","layui-icon-triangle-r","layui-icon-file","layui-icon-set-sm","layui-icon-add-circle","layui-icon-404","layui-icon-about","layui-icon-up","layui-icon-down","layui-icon-left","layui-icon-right","layui-icon-circle-dot","layui-icon-search","layui-icon-set-fill","layui-icon-group","layui-icon-friends","layui-icon-reply-fill","layui-icon-menu-fill","layui-icon-log","layui-icon-picture-fine","layui-icon-face-smile-fine","layui-icon-list","layui-icon-release","layui-icon-ok","layui-icon-help","layui-icon-chat","layui-icon-top","layui-icon-star","layui-icon-star-fill","layui-icon-close-fill","layui-icon-close","layui-icon-ok-circle","layui-icon-add-circle-fine"];
                    return arr;
                },
                unicode: function () {
                    return ["&amp;#xe6c9;","&amp;#xe67b;","&amp;#xe67a;","&amp;#xe678;","&amp;#xe679;","&amp;#xe677;","&amp;#xe676;","&amp;#xe675;","&amp;#xe673;","&amp;#xe66f;","&amp;#xe9aa;","&amp;#xe672;","&amp;#xe66b;","&amp;#xe668;","&amp;#xe6b1;","&amp;#xe702;","&amp;#xe66e;","&amp;#xe68e;","&amp;#xe674;","&amp;#xe669;","&amp;#xe666;","&amp;#xe66c;","&amp;#xe66a;","&amp;#xe667;","&amp;#xe7ae;","&amp;#xe665;","&amp;#xe664;","&amp;#xe716;","&amp;#xe656;","&amp;#xe653;","&amp;#xe663;","&amp;#xe6c6;","&amp;#xe6c5;","&amp;#xe662;","&amp;#xe661;","&amp;#xe660;","&amp;#xe65d;","&amp;#xe65f;","&amp;#xe671;","&amp;#xe65e;","&amp;#xe659;","&amp;#xe735;","&amp;#xe756;","&amp;#xe65c;","&amp;#xe715;","&amp;#xe705;","&amp;#xe6b2;","&amp;#xe6af;","&amp;#xe69c;","&amp;#xe698;","&amp;#xe657;","&amp;#xe65b;","&amp;#xe65a;","&amp;#xe681;","&amp;#xe67c;","&amp;#xe601;","&amp;#xe857;","&amp;#xe655;","&amp;#xe770;","&amp;#xe670;","&amp;#xe63d;","&amp;#xe63e;","&amp;#xe654;","&amp;#xe652;","&amp;#xe651;","&amp;#xe6fc;","&amp;#xe6ed;","&amp;#xe688;","&amp;#xe645;","&amp;#xe64f;","&amp;#xe64e;","&amp;#xe64b;","&amp;#xe62b;","&amp;#xe64d;","&amp;#xe64a;","&amp;#xe64c;","&amp;#xe650;","&amp;#xe649;","&amp;#xe648;","&amp;#xe647;","&amp;#xe646;","&amp;#xe644;","&amp;#xe62a;","&amp;#xe643;","&amp;#xe63f;","&amp;#xe642;","&amp;#xe641;","&amp;#xe640;","&amp;#xe63c;","&amp;#xe63b;","&amp;#xe63a;","&amp;#xe639;","&amp;#xe638;","&amp;#xe637;","&amp;#xe636;","&amp;#xe635;","&amp;#xe634;","&amp;#xe633;","&amp;#xe632;","&amp;#xe631;","&amp;#xe630;","&amp;#xe62f;","&amp;#xe62e;","&amp;#xe62d;","&amp;#xe62c;","&amp;#xe629;","&amp;#xe628;","&amp;#xe625;","&amp;#xe623;","&amp;#xe621;","&amp;#xe620;","&amp;#xe61f;","&amp;#xe61c;","&amp;#xe60b;","&amp;#xe619;","&amp;#xe61a;","&amp;#xe603;","&amp;#xe602;","&amp;#xe617;","&amp;#xe615;","&amp;#xe614;","&amp;#xe613;","&amp;#xe612;","&amp;#xe611;","&amp;#xe60f;","&amp;#xe60e;","&amp;#xe60d;","&amp;#xe60c;","&amp;#xe60a;","&amp;#xe609;","&amp;#xe605;","&amp;#xe607;","&amp;#xe606;","&amp;#xe604;","&amp;#xe600;","&amp;#xe658;","&amp;#x1007;","&amp;#x1006;","&amp;#x1005;","&amp;#xe608;"];
                }
            }
        };

        a.init();
        return new IconPicker();
    };

    /**
     * 选中图标
     * @param filter lay-filter
     * @param iconName 图标名称，自动识别fontClass/unicode
     */
    IconPicker.prototype.checkIcon = function (filter, iconName){
        var p = $('*[lay-filter='+ filter +']').next().find('.layui-iconpicker-item .layui-icon'),
            c = iconName;

        if (c.indexOf('#xe') > 0){
            p.html(c);
        } else {
            p.html('').attr('class', 'layui-icon ' + c);
        }
    };

    var iconPicker = new IconPicker();
    exports(_MOD, iconPicker);
});