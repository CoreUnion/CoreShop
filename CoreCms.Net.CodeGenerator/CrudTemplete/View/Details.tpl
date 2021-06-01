<script type="text/html" template  lay-done="layui.data.done(d);">
    <table class="layui-table layui-form" lay-filter="LAY-app-{{ModelClassName}}-detailsForm" id="LAY-app-{{ModelClassName}}-detailsForm">
        <colgroup>
            <col width="100">
            <col>
        </colgroup>
        <tbody>
		    {% for field in ModelFields %}
		    <tr>
                <td>
                    <label for="{{field.DbColumnName}}">{{field.ColumnDescription}}</label>
                </td>
                <td>
                    {% if field.DataType == 'bit' %}<input type="checkbox" disabled name="{{field.DbColumnName}}" value="{% raw %}{{{% endraw %}d.params.data.{{field.DbColumnName}}{% raw %}}}{% endraw %}" lay-skin="switch" lay-text="开启|关闭" lay-filter="{{field.DbColumnName}}" {% raw %}{{{% endraw %} d.params.data.{{field.DbColumnName}} ? 'checked' : '' {% raw %}}}{% endraw %}>{% else %}{% raw %}{{{% endraw %} d.params.data.{{field.DbColumnName}} || '' {% raw %}}}{% endraw %}{% endif %}
                </td>
            </tr>
		    {% endfor %}
        </tbody>
    </table>
</script>
<script>
    var debug= layui.setter.debug;
    layui.data.done = function (d) {
        //开启调试情况下获取接口赋值数据
        if (debug) { console.log(d.params.data); }

        layui.use(['admin', 'form', 'coreHelper'], function () {
            var $ = layui.$
                , setter = layui.setter
                , admin = layui.admin
                , coreHelper = layui.coreHelper
                , form = layui.form;
            form.render(null, 'LAY-app-{{ModelClassName}}-detailsForm');
        });
    };
</script>