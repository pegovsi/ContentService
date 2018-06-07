using System.IO;
using System.Threading.Tasks;
using ContentCenter.Attributes;
using ContentCenter.Helpers;
using ContentCenter.Models;
using ContentCenter.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ContentCenter.Controllers
{    
    [Route("api/v1/map")]    
    [AutorizeService]
    public class MapController : Controller
    {
        IHeaderDictionary headers = null;

        private readonly IContentService _contentService;
        private readonly IAutorizationService _autorizationService;
        private readonly IOptions<MapCatalog> _options;
        private readonly IOptions<OperationSystem> _os;

        public MapController(IContentService contentService,
            IAutorizationService autorizationService,
            IOptions<MapCatalog> options,
            IOptions<OperationSystem> os)
        {
            _contentService = contentService;
            _autorizationService = autorizationService;
            _options = options;
            _os = os;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string z, string x, string y)
        {
            var catalog =  _options.Value.Path;
            string path = string.Empty;

            if (_os.Value.Platform == System.PlatformID.Unix)
            {
                path = $"{catalog}/{z}/{x}/{y}.png";
            }
            else
            {
                path = $"{catalog}\\{z}\\{x}\\{y}.png";
            }
            
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                return new NotFoundResult();

            byte[] mas = System.IO.File.ReadAllBytes(path);
            var _type = FileHelper.GetContentType(fileInfo.Extension);
            return File(mas, _type.TypeFileString, fileInfo.Name);
        }        
    }
}