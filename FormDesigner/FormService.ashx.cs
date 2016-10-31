using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormDesigner
{
    /// <summary>
    /// Summary description for FormService
    /// </summary>
    public class FormService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            FormInfo form = new FormInfo();
            form.ContentParse = context.Request.Form["parse_form"];
            form.Id = string.IsNullOrEmpty(context.Request.Form["formid"]) ? 0 : Int32.Parse(context.Request.Form["formid"]);
            form.Action = context.Request.Form["type"];
            form = JsonConvert.DeserializeObject<FormInfo>(form.ContentParse);


            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}