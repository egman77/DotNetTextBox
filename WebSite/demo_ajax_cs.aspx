<%@Page Language="C#" ValidateRequest="false" ContentType="text/html"%>
<%@Register TagPrefix="dntb" Namespace="DotNetTextBox" Assembly="DotNetTextBox"%>
<script runat="server" language="C#">
private void Page_Load(object sender, System.EventArgs e)
{
	if (!IsPostBack)
	{
		WebEditor1.Text = "<a href=http://www.aspxcn.com.cn><img src='system_dntb/skin/xp/img/logo.gif' alt='DotNet�л�����Ȩ����' width=260 height=60 border=0></a>";
	}
}

private void Button1_OnClick(object sender, System.EventArgs e)
{
	label1.Text = "<hr><b>������DotNetTextBox�ؼ��ύ������</b>��<br>"+ Server.HtmlDecode(Server.HtmlEncode(WebEditor1.Text));
}

private void Button2_OnClick(object sender, System.EventArgs e)
{
	WebEditor1.Text="";
}
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
<title>��ʾ:Asp.net Ajax��Ӧ������(C#)</title>
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
FONT: 9pt ����,Verdana,Arial
}
-->
</style>
</head>
<body>
    <table align=center width="555">
        <tr>
            <td valign="top" style="width: 551px">
                <fieldset>
                    <legend>��ʾ˵��</legend>&nbsp; &nbsp; ��������ʾDotNetTextBox�ؼ�Ӧ��Asp.Net 
                    Ajax(��ˢ��Ч��)�ļ�����ͨ��������UpdatePanel�ؼ���ͼ��׵���DotNetText�ؼ�ʵ����ˢ�µ�Ч��!</fieldset>
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
              <asp:Button id="Button1"  OnClick="Button1_OnClick" runat="server" Text="�ύ����"></asp:Button>
          <asp:Button id="Button2"  OnClick="Button2_OnClick" runat="server" Text="�������"></asp:Button>
          <br /></center>
          <asp:label runat="server" ID="label1"></asp:label>
    </ContentTemplate>
    </asp:UpdatePanel>
      </form></td>
  </tr>
</table>
</body>
</html>