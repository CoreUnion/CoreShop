<title>{{ModelDescription}}</title>
<!--当前位置开始-->
<div class="layui-card layadmin-header">
    <div class="layui-breadcrumb" lay-filter="breadcrumb">
        <script type="text/html" template lay-done="layui.data.updateMainBreadcrumb();">
        </script>
    </div>
</div>
<!--当前位置结束-->
<style>
    /* 重写样式 */
</style>
<script type="text/html" template lay-type="Post" lay-url="{{ layui.setter.apiUrl }}Api/{{ModelClassName}}/GetIndex" lay-done="layui.data.done(d);">
    
</script>
<div class="table-body">
    <table  id="LAY-app-{{ModelClassName}}-tableBox" lay-filter="LAY-app-{{ModelClassName}}-tableBox"></table>
</div>

<script type="text/html" id="LAY-app-{{ModelClassName}}-toolbar">
    <div class="layui-form coreshop-toolbar-search-form">
        <div class="layui-form-item">
		    {% for field in ModelFields %}{% if field.DataType == 'bit' %}
		    <div class="layui-inline">
			    <label class="layui-form-label" for="{{field.DbColumnName}}">{{field.ColumnDescription}}</label>
                <div class="layui-input-inline">
                    <select name="{{field.DbColumnName}}">
                        <option value="">请选择</option>
                        <option value="True">是</option>
                        <option value="False">否</option>
                    </select>
                </div>
            </div>{% elsif field.DataType == 'datetime' %}
            <div class="layui-inline">
                <label class="layui-form-label" for="{{field.DbColumnName}}">{{field.ColumnDescription}}</label>
                <div class="layui-input-inline" style="width: 260px;">
                    <input type="text" name="{{field.DbColumnName}}" id="searchTime-{{ModelClassName}}-{{field.DbColumnName}}" placeholder="请输入{{field.ColumnDescription}}" class="layui-input">
                </div>
            </div>{% else %}
            <div class="layui-inline">
                <label class="layui-form-label" for="{{field.DbColumnName}}">{{field.ColumnDescription}}</label>
                <div class="layui-input-inline">
                    <input type="text" name="{{field.DbColumnName}}"  placeholder="请输入{{field.ColumnDescription}}" class="layui-input">
                </div>
            </div>{% endif %}{% endfor %}
            <div class="layui-inline">
                <button class="layui-btn layui-btn-sm" lay-submit lay-filter="LAY-app-{{ModelClassName}}-search"><i class="layui-icon layui-icon-search"></i>筛选</button>
            </div>
        </div>
    </div>
</script>

<script type="text/html" id="LAY-app-{{ModelClassName}}-pagebar">
    <div class="layui-btn-container">
        <button class="layui-btn layui-btn-sm" lay-event="addData"><i class="layui-icon layui-icon-add-1"></i>添加数据</button>
        <button class="layui-btn layui-btn-sm" lay-event="batchDelete"><i class="layui-icon layui-icon-delete"></i>批量删除</button>
        <button class="layui-btn layui-btn-sm" lay-event="selectExportExcel"><i class="layui-icon layui-icon-add-circle"></i>选择导出</button>
        <button class="layui-btn layui-btn-sm" lay-event="queryExportExcel"><i class="layui-icon layui-icon-download-circle"></i>查询导出</button>
    </div>
</script>

<script type="text/html" id="LAY-app-{{ModelClassName}}-tableBox-bar">
    <a class="layui-btn layui-btn-primary layui-btn-xs" lay-event="detail">查看</a>
    <a class="layui-btn layui-btn-xs" lay-event="edit">编辑</a>
    <a class="layui-btn layui-btn-danger layui-btn-xs" data-dropdown="#{{ModelClassName}}TbDelDrop{% raw %}{{d.LAY_INDEX}}{% endraw %}" no-shade="true">删除</a>
    <div class="dropdown-menu-nav dropdown-popconfirm dropdown-top-right layui-hide" id="{{ModelClassName}}TbDelDrop{% raw %}{{d.LAY_INDEX}}{% endraw %}"
         style="max-width: 200px;white-space: normal;min-width: auto;margin-left: 10px;">
        <div class="dropdown-anchor"></div>
        <div class="dropdown-popconfirm-title">
            <i class="layui-icon layui-icon-help"></i>
            确定要删除吗？
        </div>
        <div class="dropdown-popconfirm-btn">
            <a class="layui-btn layui-btn-primary cursor" btn-cancel>取消</a>
            <a class="layui-btn layui-btn-normal cursor" lay-event="del">确定</a>
        </div>
    </div>
</script>

