using GoorehApi.Endpoints.Users;
using GoorehApplication.Services.SecurityService;
using GoorehInfrastructure.DbContextes;
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
app.MapUserEndpoints();
app.Run();

