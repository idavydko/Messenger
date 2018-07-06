using Microsoft.EntityFrameworkCore;
using Messenger.UI.Models.Sms;
using Messenger.UI.Models.Facebook;

namespace Messenger.UI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<SmsUser> SmsUsers { get; set; }

        public DbSet<FacebookUser> FacebookUsers { get; set; }
    }
}
