using ContentCenter.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ContentCenter.Controllers
{
    [Route("api/v1/[controller]")]
    public class ContentController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IContentService _contentService;
        private readonly IAutorizationService _autorizationService;
        
        public ContentController(IHostingEnvironment appEnvironment,
            IContentService contentService,
            IAutorizationService autorizationService)
        {
            _appEnvironment = appEnvironment;
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

            if (_content.TypeContent == Models.TypeContent.image)
            {
                byte[] mas = System.IO.File.ReadAllBytes(_content.Path);
                return File(mas, _content.Type, _content.FileName);
            }
            else if (_content.TypeContent == Models.TypeContent.video)
                return File(_content.Data, _content.Type, _content.FileName);
            else
                return new BadRequestResult();
        }
    }
}