<script>
    var indexData;
    var debug= layui.setter.debug;
    layui.data.done = function (d) {
        //开启调试情况下获取接口赋值数据
        if (debug) { console.log(d); }

        indexData = d.data;
        layui.use(['index', 'table', 'laydate', 'util', 'coredropdown', 'coreHelper'],
            function () {
                var $ = layui.$
                    , admin = layui.admin
                    , table = layui.table
                    , form = layui.form
                    , laydate = layui.laydate
                    , setter = layui.setter
                    , coreHelper = layui.coreHelper
                    , util = layui.util
                    , view = layui.view;
			    
                var searchwhere;
                //监听搜索
                form.on('submit(LAY-app-{{ModelClassName}}-search)',
                    function(data) {
                        var field = data.field;
                        searchwhere = field;
                        //执行重载
                        table.reloadData('LAY-app-{{ModelClassName}}-tableBox',{ where: field });
                    });
                //数据绑定
                table.render({
                    elem: '#LAY-app-{{ModelClassName}}-tableBox',
                    url: layui.setter.apiUrl + 'Api/{{ModelClassName}}/GetPageList',
                    method: 'POST',
				    toolbar: '#LAY-app-{{ModelClassName}}-toolbar',
				    pagebar: '#LAY-app-{{ModelClassName}}-pagebar',
                    className: 'pagebarbox',
                    defaultToolbar: ['filter', 'print', 'exports'],
                    height: 'full-127',//面包屑142px,搜索框4行172,3行137,2行102,1行67
                    page: true,
                    limit: 30,
                    limits: [10, 15, 20, 25, 30, 50, 100, 200],
                    text: {none: '暂无相关数据'},
                    cols: [
                        [
                            { type: "checkbox", fixed: "left" },{% for field in ModelFields %}{% if field.IsIdentity == true and field.IsPrimarykey == true %}
						    { field: '{{field.DbColumnName}}', title: '{{field.ColumnDescription}}', width: 60, sort: false},{% elsif field.DataType == 'datetime' %}
						    { field: '{{field.DbColumnName}}', title: '{{field.ColumnDescription}}', width: 130, sort: false},{% elsif  field.DataType == 'bit' %}
						    { field: '{{field.DbColumnName}}', title: '{{field.ColumnDescription}}', width: 95, templet: '#switch_{{field.DbColumnName}}', sort: false , unresize: true},{% elsif  field.DbColumnName contains 'Image' or field.DbColumnName contains 'image'  or field.DbColumnName contains 'thumbnail'  or field.DbColumnName contains 'thumbnail'%}
                            { field: '{{field.DbColumnName}}', title: '{{field.ColumnDescription}}', width: 100, sort: false,
                                templet: function (d) {
                                    if (d.{{field.DbColumnName}}) {
                                        return '<a href="javascript:void(0);" onclick=layui.coreHelper.viewImage("' + d.{{field.DbColumnName}} + '")><image style="max-width:28px;max-height:28px;" src="' + d.{{field.DbColumnName}} + '"/></a>';
                                    } else {
                                        return '<a href="javascript:void(0);" onclick=layui.coreHelper.viewImage("' + setter.noImagePicUrl + '")><image style="max-width:30px;max-height:30px;" src="' + setter.noImagePicUrl + '"/></a>';
                                    }
                                }
                            },{% else %}
						    { field: '{{field.DbColumnName}}', title: '{{field.ColumnDescription}}', sort: false,width: 105 },{% endif %}{% endfor %}
                            { width: 162, align: 'center', title:'操作', fixed: 'right', toolbar: '#LAY-app-{{ModelClassName}}-tableBox-bar' }
                        ]
                    ]
                });
                //监听排序事件
                table.on('sort(LAY-app-{{ModelClassName}}-tableBox)', function(obj){
                    table.reloadData('LAY-app-{{ModelClassName}}-tableBox', {
                        initSort: obj, //记录初始排序，如果不设的话，将无法标记表头的排序状态。
                        where: { //请求参数（注意：这里面的参数可任意定义，并非下面固定的格式）
                            orderField: obj.field, //排序字段
                            orderDirection: obj.type //排序方式
                        }
                    });
                });
                //监听行双击事件
                table.on('rowDouble(LAY-app-{{ModelClassName}}-tableBox)', function (obj) {
                    //查看详情
                    doDetails(obj);
                });
                //头工具栏事件
                table.on('pagebar(LAY-app-{{ModelClassName}}-tableBox)', function (obj) {
                    var checkStatus = table.checkStatus(obj.config.id);
                    switch (obj.event) {
                    case 'addData':
                        doCreate();
                        break;
                    case 'batchDelete':
                        doBatchDelete(checkStatus);
                        break;
                    case 'selectExportExcel':
                        doSelectExportExcel(checkStatus);
                        break;
                    case 'queryExportExcel':
                        doQueryExportexcel();
                        break;
                    };
                });
                //监听工具条
                table.on('tool(LAY-app-{{ModelClassName}}-tableBox)',
                    function(obj) {
                        if (obj.event === 'detail') {
                            doDetails(obj);
                        } else if (obj.event === 'del') {
                            doDelete(obj);
                        } else if (obj.event === 'edit') {
                            doEdit(obj)
                        }
                    });
                //执行创建操作
                function doCreate(){
                    coreHelper.Post("Api/{{ModelClassName}}/GetCreate", null, function (e) {
                            if (e.code === 0) {
                                admin.popup({
                                    shadeClose: false,
                                    title: '创建数据',
                                    area: ['1200px', '90%'],
                                    id: 'LAY-popup-{{ModelClassName}}-create',
                                    success: function (layero, index) {
                                        view(this.id).render('base/{{ModelClassName}}/create', { data: e.data }).done(function () {
                                            //监听提交
                                            form.on('submit(LAY-app-{{ModelClassName}}-createForm-submit)',
                                                function(data) {
                                                    var field = data.field; //获取提交的字段
                                                    {% for field in ModelFields %}{% if field.DataType == 'bit' %}
                                                    field.{{field.DbColumnName}} = field.{{field.DbColumnName}} == 'on';{% endif %}{% endfor %}

                                                    if (debug) { console.log(field); } //开启调试返回数据
                                                    //提交 Ajax 成功后，关闭当前弹层并重载表格
                                                    coreHelper.Post("Api/{{ModelClassName}}/DoCreate", field, function (e) {
                                                            console.log(e)
                                                            if (e.code === 0) {
                                                                layui.table.reloadData('LAY-app-{{ModelClassName}}-tableBox'); //重载表格
                                                                layer.close(index); //再执行关闭
                                                                layer.msg(e.msg);
                                                            } else {
                                                                layer.msg(e.msg);
                                                            }
                                                        });
                                                });
                                        });
                                        // 禁止弹窗出现滚动条
                                        $(layero).children('.layui-layer-content').css('overflow', 'visible');
                                    }
                                    , btn: ['确定', '取消']
                                    , yes: function (index, layero) {
                                        layero.contents().find("#LAY-app-{{ModelClassName}}-createForm-submit").click();
                                    }
                                });
                            } else {
                                layer.msg(e.msg);
                            }
                        });
			    }
                //执行编辑操作
                function doEdit(obj){
                    coreHelper.Post("Api/{{ModelClassName}}/GetEdit", {id:obj.data.id}, function (e) {
                        if (e.code === 0) {
                            admin.popup({
                                shadeClose: false,
                                title: '编辑数据',
                                area: ['1200px', '90%'],
                                id: 'LAY-popup-{{ModelClassName}}-edit',
                                success: function (layero, index) {
                                    view(this.id).render('base/{{ModelClassName}}/edit', { data: e.data }).done(function () {
                                        //监听提交
                                        form.on('submit(LAY-app-{{ModelClassName}}-editForm-submit)',
                                            function(data) {
                                                var field = data.field; //获取提交的字段
                                                {% for field in ModelFields %}{% if field.DataType == 'bit' %}
                                                field.{{field.DbColumnName}} = field.{{field.DbColumnName}} == 'on';{% endif %}{% endfor %}

                                                if (debug) { console.log(field); } //开启调试返回数据
                                                //提交 Ajax 成功后，关闭当前弹层并重载表格
                                                coreHelper.Post("Api/{{ModelClassName}}/DoEdit", field, function (e) {
                                                        console.log(e)
                                                        if (e.code === 0) {
                                                            layui.table.reloadData('LAY-app-{{ModelClassName}}-tableBox'); //重载表格
                                                            layer.close(index); //再执行关闭
                                                            layer.msg(e.msg);
                                                        } else {
                                                            layer.msg(e.msg);
                                                        }
                                                    });
                                            });
                                    })
                                    // 禁止弹窗出现滚动条
                                    $(layero).children('.layui-layer-content').css('overflow', 'visible');
                                }
                                 , btn: ['确定', '取消']
                                , yes: function (index, layero) {
                                     layero.contents().find("#LAY-app-{{ModelClassName}}-editForm-submit").click();
                                }
                            });
                        } else {
                            layer.msg(e.msg);
                        }
                    });
			    }
                //执行预览操作
                function doDetails(obj) {
                    coreHelper.Post("Api/{{ModelClassName}}/GetDetails", { id: obj.data.id }, function (e) {
                        if (e.code === 0) {
                            admin.popup({
                                shadeClose: false,
                                title: '查看详情',
                                area: ['1200px', '90%'],
                                id: 'LAY-popup-{{ModelClassName}}-details',
                                success: function (layero, index) {
                                    view(this.id).render('base/{{ModelClassName}}/details', { data: e.data }).done(function () {
                                        form.render();
                                    });
                                    // 禁止弹窗出现滚动条
                                    $(layero).children('.layui-layer-content').css('overflow', 'visible');
                                }
                            });
                        } else {
                            layer.msg(e.msg);
                        }
                    });
                }
                //执行单个删除
                function doDelete(obj){
                    coreHelper.Post("Api/{{ModelClassName}}/DoDelete", { id: obj.data.id }, function (e) {
                            if (debug) { console.log(e); } //开启调试返回数据
                            table.reloadData('LAY-app-{{ModelClassName}}-tableBox');
                            layer.msg(e.msg);
                        });
			    }
                //执行批量删除
                function doBatchDelete(checkStatus){
                    var checkData = checkStatus.data;
                    if (checkData.length === 0) {
                        return layer.msg('请选择要删除的数据');
                    }
                    layer.confirm('确定删除吗？删除后将无法恢复。',
                        function(index) {
                            var delidsStr = [];
                            layui.each(checkData,
                                function(index, item) {
                                    delidsStr.push(item.id);
                                });
                            coreHelper.Post("Api/{{ModelClassName}}/DoBatchDelete", { id: delidsStr }, function (e) {
                                    if (debug) { console.log(e); } //开启调试返回数据
                                    table.reloadData('LAY-app-{{ModelClassName}}-tableBox');
                                    layer.msg(e.msg);
                                });
                        });
			    }
                //执行查询条件导出excel
                function doQueryExportexcel(){
                    layer.confirm('确定根据当前的查询条件导出数据吗？',
                        function(index) {
                            var field = searchwhere;
                            coreHelper.PostForm("Api/{{ModelClassName}}/QueryExportExcel", field, function (e) {
                                    if (debug) { console.log(e); } //开启调试返回数据
                                    if (e.code === 0) {
                                        window.open(e.data);
                                    } else {
                                        layer.msg(e.msg);
                                    }
                                });
                        });
			    }
                //执行选择目录导出数据
                function doSelectExportExcel(checkStatus){
                    var checkData = checkStatus.data;
                    if (checkData.length === 0) {
                        return layer.msg('请选择您要导出的数据');
                    }
                    layer.confirm('确定导出选择的内容吗？',
                        function(index) {
                            var delidsStr = [];
                            layui.each(checkData,
                                function(index, item) {
                                    delidsStr.push(item.id);
                                });
                            layer.close(index);
                            coreHelper.Post("Api/{{ModelClassName}}/SelectExportExcel", { id: delidsStr }, function (e) {
                                    if (debug) { console.log(e); } //开启调试返回数据
                                    if (e.code === 0) {
                                        window.open(e.data);
                                    } else {
                                        layer.msg(e.msg);
                                    }
                                });
                        });
			    }

                {% for field in ModelFields %}{% if field.DataType == 'datetime' %}
                laydate.render({
                    elem: '#searchTime-{{ModelClassName}}-{{field.DbColumnName}}',
                    type: 'datetime',
                    range: '到',
                });{% endif %}{% endfor %}

                //监听 表格复选框操作
                {% for field in ModelFields %}{% if field.DataType == 'bit' %}
                layui.form.on('switch(switch_{{field.DbColumnName}})', function (obj) {
                    coreHelper.Post("Api/{{ModelClassName}}/DoSet{{field.DbColumnName}}", { id: this.value, data: obj.elem.checked }, function (e) {
                        if (debug) { console.log(e); } //开启调试返回数据
                        //table.reloadData('LAY-app-{{ModelClassName}}-tableBox');
                        layer.msg(e.msg);
                    });
                });
                {% endif %}{% endfor %}

                //重载form
                form.render();
            });
    };
</script>
{% for field in ModelFields %}{% if field.DataType == 'bit' %}
<!--设置{{field.ColumnDescription}}-->
<script type="text/html" id="switch_{{field.DbColumnName}}">
    <input type="checkbox" name="switch_{{field.DbColumnName}}" value="{% raw %}{{d.id}}{% endraw %}" lay-skin="switch" lay-text="开启|关闭" lay-filter="switch_{{field.DbColumnName}}" {% raw %}{{{% endraw %} d.{{field.DbColumnName}} ? 'checked' : '' {% raw %}}}{% endraw %}>
</script>
{% endif %}{% endfor %}
