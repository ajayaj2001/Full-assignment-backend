using Microsoft.EntityFrameworkCore;
using System;
using Entities.Models;
using System.IO;

namespace Entities.DbContexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public void OnBeforeSaving(Guid UserId)
        {
            System.Collections.Generic.IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> productEntries = ChangeTracker.Entries();
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in productEntries)
            {
                if (entry.Entity is BaseModel userModel)
                {
                    Guid user = UserId;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            userModel.UpdatedOn = DateTime.Now;
                            userModel.UpdatedBy = user;
                            break;
                        case EntityState.Added:
                            userModel.UpdatedOn = DateTime.Now;
                            userModel.CreatedOn = DateTime.Now;
                            userModel.CreatedBy = user;
                            userModel.UpdatedBy = user;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string userPath = Path.Combine(baseDir, @"..\..\..\..\Entities\DbContext\data\user.csv");
            var temp = File.ReadAllText(Path.GetFullPath(userPath)).Split('\n');
            string[] userValues = File.ReadAllText(Path.GetFullPath(userPath)).Split('\n');

            foreach (string item in userValues)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string[] row = item.Split(",");

                    User user = new User()
                    {
                        Id = Guid.Parse(row[0]),
                        EmailAddress = row[1],
                        Password = row[2],
                        IsActive = true,
                        CreatedBy = Guid.Parse(row[0]),
                        UpdatedBy = Guid.Parse(row[0]),
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now

                    };
                    modelBuilder.Entity<User>().HasData(user);
                }
            }
            base.OnModelCreating(modelBuilder);
        }

    }
}