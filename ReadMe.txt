----------------------------------------
DotNetTextBox V6.0 Retail 商业版使用说明
----------------------------------------
一、目录说明:
VS2005打开DotNetTextBox2005.sln项目文件,VS2008打开DotNetTextBox2008.sln项目文件即可运行演示项目,其中DotNetTextBox目录是控件的核心源代码，Word_dntb目录是控件导入WORD文档功能的核心源码,AajxSupport目录是让DotNetTextBox控件完美支持UpdatePanel的支持包,Plugin目录是控件外挂功能的插件,请按插件使用说明进行安装即可(插件是可选安装,不安装不会影响控件正常运行),WebSite目录是控件的演示网站实例（包括控件详细的使用演示及代码说明）。


二、后续升级:
商业版用户将拥有免费版用户所没有的商业版独有功能升级!


三、注意事项
(1)为控件启用服务器级别的GZIP压缩技术!(大大提高控件加载速度及性能)
默认网站实例此功能已经开启,如果你要在自己项目开启商业版控件这项压缩功能,请在项目的WEB.config的system.web配置节中加入:
	<system.web>

    <!--设置启用js脚本的服务端Gzip压缩-->
		<httpModules>
			<add name="WebResourceCompression" type="WebResourceCompression.WebResourceCompressionModule"/>
		</httpModules>

(2)关于EnvDTE.Dll的问题.
如果虚拟主机运行控件没有出现Could not load file or assembly 'EnvDTE, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'的错误,那么实例网站中BIN目录下的EnvDte.dll文件可以删除.

(3)BIN目录中其它DLL的使用说明.
*Word_dntb.dll、WordPlugin.dll
说明:为导入WORD文档功能使用到的文件,如果不需要此功能可以将其删除!


四、售后服务及技术支持的联系方法：
QQ群:12462711、6809118、6464466
Email:webmaster@aspxcn.com.cn
MSN:webmaster@aspxcn.com
订制功能及技术咨询请联系QQ群里的管理员,并且提供购买控件时注册的Email即可。


----------------------------------------------------------------------------------------
[AspxCn中华网]
http://www.aspxcn.com.cn
http://www.aspxblog.cn
http://www.dotnettextbox.com.cn

QQ技术讨论群:6809118、6464466、12462711(已满)

Google Group:http://groups.google.com/group/aspxcn/topics

[本站提供服务的内容]
Asp.Net网站专业制作
-专业承接各种Asp.Net网站的开发及功能定制,详细面议请加QQ:224377645 注明:ASP.NET网站开发

联系方式：
EMAIL:webmaster@aspxcn.com.cn
MSN:webmaster@aspxcn.com