using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KittenStatusCodes;

namespace Owin
{
    public static class KittenStatusCodeExtensions
    {
        public static IAppBuilder UseKittenStatusCodes(this IAppBuilder app)
        {
            return app.UseKittenStatusCodes(new KittenStatusCodeOptions());
        }

        public static IAppBuilder UseKittenStatusCodes(this IAppBuilder app, KittenStatusCodeOptions options)
        {
            return app.Use<KittenStatusCodeMiddleware>(options);
        }
    }
}