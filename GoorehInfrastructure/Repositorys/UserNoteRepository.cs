using GoorehApplication.DTOs.UserNote;
using GoorehDomain.Entities;
using GoorehInfrastructure.DbContextes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehInfrastructure.Repositorys
{
    public class UserNoteRepository : GenericRepository<UserNote>
    {
        public UserNoteRepository(GoorehDbContext dbContext) : base(dbContext)
        {
        }

        public async Task AuthAddAsync(AddUserNoteRequest req, string user)
        {
            var findUser = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Guid == user);
            if (findUser != null)
            {
                var userNote = new UserNote
                {
                    AppUserId = findUser.Id,
                    Title = req.Title,
                    Text = req.Text,
                };
                await AddAsync(userNote);
                await SaveChangesAsync();
            }
        }

    }
}
