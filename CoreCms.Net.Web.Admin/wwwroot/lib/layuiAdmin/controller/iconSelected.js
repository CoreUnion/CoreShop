/**
 * @name iconSelected
 * @author HuangJunjie
 * @description layui 图标选择器
 * @version 2.0.0.20210819
 */

layui.define(["layer", "jquery"], function (exports) {
    var $ = layui.jquery;
    var prefix = "layui-ext-icon-selected";

    // Nice啊! 还有这种写法吗? 不错哟, 还真能用
    //layui.link(layui.cache.base + "iconSelected/css/theme.css");

    // 合并成类
    function IconSelected() {
        // Layui默认图标字典
        var layuiIcons = [
            {
                classList: "layui-icon layui-icon-heart-fill",
                name: "实心",
            },
            {
                classList: "layui-icon layui-icon-heart",
                name: "空心",
            },
            {
                classList: "layui-icon layui-icon-light",
                name: "亮度/晴",
            },
            {
                classList: "layui-icon layui-icon-time",
                name: "时间/历史",
            },
            {
                classList: "layui-icon layui-icon-bluetooth",
                name: "蓝牙",
            },
            {
                classList: "layui-icon layui-icon-at",
                name: "@艾特",
            },
            {
                classList: "layui-icon layui-icon-mute",
                name: "静音",
            },
            {
                classList: "layui-icon layui-icon-mike",
                name: "录音/麦克风",
            },
            {
                classList: "layui-icon layui-icon-key",
                name: "密钥/钥匙",
            },
            {
                classList: "layui-icon layui-icon-gift",
                name: "礼物/活动",
            },
            {
                classList: "layui-icon layui-icon-email",
                name: "邮箱",
            },
            {
                classList: "layui-icon layui-icon-rss",
                name: "RSS",
            },
            {
                classList: "layui-icon layui-icon-wifi",
                name: "WiFi",
            },
            {
                classList: "layui-icon layui-icon-logout",
                name: "退出/注销",
            },
            {
                classList: "layui-icon layui-icon-android",
                name: "Android 安卓",
            },
            {
                classList: "layui-icon layui-icon-ios",
                name: "Apple IOS 苹果",
            },
            {
                classList: "layui-icon layui-icon-windows",
                name: "Windows",
            },
            {
                classList: "layui-icon layui-icon-transfer",
                name: "穿梭框",
            },
            {
                classList: "layui-icon layui-icon-service",
                name: "客服",
            },
            {
                classList: "layui-icon layui-icon-subtraction",
                name: "减",
            },
            {
                classList: "layui-icon layui-icon-addition",
                name: "加",
            },
            {
                classList: "layui-icon layui-icon-slider",
                name: "滑块",
            },
            {
                classList: "layui-icon layui-icon-print",
                name: "打印",
            },
            {
                classList: "layui-icon layui-icon-export",
                name: "导出",
            },
            {
                classList: "layui-icon layui-icon-cols",
                name: "列",
            },
            {
                classList: "layui-icon layui-icon-screen-restore",
                name: "退出全屏",
            },
            {
                classList: "layui-icon layui-icon-screen-full",
                name: "全屏",
            },
            {
                classList: "layui-icon layui-icon-rate-half",
                name: "半星",
            },
            {
                classList: "layui-icon layui-icon-rate",
                name: "星星-空心",
            },
            {
                classList: "layui-icon layui-icon-rate-solid",
                name: "星星-实心",
            },
            {
                classList: "layui-icon layui-icon-cellphone",
                name: "手机",
            },
            {
                classList: "layui-icon layui-icon-vercode",
                name: "验证码",
            },
            {
                classList: "layui-icon layui-icon-login-wechat",
                name: "微信",
            },
            {
                classList: "layui-icon layui-icon-login-qq",
                name: "QQ",
            },
            {
                classList: "layui-icon layui-icon-login-weibo",
                name: "微博",
            },
            {
                classList: "layui-icon layui-icon-password",
                name: "密码",
            },
            {
                classList: "layui-icon layui-icon-username",
                name: "用户名",
            },
            {
                classList: "layui-icon layui-icon-refresh-3",
                name: "刷新-粗",
            },
            {
                classList: "layui-icon layui-icon-auz",
                name: "授权",
            },
            {
                classList: "layui-icon layui-icon-spread-left",
                name: "左向右伸缩菜单",
            },
            {
                classList: "layui-icon layui-icon-shrink-right",
                name: "右向左伸缩菜单",
            },
            {
                classList: "layui-icon layui-icon-snowflake",
                name: "雪花",
            },
            {
                classList: "layui-icon layui-icon-tips",
                name: "提示说明",
            },
            {
                classList: "layui-icon layui-icon-note",
                name: "便签",
            },
            {
                classList: "layui-icon layui-icon-home",
                name: "主页",
            },
            {
                classList: "layui-icon layui-icon-senior",
                name: "高级",
            },
            {
                classList: "layui-icon layui-icon-refresh",
                name: "刷新",
            },
            {
                classList: "layui-icon layui-icon-refresh-1",
                name: "刷新",
            },
            {
                classList: "layui-icon layui-icon-flag",
                name: "旗帜",
            },
            {
                classList: "layui-icon layui-icon-theme",
                name: "主题",
            },
            {
                classList: "layui-icon layui-icon-notice",
                name: "消息-通知",
            },
            {
                classList: "layui-icon layui-icon-website",
                name: "网站",
            },
            {
                classList: "layui-icon layui-icon-console",
                name: "控制台",
            },
            {
                classList: "layui-icon layui-icon-face-surprised",
                name: "表情-惊讶",
            },
            {
                classList: "layui-icon layui-icon-set",
                name: "设置-空心",
            },
            {
                classList: "layui-icon layui-icon-template-1",
                name: "模板",
            },
            {
                classList: "layui-icon layui-icon-app",
                name: "应用",
            },
            {
                classList: "layui-icon layui-icon-template",
                name: "模板",
            },
            {
                classList: "layui-icon layui-icon-praise",
                name: "赞",
            },
            {
                classList: "layui-icon layui-icon-tread",
                name: "踩",
            },
            {
                classList: "layui-icon layui-icon-male",
                name: "男",
            },
            {
                classList: "layui-icon layui-icon-female",
                name: "女",
            },
            {
                classList: "layui-icon layui-icon-camera",
                name: "相机-空心",
            },
            {
                classList: "layui-icon layui-icon-camera-fill",
                name: "相机-实心",
            },
            {
                classList: "layui-icon layui-icon-more",
                name: "菜单-水平",
            },
            {
                classList: "layui-icon layui-icon-more-vertical",
                name: "菜单-垂直",
            },
            {
                classList: "layui-icon layui-icon-rmb",
                name: "金额-人民币",
            },
            {
                classList: "layui-icon layui-icon-dollar",
                name: "金额-美元",
            },
            {
                classList: "layui-icon layui-icon-diamond",
                name: "钻石-等级",
            },
            {
                classList: "layui-icon layui-icon-fire",
                name: "火",
            },
            {
                classList: "layui-icon layui-icon-return",
                name: "返回",
            },
            {
                classList: "layui-icon layui-icon-location",
                name: "位置-地图",
            },
            {
                classList: "layui-icon layui-icon-read",
                name: "办公-阅读",
            },
            {
                classList: "layui-icon layui-icon-survey",
                name: "调查",
            },
            {
                classList: "layui-icon layui-icon-face-smile",
                name: "表情-微笑",
            },
            {
                classList: "layui-icon layui-icon-face-cry",
                name: "表情-哭泣",
            },
            {
                classList: "layui-icon layui-icon-cart-simple",
                name: "购物车",
            },
            {
                classList: "layui-icon layui-icon-cart",
                name: "购物车",
            },
            {
                classList: "layui-icon layui-icon-next",
                name: "下一页",
            },
            {
                classList: "layui-icon layui-icon-prev",
                name: "上一页",
            },
            {
                classList: "layui-icon layui-icon-upload-drag",
                name: "上传-空心-拖拽",
            },
            {
                classList: "layui-icon layui-icon-upload",
                name: "上传-实心",
            },
            {
                classList: "layui-icon layui-icon-download-circle",
                name: "下载-圆圈",
            },
            {
                classList: "layui-icon layui-icon-component",
                name: "组件",
            },
            {
                classList: "layui-icon layui-icon-file-b",
                name: "文件-粗",
            },
            {
                classList: "layui-icon layui-icon-user",
                name: "用户",
            },
            {
                classList: "layui-icon layui-icon-find-fill",
                name: "发现-实心",
            },
            {
                classList: "layui-icon layui-icon-loading layui-anim layui-anim-rotate layui-anim-loop",
                name: "loading",
            },
            {
                classList: "layui-icon layui-icon-loading-1 layui-anim layui-anim-rotate layui-anim-loop",
                name: "loading",
            },
            {
                classList: "layui-icon layui-icon-add-1",
                name: "添加",
            },
            {
                classList: "layui-icon layui-icon-play",
                name: "播放",
            },
            {
                classList: "layui-icon layui-icon-pause",
                name: "暂停",
            },
            {
                classList: "layui-icon layui-icon-headset",
                name: "音频-耳机",
            },
            {
                classList: "layui-icon layui-icon-video",
                name: "视频",
            },
            {
                classList: "layui-icon layui-icon-voice",
                name: "语音-声音",
            },
            {
                classList: "layui-icon layui-icon-speaker",
                name: "消息-通知-喇叭",
            },
            {
                classList: "layui-icon layui-icon-fonts-del",
                name: "删除线",
            },
            {
                classList: "layui-icon layui-icon-fonts-code",
                name: "代码",
            },
            {
                classList: "layui-icon layui-icon-fonts-html",
                name: "HTML",
            },
            {
                classList: "layui-icon layui-icon-fonts-strong",
                name: "字体加粗",
            },
            {
                classList: "layui-icon layui-icon-unlink",
                name: "删除链接",
            },
            {
                classList: "layui-icon layui-icon-picture",
                name: "图片",
            },
            {
                classList: "layui-icon layui-icon-link",
                name: "链接",
            },
            {
                classList: "layui-icon layui-icon-face-smile-b",
                name: "表情-笑-粗",
            },
            {
                classList: "layui-icon layui-icon-align-left",
                name: "左对齐",
            },
            {
                classList: "layui-icon layui-icon-align-right",
                name: "右对齐",
            },
            {
                classList: "layui-icon layui-icon-align-center",
                name: "居中对齐",
            },
            {
                classList: "layui-icon layui-icon-fonts-u",
                name: "字体-下划线",
            },
            {
                classList: "layui-icon layui-icon-fonts-i",
                name: "字体-斜体",
            },
            {
                classList: "layui-icon layui-icon-tabs",
                name: "Tabs 选项卡",
            },
            {
                classList: "layui-icon layui-icon-radio",
                name: "单选框-选中",
            },
            {
                classList: "layui-icon layui-icon-circle",
                name: "单选框-候选",
            },
            {
                classList: "layui-icon layui-icon-edit",
                name: "编辑",
            },
            {
                classList: "layui-icon layui-icon-share",
                name: "分享",
            },
            {
                classList: "layui-icon layui-icon-delete",
                name: "删除",
            },
            {
                classList: "layui-icon layui-icon-form",
                name: "表单",
            },
            {
                classList: "layui-icon layui-icon-cellphone-fine",
                name: "手机-细体",
            },
            {
                classList: "layui-icon layui-icon-dialogue",
                name: "聊天 对话 沟通",
            },
            {
                classList: "layui-icon layui-icon-fonts-clear",
                name: "文字格式化",
            },
            {
                classList: "layui-icon layui-icon-layer",
                name: "窗口",
            },
            {
                classList: "layui-icon layui-icon-date",
                name: "日期",
            },
            {
                classList: "layui-icon layui-icon-water",
                name: "水 下雨",
            },
            {
                classList: "layui-icon layui-icon-code-circle",
                name: "代码-圆圈",
            },
            {
                classList: "layui-icon layui-icon-carousel",
                name: "轮播组图",
            },
            {
                classList: "layui-icon layui-icon-prev-circle",
                name: "翻页",
            },
            {
                classList: "layui-icon layui-icon-layouts",
                name: "布局",
            },
            {
                classList: "layui-icon layui-icon-util",
                name: "工具",
            },
            {
                classList: "layui-icon layui-icon-templeate-1",
                name: "选择模板",
            },
            {
                classList: "layui-icon layui-icon-upload-circle",
                name: "上传-圆圈",
            },
            {
                classList: "layui-icon layui-icon-tree",
                name: "树",
            },
            {
                classList: "layui-icon layui-icon-table",
                name: "表格",
            },
            {
                classList: "layui-icon layui-icon-chart",
                name: "图表",
            },
            {
                classList: "layui-icon layui-icon-chart-screen",
                name: "图标 报表 屏幕",
            },
            {
                classList: "layui-icon layui-icon-engine",
                name: "引擎",
            },
            {
                classList: "layui-icon layui-icon-triangle-d",
                name: "下三角",
            },
            {
                classList: "layui-icon layui-icon-triangle-r",
                name: "右三角",
            },
            {
                classList: "layui-icon layui-icon-file",
                name: "文件",
            },
            {
                classList: "layui-icon layui-icon-set-sm",
                name: "设置-小型",
            },
            {
                classList: "layui-icon layui-icon-reduce-circle",
                name: "减少-圆圈",
            },
            {
                classList: "layui-icon layui-icon-add-circle",
                name: "添加-圆圈",
            },
            {
                classList: "layui-icon layui-icon-404",
                name: "404",
            },
            {
                classList: "layui-icon layui-icon-about",
                name: "关于",
            },
            {
                classList: "layui-icon layui-icon-up",
                name: "箭头 向上",
            },
            {
                classList: "layui-icon layui-icon-down",
                name: "箭头 向下",
            },
            {
                classList: "layui-icon layui-icon-left",
                name: "箭头 向左",
            },
            {
                classList: "layui-icon layui-icon-right",
                name: "箭头 向右",
            },
            {
                classList: "layui-icon layui-icon-circle-dot",
                name: "圆点",
            },
            {
                classList: "layui-icon layui-icon-search",
                name: "搜索",
            },
            {
                classList: "layui-icon layui-icon-set-fill",
                name: "设置-实心",
            },
            {
                classList: "layui-icon layui-icon-group",
                name: "群组",
            },
            {
                classList: "layui-icon layui-icon-friends",
                name: "好友",
            },
            {
                classList: "layui-icon layui-icon-reply-fill",
                name: "回复 评论 实心",
            },
            {
                classList: "layui-icon layui-icon-menu-fill",
                name: "菜单 隐身 实心",
            },
            {
                classList: "layui-icon layui-icon-log",
                name: "记录",
            },
            {
                classList: "layui-icon layui-icon-picture-fine",
                name: "图片-细体",
            },
            {
                classList: "layui-icon layui-icon-face-smile-fine",
                name: "表情-笑-细体",
            },
            {
                classList: "layui-icon layui-icon-list",
                name: "列表",
            },
            {
                classList: "layui-icon layui-icon-release",
                name: "发布 纸飞机",
            },
            {
                classList: "layui-icon layui-icon-ok",
                name: "对 OK",
            },
            {
                classList: "layui-icon layui-icon-help",
                name: "帮助",
            },
            {
                classList: "layui-icon layui-icon-chat",
                name: "客服",
            },
            {
                classList: "layui-icon layui-icon-top",
                name: "top 置顶",
            },
            {
                classList: "layui-icon layui-icon-star",
                name: "收藏-空心",
            },
            {
                classList: "layui-icon layui-icon-star-fill",
                name: "收藏-实心",
            },
            {
                classList: "layui-icon layui-icon-close-fill",
                name: "关闭-实心",
            },
            {
                classList: "layui-icon layui-icon-close",
                name: "关闭-空心",
            },
            {
                classList: "layui-icon layui-icon-ok-circle",
                name: "正确",
            },
            {
                classList: "layui-icon layui-icon-add-circle-fine",
                name: "添加-圆圈-细体",
            },
        ];

        // 生成样式
        function generatorClass(className) {
            return [prefix, className].join("-");
        }

        // 入口
        function render(elem, opts) {
            if (!opts) opts = {};

            // 初始化必要DOM
            var $elems = $(elem);

            $elems.each(function () {
                var $elem = $(this);

                $elem.hide();
                var $body = $("body");
                var $parent = $elem.parent();

                // 初始化配置
                var width = opts.width || 300;
                var offsetX = opts.offsetX || 0;
                var offsetY = opts.offsetY || 5;
                var icons = opts.icons || layuiIcons;
                var placeholder = $elem.attr("placeholder") || opts.placeholder || "请选择";
                var value = $elem.val() || opts.value;
                var zIndex = opts.zIndex || 19961005;

                // 托管事件
                function activeEvent(name, event, data) {
                    if (opts && opts.event && typeof opts.event[name] == "function") {
                        opts.event[name](event, data);
                    }
                }

                // 更新值
                function updateValueByClassList(classList) {
                    for (var i = 0; i < icons.length; i++) {
                        var icon = icons[i];

                        if (icon.classList === classList) {
                            // 处理选中效果
                            var $icons = $icon_container.find("." + generatorClass("item"));
                            $icons.removeClass("selected");
                            $icons.eq(i).addClass("selected");

                            // 清空输入框
                            $input_dom.empty();

                            // 设置值
                            var $select = $('<div class="' + generatorClass("selected-value") + '"></div>');
                            var $i = $('<i class="' + icon.classList + '"></i>').addClass(generatorClass("icon"));
                            var $name = $('<div class="' + generatorClass("name") + '">' + icon.name + "</div>");
                            $select.append($i).append($name);
                            $input_dom.append($select).append($icon_down);
                            $icon_container.removeClass("show");
                            $elem.val(classList);
                            break;
                        }
                    }
                }

                // 创建虚拟DOM
                var $placeholder = $('<span class="placeholder">' + placeholder + "</span>");
                var $input_dom = $('<div class="' + generatorClass("input") + '"></div>');
                var $icon_down = $('<i class="layui-icon layui-icon-triangle-d"></i>');
                var $icon_container = $('<div class="' + generatorClass("container") + '"></div>');
                $icon_container.css({
                    zIndex: zIndex,
                    width: width,
                    marginTop: offsetY,
                    marginLeft: offsetX,
                });
                $input_dom.append($placeholder).append($icon_down);
                $parent.append($input_dom).append($icon_container);

                // 点击body移除弹层
                $body.click(function () {
                    $icon_container.removeClass("show");
                });

                $input_dom.click(function (e) {
                    e.stopPropagation();
                    $icon_container.hasClass("show") ? $icon_container.removeClass("show") : $icon_container.addClass("show");
                });

                if (!opts) opts = {};

                icons.forEach(function (icon, index) {
                    var $icon = $('<div class="' + generatorClass("item") + '"></div>');
                    var $i = $('<i class="' + icon.classList + '"></i>').addClass(generatorClass("icon"));
                    var $name = $('<div class="' + generatorClass("name") + '">' + icon.name + "</div>");
                    $icon
                        .append($i)
                        .append($name)
                        .click(function (e) {
                            e.stopPropagation();
                            e.preventDefault();

                            var classList = icons[index].classList;
                            updateValueByClassList(classList);
                            activeEvent("select", e, {
                                index: index,
                                icons: icons,
                                icon: classList,
                            });
                        });

                    $icon_container.append($icon);
                });

                updateValueByClassList(value);
            });

            return this;
        }

        function insertIcon(slot, icons) {
            icons.forEach(function (icon) {
                switch (slot) {
                    case 1:
                        layuiIcons.unshift(icon);
                        break;
                    case -1:
                        layuiIcons.push(icon);
                        break;
                }
            });
            return this;
        }

        function addIcon(name, classList) {
            insertIcon(-1, [
                {
                    name: name,
                    classList: classList,
                },
            ]);
            return this;
        }

        function addIcons(icons) {
            insertIcon(-1, icons || []);
            return this;
        }

        function addIconBefore(name, classList) {
            insertIcon(1, [
                {
                    name: name,
                    classList: classList,
                },
            ]);
            return this;
        }

        function addIconsBefore(icons) {
            insertIcon(1, icons || []);
            return this;
        }

        this.render = render;
        this.icons = layuiIcons;
        this.addIcon = addIcon;
        this.addIcons = addIcons;
        this.addIconBefore = addIconBefore;
        this.addIconsBefore = addIconsBefore;
    }

    exports("iconSelected", new IconSelected());
});
