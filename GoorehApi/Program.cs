using GoorehApi.Endpoints.MyLogs;
using GoorehApi.Endpoints.Role;
using GoorehApi.Endpoints.UserContacts;
using GoorehApi.Endpoints.UserNotes;
using GoorehApi.Endpoints.UserProducts;
using GoorehApi.Endpoints.Users;
using GoorehApi.MyMiddleware.LogMiddlewares;
using GoorehApi.MyMiddleware.RequireAuthMiddlewares;
using GoorehApplication.Services.SecurityService;
using GoorehInfrastructure.DbContextes;
using GoorehInfrastructure.Exceptions;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//posheh haye obj va bin crash kardeh bodan error ENC0118 midad ro app.UseAuthentication() va ya 
//app.UseAuthorization(); ro in va dar nahayat ro using haye bala


builder.Services.AddOpenApi();

SecurityServices.AddServices(builder);
var app = builder.Build();
SecurityServices.UseServises(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseAuthentication();
//middleWare bayad gable az hameh ye api habasheh
//middleware ha default singleton hastan 
app.UseMiddleware<RquestLogMidleware>();
app.UseMiddleware<MyAuthMiddleWare>();
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (AddValidationException ex)
    {
        context.Response.StatusCode = 400;

        await context.Response.WriteAsJsonAsync(new
        {
            message = "Validation error",
            errors = ex.Errors
        });
    }
});
app.UseAuthorization();

app.MapUsersLogEndpoints();
app.MapRoleEndpoints();
app.MapUserEndpoints();
app.MapUserNoteEndpoints();
app.MapUserContactEndpoint();
app.MapUserProductEndPoint();
app.Run();

