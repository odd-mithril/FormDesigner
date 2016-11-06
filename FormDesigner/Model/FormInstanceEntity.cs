using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FormDesigner
{
    [Table("FormInstanceEntity")]
    public class FormInstanceEntity
    {
        #region 默认表属性
        /// <summary>
        /// 表单ID
        /// </summary>
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Created { get; set; }
        [JsonProperty]
        public string Creator { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Modified { get; set; }
        [JsonProperty]
        public string Modifier { get; set; }
        #endregion
        /// <summary>
        /// 表单名称
        /// </summary>
        [JsonProperty]
        public string FormName { get; set; }

        /// <summary>
        /// 表单描述
        /// </summary>
        [JsonProperty]
        public string FormDesc { get; set; }
        public string ContentParse { get; set; }
    }
}