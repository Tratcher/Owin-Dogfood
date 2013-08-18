using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KittenStatusCodes
{
    public class KittenStatusCodeMiddleware : OwinMiddleware
    {
        private const string ReplacementKey = "kittenstatuscode.ReplaceBody";
        private const string OriginalStreamKey = "kittenstatuscode.OriginalStream";
        private KittenStatusCodeOptions options;
        private IDictionary<int, string> links;

        public KittenStatusCodeMiddleware(OwinMiddleware next, KittenStatusCodeOptions options)
            : base(next)
        {
            this.options = options;
            links = new Dictionary<int, string>();
            GenerateLinks();
        }

        public async override Task Invoke(IOwinContext context)
        {
            context.Set(OriginalStreamKey, context.Response.Body);
            context.Response.Body = new StreamWrapper(context.Response.Body, InspectStatusCode, context);
            await Next.Invoke(context);
            
            StatusCodeAction action;
            string link;
            int statusCode = context.Response.StatusCode;
            bool? replace = context.Get<bool?>(ReplacementKey);
            if (!replace.HasValue)
            {
                // Never evaluated, no response sent yet.
                if (options.StatusCodeActions.TryGetValue(statusCode, out action)
                    && links.TryGetValue(statusCode, out link)
                    && action != StatusCodeAction.Ignore)
                {
                    await SendKitten(context, link);
                }
            }
            else if (replace.Value == true)
            {
                if (links.TryGetValue(statusCode, out link))
                {
                    await SendKitten(context, link);
                }
            }
        }

        private bool InspectStatusCode(IOwinContext context)
        {
            StatusCodeAction action;
            string link;
            int statusCode = context.Response.StatusCode;
            if (options.StatusCodeActions.TryGetValue(statusCode, out action)
                && action == StatusCodeAction.ReplaceResponseWithKittens
                && links.TryGetValue(statusCode, out link))
            {
                context.Set<bool?>(ReplacementKey, true);
                return action == StatusCodeAction.ReplaceResponseWithKittens;
            }
            context.Set<bool?>(ReplacementKey, false);
            return false;
        }

        private Task SendKitten(IOwinContext context, string link)
        {
            context.Response.Body = context.Get<Stream>(OriginalStreamKey);
            context.Response.ContentType = "text/html";
            string body = string.Format("<html><body><img src=\"{0}\" /></body></html>", 
                HttpUtility.HtmlEncode(link));
            context.Response.ContentLength = body.Length;
            return context.Response.WriteAsync(body);
        }

        // Courtesy of http://www.flickr.com/photos/girliemac/sets/72157628409467125
        private void GenerateLinks()
        {
            links.Add(100, "http://farm8.staticflickr.com/7162/6512768893_a821929823.jpg");
            links.Add(101, "http://farm8.staticflickr.com/7022/6540479029_730c095b63.jpg");

            links.Add(200, "http://farm8.staticflickr.com/7153/6512628175_6a4e8ab6ba.jpg");
            links.Add(201, "http://farm8.staticflickr.com/7149/6540221577_ed29955a01.jpg");
            links.Add(202, "http://farm8.staticflickr.com/7167/6540479079_16e97a624a.jpg");
            links.Add(204, "http://farm8.staticflickr.com/7154/6547319943_442c6509bb.jpg");
            links.Add(206, "http://farm8.staticflickr.com/7021/6514473163_4e2a681cbd.jpg");
            links.Add(207, "http://farm8.staticflickr.com/7141/6514472979_c44518c4ce.jpg");

            links.Add(300, "http://farm8.staticflickr.com/7019/6519540181_d4eae6ee7a.jpg");
            links.Add(301, "http://farm8.staticflickr.com/7022/6519540231_73756bac6c.jpg");
            links.Add(302, "http://farm8.staticflickr.com/7019/6508023829_3d44c4ac16.jpg");
            links.Add(303, "http://farm8.staticflickr.com/7007/6513125065_ef7cfa6256.jpg");
            links.Add(304, "http://farm8.staticflickr.com/7166/6540551929_eeee6bf3dd.jpg");
            links.Add(305, "http://farm8.staticflickr.com/7002/6540365403_01e93b44a3.jpg");
            links.Add(307, "http://farm8.staticflickr.com/7161/6513001269_edff1f0079.jpg");

            links.Add(400, "http://farm8.staticflickr.com/7022/6540669737_7527a5de13.jpg");
            links.Add(401, "http://farm8.staticflickr.com/7148/6508023065_8dae48a30b.jpg");
            links.Add(402, "http://farm8.staticflickr.com/7165/6513001321_8ecc400e0a.jpg");
            links.Add(403, "http://farm8.staticflickr.com/7173/6508023617_f3ffc34e17.jpg");
            links.Add(404, "http://farm8.staticflickr.com/7172/6508022985_b22200ced0.jpg");
        }
    }
}