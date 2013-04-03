using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Glav.Owin.Consumer.Test.BasicWebConsumer
{
    public class SimpleResourceReader
    {
        private IDictionary<string, object> _environment;

        public SimpleResourceReader(IDictionary<string, object> environment)
        {
            _environment = environment;
        }

        public FileReadResult ReadResource()
        {
            var result = new FileReadResult { StatusCode = 404, ContentType="text/html" };
            
            var filePath = GetFullFilePath();
            if (System.IO.File.Exists(filePath))
            {
                result.FileResponse = Encoding.UTF8.GetBytes(System.IO.File.ReadAllText(filePath));
                result.StatusCode = 200;
            } else{
                result.StatusCode = 404;
                result.FileResponse = Encoding.UTF8.GetBytes("Resource not found");
            }

            var context = _environment["System.Net.HttpListenerContext"] as System.Net.HttpListenerContext;
            if (context != null)
            {
                context.Response.StatusCode = result.StatusCode;
            }
            return result;
        }

        private string GetFullFilePath()
        {
            var dir = System.IO.Directory.GetCurrentDirectory();
            var requestPath = _environment["owin.RequestPath"] as string;
            if (string.IsNullOrWhiteSpace(requestPath))
            {
                return null;
            }

            if (requestPath[0] != '/')
            {
                return null;
            }

            if (requestPath == "/")
            {
                return string.Format("{0}\\default.html",dir);
            }

            var normalisedPath = requestPath.ToString().Replace('/','\\');
            return string.Format("{0}{1}",dir,normalisedPath);
        }

        public static FileReadResult ReadResource(IDictionary<string, object> environment)
        {
            return new SimpleResourceReader(environment).ReadResource();
        }

    }

    public class FileReadResult
    {
        public int StatusCode { get; set; }
        public byte[] FileResponse { get; set; }
        public string ContentType { get; set; }
    }
}