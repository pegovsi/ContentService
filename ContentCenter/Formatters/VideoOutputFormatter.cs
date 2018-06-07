using ContentCenter.HelperVideo;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.Formatters
{
    public class VideoOutputFormatter : IOutputFormatter
    {
        public bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
        //    if (context == null)
        //        throw new ArgumentNullException(nameof(context));

        //    if (context.Object is PushStreamResult)
        //        return true;

            return false;
        }

        public async Task WriteAsync(OutputFormatterWriteContext context)
        {
        //    if (context == null)
        //        throw new ArgumentNullException(nameof(context));

        //    using (var stream = ((PushStreamResult)context.Object))
        //    {
        //        var response = context.HttpContext.Response;
        //        if (context.ContentType != null)
        //        {
        //            response.ContentType = context.ContentType.ToString();
        //        }

        //        await stream.CopyToAsync(response.Body);
        //    }
        }
    }
}
