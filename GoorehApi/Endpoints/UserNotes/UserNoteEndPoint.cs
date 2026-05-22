using GoorehApplication.DTOs.UserNote;
using GoorehApplication.RepositorysInterfaces;
using GoorehDomain.Entities;
using GoorehInfrastructure.DbContextes;
using GoorehInfrastructure.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoorehApi.Endpoints.UserNotes
{
    public static class UserNoteEndPoint
    {
        public static IEndpointRouteBuilder MapUserNoteEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/usernote");

            group.MapPost("/add", async (GoorehDbContext db, ClaimsPrincipal claim, [FromBody] AddUserNoteRequest dto) =>
            {
                var claimguid = claim.Claims.FirstOrDefault(s => s.Type == "Guid")?.Value;
                var user = db.AppUsers.FirstOrDefault(s => s.Guid == claimguid);
                if (user != null)
                {
                    var usernote = new UserNote
                    {
                        AppUserId = user.Id,
                        Title = dto.Title,
                        Text = dto.Text,


                    };
                    db.UserNotes.Add(usernote);
                    db.SaveChanges();
                    return Results.Ok();

                }
                return Results.Ok();
            }).RequireAuthorization();
            //group.MapPost("/test",async (UserNoteRepository unr) =>
            //{
            //    return await unr.GetAll().ToListAsync();
            //});
            group.MapPost("/listrep", (UserNoteRepository unr) =>
            {
                var s = unr.GetAll().AsNoTracking();
                return Results.Ok(s);
            });
            group.MapPost("/addrep", async (UserNoteRepository rep, AddUserNoteRequest req, ClaimsPrincipal claim) =>
            {
                var user = claim.Claims.FirstOrDefault(s => s.Type == "Guid")?.Value;
                if (user is null)
                    return Results.Unauthorized();

                await rep.AuthAddAsync(req, user);
                return Results.Ok();
            });
            group.MapDelete("deleterepo/{id}", async (UserNoteRepository repo, int id) =>
            {
                await repo.Delete(id);
                await repo.SaveChangesAsync();
            });
            group.MapPut("/updaterepo{guid}", async (UserNoteRepository repo, string guid, UpdateUserNoteRequest dto) =>
            {
                var note = await repo.GetByGuidAsync(guid);
                if (note != null)
                {
                    note.Title = dto.Title;
                    note.Text = dto.Text;
                    repo.Update(note);
                }
                await repo.SaveChangesAsync();
                return Results.Ok(new { msg = "با موفقیت آپدیت شد" });
            });
            return app;
        }

    }
}
