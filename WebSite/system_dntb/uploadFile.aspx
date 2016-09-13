<%@ Page Language="C#" AutoEventWireup="true" Inherits="DotNetTextBox.UpLoad"%>
<%@ Import Namespace="DotNetTextBox" %>
<html>
<head>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
<base target="_self" />
<script language="javascript" type="text/javascript">
    var userAgent = navigator.userAgent.toLowerCase();
    var is_ie = (userAgent.indexOf('msie') != -1);
    var arr = new Array;
    var count = 1;
    var SelectControlID = 'uploadlist';
    
    if (typeof HTMLElement != "undefined" && !HTMLElement.prototype.insertAdjacentElement) {
        HTMLElement.prototype.insertAdjacentElement = function(where, parsedNode) {
            switch (where) {
                case 'beforeBegin':
                    this.parentNode.insertBefore(parsedNode, this)
                    break;
                case 'afterBegin':
                    this.insertBefore(parsedNode, this.firstChild);
                    break;
                case 'beforeEnd':
                    this.appendChild(parsedNode);
                    break;
                case 'afterEnd':
                    if (this.nextSibling) this.parentNode.insertBefore(parsedNode, this.nextSibling);
                    else this.parentNode.appendChild(parsedNode);
                    break;
            }
        }

        HTMLElement.prototype.insertAdjacentHTML = function(where, htmlStr) {
            var r = this.ownerDocument.createRange();
            r.setStartBefore(this);
            var parsedHTML = r.createContextualFragment(htmlStr);
            this.insertAdjacentElement(where, parsedHTML);
        }

        HTMLElement.prototype.insertAdjacentText = function(where, txtStr) {
            var parsedText = document.createTextNode(txtStr);
            this.insertAdjacentElement(where, parsedText);
        }
    }

    
    function addFile() {
        if (!CheckFile())
            return;
        count++;
        var str = "<span Num='" + count + "'><INPUT type='file' NAME='File' onchange='addFile()' style='font-size:10pt;width:300px;height:21px'/></span>";
        document.getElementById('MyFile').insertAdjacentHTML('beforeEnd', str);
        
    }
    
    function CheckFile() {
        var filenames = document.getElementById('MyFile').getElementsByTagName('span');
        for (var i = 0; i < filenames.length; i++) {
            if (filenames[i].getAttribute('Num') == count) {
                var file = filenames[i].getElementsByTagName('input').item(0).value;
                if (file == '') {
                    alert('<%=ResourceManager.GetString("addfile")%>');
                    return false;
                }
                if (CheckOption(file)) {
                    alert('<%=ResourceManager.GetString("exist")%>');
                    return false;
                }
                filenames[i].style.display = 'none';
                appendOptionLast(file);
                return true;
            }
        }
        return true;
    }


    function insertOptionBefore(num) {
        var elSel = document.getElementById(SelectControlID);
        if (elSel.selectedIndex >= 0) {
            var elOptNew = document.createElement('option');
            elOptNew.text = num;
            elOptNew.value = num;
            var elOptOld = elSel.options[elSel.selectedIndex];
            try {
                elSel.add(elOptNew, elOptOld); // standards compliant; doesn't work in IE
            }
            catch (ex) {
                elSel.add(elOptNew, elSel.selectedIndex); // IE only
            }
        }
    }

    function removeOptionSelected() {
        var elSel = document.getElementById(SelectControlID);
        var i;
        for (i = elSel.length - 1; i >= 0; i--) {
            if (elSel.options[i].selected) {
                elSel.remove(i);
            }
        }
    }

    function appendOptionLast(num) {
        var elOptNew = document.createElement('option');
        elOptNew.text = num;
        elOptNew.value = num;
        var elSel = document.getElementById(SelectControlID);

        try {
            elSel.add(elOptNew, null); // standards compliant; doesn't work in IE
        }
        catch (ex) {
            elSel.add(elOptNew); // IE only
        }
        elOptNew.selected = true;
    }

    function removeAll() {
        var elSel = document.getElementById(SelectControlID);
        if (elSel.length > 0) {
            for (var i = elSel.options.length - 1; i >= 0; i--) {
                elSel.remove(i);
            }
        }
    }
    
    function CheckOption(value) {
        var elSel = document.getElementById(SelectControlID);
        var i;
        for (i = elSel.length - 1; i >= 0; i--) {
            if (elSel.options[i].value == value)
                return true;
        }
        return false;
    }
    
    function loading(showmessage) {
        document.getElementById("loading").style.visibility = "visible";
        document.getElementById("statusmessage").innerHTML = showmessage;
        return true;
    }
    function newFile() {
        if (document.getElementById("file_path").value != "") {
            arr[0] = document.getElementById("file_path").value.replace(/\s/g, "\%20");
            if (document.getElementById("file_name").value == "") {
                arr[1] = document.getElementById("file_path").value;
            }
            else {
                arr[1] = document.getElementById("file_name").value;
            }
            if (is_ie) {
                window.returnValue = arr;
            }
            else {
                if (document.getElementById("insertFile").value != '<%=ResourceManager.GetString("mof")%>') {
                    window.opener.inserObject(null, 'file', arr);
                }
                else {
                    window.opener.inserObject(null, 'modfile', arr);
                }
            }
        }
        window.close();
    }
    function add(name) {
        if (is_ie) {
            var path = document.getElementById("path").innerText;
        }
        else {
            var path = document.getElementById("path").textContent;
        }
        document.getElementById("file_path").value = path + name;
        document.getElementById("file_path").focus();
    }
    var sTitle = '<%=ResourceManager.GetString("insertfile")%>';
    if (is_ie) {
        if (dialogArguments != null)
            sTitle = '<%=ResourceManager.GetString("moffile")%>';
    }
    else {
        arr = window.opener.GetFile();
        if (arr[0] != null) {
            sTitle = '<%=ResourceManager.GetString("moffile")%>';
        }
        window.focus();
    }
    document.write("<TITLE>" + sTitle + "</TITLE>");
