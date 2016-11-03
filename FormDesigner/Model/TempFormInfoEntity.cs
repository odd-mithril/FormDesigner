using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FormDesigner
{
    [Table("TempFormInfoEntity")]
    public class TempFormInfoEntity
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string ContentParse { get; set; }
    }
}