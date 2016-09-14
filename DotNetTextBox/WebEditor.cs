using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

[assembly: WebResource("DotNetTextBox.share.js", "application/x-javascript")]
[assembly: WebResource("DotNetTextBox.core.js", "application/x-javascript")]
[assembly: WebResource("DotNetTextBox.core_gecko.js", "application/x-javascript")]
namespace DotNetTextBox
{
    #region 自定义控件相关属性的初始化

    /// <summary>
    /// 控件默认标记的定义
    /// </summary>
	[ValidationPropertyAttribute("Text")]
    [ToolboxDataAttribute("<{0}:WebEditor runat=server></{0}:WebEditor>")]
    [DefaultPropertyAttribute("Text")]
    public sealed class WebEditor : WebControl, INamingContainer, IPostBackDataHandler
    {
        /// <summary>
        /// 继承Input控件的基类
        /// </summary>
		public WebEditor() : base("input")
        {
        }
        /// <summary>
        /// 定义控件属性所用到的初始属性值
        /// </summary>
        private string tooltip = "", targetid = "", codetitle = "以下是代码片段：", quotetitle = "以下是引用片段：", uploadconfig = "default.config", languages = "default", toolbarbgimg = "none", rightclickmenuconfig = "cut,copy,delete,paste,selectall,hr,quote,code,link,properties";
        private bool focus = true, child = false;
        private Color bordercolor = Color.FromArgb(221, 221, 221), backcolor = Color.FromName("White"), toolbarcolor = Color.FromArgb(239, 239, 239), editbordercolor = Color.FromArgb(205, 205, 211), codebordercolor = Color.FromArgb(101, 149, 214), codebackcolor = Color.FromArgb(232, 244, 255), quotebackcolor = Color.FromArgb(232, 244, 255), quotebordercolor = Color.FromArgb(101, 149, 214);
        private Unit borderwidth = 0, codeborder = 1, quoteborder = 1;
        private BorderStyle borderstyle = BorderStyle.None, codeborderstyle = BorderStyle.Dotted, quoteborderstyle = BorderStyle.Dotted;
        private ShadowType frameshadow;
        private XhtmlType xhtml = XhtmlType.None;
        private int adjustsize = 50, expirehours = 0, pagelength = 2000;
        private pathTypeName pathtype = pathTypeName.AbsoluteFull;
        private NewLineType newlinemode = NewLineType.BR;
        private readonly object _textChanged = new object();

        #region 自定义属性

        /// <summary>
        /// 下面开始为自定义控件添加各种的自定义属性
        /// </summary>
        [Category("操作"), Description("在更改文本属性后激发。")]
        public event EventHandler TextChanged
        {
            add { Events.AddHandler(_textChanged, value); }
            remove { Events.RemoveHandler(_textChanged, value); }
        }

        [Bindable(true), Category("Appearance"), Description("控件文本值。")]
        public string Text
        {
            set
            {
                ViewState["value"] = value;
            }
            get
            {
                if (ViewState["value"] == null)
                {
                    return String.Empty;
                }
                else
                {
                    if (this.AutoSaveImgToLocal)
                    {
                        return saveRemotingImg(ViewState["value"].ToString());
                    }
                    else
                    {
                        return ViewState["value"].ToString();
                    }
                }
            }
        }

        [Bindable(true), Category("控件编辑栏"), Description("控件编辑栏框架阴影。"), DefaultValue(ShadowType.Close)]
        public ShadowType FrameShadow
        {
            set
            {
                frameshadow = value;
            }
            get
            {
                return frameshadow;
            }
        }

