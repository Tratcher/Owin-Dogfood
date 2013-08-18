using KittenStatusCodes;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TratcherSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseErrorPage();
            app.Map("/statuscodes", statusapp =>
            {
                KittenStatusCodeOptions kittenOptions = new KittenStatusCodeOptions();
                statusapp.UseKittenStatusCodes(kittenOptions);
                statusapp.Run(new StatusCodePage(kittenOptions).Invoke);

            });
            app.UseWelcomePage();
        }
    }
}