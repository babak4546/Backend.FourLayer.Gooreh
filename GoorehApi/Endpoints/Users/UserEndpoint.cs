using Azure.Core;
using GoorehApi.Tools;
using GoorehApplication.DTOs.AuthDtos;
using GoorehDomain.Entities;
using GoorehInfrastructure.DbContextes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoorehApi.Endpoints.Users
{
    public static class UserEndpoint
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {

            var group = app.MapGroup("/users");

            group.MapPost("/signup{ip}", async (GoorehDbContext db, IConfiguration config, [FromBody] UserSignupRequestDto userDto) =>
            {
                var salt = HashingPassword.GenerateSalt();
                var pepper = config["Security:MyPepper"];
                var user = new AppUser();
                var hashed = HashingPassword.HashPassword(userDto.Password,salt,pepper??"");
                var concurrency = ConcurrencyCheck.Concurrency(userDto.Username);
                var check = await db.AppUsers.FirstOrDefaultAsync(u => u.UpperUsername == concurrency);
                if (check != null)
                {
                    return Results.Ok(new
                    {
                        msg = "نام کاربری تکراری است"
                    });
                }
                user.Username = userDto.Username;
                user.UpperUsername = concurrency;
                user.Firstname = userDto.Firstname;
                user.Lastname = userDto.Lastname;
                user.PhoneNumber = userDto.PhoneNumber;
                user.PasswordHash = hashed;
                user.Salt = salt;
                await db.AppUsers.AddAsync(user);
                await db.SaveChangesAsync();
                return Results.Ok(new
                {
                    Msg = "ثبت نام انجام شد"
                });
            });

            group.MapPost("/login", async (GoorehDbContext db, IConfiguration config, [FromBody] UserLoginRequestDto userDto) =>
            {
                var user = await db.AppUsers.FirstOrDefaultAsync(u => u.Username == userDto.UserName);
                if (user == null)
                {
                    return Results.BadRequest(new
                    {
                        Msg = "نام کاربری یا رمز عبور اشتباه می باشد",
                        IsOk = false
                    });
                }
               
                var pepper = config["Security:MyPepper"];
                var hashedPassword = HashingPassword.HashPassword(userDto.Password,user.Salt??"",pepper??"");

                if (hashedPassword != user.PasswordHash)
                {
                    return Results.NotFound(new
                    {
                        Msg = "نام کاربری یا رمز عبور اشتباه می باشد",
                        IsOk = false
                    });
                }
                db.UserLogDatas.Add(new UserLogData
                {
                    Action = "login",
                    AppUserId = user.Id,
                    LogedIn = DateTime.Now,
                });
                await db.SaveChangesAsync();
                var claims = new[]
                {
                  new Claim("Firstname",user.Firstname??"".ToString()),
                  new Claim("UserType",user.UserType.ToString()),
                  new Claim("Guid",user.Guid.ToString()),

                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? ""));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(

                  config["Jwt:Issuer"],
                  config["Jwt:Audience"],
                  claims,
                  expires: DateTime.UtcNow.AddDays(3),
                  signingCredentials: signIn);

                return Results.Ok(new UserLoginResponseDto
                {
                    Msg = "خوش آمدید",
                    IsOk = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpiresIn = token.ValidTo.ToString(),
                });
            });

            group.MapDelete("/delete/{guid}", async (GoorehDbContext db, string guid, ClaimsPrincipal claim) =>
            {
                var user = await db.AppUsers.FirstOrDefaultAsync(x => x.Guid == guid);
                if (user is null)
                {
                    return Results.NotFound("کاربر یافت نشد");
                }
                var deletedBy = claim.Claims.FirstOrDefault(s => s.Type == "Guid")?.Value;

                var deletedByUser = await db.AppUsers.FirstOrDefaultAsync(d => d.Guid == deletedBy);
                if (user is not null && deletedByUser.Id != null)
                {

                    await db.UserLogDatas.AddAsync(new UserLogData
                    {
                        Action = " Action " + " delete " + " OnUsername= " + user.Username + " OnFirstname= " + user.Firstname + " OnLastname= " + user.Lastname + " DeletedBy= " + deletedBy,
                        AppUserId = deletedByUser.Id,
                        LoggedOut = DateTime.Now,

                    });
                    //db.AppUsers.Remove(user);
                    user.IsRemoved = true;
                    user.RemovedIn = DateTime.Now;
                    await db.SaveChangesAsync();
                    return Results.Ok("با موفقیت حذف شد");

                }
                return Results.Ok();
            }).RequireAuthorization();
            group.MapPut("/edit/{id}", async (GoorehDbContext db, IConfiguration config, string id, UserEditRequestDto userDto) =>
            {
                var salt = HashingPassword.GenerateSalt();
                var pepper = config["Security:Pepper"];
                var hashed = HashingPassword.HashPassword(userDto.Password,salt,pepper??"");
                var usernameConcurrency = ConcurrencyCheck.Concurrency(userDto.Username);

                var user = await db.AppUsers.FirstOrDefaultAsync(e => e.Guid == id);

                if (user == null)
                {
                    return Results.Ok(new { msg = "کاربر یافت نشد" });
                }

                // چک تکراری بودن یوزرنیم (به جز خود کاربر)

                var usernameExists = await db.AppUsers
                    .AnyAsync(u => u.UpperUsername == usernameConcurrency && u.Guid != id);

                if (usernameExists)
                {
                    return Results.Ok(new { msg = "نام کاربری تکراری است" });
                }
                // آپدیت اطلاعات
                user.Firstname = userDto.Firstname;
                user.Lastname = userDto.Lastname;
                user.PhoneNumber = userDto.PhoneNumber;
                user.PasswordHash = hashed;
                user.UpperUsername = usernameConcurrency;
                user.Username = userDto.Username;

                await db.SaveChangesAsync();

                return Results.Ok(new { msg = "ویرایش انجام شد" });
            });
            group.MapGet("/list", async (GoorehDbContext db) =>
            {
                return Results.Ok(db.AppUsers.Where(s => s.IsRemoved == false).ToList());
            });
            group.MapPost("/logout{id}", async (GoorehDbContext db, string id) =>
            {
                var user = await db.AppUsers.FirstOrDefaultAsync(s => s.Guid == id);
                if (user != null)
                {
                    await db.UserLogDatas.AddAsync(new UserLogData
                    {
                        Action = "logout",
                        AppUserId = user.Id,
                        LoggedOut = DateTime.Now,
                    });
                    await db.SaveChangesAsync();
                    return Results.Ok();

                }
                return Results.Ok(new
                {
                    Msg = "خطا"
                });
            });
            group.MapPut("/restore/{guid}", async (GoorehDbContext db, string guid, ClaimsPrincipal claim) =>
            {

                var user = await db.AppUsers.FirstOrDefaultAsync(x => x.Guid == guid);
                if (user is null)
                {
                    return Results.NotFound("کاربر یافت نشد");
                }
                var restoredBy = claim.Claims.FirstOrDefault(s => s.Type == "Guid")?.Value;
                var restoredByUser = await db.AppUsers.FirstOrDefaultAsync(d => d.Guid == restoredBy);

                if (user is not null && restoredByUser.Id != null)
                {
                    await db.UserLogDatas.AddAsync
                       (new UserLogData
                       {
                           Action = " Action " + " restore " + " OnUsername= " + user.Username + " OnFirstname= " + user.Firstname + " OnLastname= " + user.Lastname + " restoredBy= " + restoredBy,
                           AppUserId = restoredByUser.Id,
                           LoggedOut = DateTime.Now,

                       });
                    //db.AppUsers.Remove(user);
                    user.IsRemoved = false;
                    user.RestoredIn = DateTime.Now;
                    await db.SaveChangesAsync();
                    return Results.Ok("کاربر با موفقیت بازگشت!");
                }
                return Results.Ok();
            }).RequireAuthorization();
            group.MapGet("/admincheck", (ClaimsPrincipal user) =>
            {

                if (user.Claims.FirstOrDefault(s => s.Type == "UserType")?.Value == "AppAdmin")
                {
                    return Results.Ok(new
                    {
                        msg = true
                    });
                }

                return Results.Ok(new
                {
                    msg = false
                });
            }).RequireAuthorization();
            return app;
        }
    }
}
