using Microsoft.Owin;
using Owin;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KittenStatusCodes
{
    public class StatusCodePage
    {
        private KittenStatusCodeOptions statusCodeOptions;

        // Note you may need to disable friendly error pages in IE to see some of the 4xx and 5xx results
        public StatusCodePage(KittenStatusCodeOptions statusCodeOptions)
        {
            this.statusCodeOptions = statusCodeOptions;
        }

        // Returns any status code given in the query, e.g. ?status=200, or links to some status codes.
        public Task Invoke(IOwinContext context)
        {
            string status = context.Request.Query["status"];
            int statusCode;
            if (!string.IsNullOrEmpty(status) && int.TryParse(status, out statusCode))
            {
                context.Response.StatusCode = statusCode;
                return Task.FromResult(0);
            }

            context.Response.ContentType = "text/html";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html><body>");
            foreach (var statusPair in statusCodeOptions.StatusCodeActions)
            {
                if (statusPair.Value != StatusCodeAction.Ignore)
                {
                    builder.AppendFormat("<a href=\"?status={0}\">{0} - {1}</a><br>\r\n",
                        statusPair.Key, (HttpStatusCode)statusPair.Key);
                }
            }
            builder.AppendLine("</body></html>");

            return context.Response.WriteAsync(builder.ToString());
        }
    }
}