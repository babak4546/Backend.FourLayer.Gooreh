using GoorehApplication.DTOs.LogDtos;
using GoorehInfrastructure.DbContextes;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GoorehApi.Endpoints.MyLogs
{
    public static class UserLogEndpoint
    {
        public static  IEndpointRouteBuilder MapUsersLogEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/userlogs");

            
            group.MapPost("/list/{by}", async (string? by, GoorehDbContext db) =>
            {
                var list = db.UserLogDatas.AsQueryable();

                if (by == "decs")
                {
                    return Results.Ok(await list.OrderByDescending(w => w.LogDate).ToListAsync());
                }
                else if (by == "asc")
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
            // fagat log yeh guid khas ro mifresteh
            group.MapPost("/byuser/{guid}", async (GoorehDbContext db, string guid) =>
            {
                var findUserByGuid = await db.AppUsers.FirstOrDefaultAsync(s => s.Guid == guid);

                if (findUserByGuid == null)
                    return Results.NotFound();

                var logs = await db.UserLogDatas
                    .Where(s => s.AppUserId == findUserByGuid.Id)
                    .Select(s => new SpecialUserLogData
                    {
                        LogedIn = s.LogedIn,
                        LoggedOut = s.LoggedOut,
                        Action = s.Action,
                        IpAddr = s.IpAddr,
                        SysInfo = s.SysInfo,
                        LogDate= s.LogDate
                    })
                    .ToListAsync();

                return Results.Ok(logs);
            });
            //check with this apis on thunder client
            //http://localhost:7177/userlogs/byuser/sorted/{guid}/?sort=asc
            //http://localhost:7177/userlogs/byuser/sorted/{guid}/?sort=decs
            group.MapPost("/byuser/sorted/{guid}", async (GoorehDbContext db, string guid, string? sort) =>
            {
                var findUserByGuid = await db.AppUsers.FirstOrDefaultAsync(s => s.Guid == guid);

                if (findUserByGuid == null)
                    return Results.NotFound();

                var query = db.UserLogDatas
                    .Where(s => s.AppUserId == findUserByGuid.Id);

                // switch jadid C# migeh roye sort switch kon  asc bod ... decs bod ....
                query = sort switch
                {
                    //bar asas zaman sabt log
                    // az gadimi tarin record ha 
                    "asc" => query.OrderBy(s => s.LogDate),
                    //az jadid beh ghadimi
                    "desc" => query.OrderByDescending(s => s.LogDate),
                    
                    "def" => query.OrderByDescending(s => s.LogDate),

                    // _ in yani har meqdar digehee gir az 2 ta balaee ya hamon default switch
                    _ => query.OrderByDescending(s => s.LogDate) 
                };

                var logs = await query
                    .Select(s => new SpecialUserLogData
                    {
                        LogedIn = s.LogedIn,
                        LoggedOut = s.LoggedOut,
                        Action = s.Action,
                        IpAddr = s.IpAddr,
                        SysInfo = s.SysInfo,
                        LogDate = s.LogDate
                    })
                    .ToListAsync();

                return Results.Ok(logs);
            });

            group.MapPost("/find/{id}", (string id, GoorehDbContext db) =>
            {
                var log = db.UserLogDatas.FirstOrDefault(s => s.LogGuid == id);
                return Results.Ok(log);

            });
            group.MapPost("/test", () =>
            {             
                return Results.Ok();
            });
            return app;
        }
    }
}