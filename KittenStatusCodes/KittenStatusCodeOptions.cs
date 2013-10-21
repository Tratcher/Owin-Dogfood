using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KittenStatusCodes
{
    public class KittenStatusCodeOptions
    {
        private IDictionary<int, StatusCodeAction> statusCodeActions;

        public KittenStatusCodeOptions()
        {
            statusCodeActions = new Dictionary<int, StatusCodeAction>()
            {
                { 100, StatusCodeAction.Ignore },
                { 101, StatusCodeAction.Ignore },
                { 200, StatusCodeAction.FallbackToKittens },
                { 201, StatusCodeAction.FallbackToKittens },
                { 202, StatusCodeAction.FallbackToKittens },
                { 204, StatusCodeAction.Ignore },
                { 206, StatusCodeAction.FallbackToKittens },
                { 207, StatusCodeAction.FallbackToKittens },
                { 300, StatusCodeAction.FallbackToKittens },
                { 301, StatusCodeAction.Ignore },
                { 302, StatusCodeAction.Ignore },
                { 303, StatusCodeAction.Ignore },
                { 304, StatusCodeAction.FallbackToKittens },
                { 305, StatusCodeAction.FallbackToKittens },
                { 307, StatusCodeAction.Ignore },
                { 400, StatusCodeAction.FallbackToKittens },
                { 401, StatusCodeAction.FallbackToKittens },
                { 402, StatusCodeAction.FallbackToKittens },
                { 403, StatusCodeAction.FallbackToKittens },
                { 404, StatusCodeAction.ReplaceResponseWithKittens },
                { 500, StatusCodeAction.FallbackToKittens },
            };
        }

        public IDictionary<int, StatusCodeAction> StatusCodeActions
        {
            get { return statusCodeActions; }
        }
    }
}