using GoorehInfrastructure.DbContextes;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GoorehApi.Endpoints.Logs
{
    public static class UserLogEndpoint
    {
        public static async Task<IEndpointRouteBuilder> MapUsersLogEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/userlogs");


            group.MapPost("/list{by}", async (string? by, GoorehDbContext db) =>
            {
                var list = db.UserLogDatas.AsQueryable();

                if (by == "dec")
                {
                    return Results.Ok(await list.OrderByDescending(w => w.LogDate).ToListAsync());
                }
                else if (by == "asd")
                {
                    return  Results.Ok(await list.OrderBy(w => w.LogDate).ToListAsync());
                }
                else if (by == "def")
                {

                    return Results.Ok(await list.ToListAsync());
                }
                return Results.NotFound(new
                {
                    msg = "مقدار قابل قبولی وارد نشده "
                });
            });

            group.MapPost("/find{id}", (string id, GoorehDbContext db) =>
            {
                var log = db.UserLogDatas.FirstOrDefault(s => s.LogGuid == id);
                return Results.Ok(log);
            });

            return app;
        }
    }
}