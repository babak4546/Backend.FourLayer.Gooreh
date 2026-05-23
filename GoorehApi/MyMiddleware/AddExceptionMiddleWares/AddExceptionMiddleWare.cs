using GoorehInfrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GoorehApi.MyMiddleware.AddExceptionMiddleWares
{
    /// <summary>
    /// middleware vabasteh hast beh request
    /// </summary>
    public class AddExceptionMiddleWare
    {
        private readonly RequestDelegate _onNext;

        public AddExceptionMiddleWare(RequestDelegate onNext)
        {
            _onNext = onNext;
        }
        public async Task InvokeAsync(HttpContext http)
        {

            try
            {
                await _onNext(http);
            }
            catch (AddValidationException ex)
            {
                http.Response.StatusCode = 400;


                await http.Response.WriteAsJsonAsync(new
                {
                    message = "Validation error",
                    errors = ex.Errors
                });
            }
        }
    }
}
