using System.Reflection.Emit;
using ExpenseTrackerProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerProject.Data
{
    public class ApplicationDBContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().
             HasMany(u => u.Incomes).
             WithOne(i => i.User).
             HasForeignKey(i => i.UserId);
            builder.Entity<User>().
                HasMany(u => u.Outcomes).
                WithOne(o => o.User).
                HasForeignKey(o => o.UserId);
            builder.Entity<User>().
                Property(u => u.Balance).HasDefaultValue(0);
            builder.Entity<User>().
                Property(u => u.AllowedMinus).HasDefaultValue(0);
        }
    }
}
