using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ContentCenter.HelperVideo
{
    public class PushStreamResult : IActionResult
    {
        private readonly Action<Stream, HttpContent, TransportContext> _onStreamAvailabe;
        private readonly string _contentType;

        public PushStreamResult(Action<Stream, HttpContent, TransportContext> onStreamAvailabe, string contentType)
        {
            _onStreamAvailabe = onStreamAvailabe;
            _contentType = contentType;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var stream = context.HttpContext.Response.Body;
            context.HttpContext.Response.GetTypedHeaders().ContentType 
                = new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(_contentType);

            _onStreamAvailabe(stream, null, null);
            return Task.CompletedTask;
        }
    }
}
