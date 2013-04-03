using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glav.Owin.Middleware
{
    public static class TestLoggerExtensions
    {
        public static void UseTestLogger(this IAppBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            builder.Use(typeof(TestLogger));
        }
    }
}
