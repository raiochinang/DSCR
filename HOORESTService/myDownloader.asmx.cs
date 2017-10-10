using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace HOORESTService
{
    /// <summary>
    /// Summary description for myDownloader
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class myDownloader : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public byte[] DownloadFile(string filename)
        {
            FileStream fs = null;
            fs = File.Open(filename, FileMode.Open, FileAccess.Read);
            byte[] b = new byte[fs.Length];
            fs.Read(b, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            return b;
        }

        [WebMethod]
        public void download(string p)
        {
            try
            {
                string filename = Path.GetFileName(p);
                string path = p;

                byte[] b = DownloadFile(p);
                HttpResponse r = Context.Response;
                r.Clear();
                r.BufferOutput = true;
                r.ContentType = "application/vnd.ms-excel";
                r.ContentEncoding = Encoding.UTF8;
                r.AppendHeader("Content-Disposition", "Attachment; Filename=\"" + filename + "\"");
                r.BinaryWrite(b);
                r.Flush();
                r.End();

                
            }
            catch (Exception e)
            {
                var x = e;
            }

        }
    }
}
