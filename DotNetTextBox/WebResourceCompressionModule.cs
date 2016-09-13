using System;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.Hosting;
namespace WebResourceCompression
{
    //压缩DotNetTextBox脚本资源的类(*.axd)
    public class WebResourceCompressionModule : IHttpModule
    {
        private HttpApplication _app;
        private bool _isWebResourceRequest;
        private IHttpHandler _savedHandler;
        private bool _useGzip;
        private static bool IsEncodingInAcceptList(string acceptEncodingHeader, string expectedEncoding)
        {
            if (!string.IsNullOrEmpty(acceptEncodingHeader))
            {
                foreach (string str in acceptEncodingHeader.Split(new char[] { ',' }))
                {
                    string strA = str.Trim();
                    if (string.Compare(strA, expectedEncoding, StringComparison.Ordinal) == 0)
                    {
                        return true;
                    }
                    if (strA.StartsWith(expectedEncoding, StringComparison.Ordinal) && ((strA[expectedEncoding.Length] == ';') || (strA[expectedEncoding.Length] == ' ')))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void OnBeginRequest(object sender, EventArgs e)
        {
            this._isWebResourceRequest = false;
            this._useGzip = false;
            this._savedHandler = null;
            HttpRequest request = this._app.Request;
            if (request.FilePath.EndsWith("/webresource.axd", StringComparison.OrdinalIgnoreCase) && ((request.HttpMethod == "GET") || (request.HttpMethod == "HEAD")))
            {
                string str;
                this._isWebResourceRequest = true;
                string str2 = request.QueryString["d"];
                if (IsEncodingInAcceptList(request.Headers["Accept-encoding"], "gzip"))
                {
                    str = "d=" + str2 + "&z=1";
                    this._useGzip = true;
                }
                else
                {
                    str = "d=" + str2 + "&z=0";
                }
                this._app.Context.RewritePath(request.FilePath, string.Empty, str);
            }
        }

        private void OnPostRequestHandlerExecute(object sender, EventArgs e)
        {
            if (this._isWebResourceRequest && (this._savedHandler != null))
            {
                this._app.Context.Handler = this._savedHandler;
                this._savedHandler = null;
            }
        }

        private void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (this._isWebResourceRequest)
            {
                HttpContext context = this._app.Context;
                HttpRequest request = this._app.Request;
                HttpResponse response = this._app.Response;
                WebResourceWorkerRequest wr = new WebResourceWorkerRequest(request.HttpMethod, request.RawUrl);
                HttpContext context2 = new HttpContext(wr);
                context.Handler.ProcessRequest(context2);
                Stream filter = context2.Response.Filter;
                MemoryStream stream = new MemoryStream();
                if (this._useGzip)
                {
                    GZipStream stream3 = new GZipStream(stream, CompressionMode.Compress);
                    context2.Response.Filter = stream3;
                    context2.Response.Flush();
                    stream3.Close();
                }
                else
                {
                    context2.Response.Filter = stream;
                    context2.Response.Flush();
                }
                byte[] buffer = stream.ToArray();
                response.Clear();
                if (!context.IsDebuggingEnabled)
                {
                    HttpCachePolicy cache = response.Cache;
                    cache.SetCacheability(HttpCacheability.Public);
                    cache.VaryByParams["d"] = true;
                    cache.VaryByParams["z"] = true;
                    cache.SetOmitVaryStar(true);
                    cache.SetExpires(DateTime.Now + TimeSpan.FromDays(320.0));
                    cache.SetValidUntilExpires(true);
                    cache.SetLastModified(DateTime.Now);
                }
                response.ContentType = context2.Response.ContentType;
                if (this._useGzip)
                {
                    response.AddHeader("Content-encoding", "gzip");
                }
                response.OutputStream.Write(buffer, 0, buffer.Length);
                this._savedHandler = context.Handler;
                context.Handler = null;
            }
        }

        void IHttpModule.Dispose()
        {
        }

        void IHttpModule.Init(HttpApplication app)
        {
            this._app = app;
            this._app.BeginRequest += new EventHandler(this.OnBeginRequest);
            this._app.PreRequestHandlerExecute += new EventHandler(this.OnPreRequestHandlerExecute);
            this._app.PostRequestHandlerExecute += new EventHandler(this.OnPostRequestHandlerExecute);
        }

        private class WebResourceWorkerRequest : HttpWorkerRequest
        {
            private string _method;
            private string _path;
            private string _physicalPath;
            private string _queryString;
            private string _rawUrl;

            internal WebResourceWorkerRequest(string method, string webResourceUrl)
            {
                this._method = method;
                this._rawUrl = webResourceUrl;
                int index = this._rawUrl.IndexOf('?');
                if (index < 0)
                {
                    this._path = this._rawUrl;
                    this._queryString = string.Empty;
                }
                else
                {
                    this._path = this._rawUrl.Substring(0, index);
                    this._queryString = this._rawUrl.Substring(index + 1);
                }
                this._physicalPath = HostingEnvironment.MapPath(this._path);
            }

            public override void CloseConnection()
            {
            }

            public override void EndOfRequest()
            {
                throw new InvalidOperationException();
            }

            public override void FlushResponse(bool finalFlush)
            {
            }

            public override string GetAppPath()
            {
                return HostingEnvironment.ApplicationVirtualPath;
            }

            public override string GetAppPathTranslated()
            {
                return HostingEnvironment.ApplicationPhysicalPath;
            }

            public override string GetFilePath()
            {
                return this._path;
            }

            public override string GetFilePathTranslated()
            {
                return this._physicalPath;
            }

            public override string GetHttpVerbName()
            {
                return this._method;
            }

            public override string GetHttpVersion()
            {
                return "HTTP/1.0";
            }

            public override string GetKnownRequestHeader(int index)
            {
                return null;
            }

            public override string GetLocalAddress()
            {
                return "127.0.0.1";
            }

            public override int GetLocalPort()
            {
                return 80;
            }

            public override string GetPathInfo()
            {
                return string.Empty;
            }

            public override byte[] GetPreloadedEntityBody()
            {
                return null;
            }

            public override string GetQueryString()
            {
                return this._queryString;
            }

            public override string GetRawUrl()
            {
                return this._rawUrl;
            }

            public override string GetRemoteAddress()
            {
                return "127.0.0.1";
            }

            public override int GetRemotePort()
            {
                return 0;
            }

            public override string GetServerVariable(string name)
            {
                return string.Empty;
            }

            public override string GetUnknownRequestHeader(string name)
            {
                return null;
            }

            public override string[][] GetUnknownRequestHeaders()
            {
                return null;
            }

            public override string GetUriPath()
            {
                return this._path;
            }

            public override IntPtr GetUserToken()
            {
                return IntPtr.Zero;
            }

            public override bool HeadersSent()
            {
                return false;
            }

            public override bool IsEntireEntityBodyIsPreloaded()
            {
                return true;
            }

            public override string MapPath(string path)
            {
                return HostingEnvironment.MapPath(path);
            }

            public override int ReadEntityBody(byte[] buffer, int size)
            {
                return 0;
            }

            public override void SendCalculatedContentLength(int contentLength)
            {
                throw new InvalidOperationException();
            }

            public override void SendKnownResponseHeader(int index, string value)
            {
            }

            public override void SendResponseFromFile(IntPtr handle, long offset, long length)
            {
            }

            public override void SendResponseFromFile(string filename, long offset, long length)
            {
            }

            public override void SendResponseFromMemory(byte[] data, int length)
            {
            }

            public override void SendResponseFromMemory(IntPtr data, int length)
            {
            }

            public override void SendStatus(int statusCode, string statusDescription)
            {
            }

            public override void SendUnknownResponseHeader(string name, string value)
            {
            }
        }
    }
}