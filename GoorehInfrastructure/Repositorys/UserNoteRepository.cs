using GoorehApplication.DTOs.UserNote;
using GoorehApplication.RepositorysInterfaces;
using GoorehDomain.Entities;
using GoorehInfrastructure.DbContextes;
using GoorehInfrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoorehInfrastructure.Repositorys
{
    public class UserNoteRepository :  GenericRepository<UserNote>, IAddValidatorRepository<UserNote>
    {
        public UserNoteRepository(GoorehDbContext dbContext) : base(dbContext)
        {

        }

        public  bool AddValidate(UserNote usernote)
        {
            AddErrorMessages.Clear();
            if (string.IsNullOrWhiteSpace(usernote.Text))
            {
                AddErrorMessages.Add("Text", "value is Empty or WhiteSpace ");
            }
            if (string.IsNullOrWhiteSpace(usernote.Title))
            {
                AddErrorMessages.Add("Title", "value is Empty or WhiteSpace ");

            }
            if (AddErrorMessages.Any())
            {
                throw new AddValidationException(AddErrorMessages);
            }
            return !AddErrorMessages.Any();
        
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

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
