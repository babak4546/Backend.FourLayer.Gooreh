using GoorehApi.Endpoints.Logs;
using GoorehApi.Endpoints.Role;
using GoorehApi.Endpoints.Users;
using GoorehApi.MyMiddleware.LogMiddlewares;
using GoorehApplication.Services.SecurityService;
using GoorehInfrastructure.DbContextes;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);



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
//middleWare bayad gable az hameh ye api habasheh
//middleware ha default singleton hastan 
app.UseMiddleware<RquestLogMidleware>();
app.MapUsersLogEndpoints();
app.MapRoleEndpoints();
app.MapUserEndpoints();
app.Run();

