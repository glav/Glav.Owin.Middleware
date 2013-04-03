using Owin;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System;
using Glav.Owin.Middleware;

namespace Glav.Owin.Consumer.Test.BasicWebConsumer
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    // Note the Web.Config owin:HandleAllRequests setting that is used to direct all requests to your OWIN application.
    // Alternatively you can specify routes in the global.asax file.
    public class Startup
    {
        // Invoked once at startup to configure your application.
        public void Configuration(IAppBuilder builder)
        {
            //builder.Use(typeof(TestLogger));
            // or
            builder.UseTestLogger();
            builder.Use(new Func<AppFunc, AppFunc>(ignoredNextApp => (AppFunc)Invoke));
        }

        // Invoked once per request.
        public Task Invoke(IDictionary<string, object> environment)
        {
            var response = SimpleResourceReader.ReadResource(environment);
            var responseBytes = response.FileResponse;

            // See http://owin.org/spec/owin-1.0.0.html for standard environment keys.
            Stream responseStream = (Stream)environment["owin.ResponseBody"];
            IDictionary<string, string[]> responseHeaders =
                (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];

            responseHeaders["Content-Length"] = new string[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };
            responseHeaders["Content-Type"] = new string[] { response.ContentType };

            //return Task.Factory.FromAsync(responseStream.BeginWrite, responseStream.EndWrite, responseBytes, 0, responseBytes.Length, null);
            return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }

    }
}