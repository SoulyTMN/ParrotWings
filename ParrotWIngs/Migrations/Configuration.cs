namespace ParrotWIngs.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ParrotWIngs.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ParrotWIngs.Models.ApplicationDbContext context)
        {
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            //  This method will be called after migrating to the latest version.
            context.Users.AddOrUpdate(x => x.Email,
                new ApplicationUser() { PwName = "Robert Jordan", Email = "rjordan@gmail.com", UserName = "rjordan@gmail.com", PasswordHash = new PasswordHasher().HashPassword("T3stPa$$w0rd") },
                new ApplicationUser() { PwName = "Jose Miaja", Email = "jm-miaja@terra.es", UserName = "jm-miaja@terra.es", PasswordHash = new PasswordHasher().HashPassword("T3stPa$$w0rd") },
                new ApplicationUser() { PwName = "Ivan Kashkin", Email = "iakashkin@mail.ru", UserName = "iakashkin@mail.ru", PasswordHash = new PasswordHasher().HashPassword("T3stPa$$w0rd") },
                new ApplicationUser() { PwName = "Mihail Koltsov", Email = "mekoltsov@yandex.ru", UserName = "mekoltsov@yandex.ru", PasswordHash = new PasswordHasher().HashPassword("T3stPa$$w0rd") },
                new ApplicationUser() { PwName = "Andre Marty", Email = "andremarty@sfr.fr", UserName = "andremarty@sfr.fr", PasswordHash = new PasswordHasher().HashPassword("T3stPa$$w0rd") }
                );
            context.SaveChanges();
            
            foreach (var dbUser in context.Users.ToArray())
            {
                switch (dbUser.Email) { 
                    case "rjordan@gmail.com":
                        context.UserAccounts.AddOrUpdate(x => x.UserId, new UserAccount() {  UserId = dbUser.Id, Balance = 400 });
                        break;
                    case "jm-miaja@terra.es":
                        context.UserAccounts.AddOrUpdate(x => x.UserId, new UserAccount() { UserId = dbUser.Id, Balance = 550 });
                        break;
                    case "iakashkin@mail.ru":
                        context.UserAccounts.AddOrUpdate(x => x.UserId, new UserAccount() { UserId = dbUser.Id, Balance = 550 });
                        break;
                    case "mekoltsov@yandex.ru":
                        context.UserAccounts.AddOrUpdate(x => x.UserId, new UserAccount() { UserId = dbUser.Id, Balance = 550 });
                        break;
                    case "andremarty@sfr.fr":
                        context.UserAccounts.AddOrUpdate(x => x.UserId, new UserAccount() { UserId = dbUser.Id, Balance = 450 });
                        break;
                } 
                
                if (string.IsNullOrEmpty(dbUser.SecurityStamp)) userManager.UpdateSecurityStamp(dbUser.Id);

            }

            //Reseed "Transactions" table
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.Transactions");
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('dbo.Transactions',RESEED,1)");

            string payeeId = context.Users.Where(x => x.Email == "rjordan@gmail.com").Select(x => x.Id).FirstOrDefault();
            string recipientId = context.Users.Where(x => x.Email == "jm-miaja@terra.es").Select(x => x.Id).FirstOrDefault();

            context.Transactions.AddOrUpdate(x => x.Id,
                new Transaction() { Id = 1, PayeeId = payeeId, RecipientId = recipientId, Amount = 100, Date = DateTime.ParseExact("01012016", "ddMMyyyy", CultureInfo.InvariantCulture) }
                );

            payeeId = recipientId;
            recipientId = context.Users.Where(x => x.Email == "iakashkin@mail.ru").Select(x => x.Id).FirstOrDefault();
            context.Transactions.AddOrUpdate(x => x.Id,
                new Transaction() { Id = 2, PayeeId = payeeId, RecipientId = recipientId, Amount = 50, Date = DateTime.ParseExact("02012016", "ddMMyyyy", CultureInfo.InvariantCulture) }
                );

            payeeId = context.Users.Where(x => x.Email == "rjordan@gmail.com").Select(x => x.Id).FirstOrDefault();
            recipientId = context.Users.Where(x => x.Email == "mekoltsov@yandex.ru").Select(x => x.Id).FirstOrDefault();
            context.Transactions.AddOrUpdate(x => x.Id,
                new Transaction() { Id = 3, PayeeId = payeeId, RecipientId = recipientId, Amount = 50, Date = DateTime.ParseExact("03012016", "ddMMyyyy", CultureInfo.InvariantCulture) }
                );

            recipientId = payeeId;
            payeeId = context.Users.Where(x => x.Email == "andremarty@sfr.fr").Select(x => x.Id).FirstOrDefault();
            context.Transactions.AddOrUpdate(x => x.Id,
                new Transaction() { Id = 4, PayeeId = payeeId, RecipientId = recipientId, Amount = 50, Date = DateTime.ParseExact("03012016", "ddMMyyyy", CultureInfo.InvariantCulture) }
                );
        }
    }
}
