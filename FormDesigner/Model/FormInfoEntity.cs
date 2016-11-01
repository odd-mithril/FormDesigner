using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FormDesigner
{
    [Table("FormInfoEntity")]
    public class FormInfoEntity
    {
        public int ID { get; set; }
        public string ContentParse { get; set; }
    }
}