using GoorehInfrastructure.DbContextes;

namespace GoorehApi.Endpoints.Role
{
    public static class RoleEndpoint
    {
        public static IEndpointRouteBuilder MapRoleEndpoints( this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/roles");
            group.MapPost("/list",(GoorehDbContext db ) =>
            {
                var role =db.UserRoles.AsQueryable();
                

                return Results.Ok(role.ToList());
            });


        return app;
        }
    }
}