using Microsoft.AspNetCore.Authorization;

namespace GoorehApi.MyMiddleware.RequireAuthMiddlewares
{
    public class MyAuthMiddleWare
    {
        private readonly RequestDelegate _onNext;

        public MyAuthMiddleWare(RequestDelegate onNext)
        {
            _onNext = onNext;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var hasAuth = endpoint?.Metadata.GetMetadata<IAuthorizeData>() != null;
            if (context!=null)
            {
                //var hasAuth = endpoint?.Metadata.GetMetadata<IAuthorizeData>() != null;
                if (hasAuth && !context.User.Identity.IsAuthenticated)
                {
                    context.Response.StatusCode = 401;
                    //WriteAsJsonAsync khodesh  seryalize mikoneh (object ro tabdil mikoneh beh json)
                    await context.Response.WriteAsJsonAsync(new
                    {
                        status = 401,
                        message = "Shoma baraye dastresi beh in API mojaz nistid!",
                        success = false
                    });
                    return;//yani digeh beh endpoint va ya middleware badi naro...
                }
                //ageh endpoint requeierdAuth nabod..
                await _onNext(context);
            }

        }
    }
}
