using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;


namespace DotNetTextBox
{
    #region 自定义控件的上传功能
    /// <summary>
    /// 文件列表的排序处理
    /// </summary>
    public class SortFile : IComparer
    {
        int IComparer.Compare(object a, object b)
        {
            FileInfo fa = (FileInfo)a;
            FileInfo fb = (FileInfo)b;

            if (fa.LastWriteTime < fb.LastWriteTime)
                return 1;
            if (fa.LastWriteTime > fb.LastWriteTime)
                return -1;

            return 0;
        }
    }

    /// <summary>
    /// 目录列表的排序处理
    /// </summary>
    public class SortDir : IComparer
    {
        int IComparer.Compare(object a, object b)
        {
            DirectoryInfo fa = (DirectoryInfo)a;
            DirectoryInfo fb = (DirectoryInfo)b;

            if (fa.LastWriteTime < fb.LastWriteTime)
                return 1;
            if (fa.LastWriteTime > fb.LastWriteTime)
                return -1;

            return 0;
        }
    }

    /// <summary>
    /// 上传功能页面的类
    /// </summary>
    public partial class UpLoad : System.Web.UI.Page
    {
        /// <summary>
        /// 页面控件的定义
        /// </summary>
        protected string vrPath;
        protected System.Web.UI.WebControls.HiddenField config_type;
        protected System.Web.UI.WebControls.FileUpload FileUpload1;
        protected System.Web.UI.WebControls.Button uploadBtn;
        protected System.Web.UI.WebControls.Panel batchPanel;
        protected System.Web.UI.WebControls.ImageButton selectAllBtn;
        protected System.Web.UI.WebControls.ImageButton deleteBtn;
        protected System.Web.UI.WebControls.ImageButton newfolderBtn;
        protected System.Web.UI.WebControls.ImageButton returndir;
        protected System.Web.UI.WebControls.ImageButton returndir2;
        protected System.Web.UI.WebControls.ImageButton editBtn;
        protected System.Web.UI.WebControls.ImageButton filelistturn;
        protected System.Web.UI.WebControls.ImageButton imageturn;
        protected System.Web.UI.WebControls.Button canceloading;
        protected System.Web.UI.WebControls.Button addurl;
        protected System.Web.UI.WebControls.Button insertTemplate;
        protected System.Web.UI.WebControls.Label space;
        protected System.Web.UI.WebControls.Label useSpace;
        protected System.Web.UI.WebControls.Label path;
        protected System.Web.UI.WebControls.Label maxuploadfile;
        protected System.Web.UI.WebControls.Label maxupload;
        protected System.Web.UI.WebControls.TextBox file_path;
        protected System.Web.UI.WebControls.TextBox remoteurl;
        protected System.Web.UI.WebControls.TextBox imgx;
        protected System.Web.UI.WebControls.TextBox imgy;
        protected System.Web.UI.WebControls.TextBox textx;
        protected System.Web.UI.WebControls.TextBox texty;
        protected System.Web.UI.WebControls.TextBox textsize;
        protected System.Web.UI.WebControls.TextBox fontcolor;
        protected System.Web.UI.WebControls.TextBox watermarktextinput;
        protected System.Web.UI.WebControls.TextBox watermarkimginput;
        protected System.Web.UI.WebControls.GridView File_List;
        protected System.Web.UI.WebControls.HiddenField config_maxAllUploadSize;
        protected System.Web.UI.WebControls.HiddenField config_smallImages;
        protected System.Web.UI.WebControls.HiddenField config_watermarkImages;
        protected System.Web.UI.WebControls.HiddenField config_watermakOption;
        protected System.Web.UI.WebControls.HiddenField config_watermark;
        protected System.Web.UI.WebControls.HiddenField config_autoname;
        protected System.Web.UI.WebControls.HiddenField config_allowUpload;
        protected System.Web.UI.WebControls.HiddenField config_fileFilters;
        protected System.Web.UI.WebControls.HiddenField config_maxSingleUploadSize;
        protected System.Web.UI.WebControls.HiddenField config_fileListBox;
        protected System.Web.UI.WebControls.HiddenField config_watermarkImagesName;
        protected System.Web.UI.WebControls.HiddenField config_watermarkName;
        protected System.Web.UI.WebControls.HiddenField config_smallImagesName;
        protected System.Web.UI.WebControls.HiddenField config_smallImagesType;
        protected System.Web.UI.WebControls.HiddenField config_smallImagesW;
        protected System.Web.UI.WebControls.HiddenField config_smallImagesH;
        protected System.Web.UI.WebControls.HiddenField templateContent;
        protected System.Web.UI.WebControls.Label maxSingleUploadSize;
        protected System.Web.UI.WebControls.RadioButtonList RadioButtonList1;
        protected System.Web.UI.WebControls.Button settingimg;
        protected System.Web.UI.WebControls.Button settingwatermark;
        protected System.Web.UI.WebControls.MultiView showsetupface;
        protected System.Web.UI.WebControls.DropDownList fonttype;
        protected System.Web.UI.HtmlControls.HtmlSelect uploadlist;
        protected System.Web.UI.WebControls.Panel filelistdiv;
        protected System.Web.UI.WebControls.Panel fieldsetdiv;
        protected System.Web.UI.WebControls.Panel imagesdiv;
        protected System.Web.UI.WebControls.Panel imageAttribute;
        protected XmlDocument config = new XmlDocument();
        protected string arrangement;
        protected string strFiles = "";
        protected List<int> list = new List<int>();

        private bool CheckFile(string FileName)
        {
            if (strFiles.IndexOf("," + FileName + ",") == -1)
                return false;
            strFiles = strFiles.Replace(FileName + ",", "");
            return true;
        }

