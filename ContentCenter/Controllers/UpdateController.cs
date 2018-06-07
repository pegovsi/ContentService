using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    [Route("api/v1/update")]
    [AutorizeService]
    public class UpdateController : Controller
    {
        private readonly IContentService _contentService;
        private readonly IAutorizationService _autorizationService;
        private readonly IOptions<UpdateCatalog> _options;

        public UpdateController(IContentService contentService,
            IAutorizationService autorizationService,
            IOptions<UpdateCatalog> options)
        {
            _contentService = contentService;
            _autorizationService = autorizationService;
            _options = options;
        }

        public async Task<IActionResult> Get(string key)
        {
            var catalog = _options.Value.Path;

            string path = $"{catalog}/{key}.zip";           

            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                return new NotFoundResult();

            byte[] mas = System.IO.File.ReadAllBytes(path);
            var _type = FileHelper.GetContentType(fileInfo.Extension);
            return File(mas, _type.TypeFileString, fileInfo.Name);
        }
    }
}