using GoorehApplication.DTOs.UserProductDtos;
using GoorehDomain.Entities;
using GoorehInfrastructure.DbContextes;
using Microsoft.EntityFrameworkCore;

namespace GoorehApi.Endpoints.UserProducts
{
    public static class UserProductsEndPoint
    {
        public static IEndpointRouteBuilder MapUserProductEndPoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/product");
            group.MapPost("/add",(GoorehDbContext db , AddUserProductsDto dto) =>
            {
                //var prod = new UserProduct() 
                //{ 
                //    Title = dto.Title,
                //    Value = dto.Value,
                //};
                db.UserProducts.Add(new UserProduct()
                {
                    Title = dto.Title,
                    Value = dto.Value,
                });
                db.SaveChanges();
                return Results.Ok();
            });
            group.MapPost("/update/{guid}", (string guid, GoorehDbContext db, UpdateUserProductsDto dto) =>

            {
                var entity = db.UserProducts.FirstOrDefault(s => s.Guid == guid);

                if (entity == null)
                    return Results.NotFound(new { msg = "محصول یافت نشد" });

                // concurrency check
                if (entity.ConcurrencyStamp != dto.ConcurrencyStamp)
                    return Results.Conflict(new { msg = "قبل از تغییر شما این فیلد توسط کاربر دیگر تغییر کرده است" });

                // update fields
                entity.Title = dto.Title;
                entity.Value = dto.Value;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Results.Conflict(new { msg = "قبل از تغییر شما این فیلد توسط کاربر دیگر تغییر کرده است" });
                }

                return Results.Ok(new 
                {
                    msg="با موفقیت انجام شد"

                });
            });
            return app;
        }
    }
}