</script>
<link href="stylesheet.css" rel="stylesheet" type="text/css" />
</head>
<body topmargin="0">
<form id="uploadFace" method="post" enctype="multipart/form-data" runat="server">
<table border="0" align=center style="word-break:break-all" width="100%">
<tr>
<td colspan="3" rowspan="3">
<fieldset><legend><span style="color: darkgray"><span style="color: gray"><%=ResourceManager.GetString("uploadface")%></span>&nbsp;
</span></legend>
<%=ResourceManager.GetString("uploadpath")%>：<asp:Label ID="path" runat="server" ForeColor="Black"></asp:Label>&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="returndir" OnClick="returndir_Click" ImageUrl="img/parentfolder.gif" runat=server/>..<br /><span id='MyFile'><%=ResourceManager.GetString("adduploadfile")%>：<span num='1'><asp:FileUpload ID="FileUpload1" runat="server" Font-Size="10pt" Height="21px" onchange="addFile()"
        TabIndex="2" Width="300px" /></span></span><asp:TextBox ID="remoteurl" runat="server" Text="http://"
            Visible="False" Width="250px"></asp:TextBox>
    <asp:Button ID="addurl" runat="server" OnClick="addurl_Click" Visible="False" Width="49px" />
    [<asp:Label ID="maxuploadfile" runat="server"></asp:Label>
    <asp:Label ID="maxupload" runat="server" ForeColor="red"></asp:Label>]
    <asp:Panel ID="batchPanel" runat="server" Width="100%">
        <table>
            <tr>
                <td rowspan="3">
                     <select size="4" name="uploadlist" id="uploadlist" style="width:300px;" runat="server"></select></td>
                <td rowspan="3">
                    <input type='button' value='<%=ResourceManager.GetString("delfile")%>' style="width:70px"  onclick='javascript:removeOptionSelected()' />
                    <input type='button' value='<%=ResourceManager.GetString("clearlist")%>' style="width:70px"  onclick='javascript:removeAll()' />
                    <asp:Button ID="uploadBtn" runat="server" OnClick="UploadBtn_Click" Width="70px" />
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="local"></asp:ListItem>
                        <asp:ListItem Value="remote"></asp:ListItem>
                    </asp:RadioButtonList><input type='hidden' id='hidvalue' name='hidvalue' /></td>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
        </table>
    </asp:Panel>
