using System.IO;
using System.Threading.Tasks;
using ContentCenter.Attributes;
using ContentCenter.Helpers;
using ContentCenter.Models;
using ContentCenter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ContentCenter.Controllers
{    
    [Route("api/v1/picture")]
    [AutorizeService]
    public class PictureController : Controller
    {
        private readonly IContentService _contentService;       
        private readonly IOptions<ImageCatalog> _options;
        private readonly IOptions<OperationSystem> _os;

        public PictureController(IContentService contentService,
       
            IOptions<ImageCatalog> options,
            IOptions<OperationSystem> os)
        {
            _contentService = contentService;       
            _options = options;
            _os = os;
        }

        public async Task<IActionResult> Get(string key)
        {
            var catalog = _options.Value.Path;
            string path = string.Empty;

            path = $"{catalog}{key}";

            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                return new NotFoundResult();

            byte[] mas = System.IO.File.ReadAllBytes(path);
            var _type = FileHelper.GetContentType(fileInfo.Extension);
            return File(mas, _type.TypeFileString, fileInfo.Name);
                        
        }

    }
}