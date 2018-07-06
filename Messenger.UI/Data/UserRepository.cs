using Messenger.UI.Data.Contracts;
using Messenger.UI.Models.Admin;
using Messenger.UI.Models.Facebook;
using Messenger.UI.Models.Sms;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.UI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var smsUsers = await _context.SmsUsers.AsNoTracking()
                .Select(x => new User
                {
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    CreatedDate = x.CreatedDate,
                    IsSmsUser = true
                })
                .ToListAsync();

            var fbUsers = await _context.FacebookUsers.AsNoTracking()
                .Select(x => new User
                {
                    Id = x.Id,
                    PhoneNumber = x.Username,
                    CreatedDate = x.CreatedDate,
                    IsFacebookUser = true
                })
                .ToListAsync();

            smsUsers.AddRange(fbUsers);

            return smsUsers.OrderBy(x => x.CreatedDate).ToList();
        }

        public async Task<IEnumerable<SmsUser>> GetSmsUsersAsync()
        {
            return await _context.SmsUsers.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<FacebookUser>> GetFacebookUsersAsync()
        {
            return await _context.FacebookUsers.AsNoTracking().ToListAsync();
        }

        public async Task<bool> AddSmsUser(SmsUser user)
        {
            if (await _context.SmsUsers.AnyAsync(x => x.PhoneNumber == x.PhoneNumber))
            {
                return false;
            }

            _context.SmsUsers.Add(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddFacebookUser(FacebookUser user)
        {
            if (await _context.FacebookUsers.AnyAsync(x => x.PSID == x.PSID))
            {
                return false;
            }

            _context.FacebookUsers.Add(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int userId, bool isSMSUser)
        {
            if (isSMSUser)
            {
                var smsUser = await _context.SmsUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);

                if (smsUser == null)
                    return false;

                _context.Remove(smsUser);
            }
            else
            {
                var fbUser = await _context.FacebookUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);

                if (fbUser == null)
                    return false;

                _context.Remove(fbUser);
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
