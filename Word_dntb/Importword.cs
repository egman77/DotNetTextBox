using System;
using System.Web;
using System.IO;
using System.Text;
using System.Web.UI;
using DotNetTextBox;
using Word.Plugin;

namespace Word_dntb
{
    #region word文档导入编辑器的页面代码

    /// <summary>
    /// word文档导入编辑器的类
    /// </summary>
    public partial class Importword : System.Web.UI.Page
    {
        string wordfile = "";
        public string uploadpath = "";
        protected System.Web.UI.WebControls.HiddenField worddoc;
        protected System.Web.UI.WebControls.FileUpload FileUpload1;
        protected System.Web.UI.WebControls.CheckBox saveword;
        protected System.Web.UI.WebControls.Button btnUpload;
        protected System.Web.UI.WebControls.Button canceloading;
        /// <summary>
        /// 页面的初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["uploadFolder"] != null)
            {
                //修改Word文件档上传路径
                uploadpath = Request.Cookies["uploadFolder"].Value.ToLower();
            }
            
            if (!IsPostBack)
            {
                Response.Expires = -1;
                if (Request.Cookies["languages"] != null)
                {
                    ResourceManager.SiteLanguageKey = Request.Cookies["languages"].Value;
                }
                else
                {
                    ResourceManager.SiteLanguageKey = HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"].ToLower().Split(',')[0];
                }
                btnUpload.Text = ResourceManager.GetString("importeditor");
                saveword.Text = ResourceManager.GetString("saveword");
                canceloading.Text = ResourceManager.GetString("canceloading");
            }
        }

        /// <summary>
        /// 处理上传word文件并生成HTML的操作
        /// </summary>
        public bool uploadWord()
        {
            //文件内容为空时,也允许上传时 HaseFile==false
            if (FileUpload1.HasFile)
            {
                string fileName = FileUpload1.PostedFile.FileName;
                FileInfo file = new FileInfo(fileName);
                string extendName = file.Extension.ToLower();
                try
                {
                    if (extendName == ".doc" || extendName == ".docx")
                    {
                        //默认可上传WORD文档的限制为4M,可自行修改
                        if (FileUpload1.PostedFile.ContentLength < (4096 * 1024))
                        {
                            string y = DateTime.Now.Year.ToString();
                            string m = DateTime.Now.Month.ToString();
                            string d = DateTime.Now.Day.ToString();
                            string h = DateTime.Now.Hour.ToString();
                            string n = DateTime.Now.Minute.ToString();
                            string s = DateTime.Now.Second.ToString();
                            string wordfileName = y + m + d + h + n + s;
                            Random r = new Random();
                            fileName = fileName + r.Next(1000);
                            wordfile = System.Web.HttpContext.Current.Server.MapPath(uploadpath + wordfileName + FileUpload1.PostedFile.ContentLength.ToString() + extendName);
                            if (!File.Exists(wordfile))
                            {

                                FileUpload1.PostedFile.SaveAs(wordfile);
                                string htmlname = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() +
                System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString();
                                string htmlUrl = HttpContext.Current.Server.MapPath(uploadpath + htmlname + ".html");
                                WordDntb.buildWord(wordfile, Page.MapPath(uploadpath + htmlname + ".html"));

                                string line;
                                StringBuilder strhtml = new StringBuilder();
                                StreamReader sr = new StreamReader(htmlUrl, System.Text.Encoding.Default);

                                while ((line = sr.ReadLine()) != null)
                                {
                                    strhtml.Append(line);
                                }
                                sr.Close();
                                string content = strhtml.ToString().Replace(htmlname, Request.CurrentExecutionFilePath.Replace("importword.aspx", "") + uploadpath + htmlname);
                                worddoc.Value = content;
                                if (!saveword.Checked)
                                {
                                    File.Delete(wordfile);
                                }
                                File.Delete(htmlUrl);
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
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
                return true;
            }
            else if(!string.IsNullOrWhiteSpace(FileUpload1.PostedFile.FileName)&&FileUpload1.PostedFile.ContentLength==0)
            {
                //ClientScript.RegisterStartupScript(typeof(Page), "Key", "alert('" + ResourceManager.GetString("emptyContent") + "')", true);
                var message = ResourceManager.GetString("emptyContent");
                ClientScript.RegisterStartupScript(typeof(Page), "Key", $"hideMenu('{message}',true);", true);
               
                return false;
            }
            else
            {
               
                return false;
            }
        }

        /// <summary>
        /// 处理上传按键的操作
        /// </summary>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (uploadpath != "")
            {
                if (uploadWord())
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "Key", "addeditor();", true);
                }
                else
                {
                    // errer message uploadfail noUploadedFile
                    string errerMessage = ResourceManager.GetString("uploadfail") +"<br/>"+ ResourceManager.GetString("noUploadedFile");
                    ClientScript.RegisterStartupScript(typeof(Page), "Key", $"hideMenu('{errerMessage}',false);", true);
                }
            }
        }
    }
    #endregion
}