using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormDesigner
{
    /// <summary>
    /// 设计表单信息
    /// </summary>
    public class FormInfo
    {
        /// <summary>
        /// 表单ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 表单名称
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// 表单描述
        /// </summary>
        public string FormDesc { get; set; }

        /// <summary>
        /// 表单原html模板未经处理的
        /// </summary>
        [JsonProperty("template")]
        public string Content { get; set; }

        /// <summary>
        /// 表单替换的模板 经过处理
        /// </summary>
        [JsonProperty("parse")]
        public string ContentParse { get; set; }

        /// <summary>
        /// 表单中的字段数据 控件属性
        /// </summary>
        public string ContentData { get; set; }


        /// <summary>
        /// 控件属性
        /// </summary>
        [JsonProperty("data")]
        public JArray Data { get; set; }


        /// <summary>
        /// 字段总数
        /// </summary>
        [JsonProperty("fields")]
        public int Fields { get; set; }


        /// <summary>
        /// 新增控件
        /// </summary>
        [JsonProperty("add_fields")]
        public JObject AddFields { get; set; }


        /// <summary>
        /// 是否删除0正常1删除
        /// </summary>
        public short IsDel { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime DateLine { get; set; }

        /// <summary>
        /// 当前类型 edit view
        /// </summary>
        public string Action { get; set; }


        /// <summary>
        /// 新增控件
        /// </summary>
        public List<FormField> FormFields { get; set; }

    }
}