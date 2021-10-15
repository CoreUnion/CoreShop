layui.define(["layer", "setter"],
    function (f) {
        var h = layui.jquery;
        var k = layui.layer;
        var d = layui.setter;
        var n = { version: "1.1.0", layerData: {} };
        n.open = function (s) {
            if (s.content && s.type === 2) {
                s.url = undefined
            }
            if (s.url && (s.type === 2 || s.type === undefined)) {
                s.type = 1
            }
            if (s.area === undefined) {
                s.area = s.type === 2 ? ["360px", "300px"] : "360px"
            }
            if (s.offset === undefined) {
                s.offset = "70px"
            }
            if (s.shade === undefined) {
                s.shade = 0.1
            }
            if (s.fixed === undefined) {
                s.fixed = false
            }
            if (s.resize === undefined) {
                s.resize = false
            }
            if (s.skin === undefined) {
                //s.skin = "layui-layer-admin"
                s.skin = "layui-layer-admin"
            }
            var q = s.end;
            s.end = function () {
                k.closeAll("tips");
                q && q()
            };
            if (s.url) {
                var r = s.success;
                s.success = function (t, u) {
                    h(t).data("tpl", s.tpl || "");
                    n.reloadLayer(u, s.url, r)
                }
            } else {
                if (s.tpl && s.content) {
                    s.content = n.util.tpl(s.content, s.data, d.tplOpen, d.tplClose)
                }
            }
            var i = k.open(s);
            if (s.data) {
                n.layerData["d" + i] = s.data
            }
            return i
        };
        n.closeDialog = function (i) {
            if (i) {
                k.close(n.getLayerIndex(i))
            } else {
                parent.layer.close(parent.layer.getFrameIndex(window.name))
            }
        };
        n.getLayerIndex = function (i) {
            if (!i) {
                return parent.layer.getFrameIndex(window.name)
            }
            var q = h(i).parents(".layui-layer").first().attr("id");
            if (q && q.length >= 11) {
                return q.substring(11)
            }
        };
        n.cropImg = function (s) {
            var q = "image/jpeg";
            var x = s.aspectRatio;
            var y = s.imgSrc ? s.imgSrc == 'no' ? null : s.imgSrc : "/static/images/common/empty-banner.png";

            //var y = s.imgSrc == 'no' ? null : s.imgSrc;

            var v = s.imgType;
            var t = s.onCrop;
            var u = s.limitSize;
            var w = s.acceptMime;
            var r = s.exts;
            var i = s.title;
            if (x === undefined) {
                x = 1
            }
            if (i === undefined) {
                i = "裁剪图片"
            }
            if (v) {
                q = v
            }
            layui.use(["Cropper", "upload"],
                function () {
                    var A = layui.Cropper, z = layui.upload;

                    function B() {
                        var E, F = h("#ew-crop-img");
                        var G = {
                            elem: "#ew-crop-img-upload",
                            auto: false,
                            drag: false,
                            choose: function (H) {
                                H.preview(function (J, K, I) {
                                    console.log("xuanze");
                                    q = K.type;
                                    F.attr("src", I);
                                    if (!y || !E) {
                                        y = I;
                                        B()
                                    } else {
                                        E.destroy();
                                        E = new A(F[0], D)
                                    }
                                })
                            }
                        };
                        if (u !== undefined) {
                            G.size = u
                        }
                        if (w !== undefined) {
                            G.acceptMime = w
                        }
                        if (r !== undefined) {
                            G.exts = r
                        }
                        z.render(G);
                        if (!y) {
                            return h("#ew-crop-img-upload").trigger("click")
                        }
                        var D = {
                            aspectRatio: x, preview: ".ew-crop-img-preview",
                            ready: function (e) {
                                //console.log("初始准备" + e.type);
                            },
                            cropstart: function (e) {
                                //console.log(e.type, e.detail.action);
                            },
                            cropmove: function (e) {
                                //console.log(e.type, e.detail.action);
                            },
                            cropend: function (e) {
                                //console.log(e.type, e.detail.action);
                            },
                            crop: function (e) {
                                var data = e.detail;

                                var dataX = document.getElementById('dataX');
                                var dataY = document.getElementById('dataY');
                                var dataHeight = document.getElementById('dataHeight');
                                var dataWidth = document.getElementById('dataWidth');
                                var dataRotate = document.getElementById('dataRotate');
                                var dataScaleX = document.getElementById('dataScaleX');
                                var dataScaleY = document.getElementById('dataScaleY');
                                dataX.value = Math.round(data.x);
                                dataY.value = Math.round(data.y);
                                dataHeight.value = Math.round(data.height);
                                dataWidth.value = Math.round(data.width);
                                dataRotate.value = typeof data.rotate !== 'undefined' ? data.rotate : '';
                                dataScaleX.value = typeof data.scaleX !== 'undefined' ? data.scaleX : '';
                                dataScaleY.value = typeof data.scaleY !== 'undefined' ? data.scaleY : '';
                            },
                            zoom: function (e) {
                                console.log(e.type, e.detail.ratio);
                            }
                        };
                        E = new A(F[0], D);
                        h(".ew-crop-tool").on("click",
                            "[data-method]",
                            function () {
                                var I = h(this).data(), J, H;
                                if (!E || !I.method) {
                                    return
                                }
                                I = h.extend({}, I);
                                J = E.cropped;
                                switch (I.method) {
                                    case "rotate":
                                        if (J && D.viewMode > 0) {
                                            E.clear()
                                        }
                                        break;
                                    case "getCroppedCanvas":
                                        if (q === "image/jpeg") {
                                            if (!I.option) {
                                                I.option = {}
                                            }
                                            I.option.fillColor = "#fff"
                                        }
                                        break
                                }
                                H = E[I.method](I.option, I.secondOption);
                                switch (I.method) {
                                    case "rotate":
                                        if (J && D.viewMode > 0) {
                                            E.crop()
                                        }
                                        break;
                                    case "scaleX":
                                    case "scaleY":
                                        h(this).data("option", -I.option);
                                        break;
                                    case "getCroppedCanvas":
                                        if (H) {
                                            t && t(H.toDataURL(q));
                                            n.closeDialog("#ew-crop-img")
                                        } else {
                                            k.msg("裁剪失败", { icon: 2, anim: 6 })
                                        }
                                        break
                                }
                            })

                        // docs - toggles
                        h(".my-toggles").on('click', "[data-value]",
                            function () {
                                var data = h(this).data();
                                //console.log(data.value);
                                if (!E) {
                                    return;
                                }
                                D.aspectRatio = data.value;
                                D.ready = function () {
                                    //console.log('重新准备');
                                };
                                // Restart
                                E.destroy();
                                E = new A(F[0], D);
                            });
                    }

                    var C = [
                        '<div class="layui-row">', '     <div class="layui-col-sm8" style="height: 500px;background: #e6e6e6;max-height:500px;">',
                        '          <img id="ew-crop-img" crossOrigin="anonymous" src="', y || "", '" style="max-width:100%;" alt=""/>',
                        "     </div>",
                        '     <div class="layui-col-sm4 layui-hide-xs docs-preview clearfix" style="padding:0px 15px;text-align: center;">',
                        '          <div class="ew-crop-img-preview preview-lg"></div>',
                        '          <div class="ew-crop-img-preview preview-md"></div>',
                        '          <div class="ew-crop-img-preview preview-sm"></div>',
                        '          <div class="ew-crop-img-preview preview-xs"></div>',
                        '<div class="layui-form layui-form-pane myCropperBox">',
                        '	<div class="layui-form-item  input-group-sm">',
                        '			<label class="layui-form-label" for="dataX">',
                        '				X轴坐标',
                        '			</label>',
                        '<div class="layui-input-inline" style="width:140px;">',
                        '		<input type="text" class="layui-input" id="dataX" placeholder="x">',
                        '</div>',
                        '			<div class="layui-form-mid layui-word-aux">',
                        '				像素',
                        '			</div>',
                        '	</div>',
                        '	<div class="layui-form-item  input-group-sm">',
                        '			<label class="layui-form-label" for="dataY">',
                        '				Y轴坐标',
                        '			</label>',
                        '<div class="layui-input-inline" style="width:140px;">',
                        '		<input type="text" class="layui-input" id="dataY" placeholder="y">',
                        '</div>',
                        '			<div class="layui-form-mid layui-word-aux">',
                        '				像素',
                        '			</div>',
                        '	</div>',
                        '	<div class="layui-form-item  input-group-sm">',
                        '			<label class="layui-form-label" for="dataWidth">',
                        '				宽度',
                        '			</label>',
                        '<div class="layui-input-inline" style="width:140px;">',
                        '		<input type="text" class="layui-input" id="dataWidth" placeholder="width">',
                        '</div>',
                        '			<div class="layui-form-mid layui-word-aux">',
                        '				像素',
                        '			</div>',
                        '	</div>',
                        '	<div class="layui-form-item  input-group-sm">',
                        '			<label class="layui-form-label" for="dataHeight">',
                        '				高度',
                        '			</label>',
                        '<div class="layui-input-inline" style="width:140px;">',
                        '		<input type="text" class="layui-input" id="dataHeight" placeholder="height">',
                        '</div>',
                        '			<div class="layui-form-mid layui-word-aux">',
                        '				像素',
                        '			</div>',
                        '	</div>',
                        '	<div class="layui-form-item  input-group-sm">',
                        '			<label class="layui-form-label" for="dataRotate">',
                        '				旋转',
                        '			</label>',
                        '<div class="layui-input-inline" style="width:140px;">',
                        '		<input type="text" class="layui-input" id="dataRotate" placeholder="rotate">',
                        '</div>',
                        '			<div class="layui-form-mid layui-word-aux">',
                        '				度',
                        '			</div>',
                        '	</div>',
                        '	<div class="layui-form-item  input-group-sm">',
                        '			<label class="layui-form-label" for="dataScaleX">',
                        '				左右翻转',
                        '			</label>',
                        '<div class="layui-input-inline" style="width:140px;">',
                        '		<input type="text" class="layui-input" id="dataScaleX" placeholder="scaleX">',
                        '</div>',
                        '	</div>',
                        '	<div class="layui-form-item  input-group-sm">',
                        '			<label class="layui-form-label" for="dataScaleY">',
                        '				上下翻转',
                        '			</label>',
                        '<div class="layui-input-inline" style="width:140px;">',
                        '		<input type="text" class="layui-input" id="dataScaleY" placeholder="scaleY">',
                        '</div>',
                        '	</div>',
                        '</div>',


                        '<div class="layui-btn-group my-toggles">',
                        '<button type="button" class="layui-btn layui-btn-sm myCropperImg" data-value="1.7777777777777777">16:9</button>',
                        '<button type="button" class="layui-btn layui-btn-sm" data-value="1.3333333333333333">4:3</button>',
                        '<button type="button" class="layui-btn layui-btn-sm" data-value="1">1:1</button>',
                        '<button type="button" class="layui-btn layui-btn-sm" data-value="0.6666666666666666">2:3</button>',
                        '<button type="button" class="layui-btn layui-btn-sm" data-value="NaN">自由</button>',
                        '</div>',



                        "     </div>",
                        "</div>",
                        '<div class="ew-crop-tool" style="padding: 15px 0px 0px 0;">',
                        '     <div class="layui-btn-group">',
                        '          <button title="放大" data-method="zoom" data-option="0.1" class="layui-btn icon-btn" type="button"><i class="layui-icon layui-icon-add-1"></i></button>',
                        '          <button title="缩小" data-method="zoom" data-option="-0.1" class="layui-btn icon-btn" type="button"><span style="display: inline-block;width: 12px;height: 2.5px;background: rgba(255, 255, 255, 0.9);vertical-align: middle;margin: 0 4px;"></span></button>',
                        "     </div>", '     <div class="layui-btn-group layui-hide-xs">',
                        '          <button title="向左旋转" data-method="rotate" data-option="-45" class="layui-btn icon-btn" type="button"><i class="layui-icon layui-icon-refresh-1" style="transform: rotateY(180deg) rotate(40deg);display: inline-block;"></i></button>',
                        '          <button title="向右旋转" data-method="rotate" data-option="45" class="layui-btn icon-btn" type="button"><i class="layui-icon layui-icon-refresh-1" style="transform: rotate(30deg);display: inline-block;"></i></button>',
                        "     </div>", '     <div class="layui-btn-group">',
                        '          <button title="左移" data-method="move" data-option="-10" data-second-option="0" class="layui-btn icon-btn" type="button"><i class="layui-icon layui-icon-left"></i></button>',
                        '          <button title="右移" data-method="move" data-option="10" data-second-option="0" class="layui-btn icon-btn" type="button"><i class="layui-icon layui-icon-right"></i></button>',
                        '          <button title="上移" data-method="move" data-option="0" data-second-option="-10" class="layui-btn icon-btn" type="button"><i class="layui-icon layui-icon-up"></i></button>',
                        '          <button title="下移" data-method="move" data-option="0" data-second-option="10" class="layui-btn icon-btn" type="button"><i class="layui-icon layui-icon-down"></i></button>',
                        "     </div>", '     <div class="layui-btn-group">',
                        '          <button title="左右翻转" data-method="scaleX" data-option="-1" class="layui-btn icon-btn" type="button" style="position: relative;width: 41px;"><i class="layui-icon layui-icon-triangle-r" style="position: absolute;left: 9px;top: 0;transform: rotateY(180deg);font-size: 16px;"></i><i class="layui-icon layui-icon-triangle-r" style="position: absolute; right: 3px; top: 0;font-size: 16px;"></i></button>',
                        '          <button title="上下翻转" data-method="scaleY" data-option="-1" class="layui-btn icon-btn" type="button" style="position: relative;width: 41px;"><i class="layui-icon layui-icon-triangle-d" style="position: absolute;left: 11px;top: 6px;transform: rotateX(180deg);line-height: normal;font-size: 16px;"></i><i class="layui-icon layui-icon-triangle-d" style="position: absolute; left: 11px; top: 14px;line-height: normal;font-size: 16px;"></i></button>',
                        "     </div>", '     <div class="layui-btn-group">',
                        '          <button title="重新开始" data-method="reset" class="layui-btn icon-btn" type="button"><i class="layui-icon layui-icon-refresh"></i></button>',
                        '          <button title="选择图片" id="ew-crop-img-upload" class="layui-btn icon-btn" type="button" style="border-radius: 0 2px 2px 0;"><i class="layui-icon layui-icon-upload-drag"></i></button>',
                        "     </div>",
                        '     <button data-method="getCroppedCanvas" data-option="{ &quot;maxWidth&quot;: 4096, &quot;maxHeight&quot;: 4096 }" class="layui-btn icon-btn" type="button" style="margin-left: 10px;"><i class="layui-icon">&#xe605;</i>完成</button>',
                        "</div>"
                    ].join("");
                    n.open({
                        title: i,
                        area: "900px",
                        type: 1,
                        content: C,
                        success: function (D, E) {
                            h(D).children(".layui-layer-content").css("overflow", "visible");
                            B()
                        }
                    })
                })
        };

        f("cropperImg", n)
    });