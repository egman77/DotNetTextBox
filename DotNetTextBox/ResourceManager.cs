using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace DotNetTextBox
{
    #region �Զ���ؼ�������Ӧ���ʻ�����(������)
    /// <summary>
    /// ��Դ������
    /// ���ڶ�ȡ��Ӧ���Ե���Դ�ı�
    /// </summary>
    public class ResourceManager
    {
       
        /// <summary>
        /// վ��Ĭ������
        /// </summary>
        private static string defaultLanguage;
        //public static string ckey="none";
        /// <summary>
        /// ��ȡ������վ�㵱ǰ���Եļ�ֵ
        /// </summary>
        public static string SiteLanguageKey
        {
            get
            {
                return defaultLanguage;
            }
            set
            {
                ResourceManager.defaultLanguage = value;
            }
        }
   

        /// <summary>
        /// ��̬����,����ʵ�������������ö���
        /// </summary>
        static ResourceManager()
        {      
            defaultLanguage = ConfigurationManager.AppSettings["defaultLanguage"];
        }		

        /// <summary>
        /// ������Դ���ƴ���Դ�ļ��л�ȡ��Ӧ��Դ�ı�
        /// </summary>
        /// <param name="name">��Դ����</param>
        /// <returns>��Դ�ı�</returns>
        public static string GetString(string name) 
        {
            return GetString(name,"Resources.xml",false);
        }

        /// <summary>
        /// ������Դ���ƴ���Դ�ļ��л�ȡ��Ӧ��Դ�ı�
        /// </summary>
        /// <param name="name">��Դ����</param>
        /// <param name="defaultOnly">ֻѡ��Ĭ������</param>
        /// <returns>��Դ�ı�</returns>
        public static string GetString(string name, bool defaultOnly)
        {
            return GetString(name,"Resources.xml",defaultOnly);
        }

        /// <summary>
        /// ������Դ���ƴ���Դ�ļ��л�ȡ��Ӧ��Դ�ı�
        /// </summary>
        /// <param name="name">��Դ����</param>
        /// <param name="fileName">��Դ�ļ���</param>
        /// <returns></returns>
        public static string GetString(string name, string fileName)
        {
            return GetString(name,fileName,false);
        }

        /// <summary>
        /// ������Դ���ƴ���Դ�ļ��л�ȡ��Ӧ��Դ�ı�
        /// </summary>
        /// <param name="name">��Դ����</param>
        /// <param name="fileName">��Դ�ļ���</param>
        /// <param name="defaultOnly">ʹ��Ĭ������</param>
        /// <returns>��Դ�ı�</returns>
        public static string GetString(string name, string fileName, bool defaultOnly) 
        {
        	Hashtable resources = null;

            //�û�����
            string userLanguage = null;
                       
            if (fileName != null && fileName != "")
                resources = GetResource("dntb_", userLanguage, fileName, defaultOnly);
            else
                resources = GetResource("dntb_", userLanguage, "Resources.xml", defaultOnly);

            string text = resources[name] as string;


			if (text == null && fileName != null && fileName != "") 
			{
                resources = GetResource("dntb_",userLanguage, "Resources.xml", false);

				text = resources[name] as string;
			}

            return text;
        }    

        /// <summary>
        /// ��ȡ��Դ
        /// </summary>
        /// <param name="resourceType">��Դ����</param>
        /// <param name="userLanguage">�û�����</param>
        /// <param name="fileName">��Դ�ļ���</param>
        /// <param name="defaultOnly">ֻʹ��Ĭ������</param>
        /// <returns></returns>
        private static Hashtable GetResource (string resourceType, string userLanguage, string fileName, bool defaultOnly) {


            string defaultLanguage = ResourceManager.defaultLanguage;
            if (defaultLanguage == null || defaultLanguage == "")
            {
                if (HttpContext.Current.Request.Cookies["languages"] != null)
                {
                    defaultLanguage = HttpContext.Current.Request.Cookies["languages"].Value;
                }
                else
                {
                    defaultLanguage = HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"].ToLower().Split(',')[0];
                }
            }
            string cacheKey = resourceType + defaultLanguage + userLanguage + fileName;

            // ����û�û�ж�������,��ʹ��Ĭ��
            //
            //if (string.IsNullOrEmpty(userLanguage) || defaultOnly )
                //userLanguage = defaultLanguage;
            // �ӻ����л�ȡ��Դ
            //
            Hashtable resources = LanguageCache.Get(cacheKey) as Hashtable;
            if (resources == null)
            {
                resources = new Hashtable();

                try
                {
                    resources = LoadResource(resources, defaultLanguage, cacheKey, fileName);
                }
                catch
                {
                    resources = LoadResource(resources, "zh-cn", resourceType + "zh-cn" + userLanguage + fileName, fileName);
                }
            }
            return resources;
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="target"></param>
        /// <param name="language"></param>
        /// <param name="cacheKey"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
		private static Hashtable LoadResource (Hashtable target, string language, string cacheKey, string fileName) 
        {
            string filePath;
            try
            {
                if (HttpContext.Current.Request.Cookies["configpath"] != null)
                {
                    filePath = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies["configpath"].Value) + language + "/" + fileName;
                }
                else
                {
                    filePath = HttpContext.Current.Request.PhysicalApplicationPath + "/" + ConfigurationManager.AppSettings["systemfolder"].ToString() + language + "/" + fileName;
                }
            }
            catch
            {
                EnvDTE.DTE devenv = null;
                try
                {
                    devenv = (EnvDTE.DTE)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.8.0");
                }
                catch
                {
                    devenv = (EnvDTE.DTE)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.9.0");
                }
                string projectFile = devenv.ActiveDocument.ProjectItem.ContainingProject.FileName;
                System.IO.FileInfo info = new System.IO.FileInfo(projectFile);
                filePath = info.Directory.FullName + "/system_dntb/" + language + "/" + fileName;
            }

            CacheDependency dp = new CacheDependency(filePath);

			XmlDocument d = new XmlDocument();
			try {
				d.Load( filePath );
			} catch {
				return target;
			}

			foreach (XmlNode n in d.SelectSingleNode("root").ChildNodes) 
            {
				if (n.NodeType != XmlNodeType.Comment)
                {
				
							if (target[n.Attributes["name"].Value] == null)
								target.Add(n.Attributes["name"].Value, n.InnerText);
							else
								target[n.Attributes["name"].Value] = n.InnerText;
				}
			}

            if (language == ResourceManager.defaultLanguage)
            {
                LanguageCache.Max(cacheKey, target, dp);
            }
            else
            {
                LanguageCache.Insert(cacheKey, target, dp, LanguageCache.MinuteFactor * 5);
            }
            return target;
        }

    }
    #endregion
}