using GoorehApplication.RepositorysInterfaces;
using GoorehDomain.Entities;

namespace GoorehApi.Endpoints.UserContacts
{
    public static class UserContactEndpoint
    {
        public static IEndpointRouteBuilder MapUserContactEndpoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/usercontact");

            group.MapPost("/add",async (IGenericRepository<UserContact> repo,UserContact uc) =>
            {
                await repo.AddAsync(uc);
                await repo.SaveChangesAsync();
            });
            group.MapPost("/list", (IGenericRepository<UserContact> repo) =>
            {
                return  repo.SimpleGetAll().ToList();
                
            });

            return app;
        }
        
    }
}