        [Bindable(true), Category("控件编辑栏"), Description("控件编辑栏边框色。"), DefaultValue("#CDCDD3"), TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter))]
        public Color EditBorderColor
        {
            set
            {
                editbordercolor = value;
            }
            get
            {
                return editbordercolor;
            }
        }

        [Bindable(true), Category("控件编辑栏"), Description("控件编辑栏背景色。"), DefaultValue("#FFFFFF"), TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter))]
        new public Color BackColor
        {
            set
            {
                backcolor = value;
            }
            get
            {
                return backcolor;
            }
        }

        [Bindable(true), Category("Appearance"), Description("页面加载后是否聚焦控件编辑框。"), DefaultValue(true)]
        public new bool Focus
        {
            set
            {
                focus = value;
            }
            get
            {
                return focus;
            }
        }

        [Bindable(true), Category("Appearance"), Description("控件边框样式的颜色。"), DefaultValue("#DDDDDD")]
        new public Color BorderColor
        {
            set
            {
                bordercolor = value;
            }
            get
            {
                return bordercolor;
            }
        }

        [Bindable(true), Category("Appearance"), Description("控件边框样式的宽度。"), DefaultValue("0px")]
        new public Unit BorderWidth
        {
            set
            {
                borderwidth = value;
            }
            get
            {
                if (!borderwidth.IsEmpty)
                {
                    return borderwidth;
                }
                return 0;
            }
        }

        [Bindable(true), Category("Appearance"), Description("控件边框样式。"), DefaultValue(BorderStyle.None)]
        new public BorderStyle BorderStyle
        {
            set
            {
                borderstyle = value;
            }
            get
            {
                return borderstyle;
            }
        }

        [Bindable(true), Category("Appearance"), Description("设置控件皮肤文件的路径。请使用相对路径，相对于system目录，路径最后需带斜杠。"), DefaultValue("skin/xp/")]
        public string Skin
        {
            set
            {
                ViewState["skin"] = value;
            }
            get
            {
                if (ViewState["skin"] == null)
                {
                    if (ConfigurationManager.AppSettings["skin"] == null)
                    {
                        return "skin/xp/";
                    }
                    else
                    {
                        return ConfigurationManager.AppSettings["skin"].ToString();
                    }
                }
                else
                {
                    return ViewState["skin"].ToString();
                }
            }
        }

        [Bindable(true), Category("Behavior"), Description("将鼠标放在控件时显示的工具提示。")]
        new public string ToolTip
        {
            set
            {
                tooltip = value;
            }
            get
            {
                return tooltip;
            }
        }

        [Bindable(true), Category("控件菜单栏"), Description("控件菜单栏背景色。"), DefaultValue("#EFEFEF"), TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter))]
        public Color ToolBarColor
        {
            set
            {
                toolbarcolor = value;
            }
            get
            {
                return toolbarcolor;
            }
        }

        [Bindable(true), Category("控件菜单栏"), Description("控件菜单栏背景图片的网络路径,默认为none。"), DefaultValue("none")]
        public string ToolBarBgImg
        {
            set
            {
                toolbarbgimg = value;
            }
            get
            {
                return toolbarbgimg;
            }
        }

        [Bindable(true), Category("控件菜单栏"), Description("设置控件可拖曳菜单工具栏位置记忆的失效时间(单位:小时)。"), DefaultValue(0)]
        public int ExpireHours
        {
            set
            {
                expirehours = value;
            }
            get
            {
                return expirehours;
            }
        }

        [Bindable(true), Category("高级设置"), Description("设置控件在代码状态时能否编辑，建议关闭。True=开启 False=关闭"), DefaultValue(true)]
        public bool Source
        {
            get
            {
                if (null == ViewState["Source"])
                    return true;
                else
                    return (bool)ViewState["Source"];
            }
            set { ViewState["Source"] = value; }
        }

        [Bindable(true), Category("高级设置"), Description("代码样式标题文字。")]
        public string CodeTitle
        {
            set
            {
                codetitle = value;
            }
            get
            {
                if (codetitle != "")
                {
                    return codetitle;
                }
                return "以下是代码片段：";
            }
        }

        [Bindable(true), Category("高级设置"), Description("代码样式边框色。"), TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter))]
        public Color CodeBorderColor
        {
            set
            {
                codebordercolor = value;
            }
            get
            {
                return codebordercolor;
            }
        }

        [Bindable(true), Category("高级设置"), Description("代码样式边框粗细。")]
        public Unit CodeBorder
        {
            set
            {
                codeborder = value;
            }
            get
            {
                if (!codeborder.IsEmpty)
                {
                    return codeborder;
                }
                return 1;
            }
        }

        [Bindable(true), Category("高级设置"), Description("代码样式边框样式。")]
        public BorderStyle CodeBorderStyle
        {
            set
            {
                codeborderstyle = value;
            }
            get
            {
                return codeborderstyle;
            }
        }

        [Bindable(true), Category("高级设置"), Description("代码样式背景色。"), TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter))]
        public Color CodeBackColor
        {
            set
            {
                codebackcolor = value;
            }
            get
            {
                return codebackcolor;
            }
        }

        [Bindable(true), Category("高级设置"), Description("引用样式标题文字。")]
        public string QuoteTitle
        {
            set
            {
                quotetitle = value;
            }
            get
            {
                if (quotetitle != "")
                {
                    return quotetitle;
                }
                return "以下是引用片段：";
            }
        }

        [Bindable(true), Category("高级设置"), Description("引用样式背景色。"), TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter))]
        public Color QuoteBackColor
        {
            set
            {
                quotebackcolor = value;
            }
            get
            {
                return quotebackcolor;
            }
        }

        [Bindable(true), Category("高级设置"), Description("引用样式边框色。"), TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter))]
        public Color QuoteBorderColor
        {
            set
            {
                quotebordercolor = value;
            }
            get
            {
                return quotebordercolor;
            }
        }

        [Bindable(true), Category("高级设置"), Description("引用样式边框粗细。")]
        public Unit QuoteBorder
        {
            set
            {
                quoteborder = value;
            }
            get
            {
                if (!quoteborder.IsEmpty)
                {
                    return quoteborder;
                }
                return 1;
            }
        }

        [Bindable(true), Category("高级设置"), Description("引用样式边框样式。"), DefaultValue(BorderStyle.Dotted)]
        public BorderStyle QuoteBorderStyle
        {
            set
            {
                quoteborderstyle = value;
            }
            get
            {
                return quoteborderstyle;
            }
        }

        [Bindable(true), Category("高级设置"), Description("设置扩展及收缩编辑框功能的增减幅度。"), DefaultValue(50)]
        public int AdjustSize
        {
            set
            {
                adjustsize = value;
            }
            get
            {
                return adjustsize;
            }
        }

        [Bindable(true), Category("高级设置"), Description("是否子控件。True=是，Flase=否"), DefaultValue(false)]
        public bool Child
        {
            set
            {
                child = value;
            }
            get
            {
                return child;
            }
        }

        [Bindable(true), Category("高级设置"), Description("是否自动保存编辑器内容里的远程图片到本地。True=是，Flase=否"), DefaultValue(false)]
        public bool AutoSaveImgToLocal
        {
            set
            {
                ViewState["AutoSaveImgToLocal"] = value;
            }
            get
            {
                if (ViewState["AutoSaveImgToLocal"] != null)
                {
                    return (bool)ViewState["AutoSaveImgToLocal"];
                }
                else
                {
                    return false;
                }
            }
        }

        [Bindable(true), Category("高级设置"), Description("插入文件及图片的路径形式。Relative=相对路径,AbsoluteRoot=绝对根路径,AbsoluteFull=绝对全路径"), DefaultValue(pathTypeName.AbsoluteFull)]
        public pathTypeName PathType
        {
            set
            {
                pathtype = value;
            }
            get
            {
                return pathtype;
            }
        }

        [Bindable(true), Category("高级设置"), Description("设置菜单功能配置文件。文件放置在system_dntb/menuconfig/目录下"), DefaultValue("default.config")]
        public string MenuConfig
        {
            set
            {
                ViewState["MenuConfig"] = value;
            }
            get
            {
                if (ViewState["MenuConfig"] == null)
                {
                    return "default.config";
                }
                else
                {
                    return ViewState["MenuConfig"].ToString();
                }

            }
        }

        [Bindable(true), Category("高级设置"), Description("设置System_dntb目录所在的路径，请使用相对路径，路径最后需带斜杠。"), DefaultValue("config/")]
        public string systemFolder
        {
            set
            {
                ViewState["systemFolder"] = value;
            }
            get
            {
                if (ViewState["systemFolder"] == null)
                {
                    if (ConfigurationManager.AppSettings["systemFolder"] == null)
                    {
                        return "system_dntb/";
                    }
                    else
                    {
                        return ConfigurationManager.AppSettings["systemFolder"].ToString();
                    }
                }
                else
                {
                    return ViewState["systemFolder"].ToString();
                }
            }
        }

        [Bindable(true), Category("高级设置"), Description("设置界面语言。"), DefaultValue("default")]
        public string Languages
        {
            set
            {
                languages = value;

            }
            get
            {
                return languages;
            }
        }

        [Bindable(true), Category("高级设置"), Description("转换内容为符合XHTML1.0编码规范的方式。None=不转换,Client=客户端转换,Server=服务端转换"), DefaultValue(XhtmlType.None)]
        public XhtmlType Xhtml
        {
            set
            {
                xhtml = value;
            }
            get
            {
                return xhtml;
            }
        }

        [Bindable(true), Category("高级设置"), Description("配置编辑器个性化右键菜单功能。")]
        public string RightClickMenuConfig
        {
            set
            {
                rightclickmenuconfig = value;

            }
            get
            {
                return rightclickmenuconfig;
            }
        }

        [Bindable(true), Category("高级设置"), Description("回车换行的模式,默认为<BR>。"), DefaultValue(NewLineType.BR)]
        public NewLineType NewLineMode
        {
            set
            {
                newlinemode = value;

            }
            get
            {
                return newlinemode;
            }
        }

        [Bindable(true), Category("高级设置"), Description("获取提交内容里所有图片的URL地址(网络路径)。")]
        public ArrayList GetImagesUrl
        {
            get
            {
                return geturl(Text, @"<IMG[^>]+src=\s*(?:'(?<src>[^']+)'|""(?<src>[^""]+)""|(?<src>[^>\s]+))\s*[^>]*>", "src");
            }
        }

        [Bindable(true), Category("高级设置"), Description("获取提交内容里所有FLV的URL地址(网络路径)。")]
        public ArrayList GetFlvUrl
        {
            get
            {
                return geturl(Text, @"<EMBED[^>]+file=\s*(?:'(?<file>[^']\S*.flv\b+)'|""(?<file>[^""]\S*.flv\b+)""|(?<file>[^>\s]\S*.flv\b+))\s*[^>]*>", "file");
            }
        }

        [Bindable(true), Category("高级设置"), Description("获取提交内容里所有Flash的URL地址(网络路径)。")]
        public ArrayList GetFlashUrl
        {
            get
            {
                return geturl(Text, @"<embed[^>]+src=\s*(?:'(?<src>[^']\S*.swf\b+)'|""(?<src>[^""]\S*.swf\b+)""|(?<src>[^>\s]\S*.swf\b+))\s*[^>]*>", "src");
            }
        }

        [Bindable(true), Category("高级设置"), Description("设置或获取自动分页功能中每页内容的字符长度，超出此长度则自动分页。默认为2000字符")]
        public int PageLength
        {
            set
            {
                pagelength = value;

            }
            get
            {
                return pagelength;
            }
        }

        [Bindable(true), Category("高级设置"), Description("获取自动分页内容的数组集合,通过getAutoPage[i].ToString()形式获取对应分页内容")]
        public ArrayList getAutoPage
        {
            get
            {
                ArrayList resultStr = new ArrayList();
                Double pagecont = Math.Round((Double)this.Length / this.PageLength, 3);
                if (pagecont > 1)
                {
                    for (int i = 0, j = 0; i < pagecont; i++)
                    {
                        resultStr.Add(subStringHTML(this.Text, this.PageLength, j));
                        j = j + this.PageLength;
                    }
                }
                else
                {
                    resultStr.Add(this.Text);
                }
                return resultStr;
            }
        }

        [Bindable(true), Category("高级设置"), Description("获取手动分页内容的数组集合,通过getManualPage[i].ToString()形式获取对应分页内容。")]
        public ArrayList getManualPage
        {
            get
            {
                ArrayList resultStr = new ArrayList();
                string[] htmlstr = Regex.Split(this.Text, "<hr id=pageoutput/>", RegexOptions.IgnoreCase);
                for (int i = 0; i < htmlstr.Length; i++)
                {
                    resultStr.Add(htmlstr[i]);
                }
                return resultStr;
            }
        }

        [Bindable(true), Category("高级设置"), Description("设置插入图片后将图片路径即时加入页面控件(如TextBox或Input)的ID名称,默认为不插入。")]
        public string getImagesPathID
        {
            set
            {
                targetid = value;
            }
            get
            {
                return targetid;
            }
        }

        [Bindable(true), Category("上传设置"), Description("设置上传文件夹的相对路径。路径最后需带斜杠且相对于system_dntb目录。"), DefaultValue("upload/")]
        public string UploadFolder
        {
            set
            {
                ViewState["UploadFolder"] = value;
            }
            get
            {
                if (ViewState["UploadFolder"] == null)
                {
                    return "upload/";
                }
                else
                {
                    return ViewState["UploadFolder"].ToString();
                }
            }
        }

        [Bindable(true), Category("上传设置"), Description("设置上传文件夹的空间大小。如果为0,则由上传配置文件决定空间大小。"), DefaultValue("0")]
        public string UploadFolderSize
        {
            set
            {
                ViewState["UploadFolderSize"] = value;
            }
            get
            {
                if (ViewState["UploadFolderSize"] == null)
                {
                    return "0";
                }
                else
                {
                    return ViewState["UploadFolderSize"].ToString();
                }
            }
        }

        [Bindable(true), Category("上传设置"), Description("设置上传功能配置文件的名称。"), DefaultValue("default.config")]
        public string UploadConfig
        {
            set
            {
                uploadconfig = value;
            }
            get
            {
                return uploadconfig;
            }
        }

        [Bindable(true), Category("Appearance"), Description("获取编辑控件内容的字符数，配合验证控件即可实现限制控件字符数。")]
        public Int32 Length
        {
            get
            {

                if (this.Text != null || this.Text != "")
                {
                    if (!this.DesignMode)
                    {
                        return PageCollection.NoHTML(this.Text).Length;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }


        [Bindable(true), Category("Layout"), Description("顶端菜单栏区域宽度。")]
        public string TopMenuWidth
        {
            set
            {
                ViewState["TopMenuWidth"] = value;
            }
            get
            {
                if (ViewState["TopMenuWidth"] == null)
                {
                    return "100%";
                }
                else
                {

                    return ViewState["TopMenuWidth"].ToString();
                }
            }
        }

        [Bindable(true), Category("Layout"), Description("顶端菜单栏区域对齐方式。"), DefaultValue(AlignStr.Left)]
        public AlignStr TopMenuAlign
        {
            set
            {
                ViewState["TopMenuAlign"] = value;
            }
            get
            {
                if (ViewState["TopMenuAlign"] == null)
                {
                    return AlignStr.Left;
                }
                else
                {
                    return (AlignStr)ViewState["TopMenuAlign"];
                }
            }
        }

        [Bindable(true), Category("Layout"), Description("顶端菜单栏区域高度。")]
        public string TopMenuHeight
        {
            set
            {
                ViewState["TopMenuHeight"] = value;
            }
            get
            {
                if (ViewState["TopMenuHeight"] == null)
                {
                    return "52px";
                }
                else
                {
                    return ViewState["TopMenuHeight"].ToString();
                }
            }
        }

        [Bindable(true), Category("Layout"), Description("状态栏高度。")]
        public string StatusHeight
        {
            set
            {
                ViewState["StatusHeight"] = value;
            }
            get
            {
                if (ViewState["StatusHeight"] == null)
                {
                    return "25px";
                }
                else
                {
                    return ViewState["StatusHeight"].ToString();
                }
            }
        }

        [Bindable(true), Category("Layout"), Description("右侧菜单栏区域宽度。(设置为0px即可隐藏右侧菜单栏)")]
        public string SideMenuWidth
        {
            set
            {
                ViewState["SideMenuWidth"] = value;
            }
            get
            {
                if (ViewState["SideMenuWidth"] == null)
                {
                    return "25px";
                }
                else
                {

                    return ViewState["SideMenuWidth"].ToString();
                }
            }
        }

        [Bindable(true), Category("Layout"), Description("编辑框高度。")]
        public string EditHeight
        {
            set
            {
                ViewState["EditHeight"] = value;
            }
            get
            {
                if (ViewState["EditHeight"] == null)
                {
                    return "210px";
                }
                else
                {
                    return ViewState["EditHeight"].ToString();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        new public string CssClass
        {
            get
            {
                return null;
            }
        }


      //  public override Unit  Width { get; set; }

        #endregion
        //自定义属性添加完成


        /// <summary>
        /// 将此控件呈现给指定的输出参数。
        /// </summary>
        /// <param name="writer">要写出到的 HTML 编写器</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            if (ViewState["value"] != null)
                writer.AddAttribute("value", ViewState["value"].ToString());
        }



        /// <summary>
        /// 获取自定义控件界面初始化的内容来输出控件的用户界面
        /// </summary>
		protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            bool KeyIsPass = true;
            InterFace getControl = new InterFace();
            if (BrowserIsOK(true))
            {
                if (this.Height.ToString() == "")
                    this.Height = 284;

                Int32 countH = Int32.Parse(this.TopMenuHeight.ToLower().Replace("px", "")) + Int32.Parse(this.EditHeight.ToLower().Replace("px", "")) + Int32.Parse(this.StatusHeight.ToLower().Replace("px", ""));

                if (countH != this.Height)
                    this.EditHeight = (Int32.Parse(this.EditHeight.ToLower().Replace("px", "")) + (this.Height.Value - countH)).ToString() + "px";


                if (this.Width.ToString() == "")
                    this.Width = 590;

                string browerType, HTTP_USER_AGENT, functionpath, skinpath, menuconfigpath, systemfolderpath, toolbarbgimages;
                if (ToolBarBgImg.ToLower() != "none")
                {
                    toolbarbgimages = "url(" + ToolBarBgImg + ")";
                }
                else
                {
                    toolbarbgimages = ToolBarBgImg;
                }

                if (!this.DesignMode)
                {
                    string apppath = HttpContext.Current.Request.ApplicationPath;
                    if (apppath == "/")
                        apppath = "";

                    if (ViewState["systemFolder"] == null)
                    {
                        if (ConfigurationManager.AppSettings["systemFolder"] == null)
                        {
                            systemfolderpath = HttpContext.Current.Server.MapPath(this.systemFolder);
                        }
                        else
                        {
                            systemfolderpath = HttpContext.Current.Request.PhysicalApplicationPath + "/" + this.systemFolder;
                        }
                    }
                    else
                    {
                        systemfolderpath = HttpContext.Current.Request.PhysicalApplicationPath + "/" + this.systemFolder;
                    }

                    skinpath = apppath + "/" + this.systemFolder + this.Skin;

                    functionpath = apppath + "/" + this.systemFolder;
                    HTTP_USER_AGENT = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToLower();
                    menuconfigpath = systemfolderpath + "menuconfig/" + this.MenuConfig;

                    if (HTTP_USER_AGENT.IndexOf("msie") == -1)
                    {
                        browerType = "Moz";

                        if (HTTP_USER_AGENT.IndexOf("opera") != -1)
                        {
                            HttpContext.Current.Response.Expires = 0;
                            browerType = "Opera";
                        }
                    }
                    else
                    {
                        browerType = "IE";
                    }

                    HttpContext.Current.Response.Cookies["uploadFolder"].Value = HttpContext.Current.Server.UrlPathEncode(UploadFolder);
                    if (Int32.Parse(this.UploadFolderSize) > 0)
                    {
                        HttpContext.Current.Response.Cookies["UploadFolderSize"].Value = UploadFolderSize;
                    }
                    HttpContext.Current.Response.Cookies["uploadConfig"].Value = HttpContext.Current.Server.UrlPathEncode(systemfolderpath + "uploadconfig/" + UploadConfig);
                    HttpContext.Current.Response.Cookies["configpath"].Value = HttpContext.Current.Server.UrlPathEncode(systemfolderpath);
                    string langcookie = HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"].ToLower().Split(',')[0];
                    if (this.Languages == "default")
                    {
                        HttpContext.Current.Response.Cookies["languages"].Value = langcookie;
                        ResourceManager.SiteLanguageKey = langcookie;
                    }
                    else
                    {
                        HttpContext.Current.Response.Cookies["languages"].Value = this.Languages;
                        ResourceManager.SiteLanguageKey = this.Languages;
                    }
                    //输出控件界面
                    writer.Write(getControl.getHtml(this.ClientID, this.Height.ToString(), this.Width.ToString(), menuconfigpath, Color.FromArgb(this.BorderColor.ToArgb()).Name.Substring(2), Color.FromArgb(this.toolbarcolor.ToArgb()).Name.Substring(2), toolbarbgimages, this.borderwidth.ToString(), this.borderstyle.ToString(), this.FrameShadow.ToString(), Color.FromArgb(this.EditBorderColor.ToArgb()).Name.Substring(2), this.tooltip.ToString(), skinpath, functionpath, this.TopMenuHeight, this.EditHeight, this.StatusHeight, this.TopMenuWidth.ToString(), this.TopMenuAlign.ToString(), browerType, SideMenuWidth, KeyIsPass));
                    //注册控件的客户端脚本
                    try
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(WebEditor), this.ClientID, getControl.getScript(this.ClientID, this.Source, Color.FromArgb(this.backcolor.ToArgb()).Name.Substring(2), this.focus, skinpath, functionpath, this.codetitle, Color.FromArgb(this.CodeBorderColor.ToArgb()).Name.Substring(2), Color.FromArgb(this.CodeBackColor.ToArgb()).Name.Substring(2), this.codeborderstyle.ToString(), this.codeborder.ToString(), this.quotetitle, Color.FromArgb(this.QuoteBackColor.ToArgb()).Name.Substring(2), Color.FromArgb(this.QuoteBorderColor.ToArgb()).Name.Substring(2), this.quoteborder.ToString(), this.quoteborderstyle.ToString(), browerType, this.AdjustSize, this.PathType.ToString(), this.Xhtml.ToString().ToLower(), this.RightClickMenuConfig, this.ExpireHours, this.NewLineMode.ToString().ToLower(), this.getImagesPathID), true);
                    }
                    catch
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(WebEditor), this.ClientID, getControl.getScript(this.ClientID, this.Source, Color.FromArgb(this.backcolor.ToArgb()).Name.Substring(2), this.focus, skinpath, functionpath, this.codetitle, Color.FromArgb(this.CodeBorderColor.ToArgb()).Name.Substring(2), Color.FromArgb(this.CodeBackColor.ToArgb()).Name.Substring(2), this.codeborderstyle.ToString(), this.codeborder.ToString(), this.quotetitle, Color.FromArgb(this.QuoteBackColor.ToArgb()).Name.Substring(2), Color.FromArgb(this.QuoteBorderColor.ToArgb()).Name.Substring(2), this.quoteborder.ToString(), this.quoteborderstyle.ToString(), browerType, this.AdjustSize, this.PathType.ToString(), this.Xhtml.ToString().ToLower(), this.RightClickMenuConfig, this.ExpireHours, this.NewLineMode.ToString().ToLower(), this.getImagesPathID), true);
                    }

                }
                else
                {
                    //设计状态下获取正确的配置文件路径
                    EnvDTE.DTE devenv = null;

                    //从vs2005(8),vs2008(9),vs2013(12),vs2015(14)支持到 100
                    for (int i = 8; i < 100; i++)
                    {
                        string v = string.Format("VisualStudio.DTE.{0}.0", i);
                        try
                        {
                            devenv = (EnvDTE.DTE)System.Runtime.InteropServices.Marshal.GetActiveObject(v);


                            if (devenv != null)
                                break;
                        }
                        catch //(Exception)
                        {
                            //throw;
                        }
                    }

                   // devenv = null;
                    if (devenv == null)
                    {
                        //输出错误信息
                        writer.Write("<style type=\"text/css\"><!--.error {font-size: 9pt;}--></style><center><p></p><table width=\"339\" border=\"1\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#333333\" style=\"border-collapse: collapse; border-style: dotted; border-width: 1\"><tr><td class=\"error\" width=\"335\" height=\"20\" bgcolor=\"#999999\"><font color=\"#FFFFFF\" face=\"Webdings\">&nbsp;.</font><font color=\"#FFFFFF\"><strong>错误提示</strong></font></td></tr><tr><td height=\"85\" bgcolor=\"#efefef\"><table width=\"100%\" height=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"error\"> <div align=\"center\"><font color=\"#666666\"><strong>无法创建DTE实例对象</strong></font></div></td></tr></table></td></tr></table></center>\r\n");
                        return;
                    }
                

                    //  devenv = (EnvDTE.DTE)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.8.0");

                   // Debug.WriteLine("显示调试信息"); //无法在设计时输出

                    string projectFile = devenv.ActiveDocument.ProjectItem.ContainingProject.FileName;
                    if (projectFile.IndexOf("http://") == -1)
                    {
                        System.IO.FileInfo info = new System.IO.FileInfo(projectFile);
                        projectFile = info.Directory.FullName;
                    }
                    browerType = "IE";
                    HTTP_USER_AGENT = "";
                    menuconfigpath = this.systemFolder + "menuconfig/" + this.MenuConfig;
                    if (!System.IO.File.Exists(menuconfigpath))
                    {
                        menuconfigpath = projectFile + "/" + menuconfigpath;
                    }
                    functionpath = this.systemFolder;
                    skinpath = this.systemFolder + this.Skin;
                    if (!System.IO.Directory.Exists(skinpath))
                    {
                        skinpath = projectFile + "/" + skinpath;
                    }
                    if (this.Languages == "default")
                    {
                        //控件默认的语言
                        ResourceManager.SiteLanguageKey = "zh-cn";
                    }
                    else
                    {
                        ResourceManager.SiteLanguageKey = this.Languages;
                    }
                    //输出控件设计时界面
                    writer.Write(getControl.getHtml(this.ClientID, this.Height.ToString(), this.Width.ToString(), menuconfigpath, Color.FromArgb(this.BorderColor.ToArgb()).Name.Substring(2), Color.FromArgb(this.toolbarcolor.ToArgb()).Name.Substring(2), toolbarbgimages, this.borderwidth.ToString(), this.borderstyle.ToString(), this.FrameShadow.ToString(), Color.FromArgb(this.EditBorderColor.ToArgb()).Name.Substring(2), this.tooltip.ToString(), skinpath, functionpath, this.TopMenuHeight, this.EditHeight, this.StatusHeight, this.TopMenuWidth.ToString(), this.TopMenuAlign.ToString(), browerType, SideMenuWidth, KeyIsPass));
                }




            }
            else
            {
                //输出错误信息
                writer.Write("<style type=\"text/css\"><!--.error {font-size: 9pt;}--></style><center><p></p><table width=\"339\" border=\"1\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#333333\" style=\"border-collapse: collapse; border-style: dotted; border-width: 1\"><tr><td class=\"error\" width=\"335\" height=\"20\" bgcolor=\"#999999\"><font color=\"#FFFFFF\" face=\"Webdings\">&nbsp;.</font><font color=\"#FFFFFF\"><strong>错误提示</strong></font></td></tr><tr><td height=\"85\" bgcolor=\"#efefef\"><table width=\"100%\" height=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"error\"> <div align=\"center\"><font color=\"#666666\"><strong>客户浏览器或操作系统不符合服务器控件的以下要求：</strong><br /><br />操作系统：WIN9X/ME/NT/2K/XP/VISTA<br /><br />浏览器：IE5.5+、FireFox1.0+、Opera9.0+兼容浏览器，需支持Javascript客户端语言及Cookie!</font></div></td></tr></table></td></tr></table></center>\r\n");
            }
        }

        /// <summary>
        /// 注册控件脚本资源
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                //if (HttpContext.Current.Request.IsLocal)
                //{
                if (!Child)
                {
                    string temppath = systemFolder.ToLower().Replace("system_dntb/", "");
                    if (temppath != "")
                    {
                        if (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().IndexOf(temppath) == -1)
                        {
                            temppath = "";
                        }
                    }

                    if (HttpContext.Current.Request.ApplicationPath != "/")
                    {
                        temppath = "/" + temppath;
                    }
                    Page.ClientScript.RegisterClientScriptBlock(typeof(WebEditor), "define", "PathType='" + this.PathType.ToString() + "',urlpath='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + temppath + "';", true);


                    if (HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().IndexOf("msie") == -1)
                    {
                        Page.ClientScript.RegisterClientScriptResource(typeof(WebEditor), "DotNetTextBox.core_gecko.js");
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptResource(typeof(WebEditor), "DotNetTextBox.core.js");
                    }
                    Page.ClientScript.RegisterClientScriptResource(typeof(WebEditor), "DotNetTextBox.share.js");
                }
                //}
            }
            base.OnLoad(e);
        }

        /// <summary>
        /// 自定义控件的回调事件
        /// </summary>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            bool raiseEvent = false;
            if (this.Text != postCollection[postDataKey])
                raiseEvent = true;

            this.Text = postCollection[postDataKey];
            return raiseEvent;
        }

        /// <summary>
        /// 自定义控件的动作事件
        /// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            EventHandler handler = (EventHandler)Events[_textChanged];
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// 检查客户端浏览器是否兼容控件
        /// </summary>
		private bool BrowserIsOK(bool flag)
        {
            if (!this.DesignMode)
            {
                if (HttpContext.Current.Request.Browser.EcmaScriptVersion.Major < 1)
                {
                    flag = false;
                }
                if (!HttpContext.Current.Request.Browser.Cookies)
                {
                    flag = false;
                }
                return flag;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 提交时获取内容中的各种地址
        /// </summary>
        private ArrayList geturl(string html, string regstr, string keyname)
        {
            ArrayList resultStr = new ArrayList();
            Regex r = new Regex(regstr, RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(html);

            foreach (Match m in mc)
            {
                resultStr.Add(m.Groups[keyname].Value.ToLower());
            }
            if (resultStr.Count > 0)
            {
                return resultStr;
            }
            else
            {
                //没有地址的时候返回空字符
                resultStr.Add("");
                return resultStr;
            }
        }

        public String subStringHTML(String param, int length, int starchar)
        {
            StringBuilder result = new StringBuilder();
            int n = 0;
            string temp;
            Boolean isCode = false; //是不是HTML代码
            Boolean isHTML = false; //是不是HTML特殊字符,如&nbsp;

            for (int i = 0; i < param.Length; i++)
            {
                temp = param[i].ToString();
                if (temp == "<")
                {
                    isCode = true;
                }
                else if (temp == "&")
                {
                    isHTML = true;
                }
                else if (temp == ">" && isCode)
                {
                    n = n - 1;
                    isCode = false;
                }
                else if (temp == ";" && isHTML)
                {
                    isHTML = false;
                }

                if (!isCode && !isHTML)
                {
                    n = n + 1;
                }

                if (n <= starchar + length && n > 0 ? n > starchar : n >= starchar)
                {
                    result.Append(temp);
                }
                if (n >= starchar + length)
                {
                    break;
                }
            }

            //取出截取字符串中的HTML标记
            String temp_result = result.ToString();
            //去掉HTML标记属性
            string[] HtmlTag = new string[] { "p", "div", "span", "table", "ul", "font", "b", "u", "i", "a", "h1", "h2", "h3", "h4", "h5", "h6" };
            for (int k = 0; k < HtmlTag.Length; k++)
            {

                temp_result = Regex.Replace(temp_result, "<(   )*" + HtmlTag[k] + "([^>])*>", "<" + HtmlTag[k] + ">", RegexOptions.IgnoreCase);
                temp_result = Regex.Replace(temp_result, "(<(   )*(/)(   )*" + HtmlTag[k] + "(   )*>)", "</" + HtmlTag[k] + ">", RegexOptions.IgnoreCase);
                //去掉成对的HTML标记
                temp_result = Regex.Replace(temp_result, "<" + HtmlTag[k] + ">(.*?)</" + HtmlTag[k] + ">", "");
            }

            Regex p = new Regex("<([a-zA-Z]+)[^<>]*>");//里面放正则表达式
            MatchCollection m = p.Matches(temp_result);//如果想得到匹配的多个值,就用集合



            ArrayList endHTML = new ArrayList();

            foreach (Match ms in m)
            {
                endHTML.Add(ms.ToString().Replace("<", "").Replace(">", ""));
            }
            for (int i = endHTML.Count - 1; i >= 0; i--)
            {
                result.Append("</");
                result.Append(endHTML[i].ToString());
                result.Append(">");
            }

            return result.ToString();
        }

        /// <summary>
        /// 提交时自动保存编辑器内容里的远程图片
        /// </summary>
        public string saveRemotingImg(string content)
        {
            string uploadpath = UploadFolder, systemfolderpath = "";
            int picid = 0;
            System.Net.HttpWebRequest hwq;
            System.Net.HttpWebResponse hwr;
            XmlDocument config = new XmlDocument();
            Regex re = new Regex(@"<img.*?src\s*=\s*(?:([""'])(?<src>[^""']+)\1|(?<src>[^\s>]+))", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection mc = re.Matches(content);

            if (ViewState["systemFolder"] == null)
            {
                if (ConfigurationManager.AppSettings["systemFolder"] == null)
                {
                    systemfolderpath = HttpContext.Current.Server.MapPath(this.systemFolder);
                }
                else
                {
                    systemfolderpath = HttpContext.Current.Request.PhysicalApplicationPath + this.systemFolder;
                }
            }
            else
            {
                systemfolderpath = HttpContext.Current.Request.PhysicalApplicationPath + "/" + this.systemFolder;
            }
            config.Load(systemfolderpath + "uploadconfig/" + this.UploadConfig);
            Double maxSingleUploadSize = Double.Parse(config.SelectNodes("//configuration/maxSingleUploadSize")[0].InnerText.Trim()) * 1024;
            string path = systemfolderpath + uploadpath;
            string relativepath = "", checkpath = "";
            if (HttpContext.Current.Request.ApplicationPath != "/")
            {
                checkpath = HttpContext.Current.Request.Path.ToLower().Replace(HttpContext.Current.Request.ApplicationPath.ToLower() + "/", "");
            }
            else
            {
                checkpath = HttpContext.Current.Request.Path.ToLower().Remove(0, 1);
            }
            for (int i = 0; i < checkpath.Split('/').Length - 1; i++)
            {
                relativepath += "../";
            }

            foreach (Match m in mc)
            {
                string url = m.Groups["src"].Value;
                string filepath = url;

                if (url.Substring(0, 7).ToLower() == "http://" && url.ToLower().IndexOf(HttpContext.Current.Request.Url.Host.ToLower()) == -1)
                {
                    string KuoZhangMing = "." + url.Substring(url.LastIndexOf(".") + 1);
                    string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + picid++.ToString() + KuoZhangMing;

                    filepath = relativepath + this.systemFolder + uploadpath + filename;

                    hwq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    hwr = (System.Net.HttpWebResponse)hwq.GetResponse();

                    if (hwr.ContentLength < maxSingleUploadSize)
                    {
                        string watermarkon = config.SelectNodes("//configuration/watermark")[0].InnerText.Trim().ToLower();
                        string watermarkImageson = config.SelectNodes("//configuration/watermarkImages")[0].InnerText.Trim().ToLower();
                        System.Drawing.Image saveimg = System.Drawing.Image.FromStream(hwr.GetResponseStream());

                        //为自动上传图片添加文字水印
                        if (watermarkon == "true")
                        {
                            if (KuoZhangMing.ToLower().Replace(".", string.Empty) != "gif")
                            {
                                try
                                {
                                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(saveimg);
                                    g.DrawImage(saveimg, 0, 0, saveimg.Width, saveimg.Height);
                                    System.Drawing.Font f = new System.Drawing.Font("宋体", 12);
                                    System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.ColorTranslator.FromHtml("#000000"));

                                    g.DrawString(config.SelectNodes("//configuration/watermarkText")[0].InnerText.Trim(), f, b, 15, 15);
                                    g.Dispose();
                                    if (watermarkImageson != "true")
                                    {
                                        saveimg.Save(path + filename);
                                        saveimg.Dispose();
                                    }
                                }
                                catch
                                {
                                    saveimg.Save(path + filename);
                                    saveimg.Dispose();
                                }
                            }
                            else
                            {
                                WaterMark gifmark = new WaterMark();
                                gifmark.WaterMarkType = MarkType.Text;
                                gifmark.SourceImage = saveimg;
                                gifmark.Text = config.SelectNodes("//configuration/watermarkText")[0].InnerText.Trim();
                                gifmark.TextFontFamilyStr = "宋体";
                                gifmark.TextColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                                gifmark.MarkX = 15;
                                gifmark.MarkY = 15;
                                gifmark.TextFontSize = 12;
                                gifmark.Mark();
                                if (watermarkImageson != "true")
                                {
                                    gifmark.MarkedImage.Save(path + filename);
                                    saveimg.Dispose();
                                }
                                else
                                {
                                    saveimg.Dispose();
                                    saveimg = gifmark.MarkedImage;
                                }
                            }
                        }

                        //为自动上传图片添加图片水印
                        if (watermarkImageson == "true")
                        {
                            if (KuoZhangMing.ToLower().Replace(".", string.Empty) != "gif")
                            {
                                System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Page.Server.MapPath("/" + this.systemFolder + config.SelectNodes("//configuration/watermarkImages_path")[0].InnerText.Trim()));
                                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(saveimg);
                                g.DrawImage(copyImage, new System.Drawing.Rectangle(15, 15, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
                                g.Dispose();
                                saveimg.Save(path + filename);
                                saveimg.Dispose();
                            }
                            else
                            {
                                WaterMark gifmark = new WaterMark();
                                gifmark.WaterMarkType = MarkType.Image;
                                gifmark.SourceImage = saveimg;
                                gifmark.WaterImagePath = Page.Server.MapPath("/" + this.systemFolder + config.SelectNodes("//configuration/watermarkImages_path")[0].InnerText.Trim());
                                gifmark.MarkX = 15;
                                gifmark.MarkY = 15;
                                gifmark.Mark();
                                gifmark.MarkedImage.Save(path + filename);
                                saveimg.Dispose();
                            }
                        }

                        //如果没有设置任何水印，则直接保存原始图片
                        if (watermarkImageson != "true" & watermarkon != "true")
                        {
                            saveimg.Save(path + filename);
                            saveimg.Dispose();
                        }
                    }
                    content = content.Replace(url, filepath);
                    hwr.Close();
                }
            }
            ViewState["value"] = content;
            return content;
        }
    }

    /// <summary>
    /// 自定义属性所用到枚举属性类型
    /// </summary>
    public enum ShadowType
    {
        Close = 0,
        Open = 1,
    }

    /// <summary>
    /// 自定义属性所用到枚举属性类型
    /// </summary>
    public enum pathTypeName
    {
        Relative = 0,
        AbsoluteRoot = 1,
        AbsoluteFull = 2,
    }

    /// <summary>
    /// 自定义属性所用到枚举属性类型
    /// </summary>
    public enum AlignStr
    {
        Left = 0,
        Center = 1,
        Right = 2,
    }

    /// <summary>
    /// XHTML属性所用到枚举属性类型
    /// </summary>
    public enum XhtmlType
    {
        None = 0,
        Client = 1,
        Server = 2,
    }

    /// <summary>
    /// newlinemode属性所用到枚举属性类型
    /// </summary>
    public enum NewLineType
    {
        BR = 0,
        P = 1,
    }
    #endregion


    #region 生成控件界面的HTML代码
    /// <summary>
    /// 根据自定义控件的属性值来初始化控件界面的HTML代码和客户端脚本
    /// </summary>
    public sealed class InterFace
    {
        private StringBuilder TextBoxDoc = new StringBuilder(), topMenu = new StringBuilder(), statusMenu = new StringBuilder(), bottommenu = new StringBuilder(), sidemenu = new StringBuilder(), TextBoxScript = new StringBuilder();
        private Int32 divid = 0, dragid = 0;

        #region 生成控件界面的HTML代码
        /// <summary>
        /// 生成控件界面的HTML代码
        /// </summary>

        public string getHtml(string tid, string th, string tw, string menuconfig, string bordercolor, string toolbarcolor, string toolbarbgimg, string borderwidth, string borderstyle, string frameshadow, string editbordercolor, string tooltip, string skin, string function, string toolbarheight, string editheight, string statusheight, string topwidth, string topalign, string browertype, string sidew, bool KeyIsPass)
        {
            if (KeyIsPass)
            {
                string dom0 = "", dom1 = "", dom2 = "", dom3 = "", dragend = "", customize = "";
                XmlReader reader;
                string[] parameter;
                reader = XmlReader.Create(menuconfig);
                try
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            customize = reader.Name;
                        }

                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            switch (customize.ToLower())
                            {
                                case "topmenu":
                                    topMenu.Append(getFunction(reader.Value, tid, skin, function));
                                    break;
                                case "plugin_topmenu":
                                    parameter = reader.Value.Split(',');
                                    topMenu.Append(getPlugin(parameter, tid, skin, function));
                                    break;
                                case "plugin_statusmenu":
                                    parameter = reader.Value.Split(',');
                                    statusMenu.Append(getPlugin(parameter, tid, skin, function));
                                    break;
                                case "plugin_bottommenu":
                                    parameter = reader.Value.Split(',');
                                    bottommenu.Append(getPlugin(parameter, tid, skin, function));
                                    break;
                                case "plugin_sidemenu":
                                    parameter = reader.Value.Split(',');
                                    sidemenu.Append(getPlugin(parameter, tid, skin, function));
                                    break;
                                case "statusmenu":
                                    statusMenu.Append(getFunction(reader.Value, tid, skin, function));
                                    break;
                                case "bottommenu":
                                    bottommenu.Append(getFunction(reader.Value, tid, skin, function));
                                    break;
                                case "sidemenu":
                                    sidemenu.Append(getFunction(reader.Value, tid, skin, function));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                    reader.Close();
                }
                reader.Close();
                dom0 = "<div id='dom0'>";
                dom1 = "<div id='dom1'>";
                dom2 = "<div id='dom2'>";
                dom3 = "<div id='dom3' style=\"width:" + sidew + "\">";
                dragend = "</div>";
                TextBoxDoc.Append("<!--DotNetTextBox v5.0，AspxCn.com.cn All Rights Reserved，email:webmaster@aspxcn.com.cn。-->\r\n");
                if (browertype == "IE")
                {
                    TextBoxDoc.Append("<link href='" + skin + "toolbar.css' rel=\"stylesheet\" type=\"text/css\" />\r\n");
                }
                else if (browertype == "Opera")
                {
                    dom2 = "<div id='dom2' style=\"float:right;width:130px\">";
                    tw = Int32.Parse(tw.Replace("px", "")) + 15 + "px";
                    TextBoxDoc.Append("<link href='" + skin + "toolbar_opera.css' rel=\"stylesheet\" type=\"text/css\" />\r\n");
                }
                else
                {
                   if(!string.IsNullOrEmpty(tw)&&!tw.EndsWith("%"))
                        tw = Int32.Parse(tw.Replace("px", "")) + 5 + "px";

                    dom2 = "<div id='dom2' style=\"float:right;width:130px\">";
                    if (HttpContext.Current.Request.Browser.MajorVersion < 3)
                    {
                        TextBoxDoc.Append("<link href='" + skin + "toolbar_moz.css' rel=\"stylesheet\" type=\"text/css\" />\r\n");
                    }
                    else
                    {
                        TextBoxDoc.Append("<link href='" + skin + "toolbar_moz3.css' rel=\"stylesheet\" type=\"text/css\" />\r\n");
                    }
                }

                TextBoxDoc.Append("<DIV id='fullscreendiv_" + tid + "'><TABLE id='toolbar_" + tid + "' width=" + tw + " style=\"border-style:" + borderstyle + ";border-width:" + borderwidth + ";border-color:#" + bordercolor + ";background-image:" + toolbarbgimg + "\" border=\"0\" cellPadding=\"0\" cellSpacing=\"0\" title=" + tooltip + ">\r\n");
                TextBoxDoc.Append("<tr><td rowspan=\"3\" valign=\"top\" style='padding-bottom:5px;padding-top:5px;BACKGROUND-COLOR: #" + toolbarcolor + ";height:100%;width:" + sidew + "'>" + dom3 + sidemenu.ToString() + dragend + "</td>\r\n");
                TextBoxDoc.Append("<TD id='toolbarbox_" + tid + "' colspan=\"2\" align='" + topalign + "' valign='middle' style=\"height=" + toolbarheight + ";width:" + topwidth + ";padding-bottom:3px;padding-top:3px;BACKGROUND-COLOR: #" + toolbarcolor + ";background-image:" + toolbarbgimg + "\">" + dom0 + topMenu.ToString() + dragend);
                TextBoxDoc.Append("</TD></TR>");
                TextBoxDoc.Append("<TR>");
                TextBoxDoc.Append("<TD colspan=\"2\" id='editbox_" + tid + "' height=\"100%\" style=\"BORDER:1px #" + editbordercolor + " double\" width=\"100%\">\r\n");
                if (frameshadow == "Close")
                {
                    frameshadow = "0";
                }
                else
                    frameshadow = "1";
                if (browertype != "IE")
                {
                    TextBoxDoc.Append("<IFRAME id=\"dntb_" + tid + "\" width=\"100%\" height=" + editheight + " frameBorder=" + frameshadow + "></IFRAME>\r\n");
                }
                else
                {
                    TextBoxDoc.Append("<IFRAME id=\"dntb_" + tid + "\" width=\"100%\" height=" + editheight + " frameBorder=" + frameshadow + " onblur='javascript:" + tid + "_CopyEditContent()'></IFRAME>\r\n");
                }

                TextBoxDoc.Append("</TD>\r\n");
                TextBoxDoc.Append("</TR><tr>\r\n");
                TextBoxDoc.Append("<TD id='statusbox_" + tid + "' height=\"" + statusheight + "\" style='text-align:left;BACKGROUND-COLOR: #" + toolbarcolor + ";;background-image:" + toolbarbgimg + "'>" + dom1 + bottommenu.ToString() + dragend + "</td>\r\n");
                TextBoxDoc.Append("<TD height=\"" + statusheight + "\" style=\"text-align:right;BACKGROUND-COLOR: #" + toolbarcolor + ";;background-image:" + toolbarbgimg + "\" >" + dom2 + "\r\n" + statusMenu.ToString() + dragend + "</td></tr>\r\n");
                TextBoxDoc.Append("</Table></DIV>");
            }
            else
            {
                TextBoxDoc.Append("<style type=\"text/css\"><!--.error {font-size: 9pt;text-decoration: none;color: #666666;}--></style><center><p></p><table width=\"339\" border=\"1\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#333333\" style=\"border-collapse: collapse; border-style: dotted; border-width: 1\"><tr><td class=\"error\" width=\"335\" height=\"20\" bgcolor=\"#999999\"><font color=\"#FFFFFF\" face=\"Webdings\">&nbsp;.</font><font color=\"#FFFFFF\"><strong> 错误提示</strong></font></td></tr><tr><td height=\"85\" bgcolor=\"#efefef\"><table width=\"100%\" height=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"error\"> <div align=\"center\"><p><strong>服务器控件授权域名可能产生以下错误：</strong><font color=\"#666666\"><br><br></font>运行控件的域名与授权不匹配</p><p align=\"right\"><a href='http://www.aspxcn.com.cn/default.aspx?uid=86' target=_blank><font class=\"error\">&gt;&gt;&gt;按此购买与此域名匹配的控件授权许可</font></a>&nbsp;</font></p></div></td></tr></table></td></tr></table></center>\r\n");
            }
            topMenu = null;
            bottommenu = null;
            sidemenu = null;
            statusMenu = null;
            return TextBoxDoc.ToString();
        }
        #endregion

        #region  生成控件界面的客户端脚本代码
        /// <summary>
        /// 生成控件界面的客户端脚本代码
        /// </summary>

        public string getScript(string tid, bool source, string bgcolor, bool focus, string skin, string function, string codetitle, string codebordercolor, string codebackcolor, string codeborderstyle, string codeborder, string quotetitle, string quotebackcolor, string quotebordercolor, string quoteborder, string quoteborderstyle, string browertype, int adjustsize, string PathType, string xhtml, string rightclickmenuconfig, int expirehours, string newlinemode, string getImagesPathID)
        {
            StringBuilder varStr = new StringBuilder();
            TextBoxScript.Append("var dntb_" + tid + "=document.getElementById(\"dntb_" + tid + "\").contentWindow;\r\n");
            string[] rcmenucfg = rightclickmenuconfig.ToLower().Split(',');
            string strname;
            foreach (string str in rcmenucfg)
            {
                switch (str)
                {
                    case "delete":
                        strname = "del";
                        break;
                    case "fullscreen":
                        strname = "full_screen";
                        break;
                    case "pastetext":
                    case "pasteword":
                        strname = str + "_str";
                        break;
                    default:
                        strname = str;
                        break;
                }
                if (varStr.ToString() == "")
                {
                    varStr.Append("var " + strname + "='" + ResourceManager.GetString(str) + "'");
                }
                else
                {
                    varStr.Append("," + strname + "='" + ResourceManager.GetString(str) + "'");
                }
            }
            TextBoxScript.Append(varStr.ToString() + ";\r\n");
            varStr = null;
            TextBoxScript.Append("var newlinemode='" + newlinemode + "',getimagespathid='" + getImagesPathID + "',xhtmltype='" + xhtml + "',expirehours=" + expirehours + ",domid=" + dragid + ", warning='" + ResourceManager.GetString("warning") + "',removedwordformat='" + ResourceManager.GetString("removedwordformat") + "',editlink='" + ResourceManager.GetString("editlink") + "',addlink='" + ResourceManager.GetString("addlink") + "',codetitle='" + codetitle + "',quotetitle='" + quotetitle + "',rcmenucfg='" + rightclickmenuconfig + "';");
            TextBoxScript.Append("var shine='" + ResourceManager.GetString("shine") + "',nowcolorstr='" + ResourceManager.GetString("nowcolor") + "',custom='" + ResourceManager.GetString("customfont") + "',customcolorstr='" + ResourceManager.GetString("customcolor") + "',msncheck='" + ResourceManager.GetString("msncheck") + "',icqcheck='" + ResourceManager.GetString("icqcheck") + "',sendmsg='" + ResourceManager.GetString("sendmsg") + "',errorparameter='" + ResourceManager.GetString("errorparameter") + "',replacesuccessful='" + ResourceManager.GetString("replacesuccessful") + "',notfound='" + ResourceManager.GetString("notfound") + "',findsuccessful='" + ResourceManager.GetString("findsuccessful") + "',setupfont='" + ResourceManager.GetString("setupfont") + "',inputcolorcode='" + ResourceManager.GetString("inputcolorcode") + "';");
            TextBoxScript.Append("var simsun='" + ResourceManager.GetString("simsun") + "',simhei='" + ResourceManager.GetString("simhei") + "',stliti='" + ResourceManager.GetString("stliti") + "',simyou='" + ResourceManager.GetString("simyou") + "',simkai='" + ResourceManager.GetString("simkai") + "',simfang='" + ResourceManager.GetString("simfang") + "',newsimsun='" + ResourceManager.GetString("newsimsun") + "',stcaiyun='" + ResourceManager.GetString("stcaiyun") + "',stfangso='" + ResourceManager.GetString("stfangso") + "',stxinwei='" + ResourceManager.GetString("stxinwei") + "',normal='" + ResourceManager.GetString("normal") + "',address='" + ResourceManager.GetString("address") + "',redo='" + ResourceManager.GetString("redo") + "',undo='" + ResourceManager.GetString("undo") + "';\r\n");
            TextBoxScript.Append("var skin='" + skin + "',functionstr='" + function + "',adjustsize=" + adjustsize + ",sourcestr=" + source.ToString().ToLower() + ",dd='" + ResourceManager.GetString("dd") + "',dt='" + ResourceManager.GetString("dt") + "',menulist='" + ResourceManager.GetString("menu") + "',dirlist='" + ResourceManager.GetString("dir") + "',pre='" + ResourceManager.GetString("pre") + "',selfontsize='" + ResourceManager.GetString("fontsize") + "',delhtmltag='" + ResourceManager.GetString("delhtmltag") + "',delwordtag='" + ResourceManager.GetString("delwordtag") + "',delstyletag='" + ResourceManager.GetString("delstyletag") + "',delfonttag='" + ResourceManager.GetString("delfonttag") + "',delspantag='" + ResourceManager.GetString("delspantag") + "',cleancodesuccessful='" + ResourceManager.GetString("cleancodesuccessful") + "';\r\n");
            TextBoxScript.Append("var blink='" + ResourceManager.GetString("blink") + "',marquee='" + ResourceManager.GetString("marquee") + "',delline='" + ResourceManager.GetString("delline") + "',big='" + ResourceManager.GetString("big") + "',small='" + ResourceManager.GetString("small") + "',h1='" + ResourceManager.GetString("h1") + "',h2='" + ResourceManager.GetString("h2") + "',h3='" + ResourceManager.GetString("h3") + "',h4='" + ResourceManager.GetString("h4") + "',h5='" + ResourceManager.GetString("h5") + "',h6='" + ResourceManager.GetString("h6") + "',paragraph='" + ResourceManager.GetString("paragraph") + "';");
            TextBoxScript.Append("var bMode=true,sHeader='<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=" + HttpContext.Current.Response.ContentEncoding.WebName + "\"><link href=\"" + skin + "editor.css\" rel=\"stylesheet\" type=\"text/css\" /></head><body class=\"editor\" topmargin=\"0\" leftmargin=\"0\" contentEditable=true bgcolor=#" + bgcolor + "></body></html>',sHeader2='<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=" + HttpContext.Current.Response.ContentEncoding.WebName + "\"><base target=\"_blank\" /><link href=\"" + skin + "editor.css\" rel=\"stylesheet\" type=\"text/css\" /><body class=\"editor\" topmargin=\"0\" leftmargin=\"0\" bgcolor=#" + bgcolor + "></body></html>'\r\n");
            TextBoxScript.Append("if (dntb_" + tid + ".value == null) dntb_" + tid + ".value = dntb_" + tid + ".innerHTML;\r\n");
            TextBoxScript.Append("dntb_" + tid + ".document.designMode=\"on\";\r\n");
            TextBoxScript.Append("dntb_" + tid + ".document.open();\r\n");
            TextBoxScript.Append("dntb_" + tid + ".document.write(sHeader+(document.getElementById('" + tid + "')).value);\r\n");
            TextBoxScript.Append("dntb_" + tid + ".document.close();\r\n");
            TextBoxScript.Append("window.onload=function(){onloadfunction('" + tid + "');}\r\ndocument.onmouseup=function(){onmouseupfunction('" + tid + "');}\r\ndocument.onmousemove=function(e){e=e||event;if(dragobj.o!=null){dragobj.o.style.left=(e.x-dragobj.xx[0])+\"px\";dragobj.o.style.top=(e.y-dragobj.xx[1])+\"px\";createtmpl(e,dragobj.o,'" + tid + "');}}\r\n");
            if (browertype != "IE")
            {
                TextBoxScript.Append("document.getElementById('" + tid + "_fontnametable')?eval(document.getElementById('" + tid + "_fontnametable').onload()):null;document.getElementById('" + tid + "_fontsizetable')?eval(document.getElementById('" + tid + "_fontsizetable').onload()):null;document.getElementById('" + tid + "_paragraphtable')?eval(document.getElementById('" + tid + "_paragraphtable').onload()):null;document.getElementById('" + tid + "_specialtypetable')?eval(document.getElementById('" + tid + "_specialtypetable').onload()):null;\r\n");
                TextBoxScript.Append("document.onclick=function(){" + tid + "_CopyEditContent();}\r\n");
                TextBoxScript.Append("document.onfocus=function(){" + tid + "_CopyEditContent();}\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.addEventListener('mouseup',function(e){checkformat('" + tid + "')},true);\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.addEventListener('keyup',function(e){checkformat('" + tid + "')},true);\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.addEventListener('click',popupmenu_hide,false);\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.addEventListener('dblclick',function(e){dbclickcheck(e);},true);\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.addEventListener('contextmenu',rc=function(e){addmenu('" + tid + "');e.cancelBubble = true;},true);\r\n");
                TextBoxScript.Append("var tid='" + tid + "';\r\n");
                if (focus)
                {
                    TextBoxScript.Append("if(opera){document.getElementById(\"dntb_" + tid + "\").focus();}else{dntb_" + tid + ".focus();}\r\n");
                }

                TextBoxScript.Append("function " + tid + "_CopyEditContent()\r\n");
                TextBoxScript.Append("{\r\n");
                TextBoxScript.Append("if(bMode||EditMode=='VIEW')\r\n");
                TextBoxScript.Append("{\r\n");
                if (PathType != "AbsoluteFull")
                {
                    TextBoxScript.Append("document.getElementById('" + tid + "').value = urlchange(dntb_" + tid + ".document.body.innerHTML);\r\n");
                    TextBoxScript.Append("}\r\n");
                    TextBoxScript.Append("else\r\n");
                    TextBoxScript.Append("{\r\n");
                    TextBoxScript.Append("document.getElementById('" + tid + "').value = urlchange(dntb_" + tid + ".document.body.textContent);");
                    TextBoxScript.Append("}\r\n");
                }
                else
                {
                    TextBoxScript.Append("document.getElementById('" + tid + "').value = dntb_" + tid + ".document.body.innerHTML;\r\n");
                    TextBoxScript.Append("}\r\n");
                    TextBoxScript.Append("else\r\n");
                    TextBoxScript.Append("{\r\n");
                    TextBoxScript.Append("document.getElementById('" + tid + "').value = dntb_" + tid + ".document.body.textContent;");
                    TextBoxScript.Append("}\r\n");
                }
                TextBoxScript.Append("}\r\n");
            }
            else
            {
                TextBoxScript.Append("document.getElementById('" + tid + "_fontnametable')?eval(document.getElementById('" + tid + "_fontnametable').onload):null;document.getElementById('" + tid + "_fontsizetable')?eval(document.getElementById('" + tid + "_fontsizetable').onload):null;document.getElementById('" + tid + "_paragraphtable')?eval(document.getElementById('" + tid + "_paragraphtable').onload):null;document.getElementById('" + tid + "_specialtypetable')?eval(document.getElementById('" + tid + "_specialtypetable').onload):null;\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.oncontextmenu = function(){addmenu('" + tid + "');return false;};\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.attachEvent('onclick',popupmenu_hide);\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.attachEvent('onmouseup',function(e){checkformat('" + tid + "')});\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.attachEvent('onkeyup',function(e){checkformat('" + tid + "')});\r\n");

                if (newlinemode == "br")
                {
                    TextBoxScript.Append("dntb_" + tid + ".document.attachEvent('onkeypress',function(e){insertBr(e,dntb_" + tid + ")});\r\n");
                }
                TextBoxScript.Append("dntb_" + tid + ".document.attachEvent('ondblclick',function(e){dbclickcheck(dntb_" + tid + ")});\r\n");
                TextBoxScript.Append("var EditMode" + tid + "='EDIT',ieSelectionBookmark;\r\n");
                if (focus)
                {
                    TextBoxScript.Append("dntb_" + tid + ".focus();\r\n");
                }
                TextBoxScript.Append("function " + tid + "_CopyEditContent()\r\n");
                TextBoxScript.Append("{\r\n");
                TextBoxScript.Append("if(bMode||EditMode" + tid + "=='VIEW')\r\n");
                TextBoxScript.Append("{\r\n");

                if (PathType != "AbsoluteFull")
                {
                    if (xhtml != "none")
                    {
                        TextBoxScript.Append("document.getElementById('" + tid + "').value = urlchange(getXHtml(dntb_" + tid + "));\r\n");
                    }
                    else
                    {
                        TextBoxScript.Append("document.getElementById('" + tid + "').value = urlchange(dntb_" + tid + ".document.body.innerHTML);\r\n");
                    }
                    TextBoxScript.Append("}\r\n");
                    TextBoxScript.Append("else\r\n");
                    TextBoxScript.Append("{\r\n");
                    TextBoxScript.Append("document.getElementById('" + tid + "').value = urlchange(dntb_" + tid + ".document.body.innerText);\r\n");
                    TextBoxScript.Append("}\r\n");
                }
                else
                {
                    if (xhtml != "none")
                    {
                        TextBoxScript.Append("document.getElementById('" + tid + "').value = getXHtml(dntb_" + tid + ");\r\n");
                    }
                    else
                    {
                        TextBoxScript.Append("document.getElementById('" + tid + "').value = dntb_" + tid + ".document.body.innerHTML;\r\n");
                    }
                    TextBoxScript.Append("}\r\n");
                    TextBoxScript.Append("else\r\n");
                    TextBoxScript.Append("{\r\n");
                    TextBoxScript.Append("document.getElementById('" + tid + "').value = dntb_" + tid + ".document.body.innerText;\r\n");
                    TextBoxScript.Append("}\r\n");
                }
                TextBoxScript.Append("}\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.execCommand('LiveResize',true,true);\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.execCommand('2D-Position',true,true);\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.onbeforedeactivate = function(){saveBookmark(dntb_" + tid + ")}\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.onactivate = function(){gotoBookmark(dntb_" + tid + ")}\r\n");
                TextBoxScript.Append("dntb_" + tid + ".document.onkeyup = function (){executeUndoRedo(dntb_" + tid + ");}\r\n");
                TextBoxScript.Append("saveUndoRedo(dntb_" + tid + ");\r\n");
            }
            return TextBoxScript.ToString();
        }
        #endregion

        /// <summary>
        /// 外挂插件的处理方法
        /// </summary>
        private string getPlugin(string[] parameter, string tid, string skin, string function)
        {

            if (parameter[0].ToLower() == "openwin")
            {
                // 外挂插件(弹出窗口形式)
                //return "<img src=" + skin + parameter[3] + " class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';openDialog_plugin(dntb_" + tid + ",'" + function + parameter[2] + "','" + parameter[4] + "','" + parameter[5] + "');\" title='" + ResourceManager.GetString(parameter[1]) + "' hspace=\"2\" vspace=\"0\">";
                parameter[2] = parameter[2].ToLower().Replace(".aspx", "");
                return "<span id=\"" + tid + "_popup_" + parameter[2] + "\"><img src=" + skin + parameter[3] + " onload=\"menuregister(true, '" + tid + "_popup_" + parameter[2] + "')\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','" + parameter[2] + "','" + parameter[4] + "','" + parameter[5] + "','" + tid + "_popup_" + parameter[2] + "_menu')\"  title='" + ResourceManager.GetString(parameter[1]) + "' hspace=\"2\" vspace=\"0\"></span>";
            }
            else
            {
                // 外挂插件(脚本命令形式)
                return "<img src=" + skin + parameter[3] + " class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';create(dntb_" + tid + ",'" + parameter[2] + "');\"  title='" + ResourceManager.GetString(parameter[1]) + "' unselectable=\"on\" hspace=\"2\" vspace=\"0\">";
            }
        }

        #region  根据调用值返回控件菜单功能的HTML代码
        /// <summary>
        /// 根据调用值返回控件菜单功能的HTML代码
        /// </summary>
        private string getFunction(string Name, string tid, string skin, string function)
        {
            string enwidth, enwidth2, enlw, menustr = "";
            switch (Name.ToLower())
            {
                case "br":
                    menustr = "<br />";
                    break;
                case "blank":
                    menustr = "&nbsp;";
                    break;
                case "pagebreak":
                    menustr = "<img src=\"" + skin + "img/pagebreak.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';inserObject(dntb_" + tid + ",'pagebreak');\" title='" + ResourceManager.GetString("pagebreak") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "resettoolbar":
                    menustr = "<img src=\"" + skin + "img/resettoolbar.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';delallcookie();\" title='" + ResourceManager.GetString("resettoolbar") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "min":
                    menustr = "<img src=\"" + skin + "img/min.gif\" onclick=\"resize(-adjustsize,'" + tid + "')\" style=\"cursor:pointer\" title='" + ResourceManager.GetString("minus") + "'>";
                    break;
                case "plus":
                    menustr = "<img src=\"" + skin + "img/plus.gif\" title='" + ResourceManager.GetString("plus") + "' style=\"cursor:pointer\" onclick=\"resize(adjustsize,'" + tid + "')\">";
                    break;
                case "preview":
                    menustr = "<INPUT class='status_nohighlight' id='viewstatus_" + tid + "' onclick=\"this.className='status_highlight';document.getElementById('codestatus_" + tid + "').className='status_nohighlight';document.getElementById('editstatus_" + tid + "').className='status_nohighlight';setMode(dntb_" + tid + ",document.getElementById('toolbarbox_" + tid + "'),'VIEW',false,'" + tid + "')\"  title=" + ResourceManager.GetString("previewstatus") + " type=button value=" + ResourceManager.GetString("previewbutton") + " style=\"width: 50px\">";
                    break;
                case "source":
                    menustr = "<INPUT class='status_nohighlight' id='codestatus_" + tid + "' onclick=\"this.className='status_highlight';document.getElementById('editstatus_" + tid + "').className='status_nohighlight';document.getElementById('viewstatus_" + tid + "').className='status_nohighlight';setMode(dntb_" + tid + ",document.getElementById('toolbarbox_" + tid + "'),'SOURCE',false,'" + tid + "')\"  title=" + ResourceManager.GetString("codestatus") + " type=button value=" + ResourceManager.GetString("codebutton") + " style=\"width: 50px\">";
                    break;
                case "edit":
                    menustr = "<INPUT class='status_highlight' id='editstatus_" + tid + "' onclick=\"this.className='status_highlight';document.getElementById('codestatus_" + tid + "').className='status_nohighlight';document.getElementById('viewstatus_" + tid + "').className='status_nohighlight';setMode(dntb_" + tid + ",document.getElementById('toolbarbox_" + tid + "'),'EDIT',true,'" + tid + "')\"  title=" + ResourceManager.GetString("editstatus") + " type=button value=" + ResourceManager.GetString("editbutton") + " style=\"width: 50px\">";
                    break;
                case "imgpreview":
                    menustr = "<img src=\"" + skin + "img/view.gif\" id=viewstatus_" + tid + " class=\"outcolor\" onclick=\"this.className='clickcolor';document.getElementById('codestatus_" + tid + "')?document.getElementById('codestatus_" + tid + "').className='outcolor':null;document.getElementById('editstatus_" + tid + "')?document.getElementById('editstatus_" + tid + "').className='outcolor':null;setMode(dntb_" + tid + ",document.getElementById('toolbarbox_" + tid + "'),'VIEW',false,'" + tid + "')\" hspace=\"2\" vspace=\"0\" title=" + ResourceManager.GetString("previewstatus") + ">";
                    break;
                case "imgsource":
                    menustr = "<img src=\"" + skin + "img/source.gif\" id=codestatus_" + tid + " class=\"outcolor\" onclick=\"this.className='clickcolor';document.getElementById('editstatus_" + tid + "')?document.getElementById('editstatus_" + tid + "').className='outcolor':null;document.getElementById('viewstatus_" + tid + "')?document.getElementById('viewstatus_" + tid + "').className='outcolor':null;setMode(dntb_" + tid + ",document.getElementById('toolbarbox_" + tid + "'),'SOURCE',false,'" + tid + "')\" hspace=\"2\" vspace=\"0\" title=" + ResourceManager.GetString("codestatus") + ">";
                    break;
                case "imgedit":
                    menustr = "<img src=\"" + skin + "img/edit.gif\" id=editstatus_" + tid + " class=\"clickcolor\"  onclick=\"this.className='clickcolor';document.getElementById('codestatus_" + tid + "')?document.getElementById('codestatus_" + tid + "').className='outcolor':null;document.getElementById('viewstatus_" + tid + "')?document.getElementById('viewstatus_" + tid + "').className='outcolor':null;setMode(dntb_" + tid + ",document.getElementById('toolbarbox_" + tid + "'),'EDIT',true,'" + tid + "')\" hspace=\"2\" vspace=\"0\" title=" + ResourceManager.GetString("editstatus") + ">";
                    break;
                case "toolbarhide":
                    menustr = "<img title=\"" + ResourceManager.GetString("show") + "\" src=\"" + skin + "img/show.gif\" style=\"cursor:pointer\" onclick='if(this.state==\"hide\"){document.getElementById(\"divbar" + divid + "\").style.display=\"none\";this.src=\"" + skin + "img/show.gif\";this.state=\"show\";this.title=\"" + ResourceManager.GetString("show") + "\"}else{document.getElementById(\"divbar" + divid + "\").style.display=\"inline\";this.src=\"" + skin + "img/hide.gif\";this.state=\"hide\";this.title=\"" + ResourceManager.GetString("hide") + "\"}'/><div id='divbar" + divid + "' style='display:none'>";
                    break;
                case "toolbarhideend":
                    menustr = "</div>";
                    divid = divid + 1;
                    break;
                case "toolbarshow":
                    menustr = "<img title=\"" + ResourceManager.GetString("hide") + "\" src=\"" + skin + "img/hide.gif\" style=\"cursor:pointer\" onclick='if(this.state==\"show\"){document.getElementById(\"divbar" + divid + "\").style.display=\"inline\";this.src=\"" + skin + "img/hide.gif\";this.state=\"hide\";this.title=\"" + ResourceManager.GetString("hide") + "\"}else{document.getElementById(\"divbar" + divid + "\").style.display=\"none\";this.src=\"" + skin + "img/show.gif\";this.state=\"show\";this.title=\"" + ResourceManager.GetString("show") + "\"}'/><div id='divbar" + divid + "' style='display:inline'>";
                    break;
                case "toolbarshowend":
                    menustr = "</div>";
                    divid = divid + 1;
                    break;
                case "dragtoolbar":
                    menustr = "<div id=m" + dragid + " title=\"" + ResourceManager.GetString("draglayer") + "\" class=\"drginline\"><h1 style='cursor:move;margin:0px;padding:0px;' class=\"drginline\"><img src=\"" + function + "img/drag.gif\" unselectable=\"on\"/></h1>";
                    break;
                case "dragtoolbarend":
                    menustr = "</div>";
                    dragid = dragid + 1;
                    break;
                case "codehighlight":
                    menustr = "<img src=\"" + skin + "img/code2.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';openDialog(dntb_" + tid + ",'" + function + "pastecode.aspx','450','375','codehighlight');\" title='" + ResourceManager.GetString("insertcodehighlight") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "sup":
                    menustr = "<img src=\"" + skin + "img/sup.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';inserObject(dntb_" + tid + ",'sup');\" title='" + ResourceManager.GetString("sup") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "sub":
                    menustr = "<img src=\"" + skin + "img/sub.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';inserObject(dntb_" + tid + ",'sub');\" title='" + ResourceManager.GetString("sub") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "removeformat":
                    menustr = "<img src=\"" + skin + "img/removeformat.gif\" class=\"outcolor\" onmouseover=\"this.className='overcolor';\" onmouseout=\"this.className='outcolor';\" onclick=\"this.className='clickcolor';format(dntb_" + tid + ",'removeformat');\"  title='" + ResourceManager.GetString("removeformat") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "formatstripper":
                    menustr = "<span id=\"" + tid + "_popup_formatstripper\"><img src=\"" + skin + "img/formatstripper.gif\" class=\"outcolor\" onmouseover=\"this.className='overcolor';\" onmouseout=\"this.className='outcolor';\" onclick=\"this.className='clickcolor';buildDiv('" + tid + "','formatstripper','130','150','" + tid + "_popup_formatstripper_menu');\"  title='" + ResourceManager.GetString("formatstripper") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_formatstripper')\"></span>";
                    break;
                case "pageoutput":
                    menustr = "<img src=\"" + skin + "img/pageoutput.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';inserObject(dntb_" + tid + ",'pageoutput');\" title='" + ResourceManager.GetString("pageoutput") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "calculator":
                    menustr = "<span id=\"" + tid + "_popup_calculator\"><img src=\"" + skin + "img/calculator.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','calculator','240','210','" + tid + "_popup_calculator_menu');\" title='" + ResourceManager.GetString("calculator") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_calculator')\"></span>";
                    break;
                case "getpage":
                    menustr = "<span id=\"" + tid + "_popup_getpage\"><img src=\"" + skin + "img/getpage.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','collection',350,85,'" + tid + "_popup_getpage_menu')\"  title='" + ResourceManager.GetString("getpage") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_getpage')\"></span>";
                    break;
                case "qq":
                    menustr = "<span id=\"" + tid + "_popup_qq\"><img src=\"" + skin + "img/qq.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','onlinecode',200,90,'" + tid + "_popup_qq_menu')\"  title='" + ResourceManager.GetString("qq") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_qq')\"></span>";
                    break;
                case "icq":
                    menustr = "<span id=\"" + tid + "_popup_icq\"><img src=\"" + skin + "img/icq.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','onlinecode',200,90,'" + tid + "_popup_icq_menu')\"  title='" + ResourceManager.GetString("icq") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_icq')\"></span>";
                    break;
                case "msn":
                    menustr = "<span id=\"" + tid + "_popup_msn\"><img src=\"" + skin + "img/msn.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','onlinecode',200,90,'" + tid + "_popup_msn_menu')\"  title='" + ResourceManager.GetString("msn") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_msn')\"></span>";
                    break;
                case "quote":
                    menustr = "<img src=\"" + skin + "img/quote.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';inserObject(dntb_" + tid + ",'quote');\" title='" + ResourceManager.GetString("quote") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "code":
                    menustr = "<img src=\"" + skin + "img/code.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';inserObject(dntb_" + tid + ",'code');\" title='" + ResourceManager.GetString("code") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "emot":
                    menustr = "<span id=\"" + tid + "_popup_emot\"><img src=\"" + skin + "img/emot.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','" + skin + "',320,200,'" + tid + "_popup_emot_menu')\"  title='" + ResourceManager.GetString("emot") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_emot')\"></span>";
                    break;
                case "excel":
                    menustr = "<span id=\"" + tid + "_popup_excel\"><img src=\"" + skin + "img/excel.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','importexcel',400,120,'" + tid + "_popup_excel_menu');\" title='" + ResourceManager.GetString("excel") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_excel')\"></span>";
                    break;
                case "table":
                    menustr = "<img src=\"" + skin + "img/table.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" unselectable=\"on\" onmouseup=\"this.className='clickcolor';inserTables(dntb_" + tid + ",'315','250');\" title='" + ResourceManager.GetString("table") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "time":
                    menustr = "<img src=\"" + skin + "img/nowtime.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';inserObject(dntb_" + tid + ",'nowtime');\" title='" + ResourceManager.GetString("nowtime") + "'  hspace=\"2\" vspace=\"0\">";
                    break;
                case "date":
                    menustr = "<span id=\"" + tid + "_popup_calendar\"><img src=\"" + skin + "img/date.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','calendar','240','235','" + tid + "_popup_calendar_menu');\" title='" + ResourceManager.GetString("date") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_calendar')\"></span>";
                    break;
                case "bgcolor":
                    menustr = "<span id=\"" + tid + "_popup_bgcolor\"><img src=\"" + skin + "img/bgcolor.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';colordialog('bgcolor','" + tid + "','" + tid + "_popup_bgcolor_menu');\" title='" + ResourceManager.GetString("bgcolor") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_bgcolor')\"></span>";
                    break;
                case "font":
                    menustr = "<span id=\"" + tid + "_popup_forecolor\"><img src=\"" + skin + "img/font.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';colordialog('forecolor','" + tid + "','" + tid + "_popup_forecolor_menu');\" title='" + ResourceManager.GetString("forecolor") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_forecolor')\"></span>";
                    break;
                case "insertfile":
                    menustr = "<img src=\"" + skin + "img/upload.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" unselectable=\"on\" onmouseup=\"this.className='clickcolor';inserFile(dntb_" + tid + ",'uploadFile.aspx','570','494');\" title='" + ResourceManager.GetString("uploadfile") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "inserttemplate":
                    menustr = "<img src=\"" + skin + "img/template.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';insertTemplate(dntb_" + tid + ",'uploadtemplate.aspx','570','471');\" title='" + ResourceManager.GetString("uploadtemplate") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "insertmedia":
                    menustr = "<img src=\"" + skin + "img/media.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';inserMedia(dntb_" + tid + ",'uploadmedia.aspx','570','494');\" title='" + ResourceManager.GetString("uploadmedia") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "insertimage":
                    menustr = "<img src=\"" + skin + "img/insertimage.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" unselectable=\"on\" onmouseup=\"this.className='clickcolor';inserImg(dntb_" + tid + ",'uploadImg.aspx','570','736');\" title='" + ResourceManager.GetString("uploadimg") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "createlink":
                    menustr = "<img src=\"" + skin + "img/createlink.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" unselectable=\"on\" onmouseup=\"this.className='clickcolor';insertlink(dntb_" + tid + ",'link.aspx','400','138');\" title='" + ResourceManager.GetString("createlink") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "inserthorizontalrule":
                    menustr = "<img src=\"" + skin + "img/inserthorizontalrule.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';create(dntb_" + tid + ",'inserthorizontalrule');\"  title='" + ResourceManager.GetString("inserthorizontalrule") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "underline":
                    menustr = "<img src=\"" + skin + "img/underline.gif\" id='underlineico_" + tid + "' class=\"outcolor\" onmouseover=\"buttonstate('overcolor',this);\" onmouseout=\"buttonstate('outcolor',this);\" onclick=\"buttonstate('clickcolor',this);format(dntb_" + tid + ",'underline');\"  title='" + ResourceManager.GetString("underline") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "italic":
                    menustr = "<img src=\"" + skin + "img/italic.gif\" id='italicico_" + tid + "' class=\"outcolor\" onmouseover=\"buttonstate('overcolor',this);\" onMouseOut=\"buttonstate('outcolor',this);\" onclick=\"buttonstate('clickcolor',this);format(dntb_" + tid + ",'italic');\"  title='" + ResourceManager.GetString("italic") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "bold":
                    menustr = "<img src=\"" + skin + "img/bold.gif\" id='boldico_" + tid + "' class=\"outcolor\" onmouseover=\"buttonstate('overcolor',this);\" onMouseOut=\"buttonstate('outcolor',this);\" onclick=\"buttonstate('clickcolor',this);format(dntb_" + tid + ",'bold');\"  title='" + ResourceManager.GetString("bold") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "help":
                    menustr = "<span id=\"" + tid + "_popup_help\"><img src=\"" + skin + "img/help.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','onlineHelp',245,330,'" + tid + "_popup_help_menu')\"  title='" + ResourceManager.GetString("help") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_help')\"></span>";
                    break;
                case "printer":
                    menustr = "<img src=\"" + skin + "img/print.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';format(dntb_" + tid + ",'print');\" title='" + ResourceManager.GetString("print") + "'  hspace=\"2\" vspace=\"0\">";
                    break;
                case "paste":
                    menustr = "<img src=\"" + skin + "img/paste.gif\" class=\"outcolor\" onmouseover=\"this.className='overcolor';\" onmouseout=\"this.className='outcolor';\" onclick=\"this.className='clickcolor';pasteContent(dntb_" + tid + ");\"  title='" + ResourceManager.GetString("paste") + "'  hspace=\"2\" vspace=\"0\">";
                    break;
                case "pastetext":
                    menustr = "<img src=\"" + skin + "img/pastetext.gif\" class=\"outcolor\" onmouseover=\"this.className='overcolor';\" onmouseout=\"this.className='outcolor';\" onclick=\"this.className='clickcolor';pastetext(dntb_" + tid + ");\"  title='" + ResourceManager.GetString("pastetext") + "'  hspace=\"2\" vspace=\"0\">";
                    break;
                case "pasteword":
                    menustr = "<img src=\"" + skin + "img/pasteword.gif\" class=\"outcolor\" onmouseover=\"this.className='overcolor';\" onmouseout=\"this.className='outcolor';\" onclick=\"this.className='clickcolor';pasteword(dntb_" + tid + ");\"  title='" + ResourceManager.GetString("pasteword") + "'  hspace=\"2\" vspace=\"0\">";
                    break;
                case "copy":
                    menustr = "<img src=\"" + skin + "img/copy.gif\" class=\"outcolor\" onmouseover=\"this.className='overcolor';\" onmouseout=\"this.className='outcolor';\" onclick=\"this.className='clickcolor';format(dntb_" + tid + ",'copy');\"  title='" + ResourceManager.GetString("copy") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "cut":
                    menustr = "<img src=\"" + skin + "img/cut.gif\" class=\"outcolor\" onmouseover=\"this.className='overcolor';\" onmouseout=\"this.className='outcolor';\" onclick=\"this.className='clickcolor';format(dntb_" + tid + ",'cut');\"  title='" + ResourceManager.GetString("cut") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "delete":
                    menustr = "<img src=\"" + skin + "img/delete.gif\" class=\"outcolor\" onmouseover=\"this.className='overcolor';\" onmouseout=\"this.className='outcolor';\" onclick=\"this.className='clickcolor';format(dntb_" + tid + ",'delete');\"  title='" + ResourceManager.GetString("delete") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "redo":
                    menustr = "<img src=\"" + skin + "img/redo.gif\" class=\"outcolor\" onmouseover=\"this.className='overcolor';\" onmouseout=\"this.className='outcolor';\" onclick=\"this.className='clickcolor';goUndoRedo(1,dntb_" + tid + ");\" title='" + ResourceManager.GetString("redo") + "'' unselectable=\"on\" hspace=\"2\" vspace=\"0\">";
                    break;
                case "undo":
                    menustr = "<img src=\"" + skin + "img/undo.gif\" class=\"outcolor\" onmouseover=\"this.className='overcolor';\" onmouseout=\"this.className='outcolor';\" onclick=\"this.className='clickcolor';goUndoRedo(-1,dntb_" + tid + ");\" title='" + ResourceManager.GetString("undo") + "' unselectable=\"on\" hspace=\"2\" vspace=\"0\">";
                    break;
                case "search":
                    menustr = "<span id=\"" + tid + "_popup_search\"><img src=\"" + skin + "img/search.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','search',330,180,'" + tid + "_popup_search_menu')\"  title='" + ResourceManager.GetString("search") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_search')\"></span>";
                    break;
                case "indent":
                    menustr = "<img src=\"" + skin + "img/indent.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';format(dntb_" + tid + ",'indent');\"  title='" + ResourceManager.GetString("indent") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "justifyleft":
                    menustr = "<img src=\"" + skin + "img/justifyleft.gif\" id='justifyleftico_" + tid + "' class=\"outcolor\" onMouseOver=\"buttonstate('overcolor',this);\" onMouseOut=\"buttonstate('outcolor',this);\" onClick=\"buttonstate('clickcolor',this);format(dntb_" + tid + ",'justifyleft');checkformat('" + tid + "')\"  title='" + ResourceManager.GetString("justifyleft") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "justifycenter":
                    menustr = "<img src=\"" + skin + "img/justifycenter.gif\" id='justifycenterico_" + tid + "' class=\"outcolor\" onMouseOver=\"buttonstate('overcolor',this);\" onMouseOut=\"buttonstate('outcolor',this);\" onClick=\"buttonstate('clickcolor',this);format(dntb_" + tid + ",'justifycenter');checkformat('" + tid + "')\"  title='" + ResourceManager.GetString("justifycenter") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "justifyright":
                    menustr = "<img src=\"" + skin + "img/justifyright.gif\" id='justifyrightico_" + tid + "' class=\"outcolor\" onMouseOver=\"buttonstate('overcolor',this);\" onMouseOut=\"buttonstate('outcolor',this);\" onClick=\"buttonstate('clickcolor',this);format(dntb_" + tid + ",'justifyright');checkformat('" + tid + "')\"  title='" + ResourceManager.GetString("justifyright") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "insertorderedlist":
                    menustr = "<img src=\"" + skin + "img/insertorderedlist.gif\" id='insertunorderedlistico_" + tid + "' class=\"outcolor\" onMouseOver=\"buttonstate('overcolor',this);\" onMouseOut=\"buttonstate('outcolor',this);\" onClick=\"buttonstate('clickcolor',this);format(dntb_" + tid + ",'insertunorderedlist');\"  title='" + ResourceManager.GetString("insertunorderedlist") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "insertunorderedlist":
                    menustr = "<img src=\"" + skin + "img/insertunorderedlist.gif\" id='insertorderedlistico_" + tid + "' class=\"outcolor\" onMouseOver=\"buttonstate('overcolor',this);\" onMouseOut=\"buttonstate('outcolor',this);\" onClick=\"buttonstate('clickcolor',this);format(dntb_" + tid + ",'insertorderedlist');\"  title='" + ResourceManager.GetString("insertorderedlist") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "outdent":
                    menustr = "<img src=\"" + skin + "img/outdent.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';format(dntb_" + tid + ",'outdent');\"  title='" + ResourceManager.GetString("outdent") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "symbol":
                    menustr = "<span id=\"" + tid + "_popup_specialfont\"><img src=\"" + skin + "img/specialfont.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" onClick=\"this.className='clickcolor';buildDiv('" + tid + "','specialfont',385,210,'" + tid + "_popup_specialfont_menu')\"  title='" + ResourceManager.GetString("symbol") + "' hspace=\"2\" vspace=\"0\" onload=\"menuregister(true, '" + tid + "_popup_specialfont')\"></span>";
                    break;
                case "div":
                    menustr = "<img src=\"" + skin + "img/div.gif\" class=\"outcolor\" onMouseOver=\"this.className='overcolor';\" onMouseOut=\"this.className='outcolor';\" unselectable=\"on\" onmouseup=\"this.className='clickcolor';insertDiv(dntb_" + tid + ",'315','280');\" title='" + ResourceManager.GetString("div") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "fullscreen":
                    menustr = "<img src=\"" + skin + "img/fullscreen.gif\" id='fullscreenico_" + tid + "' class=\"outcolor\" onMouseOver=\"buttonstate('overcolor',this);\" onMouseOut=\"buttonstate('outcolor',this);\"  onClick=\"buttonstate('clickcolor',this);fullscreen('" + tid + "')\" title='" + ResourceManager.GetString("fullscreen") + "' hspace=\"2\" vspace=\"0\">";
                    break;
                case "paragraph":
                    if (ResourceManager.SiteLanguageKey.IndexOf("en") != -1)
                    {
                        enwidth = "130px";
                        enwidth2 = "128";
                        enlw = "107px";
                    }
                    else
                    {

                        enwidth = "108px";
                        enwidth2 = "106";
                        enlw = "85px";
                    }
                    menustr = "<div id=" + tid + "_popup_paragraph  class=\"divinline\" style=\"width:" + enwidth + ";MARGIN-LEFT: 2px;MARGIN-Right: 5px;MARGIN-TOP: 2px\"><div class='editor_buttonnormal' onclick=\"buildDiv('" + tid + "',''," + enwidth2 + ",260,'" + tid + "_popup_paragraph_menu')\" onmouseover=\"menuContext(this, 'mouseover')\" onmouseout=\"menuContext(this, 'mouseout')\"><table onload=\"menuregister(true, '" + tid + "_popup_paragraph')\" id='" + tid + "_paragraphtable' cellspacing='0' cellpadding='0' border='0' unselectable='on'><tbody><tr><td class='editor_menunormal' id='" + tid + "_menu' unselectable='on'><div style='WIDTH: " + enlw + ";OVERFLOW: hidden' align='left' unselectable='on'>" + ResourceManager.GetString("paragraphformat") + "</div></td><td unselectable='on' width='23px' align='center'><img src='" + skin + "img/arrow.gif'/></td></tr></tbody></table></div></div>\r\n";

                    break;
                case "specialtype":
                    if (ResourceManager.SiteLanguageKey.IndexOf("en") != -1)
                    {
                        enwidth = "95px";
                        enwidth2 = "93";
                        enlw = "72px";
                    }
                    else
                    {

                        enwidth = "75px";
                        enwidth2 = "73";
                        enlw = "52px";
                    }
                    menustr = "<div id=" + tid + "_popup_specialtype class=\"divinline\" style=\"width:" + enwidth + ";MARGIN-LEFT: 2px;MARGIN-Right: 5px;MARGIN-TOP: 2px\"><div class='editor_buttonnormal' onclick=\"buildDiv('" + tid + "',''," + enwidth2 + ",180,'" + tid + "_popup_specialtype_menu')\" onmouseover=\"menuContext(this, 'mouseover')\" onmouseout=\"menuContext(this, 'mouseout')\"><table onload=\"menuregister(true, '" + tid + "_popup_specialtype')\" id='" + tid + "_specialtypetable' cellspacing='0' cellpadding='0' border='0' unselectable='on'><tbody><tr><td class='editor_menunormal' id='" + tid + "_menu' unselectable='on'><div  style='WIDTH: " + enlw + ";OVERFLOW: hidden' align='left' unselectable='on'>" + ResourceManager.GetString("specialtype") + "</div></td><td unselectable='on' width='23px' align='center'><img src='" + skin + "img/arrow.gif' /></td></tr></tbody></table></div></div>\r\n";
                    break;
                case "selfont":
                    if (ResourceManager.SiteLanguageKey.IndexOf("en") != -1)
                    {
                        enwidth = "145px";
                        enwidth2 = "143";
                        enlw = "122px";
                    }
                    else
                    {
                        enwidth = "130px";
                        enwidth2 = "128";
                        enlw = "107px";
                    }
                    menustr = "<div id=" + tid + "_popup_fontname class=\"divinline\" style=\"width:" + enwidth + ";MARGIN-LEFT: 2px;MARGIN-Right: 5px;MARGIN-TOP: 2px\"><div class='editor_buttonnormal' onclick=\"buildDiv('" + tid + "',''," + enwidth2 + ",240,'" + tid + "_popup_fontname_menu')\" onmouseover=\"menuContext(this, 'mouseover')\" onmouseout=\"menuContext(this, 'mouseout')\"><table onload=\"menuregister(true, '" + tid + "_popup_fontname')\" id='" + tid + "_fontnametable' cellspacing='0' cellpadding='0' border='0' unselectable='on'><tbody><tr><td class='editor_menunormal' id='" + tid + "_menu' unselectable='on'><div style='WIDTH:" + enlw + ";OVERFLOW: hidden' id='fontnamelist' align='left' unselectable='on'>" + ResourceManager.GetString("selectfont") + "</div></td><td unselectable='on' width='23px' align='center'><img src='" + skin + "img/arrow.gif' /></td></tr></tbody></table></div></div>\r\n";

                    break;
                case "fontsize":
                    if (ResourceManager.SiteLanguageKey.IndexOf("en") != -1)
                    {
                        enwidth = "80px";
                        enwidth2 = "78";
                        enlw = "57px";
                    }
                    else
                    {
                        enwidth = "53px";
                        enwidth2 = "51";
                        enlw = "30px";
                    }
                    menustr = "<div id=" + tid + "_popup_fontsize class=\"divinline\" style=\"width:" + enwidth + ";MARGIN-LEFT: 2px;MARGIN-Right: 5px;MARGIN-TOP: 2px\"><DIV class=editor_buttonnormal onclick=\"buildDiv('" + tid + "',''," + enwidth2 + ",250,'" + tid + "_popup_fontsize_menu')\" onmouseover=\"menuContext(this, 'mouseover')\" onmouseout=\"menuContext(this, 'mouseout')\"><TABLE onload=\"menuregister(true, '" + tid + "_popup_fontsize')\" id='" + tid + "_fontsizetable' cellSpacing=0 cellPadding=0 border=0 unselectable=\"on\"><TBODY><TR><TD class=editor_menunormal id=" + tid + "_menu unselectable=\"on\"><DIV style=\"WIDTH: " + enlw + ";OVERFLOW: hidden\" id='fontsizelist' align='left' unselectable=\"on\">" + ResourceManager.GetString("fontsize") + "</DIV></TD><TD unselectable=\"on\" width='23px' align=\"center\"><img src='" + skin + "img/arrow.gif' /></TD></TR></TBODY></TABLE></DIV></DIV>\r\n";
                    break;
                default:
                    break;
            }
            return menustr;
        }
        #endregion
    }
    #endregion
}