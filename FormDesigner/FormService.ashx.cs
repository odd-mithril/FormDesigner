using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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
            //操作行为
            string atype = context.Request["atype"];
            try
            {
                switch (atype)
                {
                    case "saveform":
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(SaveForm(context));
                        break;
                    case "previewform":
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(PreviewForm(context));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private string PreviewForm(HttpContext context)
        {
            string result = string.Empty;

            FormInfo form = new FormInfo();

            form.ContentParse = context.Request.Form["parse_form"];
            form.Id = string.IsNullOrEmpty(context.Request.Form["formid"]) ? 0 : Int32.Parse(context.Request.Form["formid"]);
            form.Action = context.Request.Form["type"];
            form = JsonConvert.DeserializeObject<FormInfo>(form.ContentParse);


            result = GetHtml(form);

            return result;
        }

        private string SaveForm(HttpContext context)
        {
            string result = string.Empty;

            FormInfo form = new FormInfo();
            try
            {
                using (FormDesignerContext dbcontext = new FormDesignerContext())
                {
                    form.ContentParse = context.Request.Form["parse_form"];
                    form.Id = string.IsNullOrEmpty(context.Request.Form["formid"]) ? 0 : Int32.Parse(context.Request.Form["formid"]);
                    form.Action = context.Request.Form["type"];
                    form = JsonConvert.DeserializeObject<FormInfo>(form.ContentParse);

                    FormInfoEntity formInfoEntity = new FormInfoEntity();
                    formInfoEntity.ContentParse = form.ContentParse;
                    dbcontext.FormInfoEntity.Add(formInfoEntity);
                    dbcontext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            result = GetHtml(form);

            return result;
        }

        #region 表单内容解析


        public static string GetHtml(FormInfo form, NameValueCollection tableData = null)
        {
            if (tableData == null) tableData = new NameValueCollection();
            string html = form.ContentParse;
            JArray dataAry = form.Data;
            int count = form.Data.Count;
            for (int i = 0; i < count; i++)
            {
                if (dataAry[i].ToString() == "") continue;

                JObject item = dataAry[i] as JObject;

                string name = "";
                string leipiplugins = dataAry[i]["leipiplugins"].ToString();
                if (leipiplugins == "checkboxs")
                    name = dataAry[i]["parse_name"].ToString();
                else
                    name = dataAry[i]["name"].ToString();

                string temp_html = "";
                string action = form.Action;
                switch (leipiplugins)
                {
                    case "text":
                        temp_html = GetTextBox(item, tableData);
                        break;
                    case "textarea":
                        temp_html = GetTextArea(item, tableData);
                        break;
                    case "radios":
                        temp_html = GetRadios(item, tableData);
                        break;
                    case "select":
                        temp_html = GetSelect(item, tableData);
                        break;
                    case "checkboxs":
                        temp_html = GetCheckboxs(item, tableData);
                        break;
                    case "macros":
                        temp_html = GetMacros(item, tableData);
                        break;
                    case "progressbar":
                        temp_html = GetProgressbar(item, tableData, action);
                        break;
                    case "qrcode"://未处理生成二维码
                        temp_html = GetQrcode(item, tableData, action);
                        break;
                    case "listctrl":
                        temp_html = GetListctrl(item, tableData, action);
                        break;
                    default:
                        temp_html = item["content"].ToString();
                        break;
                }

                html = html.Replace("{" + name + "}", temp_html);

            }
            return html;
        }

        private static string GetTextBox(JObject item, NameValueCollection formData)
        {
            string temp = "<input type=\"text\" value=\"{0}\"  name=\"{1}\"  style=\"{2}\"/>";
            string name = item["name"].ToString();

            string value = formData[name];
            if (value == null)
                value = item["value"] == null ? "" : item["value"].ToString();
            string style = item["style"] == null ? "" : item["style"].ToString();
            string temp_html = string.Format(temp, value, name, style);
            return temp_html;
        }

        private static string GetTextArea(JObject item, NameValueCollection formData)
        {
            string script = "";
            if (item["orgrich"] != null && item["orgrich"].ToString() == "1")
                script = "orgrich=\"true\" ";
            string name = item["name"].ToString();

            string value = formData[name];
            if (value == null)
                value = item["value"] == null ? "" : item["value"].ToString();
            string style = item["style"] == null ? "" : item["style"].ToString();


            string temp = "<textarea  name=\"{0}\" id=\"{1}\"  style=\"{2}\" {3}>{4}</textarea>";
            string temp_html = string.Format(temp, name, name, style, script, value);

            return temp_html;
        }

        private static string GetRadios(JObject item, NameValueCollection formData)
        {
            JArray radiosOptions = item["options"] as JArray;
            string temp = "<input type=\"radio\"  value=\"{1}\" name=\"{0}\" {2}>{3}&nbsp;";
            string temp_html = "";
            string name = item["name"].ToString();
            string value = formData[name];
            foreach (JObject op in radiosOptions)
            {
                string cvalue = op["value"].ToString();
                string Ischecked = "";
                //string cname = radiosOptions["name"].ToString();
                if (value == null)
                {
                    string check = op["checked"] != null ? op["checked"].ToString() : "";
                    if (check == "checked" || check == "true")
                        Ischecked = " checked=\"checked\" ";
                }
                else if (Ischecked == null && value != null && value == cvalue)
                    Ischecked = " checked=\"checked\" ";
                temp_html += string.Format(temp, cvalue, name, Ischecked, cvalue);
            }
            return temp_html;
        }

        private static string GetCheckboxs(JObject item, NameValueCollection formData)
        {
            string temp_html = "";
            string temp = "<input type=\"checkbox\" name=\"{0}\" value=\"{1}\" {2}>{3}&nbsp;";
            JArray checkOptions = item["options"] as JArray;

            foreach (JObject op in checkOptions)
            {
                string name = op["name"].ToString();
                string value = formData[name];
                string cvalue = op["value"].ToString();
                string Ischecked = "";
                if (value == null)
                {
                    string check = op["checked"] != null ? op["checked"].ToString() : "";
                    if (check == "checked" || check == "true")
                        Ischecked = " checked=\"checked\" ";
                }
                else if (value != null && value == cvalue)
                    Ischecked = " checked=\"checked\" ";

                temp_html += string.Format(temp, name, cvalue, Ischecked, cvalue);

            }
            return temp_html;
        }

        private static string GetSelect(JObject item, NameValueCollection formData, string action = "view")
        {

            string name = item["name"].ToString();
            string value = formData[name];

            string temp_html = item["content"].ToString();
            if (value != null)//用户设置过值
            {
                temp_html = temp_html.Replace("selected=\"selected\"", "");
                value = "value=\"" + value + "\"";
                string r = value + " selected=\"selected\"";
                temp_html = temp_html.Replace(value, r);
            }
            return temp_html;
        }

        private static string GetMacros(JObject item, NameValueCollection formData, string action = "view")
        {
            string name = item["name"].ToString();
            string value = formData[name];
            string temp_html = item["content"].ToString();

            if (value == null)
            {
                #region 制造规则值
                string type = item["orgtype"].ToString();

                string date_format = "";
                switch (type)
                {
                    case "sys_date":
                        date_format = "yyyy-MM-dd";
                        value = DateTime.Now.ToString(date_format);
                        break;
                    case "sys_date_cn":
                        date_format = "yyyy年MM月dd日";
                        value = DateTime.Now.ToString(date_format);
                        break;
                    case "sys_date_cn_short3":
                        date_format = "yyyy年";
                        value = DateTime.Now.ToString(date_format);
                        break;
                    case "sys_date_cn_short4":
                        date_format = "yyyy";
                        value = DateTime.Now.ToString(date_format);
                        break;
                    case "sys_date_cn_short1":
                        date_format = "yyyy年MM月";
                        value = DateTime.Now.ToString(date_format);
                        break;
                    case "sys_date_cn_short2":
                        date_format = "MM月dd日";
                        value = DateTime.Now.ToString(date_format);
                        break;
                    case "sys_time":
                        date_format = "HH:mm:ss";
                        value = DateTime.Now.ToString(date_format);
                        break;
                    case "sys_datetime":
                        date_format = "yyyy-MM-dd HH:mm:ss";
                        value = DateTime.Now.ToString(date_format);
                        break;
                    case "sys_week"://周
                        string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                        value = Day[Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"))].ToString();
                        break;
                    case "sys_userid":
                        //if(!$def_value)
                        //    $def_value = $controller["user"]["uid"];
                        //$tpl = str_replace("{macros}",$def_value,$tpl);
                        break;
                    case "sys_realname":
                        //if(!$def_value)
                        //    $def_value = $controller["user"]["real_name"];
                        //$tpl = str_replace("{macros}",$def_value,$tpl);
                        break;
                    default:
                        //$tpl = str_replace('{macros}','未完善的宏控件',$tpl);
                        break;
                }
                #endregion
            }
            if (action == "view")
                return value;
            return temp_html.Replace("{macros}", value);


        }

        private static string GetProgressbar(JObject item, NameValueCollection formData, string action = "view")
        {
            string name = item["name"].ToString();
            string value = formData[name];
            string temp_html = "";
            string temp = "";
            if (action == "edit")
            {
                temp = "进度 <input type=\"text\" style=\"width:40px\" name=\"{0}\" value=\"{1}\"/> %";
                if (value == null)
                    value = item["orgvalue"].ToString();
                //item['value'] = item['value']>0 ? item['value'] : float.Parse(item['orgvalue'].ToString());
                temp_html = string.Format(temp, name, value);
            }
            else if (action == "view")
            {
                temp = "<div class=\"progress progress-striped\"><div class=\"bar {0}\" style=\"width: {1}%;\"></div></div>";
                temp_html = string.Format(temp, value, value);
            }
            else if (action == "preview")
            {
                temp = "<div class=\"progress progress-striped\"><div class=\"bar {0}\" style=\"width: {1}%;\"></div></div>";
                temp_html = string.Format(temp, value, value);
            }
            return temp_html;
        }

        private static string GetQrcode(JObject item, NameValueCollection formData, string action = "view")
        {
            string name = item["name"].ToString();
            string value = formData[name];
            string temp_html = "";
            string temp = "";
            string orgType = item["orgtype"].ToString();
            string style = item["style"].ToString();
            if (orgType == "text")
            {
                orgType = "文本";
            }
            else if (orgType == "url")
            {
                orgType = "超链接";
            }
            else if (orgType == "tel")
            {
                orgType = "电话";
            }
            string qrcode_value = "";
            if (item["value"] != null)
                qrcode_value = item["value"].ToString();
            //print_R($qrcode_value);exit;  //array(value,qrcode_url)
            if (action == "edit")
            {
                temp = orgType + "二维码 <input type=\"text\" name=\"{0}\" value=\"{1}\"/>";
                temp_html = string.Format(temp, name, value);
            }
            else if (action == "view")
            {
                //可以采用  http://qrcode.leipi.org/ 

                style = "";
                if (item["orgwidth"] != null)
                {
                    style = "width:" + item["orgwidth"] + "px;";
                }
                if (item["orgheight"] != null)
                {
                    style += "height:" + item["orgheight"] + "px;";
                }
                temp = "<img src=\"{0}\" title=\"{1}\" style=\"{2}\"/>";
                temp_html = string.Format(temp_html, name, value, style);


            }
            else if (action == "preview")
            {
                style = "";
                if (item["orgwidth"] != null)
                {
                    style = "width:" + item["orgwidth"] + "px;";
                }
                if (item["orgheight"] != null)
                {
                    style += "height:" + item["orgheight"] + "px;";
                }
                temp = "<img src=\"{0}\" title=\"{1}\" style=\"{2}\"/>";
                temp_html = string.Format(temp_html, name, value, style);
            }

            return temp_html;
        }

        private static string GetListctrl(JObject item, NameValueCollection formData, string action = "view")
        {
            action = "";
            string valuetest = "{\"data_110\":[\"1\",\"2\"],\"data_111\":[\"21\",\"22\",\"22\"]}";

            string name = item["name"].ToString();
            string value = formData[name];
            string temp_html = "";
            string orgSum = item["orgsum"].ToString();
            string orgUnit = item["orgunit"].ToString();
            string orgTitle = item["orgtitle"].ToString();
            string title = item["title"].ToString();
            string style = item["style"].ToString();
            string orgcolvalue = item["orgcolvalue"].ToString();
            string orgcoltype = item["orgcoltype"].ToString();
            List<string> listTitle = new List<string>(orgTitle.Split('`'));
            List<string> listSum = new List<string>(orgSum.Split('`'));
            List<string> listUnit = new List<string>(orgUnit.Split('`'));
            List<string> listValue = new List<string>(orgcolvalue.Split('`'));
            List<string> listType = new List<string>(orgcoltype.Split('`'));
            int tdCount = listTitle.Count;


            string temp = "<table id=\"" + name + "_table\" bindTable=\"true\" cellspacing=\"0\" class=\"table table-bordered table-condensed\" style=\"" + style + "\"><thead>{0}</thead><tbody>{1}</tbody>{2}</table>";
            string btnAdd = "<span class=\"pull-right\"><button class=\"btn btn-small btn-success listAdd\" type=\"button\" tbname=\"" + name + "\"  onclick=\"tbAddRow('" + name + "')\">添加一行</button></span>"; //添加按钮
            string theader = "<tr><th colspan=\"{0}\">{1}{2}</th></tr>{3}";//头部模版

            string trTitle = "";//标题
            for (int i = 0; i < tdCount; i++)
            {
                if (i == tdCount - 1)
                    listTitle[i] = "操作";
                if (action == "view" && i == tdCount - 1) continue;//如果是查看最后一列不显示
                trTitle += string.Format("<th>{0}</th>", listTitle[i]);
            }
            trTitle = "<tr>" + trTitle + "</tr>";



            JObject dataValue = JsonConvert.DeserializeObject(valuetest) as JObject;
            int rowCount = dataValue != null ? dataValue.Count : 1;


            StringBuilder sbTr = new StringBuilder();
            string tdSum = "";//如果有统计增加一行
            SortedDictionary<int, float> SumValueDic = new SortedDictionary<int, float>();
            for (int row = 0; row < rowCount; row++)
            {
                JArray rowValue = dataValue != null ? dataValue[name + row] as JArray : null;

                string tr = "";//默认一行
                for (int i = 0; i < tdCount; i++)
                {
                    string tdname = name + "[" + i + "]";
                    string sum = listSum[i] == "1" ? "sum=\"" + tdname + "\"" : "";//是否参与统计
                    string tdValue = rowValue != null && rowValue.Count > i ? rowValue[i].ToString() : listValue[i];
                    string type = listType[i];//类型

                    if (sum != "")//一次循环计算该列的值
                    {
                        #region 计算统计值
                        float tempTdValue = 0;
                        if (SumValueDic.ContainsKey(i))
                            tempTdValue = SumValueDic[i];
                        try
                        {
                            float resultTdTemp = 0;
                            float.TryParse(tdValue, out resultTdTemp);
                            tempTdValue += resultTdTemp;
                        }
                        catch (Exception)
                        {
                            tdValue = "0";
                        }
                        if (SumValueDic.ContainsKey(i))
                            SumValueDic[i] = tempTdValue;
                        else
                            SumValueDic.Add(i, tempTdValue);
                        #endregion

                    }

                    if (i == tdCount - 1)//最后一列不显示
                    {
                        if (action == "view") continue;
                        //tr += "<td></td>";
                        else
                            tr += "<td><a href=\"javascript:void(0);\" class=\"delrow \" onclick=\"fnDeleteRow(this)\">删除</a></td>";
                        //tr += string.Format("<td><a href=\"javascript:void(0);\" class=\"delrow {0}\">删除</a></td>", dataValue != null ? "" : "hide");
                    }
                    else
                    {
                        if (action == "view")
                        {
                            tr += string.Format("<td>{0}</td>", tdValue);
                        }
                        else
                        {
                            if (type == "text")
                                tr += string.Format("<td><input class=\"input-medium\" type=\"text\" value=\"{0}\" name=\"{1}[]\" {2} onblur=\"sum_total('{1}')\"></td>", tdValue, tdname, sum);
                            else if (type == "int")
                                tr += string.Format("<td><input class=\"input-medium\" type=\"text\" value=\"{0}\" name=\"{1}[]\" {2} onblur=\"sum_total('{1}')\"></td>", tdValue, tdname, sum);
                            else if (type == "textarea")
                                tr += string.Format("<td><textarea class=\"input-medium\" name=\"{0}\" >{1}</textarea></td>", tdname, tdValue, sum);
                            else if (type == "calc")
                                tr += string.Format("<td><input class=\"input-medium\" type=\"text\" value=\"{0}\" name=\"{1}[]\" {2} ></td>", tdValue, tdname, sum);
                        }
                    }

                    if (row == 0)//统计的行只有一行
                    {
                        #region
                        if (sum != "")
                        {
                            if (action == "view")
                                tdSum += string.Format("<td>合计：value{0}{1}</td>", i, listUnit[i]);
                            else
                                tdSum += string.Format("<td>合计：<input class=\"input-small\" type=\"text\" value=\"value{0}\" name=\"{1}[total]\" {2}\">{3}</td>", i, tdname, sum, listUnit[i]);
                        }
                        else
                        {
                            tdSum += "<td></td>";
                        }
                        #endregion

                    }

                }
                sbTr.AppendFormat("<tr class=\"template\">{0}</tr>", tr);

            }

            if (!string.IsNullOrEmpty(tdSum))
            {
                foreach (int i in SumValueDic.Keys)
                    tdSum = tdSum.Replace("value" + i, SumValueDic[i].ToString());
                tdSum = string.Format("<tbody class=\"sum\"><tr>{0}</tr></tbody>", tdSum);
            }
            if (action == "view")
                theader = string.Format(theader, tdCount, title, "", trTitle);
            else
                theader = string.Format(theader, tdCount, title, btnAdd, trTitle);

            temp_html = string.Format(temp, theader, sbTr.ToString(), tdSum);

            return temp_html;
        }


        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}