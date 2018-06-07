using ContentCenter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;

namespace ContentCenter.Attributes
{
    public class AutorizeServiceAttribute: Attribute,IActionFilter
    {
        private IAutorizationService _autorizationService;

        public AutorizeServiceAttribute()
        {
            
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _autorizationService = GetService(context, typeof(IAutorizationService));

            if (_autorizationService == null)
                context.Result = new StatusCodeResult(500);

            if (!_autorizationService.IsUserFrom1CByBase64(GetToken(context)))
                context.Result = new UnauthorizedResult();
        }

        private static IAutorizationService GetService(ActionExecutingContext context, Type service)
        {           
            try
            {
                return (IAutorizationService)context.HttpContext.RequestServices.GetService(service);
            }
            catch (Exception ex)
            {
                Log.Error($"Ошибка при получении сервиса {service.ToString()}. Описание:{ex.Message}");
                return null;
            }
        }

        private static string GetToken(ActionExecutingContext context)
        {
            string authorizetoken = null;
            string token = string.Empty;

            var headers = context.HttpContext.Request.Headers;
            foreach (var item in headers)
            {
                if (item.Key == "Authorization")
                {
                    authorizetoken = item.Value.ToString();
                }
            }
            if (authorizetoken != null)
            {
                token = authorizetoken.Split(' ')[1];
            }

            return token;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }    
}
