<%@Page Language="C#" ValidateRequest="false" ContentType="text/html"%>
<%@Register TagPrefix="dntb" Namespace="DotNetTextBox" Assembly="DotNetTextBox"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server" language="c#">
    private void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            WebEditor1.Text = "<a href=http://www.aspxcn.com.cn><img src='system_dntb/skin/default/img/logo.gif' alt='DotNet中华网版权所有' width=260 height=60 border=0></a>";
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Int32.Parse(demoselect.SelectedValue) < 9)
        {
            showdemo.ActiveViewIndex = Int32.Parse(demoselect.SelectedValue);
            switch (demoselect.SelectedValue)
            {
                case "1":
                    WebEditor1.MenuConfig = "full.config";
                    WebEditor1.Width = 655;
                    break;
                case "2":
                    Response.Cookies["uploadChildFolder"].Expires = DateTime.Now;
                    break;
                case "4":
                    lng.SelectedValue = Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"].ToLower().Split(',')[0];
                    WebEditor1.MenuConfig = "default.config";
                    break;
                case "6":
                    WebEditor2.Text = "<a href=http://www.aspxcn.com.cn><img src='system_dntb/skin/default/img/logo.gif' alt='DotNet中华网版权所有' width=260 height=60 border=0></a>";
                    break;
                case "7":
                    WebEditor1.MenuConfig = "default_style1.config";
                    WebEditor1.Width = 600;
                    break;
                case "8":
                    WebEditor1.MenuConfig = "Pageoutput.config";
                    break;
                default:
                    WebEditor1.MenuConfig = "default.config";
                    WebEditor1.Width = 590;
                    break;
            }

            WebEditor1.Text = "<a href=http://www.aspxcn.com.cn><img src='system_dntb/skin/default/img/logo.gif' alt='DotNet中华网版权所有' width=260 height=60 border=0></a>";
        }
        else
        {
            Response.Redirect("http://www.aspxcn.com.cn/demo/default.aspx");
        }
    }

    private void Button1_OnClick(object sender, System.EventArgs e)
    {
        WebEditor1.Focus = true;
        switch (showdemo.ActiveViewIndex.ToString())
        {
            case "3":
                System.Data.OleDb.OleDbConnection lj = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + HttpContext.Current.Server.MapPath("Demo.mdb"));
                System.Data.OleDb.OleDbCommand ml = new System.Data.OleDb.OleDbCommand("insert into temp(contents) values('" + Server.HtmlEncode(WebEditor1.Text.Replace("'", "''")) + "')", lj);
                lj.Open();
                ml.ExecuteNonQuery();
                System.Data.OleDb.OleDbDataAdapter dr = new System.Data.OleDb.OleDbDataAdapter("select top 1 contents from temp order by id desc", lj);
                System.Data.DataSet ly = new System.Data.DataSet();
                dr.Fill(ly);
                label1.Text = "<hr><b>以下是DotNetTextBox控件提交的内容</b>：<br>" + Server.HtmlDecode(Encoding.Unicode.GetString((byte[])ly.Tables[0].Rows[0]["contents"]));
                lj.Close();
                break;
            case "6":
                WebEditor1.Text = WebEditor2.Text;
                break;
            case "8":
                ArrayList content;
                if (selectPageOutput.SelectedItem.Value == "manual")
                {
                    content = WebEditor1.getManualPage;
                }
                else
                {
                    content = WebEditor1.getAutoPage;
                }
                if (content.Count > 1)
                {
                    for (int i = 0; i < content.Count; i++)
                    {
                        label1.Text += "<br>分页"+i.ToString()+"内容:<hr size='1' style='border-style:dashed;width:99%; border-color:#999999' align=center>" + content[i].ToString()+"<br>";        
                    }
                }
                else
                {
                    label1.Text = "<hr size='1' style='border-style:dashed;width:99%; border-color:#999999' align=center><b>以下是DotNetTextBox控件提交的内容</b>：<br>" + WebEditor1.Text;  
                }
                break;
            default:
                label1.Text = "<hr size='1' style='border-style:dashed;width:99%; border-color:#999999' align=center><b>以下是DotNetTextBox控件提交的内容</b>：<br>" + WebEditor1.Text;
                break;
        }
    }

    private void Button2_OnClick(object sender, System.EventArgs e)
    {
        WebEditor1.Text = "";
    }

    private void ChangedUser(object sender, System.EventArgs e)
    {
        Response.Cookies["uploadChildFolder"].Expires = DateTime.Now;
        //根据用户权限动态修改上传配置文件
        WebEditor1.UploadConfig = selectuser.SelectedItem.Value + ".config";
        //根据用户权限动态修改上传目录(实际应用中可根据从数据库读取到的用户名来创建目录)
        WebEditor1.UploadFolder = "upload/" + selectuser.SelectedItem.Value + "/";
        //根据用户权限动态修改菜单配置 
        WebEditor1.MenuConfig = selectuser.SelectedItem.Value + ".config";
        //根据用户权限修改控件适合的界面宽度
        switch (selectuser.SelectedItem.Value)
        {
            case "guest":
                WebEditor1.Width = 525;
                break;
            case "user":
                WebEditor1.Width = 570;
                break;
            case "administrator":
                WebEditor1.Width = 575;
                break;
            default:
                WebEditor1.Width = 590;
                break;
        }
    }

    private void ChangedLng(object sender, System.EventArgs e)
    {
        //通过下拉选择框的值来动态设置控件界面显示特定的语言 
        WebEditor1.Languages = lng.SelectedItem.Value;
    }

    private void ChangedSaveTime(object sender, System.EventArgs e)
    {
        //设置菜单工具栏位置记忆失效时间后（必须是数值类型），使用者自行拖动过菜单工具栏的位置都会使用Cookie记忆!
        //此功能将让编辑器使用者即时构建自己的个性化编辑器!
        WebEditor1.ExpireHours = Int32.Parse(savetime.SelectedItem.Value);
    }

    private void ChangedMenuStyle(object sender, System.EventArgs e)
    {
        //根据选择的值设定菜单配置文件
        WebEditor1.MenuConfig = MenuStyle.SelectedValue;
        switch (MenuStyle.SelectedValue)
        {
            case "full_style2.config":
                //为样式二设定合适宽度
                WebEditor1.Width = 575;
                break;
            case "full_style3.config":
                //为样式三设定合适宽度
                WebEditor1.Width = 610;
                break;
            default:
                //为样式一设定合适宽度
                WebEditor1.Width = 655;
                break;
        }
    }

    private void ChangeArray(object sender, System.EventArgs e)
    {
        //根据选择的值设定菜单配置文件
        WebEditor1.MenuConfig = selectArray.SelectedValue;
        //设定宽度
        WebEditor1.Width = 600;
    }

    private void ChangedSkin(object sender, System.EventArgs e)
    {
        //设置新的皮肤风格
        WebEditor1.Skin = skin.SelectedItem.Value;
    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
<title>[Aspxcn中华网工作室]DotNetTextBox在线编辑器控件演示</title>
<style>
td {  font-family:"宋体","Verdana"; font-size: 9pt}
body {
	font-size: 9pt;font-family:"宋体","Verdana";
}
table {
	font-size: 9pt;font-family:"宋体","Verdana";
}
a:link {color:#000000;text-decoration :none}
a:visited {color:#000000;text-decoration :none}
a:hover {color:#507CD1;text-decoration :none;position: relative; left: 1px; top: 1px; clip:rect(   )}
.lt0 {
	BORDER: #999 1px dotted;PADDING-RIGHT: 0px; PADDING-LEFT: 0px; BACKGROUND: #EEEEEE; PADDING-BOTTOM: 0px; cursor:pointer; PADDING-TOP: 0px; 
}
.lt1 {
	BORDER: #999999 1px solid; PADDING-RIGHT: 0px; PADDING-LEFT: 0px; BACKGROUND: #CCCCCC; PADDING-BOTTOM: 0px; cursor:pointer; PADDING-TOP: 0px;
}
</style>
</head>
<body topmargin="5" bottommargin="5" bgcolor="#EEEEEE">
    <center>
    <form id="form1" runat="server">
        <div style="z-index: 999;width: 90%;height:100%; background-color:#FFFFFF; border:dotted 1px #999999; text-align: left;">
        <table bgcolor="#006699" border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 80px">
            <tr>
                <td align="left" colspan="4" style="height: 60px; width: 801px;">
                        <img align="baseline" border="0" height="60" src="system_dntb/skin/xp/img/logo.png"
                            width="260" />
                    <span style="color: #ffffff">多功能的DHTML在线所见即所得编辑器控件(For Asp.Net2.0/3.0/3.5)</span></td>
            </tr>
        </table><div align=center style="padding-top:5px">
            <table border="0" cellpadding="0" cellspacing="0" width="99%">
                <tr align=center>
                    <td class="lt0" nowrap="nowrap" onclick="top.location.href='default.htm'"
                        onmouseenter="javascript:this.className='lt1';" onmouseleave="javascript:this.className='lt0';"
                        width="10%" style="height: 22px">
                        首页</td>
                    <td nowrap="nowrap" width="1%" style="height: 22px;color:#999999">|</td>
                    <td class="lt0" nowrap="nowrap" onclick="top.location.href='Buy.htm'"
                        onmouseenter="javascript:this.className='lt1';" onmouseleave="javascript:this.className='lt0';"
                        width="10%" style="height: 22px">
                        在线购买控件</td>
                    <td nowrap="nowrap" width="1%" style="height: 22px;color:#999999">|</td>
                    <td class="lt0" nowrap="nowrap" onclick="top.location.href='download.htm'"
                        onmouseenter="javascript:this.className='lt1';" onmouseleave="javascript:this.className='lt0';"
                        width="10%" style="height: 22px">
                        控件下载</td>
                    <td nowrap="nowrap" width="1%" style="height: 22px;color:#999999">|</td>
                    <td class="lt0" nowrap="nowrap" onclick="top.location.href='demo.aspx'"
                        onmouseenter="javascript:this.className='lt1';" onmouseleave="javascript:this.className='lt0';"
                        width="10%" style="height: 22px">
                        在线演示</td>
                    <td nowrap="nowrap" width="1%" style="height: 22px;color:#999999">|</td>
                    <td class="lt0" nowrap="nowrap" onclick="top.location.href='Faq.htm'"
                        onmouseenter="javascript:this.className='lt1';" onmouseleave="javascript:this.className='lt0';"
                        width="10%" style="height: 22px">
                        常见问题</td>
                    <td nowrap="nowrap" width="1%" style="height: 22px;color:#999999">|</td>
                    <td class="lt0" nowrap="nowrap" onclick="window.open('help.htm')"
                        onmouseenter="javascript:this.className='lt1';" onmouseleave="javascript:this.className='lt0';"
                        width="10%" style="height: 22px">
                        帮助文档</td>
                    <td nowrap="nowrap" width="1%" style="height: 22px;color:#999999">|</td>
                    <td class="lt0" nowrap="nowrap" onclick="window.open('http://www.aspxcn.com.cn')"
                        onmouseenter="javascript:this.className='lt1';" onmouseleave="javascript:this.className='lt0';"
                        width="10%" style="height: 22px">
                        反馈论坛</td>
                </tr>
            </table>
                    </div>
            <hr size="1" style="border-style:dashed;width:99%; border-color:#999999" align=center />
            &nbsp;<strong>&nbsp; [请选择控件的演示实例]：</strong><asp:DropDownList ID="demoselect" runat="server" Width="400px" AutoPostBack=true OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
            <asp:ListItem Selected=True Text="Demo:控件默认状态下常见的应用演示(C#)" Value="0"></asp:ListItem>
            <asp:ListItem Text="Demo:控件所有在线编辑功能的应用演示(C#)" Value="1"></asp:ListItem>
            <asp:ListItem Text="Demo:为每个用户单独设置上传文件夹及菜单界面的应用演示(C#)" Value="2"></asp:ListItem>
            <asp:ListItem Text="Demo:控件内容提交到数据库并读取显示的应用演示(C#)" Value="3"></asp:ListItem>
            <asp:ListItem Text="Demo:控件自适应多国语言界面及界面皮肤变换的应用演示(C#)" Value="4"></asp:ListItem>
            <asp:ListItem Text="Demo:控件记忆菜单工具栏拖曳位置的应用演示(C#)" Value="5"></asp:ListItem>
            <asp:ListItem Text="Demo:同一页面放置多个控件的应用演示(C#)" Value="6"></asp:ListItem>
            <asp:ListItem Text="Demo:控件菜单自定义排列及实时拖曳菜单工具栏的应用演示(C#)" Value="7"></asp:ListItem>
            <asp:ListItem Text="Demo:控件提交内容手动及自动分页输出的应用演示(C#)" Value="8"></asp:ListItem>
            <asp:ListItem Text="Demo:控件应用案例之简易在线留言簿(C#)" Value="9"></asp:ListItem>
            </asp:DropDownList>
            [查看VB版演示(暂无)]<br />          <div>
                <br />
                <table align="center" width="95%">
                            <tr>
                                <td valign="top">
            <asp:MultiView ID="showdemo" runat="server" ActiveViewIndex="0">
                <asp:View ID="demo1" runat="server">
                                    <fieldset>
                                        <legend>演示说明</legend>&nbsp; &nbsp;&nbsp;DotNetTextBox控件默认状态下最常见的应用实例，其中后台代码详细说明了控件通过Page_Load绑定默认内容及提交控件内容时的事件响应代码。而页面调用控件代码则演示了为控件设置初始属性值的代码，本例子通过设置skin属性来更换控件的皮肤主题为XP界面！</fieldset>
                                    <br />
                                    页面声明：<br />
                                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                        <div>
                                            <span style="color: #0000ff">&lt;</span><span style="color: #800000">%@Register
                                                        </span><span style="color: #ff0000">TagPrefix</span><span style="color: #0000ff">="dntb"</span><span
                                                            style="color: #ff0000"> Namespace</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                style="color: #ff0000"> Assembly</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                    style="color: #ff0000">%</span><span style="color: #0000ff">&gt;</span></div>
                                    </div>
                                    <br />
                                    后台代码：<br />
                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                        background-color: #eeeeee">
                        <div style="font-family: Courier New">
                                            &lt;script runat=<span style="color: maroon">"server"</span> language=<span style="color: maroon">"c#"</span>&gt;&nbsp;
                            <br />
                            &nbsp; &nbsp; <span style="color: blue">private</span> <span style="color: blue">void</span>
                            Page_Load(<span style="color: blue">object</span> sender, System.EventArgs e)&nbsp;
                            <br />
                            &nbsp; &nbsp; {
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">if</span> (!IsPostBack)<br />
                            &nbsp; &nbsp; &nbsp; &nbsp; {
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//设置控件首次加载时绑定的默认内容，如无需绑定默认内容则不需要设置此值即可(默认为空内容)!</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Text = <span style="color: maroon">
                                                "&lt;a href=http://www.aspxcn.com.cn&gt;&lt;img src='system_dntb/skin/xp/img/logo.gif'
                                alt='DotNet中华网版权所有' width=260 height=60 border=0&gt;&lt;/a&gt;"</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; }
                            <br />
                            &nbsp; &nbsp; }
                            <br />
                            &nbsp; &nbsp; <span style="color: blue">private</span> <span style="color: blue">void</span>
                            Button1_OnClick(<span style="color: blue">object</span> sender, System.EventArgs
                            e)
                            <br />
                            &nbsp; &nbsp; {
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//将当前编辑器控件的内容显示到Label控件里，实际应用中可以将内容在此提交到数据库中保存&nbsp;</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; label1.Text = <span style="color: maroon">"&lt;hr size='1'
                                style='border-style:dashed;width:99%; border-color:#999999' align=center&gt;&lt;b&gt;以下是DotNetTextBox控件提交的内容&lt;/b&gt;：&lt;br&gt;"</span>
                            + WebEditor1.Text;
                            <br />
                            &nbsp; &nbsp; }
                            <br />
                            &nbsp; &nbsp; <span style="color: blue">private</span> <span style="color: blue">void</span>
                            Button2_OnClick(<span style="color: blue">object</span> sender, System.EventArgs
                            e)
                            <br />
                            &nbsp; &nbsp; {
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//清空当前编辑器控件的内容</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Text = <span style="color: maroon">""</span>;
                            <br />
                            &nbsp; &nbsp; }
                            <br />
                                            &lt;/script&gt;</div>
                    </div>
                    <br />
                                    前台代码：<br />
                                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                        <div>
                                            <span style="color: #0000ff"><span style="color: #008000">//初始化控件，可在此初始化一些控件的属性值，如要设置控件为普通界面，则设置为Skin="skin/default/"</span><br />
                                                &lt;</span><span style="color: #800000">DNTB:WebEditor
                                            </span><span style="color: #ff0000">id</span><span style="color: #0000ff">="WebEditor1"
                                                <span style="color: #ff0000">Skin</span><span style="color: #0000ff">="skin/xp/"</span></span><span
                                                    style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                        style="color: #0000ff">&gt;&lt;/</span><span style="color: #800000">DNTB:WebEditor</span><span
                                                            style="color: #0000ff">&gt;</span></div>
                                    </div>
                                    <br /><hr size="1" style="border-style:dashed;border-color:#999999" align=center />
                                    页面实际使用效果的演示：<br /><br />
                </asp:View>
                <asp:View ID="demo2" runat="server">
                                <fieldset>
                                    <legend>演示说明</legend>&nbsp; &nbsp; DotNetTextBox控件所有在线编辑功能的应用实例，用户可通过配置system_dntb/menuconfig目录下的full.config文件来随心所欲排列及增减各种控件内置的编辑功能，也可以调用插件参数来增加外部插件(plugin)功能！</fieldset>
                                <br />
                                页面声明：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">&lt;</span><span style="color: #800000">%@Register </span>
                                        <span style="color: #ff0000">TagPrefix</span><span style="color: #0000ff">="dntb"</span><span
                                                            style="color: #ff0000"> Namespace</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                style="color: #ff0000"> Assembly</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                    style="color: #ff0000">%</span><span style="color: #0000ff">&gt;</span></div>
                                </div>
                    <br />
                    后台代码：<br />
                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                        background-color: #eeeeee">
                        <div style="font-family: Courier New">
                            &nbsp; &nbsp; <span style="color: blue">private</span> <span style="color: blue">void</span>
                            ChangedMenuStyle(<span style="color: blue">object</span> sender, System.EventArgs
                            e)
                            <br />
                            &nbsp; &nbsp; {
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//根据选择的值设定菜单配置文件</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.MenuConfig = MenuStyle.SelectedValue;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">switch</span> (MenuStyle.SelectedValue)
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; {
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">case</span>
                            <span style="color: maroon">"full_style2.config"</span>:
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//为样式二设定合适宽度</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Width = <span
                                style="color: maroon">575</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">break</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">case</span>
                            <span style="color: maroon">"full_style3.config"</span>:
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//为样式三设定合适宽度</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Width = <span
                                style="color: maroon">610</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">break</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">default</span>:
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//为样式一设定合适宽度</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Width = <span
                                style="color: maroon">655</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">break</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; }
                            <br />
                            &nbsp; &nbsp; }</div>
                    </div>
                                <br />
                                前台代码：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">
                                            <span style="color: #008000">
                                                <div>
                                                    <span style="color: #000000"><span style="color: #008000">//样式选择的下拉选择框<br />
                                                    </span>[选择菜单样式]：</span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:dropdownlist
                                                    </span><span style="color: #ff0000">id</span><span style="color: #0000ff">="MenuStyle"</span><span
                                                        style="color: #ff0000"> AutoPostBack</span><span style="color: #0000ff">="true"</span><span
                                                            style="color: #ff0000"> OnSelectedIndexChanged</span><span style="color: #0000ff">="ChangedMenuStyle"</span><span
                                                                style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                                    style="color: #0000ff">&gt;</span><span style="color: #000000"><br />
                                                                    </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                                    </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="full.config"</span><span
                                                                        style="color: #0000ff">&gt;</span><span style="color: #000000">全功能样式一</span><span style="color: #0000ff">&lt;/</span><span
                                                                            style="color: #800000">asp:ListItem</span><span style="color: #0000ff">&gt;</span><span
                                                                                style="color: #000000"><br />
                                                                            </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                                            </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="full_style2.config"</span><span
                                                                                style="color: #0000ff">&gt;</span><span style="color: #000000">全功能样式二</span><span style="color: #0000ff">&lt;/</span><span
                                                                                    style="color: #800000">asp:ListItem</span><span style="color: #0000ff">&gt;</span><span
                                                                                        style="color: #000000"><br />
                                                                                    </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                                                    </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="full_style3.config"</span><span
                                                                                        style="color: #0000ff">&gt;</span><span style="color: #000000">全功能样式三</span><span style="color: #0000ff">&lt;/</span><span
                                                                                            style="color: #800000">asp:ListItem</span><span style="color: #0000ff">&gt;</span><span
                                                                                                style="color: #000000"><br />
                                                                                            </span><span style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:DropDownList</span><span
                                                                                                style="color: #0000ff">&gt;<br />
                                                                                                ......</span></div>
                                                //初始化控件，并且初始化菜单配置文件为full.config(全部在线编辑功能的配置,放置在system_dntb/menuconfig/下)</span><br />
                                            </span><span
                                                            style="color: #0000ff">
                                                <div>
                                                    <span style="color: #0000ff">&lt;</span><span style="color: #800000">DNTB:WebEditor
                                                    </span><span style="color: #ff0000">id</span><span style="color: #0000ff">="WebEditor1"</span><span
                                                            style="color: #ff0000"> MenuConfig</span><span style="color: #0000ff">="full.config"</span><span
                                                                style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                                    style="color: #0000ff">&gt;&lt;/</span><span style="color: #800000">DNTB:WebEditor</span><span
                                                                        style="color: #0000ff">&gt;</span></div>
                                            </span></div>
                                </div>
                                <br />
                                <hr size="1" style="border-style:dashed;border-color:#999999" align=center />
                                页面实际使用效果的演示：<br />
                    <br />
                    <br />
                    <center>
                        [选择菜单样式]：<asp:dropdownlist id="MenuStyle" AutoPostBack="true" OnSelectedIndexChanged="ChangedMenuStyle" runat="server">
                            <asp:ListItem Value="full.config">全功能样式一</asp:ListItem>
                            <asp:ListItem Value="full_style2.config">全功能样式二</asp:ListItem>
                            <asp:ListItem Value="full_style3.config">全功能样式三</asp:ListItem>
                        </asp:DropDownList></center>
                </asp:View><asp:View ID="demo3" runat="server">
                    <fieldset>
                        <legend>演示说明</legend>&nbsp; &nbsp;&nbsp;为每个用户单独设置上传文件夹及菜单界面的应用演示，通过设置MenuConfig属性(实现不同权限用户显示不同的菜单界面)、设置UploadFolder属性(实现为不同权限用户创建不同的上传文件夹)、设置UploadConfig属性(实现不同权限的用户不同的上传权限)，实际应用中可以结合自己项目的用户信息来动态配置这些属性！</fieldset>
                    <br />
                    页面声明：<br />
                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                        background-color: #eeeeee">
                        <div>
                            <span style="color: #0000ff">
                                <span style="color: #0000ff"></span>&lt;</span><span style="color: #800000">%@Register
                                </span><span style="color: #ff0000">TagPrefix</span><span style="color: #0000ff">="dntb"</span><span
                                    style="color: #ff0000"> Namespace</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                        style="color: #ff0000"> Assembly</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                            style="color: #ff0000">%</span><span style="color: #0000ff">&gt;</span></div>
                    </div>
                    <div>
                    </div>
                    <br />
                    后台代码：<br />
                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                        background-color: #eeeeee">
                        <div style="font-family: Courier New">
                            &nbsp; &nbsp; <span style="color: blue">private</span> <span style="color: blue">void</span>
                            ChangedUser(<span style="color: blue">object</span> sender, System.EventArgs e)
                            <br />
                            &nbsp; &nbsp; {
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//根据用户权限动态修改上传配置文件</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.UploadConfig = selectuser.SelectedItem.Value
                            + <span style="color: maroon">".config"</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//根据用户权限动态修改上传目录(实际应用中可根据从数据库读取到的用户名来创建目录)</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.UploadFolder = <span style="color: maroon">
                                "upload/"</span> + selectuser.SelectedItem.Value + <span style="color: maroon">"/"</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//根据用户权限动态修改菜单配置&nbsp;</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.MenuConfig = selectuser.SelectedItem.Value
                            + <span style="color: maroon">".config"</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//根据用户权限修改控件适合的界面宽度</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">switch</span> (selectuser.SelectedItem.Value)
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; {
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">case</span>
                            <span style="color: maroon">
                                "guest"</span>:
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Width = <span
                                style="color: maroon">525</span>;<br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">break</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">case</span>
                            <span style="color: maroon">
                                "user"</span>:
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Width = <span
                                style="color: maroon">570</span>;<br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">break</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">case</span>
                            <span style="color: maroon">
                                "administrator"</span>:
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Width = <span
                                style="color: maroon">575</span>;<br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">break</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">default</span>:
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Width = <span
                                style="color: maroon">590</span>;<br />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">break</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; }
                            <br />
                            &nbsp; &nbsp; }</div>
                    </div>
                    <br />
                    前台代码：<br />
                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                        background-color: #eeeeee">
                        <div>
                            <span style="color: #0000ff">
                                <div>
                                    <span style="color: #000000"><span style="color: #008000">//用户权限选择的下拉选择框</span><br />
                                        <div>
                                            <span style="color: #000000">[切换用户]：</span><span style="color: #ff0000">&amp;nbsp;</span><span
                                                style="color: #0000ff">&lt;</span><span style="color: #800000">asp:dropdownlist </span>
                                            <span style="color: #ff0000">id</span><span style="color: #0000ff">="selectuser"</span><span
                                                style="color: #ff0000"> AutoPostBack</span><span style="color: #0000ff">="true"</span><span
                                                    style="color: #ff0000"> OnSelectedIndexChanged</span><span style="color: #0000ff">="ChangedUser"</span><span
                                                        style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                            style="color: #ff0000"> Width</span><span style="color: #0000ff">="86px"</span><span
                                                                style="color: #0000ff">&gt;</span><span style="color: #000000"><br />
                                                                </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:listitem
                                                                </span><span style="color: #ff0000">value</span><span style="color: #0000ff">="default"</span><span
                                                                    style="color: #0000ff">&gt;</span><span style="color: #000000">默认用户</span><span style="color: #0000ff">&lt;/</span><span
                                                                        style="color: #800000">asp:listitem</span><span style="color: #0000ff">&gt;</span><span
                                                                            style="color: #000000"><br />
                                                                        </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:listitem
                                                                        </span><span style="color: #ff0000">value</span><span style="color: #0000ff">="administrator"</span><span
                                                                            style="color: #0000ff">&gt;</span><span style="color: #000000">管理员</span><span style="color: #0000ff">&lt;/</span><span
                                                                                style="color: #800000">asp:listitem</span><span style="color: #0000ff">&gt;</span><span
                                                                                    style="color: #000000"><br />
                                                                                </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:listitem
                                                                                </span><span style="color: #ff0000">value</span><span style="color: #0000ff">="user"</span><span
                                                                                    style="color: #0000ff">&gt;</span><span style="color: #000000">普通用户</span><span style="color: #0000ff">&lt;/</span><span
                                                                                        style="color: #800000">asp:listitem</span><span style="color: #0000ff">&gt;</span><span
                                                                                            style="color: #000000"><br />
                                                                                        </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:listitem
                                                                                        </span><span style="color: #ff0000">value</span><span style="color: #0000ff">="guest"</span><span
                                                                                            style="color: #0000ff">&gt;</span><span style="color: #000000">访客</span><span style="color: #0000ff">&lt;/</span><span
                                                                                                style="color: #800000">asp:listitem</span><span style="color: #0000ff">&gt;</span><span
                                                                                                    style="color: #000000"><br />
                                                                                                </span><span style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:dropdownlist</span><span
                                                                                                    style="color: #0000ff">&gt;</span></div>
                                    </span><span style="color: #0000ff"></span>
                                </div>
                                .......<br />
                                &lt;</span><span style="color: #800000">DNTB:WebEditor </span><span style="color: #ff0000">
                                    id</span><span style="color: #0000ff">="WebEditor1"</span><span style="color: #ff0000">
                                            runat</span><span style="color: #0000ff">="server"</span><span style="color: #0000ff">&gt;&lt;/</span><span
                                                style="color: #800000">DNTB:WebEditor</span><span style="color: #0000ff">&gt;</span></div>
                    </div>
                    <br />
                    <hr align="center" size="1" style="border-left-color: #999999; border-bottom-color: #999999;
                        border-top-style: dashed; border-top-color: #999999; border-right-style: dashed;
                        border-left-style: dashed; border-right-color: #999999; border-bottom-style: dashed" />
                    页面实际使用效果的演示：<br />
                    <br />
                    <center>
                        <br />
                        [切换用户]：&nbsp;<asp:DropDownList ID="selectuser" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ChangedUser" Width="86px">
                            <asp:ListItem Value="default">默认用户</asp:ListItem>
                            <asp:ListItem Value="administrator">管理员</asp:ListItem>
                            <asp:ListItem Value="user">普通用户</asp:ListItem>
                            <asp:ListItem Value="guest">访客</asp:ListItem>
                        </asp:DropDownList></center>

            </asp:View>
                <asp:View ID="demo4" runat="server">
                                <fieldset>
                                    <legend>演示说明</legend> &nbsp;&nbsp;&nbsp; DotNetTextBox控件提交内容到数据库并读取显示的应用实例,需要注意的是,提交到数据库的内容必须使用Server.HtmlEncode()进行HTML编码,否则如果内容里有非法字符的时候会导致插入数据库失败,另外在读取数据库内容的时候,必须使用Server.HtmlDecode()解码显示(本例子使用的ACCESS数据库名为:demo.mdb)!</fieldset>
                                <br />
                                页面声明：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff"><span style="color: #008000">//禁用页面验证,避免控件内容提交时出现危险提示,也可在web.config中作全局设置</span><br />
                                            &lt;<span style="color: #800000">%@Page </span><span style="color: #ff0000">Language</span><span
                                                style="color: #0000ff">="C#"</span><span style="color: #ff0000"> ValidateRequest</span><span
                                                    style="color: #0000ff">="false"</span><span style="color: #ff0000"> ContentType</span><span
                                                        style="color: #0000ff">="text/html"</span><span style="color: #ff0000">%</span><span
                                                            style="color: #0000ff">&gt;</span><br />
                                            &lt;</span><span style="color: #800000">%@Register </span>
                                        <span style="color: #ff0000">TagPrefix</span><span style="color: #0000ff">="dntb"</span><span
                                                            style="color: #ff0000"> Namespace</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                style="color: #ff0000"> Assembly</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                    style="color: #ff0000">%</span><span style="color: #0000ff">&gt;</span></div>
                                </div>
                                <br />
                                后台代码：<br />
                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                        background-color: #eeeeee">
                        <div style="font-family: Courier New">
                            <span style="color: blue">private</span> <span style="color: blue">void</span> Button1_OnClick(<span
                                style="color: blue">object</span> sender, System.EventArgs e)
                            <br />
                            {
                            <br />
                            &nbsp; &nbsp; <span style="color: green">//连接数据库</span>
                            <br />
                            &nbsp; &nbsp; System.Data.OleDb.OleDbConnection lj = <span style="color: blue">new</span>
                            System.Data.OleDb.OleDbConnection(<span style="color: maroon">"Provider=Microsoft.Jet.OleDb.4.0;Data
                                Source="</span> + HttpContext.Current.Server.MapPath(<span style="color: maroon">"Demo.mdb"</span>));
                            <br />
                            &nbsp; &nbsp; <span style="color: green">//特别注意,为了安全需要，这里要用HtmlEncode编码后插入数据库</span>
                            <br />
                            &nbsp; &nbsp; System.Data.OleDb.OleDbCommand ml = <span style="color: blue">new</span>
                            System.Data.OleDb.OleDbCommand(<span style="color: maroon">"insert into temp(contents)
                                values('"</span> + Server.HtmlEncode(WebEditor1.Text.Replace(<span style="color: maroon">"'"</span>,
                            <span style="color: maroon">"''"</span>)) + <span style="color: maroon">"')"</span>,
                            lj);
                            <br />
                            &nbsp; &nbsp; lj.Open();
                            <br />
                            &nbsp; &nbsp; <span style="color: green">//插入内容到数据库</span>
                            <br />
                            &nbsp; &nbsp; ml.ExecuteNonQuery();&nbsp;<br />
                            &nbsp; &nbsp; <span style="color: green">//读取数据库</span>
                            <br />
                            &nbsp; &nbsp; System.Data.OleDb.OleDbDataAdapter dr = <span style="color: blue">new</span>
                            System.Data.OleDb.OleDbDataAdapter(<span style="color: maroon">"select top 1 contents
                                from temp order by id desc"</span>, lj);
                            <br />
                            &nbsp; &nbsp; System.Data.DataSet ly = <span style="color: blue">new</span> System.Data.DataSet();
                            <br />
                            &nbsp; &nbsp; dr.Fill(ly);&nbsp;<br />
                            &nbsp; &nbsp; <span style="color: green">//特别注意,这里要用HtmlDecode解码还原后的二进制数据并显示</span>
                            <br />
                            &nbsp; &nbsp; label1.Text = <span style="color: maroon">"&lt;hr&gt;&lt;b&gt;以下是DotNetTextBox控件提交的内容&lt;/b&gt;：&lt;br&gt;"</span>
                            + Server.HtmlDecode(Encoding.Unicode.GetString((<span style="color: blue">byte</span>[])ly.Tables[<span
                                style="color: maroon">0</span>].Rows[<span style="color: maroon">0</span>][<span
                                    style="color: maroon">"contents"</span>]));&nbsp;<br />
                            &nbsp; &nbsp; lj.Close();&nbsp;<br />
                            }</div>
                    </div>
                    <br />
                                前台代码：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">
                                            &lt;</span><span style="color: #800000">DNTB:WebEditor </span><span style="color: #ff0000">
                                                id</span><span style="color: #0000ff">="WebEditor1"</span><span
                                                    style="color: #ff0000">
                                                        runat</span><span style="color: #0000ff">="server"</span><span
                                                        style="color: #0000ff">&gt;&lt;/</span><span style="color: #800000">DNTB:WebEditor</span><span
                                                            style="color: #0000ff">&gt;</span></div>
                                </div>
                                <br />
                                <hr size="1" style="border-style:dashed;border-color:#999999" align=center />
                                页面实际使用效果的演示：<br />
                    <br />
                </asp:View>
                <asp:View ID="demo5" runat="server">
                                <fieldset>
                                    <legend>演示说明</legend>&nbsp; &nbsp;&nbsp;控件自适应多国语言界面及界面皮肤变换的应用演示，控件默认采用检测客户端浏览器语言的方式自动匹配界面语言，如无法找到合适的语言文件则默认使用英文界面，也可以动态设置Languages属性来强制显示特定的语言（见后台代码）！</fieldset>
                                <br />
                                页面声明：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">&lt;</span><span style="color: #800000">%@Register
                                                    </span><span style="color: #ff0000">TagPrefix</span><span style="color: #0000ff">="dntb"</span><span
                                                            style="color: #ff0000"> Namespace</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                style="color: #ff0000"> Assembly</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                    style="color: #ff0000">%</span><span style="color: #0000ff">&gt;</span></div>
                                </div>
                        <br />
                                后台代码：<div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                    padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                    width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                    background-color: #eeeeee">
                                    <div style="font-family: Courier New">
                                        <div style="font-family: Courier New">
                                            &nbsp; &nbsp; <span style="color: blue">private</span> <span style="color: blue">void</span>
                                            ChangedSkin(<span style="color: blue">object</span> sender, System.EventArgs e)
                                            <br />
                                            &nbsp; &nbsp; {
                                            <br />
                                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//设置新的皮肤风格</span>
                                            <br />
                                            &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Skin = skin.SelectedItem.Value;
                                            <br />
                                            &nbsp; &nbsp; }<br />
                                            <div style="font-family: Courier New">
                                                &nbsp; &nbsp; <span style="color: blue">private</span> <span style="color: blue">void</span>
                                                ChangedLng(<span style="color: blue">object</span> sender, System.EventArgs e)
                                                <br />
                                                &nbsp; &nbsp; {
                                                <br />
                                                &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//通过下拉选择框的值来动态设置控件界面显示特定的语言&nbsp;</span>
                                                <br />
                                                &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Languages = lng.SelectedItem.Value;
                                                <br />
                                                &nbsp; &nbsp; }</div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                前台代码：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">
                                            <div>
                                                <span style="color: #000000"><span style="color: #008000">//语言选择的下拉选择框(控件自带简繁英三种语言界面)</span><br />
                                                    [语言选择]：</span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:dropdownlist
                                                    </span><span style="color: #ff0000">id</span><span style="color: #0000ff">="lng"</span><span
                                                        style="color: #ff0000"> AutoPostBack</span><span style="color: #0000ff">="true"</span><span
                                                            style="color: #ff0000"> OnSelectedIndexChanged</span><span style="color: #0000ff">="ChangedLng"</span><span
                                                                style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                                    style="color: #0000ff">&gt;</span><span style="color: #000000"><br />
                                                                    </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:listitem
                                                                    </span><span style="color: #ff0000">value</span><span style="color: #0000ff">="en-us"</span><span
                                                                        style="color: #0000ff">&gt;</span><span style="color: #000000">英文</span><span style="color: #0000ff">&lt;/</span><span
                                                                            style="color: #800000">asp:listitem</span><span style="color: #0000ff">&gt;</span><span
                                                                                style="color: #000000"><br />
                                                                            </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:listitem
                                                                            </span><span style="color: #ff0000">value</span><span style="color: #0000ff">="zh-cn"</span><span
                                                                                style="color: #0000ff">&gt;</span><span style="color: #000000">简体中文</span><span style="color: #0000ff">&lt;/</span><span
                                                                                    style="color: #800000">asp:listitem</span><span style="color: #0000ff">&gt;</span><span
                                                                                        style="color: #000000"><br />
                                                                                    </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:listitem
                                                                                    </span><span style="color: #ff0000">value</span><span style="color: #0000ff">="zh-tw"</span><span
                                                                                        style="color: #0000ff">&gt;</span><span style="color: #000000">繁体中文</span><span style="color: #0000ff">&lt;/</span><span
                                                                                            style="color: #800000">asp:listitem</span><span style="color: #0000ff">&gt;</span><span
                                                                                                style="color: #000000"><br />
                                                                                            </span><span style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:dropdownlist</span><span
                                                                                                style="color: #0000ff">&gt;<br />
                                                                                                <span style="color: #008000">//皮肤选择的下拉选择框</span><br />
                                                                                                <div>
                                                                                                    <span style="color: #000000">[控件皮肤选择]：</span><span style="color: #0000ff">&lt;</span><span
                                                                                                        style="color: #800000">asp:DropDownList </span><span style="color: #ff0000">ID</span><span
                                                                                                            style="color: #0000ff">="skin"</span><span style="color: #ff0000"> runat</span><span
                                                                                                                style="color: #0000ff">="server"</span><span style="color: #ff0000"> AutoPostBack</span><span
                                                                                                                    style="color: #0000ff">="true"</span><span style="color: #ff0000"> OnSelectedIndexChanged</span><span
                                                                                                                        style="color: #0000ff">="ChangedSkin"</span><span style="color: #0000ff">&gt;</span><span
                                                                                                                            style="color: #000000"><br />
                                                                                                                        </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                                                                                        </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="skin/xp/"</span><span
                                                                                                                            style="color: #0000ff">&gt;</span><span style="color: #000000">XP风格</span><span style="color: #0000ff">&lt;/</span><span
                                                                                                                                style="color: #800000">asp:ListItem</span><span style="color: #0000ff">&gt;</span><span
                                                                                                                                    style="color: #000000"><br />
                                                                                                                                </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                                                                                                </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="skin/default/"</span><span
                                                                                                                                    style="color: #0000ff">&gt;</span><span style="color: #000000">标准风格</span><span style="color: #0000ff">&lt;/</span><span
                                                                                                                                        style="color: #800000">asp:ListItem</span><span style="color: #0000ff">&gt;</span><span
                                                                                                                                            style="color: #000000"><br />
                                                                                                                                        </span><span style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:DropDownList</span><span
                                                                                                                                            style="color: #0000ff">&gt;</span></div>
                                                                                            </span></div>
                                            .......<br />
                                            <span style="color: #008000">//初始化控件，也可在此初始化Languages属性值为特定语言即可强制让控件显示特定语言(如设置为:Languages="en-us")</span><br />
                                            &lt;</span><span style="color: #800000">DNTB:WebEditor
                                        </span><span style="color: #ff0000">id</span><span style="color: #0000ff">="WebEditor1"</span><span
                                                    style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                        style="color: #0000ff">&gt;&lt;/</span><span style="color: #800000">DNTB:WebEditor</span><span
                                                            style="color: #0000ff">&gt;</span></div>
                                </div>
                                <br />
                                <hr size="1" style="border-style:dashed;border-color:#999999" align=center />
                                页面实际使用效果的演示：<br />
                    <br />
                                <br />        
                                <center>
[语言选择]：<asp:dropdownlist id="lng" AutoPostBack="true" OnSelectedIndexChanged="ChangedLng" runat="server">
<asp:listitem value="en-us">英文</asp:listitem>
<asp:listitem value="zh-cn">简体中文</asp:listitem>
<asp:listitem value="zh-tw">繁体中文</asp:listitem>
</asp:dropdownlist>&nbsp;[控件皮肤选择]：<asp:DropDownList ID="skin" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ChangedSkin">
                                        <asp:ListItem Value="skin/xp/">XP风格</asp:ListItem>
                                        <asp:ListItem Value="skin/default/">标准风格</asp:ListItem>
                                    </asp:DropDownList></center>
                </asp:View>
                <asp:View ID="demo6" runat="server">
                                <fieldset>
                                    <legend>演示说明</legend>&nbsp; &nbsp;&nbsp;DotNetTextBox控件记忆菜单工具栏拖曳位置的应用演示，通过设置控件的ExpireHours属性来指定可拖曳菜单工具栏位置记忆的失效时间，控件默认失效时间为0（即关闭浏览器窗口后马上失效），此功能将让每个使用者可视化构建自己的个性化在线编辑器!当设定一个以小时为单位的数值后（如设定了ExpireHours=1），编辑器控件可拖曳菜单工具栏位置的变化在一小时内都会被保留并重现！</fieldset>
                                <br />
                                页面声明：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">&lt;</span><span style="color: #800000">%@Register </span>
                                        <span style="color: #ff0000">TagPrefix</span><span style="color: #0000ff">="dntb"</span><span
                                                            style="color: #ff0000"> Namespace</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                style="color: #ff0000"> Assembly</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                    style="color: #ff0000">%</span><span style="color: #0000ff">&gt;</span></div>
                                </div>
                                <br />
                                后台代码：<br />
                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                        background-color: #eeeeee">
                        <div style="font-family: Courier New">
                            &nbsp; &nbsp; <span style="color: blue">private</span> <span style="color: blue">void</span>
                            ChangedSaveTime(<span style="color: blue">object</span> sender, System.EventArgs
                            e)
                            <br />
                            &nbsp; &nbsp; {
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//设置菜单工具栏位置记忆失效时间后（必须是数值类型），每个使用者自行拖动过菜单工具栏的位置都会使用Cookie记忆!</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//此功能将让每个使用者可视化构建自己的个性化编辑器!</span>&nbsp;<br />
                            &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.ExpireHours = Int32.Parse(savetime.SelectedItem.Value);
                            <br />
                            &nbsp; &nbsp; }</div>
                    </div>
                    <br />
                                前台代码：<br />
                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                        background-color: #eeeeee">
                        <div>
                            <span style="color: #000000"><span style="color: #008000">//菜单位置保存时间的下拉选择框，时间设定只对当前页面里的控件生效</span>
                                <br />
                                [菜单位置保存时间(小时)]：</span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:dropdownlist
                                </span><span style="color: #ff0000">id</span><span style="color: #0000ff">="savetime"</span><span
                                    style="color: #ff0000"> AutoPostBack</span><span style="color: #0000ff">="true"</span><span
                                        style="color: #ff0000"> OnSelectedIndexChanged</span><span style="color: #0000ff">="ChangedSaveTime"</span><span
                                            style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                style="color: #0000ff">&gt;</span><span style="color: #000000"><br />
                                                </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="0"</span><span
                                                    style="color: #0000ff">&gt;</span><span style="color: #000000">立刻失效</span><span style="color: #0000ff">&lt;/</span><span
                                                        style="color: #800000">asp:ListItem</span><span style="color: #0000ff">&gt;</span><span
                                                            style="color: #000000"><br />
                                                        </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                        </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="1"</span><span
                                                            style="color: #0000ff">&gt;</span><span style="color: #000000">1小时</span><span style="color: #0000ff">&lt;/</span><span
                                                                style="color: #800000">asp:ListItem</span><span style="color: #0000ff">&gt;</span><span
                                                                    style="color: #000000"><br />
                                                                </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                                </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="24"</span><span
                                                                    style="color: #0000ff">&gt;</span><span style="color: #000000">24小时</span><span style="color: #0000ff">&lt;/</span><span
                                                                        style="color: #800000">asp:ListItem</span><span style="color: #0000ff">&gt;</span><span
                                                                            style="color: #000000"><br />
                                                                        </span><span style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:DropDownList</span><span
                                                                            style="color: #0000ff">&gt;</span><span style="color: #000000"><br />
                                                                                .......<br />
                                                                            </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">DNTB:WebEditor
                                                                            </span><span style="color: #ff0000">id</span><span style="color: #0000ff">="WebEditor1"</span><span
                                                                                style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                                                    style="color: #0000ff">&gt;&lt;/</span><span style="color: #800000">DNTB:WebEditor</span><span
                                                                                        style="color: #0000ff">&gt;</span></div>
                    </div>
                                <br />
                                <hr size="1" style="border-style:dashed;border-color:#999999" align=center />
                                页面实际使用效果的演示：<br />
                    <br />
                                <br />
                                <center>
                                    [菜单位置保存时间(小时)]：<asp:dropdownlist id="savetime" AutoPostBack="true" OnSelectedIndexChanged="ChangedSaveTime" runat="server">
                                        <asp:ListItem Value="0">立刻失效</asp:ListItem>
                                        <asp:ListItem Value="1">1小时</asp:ListItem>
                                        <asp:ListItem Value="24">24小时</asp:ListItem>
                                    </asp:DropDownList></center>
                </asp:View>
                <asp:View ID="demo7" runat="server">
                                <fieldset>
                                    <legend>演示说明</legend>&nbsp; &nbsp;&nbsp;使用两个以上的DotNetTextBox控件的多控件应用演示，演示中使用者点击提交按键后控件2的内内容将会传送到控件1之中,另外要注意的是页面中的第二个及以上的控件要在初始化属性中设置Child="true"(即只能有一个控件可以不设置Child="true")！</fieldset>
                                <br />
                                页面声明：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">&lt;</span><span style="color: #800000">%@Register </span>
                                        <span style="color: #ff0000">TagPrefix</span><span style="color: #0000ff">="dntb"</span><span
                                                            style="color: #ff0000"> Namespace</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                style="color: #ff0000"> Assembly</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                    style="color: #ff0000">%</span><span style="color: #0000ff">&gt;</span></div>
                                </div>
                                <br />
                                后台代码：<br />
                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                        background-color: #eeeeee">
                        <div style="font-family: Courier New">
                            <span style="color: blue">private</span> <span style="color: blue">void</span> Page_Load(<span
                                style="color: blue">object</span> sender, System.EventArgs e)
                            <br />
                            {
                            <br />
                            &nbsp; &nbsp; <span style="color: blue">if</span> (!IsPostBack)
                            <br />
                            &nbsp; &nbsp; {
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Text = <span style="color: maroon">"&lt;a href=http://www.aspxcn.com.cn&gt;&lt;img
                                src='system_dntb/skin/default/img/logo.gif' alt='DotNet中华网版权所有' width=260 height=60
                                border=0&gt;&lt;/a&gt;"</span>;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; WebEditor2.Text = <span style="color: maroon">"&lt;a href=http://www.aspxcn.com.cn&gt;&lt;img
                                src='system_dntb/skin/xp/img/logo.gif' alt='DotNet中华网版权所有' width=260 height=60 border=0&gt;&lt;/a&gt;"</span>;
                            <br />
                            &nbsp; &nbsp; }
                            <br />
                            }<br />
                            <div style="font-family: Courier New">
                                <span style="color: blue">private</span> <span style="color: blue">void</span> Button1_OnClick(<span
                                    style="color: blue">object</span> sender, System.EventArgs e)
                                <br />
                                {
                                <br />
                                &nbsp; &nbsp; <span style="color: green">//控件2的内容传送到控件1</span>
                                <br />
                                &nbsp; &nbsp; WebEditor1.Text=WebEditor2.Text;
                                <br />
                                }</div>
                        </div>
                    </div>
                                <br />
                                前台代码：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">
                                            <span style="color: #008000">//设置两个不同ID的DotNetTextBox控件，并且WebEditor2的界面为普通样式(skin/default/)</span><br />
                                            &lt;</span><span style="color: #800000">DNTB:WebEditor </span><span style="color: #ff0000">
                                                id</span><span style="color: #0000ff">="WebEditor2"</span><span
                                                style="color: #ff0000">
                                                    Skin</span><span style="color: #0000ff">="skin/default/" <span style="color: #ff0000">
                                                        Child</span><span style="color: #0000ff">="True"</span></span><span
                                                    style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                        style="color: #0000ff">&gt;&lt;/</span><span style="color: #800000">DNTB:WebEditor</span><span
                                                            style="color: #0000ff">&gt;<br />
                                                            &lt;<span style="color: #800000">DNTB:WebEditor </span><span style="color: #ff0000">
                                                                id</span><span style="color: #0000ff">="WebEditor1"</span><span
                                                    style="color: #ff0000">
                                                                        runat</span><span style="color: #0000ff">="server"</span><span
                                                        style="color: #0000ff">&gt;&lt;/</span><span style="color: #800000">DNTB:WebEditor</span><span
                                                            style="color: #0000ff">&gt;</span></span></div>
                                </div>
                                <br />
                                <hr size="1" style="border-style:dashed;border-color:#999999" align=center />
                                页面实际使用效果的演示：<br /><br />
                    <br /><center><dntb:WebEditor ID="WebEditor2" runat="server" Focus="false" Skin="skin/default/" Child="True" /></center><br />
                </asp:View>
                <asp:View ID="demo8" runat="server">
                                <fieldset>
                                    <legend>演示说明</legend>&nbsp; &nbsp;&nbsp;控件菜单自定义排列及实时拖曳菜单工具栏的应用演示，通过配置system_dntb/menuconfig/下相应的配置文件，即可构建用户个性化的编辑器控件。配置文件中通过&lt;dragtoolbar&gt;及&lt;dragtoolbarend&gt;配置节，即可配置相应的可拖曳菜单工具栏。配置完成后在控件界面点击<img
                                        src="system_dntb/img/drag.gif" />图标即实时拖曳菜单工具栏!</fieldset>
                                <br />
                                页面声明：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">&lt;</span><span style="color: #800000">%@Register </span>
                                        <span style="color: #ff0000">TagPrefix</span><span style="color: #0000ff">="dntb"</span><span
                                                            style="color: #ff0000"> Namespace</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                style="color: #ff0000"> Assembly</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                    style="color: #ff0000">%</span><span style="color: #0000ff">&gt;</span></div>
                                </div>
                                <br />
                                后台代码：<br />
                    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                        background-color: #eeeeee">
                        <div style="font-family: Courier New">
                            &nbsp; &nbsp; <span style="color: blue">private</span> <span style="color: blue">void</span>
                            ChangeArray(<span style="color: blue">object</span> sender, System.EventArgs e)
                            <br />
                            &nbsp; &nbsp; {
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//根据选择的值设定菜单配置文件</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.MenuConfig = selectArray.SelectedValue;
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//设定控件宽度</span>
                            <br />
                            &nbsp; &nbsp; &nbsp; &nbsp; WebEditor1.Width = <span style="color: maroon">600</span>;
                            <br />
                            &nbsp; &nbsp; }</div>
                    </div>
                                <br />
                                前台代码：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">
                                            <div>
                                                <span style="color: #000000"><span style="color: #008000">//排列样式的下拉选择框</span><br />
                                                    [选择排列样式]：<span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:dropdownlist
                                                    </span><span style="color: #ff0000">id</span><span style="color: #0000ff">="selectArray"</span><span
                                                        style="color: #ff0000"> AutoPostBack</span><span style="color: #0000ff">="true"</span><span
                                                            style="color: #ff0000"> OnSelectedIndexChanged</span><span style="color: #0000ff">="ChangeArray"</span><span
                                                                style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                                    style="color: #0000ff">&gt;</span><span style="color: #000000"><br />
                                                                    </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                                    </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="default_style1.config"</span><span
                                                                        style="color: #0000ff">&gt;</span><span style="color: #000000">单列顶端工具栏的排列</span><span
                                                                            style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:ListItem</span><span
                                                                                style="color: #0000ff">&gt;</span><span style="color: #000000"><br />
                                                                                </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                                                </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="default_style2.config"</span><span
                                                                                    style="color: #0000ff">&gt;</span><span style="color: #000000">三列顶端工具栏的排列</span><span
                                                                                        style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:ListItem</span><span
                                                                                            style="color: #0000ff">&gt;</span><span style="color: #000000"><br />
                                                                                            </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                                                            </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="default_style3.config"</span><span
                                                                                                style="color: #0000ff">&gt;<span style="color: #000000">带隐藏工具栏的排列</span></span><span style="color: #000000"></span><span
                                                                                                    style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:ListItem</span><span
                                                                                                        style="color: #0000ff">&gt;<br />
                                                                                                    </span><span style="color: #000000"><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="default_style4.config"</span><span
                                                                                                style="color: #0000ff">&gt;<span
                                                                                                                style="color: #000000">带左侧工具栏的排列</span><span
                                                                                                    style="color: #0000ff"></span></span><span style="color: #000000"></span><span
                                                                                                    style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:ListItem</span><span
                                                                                                        style="color: #0000ff">&gt;</span><br />
                                                                                                        </span><span style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:DropDownList</span><span
                                                                                                            style="color: #0000ff">&gt;</span></span><span
                                                                                                style="color: #0000ff"></span></div>
                                            .......<br />
                                            <span style="color: #008000"></span>
                                            &lt;</span><span style="color: #800000">DNTB:WebEditor </span><span style="color: #ff0000">
                                                id</span><span style="color: #0000ff">="WebEditor1" <span style="color: #ff0000">MenuConfig</span><span style="color: #0000ff">=</span>"default_style1.config"<span
                                                    style="color: #ff0000"></span></span><span
                                                    style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                        style="color: #0000ff">&gt;&lt;/</span><span style="color: #800000">DNTB:WebEditor</span><span
                                                            style="color: #0000ff">&gt;</span></div>
                                </div>
                                <br />
                                <hr size="1" style="border-style:dashed;border-color:#999999" align=center />
                                页面实际使用效果的演示：<br /><br />
                    <br /><center>
                    [选择排列样式]：<asp:dropdownlist id="selectArray" AutoPostBack="true" OnSelectedIndexChanged="ChangeArray" runat="server">
                        <asp:ListItem Value="default_style1.config">单列顶端工具栏的排列</asp:ListItem>
                        <asp:ListItem Value="default_style2.config">三列顶端工具栏的排列</asp:ListItem>
                        <asp:ListItem Value="default_style3.config">带隐藏工具栏的排列</asp:ListItem>
                        <asp:ListItem Value="default_style4.config">带左侧工具栏的排列</asp:ListItem>
                    </asp:DropDownList></center></asp:View>
                    <asp:View ID="demo9" runat="server">
                                <fieldset>
                                    <legend>演示说明</legend>&nbsp; &nbsp;&nbsp;控件提交内容后自动及手动分页的应用演示,通过WebEditor1.getManualPage(手动分页)及WebEditor1.getAutoPage(自动分页)两个属性来实现内容的手动及自动分页储存!其中手动分页是通过控件中<img
                                        src="system_dntb/skin/XP/img/pageoutput.gif" />插入内容分页符来作为分页的标识,自动分页则通过设定PageLength属性来界定分页的最大字符长度(默认为2000字符分一页)!</fieldset>
                                <br />
                                页面声明：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">&lt;</span><span style="color: #800000">%@Register </span>
                                        <span style="color: #ff0000">TagPrefix</span><span style="color: #0000ff">="dntb"</span><span
                                                            style="color: #ff0000"> Namespace</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                style="color: #ff0000"> Assembly</span><span style="color: #0000ff">="DotNetTextBox"</span><span
                                                                    style="color: #ff0000">%</span><span style="color: #0000ff">&gt;</span></div>
                                </div>
                                <br />
                                后台代码：<br />
                        <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                            padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                            width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                            background-color: #eeeeee">
                            <div style="font-family: Courier New">
                                &nbsp; &nbsp; <span style="color: blue">private</span> <span style="color: blue">void</span>
                                Button1_OnClick(<span style="color: blue">object</span> sender, System.EventArgs
                                e)
                                <br />
                                &nbsp; &nbsp; {
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; ArrayList content;
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">if</span> (selectPageOutput.SelectedItem.Value
                                == <span style="color: maroon">"manual"</span>)
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; {
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//得到手动分页内容的分页集合,在控件编辑框中插入内容分页符作为分页标准!&nbsp;</span>
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; content = WebEditor1.getManualPage;
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; }
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">else</span>
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; {
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//得到自动分页内容的分页集合,以控件的WebEditor1.PageLength属性设定的最大分页字符数作为分页标准&nbsp;</span>
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; content = WebEditor1.getAutoPage;
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; }
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//判断提交内容的分页数是否大于1&nbsp;&nbsp;&nbsp;</span>
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">if</span> (content.Count &gt;
                                <span style="color: maroon">1</span>)
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; {
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//历遍集合将分页内容分篇添加到数据库&nbsp;&nbsp;&nbsp;</span>
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">for</span> (<span
                                    style="color: blue">int</span> i = <span style="color: maroon">0</span>; i &lt;
                                content.Count; i++)
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; {
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//此处可通过content[i].ToString()将相应分页内容添加到数据库
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</span>
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; }
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; }
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: blue">else</span>
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; {
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="color: green">//如果分页小于1,则正常通过WebEditor1.Text添加内容到数据库&nbsp;&nbsp;&nbsp;</span>
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; }
                                <br />
                                &nbsp; &nbsp; }</div>
                        </div>
                                <br />
                                前台代码：<br />
                                <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                                        padding-left: 4px; font-size: 13px; padding-bottom: 4px; border-left: #cccccc 1px solid;
                                        width: 98%; word-break: break-all; padding-top: 4px; border-bottom: #cccccc 1px solid;
                                        background-color: #eeeeee">
                                    <div>
                                        <span style="color: #0000ff">
                                            <div>
                                                <span style="color: #000000"><span style="color: #008000">//分页类型的下拉选择框</span><br />
                                                    [选择分页类型]：<span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:dropdownlist
                                                    </span><span style="color: #ff0000">id</span><span style="color: #0000ff">="selectPageOutput"</span><span style="color: #0000ff"><span
                                                        style="color: #ff0000"> </span></span><span> </span><span
                                                                style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                                    style="color: #0000ff">&gt;</span><span style="color: #000000"><br />
                                                                    </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                                    </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="manual"</span><span
                                                                        style="color: #0000ff">&gt;</span><span style="color: #000000">手动分页</span><span
                                                                            style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:ListItem</span><span
                                                                                style="color: #0000ff">&gt;</span><span style="color: #000000"><br />
                                                                                </span><span style="color: #0000ff">&lt;</span><span style="color: #800000">asp:ListItem
                                                                                </span><span style="color: #ff0000">Value</span><span style="color: #0000ff">="auto"</span><span
                                                                                    style="color: #0000ff">&gt;</span><span style="color: #000000">自动分页</span><span
                                                                                        style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:ListItem</span><span
                                                                                            style="color: #0000ff">&gt;</span><span style="color: #000000"><br />
                                                                                                        </span><span style="color: #0000ff">&lt;/</span><span style="color: #800000">asp:DropDownList</span><span
                                                                                                            style="color: #0000ff">&gt;</span></span><span
                                                                                                style="color: #0000ff"></span></div>
                                            .......<br />
                                            <span style="color: #008000">//分页类型的下拉选择框</span><br />
                                            <span style="color: #008000"></span>
                                            &lt;</span><span style="color: #800000">DNTB:WebEditor </span><span style="color: #ff0000">
                                                id</span><span style="color: #0000ff">="WebEditor1" <span style="color: #ff0000">MenuConfig</span><span style="color: #0000ff">=</span>"Pageoutput.config"</span><span
                                                    style="color: #ff0000"> runat</span><span style="color: #0000ff">="server"</span><span
                                                        style="color: #0000ff">&gt;&lt;/</span><span style="color: #800000">DNTB:WebEditor</span><span
                                                            style="color: #0000ff">&gt;</span></div>
                                </div>
                                <br />
                                <hr size="1" style="border-style:dashed;border-color:#999999" align=center />
                                页面实际使用效果的演示：<br /><br />
                    <br /><center>
                        [选择分页类型]：<asp:dropdownList id="selectPageOutput" AutoPostBack="true" OnSelectedIndexChanged="ChangeArray" runat="server">
                        <asp:ListItem Value="manual">手动分页</asp:ListItem>
                        <asp:ListItem Value="auto">自动分页</asp:ListItem>
                    </asp:dropdownList></center></asp:View>
            </asp:MultiView>
             <br />
            <center>
          
            <dntb:WebEditor ID="WebEditor1" runat="server" Focus="false" Skin="skin/default/" Child="True"/><br />
                <br />
                                              <asp:Button id="Button1"  OnClick="Button1_OnClick" runat="server" Text="提交内容"></asp:Button>
          <asp:Button id="Button2"  OnClick="Button2_OnClick" runat="server" Text="清空内容" style="margin-top: 0px"></asp:Button>
                                    </center>
            </TD></TR></TABLE></div>
            <asp:Label ID="label1" runat="server"></asp:Label><br />
            <br />
            <br />
            <br />
            <table bgcolor="#006699" border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 30px">
                <tr>
                    <td>
                        <img alt="DotNet中华网" border="0" src="system_dntb/skin/default/img/aspnet2.gif" />
                        <a href="http://www.aspxcn.com.cn"><font color="#000000"><span><span style="color: #ffffff">
                            <span style="font-family: Arial"><span style="font-size: 12pt">©</span> </span>2003-2009
                            DotNetTextBox™ v6.0 DotNet中华网工作室版权所有</span></span></font></a><span style="color: #ffffff">
                                网站备案:</span><a href="http://www.miibeian.gov.cn" target="_blank"><span style="color: #ffffff;
                                    text-decoration: underline">粤ICP备06045090号</span></a></td>
                </tr>
            </table>
        </DIV></form></center>
</body>
</html>