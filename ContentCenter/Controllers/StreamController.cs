using System.IO;
using System.Threading.Tasks;
using ContentCenter.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContentCenter.Controllers
{    
    [Route("api/v1/stream")]
    public class StreamController : Controller
    {
        private readonly IContentService _contentService;
        private readonly IAutorizationService _autorizationService;

        public StreamController(IContentService contentService,
            IAutorizationService autorizationService)
        {
            _contentService = contentService;
            _autorizationService = autorizationService;
        }

        public async Task<IActionResult> Get(string key, string token)
        {
            if (!_autorizationService.IsUserFrom1C(token))
                return new UnauthorizedResult();


            if (string.IsNullOrEmpty(key))
                return new BadRequestResult();

            var _content = await _contentService.GetContentAsync(key);
           
            if (_content.TypeContent == Models.TypeContent.video)
            {
                MemoryStream stream = new MemoryStream(_content.Data);               
                return new FileStreamResult(stream, _content.Type);
            }
            else
                return new BadRequestResult();
        }
    }
}