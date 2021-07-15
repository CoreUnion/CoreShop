/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: {{ModelCreateTime}}
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// {{ModelDescription}}
    /// </summary>
    public partial class {{ModelClassName}}
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public {{ModelClassName}}()
        {
        }
		{% for field in ModelFields %}
        /// <summary>
        /// {{field.ColumnDescription}}
        /// </summary>
        [Display(Name = "{{field.ColumnDescription}}")]
		{% if field.IsIdentity == true and field.IsPrimarykey == true %}
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        {% elsif  field.IsIdentity == true and field.IsPrimarykey == false %}
        [SugarColumn(IsIdentity = true)]
        {% elsif  field.IsIdentity == false and field.IsPrimarykey == true %}
        [SugarColumn(IsPrimaryKey = true)]
        {% else %}{% endif %}
        {% if field.IsNullable == false %}[Required(ErrorMessage = "请输入{0}")]{% endif %}
        {% if field.DataType == 'nvarchar' and field.Length > 0 %}[StringLength(maximumLength:{{field.Length}},ErrorMessage = "{0}不能超过{1}字")]{% endif %}
        {% if field.DataType == 'varchar' and field.Length > 0 %}[StringLength(maximumLength:{{field.Length}},ErrorMessage = "{0}不能超过{1}字")]{% endif %}
        {% if field.DataType == 'nvarchar' or field.DataType == 'varchar'  or field.DataType == 'text' %}
        public System.String {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'int' and field.IsNullable == false  %}
        public System.Int32 {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'int' and field.IsNullable == true %}
        public System.Int32? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bigint' and field.IsNullable == false  %}
        public System.Int64 {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bigint' and field.IsNullable == true %}
        public System.Int64? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'float' and field.IsNullable == false  %}
        public float {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'float' and field.IsNullable == true %}
        public float? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bit' and field.IsNullable == false %}
        public System.Boolean {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bit' and field.IsNullable == true %}
        public System.Boolean? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'datetime' and field.IsNullable == false %}
        public System.DateTime {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'datetime' and field.IsNullable == true %}
        public System.DateTime? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'date' and field.IsNullable == false %}
        public System.DateTime {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'date' and field.IsNullable == true %}
        public System.DateTime? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'uniqueidentifier' and field.IsNullable == false %}
        public System.Guid {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'uniqueidentifier' and field.IsNullable == true %}
        public System.Guid? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'decimal' and field.IsNullable == false %}
        public System.Decimal {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'decimal' and field.IsNullable == true %}
        public System.Decimal? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'numeric' and field.IsNullable == false %}
        public System.Decimal {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'numeric' and field.IsNullable == true %}
        public System.Decimal? {{field.DbColumnName}}  { get; set; }
        {% else %}
        {% endif %}
		{% endfor %}
    }
}
