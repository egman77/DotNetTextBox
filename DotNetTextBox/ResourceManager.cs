using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace DotNetTextBox
{
    #region 自定义控件的自适应国际化功能(多语言)
    /// <summary>
    /// 资源管理类
    /// 用于读取相应语言的资源文本
    /// </summary>
    public class ResourceManager
    {
       
        /// <summary>
        /// 站点默认语言
        /// </summary>
        private static string defaultLanguage;
        //public static string ckey="none";
        /// <summary>
        /// 获取或设置站点当前语言的键值
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
        /// 静态构造,用于实例化上下文引用对象
        /// </summary>
        static ResourceManager()
        {      
            defaultLanguage = ConfigurationManager.AppSettings["defaultLanguage"];
        }		

        /// <summary>
        /// 根据资源名称从资源文件中获取相应资源文本
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns>资源文本</returns>
        public static string GetString(string name) 
        {
            return GetString(name,"Resources.xml",false);
        }

        /// <summary>
        /// 根据资源名称从资源文件中获取相应资源文本
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="defaultOnly">只选择默认语言</param>
        /// <returns>资源文本</returns>
        public static string GetString(string name, bool defaultOnly)
        {
            return GetString(name,"Resources.xml",defaultOnly);
        }

        /// <summary>
        /// 根据资源名称从资源文件中获取相应资源文本
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="fileName">资源文件名</param>
        /// <returns></returns>
        public static string GetString(string name, string fileName)
        {
            return GetString(name,fileName,false);
        }

        /// <summary>
        /// 根据资源名称从资源文件中获取相应资源文本
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="fileName">资源文件名</param>
        /// <param name="defaultOnly">使用默认语言</param>
        /// <returns>资源文本</returns>
        public static string GetString(string name, string fileName, bool defaultOnly) 
        {
        	Hashtable resources = null;

            //用户语言
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
        /// 获取资源
        /// </summary>
        /// <param name="resourceType">资源类型</param>
        /// <param name="userLanguage">用户语言</param>
        /// <param name="fileName">资源文件名</param>
        /// <param name="defaultOnly">只使用默认语言</param>
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

            // 如果用户没有定制语言,则使用默认
            //
            //if (string.IsNullOrEmpty(userLanguage) || defaultOnly )
                //userLanguage = defaultLanguage;
            // 从缓存中获取资源
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
        /// 加载资源
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