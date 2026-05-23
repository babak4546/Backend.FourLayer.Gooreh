using Azure.Core;
using GoorehApi.Tools;
using GoorehApplication.DTOs.AuthDtos;
using GoorehDomain.Entities;
using GoorehDomain.Enums;
using GoorehInfrastructure.DbContextes;
using GoorehInfrastructure.Migrations;
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

            group.MapPost("/signup", async (GoorehDbContext db, IConfiguration config, [FromBody] UserSignupRequestDto userDto) =>
            {
                var salt = HashingPassword.GenerateSalt();
                var pepper = config["Security:MyPepper"];
                var user = new AppUser();
                var hashed = HashingPassword.HashPassword(userDto.Password, salt, pepper ?? "");
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
                user.RemovedIn = null;
                user.RestoredIn = null;
                user.LockoutEnd = null;
                await db.AppUsers.AddAsync(user);
                await db.SaveChangesAsync();
                return Results.Ok(new
                {
                    Msg = "ثبت نام انجام شد"
                });
            });

            group.MapPost("/login", async (GoorehDbContext db, IConfiguration config, [FromBody] UserLoginRequestDto userDto) =>
            {
                var concurrency = ConcurrencyCheck.Concurrency(userDto.UserName);
                var user = await db.AppUsers.FirstOrDefaultAsync(u => u.UpperUsername == concurrency);
                if (user == null)
                {
                    return Results.BadRequest(new
                    {
                        Msg = "نام کاربری یا رمز عبور اشتباه می باشد",
                        IsOk = false
                    });
                }
                if (user.IsRemoved==true)
                {
                    return Results.NotFound(new
                    {
                        msg="کاربر یافت نشد!",
                        IsOk=false
                    });
                }
                // ageh LockoutEnd (DateTime) megdar dashteh basheh va LockoutEnd megdarsh az lahzeh 
                // ejraye dobarehye endPoint bishtar basheh badRequest Mideh
                if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.Now)
                {
                    return Results.BadRequest(new
                    {
                        Msg = "حساب شما موقتاً قفل شده است. لطفاً چند ثانیه بعد دوباره تلاش کنید.",
                        IsOk = false
                    });
                }
                user.LockoutEnd = null;
                await db.SaveChangesAsync();
                // ba IConfiguration config beh file appsettings.json dastresi peyda mikonam va megdar pepper ro mikhonam
                var pepper = config["Security:MyPepper"];
                // inja ba password va salt keh az Db miad va pepper hash misazam
                var hashedPassword = HashingPassword.HashPassword(userDto.Password, user.Salt ?? "", pepper ?? "");

                if (hashedPassword != user.PasswordHash)
                {
                    // tedad dafaat vard kaardan password eshtebah 
                    user.AccessFailedCount++;
                    //  shart dovom migheh ageh ye karbar khas megdar LockoutEnabled== ture bood lock nasheh
                    if (user.AccessFailedCount >= 5 && user.IsLockedUp == true)
                    {
                        user.LockoutEnd = DateTime.Now.AddSeconds(15);
                        await db.SaveChangesAsync();

                        return Results.BadRequest(new
                        {
                            Msg = "به دلیل تلاش‌های زیاد، حساب شما برای 15 ثانیه قفل شد.",
                            IsOk = false
                        });
                    }

                    await db.SaveChangesAsync();

                    return Results.NotFound(new
                    {
                        Msg = "نام کاربری یا رمز عبور اشتباه می باشد",
                        IsOk = false
                    });
                }

                // meghdar AccessFailedCount reset misheh va LockoutEnd ham null
                user.AccessFailedCount = 0;
                user.LockoutEnd = null;

                db.UserLogDatas.Add(new UserLogData
                {
                    Action = "login",
                    AppUserId = user.Id,
                    LogedIn = DateTime.Now,
                    LogDate = DateTime.Now,
                });

                await db.SaveChangesAsync();


                var claims = new[]
                {
                     new Claim("Firstname", user.Firstname ?? ""),
                     new Claim("UserType", user.UserType.ToString()),
                     new Claim("Guid", user.Guid.ToString()),
                 };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? ""));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    config["Jwt:Issuer"],
                    config["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddDays(3),
                    signingCredentials: signIn
                );

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
                        //faghat baraye test, to db baraye :OnUsername ,delete ,OnFirstname, restoredBy ,ActOnGuid model nasakhtam

                        Action = " Action " + " delete " + " OnUsername= " + user.Username + " OnFirstname= " + user.Firstname + " OnLastname= " + user.Lastname + " DeletedBy= " + deletedBy + " ActOnGuid= " + guid,
                        AppUserId = deletedByUser.Id,
                        LogDate = DateTime.Now,

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
                var pepper = config["Security:MyPepper"];
                var hashed = HashingPassword.HashPassword(userDto.Password, salt, pepper ?? "");
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
            group.MapPost("/logout/{id}", async (GoorehDbContext db, string id) =>
            {
                var user = await db.AppUsers.FirstOrDefaultAsync(s => s.Guid == id);
                if (user != null)
                {
                    await db.UserLogDatas.AddAsync(new UserLogData
                    {

                        Action = "logout",
                        AppUserId = user.Id,
                        LogDate = DateTime.Now,
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
                           //faghat baraye test to db baraye :OnUsername ,restore ,OnFirstname, restoredBy ,ActOnGuid jadval nasakhtam 

                           Action = " Action " + " restore " + " OnUsername= " + user.Username + " OnFirstname= " + user.Firstname + " OnLastname= " + user.Lastname + " restoredBy= " + restoredBy + "ActOnGuid= " + guid,
                           AppUserId = restoredByUser.Id,
                           LogDate = DateTime.Now,


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
            //baraye test Lockout
            group.MapPost("/makeadmin/{id}", async (GoorehDbContext db, string id) =>
            {
                var user = await db.AppUsers.FirstOrDefaultAsync(s => s.Guid == id);
                if (user == null)
                {
                    return Results.NotFound(new
                    {
                        msg = "کاربر یافت نشد "
                    });
                }
                user.UserType = UserTypeEnum.AppAdmin;
                user.IsLockedUp = false;
                await db.SaveChangesAsync();
                return Results.Ok();
            });
            //baraye inkeh  PK AppUsers Fk UserLogData hast aval bayad log ha hazf shavand bad khode user 
            //dar gir in sorat log ha  bayad tavasot karbar hazf beshan baad user hazf besheh 
            group.MapPost("/deleteuserbyforce/{guid}", async (GoorehDbContext db, string guid) =>
            {
                var findUserByGuid = await db.AppUsers.FirstOrDefaultAsync(s => s.Guid == guid);
                if (findUserByGuid != null)
                {
                    var log = await db.UserLogDatas.Where(f => f.AppUserId == findUserByGuid.Id).ToListAsync();
                    db.RemoveRange(log);
                    db.Remove(findUserByGuid);
                    await db.SaveChangesAsync();
                    return Results.Ok(new
                    {
                        msg = "انجام شد"
                    });
                }
                return Results.NotFound();
            });
            //pishnahad copilot
            group.MapPost("/deleteuserforce/{guid}", async (GoorehDbContext db, string guid) =>
            {
                var user = await db.AppUsers.FirstOrDefaultAsync(s => s.Guid == guid);
                using var transaction = await db.Database.BeginTransactionAsync();
                //rollback Transaction
                try
                {

                    if (user == null)
                        return Results.NotFound();

                    // delete bedone load log to RAM  /az ef 7 beh bad ezafeh shodeh
                    await db.UserLogDatas
                        .Where(f => f.AppUserId == user.Id)
                        .ExecuteDeleteAsync();


                    await db.AppUsers
                        .Where(u => u.Id == user.Id)
                        .ExecuteDeleteAsync();

                    return Results.Ok(new { msg = "انجام شد" });
                }
                catch
                {
                    await transaction.RollbackAsync();
                }
                return Results.NotFound();
            });
            group.MapPost("/updateusername/{guid}", async (GoorehDbContext db, string guid, ChangeUsernameDto usernameDto) =>
            {
                var user = await db.AppUsers.FirstOrDefaultAsync(s => s.Guid == guid);

                if (user == null)
                {
                    return Results.NotFound(new
                    {
                        msg = "کاربر پیدا نشد"
                    });
                }

                var newUpper = ConcurrencyCheck.Concurrency(usernameDto.Username);

                // check mikoneh to db appUser Tekrari hast ya nah?
                bool exists = await db.AppUsers
                    .AnyAsync(s => s.UpperUsername == newUpper && s.Id != user.Id);

                if (exists)
                {
                    return Results.BadRequest(new
                    {
                        msg = "نام کاربری تکراری است"
                    });
                }

                // niazi beh db saveChange nist!
                await db.AppUsers
                    .Where(u => u.Guid == guid)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(p => p.Username, usernameDto.Username)
                        .SetProperty(p => p.UpperUsername, newUpper)
                    );

                return Results.Ok(new
                {
                    msg = "انجام شد"
                });
            });

            return app;
        }

    }
}
