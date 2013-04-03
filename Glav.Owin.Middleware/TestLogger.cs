using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glav.Owin.Middleware
{
    
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class TestLogger
    {
        private readonly AppFunc _next;

        public TestLogger(AppFunc next)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }
            _next = next;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Hitting TestLogger, path: {0}", environment["owin.RequestPath"]));
            return _next(environment);
        }
    }
}
