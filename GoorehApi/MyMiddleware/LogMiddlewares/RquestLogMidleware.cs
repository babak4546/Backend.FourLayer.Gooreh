using GoorehDomain.Entities;
using GoorehInfrastructure.DbContextes;

namespace GoorehApi.MyMiddleware.LogMiddlewares
{
    public class RquestLogMidleware
    {
        private readonly RequestDelegate _onNext;
        //dotnet ejazeh nemideh yeh scoped to Ctor Singleton ejra besheh pass ba di nemisheh Db ro bedim beh Middleware
        //private readonly GoorehDbContext _dbContext; pass in khata mideh
      
        public RquestLogMidleware(RequestDelegate onNext)
        {
            _onNext = onNext;
           
        }
        //pipline: masir oboor yeh request az vagti keh vared barnameh misheh ta vagti kharej misheh
        public async Task InvokeAsync(HttpContext context)
        {
            var db = context.RequestServices.GetRequiredService<GoorehDbContext>();
            var path = context.Request.Path;
            var method = context.Request.Method;

            //gereftan guid az rout
            //var guidFromRoute = context.Request.RouteValues["guid"]?.ToString();
            //gereftan guid az claims
            var claimGuid = context.User.FindFirst("Guid")?.Value;
            //gereftan IP 
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var hasToken = context.Request.Headers.ContainsKey("Authorization");
           
            var log = new MiddlewareLog
            {
                MiddleWareDate = DateTime.Now,
                Path = path,
                Method = method,
                Title = "Unkown",
                ContextUserGuid = claimGuid,
                ContextUserIp = ip,
                
            };
           db.MiddlewareLogs.Add(log);
            db.SaveChanges();
            //scaler ,swagger va.. ham kar nemikonan chon token nadaran
            //if (!hasToken)
            //{
            //    context.Response.StatusCode = 401;
            //    await context.Response.WriteAsync("Token nadiri , ejazeh anjam kari nadari");
            //    return;
            //}

            await _onNext(context);
        }
    }
}
