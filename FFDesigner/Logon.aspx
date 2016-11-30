<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logon.aspx.cs" Inherits="Logon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <script src="js/jquery.min.js"></script>
</head>
<body>
    <form id="form1">
        <div>
            <h3>
                <font face="Verdana">Logon Page</font>
            </h3>
            <table>
                <tr>
                    <td>Email:</td>
                    <td>
                        <input id="txtUserName" name="txtUserName" type="text" /></td>
                </tr>
                <tr>
                    <td>Password:</td>
                    <td>
                        <input id="txtUserPass" name="txtUserPass" type="password" /></td>
                </tr>
                <tr>
                    <td>Persistent Cookie:</td>
                    <td>
                        <input id="chkPersistCookie" name="chkPersistCookie" type="checkbox" />
                    </td>
                    <td></td>
                </tr>
            </table>
            <input type="button" value="Logon" id="cmdLogin" /><p></p>
        </div>
    </form>
    <script type="text/javascript">
        //将form表单的控件序列化
        $.fn.serializeObject = function () {
            var o = {};
            var a = this.serializeArray();
            $.each(a, function () {
                if (o[this.name] !== undefined) {
                    if (!o[this.name].push) {
                        o[this.name] = [o[this.name]];
                    }
                    o[this.name].push(this.value || '');
                } else {
                    o[this.name] = this.value || '';
                }
            });
            return o;
        };
        $("#cmdLogin").click(function () {
            var formdata = JSON.stringify($('#form1').serializeObject());
            $.ajax({
                type: "post",
                url: "ValidateUser.ashx?type=Validate&UName=" + $("#txtUserName").val() + "&UPwd=" + $("#txtUserPass").val() + "&PCookie=" + $("#chkPersistCookie").val(),
                contentType: 'application/json; charset=utf-8',
                dataType: 'text',
                success: function (result) {
                    console.log(result);
                    location.href = result;
                }
            });
        });
    </script>
</body>
</html>
