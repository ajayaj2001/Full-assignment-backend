using Microsoft.EntityFrameworkCore;
using System;
using Entities.DbContexts;
using System.IO;
using Entities.Models;

namespace UserUnitTest.InMemoryContext
{
    public static class InMemorydbContext
    {
        /// <summary>
        /// This method is used to create the InMemeorydatabase
        /// </summary>
        public static UserContext userContext()
        {
            DbContextOptions<UserContext> options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: "training_db").Options;
            UserContext context = new UserContext(options);

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string userPath = Path.Combine(baseDir, @"..\..\..\InMemoryContext\data\user.csv");
            string[] userValues = File.ReadAllText(Path.GetFullPath(userPath)).Split('\n');

            foreach (string item in userValues)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string[] row = item.Split(",");
                    {
                        if (context.Users.Find(Guid.Parse(row[0])) == null)
                        {
                            context.Users.Add(new User()
                            {
                                Id = Guid.Parse(row[0]),
                                EmailAddress = row[1],
                                Password = row[2],
                                IsActive = true,
                            });
                        }
                    }
                }
            }
            context.SaveChanges();
            return context;
        }
    }
}