using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Services.Contracts;
using System.Net;

namespace WebApi.Extensions
{


    // static classların tüm üyeleri de static olmalıdır.
    public static class ExceptionMiddlewareExtensions
    {

        /**
         * Bir istek geldiğinde istek Middleware'lerden geçer ve bir hata oluştuğunda bu hata ExceptionHandler Middleware tarafından yakalanır.
         * 
         * StatusCode default olarak 500 olarak belirlenir ve hata mesajı döndürülür.
         * Hata varsa loglanır ve hata mesajı JSON formatında döndürülür.
         * 
         */

        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerService logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature is not null)
                    {

                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };


                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }


    }
}
