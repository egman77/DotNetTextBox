using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OleDb;

namespace DotNetTextBox
{
    public partial class Importexcel : System.Web.UI.Page
    {
        string excelfile = "";
        public string uploadpath = "";
        protected System.Web.UI.WebControls.HiddenField exceldoc;
        protected System.Web.UI.WebControls.FileUpload FileUpload1;
        protected System.Web.UI.WebControls.CheckBox saveexcel;
        protected System.Web.UI.WebControls.Button btnUpload;
        protected System.Web.UI.WebControls.Button canceloading;
        protected System.Web.UI.WebControls.DropDownList borderstyle;
        protected System.Web.UI.WebControls.TextBox bordersize;
        protected System.Web.UI.WebControls.TextBox tablebordercolor;

        /// <summary>
        /// 页面的初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["uploadFolder"] != null)
            {
                //修改excel文档上传路径
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
                saveexcel.Text = ResourceManager.GetString("saveexcel");
                canceloading.Text = ResourceManager.GetString("canceloading");
            }
        }

        /// <summary>
        /// 处理上传excel文件并生成HTML的操作
        /// </summary>
        public bool uploadExcel()
        {
            if (FileUpload1.HasFile)
            {
                string fileName = FileUpload1.PostedFile.FileName;
                FileInfo file = new FileInfo(fileName);
                string extendName = file.Extension.ToLower();
                try
                {
                    if (extendName == ".xls" || extendName == ".xlsx")
                    {
                        //默认可上传Excel文档的限制为4M,可自行修改
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
                            excelfile = System.Web.HttpContext.Current.Server.MapPath(uploadpath + wordfileName + FileUpload1.PostedFile.ContentLength.ToString() + extendName);
                            if (!File.Exists(excelfile))
                            {

                                FileUpload1.PostedFile.SaveAs(excelfile);
                                exceldoc.Value = ConvertToHtmlFile(import(excelfile),borderstyle.SelectedValue,bordersize.Text,tablebordercolor.Text);
                                if (!saveexcel.Checked)
                                {
                                    File.Delete(excelfile);
                                }
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
            else
            {
                return false;
            }
        }

        public DataTable import(string filepath)
        {
            DataTable rs = new DataTable();
            bool canopen = false;

            OleDbConnection conn = new OleDbConnection("provider=microsoft.jet.oledb.4.0;" +
            "data source=" + filepath + ";" +
            "extended properties=\"excel 8.0;\"");

            try//尝试数据连接是否可用 
            {
                conn.Open();
                conn.Close();
                canopen = true;
            }
            catch { }

            if (canopen)
            {
                try//如果数据连接可以打开则尝试读入数据 
                {
                    OleDbCommand myoledbcommand = new OleDbCommand("select * from [sheet1$]", conn);
                    OleDbDataAdapter mydata = new OleDbDataAdapter(myoledbcommand);
                    mydata.Fill(rs);
                    conn.Close();
                }
                catch//如果数据连接可以打开但是读入数据失败，则从文件中提取出工作表的名称，再读入数据 
                {
                }
            }
            return rs;
        }

        public ArrayList ExcelSheetName(string filepath)
        {
            ArrayList al = new ArrayList();
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable sheetNames = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            conn.Close();
            foreach (DataRow dr in sheetNames.Rows)
            {
                al.Add(dr[2]);
            }
            return al;
        }

        public string ConvertToHtmlFile(DataTable targetTable, string borderstyle, string bordersize, string tablebordercolor)
        {
            string myHtmlFile = "";
            if (targetTable == null) { throw new System.ArgumentNullException("targetTable"); }
            else
            {
            }

            StringBuilder myBuilder = new StringBuilder();

            myBuilder.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");

            myBuilder.Append("<head>");
            myBuilder.Append("<title>");
            myBuilder.Append("Page-");
            myBuilder.Append(Guid.NewGuid().ToString());
            myBuilder.Append("</title>");
            myBuilder.Append("</head>");
            myBuilder.Append("<body>");
            myBuilder.Append("<table border=1 borderColor=" + tablebordercolor + " cellpadding='5' cellspacing='0' ");
            myBuilder.Append("style='border:" + borderstyle + " " + bordersize + "px;font-size: x-small;'>");
            myBuilder.Append("<tr align='left' valign='top'>");

            foreach (DataColumn myColumn in targetTable.Columns)
            {
                myBuilder.Append("<td align='left' valign='top'>");
                myBuilder.Append(myColumn.ColumnName);
                myBuilder.Append("</td>");
            }

            myBuilder.Append("</tr>");

            foreach (DataRow myRow in targetTable.Rows)
            {
                myBuilder.Append("<tr align='left' valign='top'>");

                foreach (DataColumn myColumn in targetTable.Columns)
                {
                    myBuilder.Append("<td align='left' valign='top'>");
                    myBuilder.Append(myRow[myColumn.ColumnName].ToString());
                    myBuilder.Append("</td>");
                }
                myBuilder.Append("</tr>");
            }
            myBuilder.Append("</table>");
            myBuilder.Append("</body>");
            myBuilder.Append("</html>");
            myHtmlFile = myBuilder.ToString();
            return myHtmlFile;
        }


        /// <summary>
        /// 处理上传按键的操作
        /// </summary>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (uploadpath != "")
            {
                if (uploadExcel())
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "Key", "addeditor();", true);
                }
            }
        }
    }
}
