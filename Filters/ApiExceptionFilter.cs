using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using WebApplication5.Services;
using WebApplication6.Services;

namespace WebApplication6.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var apiEx = context.Exception as ApiException;

            if (apiEx != null)
            {
                context.Response = context.Request.CreateResponse(
                    apiEx.StatusCode,
                    new { message = apiEx.Message });
                return;
            }

            context.Response = context.Request.CreateResponse(
                HttpStatusCode.InternalServerError,
                new { message = "ОШИБКА ВНУТРИ СЕРВЕРА: " + context.Exception.Message });
        }
    }
}