        /// <summary>
        /// 页面的初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                if (Page.Request.Form["hidvalue"] == null)
                    strFiles = "";
                else
                    strFiles = "," + Page.Request.Form["hidvalue"].Trim(',') + ",";
                HttpFileCollection files1 = HttpContext.Current.Request.Files;
                for (int i = 0; i < files1.Count; i++)
                {
                    if (CheckFile(files1[i].FileName))
                        list.Add(i);
                }
            }
            else
            {
                Response.Expires = -1;
                if (Request.Cookies["uploadConfig"] != null)
                {
                    config.Load(Server.UrlDecode(Request.Cookies["uploadConfig"].Value.ToLower()));
                }
                else
                {
                    config.Load(HttpContext.Current.Request.PhysicalApplicationPath + "/system_dntb/uploadconfig/default.config");
                }
                if (Request.Cookies["languages"] != null)
                {
                    ResourceManager.SiteLanguageKey = Request.Cookies["languages"].Value;
                }
                else
                {
                    ResourceManager.SiteLanguageKey = HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"].ToLower().Split(',')[0];
                }
                if (config_type.Value == "Images")
                {
                    config_fileFilters.Value = config.SelectNodes("//configuration/imagesFilters")[0].InnerText;
                    config_watermarkName.Value = config.SelectNodes("//configuration/watermarkName")[0].InnerText;
                    config_watermarkImagesName.Value = config.SelectNodes("//configuration/watermarkImagesName")[0].InnerText;
                    config_smallImagesName.Value = config.SelectNodes("//configuration/smallImagesName")[0].InnerText;
                    config_smallImagesType.Value = config.SelectNodes("//configuration/smallImagesType")[0].InnerText;
                    config_smallImagesW.Value = config.SelectNodes("//configuration/smallImagesW")[0].InnerText;
                    config_smallImagesH.Value = config.SelectNodes("//configuration/smallImagesH")[0].InnerText;
                    config_smallImages.Value = config.SelectNodes("//configuration/smallImages")[0].InnerText.Trim().ToLower();
                    config_watermark.Value = config.SelectNodes("//configuration/watermark")[0].InnerText.Trim().ToLower();
                    config_watermarkImages.Value = config.SelectNodes("//configuration/watermarkImages")[0].InnerText.Trim().ToLower();
                    
                    if (config.SelectNodes("//configuration/watermarkOption")[0].InnerText.Trim().ToLower() == "off")
                    {
                        settingwatermark.Enabled = false;
                    }

                    if (config.SelectNodes("//configuration/imageAttribute")[0].InnerText.Trim().ToLower() != "true")
                    {
                        imageAttribute.Visible = false;
                    }
                    else
                    {
                        if (config.SelectNodes("//configuration/imageAttributeView")[0].InnerText.Trim().ToLower() != "show")
                        {
                            imagesdiv.Visible = false;
                            imageturn.ToolTip = ResourceManager.GetString("showimagesdiv");
                            imageturn.ImageUrl = "img/show.gif";
                            imageturn.CommandName = "show";
                        }
                        else
                        {
                            imageturn.ToolTip = ResourceManager.GetString("hideimagesdiv");
                        }
                    }

                    watermarktextinput.Text = config.SelectNodes("//configuration/watermarkText")[0].InnerText.Trim();
                    watermarkimginput.Text = config.SelectNodes("//configuration/watermarkImages_path")[0].InnerText.Trim();
                    settingimg.Text = ResourceManager.GetString("imgsetting");
                    settingwatermark.Text = ResourceManager.GetString("watermarksetting");
                    fonttype.Items.AddRange(new ListItem[] { new ListItem(ResourceManager.GetString("removefont"), "system"), new ListItem(ResourceManager.GetString("simsun"), "simsun"), new ListItem(ResourceManager.GetString("simhei"), "simhei"), new ListItem(ResourceManager.GetString("simyou"), "simyou"), new ListItem(ResourceManager.GetString("simkai"), "simkai"), new ListItem("Arial", "Arial"), new ListItem("Verdana", "Verdana"), new ListItem("Century", "Century") });
                    
                }
                else if(config_type.Value == "File")
                {
                    config_fileFilters.Value = config.SelectNodes("//configuration/fileFilters")[0].InnerText;
                }
                else if (config_type.Value == "Media")
                {
                    config_fileFilters.Value = config.SelectNodes("//configuration/mediaFilters")[0].InnerText;
                }
                else if (config_type.Value == "Template")
                {
                    config_fileFilters.Value = config.SelectNodes("//configuration/templateFilters")[0].InnerText;
                    insertTemplate.Text = ResourceManager.GetString("inserttemplate");
                }
                if (Request.Cookies["UploadFolderSize"] != null)
                {
                    config_maxAllUploadSize.Value = Request.Cookies["UploadFolderSize"].Value;
                }
                else
                {
                    config_maxAllUploadSize.Value = config.SelectNodes("//configuration/maxAllUploadSize")[0].InnerText.Trim();
                }

                addurl.Text = ResourceManager.GetString("uploadadd");
                addurl.OnClientClick = "loading('" + ResourceManager.GetString("loading") + "')";
                config_autoname.Value = config.SelectNodes("//configuration/autoname")[0].InnerText.Trim().ToLower();
                config_allowUpload.Value = config.SelectNodes("//configuration/allowUpload")[0].InnerText.Trim().ToLower();
                config_maxSingleUploadSize.Value = config.SelectNodes("//configuration/maxSingleUploadSize")[0].InnerText.Trim();
                config_fileListBox.Value=config.SelectNodes("//configuration/fileListBox")[0].InnerText;
                maxupload.Text = config.SelectNodes("//configuration/maxUpload")[0].InnerText;
                selectAllBtn.ToolTip = ResourceManager.GetString("selectall");
                editBtn.ToolTip = ResourceManager.GetString("rename");
                deleteBtn.ToolTip = ResourceManager.GetString("delete");
                deleteBtn.OnClientClick = "loading('" + ResourceManager.GetString("deleteing") + "')";
                newfolderBtn.ToolTip = ResourceManager.GetString("builddir");
                newfolderBtn.OnClientClick = "loading('" + ResourceManager.GetString("building") + "')";
                returndir.ToolTip = ResourceManager.GetString("returndir");
                returndir.OnClientClick = "loading('" + ResourceManager.GetString("returning") + "')";
                returndir2.ToolTip = ResourceManager.GetString("returndir");
                returndir2.OnClientClick = "loading('" + ResourceManager.GetString("returning") + "')";
                canceloading.Text = ResourceManager.GetString("canceloading");
                uploadBtn.Text = ResourceManager.GetString("upload");
                uploadBtn.OnClientClick = "loading('" + ResourceManager.GetString("loading") + "')";
                maxuploadfile.Text = ResourceManager.GetString("maxuploadfile");
                RadioButtonList1.Items[0].Text = ResourceManager.GetString("localupload");
                RadioButtonList1.Items[1].Text = ResourceManager.GetString("remoteupload");
                filelistturn.ToolTip = ResourceManager.GetString("hidefilelist");
                File_List.Columns[1].HeaderText = ResourceManager.GetString("filename");
                File_List.Columns[2].HeaderText = ResourceManager.GetString("creationtime");
                File_List.Columns[3].HeaderText = ResourceManager.GetString("filesize");


                if (config.SelectNodes("//configuration/delete")[0].InnerText == "false")
                {
                    deleteBtn.Style.Value = "filter: gray() alpha(opacity=30);opacity:0.3";
                    deleteBtn.Enabled = false;
                }

                if (config.SelectNodes("//configuration/edit")[0].InnerText == "false")
                {
                    editBtn.Style.Value = "filter: gray() alpha(opacity=30);opacity:0.3";
                    editBtn.Enabled = false;
                }

                if (config.SelectNodes("//configuration/folder")[0].InnerText == "false")
                {
                    newfolderBtn.Style.Value = "filter: gray() alpha(opacity=30);opacity:0.3";
                    newfolderBtn.Enabled = false;
                }

                if (config.SelectNodes("//configuration/fileListView")[0].InnerText.ToLower() != "show")
                {
                    filelistdiv.Visible = false;
                    filelistturn.CommandName = "show";
                    filelistturn.ToolTip = ResourceManager.GetString("showfilelist");
                    filelistturn.ImageUrl = "img/show.gif";
                }

                if (config_fileListBox.Value == "false")
                {
                    fieldsetdiv.Visible = false;
                }

                maxSingleUploadSize.Text = Double.Parse(config_maxSingleUploadSize.Value) < 1024 ? config_maxSingleUploadSize.Value + "KB" : (Double.Parse(config_maxSingleUploadSize.Value) / 1024).ToString("f2") + "MB";
                
                if (Request.Cookies["uploadFolder"] != null)
                {

                    if (Request.Cookies["uploadChildFolder"] == null)
                    {
                        path.Text = Server.UrlDecode(Request.Cookies["uploadFolder"].Value.ToLower());
                        returndir.Style.Value = "filter: gray() alpha(opacity=30);opacity:0.3";
                    }
                    else
                    {
                        path.Text = Server.UrlDecode(Request.Cookies["uploadChildFolder"].Value.ToLower());
                        if (Server.UrlDecode(Request.Cookies["uploadFolder"].Value.ToLower()) == path.Text)
                        {
                            returndir.Style.Value = "filter: gray() alpha(opacity=30);opacity:0.3";
                        }
                        else
                        {
                            returndir.Style.Value = "";
                        }
                    }
                }

                if (path.Text=="" || config_allowUpload.Value != "true")
                {
                    FileUpload1.Enabled = false;
                    uploadBtn.Enabled = false;
                    try
                    {
                        addurl.Enabled = false;
                        remoteurl.Enabled = false;
                    }
                    catch
                    {
                    }
                    space.Text = "0MB";
                    useSpace.Text = "0MB";
                    path.Text = "";
                    File_List.Visible = false;
                    deleteBtn.Enabled = false;
                    editBtn.Enabled = false;
                    selectAllBtn.Enabled = false;
                }
                else
                {
                    vrPath = Server.MapPath(path.Text);
                    if (!Directory.Exists(vrPath))
                        Directory.CreateDirectory(vrPath);
                    getDirSize(vrPath);
                }
            }
        }

        //处理水印控制点击
        public void settingwatermark_Click(object sender,EventArgs e)
        {
            showsetupface.ActiveViewIndex = Int32.Parse(settingwatermark.CommandArgument);
            settingwatermark.Enabled = false;
            settingimg.Enabled = true;
        }

        //处理图片属性点击
        public void settingimg_Click(object sender, EventArgs e)
        {
            showsetupface.ActiveViewIndex = Int32.Parse(settingimg.CommandArgument);
            settingwatermark.Enabled = true;
            settingimg.Enabled = false;
        }

        /// <summary>
        /// 处理对当前目录的Cookie记录
        /// </summary>
        public void SetServerCookie(object sender, CommandEventArgs e)
        {
            Response.Cookies["uploadChildFolder"].Value = Server.UrlEncode(path.Text + e.CommandArgument.ToString() + "/");
            path.Text = path.Text + e.CommandArgument.ToString() + "/";
            getDirSize(Server.MapPath(path.Text));
            if (Server.UrlDecode(Request.Cookies["uploadFolder"].Value.ToLower()) == path.Text)
            {
                returndir.Style.Value = "filter: gray() alpha(opacity=30);opacity:0.3";
            }
            else
            {
                returndir.Style.Value = "";
            }
        }

        /// <summary>
        /// 返回目录里文件的空间使用量(MB)
        /// </summary>
        protected void getDirSize(string vrPath)
        {
            Double Size = 0;
            if (config_fileListBox.Value != "false")
            {
                DirectoryInfo d = new DirectoryInfo(vrPath);
                FileInfo[] fis = d.GetFiles();
                DirectoryInfo[] diris=d.GetDirectories();
                ArrayList showfile = new ArrayList();
                Array.Sort(fis,new SortFile());
                Array.Sort(diris, new SortDir());
                string[] Filters = config_fileFilters.Value.Split(',');

                foreach (DirectoryInfo dir in diris)
                {
                    showfile.Add(dir);
                }

                foreach (FileInfo fi in fis)
                {
                    for (int i = 0; i <= Filters.Length - 1; i++)
                    {
                        if (fi.Extension.ToLower() == "." + Filters[i].ToString().ToLower())
                        {
                            showfile.Add(fi);
                            break;
                        }
                    }
                    
                }
                File_List.DataSource = showfile;
                File_List.DataBind();
                showfile.Clear();
            }
            Size = GetDirectoryLength(Server.MapPath(Server.UrlDecode(Request.Cookies["uploadFolder"].Value.ToLower())));
            Size = Convert.ToDouble((Double)Size / 1024);
            useSpace.Text = Size < 1024 ? Size.ToString("f1") + "KB" : (Size / 1024).ToString("f2") + "MB";
            space.Text = (Double.Parse(config_maxAllUploadSize.Value) - Size) < 1024 ? (Double.Parse(config_maxAllUploadSize.Value) - Size).ToString("f1") + "KB" : ((Double.Parse(config_maxAllUploadSize.Value) - Size) / 1024).ToString("f2") + "MB";
            uploadBtn.CommandArgument = Convert.ToString(Size);
            if (Size >= Double.Parse(config_maxAllUploadSize.Value))
            {
                FileUpload1.Enabled = false;
                uploadBtn.Enabled = false;
            }
        }

        /// <summary>
        /// 返回上传空间占用大小
        /// </summary>
        public Double GetDirectoryLength(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                return 0;
            Double len = 0;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    try
                    {
                        len += GetDirectoryLength(dis[i].FullName);
                    }
                    catch
                    {
                    }
                }
            }
            return len;
        }

        /// <summary>
        /// 返回上层目录按键操作
        /// </summary>
        protected void returndir_Click(object sender, EventArgs e)
        {
            if (Request.Cookies["uploadChildFolder"] != null)
            {
                if (Server.UrlDecode(Request.Cookies["uploadFolder"].Value.ToLower()) != path.Text)
                {
                    getDirSize(Server.MapPath(path.Text + "../"));


                    if (path.Text.IndexOf("../") == -1)
                    {
                        arrangement = ".";
                    }
                    else
                    {
                        string subString = path.Text;
                        while (subString.IndexOf("../") >= 0)
                        {
                            subString = subString.Substring(subString.IndexOf("../") + 3);
                            arrangement += "../";
                        }
                    }

                    if (arrangement == ".")
                    {
                        path.Text = Server.MapPath(path.Text + "../").ToLower().Replace(Server.MapPath(arrangement).ToLower() + @"\", string.Empty).Replace(@"\", @"/");
                    }
                    else
                    {
                        path.Text = arrangement + Server.MapPath(path.Text + "../").ToLower().Replace(Server.MapPath(arrangement).ToLower(), string.Empty).Replace(@"\", @"/");
                    }
                    Response.Cookies["uploadChildFolder"].Value = Server.UrlEncode(path.Text);
                    if (Server.UrlDecode(Request.Cookies["uploadFolder"].Value.ToLower()) == path.Text)
                    {
                        returndir.Style.Value = "filter: gray() alpha(opacity=30);opacity:0.3";
                    }
                }
            }
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        protected void newfolderBtn_Click(object sender, EventArgs e)
        {
            string newfolder = "newFolder" + DateTime.Now.ToString("yy-MM-dd-hhmmss");
            Directory.CreateDirectory(Server.MapPath(path.Text + newfolder));
            File_List.EditIndex = 0;
            getDirSize(Server.MapPath(path.Text));
            (File_List.Rows[0].FindControl("editBtn") as Button).Text = ResourceManager.GetString("build");
            (File_List.Rows[0].FindControl("editName") as TextBox).Focus();
        }

        /// <summary>
        /// 处理切换本地及远程上传的操作
        /// </summary>
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "remote")
            {
                uploadlist.Items.Clear();
                addurl.Visible = true;
                remoteurl.Visible = true;
                FileUpload1.Visible = false;
            }
            else
            {
                addurl.Visible = false;
                remoteurl.Visible = false;
                FileUpload1.Visible = true;
            }
        }


        //添加远程文件到列表的操作
        protected void addurl_Click(object sender, EventArgs e)
        {

            if (remoteurl.Text != "")
            {
                uploadlist.Items.Add(remoteurl.Text);
                uploadlist.SelectedIndex = uploadlist.Items.Count - 1;
                remoteurl.Text = "";
            }
            else
            {
                ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("addfile") + "')", true);
            }
        }

        /// <summary>
        /// 处理上传的操作
        /// </summary>
        protected void UploadBtn_Click(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "local")
            {
                if (list.ToArray()[0] > Int32.Parse(maxupload.Text))
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("overuploadfile") + "')", true);
                    return;
                }

                if (list.ToArray()[0] == 0)
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("addfile") + "')", true);
                    return;
                }

                //处理本地上传
                HttpFileCollection files = HttpContext.Current.Request.Files;
                
                for (int i = 0; i < list.ToArray()[0]; i++)
                {
                    string uploadPath = path.Text;
                    string fileName;
                    string typeName = System.IO.Path.GetExtension(files[i].FileName).ToLower();
                    string[] Filters = config_fileFilters.Value.Split(',');
                    bool errortype = true;
                    for (int k = 0; k <= Filters.Length - 1; k++)
                    {
                        if (typeName == "." + Filters[k].ToString().ToLower())
                        {
                            errortype = false;
                            break;
                        }
                    }
                    if (!errortype)
                    {
                        if (files[i].ContentLength < Double.Parse(config_maxSingleUploadSize.Value) * 1024 && Double.Parse(uploadBtn.CommandArgument) * 1024 + FileUpload1.PostedFile.ContentLength < Double.Parse(config_maxAllUploadSize.Value) * 1024)
                        {
                            if (config_autoname.Value == "true")
                            {
                                string y = DateTime.Now.Year.ToString();
                                string m = DateTime.Now.Month.ToString();
                                string d = DateTime.Now.Day.ToString();
                                string h = DateTime.Now.Hour.ToString();
                                string n = DateTime.Now.Minute.ToString();
                                string s = DateTime.Now.Second.ToString();
                                fileName = y + m + d + h + n + s;
                                Random r = new Random();
                                fileName = fileName + r.Next(1000);
                                fileName = fileName + typeName;
                            }
                            else
                            {
                                fileName = System.IO.Path.GetFileName(files[i].FileName);
                            }

                            string webFilePath = Server.MapPath(uploadPath) + fileName;
                            if (!File.Exists(webFilePath))
                            {
                                try
                                {
                                    files[i].SaveAs(webFilePath);
                                    if (config_watermark.Value == "true")
                                    {
                                        if (config_watermarkName.Value == "false")
                                        {
                                            File.Move(webFilePath, Server.MapPath(uploadPath + "temp_" + fileName));
                                            watermarkText(Server.MapPath(uploadPath + "temp_" + fileName), webFilePath, typeName);
                                            File.Delete(Server.MapPath(uploadPath + "temp_" + fileName));
                                        }
                                        else
                                        {
                                            watermarkText(webFilePath, Server.MapPath(uploadPath + config_watermarkName.Value + fileName), typeName);
                                        }
                                    }
                                    if (config_watermarkImages.Value == "true")
                                    {
                                        if (config_watermarkImagesName.Value == "false")
                                        {
                                            File.Move(webFilePath, Server.MapPath(uploadPath + "temp_" + fileName));
                                            watermarkImages(Server.MapPath(uploadPath + "temp_" + fileName), webFilePath, Server.MapPath(watermarkimginput.Text), typeName);
                                            File.Delete(Server.MapPath(uploadPath + "temp_" + fileName));
                                        }
                                        else
                                        {
                                            watermarkImages(webFilePath, Server.MapPath(uploadPath + config_watermarkImagesName.Value + fileName), Server.MapPath(watermarkimginput.Text), typeName);
                                        }

                                    }

                                    if (config_smallImages.Value == "true")
                                    {
                                        if (config_smallImagesName.Value == "false")
                                        {
                                            File.Move(webFilePath, Server.MapPath(uploadPath + "temp_" + fileName));
                                            MakeThumbnail(Server.MapPath(uploadPath + "temp_" + fileName), webFilePath, Int32.Parse(config_smallImagesW.Value), Int32.Parse(config_smallImagesH.Value), config_smallImagesType.Value);
                                            File.Delete(Server.MapPath(uploadPath + "temp_" + fileName));
                                        }
                                        else
                                        {
                                            MakeThumbnail(webFilePath, Server.MapPath(uploadPath + config_smallImagesName.Value + fileName), Int32.Parse(config_smallImagesW.Value), Int32.Parse(config_smallImagesH.Value), config_smallImagesType.Value);
                                        }
                                    }

                                    if (config_fileListBox.Value != "false")
                                    {
                                        getDirSize(Server.MapPath(uploadPath));
                                    }
                                    file_path.Text = uploadPath + fileName;
                                    if (config_type.Value == "Images")
                                    {
                                        showsetupface.ActiveViewIndex = 0;
                                        settingwatermark.Enabled = true;
                                        settingimg.Enabled = false;
                                    }
                                    ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("uploadsuccessful") + "')", true);
                                }
                                catch (Exception ex)
                                {
                                    ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("uploadfail") + ex.Message + "')", true);
                                }
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("exist") + "')", true);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("overmaxsize") + "')", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("errortype") + "')", true);
                    }
                }

            }
            else
            {
                if (uploadlist.Items.Count > Int32.Parse(maxupload.Text))
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("overuploadfile") + "')", true);
                    return;
                }

                if (uploadlist.Items.Count == 0)
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("addfile") + "')", true);
                    return;
                }

                //处理远程上传
                for (int k = 0; k <= uploadlist.Items.Count - 1; k++)
                {
                    if (uploadlist.Items[k].Text != "")
                    {

                        string[] Filters = config_fileFilters.Value.Split(',');

                        string[] afilename = uploadlist.Items[k].Text.ToLower().Split('/');

                        bool errortype = true;

                        string[] bfilename = afilename[afilename.Length - 1].Split('.');
                        string typeName = bfilename[bfilename.Length - 1].ToLower();

                        for (int i = 0; i <= Filters.Length - 1; i++)
                        {
                            if (typeName == Filters[i].ToString().ToLower())
                            {
                                errortype = false;
                                break;
                            }
                        }


                        if (!errortype)
                        {
                            HttpWebRequest hwq = (HttpWebRequest)WebRequest.Create(uploadlist.Items[k].Text);
                            HttpWebResponse hwr = (HttpWebResponse)hwq.GetResponse();
                            
                            if (hwr.ContentLength < Double.Parse(config_maxSingleUploadSize.Value) * 1024 && Double.Parse(uploadBtn.CommandArgument) * 1024 + hwr.ContentLength < Double.Parse(config_maxAllUploadSize.Value) * 1024)
                            {
                                string uploadPath = path.Text;
                                string fileName;
                                if (config_autoname.Value == "true")
                                {
                                    string y = DateTime.Now.Year.ToString();
                                    string m = DateTime.Now.Month.ToString();
                                    string d = DateTime.Now.Day.ToString();
                                    string h = DateTime.Now.Hour.ToString();
                                    string n = DateTime.Now.Minute.ToString();
                                    string s = DateTime.Now.Second.ToString();
                                    fileName = y + m + d + h + n + s;
                                    Random r = new Random();
                                    fileName = fileName + r.Next(1000);
                                    fileName = fileName + "." + typeName;
                                }
                                else
                                {
                                    fileName = afilename[afilename.Length - 1];
                                }

                                string webFilePath = Server.MapPath(uploadPath) + fileName;

                                if (!File.Exists(webFilePath))
                                {
                                    try
                                    {
                                        System.Drawing.Image bmp = System.Drawing.Image.FromStream(hwr.GetResponseStream());
                                        bmp.Save(webFilePath);

                                        if (config_watermark.Value == "true")
                                        {
                                            if (config_watermarkName.Value == "false")
                                            {
                                                File.Move(webFilePath, Server.MapPath(uploadPath + "temp_" + fileName));
                                                watermarkText(Server.MapPath(uploadPath + "temp_" + fileName), webFilePath, typeName);
                                                File.Delete(Server.MapPath(uploadPath + "temp_" + fileName));
                                            }
                                            else
                                            {
                                                watermarkText(webFilePath, Server.MapPath(uploadPath + config_watermarkName.Value + fileName), typeName);
                                            }
                                        }
                                        if (config_watermarkImages.Value == "true")
                                        {
                                            if (config_watermarkImagesName.Value == "false")
                                            {
                                                File.Move(webFilePath, Server.MapPath(uploadPath + "temp_" + fileName));
                                                watermarkImages(Server.MapPath(uploadPath + "temp_" + fileName), webFilePath, Server.MapPath(watermarkimginput.Text), typeName);
                                                File.Delete(Server.MapPath(uploadPath + "temp_" + fileName));
                                            }
                                            else
                                            {
                                                watermarkImages(webFilePath, Server.MapPath(uploadPath + config_watermarkImagesName.Value + fileName), Server.MapPath(watermarkimginput.Text), typeName);
                                            }

                                        }

                                        if (config_smallImages.Value == "true")
                                        {
                                            if (config_smallImagesName.Value == "false")
                                            {
                                                File.Move(webFilePath, Server.MapPath(uploadPath + "temp_" + fileName));
                                                MakeThumbnail(Server.MapPath(uploadPath + "temp_" + fileName), webFilePath, Int32.Parse(config_smallImagesW.Value), Int32.Parse(config_smallImagesH.Value), config_smallImagesType.Value);
                                                File.Delete(Server.MapPath(uploadPath + "temp_" + fileName));
                                            }
                                            else
                                            {
                                                MakeThumbnail(webFilePath, Server.MapPath(uploadPath + config_smallImagesName.Value + fileName), Int32.Parse(config_smallImagesW.Value), Int32.Parse(config_smallImagesH.Value), config_smallImagesType.Value);
                                            }
                                        }

                                        if (config_fileListBox.Value != "false")
                                        {
                                            getDirSize(Server.MapPath(uploadPath));
                                        }
                                        file_path.Text = uploadPath + fileName;
                                        if (config_type.Value == "Images")
                                        {
                                            showsetupface.ActiveViewIndex = 0;
                                            settingwatermark.Enabled = true;
                                            settingimg.Enabled = false;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("uploadfail") + ex.Message + "')", true);
                                    }
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("exist") + "')", true);
                                }
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("overmaxsize") + "')", true);
                                break;
                            }
                            hwr.Close();
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("errortype") + "')", true);
                        }

                    }
                }
                uploadlist.Items.Clear();
                ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("uploadsuccessful") + "')", true);
            }
        }

        /// <summary>
        /// 处理缩略图操作
        /// </summary>
        public void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 处理文字水印的操作
        /// </summary>
        protected void watermarkText(string Path,string newPath,string type)
        { 
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            if (type.ToLower().Replace(".",string.Empty)!= "gif")
            {
               
                try
                {
                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
                    g.DrawImage(image, 0, 0, image.Width, image.Height);
                    System.Drawing.Font f = new System.Drawing.Font(fonttype.SelectedValue, Int32.Parse(textsize.Text));
                    System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.ColorTranslator.FromHtml(fontcolor.Text));

                    int x, y;
                    x = Int32.Parse(textx.Text);
                    y = Int32.Parse(texty.Text);
                    g.DrawString(watermarktextinput.Text.Trim(), f, b, x, y);
                    g.Dispose();
                    image.Save(newPath);
                    image.Dispose();
                }
                catch
                {
                    image.Save(newPath);
                    image.Dispose();
                }
            }
            else
            {
                WaterMark gifmark = new WaterMark();
                gifmark.WaterMarkType = MarkType.Text;
                gifmark.SourceImage = image;
                gifmark.Text=watermarktextinput.Text.Trim();
                gifmark.TextFontFamilyStr=fonttype.SelectedValue;
                gifmark.TextColor=System.Drawing.ColorTranslator.FromHtml(fontcolor.Text);
                gifmark.MarkX=Int32.Parse(textx.Text);
                gifmark.MarkY=Int32.Parse(texty.Text);
                gifmark.TextFontSize = Int32.Parse(textsize.Text);
                gifmark.Mark();
                gifmark.MarkedImage.Save(newPath);
                image.Dispose();
            }
            
        }

        /// <summary>
        /// 处理图片水印的操作
        /// </summary>
        protected void watermarkImages(string Path,string newPath,string images_path,string type)
        {

            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            
            if (type.ToLower().Replace(".", string.Empty) != "gif")
            {
                try
                {
                    System.Drawing.Image copyImage = System.Drawing.Image.FromFile(images_path);
                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
                    int x, y;

                    x = Int32.Parse(imgx.Text);
                    y = Int32.Parse(imgy.Text);

                    g.DrawImage(copyImage, new System.Drawing.Rectangle(x, y, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
                    g.Dispose();

                    image.Save(newPath);
                    image.Dispose();
                }
                catch
                {
                    image.Save(newPath);
                    image.Dispose();
                }
            }
            else
            {
                WaterMark gifmark = new WaterMark();
                gifmark.WaterMarkType = MarkType.Image;
                gifmark.SourceImage = image;
                gifmark.WaterImagePath = images_path;
                gifmark.MarkX = Int32.Parse(textx.Text);
                gifmark.MarkY = Int32.Parse(texty.Text);
                gifmark.Mark();
                gifmark.MarkedImage.Save(newPath);
                image.Dispose();
            }
        }

        /// <summary>
        /// 处理全选的操作
        /// </summary>
        protected void checkAll(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Text == ResourceManager.GetString("selectall"))
            {
                foreach (GridViewRow gvr in File_List.Rows)
                {
                    CheckBox cb1 = (CheckBox)gvr.FindControl("check");
                    cb1.Checked = cb.Checked;
                }
                if (cb.Checked)
                {
                    selectAllBtn.ToolTip = ResourceManager.GetString("unselect");
                    
                }
                else
                {
                    selectAllBtn.ToolTip = ResourceManager.GetString("selectall");
                }
            }

        }

        /// <summary>
        /// 处理收缩展开文件列表的操作
        /// </summary>
        protected void filelistturn_Click(object sender, EventArgs e)
        {
            if (filelistturn.CommandName == "hide")
            {
                filelistdiv.Visible = false;
                filelistturn.ImageUrl = "img/show.gif";
                filelistturn.ToolTip = ResourceManager.GetString("showfilelist");
                filelistturn.CommandName = "show";
            }
            else
            {
                filelistdiv.Visible = true;
                filelistturn.ImageUrl = "img/hide.gif";
                filelistturn.ToolTip = ResourceManager.GetString("hidefilelist");
                filelistturn.CommandName = "hide";
            }
        }

        /// <summary>
        /// 处理收缩展开图像属性的操作
        /// </summary>
        protected void imageturn_Click(object sender, EventArgs e)
        {
            if (imageturn.CommandName == "hide")
            {
                imagesdiv.Visible = false;
                imageturn.ImageUrl = "img/show.gif";
                imageturn.ToolTip = ResourceManager.GetString("showimagesdiv");
                imageturn.CommandName = "show";
            }
            else
            {
                imagesdiv.Visible = true;
                imageturn.ImageUrl = "img/hide.gif";
                imageturn.ToolTip = ResourceManager.GetString("hideimagesdiv");
                imageturn.CommandName = "hide";
            }
        }

        /// <summary>
        /// 处理全选按键的操作
        /// </summary>
        protected void selectAllBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectAllBtn.ToolTip == ResourceManager.GetString("selectall"))
                {
                    CheckBox cb = (CheckBox)File_List.HeaderRow.FindControl("checkall");
                    cb.Checked = true;
                    foreach (GridViewRow gvr in File_List.Rows)
                    {
                        CheckBox cb1 = (CheckBox)gvr.FindControl("check");
                        cb1.Checked = true;
                    }
                    selectAllBtn.ToolTip = ResourceManager.GetString("unselect");
                    selectAllBtn.ImageUrl = "img/unselect.gif";
                }
                else
                {
                    CheckBox cb = (CheckBox)File_List.HeaderRow.FindControl("checkall");
                    cb.Checked = false;
                    foreach (GridViewRow gvr in File_List.Rows)
                    {
                        CheckBox cb1 = (CheckBox)gvr.FindControl("check");
                        cb1.Checked = false;
                    }
                    selectAllBtn.ToolTip = ResourceManager.GetString("selectall");
                    selectAllBtn.ImageUrl = "img/selectall.gif";
                }
            }
            catch
            {
                ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('"+ResourceManager.GetString("noselect")+"')", true);
            }
        }

        /// <summary>
        /// 处理插入模板的操作
        /// </summary>
        protected void insertTemplate_Click(object sender, EventArgs e)
        {
            if (file_path.Text != "")
            {
                string line;
                StringBuilder strhtml = new StringBuilder();
                StreamReader sr = new StreamReader(Server.UrlDecode(HttpContext.Current.Request.Cookies["configpath"].Value)+file_path.Text.ToLower(), System.Text.Encoding.Default);
                    while ((line = sr.ReadLine()) != null)
                    {
                        strhtml.Append(line);
                    }
                    sr.Close();
                    templateContent.Value = strhtml.ToString();
                    ClientScript.RegisterStartupScript(typeof(Page), "Key", "insertTemplate();", true);

            }

        }

        /// <summary>
        /// 处理删除按键的操作
        /// </summary>
        protected void deleteBtn_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow dl in File_List.Rows)
            {
                CheckBox cb = (CheckBox)dl.FindControl("check");

                if (cb.Checked)
                {
                    GridViewRow row = (GridViewRow)cb.NamingContainer;
                    try
                    {
                        if ((File_List.Rows[row.RowIndex].FindControl("LengthCont") as Label).Text != "")
                        {
                            File.Delete(Server.MapPath(path.Text + (File_List.Rows[row.RowIndex].FindControl("ListID") as LinkButton).Text));
                        }
                        else
                        {
                            Directory.Delete(Server.MapPath(path.Text + (File_List.Rows[row.RowIndex].FindControl("ListID") as LinkButton).Text), true);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            getDirSize(Server.MapPath(path.Text));
        }

        /// <summary>
        /// 处理重命名按键的操作
        /// </summary>
        protected void editBtn_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow dl in File_List.Rows)
            {
                CheckBox cb = (CheckBox)dl.FindControl("check");

                if (cb.Checked)
                {
                    GridViewRow row = (GridViewRow)cb.NamingContainer;
                    File_List.EditIndex = row.RowIndex;
                    getDirSize(Server.MapPath(path.Text));
                    (File_List.Rows[File_List.EditIndex].FindControl("editName") as TextBox).Focus();
                    break;
                }
            }
        }

        /// <summary>
        /// 处理重命名取消的操作
        /// </summary>
        protected void File_List_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            File_List.EditIndex = -1;
            getDirSize(Server.MapPath(path.Text));
        }

        /// <summary>
        /// 处理重命名确定的操作,加强安全过滤
        /// </summary>
        protected void File_List_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string oldname = (File_List.Rows[e.RowIndex].FindControl("editBtn") as Button).CommandArgument;

            if ((File_List.Rows[e.RowIndex].FindControl("cancel") as Button).CommandArgument != "directory")
            {
                string[] typename = oldname.Split('.');
                File.Move(Server.MapPath(path.Text + oldname), Server.MapPath(path.Text)+(File_List.Rows[e.RowIndex].FindControl("editName") as TextBox).Text.Replace("\"", string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty) + "." + typename[typename.Length - 1].ToLower());

            }
            else
            {
                string newName = (File_List.Rows[e.RowIndex].FindControl("editName") as TextBox).Text.Replace("\"", string.Empty).Replace("/",string.Empty).Replace("\\",string.Empty);
                if (oldname != newName)
                {
                    Directory.Move(Server.MapPath(path.Text + oldname), Server.MapPath(path.Text) + newName);
                }
            }
            File_List.EditIndex = -1;
            getDirSize(Server.MapPath(path.Text));
        }
    }

    /// <summary>
    /// 通用页面代码
    /// </summary>
    public partial class PageCode : System.Web.UI.Page
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["languages"] != null)
                {
                    ResourceManager.SiteLanguageKey = Request.Cookies["languages"].Value;
                }
                else
                {
                    ResourceManager.SiteLanguageKey = HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"].ToLower().Split(',')[0];
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// 高亮代码功能的页面代码
    /// </summary>
    public partial class CodeConverter : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Button btnCodeHtmlify;
        protected System.Web.UI.WebControls.DropDownList ddlLanguages;
        protected System.Web.UI.WebControls.DropDownList ddlStyleType;
        protected System.Web.UI.WebControls.CheckBox chkIncludeLineNumbers;
        protected System.Web.UI.WebControls.RadioButton rdoUsePreTag;
        protected System.Web.UI.WebControls.TextBox txtCodeInput;
        protected System.Web.UI.WebControls.HiddenField htmlcontent;
        protected System.Web.UI.WebControls.RadioButton rdoConvertWhitespace;
        private Languages languages = null;

        /// <summary>
        /// Base Initialize
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (languages == null)
            {
                string defFile = Path.Combine(MapPathSecure(this.TemplateSourceDirectory), Languages.AppFilename);

                languages = Languages.Load(defFile);

                // If the defintion file doesn't exist on the server 
                // take the default languages.
                if (languages == null || languages.CodeLanguages.Count == 0)
                    languages = Languages.Load();
            }
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCodeHtmlify.Click += new System.EventHandler(this.btnCodeHtmlify_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }


        /// <summary>
        /// On page load all the files cs files in the dir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (IsPostBack == false)
            {
                // Load the possible Languanges
                this.ddlLanguages.Items.Clear();

                //Response.Write(Path.Combine(MapPathSecure(this.TemplateSourceDirectory), Languages.AppFilename));
                foreach (Language lang in languages.CodeLanguages)
                    this.ddlLanguages.Items.Add(lang.Name); 
                if (Request.Cookies["languages"] != null)
                {
                    ResourceManager.SiteLanguageKey = Request.Cookies["languages"].Value;
                }
                else
                {
                    ResourceManager.SiteLanguageKey = HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"].ToLower().Split(',')[0];
                }
                btnCodeHtmlify.Text = ResourceManager.GetString("HighlightButton");
                chkIncludeLineNumbers.Text = ResourceManager.GetString("LineNumberMarginVisibleCheckBox");
                rdoConvertWhitespace.Text = ResourceManager.GetString("convertwhitespace");
                rdoUsePreTag.Text = ResourceManager.GetString("usepre");
            }
        }
        /// <summary>
        /// This will display the colorized code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCodeHtmlify_Click(object sender, System.EventArgs e)
        {
            Language lang = (Language)languages.CodeLanguages[this.ddlLanguages.SelectedIndex];

            lang.ShowLineNumbers = this.chkIncludeLineNumbers.Checked;
            lang.StyleType = (StyleType)this.ddlStyleType.SelectedIndex;

            lang.UsePreTag = this.rdoUsePreTag.Checked; ;
            string htmlcode = lang.ApplyStyles(this.txtCodeInput.Text); ;

            if ((StyleType)this.ddlStyleType.SelectedIndex == StyleType.GlobalStyles)
            {
                htmlcode = string.Format("<style>{0}</style>\n{1}", lang.CodeElementsCSSStyles(), htmlcode);
            }
            htmlcontent.Value = htmlcode;
            ClientScript.RegisterStartupScript(typeof(Page), "Key", @"inserteditor()", true);
        }
    }

    #region 自定义控件的页面采集功能
    public partial class PageCollection : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Button btnReturn;
        protected System.Web.UI.WebControls.Button canceloading;
        protected System.Web.UI.WebControls.TextBox txtUrl;
        protected System.Web.UI.WebControls.DropDownList seltype;
        protected System.Web.UI.WebControls.HiddenField tempcontent;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnReturn.Text = ResourceManager.GetString("insertpage");
                seltype.Items.Add(new ListItem(ResourceManager.GetString("getallcontent"), "1"));
                seltype.Items.Add(new ListItem(ResourceManager.GetString("getnoscriptcontent"), "2"));
                seltype.Items.Add(new ListItem(ResourceManager.GetString("getalltext"), "3"));
                seltype.Items.Add(new ListItem(ResourceManager.GetString("getallimg"), "4"));
                seltype.Items.Add(new ListItem(ResourceManager.GetString("getalllink"), "5"));
                canceloading.Text = ResourceManager.GetString("canceloading");
                if (Request.Cookies["languages"] != null)
                {
                    ResourceManager.SiteLanguageKey = Request.Cookies["languages"].Value;
                }
                else
                {
                    ResourceManager.SiteLanguageKey = HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"].ToLower().Split(',')[0];
                }
            }
        }

        public void btnReturn_Click(object sender, System.EventArgs e)
        {
            string url = txtUrl.Text.Trim();

            WebClient wb = new WebClient();
            try
            {
                byte[] pagedata = wb.DownloadData(@url);
                string result = Encoding.Default.GetString(pagedata);
                Match charSetMatch = Regex.Match(result, "<meta([^<]*)charset=([^<]*)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string webCharSet = charSetMatch.Groups[2].Value;
                if (webCharSet != "")
                {
                    result = Encoding.GetEncoding(webCharSet).GetString(pagedata);
                }

                string returnvalue = "";
                switch (seltype.SelectedValue)
                {
                    case "1":
                        returnvalue = result;
                        break;
                    case "2":
                        returnvalue = wipeScript(result);
                        break;
                    case "3":
                        returnvalue = NoHTML(result);
                        break;
                    case "4":
                        returnvalue = getImages(result);
                        break;
                    case "5":
                        returnvalue = getLink(result);
                        break;
                    default:
                        break;
                }

                tempcontent.Value = returnvalue;
                ClientScript.RegisterStartupScript(typeof(Page), "Key", "addeditor();", true);
            }
            catch
            {
                ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("getpageerror") + "')", true);

            }

        }

        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"<script[\s\S]+</script *>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<style[\s\S]+</style *>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }


        public static string wipeScript(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex5.Replace(html, ""); //过滤frameset
            return html;
        }

        public string getImages(string html)
        {
            string resultStr = "";
            string temp = "";
            string url = "";
            string[] url2;
            Regex r = new Regex(@"<IMG[^>]+src=\s*(?:'(?<src>[^']+)'|""(?<src>[^""]+)""|(?<src>[^>\s]+))\s*[^>]*>", RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(html);
            foreach(Match m in mc)
            {
                temp = m.Groups["src"].Value.ToLower();
                if (temp.IndexOf("http") == 0)
                {
                    resultStr += m.Value + "<br />";
                }
                else
                {
                    url2 = txtUrl.Text.Trim().Split('/');

                    try
                    {
                        if (url2.Length > 3)
                        {
                            url = txtUrl.Text.Trim().Replace(url2[url2.Length - 1], "");
                        }
                        else
                        {
                            url = txtUrl.Text.Trim();
                        }
                    }
                    catch
                    {
                        url = txtUrl.Text.Trim();
                    }

                    if (temp.IndexOf("/") == 0)
                    {

                        resultStr += m.Value.Replace(m.Groups["src"].Value, "http://" + url2[2] + m.Groups["src"].Value) + "<br/>";
                    }
                    else
                    {
                        resultStr += m.Value.Replace(m.Groups["src"].Value, url + m.Groups["src"].Value) + "<br/>";
                    }
                }
            }
            return resultStr;
        }

        public string getLink(string html)
        {
            string resultStr = "";
            string temp = "";
            string url = "";
            string[] url2;
            Regex re = new Regex(@"<a[^>]+href=\s*(?:'(?<href>[^']+)'|""(?<href>[^""]+)""|(?<href>[^>\s]+))\s*[^>]*>(?<text>.*?)</a>", RegexOptions.IgnoreCase);

            MatchCollection mc = re.Matches(html);
            foreach (Match m in mc)
            {
                temp = m.Groups["href"].Value.ToLower();
                if (temp.IndexOf("http") == 0)
                {
                    resultStr += m.Value + "<br/>";
                }
                else
                {
                    url2 = txtUrl.Text.Trim().Split('/');
                    try
                    {
                        if (url2.Length > 1)
                        {
                            url = txtUrl.Text.Trim().Replace(url2[url2.Length - 1], "");
                        }
                        else
                        {
                            url = txtUrl.Text.Trim();
                        }
                    }
                    catch
                    {
                        url = txtUrl.Text.Trim();
                    }

                    if (temp.IndexOf("/") == 0)
                    {
                        resultStr += m.Value.Replace(m.Groups["href"].Value, "http://" + url2[2] + m.Groups["href"].Value) + "<br/>";
                    }
                    else if (temp.IndexOf("mailto") == 0)
                    {
                        resultStr += m.Value + "<br/>";
                    }
                    else
                    {
                        resultStr += m.Value.Replace(m.Groups["href"].Value, url + m.Groups["href"].Value) + "<br/>";
                    }
                }
            }

            return resultStr;
        }
    }
    #endregion
}