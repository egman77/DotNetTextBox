<%@Page Language="VB" ValidateRequest="false" ContentType="text/html"%>
<%@Register TagPrefix="dntb" Namespace="DotNetTextBox" Assembly="DotNetTextBox"%>
<script runat="server" language="vb">
Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) 
    If Not IsPostBack Then 
        WebEditor1.Text = "<a href=http://www.aspxcn.com.cn><img src='system_dntb/skin/xp/img/logo.gif' alt='DotNet中华网版权所有' width=260 height=60 border=0></a>" 
    End If 
End Sub 

Private Sub Button1_OnClick(ByVal sender As Object, ByVal e As System.EventArgs) 
    label1.Text = "<hr><b>以下是DotNetTextBox控件提交的内容</b>：<br>" + Server.HtmlDecode(Server.HtmlEncode(WebEditor1.Text)) 
End Sub 

Private Sub Button2_OnClick(ByVal sender As Object, ByVal e As System.EventArgs) 
    WebEditor1.Text = "" 
End Sub 
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
<title>演示:Asp.net Ajax的应用例子(C#)</title>
<style type="text/css">
<!--
body {
	font-size: 9pt;
}
hr {
	border: dotted #AFAFAF;
}
input {
	font-size: 9pt;
	border: 1px ridge #999999;
	background-color: #FFFFFF;
	cursor:pointer;
}
TABLE {
FONT: 9pt 宋体,Verdana,Arial
}
-->
</style>
</head>
<body>
    <table align=center width="555">
        <tr>
            <td valign="top" style="width: 551px">
                <fieldset>
                    <legend>演示说明</legend>&nbsp; &nbsp; 本例子演示DotNetTextBox控件应用Asp.Net 
                    Ajax(无刷新效果)的技术，通过放置在UpdatePanel控件里就简易地让DotNetText控件实现无刷新的效果!</fieldset>
            </td>
        </tr>
    </table>
    <br />
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td><form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <center>
    <dntb:WebEditor ID="WebEditor1" runat="server"/>
              <asp:Button id="Button1"  OnClick="Button1_OnClick" runat="server" Text="提交内容"></asp:Button>
          <asp:Button id="Button2"  OnClick="Button2_OnClick" runat="server" Text="清空内容"></asp:Button>
          <br /></center>
          <asp:label runat="server" ID="label1"></asp:label>
    </ContentTemplate>
    </asp:UpdatePanel>
      </form></td>
  </tr>
</table>
</body>
</html>