<%=ResourceManager.GetString("filepath")%>：<asp:TextBox ID="file_path" runat="server" Width="316px" TabIndex="1"></asp:TextBox>
<input language="javascript" onclick="javascript:newFile()" type="button" value='<%=ResourceManager.GetString("insertfile")%>' id="insertFile" /><br />
<%=ResourceManager.GetString("filetitle")%>：<asp:TextBox ID="file_name" runat="server" Width="316px"></asp:TextBox>&nbsp;<br />
[ <%=ResourceManager.GetString("uploaduse")%>：<asp:label ID="useSpace" ForeColor="Red" runat=server />，<%=ResourceManager.GetString("have")%>：<asp:label ID="space" ForeColor="Red" runat=server /><%=ResourceManager.GetString("singlesize")%>：<asp:Label ID="maxSingleUploadSize" runat="server" ForeColor="Red"></asp:Label>
]</fieldset>
<asp:Panel ID="fieldsetdiv" runat="server"><fieldset style="text-align: center"><legend><span style="color: gray"><%=ResourceManager.GetString("filelist")%>&nbsp;<asp:ImageButton ID="filelistturn" CommandName="hide" ImageAlign=AbsBottom ImageUrl="img/hide.gif" runat="server" OnClick="filelistturn_Click" /></span>&nbsp;</legend>
<asp:Panel id="filelistdiv" runat=server><div style="border-right: 1.5pt inset; border-top: 1.5pt inset; vertical-align: middle;
overflow: auto; border-left: 1.5pt inset; width: 100%; border-bottom: 1.5pt inset;
height: 240px; background-color: white">
<asp:GridView runat="server" id="File_List" HeaderStyle-HorizontalAlign=Center AutoGenerateColumns="False" HeaderStyle-BackColor="buttonface" HeaderStyle-ForeColor=windowtext HeaderStyle-Font-Bold="True" Width="100%" BorderWidth="1px" OnRowCancelingEdit="File_List_RowCancelingEdit" OnRowUpdating="File_List_RowUpdating">
<Columns>
<asp:TemplateField>
<HeaderTemplate>
<asp:CheckBox ID="checkall" runat="server" Text=<%#ResourceManager.GetString("selectall")%> AutoPostBack="true" OnCheckedChanged="checkAll" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="check" runat="server" />
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" Width="45px" />
</asp:TemplateField>
<asp:TemplateField>
<EditItemTemplate>
<asp:TextBox ID="editName" Text=<%#DataBinder.Eval(Container.DataItem,"Attributes").ToString().ToLower()=="directory"?DataBinder.Eval(Container.DataItem,"Name"):DataBinder.Eval(Container.DataItem,"Name").ToString().Remove(DataBinder.Eval(Container.DataItem,"Name").ToString().LastIndexOf(DataBinder.Eval(Container.DataItem,"Extension").ToString()))%> Width="100px" runat=server></asp:TextBox> <asp:Button ID="editBtn" CommandName="Update" CommandArgument=<%#DataBinder.Eval(Container.DataItem,"Name")%> runat=server Text=<%#ResourceManager.GetString("edit")%> /> <asp:Button ID="Cancel" runat=server Text=<%#ResourceManager.GetString("cancel")%> CommandArgument=<%#DataBinder.Eval(Container.DataItem,"Attributes").ToString().ToLower()%> CommandName="Cancel" />
</EditItemTemplate>
<ItemTemplate>
<img src="img/filetype/<%#DataBinder.Eval(Container.DataItem,"Attributes").ToString().ToLower()=="directory"?"folder":((string)DataBinder.Eval(Container.DataItem,"Extension")).Replace(".","")%>.gif" /><asp:LinkButton ID="ListID" Text=<%#DataBinder.Eval(Container.DataItem,"Name")%> style="cursor:pointer; word-break:break-all" ForeColor="#000000" Font-Underline=false onmouseout="this.style.textDecoration='none'" onmouseover="this.style.textDecoration='underline'" CommandArgument=<%#DataBinder.Eval(Container.DataItem,"Name").ToString()%> OnCommand="SetServerCookie" OnClientClick=<%#DataBinder.Eval(Container.DataItem,"Attributes").ToString().ToLower()!="directory"?DataBinder.Eval(Container.DataItem,"Name","javascript:add(\"{0}\");return false;"):""%> runat="server"/>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="LastWriteTime" ReadOnly="True" HtmlEncode=False DataFormatString="{0:D}" >
<ItemStyle HorizontalAlign="Center" Width="25%" />
</asp:BoundField>
<asp:TemplateField>
<ItemTemplate>
<asp:Label ID="LengthCont" Text=<%#DataBinder.Eval(Container.DataItem,"Attributes").ToString().ToLower()=="directory"?"":DataBinder.Eval(Container.DataItem,"Length","{0:#,### Bytes}")%> runat=server />
</ItemTemplate>
<ItemStyle HorizontalAlign=Center Width="25%" />
</asp:TemplateField>
</Columns>
<HeaderStyle Font-Bold="True" ForeColor="WindowText" BackColor="Control" BorderWidth="1px" HorizontalAlign="Center" />
</asp:GridView></div><table width="100%" border="0px"><tr><td valign="baseline" style="height: 23px">
[<%=ResourceManager.GetString("controlmenu")%>]：<asp:ImageButton id="selectAllBtn" runat="server" onMouseOver="this.className='overcolor'" onMouseOut="this.className=''" ImageUrl="img/selectall.gif" onclick="selectAllBtn_Click" />&nbsp;
<asp:ImageButton ID="deleteBtn" runat="server" onMouseOver="this.className='overcolor'" onMouseOut="this.className=''" ImageUrl="img/delete.gif" OnClick="deleteBtn_Click" />&nbsp;&nbsp;<asp:ImageButton id="editBtn" ImageUrl="img/rename.gif" onMouseOver="this.className='overcolor'" onMouseOut="this.className=''" runat="server" onclick="editBtn_Click" />&nbsp;&nbsp;<asp:ImageButton ID="newfolderBtn" onMouseOver="this.className='overcolor'" onMouseOut="this.className=''" runat="server" ImageUrl="img/newfolder.gif" OnClick="newfolderBtn_Click" />&nbsp;&nbsp;<asp:ImageButton ID="returndir2" OnClick="returndir_Click" ImageUrl="img/parentfolder.gif" onMouseOver="this.className='overcolor'" onMouseOut="this.className=''" runat=server/>&nbsp;&nbsp;<input language="javascript" onmouseup="if(is_ie){showModalDialog('find.aspx',this,'dialogWidth:320px;dialogHeight:130px;status:0;scroll:no');}else{window.find();}"
type=image src="img/search.gif" onMouseOver="this.className='overcolor'" onMouseOut="this.className=''" title='<%=ResourceManager.GetString("findfile")%>' />&nbsp;
<input language="javascript" onclick="window.close();" type="image" onMouseOver="this.className='overcolor'" onMouseOut="this.className=''" src="img/close.gif" title="<%=ResourceManager.GetString("close")%>" /></td>
<td align="right" style="height: 23px">
<img border=0px src="img/logo_S.png" /></td>
</tr></table></asp:Panel>
</fieldset></asp:Panel>
<asp:HiddenField ID="config_watermark" runat="server" />
<asp:HiddenField ID="config_watermarkText" runat="server" />
<asp:HiddenField ID="config_watermarkImages" runat="server" />
<asp:HiddenField ID="config_watermarkImages_path" runat="server" />
<asp:HiddenField ID="config_smallImages" runat="server" />
<asp:HiddenField ID="config_smallImagesName" runat="server" />
<asp:HiddenField ID="config_maxAllUploadSize" runat="server" />
<asp:HiddenField ID="config_autoname" runat="server" />
<asp:HiddenField ID="config_allowUpload" runat="server" />
<asp:HiddenField ID="config_fileFilters" runat="server" />
<asp:HiddenField ID="config_maxSingleUploadSize" runat="server" />
<asp:HiddenField ID="config_fileListBox" runat="server" />
<asp:HiddenField ID="config_watermarkImagesName" runat="server" />
<asp:HiddenField ID="config_watermarkName" runat="server" />
<asp:HiddenField ID="config_smallImagesType" runat="server" />
<asp:HiddenField ID="config_smallImagesW" runat="server" />
<asp:HiddenField ID="config_smallImagesH" runat="server" />
<asp:HiddenField ID="config_type" Value="File"  runat="server" />
</td>
</tr>
</table>
<div id="loading" style="border-right: #333333 1px dashed; border-top: #333333 1px dashed;
font-size: 9pt; visibility: hidden; border-left: #333333 1px dashed;
width: 270px; color: #000000; border-bottom: #333333 1px dashed; position: absolute; height: 120px; background-color: #ffffff">
<center>
<br />
<br />
<span id="statusmessage"></span></center>
<br />
<center>
<asp:Button ID="canceloading" runat="server" Style="border-top-style: dashed; border-right-style: dashed;
border-left-style: dashed; border-bottom-style: dashed" />&nbsp;</center>
<br />
</div>
<script type="text/javascript">
    var load = document.getElementById('loading');
    var ierevise = 0;
    window.onload = function() { resizeLoad() };
    function resizeLoad() {
        if (is_ie) {
            var iever = parseFloat(navigator.userAgent.match(/MSIE (\d+\.\d+)/)[1]);
            if (iever < 6) {
                ierevise = 150;
            }
            else if (iever == 6) {
                ierevise = 35
            }
            if (document.body.scrollWidth > (window.screen.availWidth - 100)) {
                window.dialogWidth = (window.screen.availWidth - 100).toString() + "px";
            } else {
                if (document.body.scrollWidth > 0) {
                    window.dialogWidth = (document.body.scrollWidth).toString() + "px";
                }
            }

            if (document.body.scrollHeight > (window.screen.availHeight - 70)) {
                window.dialogHeight = (window.screen.availHeight - 50).toString() + "px";
            } else {
                if (document.body.scrollHeight > 0) {
                    window.dialogHeight = (document.body.scrollHeight - 115 + ierevise).toString() + "px";
                }
            }
            document.body.bgColor = "ButtonFace";
            if (window.dialogArguments) {
                document.getElementById("file_path").value = dialogArguments[0];
                document.getElementById("file_name").value = dialogArguments[1];
                document.getElementById("insertFile").value = '<%=ResourceManager.GetString("mof")%>';
            }
        }
        else {
            if (document.body.scrollWidth > (window.screen.availWidth - 100)) {
                window.innerWidth = window.screen.availWidth - 100;
            } else {
                window.innerWidth = document.body.scrollWidth;
            }

            if (document.body.scrollHeight > (window.screen.availHeight - 70)) {
                window.innerHeight = document.body.scrollHeight - 50;
            } else {
                window.innerHeight = document.body.scrollHeight - 115;
            }
            document.body.bgColor = "#E0E0E0";
            if (arr[0] != null) {
                document.getElementById("file_path").value = arr[0];
                document.getElementById("file_name").value = arr[1];
                document.getElementById("insertFile").value = '<%=ResourceManager.GetString("mof")%>';
            }
        }
        load.style.top = parseInt((document.body.clientHeight - load.offsetHeight) / 2 + document.body.scrollTop);
        load.style.left = parseInt((document.body.clientWidth - load.offsetWidth) / 2 + document.body.scrollLeft);
    }
</script>
</form>
</body>
</html>
